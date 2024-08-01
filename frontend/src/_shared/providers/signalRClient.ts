import type { HubConnection } from "@microsoft/signalr";
import type { InjectionKey } from "vue";

export const Key = Symbol("signalr-client") as InjectionKey<string>;

export class SignalRClientImpl implements SignalRClient {

  private lobby: LobbyClient;

  constructor(hubConnection: HubConnection) {
    this.lobby = new LobbyClientImpl(hubConnection);
  }

  public async createLobby(name: string) {
    await this.lobby.createLobby(name);
  }

  public async joinLobby(gameId: string, name: string) {
    await this.lobby.joinLobby(gameId, name);
  }

  public async fetchLobby(name: string) {
    return await this.lobby.fetchLobby(name);
  }

  public handleLobbyCreated(cb: (gameId: string) => void) {
    this.lobby.handleLobbyCreated(cb);
  }

  public handleLobbyMembersChanged(cb: (players: string[]) => void) {
    this.lobby.handleLobbyMembersChanged(cb);
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

  async joinLobby(gameId: string, name: string) {
    await this.hubConnection.invoke("JoinLobby", gameId, name);
  }

  async fetchLobby(name: string) {
    const result = await this.hubConnection.invoke("FetchLobby", name);

    return {
      players: result as string[]
    }
  }

  handleLobbyMembersChanged(cb: (players: string[]) => void) {
    this.hubConnection.on("OnLobbyMembersChanged", cb);
  }

  handleLobbyCreated(cb: (gameId: string) => void) {
    this.hubConnection.on("OnLobbyCreated", cb);
  }
}

export interface LobbyClient {
  createLobby: (name: string) => Promise<void>;
  joinLobby: (gameId: string, name: string) => Promise<void>;
  fetchLobby: (name: string) => Promise<{ players: string[] }>;
  handleLobbyMembersChanged: (cb: (players: string[]) => void) => void;
  handleLobbyCreated: (cb: (gameId: string) => void) => void;
}