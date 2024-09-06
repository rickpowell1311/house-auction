<script setup lang="ts">
import JumpIn from '@/_shared/components/animations/JumpIn.vue';
import Main from '@/_shared/components/layout/Main.vue';
import Loader from '@/_shared/components/Loader.vue';
import type { OnLobbyMembersChangedReactionGamer } from '@/_shared/providers/generated/HouseAuction.Lobby.Reactions';
import type { FetchLobbyResponseGamer } from '@/_shared/providers/generated/HouseAuction.Lobby.Requests';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key } from '@/_shared/providers/signalRClient';
import { PhCheck } from '@phosphor-icons/vue';
import { computed, inject, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import Button from '../_shared/components/Button.vue';
import Copy from './_shared/components/Copy.vue';
import JoinGame from './_shared/components/JoinGame.vue';
import Reconnect from './_shared/components/Reconnect.vue';

const signalRClient = inject<SignalRClient>(Key);
const route = useRoute();
const router = useRouter();
const params = route.params as Record<string, string>;
const gameId = params.gameId;
const players = ref<OnLobbyMembersChangedReactionGamer[] | FetchLobbyResponseGamer[] | undefined>();
const hasJoined = ref(false);
const isGameReady = ref(false);
const hasGameStarted = ref(false);
const creator = computed(() => {
  return players.value?.find(x => x.isCreator);
})

const iAmCreator = computed(() => {
  const me = players.value?.find(x => x.isMe);

  return creator.value && me && creator.value.name == me.name;
})

const readyUp = async () => {
  await signalRClient?.hub.readyUp({
    gameId: gameId
  })
}

const startGame = async () => {
  await signalRClient?.hub?.startGame({
    gameId: gameId
  })
}

onMounted(async () => {
  try {
    const lobby = await signalRClient?.hub.fetchLobby({ gameId: gameId });
    players.value = lobby?.gamers;
    hasJoined.value = lobby?.hasJoined === true;
    hasGameStarted.value = lobby?.hasGameStarted === true;
  }
  catch {
    // Something wen't wrong :( push back to home to try again
    router.push('/home');
  }

  signalRClient?.subscribe({
    onLobbyMembersChanged(reaction) {
      players.value = reaction.gamers;
      hasJoined.value = reaction.gamers?.some(x => x.isMe) ?? false;
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
              <p class="flex gap-2 items-center text-lg">
                <span>Game ID:</span>
                <span class="text-primary text-bold">{{ gameId }}</span>
                <Copy :value="gameId" />
              </p>
            </div>
            <div v-if="hasJoined" class="flex flex-col gap-4">
              <h2>Players</h2>
              <div class="flex flex-col gap-2">
                <div v-for="player in players" :key="player.name"
                  class="animate-fade flex items-center justify-between gap-4 min-h-8">
                  <p>{{ player.name }}</p>
                  <Button v-if="player.isMe && !player.isReady" @click="readyUp">Ready Up</Button>
                  <PhCheck v-if="player.isReady" class="text-primary animate-flip-down text-lg" weight="bold" />
                </div>
              </div>
            </div>
            <div v-else-if="!hasGameStarted">
              <JoinGame :game-id="gameId" />
            </div>
            <div v-else>
              <Reconnect :game-id="gameId" />
            </div>
            <div>
              <Button v-if="iAmCreator" :disabled="!isGameReady" @click="startGame">Start Game</Button>
              <p v-else-if="isGameReady" class="text-primary">Waiting <span v-if="creator">for {{ creator?.name
                  }}</span> to start the game</p>
            </div>
          </div>
        </div>
      </Main>
    </div>
  </JumpIn>
</template>