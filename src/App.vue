<script setup lang="ts">
import { RouterView } from 'vue-router';
</script>

<template>
  <div class="h-screen w-screen bg-secondary text-white">
    <RouterView class="animate-jump-in animate-once animate-ease-in" />
  </div>
</template>
