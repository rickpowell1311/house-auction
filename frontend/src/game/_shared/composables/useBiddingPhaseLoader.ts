import { type GetBiddingPhaseResponse } from "@/_shared/providers/generated/HouseAuction.Bidding.Requests";
import { Key as SignalRKey, type SignalRClient } from "@/_shared/providers/signalRClient";
import { inject, ref } from "vue";

export const useBiddingPhaseLoader = (gameId: string) => {

  const signalRClient = inject<SignalRClient>(SignalRKey);
  const biddingPhase = ref<GetBiddingPhaseResponse | undefined>();
  const error = ref<unknown | undefined>();

  const fetchBiddingPhase = async () => {
    try {
      biddingPhase.value = await signalRClient?.hub.getBiddingPhase({
        gameId
      })
    }
    catch (err) {
      // If the bidding phase can't be loaded - push back to the lobby
      error.value = err;
    }
  }

  fetchBiddingPhase();

  return { biddingPhase, error };
}