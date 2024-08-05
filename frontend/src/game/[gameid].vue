<script setup lang="ts">
import { onMounted } from "vue";
import Main from "../_shared/components/layout/Main.vue";
import Flip from "./_shared/components/animations/Flip.vue";
import BidCardContents from "./_shared/components/BidCardContents.vue";
import Card from "./_shared/components/Card.vue";
import Deck from "./_shared/components/Deck.vue";
import HouseCardContents from "./_shared/components/storyblok/HouseCardContents.vue";
import Table from "./_shared/components/Table.vue";
import { useNextCardsDealer } from "./_shared/composables/useNextCardsDealer";

const { allCards, dealtCards, dealNext } = useNextCardsDealer();
const cardsPerRound = 4;

onMounted(() => {
  dealNext(cardsPerRound);
});

</script>
<template>
  <div class="h-1/6" />
  <div class="container mx-auto">
    <Main>
      <div class="flex items-center">
        <Table v-if="allCards.length > 0">
          <div class="flex gap-2 flex-wrap items-center w-full">
            <Deck class="hover:cursor-pointer" @click="() => dealNext(cardsPerRound)" />
            <template v-for="card in dealtCards" :key="card.content.value">
              <Flip>
                <Card :is-face-up="true">
                  <HouseCardContents v-if="card" :blok="card.content" />
                </Card>
              </Flip>
            </template>
            <Card :is-face-up="true">
              <BidCardContents :amount="15000" />
            </Card>
          </div>
        </Table>
      </div>
    </Main>
  </div>
</template>