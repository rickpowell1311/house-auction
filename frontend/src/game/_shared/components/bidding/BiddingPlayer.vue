<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
import { type SignalRClient, Key as SignalRClientKey } from '@/_shared/providers/signalRClient';
import { PhMinus, PhPlus } from '@phosphor-icons/vue';
import { computed, inject, onMounted, ref, watch } from 'vue';
import CoinStash from '../coins/CoinStash.vue';

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

const available = ref(props.coins.available);
const isBidding = ref(props.isBidding);
const minimum = ref(props.coins.minimum);
const currentBid = ref(props.coins.amount ?? 0);
const stagedBid = ref(props.coins.amount ?? 0);
const remaining = computed(() => available.value - stagedBid.value);
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

        if (reaction.player === props.name) {
          stagedBid.value = 0;
          available.value = reaction.result?.remainingCoins ?? 0;
        }

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
      stagedBid.value = 0;
      hasPassed.value = false;
    }
  } as IHouseAuctionReceiver)
})

const addToBid = () => {
  if (stagedBid.value < available.value) {
    stagedBid.value++;
  }
};

const removeFromBid = () => {
  if (stagedBid.value > 0) {
    stagedBid.value--;
  }
};

const makeBid = async () => {
  await singalRClient?.hub.bid({
    amount: stagedBid.value,
    gameId: props.gameId
  })
}

const pass = async () => {
  await singalRClient?.hub.pass({
    gameId: props.gameId
  })
}

watch(isBidding, val => {
  if (val && props.isMe) {
    // By default set the bid to the minimum
    stagedBid.value = minimum.value;
  }
})

</script>

<template>
  <div class="flex flex-col flex-grow items-center justify-end gap-4">
    <div class="flex flex-col gap-4">
      <div v-if="!isBidding">
        <div class="flex flex-col gap-2 items-center">
          <CoinStash :amount="currentBid" />
          <p v-if="hasPassed" class="text-primary text-center">Passed</p>
          <p v-else-if="currentBid === 0" class="text-primary text-center">Yet to bid</p>
          <p v-if="currentBid > 0" class="text-primary text-center">Bid {{ currentBid }} coins</p>
        </div>
      </div>
      <div v-if="props.isMe" class="flex flex-col gap-8">
        <div v-if="isBidding" class="flex flex-col gap-3 items-center">
          <CoinStash :amount="stagedBid" />
          <div v-if="!hasPassed" class="flex gap-4 items-center justify-evenly">
            <PhMinus class="cursor-pointer select-none text-primary font-bold text-3xl" weight="bold"
              @click="removeFromBid" />
            <p class="text-white text-3xl select-none">{{ stagedBid }}</p>
            <PhPlus class="cursor-pointer select-none text-primary font-bold text-3xl" weight="bold"
              @click="addToBid" />
          </div>
          <div class="flex items-center gap-5">
            <Button v-if="!hasPassed" :disabled="stagedBid == 0 || stagedBid < (minimum ? minimum : 0)"
              @click="makeBid">Bid</Button>
            <Button @click="pass">Pass</Button>
          </div>
        </div>
        <div class="flex flex-col gap-2 items-center">
          <CoinStash :amount="remaining" />
          <p class="text-white text-center">{{ remaining }} coins remaining</p>
        </div>
      </div>
      <div v-if="isBidding && !props.isMe">
        <p class="text-primary text-center">"I know <span class="italic">how</span> to bid..."</p>
      </div>
    </div>
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