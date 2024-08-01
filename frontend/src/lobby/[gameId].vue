<script setup lang="ts">
import { type SignalRClient, Key } from '@/_shared/providers/signalRClient';
import { inject, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

const signalRClient = inject<SignalRClient>(Key);
const route = useRoute();
const gameId = route.params["gameId"];
const players = ref<string[]>([]);

onMounted(async () => {
  signalRClient?.handleLobbyMembersChanged((val) => {
    players.value = val;
  })
  const result = await signalRClient?.fetchLobby(gameId);
  players.value = result?.players ?? [];
})
</script>

<template>
  <p>Players:</p>
  <ul>
    <li v-for="player in players" :key="player">{{ player }}</li>
  </ul>
</template>