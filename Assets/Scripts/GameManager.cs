using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json; // JSON 파싱을 위해 사용
using UnityEngine.UI; // UI 관련 네임스페이스 추가

public class GameManager : MonoBehaviour
{
    public GameObject MainTower; // MainTower 오브젝트
    public GameObject BottomMenuBar; // BottomMenuBar 오브젝트를 Inspector에서 드래그하여 연결
    public List<Hero> HERO_LIST = new List<Hero>(); // 전체 영웅 리스트
    public GameObject HERO_PREFAB; // 영웅 프리팹
    public List<Hero> HERO_IN_DECK = new List<Hero>(); // 인스턴스화된 영웅 리스트

    void Start()
    {
        // JSON 파일 경로 설정
        string filePath = Application.dataPath + "/Data/demo_heroes_updated.json";
        if (File.Exists(filePath))
        {
            // JSON 파일을 읽고 파싱하여 HERO_LIST 채우기
            string jsonData = File.ReadAllText(filePath);
            var heroesData = JsonConvert.DeserializeObject<HeroData>(jsonData);

            foreach (var heroData in heroesData.heroes)
            {
                Hero newHero = new Hero
                {
                    ID = heroData.ID.ToString("D3"), // ID를 3자리 문자열로 변환
                    RANK = heroData.RANK,
                    NAME = heroData.NAME,
                    POS = heroData.POS,
                    ATK_BASE = heroData.ATK,
                    DEF_BASE = heroData.DEF,
                    HP_BASE = heroData.HP,
                    ATK_RANGE_BASE = heroData.ATK_RANGE,
                    ATK_SPD_BASE = heroData.ATK_SPD
                };

                HERO_LIST.Add(newHero);
            }

            Debug.Log("HERO_LIST가 JSON 파일로부터 성공적으로 초기화되었습니다.");
        }
        else
        {
            Debug.LogWarning("JSON 파일을 찾을 수 없습니다: " + filePath);
        }
    }

    // SummonHero 함수: 소환 버튼 클릭 시 실행되는 함수
  public void SummonHero()
{
    if (MainTower != null)
    {
        int LOTTERY_LEVEL;
        try
        {
            LOTTERY_LEVEL = MainTower.GetComponent<Tower>().TOWER_LEVEL;
            Debug.Log("MainTower의 TOWER_LEVEL이 성공적으로 가져와졌습니다.");
        }
        catch
        {
            Debug.LogError("MainTower의 TOWER_LEVEL을 가져오는 데 실패했습니다.");
            return;
        }

        List<Hero> SELECT_HERO_LIST = new List<Hero>();

        for (int i = 0; i < 5; i++)
        {
            float randomValue = Random.value;
            int RANK = DetermineHeroRank(LOTTERY_LEVEL, randomValue);

            List<Hero> filteredHeroes = HERO_LIST.FindAll(hero => hero.RANK == RANK);

            if (filteredHeroes.Count > 0)
            {
                Hero selectedHero = filteredHeroes[Random.Range(0, filteredHeroes.Count)];
                SELECT_HERO_LIST.Add(selectedHero);
            }
            else
            {
                Debug.LogWarning($"RANK {RANK}에 해당하는 영웅이 없습니다.");
            }
        }

        if (BottomMenuBar == null)
        {
            Debug.LogError("BottomMenuBar가 null 상태입니다. Inspector에서 제대로 연결되었는지 확인하세요.");
            return;
        }

        GameObject summonButton = GameObject.Find("SummonButton");
        if (summonButton == null)
        {
            Debug.LogError("SummonButton을 찾을 수 없습니다.");
            return;
        }

        summonButton.SetActive(false);
        DisplaySummonedHeroes(SELECT_HERO_LIST);
    }
    else
    {
        Debug.LogWarning("MainTower가 설정되지 않았습니다.");
    }
}

