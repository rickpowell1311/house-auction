<script setup lang="ts">
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { onMounted, provide, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import Loader from '../components/Loader.vue';
import type { SignalRClient } from './signalRClient';
import { Key, SignalRClientImpl } from './signalRClient';

const connection = ref<HubConnection>(new HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_SIGNALR_URL}/house-auction`, { withCredentials: false })
  .build());

const isConnected = ref(false);
const router = useRouter();

provide<SignalRClient>(Key, new SignalRClientImpl(connection.value as HubConnection));

onMounted(async () => {
  await connection.value.start();

  // If we're refreshing a SignalR connection we always want to boot out to the home page
  router.push("/home");
})

watch(connection.value, val => {
  isConnected.value = val.state === "Connected";
})
</script>

<template>
  <Loader v-if="!isConnected" />
  <slot v-else />
</template>