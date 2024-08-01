<script setup lang="ts">
import { RouterView } from 'vue-router';
import Loading from './_shared/providers/Loading.vue';
import SignalR from './_shared/providers/SignalR.vue';
</script>

<template>
  <div class="h-screen w-screen">
    <SignalR>
      <Loading>
        <RouterView />
      </Loading>
    </SignalR>
  </div>
</template>
