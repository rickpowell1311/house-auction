import { parse } from '@vue/compiler-sfc';
import { execSync } from 'child_process';
import * as fs from 'fs';
import * as path from 'path';
import ts from "typescript";
import { describe, expect, test } from 'vitest';

describe('architecture', () => {

  test('TypeScript only references parent or sibling _shared folders', () => {
    const projectDir = path.resolve(__dirname);
    const files = getAllFiles(projectDir);
    const importErrors = new Array<string>();
    files.forEach(file => {
      const imports = getImports(file);
      for (const imp of imports) {

        // Exclude packages from check
        if (!imp.includes('.') && !imp.includes('@/')) {
          continue;
        }

        // Convert import to a static file path
        const staticImport = path.resolve(__dirname, imp.startsWith('@/') ? imp.replace('@/', '') : imp);
        let importRelativeToFile = path.relative(file, staticImport);

        // Unify path separators to be cross-platform before making further checks
        importRelativeToFile = crossPlatformPathName(importRelativeToFile.replace(/\\/g, '/'));

        // Exclude imports not to shared folders 
        if (!importRelativeToFile.includes('_shared')) {
          continue;
        }

        /* Check that
          1. import starts with ./ or ../ and
          2. _shared folder reference is always immediately after ../ or ./ 
          3. or import is immediately to a child directory with the same name
          (unplugin router conventions mean that a file named dashboard.vue can
          be a layout to routes in a "dashboard" child directory)
        */
        const hasValidStart = importRelativeToFile.startsWith('./')
          || importRelativeToFile.startsWith('../');

        const hasImmediatelyFollowingShared = replaceAll(
          replaceAll(importRelativeToFile, '../', ''), './', '')
          .startsWith('_shared');

        const fileName = crossPlatformPathName(file).split('/').pop();
        const isLayoutToChildDirectory = replaceAll(
          replaceAll(importRelativeToFile, '../', ''), './', '')
          .startsWith(`${fileName?.replace('.vue', '')}`);

        const isValid = hasValidStart && (hasImmediatelyFollowingShared || isLayoutToChildDirectory);

        if (!isValid) {
          importErrors.push(`File ${file} has an invalid import '${imp}' from a _shared folder. Code should only be imported from _shared folders if they are in parent or sibling directories`);
        }
      }
    });

    expect(importErrors, importErrors.join('\n')).toHaveLength(0);
  });

  test.each([{ validDirectories: ['components', 'helpers', 'providers', 'composables'] }])
    ('Child directories of _shared folders should have valid names', testCase => {
      const projectDir = path.resolve(__dirname);
      const sharedDirectories = fs.readdirSync(projectDir, { withFileTypes: true, recursive: true })
        .filter((item) => item.isDirectory())
        .filter(x => x.name.startsWith('_shared'))
        .map(x => crossPlatformPathName(`${x.path}/${x.name}`));

      const errors = new Array<string>();

      sharedDirectories.forEach(sharedFolder => {
        const invalidDirectories = fs.readdirSync(sharedFolder, { withFileTypes: true })
          .filter((item) => item.isDirectory())
          .map(x => x.name)
          .filter(x => !testCase.validDirectories.includes(x));

        invalidDirectories.forEach(invalidDir => {
          errors.push(`Directory '${invalidDir}' in ${sharedFolder} should be one of ${testCase.validDirectories.join(', ')}`);
        });
      });

      expect(errors, errors.join('\n')).toHaveLength(0);
    })

  test.each([{ invalidDirectories: ['components', 'helpers', 'providers', 'composables'] }])
    ('Child directories of non-_shared directories should not have directory names reserved for _shared directories', testCase => {
      const projectDir = path.resolve(__dirname);
      const nonSharedDirectories = fs.readdirSync(projectDir, { withFileTypes: true, recursive: true })
        .filter((item) => item.isDirectory())
        .map(x => crossPlatformPathName(`${x.path}/${x.name}`))
        .filter(x => !x.includes('_shared'));

      const errors = new Array<string>();

      nonSharedDirectories.forEach(nonSharedFolder => {
        const invalidDirectories = fs.readdirSync(nonSharedFolder, { withFileTypes: true })
          .filter((item) => item.isDirectory())
          .map(x => x.name)
          .filter(x => testCase.invalidDirectories.includes(x));

        invalidDirectories.forEach(invalidDir => {
          errors.push(`Directory '${invalidDir}' in ${nonSharedFolder} should not be one of ${testCase.invalidDirectories.join(', ')}. Directories with these names are reserved for _shared directories only`);
        });
      });

      expect(errors, errors.join('\n')).toHaveLength(0);
    })

  test('Files in composables folders should have a name starting with use', () => {
    const projectDir = path.resolve(__dirname);
    const files = getAllFiles(projectDir);
    const errors = new Array<string>();
    files.forEach(file => {
      if (file.includes('composables')
        && path.basename(file).endsWith('.ts')
        && !path.basename(file).startsWith('use')) {
        errors.push(`File ${file} in composable folder should have a name starting with 'use'`);
      }
    });

    expect(errors, errors.join('\n')).toHaveLength(0);
  })

  test('ESLint does not detect any issues', () => {
    try {
      execSync('npm run lint', { stdio: 'inherit' });
    } catch (error) {
      expect(error).toBeFalsy();
    }
  })
})

const getAllFiles = (dir: string, fileList: string[] = []): string[] => {
  const files = fs.readdirSync(dir);
  files.forEach(file => {
    const filePath = path.join(dir, file);
    if (fs.statSync(filePath).isDirectory()) {
      getAllFiles(filePath, fileList);
    } else if (!filePath.includes('test.') && (filePath.endsWith('.ts') || filePath.endsWith('.tsx') || filePath.endsWith('.vue'))) {
      fileList.push(filePath);
    }
  });
  return fileList;
}

const getImportsFromTS = (content: string, file: string): string[] => {
  const sourceFile = ts.createSourceFile(
    file,
    content,
    ts.ScriptTarget.ESNext,
    true
  );

  const imports: string[] = [];

  const visit = (node: ts.Node) => {
    if (ts.isImportDeclaration(node)) {
      const importPath = (node.moduleSpecifier as ts.StringLiteral).text;
      imports.push(importPath);
    }
    ts.forEachChild(node, visit);
  }

  visit(sourceFile);
  return imports;
}

const replaceAll = (str: string, find: string, replace: string) => {
  return str.split(find).join(replace);
}

const getImports = (file: string): string[] => {
  const content = fs.readFileSync(file).toString();
  if (file.endsWith('.vue')) {
    const { descriptor } = parse(content);
    if (descriptor.script && descriptor.script.lang === 'ts') {
      return getImportsFromTS(descriptor.script.content, file);
    }
    if (descriptor.scriptSetup && descriptor.scriptSetup.lang === 'ts') {
      return getImportsFromTS(descriptor.scriptSetup.content, file);
    }
    return [];
  } else {
    return getImportsFromTS(content, file);
  }
}

const crossPlatformPathName = (path: string) => {
  return path.replace(/\\/g, '/');
}