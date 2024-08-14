export const shift = <T>(vals: T[], orderBy: (x: T) => number, first: T) => {

  const copy = [...vals]
  const index = copy.indexOf(first);
  const sorted = copy.sort((a, b) => orderBy(a) - orderBy(b));

  return [...sorted.splice(index), ...sorted.splice(0, index)]
}