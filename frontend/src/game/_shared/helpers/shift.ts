export const shift = <T>(vals: T[], orderBy: (x: T) => number, first: T) => {

  const copy = [...vals]
  const index = copy.indexOf(first);

  return [...copy.splice(index), ...copy.splice(0, index)]
}