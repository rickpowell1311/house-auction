<script setup lang="ts">
import { useField } from 'vee-validate';
import ValidationError from './ValidationError.vue';

interface Props {
  name: string;
  type: string;
  label: string;
  placeholder?: string;
}

const props = defineProps<Props>();

const { value, errorMessage, meta } = useField(() => props.name);

</script>
<template>
  <div class="relative">
    <label :id="name" :for="props.name" class="sr-only">{{ props.label }}</label>
    <input :id="name" v-model="value" :type="props.type" :name="props.name" :placeholder="placeholder"
      class="w-full rounded-md border-gray-200 pe-10 shadow-sm sm:text-sm text-white bg-secondary placeholder-gray-400">
    <ValidationError v-if="meta.dirty">{{ errorMessage }}</ValidationError>
  </div>
</template>