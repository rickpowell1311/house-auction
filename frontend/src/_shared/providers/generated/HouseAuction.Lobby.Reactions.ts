/* THIS (.ts) FILE IS GENERATED BY Tapper */
/* eslint-disable */
/* tslint:disable */

/** Transpiled from HouseAuction.Lobby.Reactions.OnGameBegun.OnGameBegunReaction */
export type OnGameBegunReaction = {
    /** Transpiled from string */
    gameId?: string;
}

/** Transpiled from HouseAuction.Lobby.Reactions.OnLobbyMembersChanged.OnLobbyMembersChangedReaction */
export type OnLobbyMembersChangedReaction = {
    /** Transpiled from System.Collections.Generic.List<HouseAuction.Lobby.Reactions.OnLobbyMembersChanged.OnLobbyMembersChangedReactionGamer> */
    gamers?: OnLobbyMembersChangedReactionGamer[];
}

/** Transpiled from HouseAuction.Lobby.Reactions.OnLobbyMembersChanged.OnLobbyMembersChangedReactionGamer */
export type OnLobbyMembersChangedReactionGamer = {
    /** Transpiled from string */
    name?: string;
    /** Transpiled from bool */
    isMe: boolean;
    /** Transpiled from bool */
    isReady: boolean;
}

