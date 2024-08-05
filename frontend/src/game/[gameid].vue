<script setup lang="ts">
import Main from "../_shared/components/layout/Main.vue";
import BiddingRound from "./_shared/components/bidding/BiddingRound.vue";

</script>
<template>
  <div class="h-1/6" />
  <div class="container mx-auto">
    <Main>
      <div class="flex flex-col gap-16">
        <BiddingRound />
      </div>
    </Main>
  </div>
</template>