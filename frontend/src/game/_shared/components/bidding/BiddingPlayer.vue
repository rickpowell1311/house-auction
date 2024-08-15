<script setup lang="ts">
import Bid from './Bid.vue';

const props = defineProps<{
  gameId: string;
  name: string;
  isMe: boolean;
  isBidding: boolean;
  hasPassed: boolean;
  coins: {
    available: number;
    minimum: number;
    amount?: number;
  }
}>();

const emit = defineEmits<{
  pass: []
  bid: [amount: number]
}>();

const onBid = (amount: number) => {
  emit("bid", amount);
}

const onPass = () => {
  emit("pass");
}

</script>

<template>
  <div class="flex flex-col flex-grow items-center justify-end gap-4">
    <Bid :game-id="props.gameId" :available="props.coins.available" :minimum="props.coins.minimum"
      :amount="props.coins.amount" :is-bidding="props.isBidding" :is-me="props.isMe" :name="props.name"
      :has-passed="hasPassed" @bid="onBid" @pass="onPass" />
    <h2 v-if="props.isBidding" class="animate-bounce animate-infinite">
      <span v-if="props.isMe" class="text-white">{{ name }}</span>
      <span v-else class="text-primary">{{ name }}</span>
    </h2>
    <h4 v-else>
      <span v-if="props.isMe" class="text-white">{{ name }}</span>
      <span v-else class="text-primary">{{ name }}</span>
    </h4>
  </div>
</template>