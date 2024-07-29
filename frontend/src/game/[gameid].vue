<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import Button from "../_shared/components/Button.vue";
import Main from "../_shared/components/layout/Main.vue";
import Section from "../_shared/components/layout/Section.vue";
import Flip from "./_shared/components/animations/Flip.vue";
import Card from "./_shared/components/Card.vue";
import Deck from "./_shared/components/Deck.vue";
import CardContents from "./_shared/components/storyblok/CardContents.vue";
import Table from "./_shared/components/Table.vue";
import { useStoryblokCardDealer } from "./_shared/composables/useStoryblokCardDealer";
import { shuffle } from "./_shared/helpers/shuffle";

const { allCards, dealtCards, deal } = useStoryblokCardDealer();
const usedCards = ref<number[]>([]);
const cardsPerRound = 4;

const availableCards = computed(() => {
  return allCards.value.filter(x => !usedCards.value.includes(x));
});

const dealNext = () => {
  const randomized = shuffle(availableCards.value);

  const next = randomized
    .slice(0, cardsPerRound <= randomized.length ? cardsPerRound : randomized.length);

  usedCards.value = [...usedCards.value, ...next];
  console.log(next);
  deal(next);
};

onMounted(() => {
  dealNext();
});

</script>
<template>
  <div class="h-1/6" />
  <div class="container mx-auto">
    <Main>
      <div class="flex items-center">
        <Table v-if="allCards.length > 0">
          <div class="flex gap-2 flex-wrap items-center w-full">
            <Deck />
            <template v-for="card in dealtCards" :key="card.content.value">
              <Flip>
                <Card :is-face-up="true">
                  <CardContents v-if="card" :blok="card.content" />
                </Card>
              </Flip>
            </template>
          </div>
          <Section>
            <div v-if="availableCards.length > 0">
              <Button @click="() => dealNext()">Deal</Button>
            </div>
          </Section>
        </Table>
      </div>
    </Main>
  </div>
</template>