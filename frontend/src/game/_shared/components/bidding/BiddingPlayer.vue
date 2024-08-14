<script setup lang="ts">
import Bid from './Bid.vue';

const props = defineProps<{
  gameId: string;
  name: string;
  isMe: boolean;
  isBidding: boolean;
  coins: {
    available: number;
    minimum: number;
    amount?: number;
  }
}>();
</script>

<template>
  <div class="flex flex-col items-center justify-end gap-4 w-40">
    <Bid :game-id="props.gameId" :available="props.coins.available" :minimum="props.coins.minimum" :amount="props.coins.amount"
      :is-bidding="props.isBidding" :is-me="props.isMe" />
    <h2 v-if="props.isBidding" class="animate-bounce animate-infinite">{{ name }}</h2>
    <h4 v-else class="text-primary">{{ name }}</h4>
  </div>
</template>