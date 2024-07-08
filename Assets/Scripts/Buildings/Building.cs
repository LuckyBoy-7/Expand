using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Lucky.Extensions;
using Lucky.Interactive;
using Lucky.Loader;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : InteractableUI
{
    public int maxSoldiers = 20;

    public int MaxSoldiers
    {
        get => maxSoldiers;
        set
        {
            maxSoldiers = value;
            buildingUIController.UpdateUI(CurrentSoldiers, MaxSoldiers);
        }
    }

    protected float soldierMoveSpeed = 2;
    public float soldierMoveReduceRate = 0;
    public float SoldierMoveSpeed => soldierMoveSpeed * (1 - soldierMoveReduceRate) * BuildingsManager.instance.soldierMoveSpeedMultiplier;


    public int currentSoldiers;

    public int CurrentSoldiers
    {
        get => currentSoldiers;
        set
        {
            currentSoldiers = value;
            buildingUIController.UpdateUI(currentSoldiers, MaxSoldiers);
        }
    }

    public List<Building> connectedBuildings = new();

    public int possibleSoldiers => CurrentSoldiers + ComingSoldiers;

    private int comingSoldiers;

    public int ComingSoldiers
    {
        get => comingSoldiers;
        set
        {
            comingSoldiers = value;
            buildingUIController.UpdateUI(CurrentSoldiers, MaxSoldiers);
        }
    }

    [Range(0, 1)] public float transferRate = 0.5f;

    public BuildingUIController buildingUIController;
    public float transferDuration = 0.1f;

    protected virtual void Awake()
    {
        buildingUIController = GetComponent<BuildingUIController>();
        buildingUIController.UpdateUI(CurrentSoldiers, MaxSoldiers);
    }

    protected override void Start()
    {
        base.Start();
        this.CreateFuncTimer(TryBalance, () => transferDuration);
    }

    protected override void Update()
    {
        base.Update();
        if (MaxSoldiers == 0)
        {
            Destroyed();
            return;
        }
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
            soldiers = Mathf.Min((int)(CurrentSoldiers * transferRate), toBuilding.MaxSoldiers - toBuilding.possibleSoldiers);
        else
            soldiers = Mathf.Min(number, CurrentSoldiers, toBuilding.MaxSoldiers - toBuilding.possibleSoldiers);
        toBuilding.ComingSoldiers += soldiers;
        CurrentSoldiers -= soldiers;
        for (int i = 0; i < soldiers; i++)
        {
            var soldier = SoldiersManager.instance.soldierPool.Get();
            soldier.gameObject.SetActive(true);
            soldier.Init(transform.position, 5, soldierMoveSpeed, this, toBuilding);
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
        foreach (var building in BuildingsManager.instance.buildings.Where(b => b.IsPositionInBounds(GameCursor.MouseWorldPos)))
        {
            if (building != this && !connectedBuildings.Contains(building))
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

    public void Destroyed()
    {
        Destroy(gameObject);
        foreach (var connectedBuilding in connectedBuildings.ToList())
            BreakLineAndConnection(connectedBuilding);

        Destroy(buildingUIController.ghostLine.gameObject);
        BuildingsManager.instance.UnRegister(this);
        BuildingsManager.instance.CheckGameState();
    }

    public void BreakLineAndConnection(Building building)
    {
        building.connectedBuildings.Remove(this);
        connectedBuildings.Remove(building);
        buildingUIController.BreakLine(this, building);
    }
}