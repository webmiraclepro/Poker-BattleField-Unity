using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

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

    public void SetActivePlayer(bool activated)
    {
        _callButton.gameObject.SetActive(activated);
        _betButton.gameObject.SetActive(activated);
        _raiseButton.gameObject.SetActive(activated);
        _foldButton.gameObject.SetActive(activated);
    }
}
