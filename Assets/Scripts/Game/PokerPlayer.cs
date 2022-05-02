using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Action = HoldemEngine.Action;

namespace PokerBattleField
{
    public class PokerPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [SerializeField]
        private Transform _cameraPosition;

        [SerializeField]
        private CardSlot[] _holeCardSlots;
        
        [SerializeField]
        public PokerButtonSlot _buttonSlot;

        private int _id = 0;

        private GameManager _gameManager;

        private ControlPanel _controlPanel;

        private List<Action> _ableActions;

        private Dictionary<Action.ActionTypes, double> _amountMap;


        public CardSlot[] HoleCardSlots
        {
            get { return _holeCardSlots; }
        }

        public PokerButtonSlot ButtonSlot { get { return _buttonSlot; } }

        public int ID { get { return _id; } }
        


        public void Awake()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _controlPanel = GameObject.Find("ControlPanel").GetComponent<ControlPanel>();

            _amountMap = new Dictionary<Action.ActionTypes, double>();

            foreach(Action.ActionTypes actionType in Enum.GetValues(typeof(Action.ActionTypes)))
            {
                _amountMap[actionType] = 0;
            }
        }

        public void Start()
        {
            if (photonView.IsMine)
            {
                _controlPanel.CallButton.onClick.AddListener(() => {
                    double amount = _amountMap[Action.ActionTypes.Call];
                    photonView.RPC("ThrowAction", RpcTarget.All, Action.ActionTypes.Call, amount);
                });

                _controlPanel.BetButton.onClick.AddListener(() => {
                    double amount = _amountMap[Action.ActionTypes.Bet];
                    photonView.RPC("ThrowAction", RpcTarget.All, Action.ActionTypes.Bet, amount);
                });

                _controlPanel.RaiseButton.onClick.AddListener(() => {
                    double amount = _amountMap[Action.ActionTypes.Raise];
                    photonView.RPC("ThrowAction", RpcTarget.All, Action.ActionTypes.Raise, amount);
                });

                _controlPanel.CheckButton.onClick.AddListener(() => {
                    photonView.RPC("ThrowAction", RpcTarget.All, Action.ActionTypes.Check, .0d);
                });

                _controlPanel.FoldButton.onClick.AddListener(() => {
                    photonView.RPC("ThrowAction", RpcTarget.All, Action.ActionTypes.Fold, .0d);
                });
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            _id = (int) info.Sender.ActorNumber - 1;

            // Set player parent hierachy for keeping local world
            gameObject.transform.SetParent(GameObject.Find("PlayerSlots").transform);

            if (photonView.IsMine)
            {
                // Move camera to player view position
                GameObject mainCamera = GameObject.FindWithTag("MainCamera");
                mainCamera.transform.SetPositionAndRotation(_cameraPosition.transform.position, _cameraPosition.transform.rotation);

                Hashtable props = new Hashtable
                {
                    {PokerGame.PLAYER_INSTANTIATED_CHARACTOR, true}
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
        }

        public void SetActive(bool activated)
        {
            _controlPanel.SetActive(activated);
        }

        public void SetAbleActions(List<Action> actions)
        {
            _ableActions = actions;
            _controlPanel.SetAbleActions(actions);

            foreach(Action action in actions)
            {
                _amountMap[action.ActionType] = action.Amount;
            }
        }

        [PunRPC]
        public void ThrowAction(Action.ActionTypes actionType, double amount)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _gameManager.DispatchAction(actionType, amount);
            }
        }

    }
}