<script setup lang="ts">
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { onBeforeMount, provide, ref } from 'vue';
import { getHubProxyFactory, getReceiverRegister } from './generated/TypedSignalR.Client';
import { Key, type SignalRClient } from './signalRClient';

const connection = ref(new HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_SIGNALR_URL}/house-auction`, { withCredentials: false })
  .build());
const connectionReady = ref(false);

const hub = getHubProxyFactory("IHouseAuctionHub")
  .createHubProxy(connection.value as HubConnection);

const provider = {
  hub: hub,
  subscribe: receiver => getReceiverRegister("IHouseAuctionReceiver").register(connection.value as HubConnection, receiver)
} satisfies SignalRClient

provide<SignalRClient>(Key, provider);

onBeforeMount(async () => {
  try {
    await connection.value.start();
    connectionReady.value = true;
  } catch (error) {
    console.error(error);
  }
})
</script>

<template>
  <slot v-if="connectionReady" />
</template>