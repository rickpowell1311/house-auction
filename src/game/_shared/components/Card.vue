<script setup lang="ts">
interface Props {
  isFaceUp: boolean;
}

const props = defineProps<Props>();
</script>

<template>
  <div class="min-w-48 w-48 h-64 border-white bg-white border-4 rounded-2xl shadow-lg shadow-black">
    <slot v-if="props.isFaceUp" />
    <div v-else class="w-full h-full rounded-2xl bg-primary flex items-center justify-center">
      <div class="overflow-hidden w-20 min-w-20 h-20 border-2 rounded-2xl border-white bg-secondary p-4">
        <img class="object-cover w-full h-full" src="../../../assets/house.png">
      </div>
    </div>
  </div>
</template>