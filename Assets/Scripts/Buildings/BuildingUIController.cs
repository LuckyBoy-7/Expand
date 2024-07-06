using System.Collections;
using System.Collections.Generic;
using Lucky.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIController : MonoBehaviour
{
    public TMP_Text text;
    public Image outline;

    public void UpdateUI(int curSoldiers, int maxSoldiers)
    {
        text.text = $"{curSoldiers}/{maxSoldiers}";
    }

    public void Selected()
    {
        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
    }
}