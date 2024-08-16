<script setup lang="ts">
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key as SignalRClientKey } from '@/_shared/providers/signalRClient';
import { inject, onMounted, ref, watch } from 'vue';
import { useStoryblokCardDealer } from '../../composables/useStoryblokCardDealer';
import Card from '../Card.vue';
import Deck from '../Deck.vue';
import HouseCardContents from '../HouseCardContents.vue';
import Flip from '../animations/Flip.vue';

const { isReadyToDeal, cards, deal, dealt } = useStoryblokCardDealer();
const props = defineProps<{ properties: number[] }>();
const cardIndexes = Array.from(Array(props.properties.length).keys());
const signalRClient = inject<SignalRClient>(SignalRClientKey);

const propertyCards = ref<number[]>([]);
const isDeckEmpty = ref(false);

watch(isReadyToDeal, val => {
  if (val) {
    propertyCards.value = props.properties
  }
})

watch(propertyCards, val => {
  deal(val);
})

onMounted(() => {
  signalRClient?.subscribe({
    onBiddingRoundComplete(reaction) {
      propertyCards.value = reaction.nextRound?.properties ?? []
      isDeckEmpty.value = reaction.nextRound?.isLastRound === true
    },
  } as IHouseAuctionReceiver)
})

</script>

<template>
  <div v-if="isReadyToDeal" class="flex gap-2 flex-wrap justify-center items-center w-full">
    <Deck v-if="!isDeckEmpty" />
    <template v-for="cardIndex in cardIndexes" :key="cardIndex">
      <Card v-if="dealt.length <= cardIndex" :type="'place-holder'" />
      <Flip v-else>
        <Card :type="'face-up'">
          <HouseCardContents v-if="dealt[cardIndex]" :blok="cards[dealt[cardIndex] - 1].content" />
        </Card>
      </Flip>
    </template>
  </div>
</template>