import { computed, ref, watch } from "vue";
import { shuffle } from "../helpers/shuffle";
import { useStoryblokCardDealer } from "./useStoryblokCardDealer";

export const useNextCardsDealer = () => {
  const { allCards, dealtCards, deal } = useStoryblokCardDealer();
  const usedCards = ref<number[]>([]);
  const dealRequested = ref<number | undefined>();

  const availableCards = computed(() => {
    return allCards.value.filter(x => !usedCards.value.includes(x));
  });

  const dealNext = (numberOfCards: number) => {
    if (availableCards.value.length === 0) {
      dealRequested.value = 4;
      return;
    }

    const randomized = shuffle(availableCards.value);

    const next = randomized
      .slice(0, numberOfCards <= randomized.length ? numberOfCards : randomized.length);

    usedCards.value = [...usedCards.value, ...next];
    deal(next);
  };

  watch(availableCards, () => {
    if (dealRequested.value) {
      dealNext(dealRequested.value);
      dealRequested.value = undefined;
    }
  })

  return { allCards, dealtCards, dealNext };
}