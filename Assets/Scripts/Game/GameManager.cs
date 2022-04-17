using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using PhotonPeer = ExitGames.Client.Photon.PhotonPeer;
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
        private readonly int _initChips = 1000;

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
            PhotonPeer.RegisterType(typeof(PokerAction), (byte)'A', PokerAction.Serialize, PokerAction.Deserialize);
        }

        public void Start()
        {
            SpawnPlayer();

            if (PhotonNetwork.IsMasterClient)
            {
                InitEngine();
                UpdatePlayersScore();
                UpdateNextPlayer();                
            }
        }
        
        private void InitEngine()
        {
            _seats = new Seat[PhotonNetwork.PlayerList.Length];
            foreach (Player p  in PhotonNetwork.PlayerList)
            {
                _seats[p.ActorNumber - 1] = new Seat(p.ActorNumber, p.NickName, _initChips);
            }

            _history = new HandHistory(_seats, _handNumber, _button, _blinds, 0, BettingStructure.Limit); 
            _engine = new HandEngine(_history);
        }

        private void UpdatePlayersScore()
        {
            foreach(Player p in PhotonNetwork.PlayerList)
            {
                p.SetScore((int)_seats[p.ActorNumber - 1].Chips);
            }
        }

        private void SpawnPlayer()
        {
            int playerIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (playerIdx >= 0 && playerIdx < _playerSlots.Length)
            {
                Transform transform = _playerSlots[playerIdx];
                PhotonNetwork.Instantiate("Player", transform.position, transform.rotation);
                
                // Set local player
                _player = GameObject.FindWithTag("Player").GetComponent<PokerPlayer>();
            }
        }

        private void UpdateNextPlayer()
        {
            List<int> actionTypes = new List<int>();
            List<double> amounts = new List<double>();

            foreach (PokerAction action in _engine.GetAbleActions())
            {
                actionTypes.Add((int)action.ActionType);
                amounts.Add(action.Amount);
            }

            photonView.RPC("SetNextPlayer", RpcTarget.All, _engine.PlayerIdx, _engine.GetAbleActions().ToArray());
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadScene(0);
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public void DispatchAction(PokerAction.ActionTypes actionType, double amount)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            bool gameOver = _engine.Bet(actionType, amount);
            
            UpdatePlayersScore();
            UpdateNextPlayer();

            if (gameOver)
            {
                Debug.Log(_history.ToString(true));
                InitEngine();
            }
        }

        [PunRPC]
        public void SetNextPlayer(int playerIdx, Action[] actions)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == playerIdx + 1)
            {
                _player.SetActive(true);
                _player.SetAbleActions(actions.ToList());
            }
            else
            {
                _player.SetActive(false);
            }
        }
    }
}