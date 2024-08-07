<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import { computed, ref } from 'vue';
import CoinStash from '../coins/CoinStash.vue';
import { PhMinus, PhPlus } from '@phosphor-icons/vue';

const props = defineProps<{
  isBidding: boolean;
  isMe: boolean;
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
  <div v-if="!props.isBidding">
    <div class="flex flex-col gap-2 items-center">
      <CoinStash :amount="currentBid" />
      <p v-if="currentBid === 0" class="text-primary text-center">Yet to bid</p>
      <p v-else class="text-primary text-center">Bid {{ currentBid }} coins <span
          v-if="props.available < (minimum ? minimum : 0)">(cannot place any more bids)</span></p>
    </div>
  </div>
  <div v-else-if="props.isMe" class="flex flex-col gap-8">
    <div class="flex flex-col gap-3 items-center">
      <CoinStash :amount="currentBid" />
      <div class="flex gap-4 items-center justify-evenly">
        <PhMinus class="cursor-pointer select-none text-primary font-bold" weight="bold" @click="removeFromBid" />
        <p class="text-white text-2xl select-none">{{ currentBid }}</p>
        <PhPlus class="cursor-pointer select-none text-primary font-bold" weight="bold" @click="addToBid" />
      </div>
      <div class="flex items-center gap-2">
        <Button :disabled="currentBid < (props.minimum ? props.minimum : 0)">Bid</Button>
        <Button>Pass</Button>
      </div>
    </div>
    <div class="flex flex-col gap-2 items-center">
      <CoinStash :amount="remaining" />
      <p class="text-white text-center">{{ remaining }} coins remaining</p>
    </div>
  </div>
  <div v-else>
    <p class="text-primary text-center">"I know <span class="italic">how</span> to bid..."</p>
  </div>
</template>