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
        private CardSlot[] _flopCardSlots;

        [SerializeField]
        private CardSlot _turnCardSlot;

        [SerializeField]
        private CardSlot _riverCardSlot;

        [SerializeField]
        private CardDealer _cardDealer;

        private int _currentPlayer;

        private PokerPlayer _player;

        private void Awake()
        {
            Instance = this;
            PhotonPeer.RegisterType(typeof(PokerAction), (byte)'A', PokerAction.Serialize, PokerAction.Deserialize);
        }

        public void Start()
        {
            SpawnPlayer();

            _cardDealer.StartInitialDealing(() => {
                Hashtable props = new Hashtable
                {
                    {PokerGame.PLAYER_DEALT_INITIAL_CARDS, true}
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            });

            if (PhotonNetwork.IsMasterClient)
            {
                InitEngine();
                UpdatePlayersScore();
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (changedProps.ContainsKey(PokerGame.PLAYER_DEALT_INITIAL_CARDS) && CheckPlayersStatus(PokerGame.PLAYER_DEALT_INITIAL_CARDS))
                {
                    string[] holeCards = new string[_history.HoleCards.Length];
                    for (int i = 0; i < _history.HoleCards.Length; i++)
                    {
                        holeCards[i] = HoldemHand.Hand.MaskToString(_history.HoleCards[i]);
                    }

                    photonView.RPC("DealHoleCards", RpcTarget.All, holeCards);
                    UpdateNextPlayer();
                }
            }
        }

        [PunRPC]
        public void DealHoleCards(string[] holeCards)
        {
            string[] cards = holeCards[PhotonNetwork.LocalPlayer.ActorNumber - 1].Split(new char[] { ' ' });
            Card card1 = _player.HoleCardSlots[0].TopCard();
            Card card2 = _player.HoleCardSlots[1].TopCard();
            card1.FaceValue = cards[0];
            card2.FaceValue = cards[1];
            card1.Toggle();
            card2.Toggle();
        }

        [PunRPC]
        public void DealFlopCards(string flopCards)
        {
            string[] cards = flopCards.Split(new char[] { ' ' });
            
            for (int i = 0; i < cards.Length; i++)
            {
                Card card = _flopCardSlots[i].TopCard();
                card.FaceValue = cards[i];
                card.Toggle();
            }
        }

        [PunRPC]
        public void DealTurnCard(string turnCard)
        {
            Card card = _turnCardSlot.TopCard();
            card.FaceValue = turnCard;
            card.Toggle();
        }

        [PunRPC]
        public void DealRiverCard(string riverCard)
        {
            Card card = _riverCardSlot.TopCard();
            card.FaceValue = riverCard;
            card.Toggle();
        }

        [PunRPC]
        public void ShowOtherHoleCards()
        {
            foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                PokerPlayer pp = player.GetComponent<PokerPlayer>();

                if (!pp.photonView.IsMine)
                {
                    Card card1 = pp.HoleCardSlots[0].TopCard();
                    Card card2 = pp.HoleCardSlots[1].TopCard();
                    card1.Toggle();
                    card2.Toggle();
                }
            }
        }

        private bool CheckPlayersStatus(string statusKey)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }
            
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object status;
                
                if (p.CustomProperties.TryGetValue(statusKey, out status))
                {
                    if (!(bool) status)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        private void InitEngine()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

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
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
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
                _player = PhotonNetwork.Instantiate("Player", transform.position, transform.rotation).GetComponent<PokerPlayer>();
            }
        }

        private void UpdateNextPlayer()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

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

            Round round = _engine.Bet(actionType, amount);
            
            switch (round)
            {
                case Round.Over:
                case Round.River:
                    photonView.RPC("ShowOtherHoleCards", RpcTarget.All);
                    photonView.RPC("GameOver", RpcTarget.All, _history.ToString());
                    break;

                case Round.Preflop:
                    photonView.RPC("DealFlopCards", RpcTarget.All, HoldemHand.Hand.MaskToString(_history.Flop));
                    UpdateNextPlayer();
                    break;

                case Round.Flop:
                     photonView.RPC("DealTurnCard", RpcTarget.All, HoldemHand.Hand.MaskToString(_history.Turn));
                    UpdateNextPlayer();
                    break;
                
                case Round.Turn: 
                    photonView.RPC("DealRiverCard", RpcTarget.All, HoldemHand.Hand.MaskToString(_history.River));
                    UpdateNextPlayer();
                    break;

                default:
                    UpdateNextPlayer();
                    break;
            }

            UpdatePlayersScore();
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

        [PunRPC]
        public void GameOver(string history)
        {
            _player.GameOver(history);
        }
    }
}