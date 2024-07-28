<script setup lang="ts">
import JumpIn from "./_shared/components/animations/JumpIn.vue";
import Button from "./_shared/components/Button.vue";
import Main from "./_shared/components/layout/Main.vue";
import Section from "./_shared/components/layout/Section.vue";
</script>
<template>
  <div class="h-1/6" />
  <JumpIn>
    <div class="container mx-auto">
      <Main>
        <div class="flex justify-center">
          <div class="flex flex-col items-start justify-center">
            <h1>House Auction</h1>
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
  </JumpIn>
</template>