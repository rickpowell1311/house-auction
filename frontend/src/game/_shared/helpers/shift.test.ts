import { describe, expect, test } from "vitest";
import { shift } from "./shift";

describe('Shift', () => {
  test('Shift 0,1,2 with 0 does nothing', () => {
    const result = shift([0, 1, 2], x => x, 0);
    expect(result).toEqual([0, 1, 2]);
  })

  test('Shift 0,1,2 with 1 shifts as expected', () => {
    const result = shift([0, 1, 2], x => x, 1);
    expect(result).toEqual([1, 2, 0]);
  })

  test('Shift 0,1,2 with 2 shifts as expected', () => {
    const result = shift([0, 1, 2], x => x, 2);
    expect(result).toEqual([2, 0, 1]);
  })
})