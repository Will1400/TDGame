﻿using System.Linq;
using Mirror;
using TDGame.Events.Base;
using UnityEngine;

namespace TDGame.Network.Player
{
    public class PlayerManager : NetworkBehaviour
    {
        public static PlayerManager Instance;

        public readonly SyncList<PlayerData> PlayerDatas = new SyncList<PlayerData>();

        [SerializeField]
        private GameEvent clientPlayersChangedEvent;

        public void Awake()
        {
            Instance ??= this;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            Instance = null;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            Instance = null;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            UpdatePlayers();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            PlayerDatas.Callback += (op, index, item, newItem) =>
            {
                Debug.Log("Player data changed ");
                clientPlayersChangedEvent.Raise();
            };
            clientPlayersChangedEvent.Raise();
        }

        [Server]
        public void UpdatePlayers()
        {
            PlayerDatas.Clear();
            PlayerDatas.AddRange(TDGameNetworkManager.Instance.connectedPlayers.Values.ToArray());
        }

        [Server]
        public void PlayerConnected(NetworkConnection connection)
        {
            var connected = TDGameNetworkManager.Instance.connectedPlayers[connection.connectionId];
            PlayerDatas.Add(TDGameNetworkManager.Instance.connectedPlayers[connection.connectionId]);
        }

        [Server]
        public void PlayerDisconnected()
        {
            PlayerDatas.Clear();
            PlayerDatas.AddRange(TDGameNetworkManager.Instance.connectedPlayers.Values.ToArray());
        }
    }
}