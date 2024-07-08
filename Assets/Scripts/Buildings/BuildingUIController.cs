using System;
using System.Collections;
using System.Collections.Generic;
using Lucky.Collections;
using Lucky.Interactive;
using Lucky.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIController : MonoBehaviour
{
    public TMP_Text text;
    public Image outline;
    public LineRenderer ghostLine;
    private bool hasStartGhostLine;
    public DefaultDict<Building, LineRenderer> buildingToLine = new(() => null);


    private void Start()
    {
        ghostLine = LineManager.instance.CreateLine();
        ghostLine.enabled = false;
    }

    private void Update()
    {
        if (hasStartGhostLine)
        {
            ghostLine.SetPosition(0, transform.position);
            ghostLine.SetPosition(1, GameCursor.MouseWorldPos);
        }
    }

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

    public void StartGhostLine()
    {
        if (ghostLine)
            ghostLine.enabled = true;
        hasStartGhostLine = true;
        ghostLine.SetPosition(0, transform.position);
        ghostLine.SetPosition(1, GameCursor.MouseWorldPos);
    }

    public void EndGhostLine()
    {
        if (ghostLine)
            ghostLine.enabled = false;
        hasStartGhostLine = false;
    }

    public void ConnectWith(Building building)
    {
        var line = LineManager.instance.CreateLine();
        buildingToLine[building] = line;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, building.transform.position);
    }

    public void BreakLine(Building b1, Building b2)
    {
        LineRenderer line = null;
        line = b1.buildingUIController.buildingToLine[b2];
        b1.buildingUIController.buildingToLine[b2] = null;
        if (line)
            Destroy(line.gameObject);
        line = b2.buildingUIController.buildingToLine[b1];
        b2.buildingUIController.buildingToLine[b1] = null;
        if (line)
            Destroy(line.gameObject);
    }
}