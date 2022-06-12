using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanActingGuiController : MonoBehaviour
{
    // 플레이어가 컨트롤하는 휴먼 유닛이
    // 만약 추가적인 GUI가 필요하다고 느낄때 여기에 스크립트를 추가합니다.
    // 예를 들어 BuildTool 아이템에서 어떤 아이템을 설치할지 결정할때 이 스크립트가 실행됩니다.


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Field
    #region Prefab
    [SerializeField] GameObject TextObject; // 텍스트를 집어넣을 수 있는 게임오브젝트입니다.



    #endregion


    #endregion
    #region Function
    // ShowSubItem 함수 설명
    // 자신의 자식으로 아이템의 서브 아이템을 UI로 보여줍니다.

    // HideSubItem 함수 설명
    // 자신의 자식을 전부 제거합니다.
    public void ShowSubItem(UnitItemPack.BuildTool buildTool)
    {
        // 머신들 인벤토리 상태를 보여줍니다.
        // 9개의 방향으로 머신 이름을 보여주고, 갯수도 보여줍니다.
        // selectedIndex은 다른색으로 보여줍니다.

        // 각 인덱스마다
        // EachClassIndex를 표시합니다.

        if (buildTool.IsEachClassIndexNull()) return; // 빌드툴 아이템이 없습니다.

        HideSubItem();

        float radius = 3.0f;
        for (int typeIndex = 0; typeIndex < 9; typeIndex++) // 9번 반복합니다.
        {
            float deg = 110 + typeIndex * 40;
            float x = radius * Mathf.Cos(deg * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(deg * Mathf.Deg2Rad);
            GameObject instantiatedObject = Instantiate(TextObject, transform.position + new Vector3(x, y, 0), Quaternion.identity);
            instantiatedObject.transform.parent = transform;
            // 텍스트 바꾸기
            // 머신의 이름 삽입
            //string MachineName = buildTool.EachClassIndex[buildTool.selectedIndex].ToString();
            //string MachineName = buildTool.GetEachClassIndexElement(typeIndex).ToString();
            string MachineName = GameObjectList.MachineIdToNameKR(buildTool.GetEachClassIndexElement(typeIndex));


            switch (buildTool.GetEachClassIndexElement(typeIndex)) // 숫자를 통해서
            {
                default:
                    break;
            }
            // 머신의 갯수 삽입
            if(buildTool.GetEachClassIndexElement(typeIndex) != 0)
            {
                MachineName += " (" + buildTool.subItem[("MachineUnit" + buildTool.GetEachClassIndexElement(typeIndex).ToString())] + " Left)";
            }

            // 게임오브젝트에 텍스트 삽입
            instantiatedObject.GetComponent<TextMesh>().text = MachineName;

            // 색깔 바꾸기: 이 게임오브젝트가 현재 선택된 인덱스와 같은경우.
            if(typeIndex == buildTool.GetSelectedIndex())
            {
                instantiatedObject.GetComponent<TextMesh>().color = Color.cyan;
            }
        }


    }

    public void HideSubItem()
    {
        // 자신의 자식을 전부 제거합니다.
        for(int childIndex = transform.childCount - 1; childIndex > -1; childIndex--) // 맨 마지막 차일드부터 첫번째 차일드까지 도는 루프입니다.
        {
            Destroy(transform.GetChild(childIndex).gameObject);
        }
    }


    #endregion
}
