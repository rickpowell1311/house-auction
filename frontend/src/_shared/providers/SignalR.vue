<script setup lang="ts">
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { onMounted, provide, ref } from 'vue';
import { useRouter } from 'vue-router';
import Loader from '../components/Loader.vue';
import type { SignalRClient } from './signalRClient';
import { Key, SignalRClientImpl } from './signalRClient';

var hubConnection = new HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_SIGNALR_URL}/house-auction`, { withCredentials: false })
  .build()

const connection = ref<HubConnection>(hubConnection);
const router = useRouter();

provide<SignalRClient>(Key, new SignalRClientImpl(connection.value as HubConnection));

onMounted(async () => {
  await connection.value.start();

  // If we're refreshing a SignalR connection we always want to boot out to the root page
  router.push("/");
})
</script>

<template>
  <Loader v-if="connection.state !== HubConnectionState.Connected" />
  <slot v-else />
</template>