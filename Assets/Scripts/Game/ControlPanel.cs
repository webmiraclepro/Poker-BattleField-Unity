using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Photon.Pun;
using Action = HoldemEngine.Action;

namespace PokerBattleField
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _callButton;

        [SerializeField]
        private Button _betButton;

        [SerializeField]
        private Button _raiseButton;

        [SerializeField]
        private Button _foldButton;

        [SerializeField]
        private Button _checkButton;

        [SerializeField]
        private GameObject _backgroundImage;

        [SerializeField]
        private GameObject _progressImage;

        private float _timer = -1;
        private Dictionary<Action.ActionTypes, bool> _ableActions;

        public Button CallButton 
        {
            get { return _callButton; }
        }

        public Button BetButton 
        {
            get { return _betButton; }
        }

        public Button RaiseButton 
        {
            get { return _raiseButton; }
        }
        
        public Button FoldButton 
        {
            get { return _foldButton; }
        }
        
        public Button CheckButton 
        {
            get { return _checkButton; }
        }

        private void Awake()
        {
            _ableActions = new Dictionary<Action.ActionTypes, bool>();

            _foldButton.onClick.AddListener(() => {
                _timer = -1;
            });
            _checkButton.onClick.AddListener(() => {
                _timer = -1;
            });
            _callButton.onClick.AddListener(() => {
                _timer = -1;
            });
            _betButton.onClick.AddListener(() => {
                _timer = -1;
            });
            _raiseButton.onClick.AddListener(() => {
                _timer = -1;
            });
        }

        public void SetActive(bool activated)
        {
            _backgroundImage.SetActive(activated);
            _progressImage.SetActive(activated);
            SetActiveButtons(activated);

            if (activated)
            {
                _timer = 0;
            }
        }

        private void Update()
        {
            if (_timer < 0)
            {
                return;
            }

            _timer += Time.deltaTime;
            _progressImage.GetComponent<RectTransform>().localScale = new Vector3(_timer / 10, 1f, 1f);

            if (_timer >= 10)
            {
                SetActive(false);
                _timer = -1;

                if (_ableActions[Action.ActionTypes.Fold])
                {
                    _foldButton.onClick.Invoke();
                }
                else if (_ableActions[Action.ActionTypes.Check])
                {
                    _checkButton.onClick.Invoke();
                }
                else if (_ableActions[Action.ActionTypes.Call])
                {
                    _callButton.onClick.Invoke();
                }
                else if (_ableActions[Action.ActionTypes.Bet])
                {
                    _betButton.onClick.Invoke();
                }
                else
                {
                    _raiseButton.onClick.Invoke();
                }
            }
        }

        private void SetActiveButtons(bool activated)
        {
            _callButton.gameObject.SetActive(activated);
            _betButton.gameObject.SetActive(activated);
            _raiseButton.gameObject.SetActive(activated);
            _foldButton.gameObject.SetActive(activated);
            _checkButton.gameObject.SetActive(activated);
        }

        public void SetAbleActions(List<Action> actions)
        {
            SetActiveButtons(false);
            
            foreach(Action.ActionTypes actionType in Enum.GetValues(typeof(Action.ActionTypes)))
            {
                _ableActions[actionType] = false;
            }
            
            foreach(Action action in actions)
            {
                _ableActions[action.ActionType] = true;

                switch (action.ActionType)
                {
                    case Action.ActionTypes.Fold:
                        _foldButton.gameObject.SetActive(true);
                        break;
                    
                    case Action.ActionTypes.Check:
                        _checkButton.gameObject.SetActive(true);
                        break;

                    case Action.ActionTypes.Call:
                        _callButton.gameObject.SetActive(true);
                        _callButton.GetComponentInChildren<Text>().text = "Call ($" + action.Amount + ")";
                        break;
                    
                    case Action.ActionTypes.Bet:
                        _betButton.gameObject.SetActive(true);
                        _betButton.GetComponentInChildren<Text>().text = "Bet ($" + action.Amount + ")";
                        break;
                    
                    case Action.ActionTypes.Raise:
                        _raiseButton.gameObject.SetActive(true);
                        _raiseButton.GetComponentInChildren<Text>().text = "Raise ($" + action.Amount + ")";
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }
}
