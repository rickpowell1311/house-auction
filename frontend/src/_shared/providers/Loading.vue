<script setup lang="ts">
import { provide, ref } from "vue";
import Loader from "../components/Loader.vue";
import { Key, type Loading } from "./loading";

const val = ref(false);

const toggleLoading = (isLoading: boolean) => {
  val.value = isLoading;
}

provide<Loading>(Key, {
  isLoading: val,
  toggleLoading
});
</script>

<template>
  <Loader v-if="val" />
  <slot v-else />
</template>