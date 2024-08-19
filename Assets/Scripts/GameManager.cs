using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject MainTower; // MainTower 오브젝트를 연결
    public Button SummonButton; // SummonButton을 연결할 변수
    public GameObject HeroPrefab; // 생성할 hero 프리팹을 연결할 변수

    void Start()
    {
        // SummonButton에 클릭 이벤트를 연결
        SummonButton.onClick.AddListener(CreateNewHero);
    }

    void Update()
    {

    }

    // SummonButton이 클릭될 때 호출되는 메서드
    void CreateNewHero()
    {
        // MainTower 컴포넌트에서 heroes 리스트를 가져오기
        tower towerComponent = MainTower.GetComponent<tower>();

        if (towerComponent != null && HeroPrefab != null)
        {
            // hero 프리팹을 인스턴스화
            GameObject newHeroObject = Instantiate(HeroPrefab);

            // 생성된 hero를 MainTower의 하위로 설정
            newHeroObject.transform.SetParent(MainTower.transform);

            // hero 오브젝트를 MainTower의 heroes 리스트에 추가
            towerComponent.heroes.Add(newHeroObject);

            Debug.Log("New hero prefab instantiated and added to the MainTower!");
        }
        else
        {
            Debug.LogWarning("Tower component or HeroPrefab not found.");
        }
    }
}
