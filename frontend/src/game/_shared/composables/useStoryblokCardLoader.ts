import type { CardStoryblok } from "@/_shared/components/storyblok/schema/component-types";
import { useStoryblokApi } from "@storyblok/vue";
import type { ISbStoryData } from "storyblok";
import { ref } from "vue";

export const useStoryblokCardLoader = () => {

  const storyblokApi = useStoryblokApi();
  const cards = ref<ISbStoryData<CardStoryblok>[]>([]);

  const fetchCards = async () => {
    const response = await storyblokApi.getStories({
      content_type: "card",
      version: import.meta.env.VITE_CMS_SHOW_DRAFTS === "true" ? "draft" : "published",
      per_page: 30
    });

    const result = response.data.stories as ISbStoryData<CardStoryblok>[];

    // Preload images
    result.forEach(x => {
      const img = new Image();
      img.src = x.content.picture.filename;
    })

    return result.sort((a, b) => Number(a.content.value) - Number(b.content.value));
  };

  fetchCards()
    .then(result => cards.value = result);

  return cards;
}