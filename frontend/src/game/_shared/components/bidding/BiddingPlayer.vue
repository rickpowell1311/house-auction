<script setup lang="ts">
import Bid from './Bid.vue';

const props = defineProps<{
  name: string;
  isBidding: boolean;
  coins: {
    available: number;
    minimum: number;
    amount?: number;
  }
}>();
</script>

<template>
  <div class="flex flex-col items-center justify-end gap-4 min-w-72 min-h-72">
    <Bid :available="props.coins.available" :minimum="props.coins.minimum" :amount="props.coins.amount"
      :is-bidding="props.isBidding" />
    <h3 v-if="props.isBidding" class="animate-bounce animate-infinite">{{ name }}</h3>
    <h4 v-else>{{ name }}</h4>
  </div>
</template>