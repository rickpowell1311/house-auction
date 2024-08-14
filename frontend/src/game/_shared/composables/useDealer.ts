import { ref } from "vue";

export const useDealer = (options?: { delay: number }) => {

  const dealt = ref<Array<number>>([]);
  const toDeal = ref<number[]>([]);

  const dealNextCard = () => {
    setTimeout(() => {
      if (dealt.value.length === toDeal.value.length) {
        return;
      }

      dealt.value = toDeal.value.slice(0, dealt.value.length + 1)
      dealNextCard();
    }, options?.delay ?? 200)
  }

  const deal = (numbers: number[]) => {
    dealt.value = new Array<number>();
    toDeal.value = numbers;

    dealNextCard();
  }

  return { dealt, deal }
}