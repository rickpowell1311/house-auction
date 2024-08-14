<script setup lang="ts">
import { type GetBiddingPhaseResponse } from '@/_shared/providers/generated/HouseAuction.Bidding.Requests';
import { inject, onMounted, ref, watch } from 'vue';
import { useStoryblokCardDealer } from '../../composables/useStoryblokCardDealer';
import { shift } from '../../helpers/shift';
import BiddingPlayer from './BiddingPlayer.vue';
import BiddingDeal from './BiddingDeal.vue';
import { type SignalRClient, Key as SignalRClientKey } from '@/_shared/providers/signalRClient';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';

interface BiddingPhaseProps extends GetBiddingPhaseResponse {
  gameId: string;
}

const props = defineProps<BiddingPhaseProps>();
const signalRClient = inject<SignalRClient>(SignalRClientKey);
const me = props.players?.me;
const all = [...(props.players?.others ?? []), me];
const others = shift(all, x => x?.order ?? 0, me)
  .filter(x => x?.name !== me?.name);
const allBids = [props.players?.me?.bid?.amount ?? 0, ...others.map(x => x?.bid?.amount ?? 0)].sort((a, b) => b - a);
const minBid = Math.max(...allBids) + 1;

const previousPlays = ref<{ name: string, bid?: number, passed: boolean }[]>([]);

const activePlayer = ref(all.find(x => x?.isTurn === true)?.name ?? undefined);

onMounted(() => {
  signalRClient?.subscribe({
    onPlayerTurnFinished(reaction) {
      previousPlays.value = [...previousPlays.value, {
        name: reaction.player ?? '',
        bid: reaction.result?.bid,
        passed: reaction.result?.passed === true
      }];
      activePlayer.value = reaction.nextPlayer;
    }
  } as IHouseAuctionReceiver)
})

</script>
<template>
  <div class="flex flex-col gap-6">
    <BiddingDeal :properties="props.deck?.propertiesOnTheTable ?? []" />
    <div class="flex gap-8 flex-wrap justify-center items-end w-full">
      <BiddingPlayer :game-id="gameId" :name="me?.name ?? ''" :is-me="true" :is-bidding="me?.name === activePlayer"
        :coins="{ available: me?.coins ?? 0, minimum: minBid }" />
      <BiddingPlayer v-for="player in others" :key="player?.name ?? ''" :game-id="gameId" :name="player?.name ?? ''" :is-me="false"
        :is-bidding="player?.name === activePlayer" :coins="{ available: 0, minimum: minBid, amount: player?.bid?.amount }" />
    </div>
  </div>
</template>