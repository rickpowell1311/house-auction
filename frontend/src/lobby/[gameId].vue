<script setup lang="ts">
import JumpIn from '@/_shared/components/animations/JumpIn.vue';
import Main from '@/_shared/components/layout/Main.vue';
import Loader from '@/_shared/components/Loader.vue';
import type { OnLobbyMembersChangedReactionGamer } from '@/_shared/providers/generated/HouseAuction.Lobby.Reactions';
import type { FetchLobbyResponseGamer } from '@/_shared/providers/generated/HouseAuction.Lobby.Requests';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key } from '@/_shared/providers/signalRClient';
import { computed, inject, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import Button from '../_shared/components/Button.vue';

const signalRClient = inject<SignalRClient>(Key);
const route = useRoute();
const router = useRouter();
const params = route.params as Record<string, string>;
const gameId = params.gameId;
const players = ref<OnLobbyMembersChangedReactionGamer[] | FetchLobbyResponseGamer[] | undefined>();
const isGameReady = ref(false);
const creator = computed(() => {
  return players.value?.find(x => x.isCreator);
})

const iAmCreator = computed(() => {
  const me = players.value?.find(x => x.isMe);

  return creator.value && me && creator.value.name == me.name;
})

const readyUp = async () => {
  await signalRClient?.hub.readyUp({
    gameId: gameId,
    name: players.value?.find(x => x.isMe)?.name
  })
}

const startGame = async () => {
  await signalRClient?.hub?.startGame({
    gameId: gameId,
    name: players.value?.find(x => x.isMe)?.name
  })
}

onMounted(async () => {

  try {
    const lobby = await signalRClient?.hub.fetchLobby({ gameId: gameId });
    players.value = lobby?.gamers;
  }
  catch {
    // If we can't load the lobby, it might be because of a refresh of the page.
    // In this case, push to home and force a rejoin
    router.push('/home');
  }
  
  signalRClient?.subscribe({
    onLobbyMembersChanged(reaction) {
      players.value = reaction.gamers;
    },
    onGameReadinessChanged(reaction) {
      isGameReady.value = reaction.isReadyToStart;
    },
    onGameStarted(reaction) {
      router.push(`/game/${gameId}`)
    }
  } as IHouseAuctionReceiver)
})
</script>

<template>
  <Loader v-if="!players" />
  <div v-else class="h-1/6" />
  <JumpIn>
    <div class="container mx-auto">
      <Main>
        <div class="flex justify-center">
          <div class="flex flex-col gap-12">
            <div class="flex flex-col gap-4">
              <h1 class="text-primary">Lobby</h1>
              <p>Game ID: <span class="text-primary text-bold">{{ gameId }}</span></p>
            </div>
            <div class="flex flex-col gap-4">
              <h2>Players</h2>
              <div class="flex flex-col gap-2">
                <div v-for="player in players" :key="player.name"
                  class="animate-fade flex items-center justify-between gap-4 min-h-8">
                  <p>{{ player.name }}</p>
                  <Button v-if="player.isMe && !player.isReady" @click="readyUp">Ready Up</Button>
                  <span v-if="player.isReady"
                    class="material-symbols-rounded text-primary animate-flip-down">check</span>
                </div>
              </div>
            </div>
            <div>
              <Button v-if="iAmCreator" :disabled="!isGameReady" @click="startGame">Start Game</Button>
              <p v-else-if="isGameReady" class="text-primary">Waiting <span v-if="creator">for {{ creator?.name }}</span> to start the game</p>
            </div>
          </div>
        </div>
      </Main>
    </div>
  </JumpIn>
</template>