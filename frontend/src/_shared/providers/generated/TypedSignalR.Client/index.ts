/* THIS (.ts) FILE IS GENERATED BY TypedSignalR.Client.TypeScript */
/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
import type { HubConnection, IStreamResult, Subject } from '@microsoft/signalr';
import type { IHouseAuctionHub, IHouseAuctionReceiver } from './HouseAuction';
import type { CreateLobbyRequest, CreateLobbyResponse, FetchLobbyRequest, FetchLobbyResponse, JoinLobbyRequest, ReadyUpRequest, StartGameRequest, GetDisconnectedPlayersRequest, GetDisconnectedPlayersResponse, ReconnectRequest } from '../HouseAuction.Lobby.Requests';
import type { GetBiddingPhaseRequest, GetBiddingPhaseResponse, BidRequest, PassRequest } from '../HouseAuction.Bidding.Requests';
import type { OnLobbyMembersChangedReaction, OnGameReadinessChangedReaction, OnGameStartedReaction } from '../HouseAuction.Lobby.Reactions';
import type { OnPlayerTurnComplete, OnBiddingRoundComplete } from '../HouseAuction.Bidding.Reactions';


// components

export type Disposable = {
    dispose(): void;
}

export type HubProxyFactory<T> = {
    createHubProxy(connection: HubConnection): T;
}

export type ReceiverRegister<T> = {
    register(connection: HubConnection, receiver: T): Disposable;
}

type ReceiverMethod = {
    methodName: string,
    method: (...args: any[]) => void
}

class ReceiverMethodSubscription implements Disposable {

    public constructor(
        private connection: HubConnection,
        private receiverMethod: ReceiverMethod[]) {
    }

    public readonly dispose = () => {
        for (const it of this.receiverMethod) {
            this.connection.off(it.methodName, it.method);
        }
    }
}

// API

export type HubProxyFactoryProvider = {
    (hubType: "IHouseAuctionHub"): HubProxyFactory<IHouseAuctionHub>;
}

export const getHubProxyFactory = ((hubType: string) => {
    if(hubType === "IHouseAuctionHub") {
        return IHouseAuctionHub_HubProxyFactory.Instance;
    }
}) as HubProxyFactoryProvider;

export type ReceiverRegisterProvider = {
    (receiverType: "IHouseAuctionReceiver"): ReceiverRegister<IHouseAuctionReceiver>;
}

export const getReceiverRegister = ((receiverType: string) => {
    if(receiverType === "IHouseAuctionReceiver") {
        return IHouseAuctionReceiver_Binder.Instance;
    }
}) as ReceiverRegisterProvider;

// HubProxy

class IHouseAuctionHub_HubProxyFactory implements HubProxyFactory<IHouseAuctionHub> {
    public static Instance = new IHouseAuctionHub_HubProxyFactory();

    private constructor() {
    }

    public readonly createHubProxy = (connection: HubConnection): IHouseAuctionHub => {
        return new IHouseAuctionHub_HubProxy(connection);
    }
}

class IHouseAuctionHub_HubProxy implements IHouseAuctionHub {

    public constructor(private connection: HubConnection) {
    }

    public readonly createLobby = async (request: CreateLobbyRequest): Promise<CreateLobbyResponse> => {
        return await this.connection.invoke("CreateLobby", request);
    }

    public readonly fetchLobby = async (request: FetchLobbyRequest): Promise<FetchLobbyResponse> => {
        return await this.connection.invoke("FetchLobby", request);
    }

    public readonly joinLobby = async (request: JoinLobbyRequest): Promise<void> => {
        return await this.connection.invoke("JoinLobby", request);
    }

    public readonly readyUp = async (request: ReadyUpRequest): Promise<void> => {
        return await this.connection.invoke("ReadyUp", request);
    }

    public readonly startGame = async (request: StartGameRequest): Promise<void> => {
        return await this.connection.invoke("StartGame", request);
    }

    public readonly getDisconnectedPlayers = async (request: GetDisconnectedPlayersRequest): Promise<GetDisconnectedPlayersResponse> => {
        return await this.connection.invoke("GetDisconnectedPlayers", request);
    }

    public readonly reconnect = async (request: ReconnectRequest): Promise<void> => {
        return await this.connection.invoke("Reconnect", request);
    }

    public readonly onDisconnectedAsync = async (): Promise<void> => {
        return await this.connection.invoke("OnDisconnectedAsync");
    }

    public readonly getBiddingPhase = async (request: GetBiddingPhaseRequest): Promise<GetBiddingPhaseResponse> => {
        return await this.connection.invoke("GetBiddingPhase", request);
    }

    public readonly bid = async (request: BidRequest): Promise<void> => {
        return await this.connection.invoke("Bid", request);
    }

    public readonly pass = async (request: PassRequest): Promise<void> => {
        return await this.connection.invoke("Pass", request);
    }
}


// Receiver

class IHouseAuctionReceiver_Binder implements ReceiverRegister<IHouseAuctionReceiver> {

    public static Instance = new IHouseAuctionReceiver_Binder();

    private constructor() {
    }

    public readonly register = (connection: HubConnection, receiver: IHouseAuctionReceiver): Disposable => {

        const __onLobbyMembersChanged = (...args: [OnLobbyMembersChangedReaction]) => receiver.onLobbyMembersChanged(...args);
        const __onGameReadinessChanged = (...args: [OnGameReadinessChangedReaction]) => receiver.onGameReadinessChanged(...args);
        const __onGameStarted = (...args: [OnGameStartedReaction]) => receiver.onGameStarted(...args);
        const __onPlayerTurnComplete = (...args: [OnPlayerTurnComplete]) => receiver.onPlayerTurnComplete(...args);
        const __onBiddingRoundComplete = (...args: [OnBiddingRoundComplete]) => receiver.onBiddingRoundComplete(...args);
        const __notifyError = (...args: [string]) => receiver.notifyError(...args);

        connection.on("OnLobbyMembersChanged", __onLobbyMembersChanged);
        connection.on("OnGameReadinessChanged", __onGameReadinessChanged);
        connection.on("OnGameStarted", __onGameStarted);
        connection.on("OnPlayerTurnComplete", __onPlayerTurnComplete);
        connection.on("OnBiddingRoundComplete", __onBiddingRoundComplete);
        connection.on("NotifyError", __notifyError);

        const methodList: ReceiverMethod[] = [
            { methodName: "OnLobbyMembersChanged", method: __onLobbyMembersChanged },
            { methodName: "OnGameReadinessChanged", method: __onGameReadinessChanged },
            { methodName: "OnGameStarted", method: __onGameStarted },
            { methodName: "OnPlayerTurnComplete", method: __onPlayerTurnComplete },
            { methodName: "OnBiddingRoundComplete", method: __onBiddingRoundComplete },
            { methodName: "NotifyError", method: __notifyError }
        ]

        return new ReceiverMethodSubscription(connection, methodList);
    }
}

