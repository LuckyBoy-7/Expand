using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using DG.Tweening;
using Lucky.Extensions;
using Lucky.Managers;
using Lucky.Loader;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Ease = Lucky.Utilities.Ease;
using Random = UnityEngine.Random;

public class BuildingsManager : Singleton<BuildingsManager>
{
    public float soldierMoveSpeedMultiplier = 50;
    public float produceSpeedMultiplier = 2;
    public Transform buildingContainer;
    public Transform soldierContainer;
    private Building selectedBuilding;
    public List<Building> buildings = new();
    public List<Building> buildingsToSpawn = new();
    public float spawnBuildingDuration = 30;
    public float minBuildingDist = 300;
    public float maxBuildingDist => 600 + Mathf.Log(4 * Time.time + 1) / 2;

    protected override void Awake()
    {
        base.Awake();


        buildingsToSpawn = new List<Building>
        {
            Res.circleBuildingPrefab,
            Res.triangleBuildingPrefab,
            Res.triangleBuildingPrefab,
            Res.triangleBuildingPrefab,
            Res.triangleBuildingPrefab,
            Res.triangleBuildingPrefab,
        };
    }

    private void Start()
    {
        SpawnBuilding(Res.circleBuildingPrefab, Vector3.zero).CurrentSoldiers = 2;
        SpawnBuilding(Res.triangleBuildingPrefab);
        SpawnBuilding(Res.triangleBuildingPrefab);
        this.CreateFuncTimer(SpawnOneBuildingRandomly, () => spawnBuildingDuration);
    }

    private void SpawnOneBuildingRandomly()
    {
        // 抽位置
        if (!TryGetValidPosToSpawnBuilding(out var possiblePos))
            return;

        // 抽建筑
        Building buildingPrefab = buildingsToSpawn.Choice();
        SpawnBuilding(buildingPrefab, possiblePos);
    }

    private bool TryGetValidPosToSpawnBuilding(out Vector3 possiblePos)
    {
        possiblePos = Vector3.zero;

        if (buildings.Count == 0)
            return false;
        float rate = Ease.SineEaseOut(Mathf.Clamp(buildings.Count / 50f, 0, 1));
        float radius = minBuildingDist + rate * maxBuildingDist;

        // 如果buildings长度为零理论上就是直接结束了，所以应该问题不大
        Building pivotBuilding = buildings.Choice();
        Vector3 pos = pivotBuilding.transform.position + (Vector3)Random.insideUnitCircle * radius;
        int tryCount = 0;
        while (buildings.Any(build => (build.transform.position - pos).magnitude < minBuildingDist))
        {
            pos = pivotBuilding.transform.position + (Vector3)Random.insideUnitCircle * radius;
            if (tryCount++ > maxBuildingDist) // 这都随机不出来，就退出
                return false;
        }

        possiblePos = pos;
        return true;
    }

    public Building SpawnBuilding(Building prefab, Vector3 pos)
    {
        Building building = Instantiate(prefab, buildingContainer);
        building.transform.position = pos;
        buildings.Add(building);
        return building;
    }

    public Building SpawnBuilding(Building prefab)
    {
        // 抽位置
        if (!TryGetValidPosToSpawnBuilding(out var possiblePos))
            return null;
        return SpawnBuilding(prefab, possiblePos);
    }

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
        if (buildings.Count == 0 || buildings.All(b => b.possibleSoldiers == 0))
        {
            float duration = 1f;
            EventManager.instance.Broadcast("Gameover", duration);
            SceneManager.LoadScene("EndScene");
            Time.timeScale = 1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightAlt))
        {
            buildings.Clear();
            CheckGameState();
        }
    }
}