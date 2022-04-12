using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using HoldemEngine;

namespace Photon.Pun.Poker 
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance = null;

        private HandEngine _engine;
        private HandHistory _history;
        private Seat[] _seats;
        private ulong _handNumber = 0;
        private uint _button = 0;
        
        [SerializeField]
        private double[] _blinds = new double[] { 10, 20 };

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

            Initialize();    
            
            StartGame();
        }
        
        private void Initialize()
        {
            _seats = new Seat[PhotonNetwork.PlayerList.Length];
            
            for (int idx = 0; idx < PhotonNetwork.PlayerList.Length; idx++)
            {
                Player p = PhotonNetwork.PlayerList[idx];
                _seats[idx] = new Seat(idx + 1, p.NickName, 1000);
            }
            
            _history = new HandHistory(_seats, _handNumber, _button, _blinds, 0, BettingStructure.Limit); 
            _engine = new HandEngine(_history);
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
            SceneManager.LoadScene("LobbyScene");
        }

        public override void OnLeftRoom()
        {
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