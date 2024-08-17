<script setup lang="ts">
import Loader from "@/_shared/components/Loader.vue";
import { watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import Main from "../_shared/components/layout/Main.vue";
import BiddingPhase from "./_shared/components/bidding/BiddingPhase.vue";
import { useBiddingPhaseLoader } from "./_shared/composables/useBiddingPhaseLoader";

const route = useRoute();
const router = useRouter();
const params = route.params as Record<string, string>;
const gameId = params.gameid;

const { biddingPhase, error } = useBiddingPhaseLoader(gameId);

watch(error, err => {
  if (err) {
    // If the bidding phase can't be loaded, push user back to the lobby
    router.push(`/lobby/${gameId}`);
  }
})

</script>
<template>
  <Loader v-if="!biddingPhase" />
  <template v-else>
    <div class="h-1/6" />
    <div class="container mx-auto">
      <Main>
        <div class="flex flex-col gap-16">
          <BiddingPhase :game-id="gameId" :="biddingPhase" />
        </div>
      </Main>
    </div>
  </template>
</template>