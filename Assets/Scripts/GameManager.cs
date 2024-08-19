using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject MainTower; // MainTower object
    public Button SummonButton; // Button to summon heroes
    public GameObject HeroPrefab; // Prefab for heroes
    public CircularGrid gridSystem; // Reference to the CircularGrid script
    public CircularBuildingPlacer buildingPlacer; // Reference to the building placer script

    void Start()
    {
        // Hook up the SummonButton click event
        SummonButton.onClick.AddListener(CreateNewHero);

        // Initialize the grid system
        InitializeGridSystem();
    }

    void Update()
    {
        // Add any updates that need to interact with the grid or other game logic
    }

    void InitializeGridSystem()
    {
        if (gridSystem != null)
        {
            gridSystem.GenerateGrid();
        }
    }

    void CreateNewHero()
    {
        // Example interaction: place a building on the grid when summoning a hero
        if (MainTower != null && HeroPrefab != null)
        {
            // Summon hero at the MainTower
            tower towerComponent = MainTower.GetComponent<tower>();

            if (towerComponent != null)
            {
                GameObject newHeroObject = Instantiate(HeroPrefab);
                newHeroObject.transform.SetParent(MainTower.transform);
                towerComponent.heroes.Add(newHeroObject);

                // Optionally, place a building on the grid
                PlaceBuildingOnGrid();
            }
        }
    }

    void PlaceBuildingOnGrid()
    {
        if (buildingPlacer != null)
        {
            // Assume that buildingPlacer will handle the actual placement
            buildingPlacer.PlaceBuilding();
        }
    }
}
