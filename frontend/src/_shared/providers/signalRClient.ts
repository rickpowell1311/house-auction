import type { InjectionKey } from "vue";
import type { IHouseAuctionHub, IHouseAuctionReceiver } from "./generated/TypedSignalR.Client/HouseAuction";

export const Key = Symbol("signalr-client") as InjectionKey<string>;

export interface SignalRClient {
  hub: IHouseAuctionHub;
  subscribe: (receiver: IHouseAuctionReceiver) => void;
}