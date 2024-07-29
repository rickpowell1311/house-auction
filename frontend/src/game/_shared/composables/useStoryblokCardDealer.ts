import type { CardStoryblok } from "@/_shared/components/storyblok/schema/component-types";
import { useStoryblokApi } from "@storyblok/vue";
import type { ISbStoryData } from "storyblok";
import { computed, ref } from "vue";
import { useDealer } from "./useDealer";

export const useStoryblokCardDealer = () => {

  const storyblokApi = useStoryblokApi();
  const allCards = ref<ISbStoryData<CardStoryblok>[]>([]);
  const { dealt, deal } = useDealer();

  const fetchCards = async () => {
    const response = await storyblokApi.getStories({
      content_type: "card",
      version: import.meta.env.VITE_CMS_SHOW_DRAFTS === "true" ? "draft" : "published"
    });
    return response.data.stories as ISbStoryData<CardStoryblok>[];
  };

  const dealtCards = computed(() => {
    return dealt.value
      .filter(x => x.dealt)
      .sort((a, b) => a.order - b.order)
      .map(x => allCards.value.find(y => Number(y.content.value) === x.number))
      .filter(x => x);
  });

  fetchCards()
    .then(result => allCards.value = result);

  return { dealtCards, deal };
}