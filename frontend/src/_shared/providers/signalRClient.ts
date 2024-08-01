import type { HubConnection } from "@microsoft/signalr";
import type { InjectionKey } from "vue";

export const Key = Symbol("signalr-client") as InjectionKey<string>;

export class SignalRClientImpl implements SignalRClient {

  private lobby: LobbyClient;

  constructor(hubConnection: HubConnection) {
    this.lobby = new LobbyClientImpl(hubConnection);
  }

  public createLobby(name: string) {
    this.lobby.createLobby(name);
  }

  public handleLobbyCreated(cb: (gameId: string) => void) {
    this.lobby.handleLobbyCreated(cb);
  }
}

export interface SignalRClient extends LobbyClient {
}

export class LobbyClientImpl implements LobbyClient {
  constructor(private hubConnection: HubConnection) {
  }

  async createLobby(name: string) {
    await this.hubConnection.invoke("CreateLobby", name);
  }

  handleLobbyCreated(cb: (gameId: string) => void) {
    this.hubConnection.on("OnLobbyCreated", cb);
  }
}

export interface LobbyClient {
  createLobby: (name: string) => void;
  handleLobbyCreated: (cb: (gameId: string) => void) => void;
}