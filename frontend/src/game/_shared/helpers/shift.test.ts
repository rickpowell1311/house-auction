import { describe, expect, test } from "vitest";
import { shift } from "./shift";

describe('Shift', () => {
  test('Shift 1,2,3,4,5 with 1 does nothing', () => {
    const result = shift([1, 2, 3, 4, 5], x => x, 1);
    expect(result).toEqual([1, 2, 3, 4, 5]);
  })

  test('Shift 1,2,3,4,5 with 3 shifts results', () => {
    const result = shift([1, 2, 3, 4, 5], x => x, 3);
    expect(result).toEqual([3, 4, 5, 1, 2]);
  })
})