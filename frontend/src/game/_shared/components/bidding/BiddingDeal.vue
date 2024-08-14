<script setup lang="ts">
import { watch } from 'vue';
import { useStoryblokCardDealer } from '../../composables/useStoryblokCardDealer';
import Deck from '../Deck.vue';
import Card from '../Card.vue';
import HouseCardContents from '../HouseCardContents.vue';
import Flip from '../animations/Flip.vue';

const { isReadyToDeal, cards, deal, dealt } = useStoryblokCardDealer();

const props = defineProps<{ properties: number[] }>();
const cardIndexes = Array.from(Array(props.properties.length).keys());

watch(isReadyToDeal, val => {
  if (val) {
    deal(props.properties);
  }
})

</script>

<template>
  <div v-if="isReadyToDeal" class="flex gap-2 flex-wrap justify-center items-center w-full">
    <Deck />
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