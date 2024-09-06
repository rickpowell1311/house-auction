<script setup lang="ts">
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { onBeforeMount, provide, ref } from 'vue';
import Section from '../components/layout/Section.vue';
import Loader from '../components/Loader.vue';
import Modal from '../components/Modal.vue';
import { getHubProxyFactory, getReceiverRegister } from './generated/TypedSignalR.Client';
import { Key, type SignalRClient } from './signalRClient';

const connection = ref(new HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_SIGNALR_URL}/house-auction`, { withCredentials: false })
  .build());

const connectionReady = ref(false);
const connectionError = ref<string | undefined>();

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
    connectionError.value = String(error);
    console.error(error);
  }
})
</script>

<template>
  <slot v-if="connectionReady" />
  <Loader v-else-if="!connectionError" />
  <Modal v-else title="Connection Error">
    <div class="container mx-auto">
      <Section class="animate-bounce">
        <h1 class="text-center animate-wiggle animate-infinite">😭</h1>
      </Section>
      <Section>
        <h2 class="shadow-gray-300 text-primary text-center">House Auction is unavailable right now</h2>
      </Section>
    </div>
  </Modal>
</template>