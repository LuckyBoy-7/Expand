using System.Collections.Generic;
using Lucky.Managers;
using UnityEngine;

public class BuildingsManager : Singleton<BuildingsManager>
{
    public Building selectedBuilding;
    public Transform soldierContainer;
    public List<Building> buildings = new();

    public void Register(Building building) => buildings.Add(building);

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
}