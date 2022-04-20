using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Photon.Pun.Poker;
using Photon.Pun;

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

    public void SetActive(bool activated)
    {
        _backgroundImage.SetActive(activated);
        SetActiveButtons(activated);
    }

    private void SetActiveButtons(bool activated)
    {
        _callButton.gameObject.SetActive(activated);
        _betButton.gameObject.SetActive(activated);
        _raiseButton.gameObject.SetActive(activated);
        _foldButton.gameObject.SetActive(activated);
        _checkButton.gameObject.SetActive(activated);
    }

    public void SetAbleActions(List<HoldemEngine.Action> actions)
    {
        SetActiveButtons(false);

        foreach(HoldemEngine.Action action in actions)
        {
            switch (action.ActionType)
            {
                case HoldemEngine.Action.ActionTypes.Fold:
                    _foldButton.gameObject.SetActive(true);
                    break;
                
                case HoldemEngine.Action.ActionTypes.Check:
                    _checkButton.gameObject.SetActive(true);
                    break;

                case HoldemEngine.Action.ActionTypes.Call:
                    _callButton.gameObject.SetActive(true);
                    _callButton.GetComponentInChildren<Text>().text = "Call ($" + action.Amount + ")";
                    break;
                
                case HoldemEngine.Action.ActionTypes.Bet:
                    _betButton.gameObject.SetActive(true);
                    _betButton.GetComponentInChildren<Text>().text = "Bet ($" + action.Amount + ")";
                    break;
                
                case HoldemEngine.Action.ActionTypes.Raise:
                    _raiseButton.gameObject.SetActive(true);
                    _raiseButton.GetComponentInChildren<Text>().text = "Raise ($" + action.Amount + ")";
                    break;
                
                default:
                    break;
            }
        }
    }
}
