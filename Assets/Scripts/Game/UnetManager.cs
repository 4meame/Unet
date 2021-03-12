using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnetManager : NetworkManager
{
    public static UnetManager instance;
    public bool isServer;
    public bool isClient;
    void Awake() {
        instance = this;
    }
    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);
        Debug.Log("OnClientConnect");
    }
    public override void OnClientDisconnect(NetworkConnection conn) {
        base.OnClientDisconnect(conn);
        Debug.Log("OnClientDisconnect");
    }
    public override void OnClientSceneChanged(NetworkConnection conn) {
        base.OnClientSceneChanged(conn);
        Debug.Log("OnClientSceneChanged");
    }
    public virtual void OnServerConnect(NetworkConnection conn) {
        base.OnServerConnect(conn);
        Debug.Log("OnServerConnect");
    }
    public virtual void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);
        Debug.Log("OnServerConnect");
    }
    public override void OnServerSceneChanged(string sceneName) {
        base.OnServerSceneChanged(sceneName);
        Debug.Log("OnClientSceneChanged");
    }
    public override void OnStartServer() {
        base.OnStartServer();
        isServer = true;
        Debug.Log("OnStartServer");
    }
    public override void OnStartClient(NetworkClient client) {
        base.OnStartClient(client);
        isClient = true;
        Debug.Log("OnStartClient");
    }
    public override void OnStartHost() {
        base.OnStartHost();
        Debug.Log("OnStartHost");
    }
    public override void OnStopServer() {
        base.OnStopServer();
        isServer = false;
        Debug.Log("OnStopServer");
    }
    public override void OnStopHost() {
        base.OnStopHost();
        isClient = false;
        Debug.Log("OnStopHost");
    }
}
