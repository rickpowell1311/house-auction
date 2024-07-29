<script setup lang="ts">
import JumpIn from "./_shared/components/animations/JumpIn.vue";
import Button from "./_shared/components/Button.vue";
import Main from "./_shared/components/layout/Main.vue";
</script>
<template>
  <div class="h-1/6" />
  <JumpIn>
    <div class="container mx-auto">
      <Main>
        <div class="flex justify-center">
          <div class="flex flex-col items-center justify-center gap-16">
            <h1 class="text-center text-shadow-sm shadow-gray-300">House Auction</h1>
            <div class="flex flex-col gap-4 items-start">
              <Button>Create Game</Button>
              <Button>Join Game</Button>
            </div>
          </div>
        </div>
      </Main>
    </div>
  </JumpIn>
</template>