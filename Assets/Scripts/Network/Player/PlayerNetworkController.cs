﻿using Mirror;
using System;
using System.Collections;
using UnityEngine;

namespace TDGame.Network.Player
{
    public class PlayerNetworkController : NetworkBehaviour
    {
        [SyncVar]
        [SerializeField]
        string Name;

        [SyncVar]
        [SerializeField]
        int connectionId;

        public void Setup(PlayerData playerData)
        {
            Name = playerData.Name;
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Cmd_SpawnTestCube();
                }
            }
        }

        [Command]
        void Cmd_SpawnTestCube()
        {
            Debug.Log(netIdentity.connectionToClient);
            var building = Instantiate(BuildManager.Instance.GetBuilding(0));
            NetworkServer.Spawn(building, connectionToClient);
        }
    }
}