    private int DetermineHeroRank(int towerLevel, float randomValue)
    {
        int rank = 1;
        switch (towerLevel)
        {
            case 1: rank = 1; break;
            case 2: rank = randomValue < 0.75f ? 1 : 2; break;
            case 3: rank = randomValue < 0.55f ? 1 : (randomValue < 0.8f ? 2 : 3); break;
            case 4: rank = randomValue < 0.45f ? 1 : (randomValue < 0.7f ? 2 : 3); break;
            case 5: rank = randomValue < 0.3f ? 1 : (randomValue < 0.6f ? 2 : 3); break;
            default: rank = 1; break;
        }
        return rank;
    }

private void DisplaySummonedHeroes(List<Hero> heroList)
{
    foreach (Hero hero in heroList)
    {
        // HeroBlock 생성 (영웅의 이름을 포함)
        GameObject heroBlock = new GameObject($"HeroBlock_{hero.NAME}");
        heroBlock.transform.SetParent(BottomMenuBar.transform);

        // HeroBlock에 Image 컴포넌트 추가 및 기본 설정
        Image heroImage = heroBlock.AddComponent<Image>();
        heroImage.color = Color.black; // 기본 배경 색상 설정
        RectTransform rectTransform = heroBlock.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(150, 200); // HeroBlock 크기 설정
        rectTransform.localScale = Vector3.one; // 스케일 초기화

        // 영웅의 이미지 추가 (Resources 폴더에서 이미지 파일 로드)
        GameObject heroPicture = new GameObject($"HeroImage_{hero.NAME}");
        heroPicture.transform.SetParent(heroBlock.transform);
        Image heroSprite = heroPicture.AddComponent<Image>();

        // 이미지 로드 시 확장자 제외하고 Resources 폴더에서 가져오기
        string imagePath = "HeroImages/" + hero.NAME;
        Debug.Log($"로드 시도 중인 이미지 경로: {imagePath}");

        // 이미지 로드
        heroSprite.sprite = Resources.Load<Sprite>(imagePath);

        // 이미지가 로드되지 않을 경우 기본 이미지 설정 (디버깅용)
        if (heroSprite.sprite == null)
        {
            Debug.LogWarning($"이미지를 찾을 수 없습니다: {hero.NAME}. Resources/HeroImages 폴더에 이미지 파일을 확인하세요.");
        }
        else
        {
            Debug.Log($"이미지 로드 성공: {hero.NAME}");
        }

        // RectTransform을 사용해 이미지 크기 조정
        RectTransform spriteTransform = heroPicture.GetComponent<RectTransform>();
        spriteTransform.sizeDelta = new Vector2(256, 256); // 이미지 크기를 256x256으로 조정
        spriteTransform.localScale = Vector3.one; // 스케일을 초기화하여 왜곡 방지
        spriteTransform.anchoredPosition = new Vector2(0, 30); // 이미지 위치 조정

        // 영웅의 이름과 등급 텍스트 추가
        GameObject heroNameObj = new GameObject($"HeroName_{hero.NAME}");
        heroNameObj.transform.SetParent(heroBlock.transform);
        Text heroNameText = heroNameObj.AddComponent<Text>();
        heroNameText.text = $"{hero.NAME} - Rank {hero.RANK}";
        heroNameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        heroNameText.alignment = TextAnchor.MiddleCenter;
        heroNameText.color = Color.white;
        RectTransform nameTransform = heroNameObj.GetComponent<RectTransform>();
        nameTransform.sizeDelta = new Vector2(140, 30);
        nameTransform.anchoredPosition = new Vector2(0, -80);

        // 클릭 이벤트 연결
        Button heroButton = heroBlock.AddComponent<Button>();
        heroButton.onClick.AddListener(() => InstantiateHero(hero));
    }
}

    private void InstantiateHero(Hero hero)
    {
        if (HERO_PREFAB != null)
        {
            GameObject instantiatedHero = Instantiate(HERO_PREFAB, Vector3.zero, Quaternion.identity);
            HERO_IN_DECK.Add(hero);
        }
        else
        {
            Debug.LogWarning("HERO_PREFAB이 설정되지 않았습니다.");
        }
    }
}

// JSON 파싱용 클래스
[System.Serializable]
public class HeroData
{
    public List<HeroDetails> heroes;
}

[System.Serializable]
public class HeroDetails
{
    public int ID;
    public int RANK;
    public string NAME;
    public string POS;
    public int ATK;
    public int DEF;
    public int HP;
    public int ATK_RANGE;
    public int ATK_SPD;
}