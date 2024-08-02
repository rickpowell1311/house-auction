<script setup lang="ts">
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { onMounted, provide, ref } from 'vue';
import { useRouter } from 'vue-router';
import { getHubProxyFactory, getReceiverRegister } from './generated/TypedSignalR.Client';
import { Key, type SignalRClient } from './signalRClient';

const connection = ref(new HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_SIGNALR_URL}/house-auction`, { withCredentials: false })
  .build());

const isConnected = ref(false);
const router = useRouter();

const hub = getHubProxyFactory("IHouseAuctionHub")
  .createHubProxy(connection.value as HubConnection);

const provider = {
  hub: hub,
  subscribe: receiver => getReceiverRegister("IHouseAuctionReceiver").register(connection.value as HubConnection, receiver)
} satisfies SignalRClient

provide<SignalRClient>(Key, provider);

onMounted(async () => {
  try {
    await connection.value.start();
  } catch (error) {
    console.error(error);
  }

  // If we're refreshing a SignalR connection we always want to boot out to the home page
  router.push("/home");
})
</script>

<template>
  <slot />
</template>