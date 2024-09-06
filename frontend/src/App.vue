<script setup lang="ts">
import { RouterView } from 'vue-router';
import Background from './_shared/components/layout/Background.vue';
import SignalR from './_shared/providers/SignalR.vue';

const dimension = 12;

const rows = Array.from(Array(dimension).keys());
const cols = Array.from(Array(dimension).keys());
</script>

<template>
  <div class="h-screen w-screen">
    <Background class="fixed h-screen w-screen -z-10" />
    <SignalR>
      <RouterView />
    </SignalR>
  </div>
</template>
