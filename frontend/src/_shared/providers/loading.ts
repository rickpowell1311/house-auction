import type { InjectionKey, Ref } from "vue";

export const Key = Symbol("loading") as InjectionKey<string>;

export interface Loading {
  isLoading: Ref<boolean>;
  toggleLoading: (isLoading: boolean) => void;
}