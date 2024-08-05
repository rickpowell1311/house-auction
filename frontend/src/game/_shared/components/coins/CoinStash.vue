<script setup lang="ts">
import { computed } from 'vue';
import Coin from './Coin.vue';

const props = defineProps<{ amount: number }>();

const maxCoinsInPile = 5;
const piles = computed(() => {
  const result = new Array<{ index: number; coins: { index: number }[] }>();

  for (let i = 0; i < props.amount; i += maxCoinsInPile) {

    const coinsInPile = Math.min(maxCoinsInPile, props.amount - i);
    const coins = new Array(coinsInPile).fill(0).map((_, index) => ({
      index: i + index,
    }));

    result.push({
      index: i,
      coins: coins,
    });
  }

  return result;
})

</script>

<template>
  <div class="flex gap-[2px] items-end min-h-8">
    <div v-for="pile in piles" :key="pile.index">
      <div class="flex flex-col gap-[1px]">
        <Coin v-for="coin in pile.coins" :key="coin.index" />
      </div>
    </div>
  </div>
</template>