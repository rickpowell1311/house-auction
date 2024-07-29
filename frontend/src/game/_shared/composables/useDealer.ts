import { ref } from "vue";

export const useDealer = (options?: { delay: number }) => {

  const dealt = ref<Array<{ number: number, dealt: boolean, order: number }>>([]);
  const toDeal = ref<number[]>([]);

  const dealNextCard = () => {
    setTimeout(() => {
      let next = undefined as number | undefined;
      for (let i = 0; i < toDeal.value.length; i++) {
        const target = dealt.value.find(x => x.number == toDeal.value[i]);
        if (!target) {
          break;
        }
        if (!target.dealt) {
          next = i;
          break;
        }
      }

      if (next === undefined) {
        return;
      }

      const update = [...dealt.value];
      update[next].dealt = true;

      dealt.value = update;
      dealNextCard();
    }, options?.delay ?? 200)
  }

  const deal = (numbers: number[]) => {
    const initial = new Array<{ number: number, dealt: boolean, order: number }>();
    for (let i = 0; i < numbers.length; i++) {
      initial.push({ number: numbers[i], dealt: false, order: i });
    }

    dealt.value = initial;
    toDeal.value = numbers;

    dealNextCard();
  }

  return { dealt, deal }
}