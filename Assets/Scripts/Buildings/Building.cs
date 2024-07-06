using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Lucky.Interactive;
using Lucky.Managers;
using UnityEngine;
using UnityEngine.UI;

public class Building : InteractableUI
{
    public List<Building> connectedBuildings = new();
    public int maxSoldiers = 20;
    public int _currentSoldiers = 0;

    public int CurrentSoldiers
    {
        get => _currentSoldiers;
        set
        {
            _currentSoldiers = value;
            buildingUIController.UpdateUI(_currentSoldiers, maxSoldiers);
        }
    }

    public int possibleSoldiers => CurrentSoldiers + ComingSoldiers;

    private int _comingSoldiers;

    public int ComingSoldiers
    {
        get => _comingSoldiers;
        set
        {
            _comingSoldiers = value;
            buildingUIController.UpdateUI(CurrentSoldiers, maxSoldiers);
        }
    }

    [Range(0, 1)] public float transferRate = 0.5f;

    public BuildingUIController buildingUIController;
    private Soldier soldierPrefab;

    protected override void Awake()
    {
        base.Awake();
        buildingUIController = GetComponent<BuildingUIController>();
        buildingUIController.UpdateUI(CurrentSoldiers, maxSoldiers);

        soldierPrefab = Resources.Load<Soldier>("Prefabs/Soldier");
    }

    private void Start()
    {
        BuildingsManager.instance.Register(this);
    }

    protected virtual void Update()
    {
        TryBalance();
    }

    private void TryBalance()
    {
        var leastSoldiers = connectedBuildings.Count > 0 ? connectedBuildings.Min(building => building.possibleSoldiers) : int.MaxValue;
        if (leastSoldiers < CurrentSoldiers && CurrentSoldiers > 0)
        {
            var building = connectedBuildings.Find(building => building.possibleSoldiers == leastSoldiers);
            TryTransferTo(building, 1);
        }
    }


    protected override void OnCursorReleaseInBounds()
    {
        base.OnCursorReleaseInBounds();
        BuildingsManager.instance.OnBuildingSelected(this);
    }

    public void TryTransferTo(Building toBuilding, int number = -1)
    {
        // 这里到时候判断一下，如果之前已经连过线了，那就把线断掉
        // if (connectedBuilding == toBuilding)
        // 当前能送过去的个数，对方能容纳的个数
        int soldiers;
        if (number == -1)
            soldiers = Mathf.Min((int)(CurrentSoldiers * transferRate), toBuilding.maxSoldiers - toBuilding.possibleSoldiers);
        else
            soldiers = Mathf.Min(number, CurrentSoldiers, toBuilding.maxSoldiers - toBuilding.possibleSoldiers);
        toBuilding.ComingSoldiers += soldiers;
        CurrentSoldiers -= soldiers;
        for (int i = 0; i < soldiers; i++)
        {
            var soldier = Instantiate(soldierPrefab, BuildingsManager.instance.soldierContainer);
            soldier.InitPos(transform.position, 5);
            soldier.targetBuilding = toBuilding;
        }
    }

    protected override void OnCursorPress()
    {
        base.OnCursorPress();
        buildingUIController.StartGhostLine();
    }

    protected override void OnCursorRelease()
    {
        base.OnCursorRelease();
        buildingUIController.EndGhostLine();
        foreach (var coll in Physics2D.OverlapPointAll(GameCursor.MouseWorldPos))
        {
            var building = coll.GetComponent<Building>();
            if (building != null && building != this && !connectedBuildings.Contains(building))
            {
                TryConnectBuilding(building);
                return;
            }
        }
    }

    private void TryConnectBuilding(Building building)
    {
        connectedBuildings.Add(building);
        building.connectedBuildings.Add(this);
        buildingUIController.ConnectWith(building);
    }
}