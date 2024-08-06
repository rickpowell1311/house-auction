/* THIS (.ts) FILE IS GENERATED BY Tapper */
/* eslint-disable */
/* tslint:disable */

/** Transpiled from HouseAuction.Lobby.Requests.CreateLobby.CreateLobbyRequest */
export type CreateLobbyRequest = {
    /** Transpiled from string */
    name?: string;
}

/** Transpiled from HouseAuction.Lobby.Requests.CreateLobby.CreateLobbyResponse */
export type CreateLobbyResponse = {
    /** Transpiled from string */
    gameId?: string;
}

/** Transpiled from HouseAuction.Lobby.Requests.FetchLobby.FetchLobbyRequest */
export type FetchLobbyRequest = {
    /** Transpiled from string */
    gameId?: string;
}

/** Transpiled from HouseAuction.Lobby.Requests.FetchLobby.FetchLobbyResponse */
export type FetchLobbyResponse = {
    /** Transpiled from System.Collections.Generic.List<string> */
    gamers?: string[];
}

/** Transpiled from HouseAuction.Lobby.Requests.GetMyName.GetMyNameRequest */
export type GetMyNameRequest = {
    /** Transpiled from string */
    gameId?: string;
}

/** Transpiled from HouseAuction.Lobby.Requests.GetMyName.GetMyNameResponse */
export type GetMyNameResponse = {
    /** Transpiled from string */
    name?: string;
}

/** Transpiled from HouseAuction.Lobby.Requests.JoinLobby.JoinLobbyRequest */
export type JoinLobbyRequest = {
    /** Transpiled from string */
    name?: string;
    /** Transpiled from string */
    gameId?: string;
}

/** Transpiled from HouseAuction.Lobby.Requests.ReadyUp.ReadyUpRequest */
export type ReadyUpRequest = {
    /** Transpiled from string */
    gameId?: string;
    /** Transpiled from string */
    name?: string;
}

