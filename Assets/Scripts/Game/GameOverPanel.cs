using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private Text _infoText;


    public void SetInfoText(string text)
    {
    	_infoText.text = text;
    }
}
