using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObjectOfUnit : MonoBehaviour
{
    public bool isRenderingChangable = true; // 이 값은

    void Start()
    {
        //hierarchyGameObjectNameForRendering

        Transform targetTf = transform;
        string hierarchyName = gameObject.name;
        // UnitBase까지 찾아갑니다.
        for (int count = 0; count < 100; count++) // 무한루프를 금지합니다.
        {
            targetTf = targetTf.parent;
            //Debug.Log("LOG COOU - " + hierarchyName);
            hierarchyName = targetTf.gameObject.name + "/" + hierarchyName; // ERROR! - targetTf.gameObject.name에서 오류 발생 - NullReferenceException: Object reference not set to an instance of an object
            //Debug.Log(hierarchyName);
            //hierarchyName = string.Format("{0}{1}{2}", gameObject.name, '/', hierarchyName);
            if (targetTf.parent == null)
            {
                //Debug.Log("부모 트랜스폼을 찾을 수 없음. 횟수 - " + count);
                break;
            }
            if (targetTf.gameObject.GetComponent<UnitBase>() != null)
            {
                break; // 마더를 찾았습니다.
            }
        }
        if(targetTf.GetComponent<UnitBase>() != null)
        {
            targetTf.gameObject.GetComponent<UnitBase>().hierarchyGameObjectNameForRendering.Add(hierarchyName);
        }
        else
        {
            Debug.Log("<!> ERROR_ChildObjectOfUnit.Start : 최종 부모 게임오브젝트에 UnitBase컴포넌트가 존재하지 않습니다. 게임오브젝트 이름 : " + transform.name);
        }
    }
}