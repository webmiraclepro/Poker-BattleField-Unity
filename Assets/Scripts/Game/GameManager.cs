﻿using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.Poker 
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance = null;

        [SerializeField]
        private Transform[] _playerSlots;

        [SerializeField]
        private ControlPanel _controlPanel;

        private int _currentPlayer;

        private PokerPlayer _player;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            Hashtable props = new Hashtable
            {
                {PokerGame.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            StartGame();
        }
        
        private void StartGame()
        {
            Debug.Log("StartGame!");
            int playerIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (playerIdx >= 0 && playerIdx < _playerSlots.Length)
            {
                Transform transform = _playerSlots[playerIdx];
                PhotonNetwork.Instantiate("Player", transform.position, transform.rotation);
                
                // Initialize current player with master client number
                if (PhotonNetwork.IsMasterClient) 
                {
                    photonView.RPC("SetCurrentPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber); 
                }
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected");
            SceneManager.LoadScene("LobbyScene");
        }

        public override void OnLeftRoom()
        {
            Debug.Log("LeftRoom");
            PhotonNetwork.Disconnect();
        }

        public void SetLocalPlayer(PokerPlayer player)
        {
            _player = player;
        }

        public void OnCall()
        {
            _player.photonView.RPC("Call", RpcTarget.All);
        }

        [PunRPC]
        public void SetCurrentPlayer(int playerNumber)
        {
            _currentPlayer = playerNumber;

            if (PhotonNetwork.LocalPlayer.ActorNumber == _currentPlayer)
            {
                _controlPanel.SetActivePlayer(true);
            }
            else 
            {
                _controlPanel.SetActivePlayer(false);
            }
        }
    }
}