<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import { Key as SignalRKey, type SignalRClient } from '@/_shared/providers/signalRClient';
import router from '@/router';
import { inject, onMounted, ref } from 'vue';

const signalRClient = inject<SignalRClient>(SignalRKey);
const props = defineProps<{
  gameId: string;
}>();
const disconnectedPlayers = ref<string[]>([]);

onMounted(async () => {
  disconnectedPlayers.value = (await signalRClient?.hub.getDisconnectedPlayers({
    gameId: props.gameId
  }))?.players ?? [];
})

const reconnect = async (player: string) => {
  await signalRClient?.hub.reconnect({
    gameId: props.gameId,
    gamer: player
  });

  router.push(`/game/${props.gameId}`);
}
</script>

<template>
  <div v-if="disconnectedPlayers.length > 0" class="flex flex-col gap-4">
    <h2>Players</h2>
    <div class="flex flex-col gap-2">
      <div v-for="player in disconnectedPlayers" :key="player"
        class="animate-fade flex items-center justify-between gap-4 min-h-8">
        <p>{{ player }}</p>
        <Button @click="() => reconnect(player)">Rejoin as {{ player }}</Button>
      </div>
    </div>
  </div>
  <div v-else>
    <p class="text-primary">Game already started. Sucks to be you</p>
  </div>
</template>