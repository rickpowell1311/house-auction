<script setup lang="ts">
import JumpIn from '@/_shared/components/animations/JumpIn.vue';
import Main from '@/_shared/components/layout/Main.vue';
import Loader from '@/_shared/components/Loader.vue';
import type { OnLobbyMembersChangedReactionGamer } from '@/_shared/providers/generated/HouseAuction.Lobby.Reactions';
import type { FetchLobbyResponseGamer } from '@/_shared/providers/generated/HouseAuction.Lobby.Requests';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key } from '@/_shared/providers/signalRClient';
import { inject, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import Button from '../_shared/components/Button.vue';

const signalRClient = inject<SignalRClient>(Key);
const route = useRoute();
const params = route.params as Record<string, string>;
const gameId = params.gameId;
const players = ref<OnLobbyMembersChangedReactionGamer[] | FetchLobbyResponseGamer[] | undefined>();
const ready = ref(false);

const readyUp = async () => {
  await signalRClient?.hub.readyUp({
    gameId: gameId,
    name: players.value?.find(x => x.isMe)?.name
  })

  ready.value = true;
}

onMounted(async () => {
  const lobby = await signalRClient?.hub.fetchLobby({ gameId: gameId });
  console.log(lobby)
  players.value = lobby?.gamers;

  signalRClient?.subscribe({
    async onLobbyMembersChanged(reaction) {
      players.value = reaction.gamers;
      console.log(reaction);
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
                  class="animate-fade flex items-center justify-between gap-4">
                  <p>{{ player.name }}</p>
                  <Button v-if="player.isMe && !ready" @click="readyUp">Ready Up</Button>
                  <span v-if="(player.isMe && ready) || player.isReady"
                    class="material-symbols-rounded text-primary animate-flip-down">check</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </Main>
    </div>
  </JumpIn>
</template>