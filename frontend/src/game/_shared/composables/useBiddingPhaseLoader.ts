import { type GetBiddingPhaseResponse } from "@/_shared/providers/generated/HouseAuction.Bidding.Requests";
import { Key as SignalRKey, type SignalRClient } from "@/_shared/providers/signalRClient";
import { inject, ref } from "vue";

export const useBiddingPhaseLoader = (gameId: string) => {
  const signalRClient = inject<SignalRClient>(SignalRKey);

  const biddingPhase = ref<GetBiddingPhaseResponse | undefined>();

  const fetchBiddingPhase = async () => {
    biddingPhase.value = await signalRClient?.hub.getBiddingPhase({
      gameId
    })
  }

  fetchBiddingPhase();

  return biddingPhase;
}