import { computed } from "vue";
import { useDealer } from "./useDealer";
import { useStoryblokCardLoader } from "./useStoryblokCardLoader";

export const useStoryblokCardDealer = () => {

  const storyblokCardLoader = useStoryblokCardLoader();
  const { dealt, deal } = useDealer();
  const isReadyToDeal = computed(() => {
    return storyblokCardLoader.value.length > 0
  })

  return { isReadyToDeal, cards: storyblokCardLoader, deal, dealt };
}