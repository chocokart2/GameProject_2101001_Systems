using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampainButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 필드
    WorldManager.Header loadData;


    #endregion
    
    public void ButtonPressed()
    {
        if(!GameManager.DataLoading("Assets/GameFile/Campain/Header/Header.json", ref loadData))
        {
            // 세계를 만듭니다.
            Debug.Log("새 시나리오를 만듭니다");
            // 시나리오 선택

            WorldCreater.CreateWorld("Assets/GameFile/Campain", "Busan");
        }
        else
        {
            // 저장한 데이터를 로딩하는 씬
            Debug.Log("데이터를 찾았습니다");
        }
    }
}
