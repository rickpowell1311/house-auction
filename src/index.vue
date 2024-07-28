<script setup lang="ts">
import Button from "./_shared/components/Button.vue";
import Section from "./_shared/components/Section.vue";
import Main from "./_shared/components/Main.vue";
</script>
<template>
  <div class="flex flex-col h-full items-center">
    <div class="h-1/3" />
    <div class="container mx-auto">
      <Main>
        <div class="flex justify-center">
          <div class="flex flex-col items-start justify-center">
            <h1 class="text-[72px] font-bold">House Auction</h1>
            <Section>
              <div class="flex flex-col gap-4 items-start">
                <Button>Create Game</Button>
                <Button>Join Game</Button>
              </div>
            </Section>
          </div>
        </div>
      </Main>
    </div>
  </div>
</template>