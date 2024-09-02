<script setup lang="ts">
import { type GetBiddingPhaseResponse } from '@/_shared/providers/generated/HouseAuction.Bidding.Requests';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key as SignalRClientKey } from '@/_shared/providers/signalRClient';
import { inject, onMounted, ref } from 'vue';
import { shift } from '../../helpers/shift';
import BiddingDeal from './BiddingDeal.vue';
import BiddingPlayer from './BiddingPlayer.vue';

interface BiddingPhaseProps extends GetBiddingPhaseResponse {
  gameId: string;
}

const props = defineProps<BiddingPhaseProps>();
const signalRClient = inject<SignalRClient>(SignalRClientKey);
const me = props.players?.me;
const ordered = shift(
  [...(props.players?.others ?? []), me].sort((a, b) => (a?.order ?? 0) - (b?.order ?? 0)),
  x => x?.order ?? 0,
  me)
const others = ordered.filter(x => x?.name !== me?.name ?? '');
const allBids = [...ordered.map(x => x?.bid?.amount ?? 0)].sort((a, b) => b - a);

const previousPlays = ref<{ name: string, bid?: number, passed: boolean }[]>([]);
const activePlayer = ref(ordered.find(x => x?.isTurn === true)?.name ?? undefined);
const highestBid = ref<number>(Math.max(...allBids))
const currentRound = ref<number>(props.round ?? props.totalRounds ?? 1);

const onBid = (amount: number) => {
  highestBid.value = amount;
}

const onPass = () => {

}

onMounted(() => {
  signalRClient?.subscribe({
    onPlayerTurnComplete(reaction) {
      previousPlays.value = [...previousPlays.value, {
        name: reaction.player ?? '',
        bid: reaction.result?.bid,
        passed: reaction.result?.passed === true
      }];
      activePlayer.value = reaction.nextPlayer;

      if (reaction.result?.bid) {
        highestBid.value = reaction.result.bid;
      }
    },
    onBiddingRoundComplete(reaction) {
      previousPlays.value = [];
      highestBid.value = 0;
      currentRound.value++;
    },
  } as IHouseAuctionReceiver)
})

</script>
<template>
  <div class="flex flex-col gap-6">
    <h2 class="text-center text-primary">Round {{ currentRound }}/{{ props.totalRounds }}
    </h2>
    <BiddingDeal :properties="props.deck?.propertiesOnTheTable ?? []" :round-number="round"
      :total-rounds="props.totalRounds" />
    <div class="flex gap-8 flex-wrap justify-center w-full">
      <BiddingPlayer :game-id="gameId" :name="me?.name ?? ''" :is-me="true" :is-bidding="me?.name === activePlayer"
        :coins="{ available: me?.coins ?? 0, minimum: highestBid + 1 }" :has-passed="me?.bid?.hasPassed === true"
        bid="onBid" @pass="onPass" />
      <BiddingPlayer v-for="player in others" :key="player?.name ?? ''" :game-id="gameId" :name="player?.name ?? ''"
        :is-me="false" :is-bidding="player?.name === activePlayer"
        :coins="{ available: 0, minimum: highestBid + 1, amount: player?.bid?.amount ?? 0 }"
        :has-passed="player?.bid?.hasPassed === true" @bid="onBid" @pass="onPass" />
    </div>
  </div>
</template>