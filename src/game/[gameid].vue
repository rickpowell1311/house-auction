<script setup lang="ts">
import type { CardStoryblok } from "@/_shared/components/storyblok/schema/component-types";
import { useStoryblokApi, type ISbStoryData } from "@storyblok/vue";
import { onMounted, ref } from "vue";
import JumpIn from "../_shared/components/animations/JumpIn.vue";
import Main from "../_shared/components/layout/Main.vue";
import Card from "./_shared/components/storyblok/Card.vue";

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
  <JumpIn>
    <div class="container mx-auto">
      <Main>
        <div class="flex gap-2">
          <template v-for="card in cards" :key="card.content.value">
            <Card :blok="card.content" />
          </template>
        </div>
      </Main>
    </div>
  </JumpIn>
</template>