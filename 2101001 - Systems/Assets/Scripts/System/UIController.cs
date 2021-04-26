using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public LayerMask unitLayerMask;
    public LayerMask tileLayerMask;


    GameObject selectedUnit;
    UnitMovable selectedUnitMovable;

    //List<GameObject>




    // Start is called before the first frame update
    void Start()
    {
        selectedUnit = null;
        selectedUnitMovable = null;
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {
            UnitSelect();
        }
        if (Input.GetMouseButtonDown(1))
        {

            // 유닛 선택 시: 이동 명령
            if ((selectedUnit != null) && (selectedUnitMovable != null)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            }
        }
        if (Input.GetMouseButtonUp(2)) // 마우스 휠 
        {

        }
    }

    void UnitSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50.0f, unitLayerMask))
        {
            if (hit.transform.CompareTag("Unit"))
            {
                // 통제권이 있는지 확인합니다.
                // 인풋, 혹은 아웃풋에 접근할 수 있는지 확인합니다.
                //
            }
        }
        // 부딛힌 게임오브젝트 확인




    }
}
