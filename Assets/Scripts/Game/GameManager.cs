using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using HoldemEngine;
using PokerAction = HoldemEngine.Action;

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
            
            foreach (Player p  in PhotonNetwork.PlayerList)
            {
                _seats[p.ActorNumber - 1] = new Seat(p.ActorNumber, p.NickName, 1000);
            }
            
            // It is possible to start game if there are at least two players
            if (_seats.Length > 1) 
            {
                _history = new HandHistory(_seats, _handNumber, _button, _blinds, 0, BettingStructure.Limit); 
                _engine = new HandEngine(_history);
            }
        }

        private void StartGame()
        {
            int playerIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (playerIdx >= 0 && playerIdx < _playerSlots.Length)
            {
                Transform transform = _playerSlots[playerIdx];
                PhotonNetwork.Instantiate("Player", transform.position, transform.rotation);
                
                // Set local player
                _player = GameObject.FindWithTag("Player").GetComponent<PokerPlayer>();

                // Initialize current player with master client number
                if (PhotonNetwork.IsMasterClient) 
                {
                    photonView.RPC("SetCurrentPlayer", RpcTarget.All, _engine.PlayerIdx); 
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


        [PunRPC]
        public void DispatchAction(PokerAction.ActionTypes actionType, double amount)
        {
            Debug.Log("Action: " + actionType + ", Amount: " + amount);
            bool gameOver = _engine.Bet(actionType, amount);

            if (gameOver)
            {
                Debug.Log(_history.ToString(true));
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SetCurrentPlayer", RpcTarget.All, _engine.PlayerIdx);
            }
        }

        [PunRPC]
        public void SetCurrentPlayer(int playerIdx)
        {
            _currentPlayer = _seats[playerIdx].SeatNumber;

            if (PhotonNetwork.LocalPlayer.ActorNumber == _currentPlayer)
            {
                _player.SetActive(true);
                _player.SetAbleActions(_engine.GetAbleActions());
            }
            else 
            {
                _player.SetActive(false);
            }
        }
    }
}