using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

namespace PokerBattleField 
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance = null;

        private HandEngine _engine;
        private Seat[] _seats;

        [SerializeField]
        private int _initChips = 1000;

        [SerializeField]
        private double[] _blinds = new double[] { 10, 20 };

        [SerializeField]
        private Transform[] _playerSlots;

        [SerializeField]
        private CardDealer _cardDealer;

        [SerializeField]
        private GameObject _gameOverPanel;
        
        [SerializeField]
        private PokerButton _bigBlindButton;
        
        [SerializeField]
        private PokerButton _smallBlindButton;
        
        [SerializeField]
        private PokerButton _dealerButton;

        private int _currentPlayer = -1;

        private PokerPlayer _player;

        void Awake()
        {
            Instance = this;
            PhotonPeer.RegisterType(typeof(PokerAction), (byte)'A', PokerAction.Serialize, PokerAction.Deserialize);
        }

        void Start()
        {
            SpawnPlayer();

            if (PhotonNetwork.IsMasterClient)
            {
                InitEngine();
                UpdatePlayersScore();
            }
        }

        private void SpawnPlayer()
        {
            int playerIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (playerIdx >= 0 && playerIdx < _playerSlots.Length)
            {
                Transform transform = _playerSlots[playerIdx];
                GameObject pObj = PhotonNetwork.Instantiate("Player", transform.position, transform.rotation);
                _player = pObj.GetComponent<PokerPlayer>();
            }
        }

        private void InitEngine()
        {
            _seats = new Seat[PhotonNetwork.PlayerList.Length];
            foreach (Player p  in PhotonNetwork.PlayerList)
            {
                _seats[p.ActorNumber - 1] = new Seat(p.ActorNumber, p.NickName, _initChips);
            }

            _engine = new HandEngine(_seats, _blinds, 0, BettingStructure.Limit);
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

        private void InitCardDeck()
        {
            StartCoroutine(_cardDealer.Initialize());
        }

        private void StartRound()
        {
            // StartRound
            _engine.InitRound();

            // Deal player hole cards
            string[] holeCards = new string[_engine.History.HoleCards.Length];
            
            for (int i = 0; i < _engine.History.HoleCards.Length; i++)
            {
                holeCards[i] = HoldemHand.Hand.MaskToString(_engine.History.HoleCards[i]);
            }

            photonView.RPC("DealHoleCards", RpcTarget.All, holeCards);
        }  

        private void InitPokerButtons()
        {
            // Set dealer, small blind and big blind
            photonView.RPC("SetDealerAndBlinds", RpcTarget.All, _engine.BtnIdx, _engine.SbIdx, _engine.BbIdx);
            
            // Let UTG player act at first 
            UpdateNextPlayer();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (PokerGame.CheckPlayersStatus(changedProps, PokerGame.CHARACTER_READY))
            {
                InitCardDeck();
            }
            else if (PokerGame.CheckPlayersStatus(changedProps, PokerGame.CARD_DEALER_READY) && PhotonNetwork.IsMasterClient)
            {
                StartRound();
            }
            else if (PokerGame.CheckPlayersStatus(changedProps, PokerGame.DEALT_HOLE_CARDS) && PhotonNetwork.IsMasterClient)
            {
                InitPokerButtons();
            }
        }


        [PunRPC]
        public void DealHoleCards(string[] holeCards)
        {
            StartCoroutine(_cardDealer.DealHoleCards(holeCards, _DealHoleCards()));
        }

        private IEnumerator _DealHoleCards()
        {
            foreach (CardSlot slot in _player.HoleCardSlots)
            {
                slot.TopCard().Toggle();
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        [PunRPC]
        public void SetDealerAndBlinds(int dealerIdx, int sbIdx, int bbIdx)
        {
            foreach (GameObject pObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                PokerPlayer pp = pObj.GetComponent<PokerPlayer>();

                if (pp.ID == dealerIdx)
                {
                    _dealerButton.ParentSlot = pp.ButtonSlot;
                }
                else if (pp.ID == sbIdx)
                {
                    _smallBlindButton.ParentSlot = pp.ButtonSlot;
                }
                else if (pp.ID == bbIdx)
                {
                    _bigBlindButton.ParentSlot = pp.ButtonSlot;
                }
            }
        }

        [PunRPC]
        public void DealFlopCards(string flopCards)
        {
            StartCoroutine(_cardDealer.DealFlopCards(flopCards.Split(new char[] { ' ' })));
        }

        [PunRPC]
        public void DealTurnCard(string turnCard)
        {
            StartCoroutine(_cardDealer.DealTurnCard(turnCard));
        }

        [PunRPC]
        public void DealRiverCard(string riverCard)
        {
            StartCoroutine(_cardDealer.DealRiverCard(riverCard));
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

        private void UpdateNextPlayer()
        {
            _currentPlayer = _engine.PlayerIdx;
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
            Round round = _engine.Bet(actionType, amount);
            
            switch (round)
            {
                case Round.Over:
                case Round.River:
                    photonView.RPC("ShowOtherHoleCards", RpcTarget.All);
                    photonView.RPC("RoundOver", RpcTarget.All, _engine.History.ToString(true));
                    break;

                case Round.Preflop:
                    photonView.RPC("DealFlopCards", RpcTarget.All, HoldemHand.Hand.MaskToString(_engine.History.Flop));
                    UpdateNextPlayer();
                    break;
    
                case Round.Flop:
                     photonView.RPC("DealTurnCard", RpcTarget.All, HoldemHand.Hand.MaskToString(_engine.History.Turn));
                    UpdateNextPlayer();
                    break;
                
                case Round.Turn: 
                    photonView.RPC("DealRiverCard", RpcTarget.All, HoldemHand.Hand.MaskToString(_engine.History.River));
                    UpdateNextPlayer();
                    break;

                case Round.NextTurn:
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
        public void RoundOver(string history)
        {
            PokerGame.SetPlayerStatus(PokerGame.CARD_DEALER_READY, false);
            PokerGame.SetPlayerStatus(PokerGame.DEALT_HOLE_CARDS, false);

            _player.SetActive(false);
            _gameOverPanel.SetActive(true);
            _gameOverPanel.GetComponent<GameOverPanel>().SetInfoText(history);
        }

        public void ContinueRound()
        {
            _gameOverPanel.SetActive(false);
            InitCardDeck();
        }
    }
}