<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key as SignalRClientKey } from '@/_shared/providers/signalRClient';
import { PhMinus, PhPlus } from '@phosphor-icons/vue';
import { computed, inject, onMounted, ref } from 'vue';
import CoinStash from '../coins/CoinStash.vue';

const props = defineProps<{
  gameId: string;
  isBidding: boolean;
  isMe: boolean;
  hasPassed: boolean;
  name: string;
  available: number;
  amount?: number;
  minimum?: number;
}>();

const available = ref(props.available);
const isBidding = ref(props.isBidding);
const minimum = ref(props.minimum);
const currentBid = ref(props.amount ? props.amount : 0);
const remaining = computed(() => available.value - currentBid.value);
const hasPassed = ref(props.hasPassed);

const emit = defineEmits<{
  pass: []
  bid: [amount: number]
}>();

const singalRClient = inject<SignalRClient>(SignalRClientKey);

onMounted(() => {
  singalRClient?.subscribe({
    onPlayerTurnComplete(reaction) {
      if (reaction.result?.passed === true && reaction.player === props.name) {
        hasPassed.value = true;
        emit("pass");
      } else if (reaction.result?.bid) {
        minimum.value = reaction.result.bid + 1;
        if (reaction.player === props.name) {
          currentBid.value = reaction.result?.bid
          emit("bid", currentBid.value);
        }
      }

      isBidding.value = reaction.nextPlayer === props.name
    },
    onBiddingRoundComplete(reaction) {
      if (props.isMe) {
        available.value = reaction.coinsRemaining;
      }
      minimum.value = 0;
      currentBid.value = 0;
      hasPassed.value = false;
    }
  } as IHouseAuctionReceiver)
})

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

const makeBid = async () => {
  await singalRClient?.hub.bid({
    amount: currentBid.value,
    gameId: props.gameId
  })
}

const pass = async () => {
  await singalRClient?.hub.pass({
    gameId: props.gameId
  })
}

</script>

<template>
  <div v-if="!isBidding">
    <div class="flex flex-col gap-2 items-center">
      <CoinStash :amount="currentBid" />
      <p v-if="hasPassed" class="text-primary text-center">Passed</p>
      <p v-else-if="currentBid === 0" class="text-primary text-center">Yet to bid</p>
      <p v-if="currentBid > 0" class="text-primary text-center">Bid {{ currentBid }} coins</p>
    </div>
  </div>
  <div v-else-if="props.isMe" class="flex flex-col gap-8">
    <div class="flex flex-col gap-3 items-center">
      <CoinStash :amount="currentBid" />
      <div v-if="!hasPassed" class="flex gap-4 items-center justify-evenly">
        <PhMinus class="cursor-pointer select-none text-primary font-bold text-3xl" weight="bold"
          @click="removeFromBid" />
        <p class="text-white text-3xl select-none">{{ currentBid }}</p>
        <PhPlus class="cursor-pointer select-none text-primary font-bold text-3xl" weight="bold" @click="addToBid" />
      </div>
      <div class="flex items-center gap-5">
        <Button v-if="!hasPassed" :disabled="currentBid < (minimum ? minimum : 0)" @click="makeBid">Bid</Button>
        <Button @click="pass">Pass</Button>
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