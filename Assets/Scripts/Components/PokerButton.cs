using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerButton : MonoBehaviour
{
    public PokerButtonSlot ParentSlot { get; set; }

    private void Awake()
    {
        SetVisible(false);
    }

    public void SetVisible(bool visibility)
    {
        foreach(MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = visibility;
        }
    }
}
