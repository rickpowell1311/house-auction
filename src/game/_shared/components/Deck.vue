<script setup lang="ts">
import Card from "./Card.vue";
</script>

<template>
  <div class="pb-2 pl-2">
    <div class="relative">
      <div
        class="absolute w-full h-full -mx-1 my-1 rounded-2xl bg-white -z-10 shadow-lg border-solid border-black border-l border-b" />
      <div
        class="absolute w-full h-full -mx-2 my-2 rounded-2xl bg-white -z-20 shadow-lg shadow-black border-solid border-black border-l border-b" />
      <div class="border-solid border-black border-l border-b rounded-2xl">
        <Card :is-face-up="false" class="shadow-none" />
      </div>
    </div>
  </div>
</template>