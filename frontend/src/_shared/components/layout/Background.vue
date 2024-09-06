<script setup lang="ts">
import { PhHouse } from '@phosphor-icons/vue';

const dimension = 12;

const rows = Array.from(Array(dimension).keys());
const cols = Array.from(Array(dimension).keys());
</script>
<template>
  <div class="h-full w-full top-0 left-0 opacity-5 text-gray-600 flex flex-col justify-between gap-8 overflow-clip">
    <div v-for="col in cols" :key="col">
      <div class="h-full flex justify-between gap-8">
        <PhHouse v-for="item in rows" :key="item" class="w-full h-full max-w-24 max-h-24 -rotate-45 animate-wiggle" />
      </div>
    </div>
  </div>
</template>