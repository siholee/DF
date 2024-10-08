using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject MainTower; // MainTower object
    public Button SummonButton; // Button to summon heroes
    public GameObject HeroPrefab; // Prefab for heroes
    public int ROUND;
    public int RESOURCE1;
    public int RESOURCE2;
    public List<Hero> HERO_LIST;
    public List<Hero> HERO_ON_DECK;
    public List<Hero> SELECT_HERO_LIST;

    void Start()
    {

    }

    void Update()
    {

    }

   void SummonHero()
   {
    if (MainTower != null)
    {
        // MainTower의 TOWER_LEVEL을 가져옴
        int LOTTERY_LEVEL = MainTower.GetComponent<tower>().TOWER_LEVEL;

        // List<Hero> SELECT_HERO_LIST 초기화
        List<Hero> SELECT_HERO_LIST = new List<Hero>();

        // 확률에 따라 RANK 결정
        for (int i = 0; i < 5; i++)
        {
            float randomValue = Random.value; // 0.0f에서 1.0f 사이의 랜덤 값
            int RANK = 1;

            switch (LOTTERY_LEVEL)
            {
                case 1: // 최대 30
                    if (randomValue < 1.0f) RANK = 1;
                    break;
                case 2: // 최대 25
                    if (randomValue < 1.0f) RANK = 1;
                    else if (randomValue < 1.25f) RANK = 2;
                    break;
                case 3: // 최대 18
                    if (randomValue < 0.75f) RANK = 1;
                    else if (randomValue < 1.0f) RANK = 2;
                    break;
                case 4: // 최대 10
                    if (randomValue < 0.55f) RANK = 1;
                    else if (randomValue < 0.8f) RANK = 2;
                    else if (randomValue < 0.95f) RANK = 3;
                    break;
                case 5: // 최대 9
                    if (randomValue < 0.45f) RANK = 1;
                    else if (randomValue < 0.7f) RANK = 2;
                    else RANK = 3;
                    break;
                default:
                    RANK = 1;
                    break;
            }

            // HERO_LIST에서 HERO_RANK가 RANK와 동일한 영웅들을 필터링
            List<Hero> filteredHeroes = HERO_LIST.FindAll(hero => hero.HERO_RANK == RANK);

            // 필터링된 리스트가 비어 있지 않으면 랜덤하게 하나 선택하여 SELECT_HERO_LIST에 추가
            if (filteredHeroes.Count > 0)
            {
                Hero selectedHero = filteredHeroes[Random.Range(0, filteredHeroes.Count)];
                SELECT_HERO_LIST.Add(selectedHero);

                // 로그 출력 (디버깅용)
                Debug.Log($"소환된 영웅: {selectedHero.NAME}, RANK: {selectedHero.HERO_RANK}");
            }
            else
            {
                Debug.LogWarning($"RANK {RANK}에 해당하는 영웅이 없습니다.");
            }
        } } else {
                Debug.LogWarning("MainTower가 설정되지 않았습니다.");
                }
            }
}
