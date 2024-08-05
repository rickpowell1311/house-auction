<script setup lang="ts">
import { computed, onMounted } from "vue";
import Main from "../_shared/components/layout/Main.vue";
import Flip from "./_shared/components/animations/Flip.vue";
import Card from "./_shared/components/Card.vue";
import Bid from "./_shared/components/coins/Bid.vue";
import Deck from "./_shared/components/Deck.vue";
import HouseCardContents from "./_shared/components/storyblok/HouseCardContents.vue";
import { useNextCardsDealer } from "./_shared/composables/useNextCardsDealer";

const { allCards, dealtCards, dealNext } = useNextCardsDealer();
const cardsPerRound = 4;

const cardIndexes = computed(() => {
  return Array.from({ length: cardsPerRound }, (_, i) => i);
});

onMounted(() => {
  dealNext(cardsPerRound);
});

</script>
<template>
  <div class="h-1/6" />
  <div class="container mx-auto">
    <Main>
      <div class="flex flex-col gap-16">
        <div class="flex gap-2 flex-wrap justify-center items-center w-full">
          <Deck class="hover:cursor-pointer" @click="() => dealNext(cardsPerRound)" />
          <template v-for="cardIndex in cardIndexes" :key="cardIndex">
            <Card v-if="dealtCards.length <= cardIndex" :type="'place-holder'" />
            <Flip v-else>
              <Card :type="'face-up'">
                <HouseCardContents v-if="dealtCards[cardIndex]" :blok="dealtCards[cardIndex].content" />
              </Card>
            </Flip>
          </template>
        </div>
        <div class="flex gap-8 flex-wrap justify-center items-center w-full">
          <div class="flex flex-col items-center gap-4">
            <Bid :available="13" :minimum="4" />
            <h4>Rick</h4>
          </div>
          <div class="flex flex-col items-center gap-4">
            <Bid :available="4" :minimum="4" />
            <h4>Dave</h4>
          </div>
          <div class="flex flex-col items-center gap-4">
            <Bid :available="7" :minimum="4" />
            <h4>Merlin</h4>
          </div>
        </div>
      </div>
    </Main>
  </div>
</template>