import { apiPlugin, StoryblokVue } from "@storyblok/vue";
import type { App, Component } from "vue";

const discoverStoryblokComponents = async (): Promise<{ componentName: string, defaultExport: Component }[]> => {
  const modules = import.meta.glob('./**/components/storyblok/**/*.vue');

  const result = new Array<{ componentName: string, defaultExport: Component }>();

  for (const path in modules) {
    const module = await modules[path]() as { default: Component };

    // Extract the component name from the file path
    const lastPartOfFilePath = path.split('/').at(-1);
    const componentName = lastPartOfFilePath
      ?.replace(/\.\w+$/, '')
      .replace(/([a-z0–9])([A-Z])/g, "$1_$2")
      .toLowerCase()

    if (!componentName) {
      continue;
    }

    result.push({ componentName, defaultExport: module.default });
  }

  return result;
};

const registerStoryblokComponents = async (app: App<Element>) => {
  const components = await discoverStoryblokComponents();
  components.forEach(({ componentName, defaultExport }) => {
    app.component(componentName, defaultExport);
  });
}

export const addStoryblok = async (app: App<Element>) => {
  app.use(StoryblokVue, {
    accessToken: import.meta.env.VITE_STORYBLOK_TOKEN,
    bridge: false,
    use: [apiPlugin],
    apiOptions: {
      region: 'eu'
    }
  })
  await registerStoryblokComponents(app);
}