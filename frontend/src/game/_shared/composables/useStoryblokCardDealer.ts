import type { CardStoryblok } from "@/_shared/components/storyblok/schema/component-types";
import { useStoryblokApi } from "@storyblok/vue";
import type { ISbStoryData } from "storyblok";
import { computed, ref } from "vue";
import { useDealer } from "./useDealer";

export const useStoryblokCardDealer = () => {

  const storyblokApi = useStoryblokApi();
  const allCards = ref<ISbStoryData<CardStoryblok>[]>([]);
  const allCardNumbers = computed(() => allCards.value.map(x => Number(x.content.value)));
  const { dealt, deal } = useDealer();

  const dealtCards = computed(() => {
    return dealt.value
      .filter(x => x.dealt === true)
      .sort((a, b) => a.order - b.order)
      .map(x => allCards.value.find(y => Number(y.content.value) === x.number))
      .filter(x => x);
  });

  const fetchCards = async () => {
    const response = await storyblokApi.getStories({
      content_type: "card",
      version: import.meta.env.VITE_CMS_SHOW_DRAFTS === "true" ? "draft" : "published",
      per_page: 30
    });
    console.log(response.data.stories.map(x => x.content?.value));
    return response.data.stories as ISbStoryData<CardStoryblok>[];
  };

  fetchCards()
    .then(result => allCards.value = result);

  return { allCards: allCardNumbers, dealtCards, deal };
}