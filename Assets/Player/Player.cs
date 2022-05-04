using RTS;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public string username;
    public bool human;
    public HUD hud;
    public WorldObject SelectedObject { get; set; }
    public int startMoney, startMoneyLimit, startPower, startPowerLimit;
    public int StartWood, StartWoodLimit;
    public int StartFood, StartFoodLimit;
    private Dictionary<ResourceType, int> resources, resourceLimits;
    public Material notAllowedMaterial, allowedMaterial;
    private Building tempBuilding;
    private Unit tempCreator;
    private bool findingPlacement = false;
    // Start is called before the first frame update
    private Dictionary<ResourceType, int> InitResourceList()
    {
        Dictionary<ResourceType, int> list = new Dictionary<ResourceType, int>();
        list.Add(ResourceType.Money, 0);
        list.Add(ResourceType.Power, 0);
        list.Add(ResourceType.Food, 0);
        list.Add(ResourceType.Wood, 0);
        return list;
    }
    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
    }

    public void IncrementResourceLimit(ResourceType type, int amount)
    {
        resourceLimits[type] += amount;
    }
    private void AddStartResourceLimits()
    {
        IncrementResourceLimit(ResourceType.Money, startMoneyLimit);
        IncrementResourceLimit(ResourceType.Power, startPowerLimit);
        IncrementResourceLimit(ResourceType.Wood, StartWoodLimit);
        IncrementResourceLimit(ResourceType.Food, StartFoodLimit);

    }

    private void AddStartResources()
    {
        AddResource(ResourceType.Money, startMoney);
        AddResource(ResourceType.Power, startPower);
        AddResource(ResourceType.Wood, StartWood);
        AddResource(ResourceType.Food, StartFood);

    }
    void Awake()
    {
        resources = InitResourceList();
        resourceLimits = InitResourceList();
    }
    void Start()
    {
        hud = GetComponentInChildren<HUD>();
        AddStartResourceLimits();
        AddStartResources();
    }

    // Update is called once per frame
    void Update()
    {
        if (human)
        {
            hud.SetResourceValues(resources, resourceLimits);
        }
    }

    public void AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation, Building creator)
    {
        Units units = GetComponentInChildren<Units>();
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
        newUnit.transform.parent = units.transform;
        Unit unitObject = newUnit.GetComponent<Unit>();
        if (unitObject && spawnPoint != rallyPoint) unitObject.StartMove(rallyPoint);
        if (unitObject)
        {
            unitObject.SetBuilding(creator);
            if (spawnPoint != rallyPoint) unitObject.StartMove(rallyPoint);
        }
    }

    public void CreateBuilding(string buildingName, Vector3 buildPoint, Unit creator, Rect playingArea)
    {
        GameObject newBuilding = (GameObject)Instantiate(ResourceManager.GetBuilding(buildingName), buildPoint, new Quaternion());
        tempBuilding = newBuilding.GetComponent<Building>();
        if (tempBuilding)
        {
            tempCreator = creator;
            findingPlacement = true;
            tempBuilding.SetTransparentMaterial(notAllowedMaterial, true);
            tempBuilding.SetColliders(false);
            tempBuilding.SetPlayingArea(playingArea);
        }
        else Destroy(newBuilding);
    }

    public bool IsFindingBuildingLocation()
    {
        return findingPlacement;
    }

    public void FindBuildingLocation()
    {
        Vector3 newLocation = WorkManager.FindHitPoint();
        newLocation.y = 0;
        tempBuilding.transform.position = newLocation;
    }
}
