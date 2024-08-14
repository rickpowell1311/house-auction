<script setup lang="ts">
import { type GetBiddingPhaseResponse } from '@/_shared/providers/generated/HouseAuction.Bidding.Requests';
import { watch } from 'vue';
import { useStoryblokCardDealer } from '../../composables/useStoryblokCardDealer';
import { shift } from '../../helpers/shift';
import Card from '../Card.vue';
import Deck from '../Deck.vue';
import HouseCardContents from '../HouseCardContents.vue';
import Flip from '../animations/Flip.vue';
import BiddingPlayer from './BiddingPlayer.vue';
import BiddingDeal from './BiddingDeal.vue';

const props = defineProps<GetBiddingPhaseResponse>();
const { isReadyToDeal, cards, deal, dealt } = useStoryblokCardDealer();

const me = props.players?.me;
const all = [...(props.players?.others ?? []), me];
const others = shift(all, x => x?.order ?? 0, me)
  .filter(x => x?.name !== me?.name);
const allBids = [props.players?.me?.bid?.amount ?? 0, ...others.map(x => x?.bid?.amount ?? 0)].sort((a, b) => b - a);
const minBid = Math.max(...allBids) + 1;

watch(isReadyToDeal, val => {
  if (val) {
    deal(props.deck?.propertiesOnTheTable ?? []);
  }
})

</script>
<template>
  <div class="flex flex-col gap-6">
    <BiddingDeal :properties="props.deck?.propertiesOnTheTable ?? []" />
    <div class="flex gap-8 flex-wrap justify-center items-end w-full">
      <BiddingPlayer :name="me?.name ?? ''" :is-me="true" :is-bidding="me?.isTurn === true"
        :coins="{ available: me?.coins ?? 0, minimum: minBid }" />
      <BiddingPlayer v-for="player in others" :key="player?.name ?? ''" :name="player?.name ?? ''" :is-me="false"
        :is-bidding="player?.isTurn === true" :coins="{ available: 0, minimum: minBid, amount: player?.bid?.amount }" />
    </div>
  </div>
</template>