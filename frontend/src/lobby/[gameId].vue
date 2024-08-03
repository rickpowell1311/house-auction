<script setup lang="ts">
import JumpIn from '@/_shared/components/animations/JumpIn.vue';
import Main from '@/_shared/components/layout/Main.vue';
import Loader from '@/_shared/components/Loader.vue';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key } from '@/_shared/providers/signalRClient';
import { inject, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import Button from '../_shared/components/Button.vue';

const signalRClient = inject<SignalRClient>(Key);
const route = useRoute();
const params = route.params as Record<string, string>;
const gameId = params.gameId;
const players = ref<string[] | undefined>();

onMounted(async () => {
  signalRClient?.subscribe({
    async onLobbyMembersChanged(members) {
      players.value = members;
    }
  } as IHouseAuctionReceiver)

  const result = await signalRClient?.hub.fetchLobby(gameId);
  players.value = result;
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
                <div v-for="player in players" :key="player"
                  class="animate-fade flex items-center justify-between gap-4">
                  <p>{{ player }}</p>
                  <Button>Ready Up</Button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </Main>
    </div>
  </JumpIn>
</template>