<script setup lang="ts">
import type { CardStoryblok } from "@/_shared/components/storyblok/schema/component-types";
import { useStoryblokApi, type ISbStoryData } from "@storyblok/vue";
import { onMounted, ref } from "vue";
import Main from "../_shared/components/layout/Main.vue";
import Card from "./_shared/components/Card.vue";
import CardContents from "./_shared/components/storyblok/CardContents.vue";
import Table from "./_shared/components/Table.vue";

const storyblokApi = useStoryblokApi();
const cards = ref<ISbStoryData<CardStoryblok>[]>([]);

const fetchCards = async () => {
  const response = await storyblokApi.getStories({
    content_type: "card",
    version: import.meta.env.VITE_CMS_SHOW_DRAFTS === "true" ? "draft" : "published"
  });
  return response.data.stories as ISbStoryData<CardStoryblok>[];
};

onMounted(async () => {
  cards.value = await fetchCards();
});

</script>
<template>
  <div class="h-1/6" />
  <div class="container mx-auto">
    <Main>
      <div class="flex justify-center items-center">
        <Table>
          <div class="flex gap-2 flex-wrap">
            <template v-for="card in cards" :key="card.content.value">
              <Card :is-face-up="false" class="animate-flip-down animate-duration-200">
                <CardContents :blok="card.content" />
              </Card>
              <Card :is-face-up="true" />
            </template>
          </div>
        </Table>
      </div>
    </Main>
  </div>
</template>