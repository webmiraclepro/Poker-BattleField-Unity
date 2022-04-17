using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Action = HoldemEngine.Action;

namespace Photon.Pun.Poker
{
    public class PokerPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [SerializeField]
        private Transform _cameraPosition;

        private GameManager _gameManager;

        private ControlPanel _controlPanel;

        private List<Action> _ableActions;

        private Dictionary<Action.ActionTypes, double> _amountMap;


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
                    photonView.RPC("ThrowAction", RpcTarget.All, Action.ActionTypes.Check, 0);
                });

                _controlPanel.FoldButton.onClick.AddListener(() => {
                    photonView.RPC("ThrowAction", RpcTarget.All, Action.ActionTypes.Fold, 0);
                });
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // Set player parent hierachy for keeping local world
            gameObject.transform.SetParent(GameObject.Find("PlayerSlots").transform);

            if (photonView.IsMine)
            {
                // Move camera to player view position
                GameObject mainCamera = GameObject.FindWithTag("MainCamera");
                mainCamera.transform.SetPositionAndRotation(_cameraPosition.transform.position, _cameraPosition.transform.rotation);

                // Set local player with player tag
                gameObject.tag = "Player";
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
            _gameManager.photonView.RPC("DispatchAction", RpcTarget.All, actionType, amount);
        }

    }
}