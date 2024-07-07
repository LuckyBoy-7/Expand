using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using DG.Tweening;
using Lucky.Extensions;
using Lucky.Managers;
using UnityEngine;
using Ease = Lucky.Utilities.Ease;
using Random = UnityEngine.Random;

public class BuildingsManager : Singleton<BuildingsManager>
{
    public Transform buildingContainer;
    public Transform soldierContainer;
    private Building selectedBuilding;
    public List<Building> buildings = new();
    public List<Building> buildingsToSpawn = new();
    public float spawnBuildingDuration = 1f;
    public float minBuildingDist = 300;
    public float maxBuildingDist = 1000;

    protected override void Awake()
    {
        base.Awake();
        buildingsToSpawn = new List<Building>
        {
            Resources.Load<Building>("Prefabs/CircleBuilding"),
            Resources.Load<Building>("Prefabs/CircleBuilding"),
            Resources.Load<Building>("Prefabs/CircleBuilding"),
            Resources.Load<Building>("Prefabs/SquareBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
            Resources.Load<Building>("Prefabs/TriangleBuilding"),
        };
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnOneBuildingRandomly), spawnBuildingDuration, spawnBuildingDuration);
    }

    private void SpawnOneBuildingRandomly()
    {
        if (buildings.Count == 0)
            return;
        float rate = Ease.SineEaseOut(Mathf.Clamp(buildings.Count / 50f, 0, 1));
        float radius = minBuildingDist + rate * maxBuildingDist;

        // 如果buildings长度为零理论上就是直接结束了，所以应该问题不大
        Building pivotBuilding = buildings.Choice();
        Vector3 possiblePos = pivotBuilding.transform.position + (Vector3)Random.insideUnitCircle * radius;
        int tryCount = 0;
        while (buildings.Any(build => (build.transform.position - possiblePos).magnitude < minBuildingDist))
        {
            possiblePos = pivotBuilding.transform.position + (Vector3)Random.insideUnitCircle * radius;
            if (tryCount++ > maxBuildingDist) // 这都随机不出来，就退出
                return;
        }

        Building buildingPrefab = buildingsToSpawn.Choice();
        Instantiate(buildingPrefab, buildingContainer).transform.position = possiblePos;
    }

    public void Register(Building building) => buildings.Add(building);
    public void UnRegister(Building building) => buildings.Remove(building);

    public void OnBuildingSelected(Building building)
    {
        if (selectedBuilding == null)
        {
            selectedBuilding = building;
            selectedBuilding.buildingUIController.Selected();
        }
        else if (building == selectedBuilding)
        {
            selectedBuilding.buildingUIController.Deselect();
            selectedBuilding = null;
        }
        else
        {
            selectedBuilding.buildingUIController.Deselect();
            selectedBuilding.TryTransferTo(building);
            LineManager.instance.ShowOneWayLine(selectedBuilding.transform.position, building.transform.position, 0.5f);
            selectedBuilding = null;
        }
    }

    public void CheckGameState()
    {
        if (buildings.Count == 0)
        {
            float duration = 1f;
            EventManager.instance.Broadcast("Gameover", duration);
            print("Gameover");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buildings.Clear();
            CheckGameState();
        }
    }
}