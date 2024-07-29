<script setup lang="ts">
import { onMounted } from "vue";
import Button from "../_shared/components/Button.vue";
import Main from "../_shared/components/layout/Main.vue";
import Section from "../_shared/components/layout/Section.vue";
import Flip from "./_shared/components/animations/Flip.vue";
import Card from "./_shared/components/Card.vue";
import Deck from "./_shared/components/Deck.vue";
import CardContents from "./_shared/components/storyblok/CardContents.vue";
import Table from "./_shared/components/Table.vue";
import { useStoryblokCardDealer } from "./_shared/composables/useStoryblokCardDealer";

const { dealtCards, deal } = useStoryblokCardDealer();

onMounted(() => {
  deal([1, 30]);
});

</script>
<template>
  <div class="h-1/6" />
  <div class="container mx-auto">
    <Main>
      <div class="flex items-center">
        <Table>
          <div class="flex gap-2 flex-wrap items-center w-full">
            <Deck />
            <template v-for="card in dealtCards" :key="card.content.value">
              <Flip>
                <Card :is-face-up="true">
                  <CardContents v-if="card" :blok="card.content" />
                </Card>
              </Flip>
            </template>
          </div>
          <Section>
            <Button @click="() => deal([30, 1])">Redeal</Button>
          </Section>
        </Table>
      </div>
    </Main>
  </div>
</template>