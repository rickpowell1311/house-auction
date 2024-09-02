<script setup lang="ts">
const props = defineProps<{
  isSecondary?: boolean;
}>();
</script>

<template>
  <button
    class="p-2 rounded-lg font-bold hover:scale-125 transition-all duration-300 disabled:bg-gray-400 disabled:scale-100 min-w-32 border-2 border-primary disabled:border-gray-400"
    :class="[
      props.isSecondary === true ? 'text-primary' : 'bg-primary'
    ]">
    <slot />
  </button>
</template>
