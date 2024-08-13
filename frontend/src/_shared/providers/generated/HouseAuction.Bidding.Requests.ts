/* THIS (.ts) FILE IS GENERATED BY Tapper */
/* eslint-disable */
/* tslint:disable */

/** Transpiled from HouseAuction.Bidding.Requests.Bid.BidRequest */
export type BidRequest = {
    /** Transpiled from string */
    gameId?: string;
    /** Transpiled from int */
    amount: number;
}

/** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseRequest */
export type GetBiddingPhaseRequest = {
    /** Transpiled from string */
    gameId?: string;
}

/** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseResponse */
export type GetBiddingPhaseResponse = {
    /** Transpiled from bool */
    isFinished: boolean;
    /** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseDeckResponse */
    deck?: GetBiddingPhaseDeckResponse;
    /** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhasePlayersResponse */
    players?: GetBiddingPhasePlayersResponse;
}

/** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseDeckResponse */
export type GetBiddingPhaseDeckResponse = {
    /** Transpiled from System.Collections.Generic.List<int> */
    propertiesOnTheTable?: number[];
    /** Transpiled from int */
    totalProperties: number;
}

/** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhasePlayersResponse */
export type GetBiddingPhasePlayersResponse = {
    /** Transpiled from string */
    latestWinner?: string;
    /** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseMeResponse */
    me?: GetBiddingPhaseMeResponse;
    /** Transpiled from System.Collections.Generic.List<HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseOtherPersonResponse> */
    others?: GetBiddingPhaseOtherPersonResponse[];
}

/** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseMeResponse */
export type GetBiddingPhaseMeResponse = {
    /** Transpiled from string */
    name?: string;
    /** Transpiled from int */
    order: number;
    /** Transpiled from bool */
    isTurn: boolean;
    /** Transpiled from System.Collections.Generic.List<int> */
    boughtProperties?: number[];
    /** Transpiled from int */
    coins: number;
    /** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseBidResponse */
    bid?: GetBiddingPhaseBidResponse;
}

/** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseOtherPersonResponse */
export type GetBiddingPhaseOtherPersonResponse = {
    /** Transpiled from string */
    name?: string;
    /** Transpiled from int */
    order: number;
    /** Transpiled from bool */
    isTurn: boolean;
    /** Transpiled from int */
    numberOfBoughtProperties: number;
    /** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseBidResponse */
    bid?: GetBiddingPhaseBidResponse;
}

/** Transpiled from HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseBidResponse */
export type GetBiddingPhaseBidResponse = {
    /** Transpiled from int */
    amount?: number;
    /** Transpiled from bool */
    hasPassed: boolean;
}

/** Transpiled from HouseAuction.Bidding.Requests.Pass.PassRequest */
export type PassRequest = {
    /** Transpiled from string */
    gameId?: string;
}

