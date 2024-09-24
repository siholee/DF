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
    public int round;
    public int RESOURCE1;
    public int RESOURCE2;
    public List<Hero> HERO_LIST;
    public List<Hero> HERO_ON_DECK;

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

    void SummonHero()
    {
        if (MainTower != null)
        {
            // MainTower의 TOWER_LEVEL을 가져옴
            int LOTTERY_LEVEL = MainTower.GetComponent<tower>().TOWER_LEVEL;

            // 확률에 따라 RANK 결정 (예시로 3개의 RANK로 나눔: 1, 2, 3)
            float randomValue = Random.value; // 0.0f에서 1.0f 사이의 랜덤 값
            int RANK;

            if (randomValue < 0.5f) // 50% 확률로 RANK 1
            {
                RANK = 1;
            }
            else if (randomValue < 0.8f) // 30% 확률로 RANK 2
            {
                RANK = 2;
            }
            else // 20% 확률로 RANK 3
            {
                RANK = 3;
            }

            // HERO_LIST에서 HERO_RANK가 RANK와 동일한 영웅들을 필터링
            List<Hero> filteredHeroes = HERO_LIST.FindAll(hero => hero.HERO_RANK == RANK);

            // 필터링된 리스트가 비어 있지 않으면 랜덤하게 하나 선택
            if (filteredHeroes.Count > 0)
            {
                Hero selectedHero = filteredHeroes[Random.Range(0, filteredHeroes.Count)];

                // HERO_ON_DECK에 추가
                HERO_ON_DECK.Add(selectedHero);

                // 로그 출력 (디버깅용)
                Debug.Log($"소환된 영웅: {selectedHero.NAME}, RANK: {selectedHero.HERO_RANK}");
            }
            else
            {
                Debug.LogWarning($"RANK {RANK}에 해당하는 영웅이 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("MainTower가 설정되지 않았습니다.");
        }
    }
}
