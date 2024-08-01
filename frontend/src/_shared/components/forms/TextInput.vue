<script setup lang="ts">
import type { InputTypeHTMLAttribute } from 'vue';

interface Props {
  label: string;
  name: string;
  type: InputTypeHTMLAttribute;
  placeHolder?: string;
}

const props = defineProps<Props>();
</script>
<template>
  <div class="relative">
    <label :for="props.name" class="sr-only">{{ props.label }}</label>
    <input :id="props.name" :type="props.type" :placeholder="props.placeHolder"
      class="w-full rounded-md border-gray-200 pe-10 shadow-sm sm:text-sm text-white bg-secondary placeholder-gray-400">
  </div>
</template>