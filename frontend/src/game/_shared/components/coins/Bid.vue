<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import { computed, ref } from 'vue';
import CoinStash from './CoinStash.vue';

const props = defineProps<{
  available: number;
  amount?: number;
  minimum?: number;
}>();

const currentBid = ref(props.amount ? props.amount : 0);
const remaining = computed(() => props.available - currentBid.value);

const addToBid = () => {
  if (currentBid.value < props.available) {
    currentBid.value++;
  }
};

const removeFromBid = () => {
  if (currentBid.value > 0) {
    currentBid.value--;
  }
};

</script>

<template>
  <div class="flex flex-col gap-8">
    <div class="flex flex-col gap-3">
      <div class="flex gap-4 items-center justify-evenly">
        <span class="material-symbols-rounded cursor-pointer text-primary font-bold"
          @click="removeFromBid">remove</span>
        <div class="text-white text-2xl">{{ currentBid }}</div>
        <span class="material-symbols-rounded cursor-pointer text-primary font-bold" @click="addToBid">add</span>
      </div>
      <Button :disabled="currentBid < (props.minimum ? props.minimum : 0)">Bid</Button>
    </div>
    <div class="flex flex-col gap-2">
      <CoinStash :amount="remaining" />
      <p class="text-white text-center">{{ remaining }} coins remaining</p>
    </div>
  </div>
</template>