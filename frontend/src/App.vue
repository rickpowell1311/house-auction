<script setup lang="ts">
import { RouterView } from 'vue-router';
import SignalR from './_shared/providers/SignalR.vue';
</script>

<template>
  <div class="h-screen w-screen">
    <SignalR>
      <RouterView />
    </SignalR>
  </div>
</template>
