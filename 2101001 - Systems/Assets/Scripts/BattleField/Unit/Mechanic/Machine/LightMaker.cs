using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMaker : MonoBehaviour
{
    public GameObject LightMakingGameObject;
    public Vector3 direction;

    [SerializeField] int lightType = -1; // 빛의 종류, 1은 랜더링용, -1은 미지정

    bool InitTrigger = false;
    LightMakerChildRay childLightMakerChildRay;

    // 빛의 범위 형태: 구형, 레이저형

    // Start is called before the first frame update
    void Start()
    {
        GameObject InstantiatedObject = Instantiate(LightMakingGameObject, transform.position, Quaternion.identity);
        InstantiatedObject.transform.parent = transform;
        InstantiatedObject.gameObject.name = "LightMakerChild";
        childLightMakerChildRay = transform.Find("LightMakerChild").GetComponent<LightMakerChildRay>();
    }


    // 튜플 (전자기파의 파장, 그 파장을 맞은 갯수) - 취소됨



    // 인스펙터 대신에 사용할 수 있는 함수.
    // 레이저 감시용이든 아니든 일단 빛에 닿으면 유닛이 드러날 수 있습니다.
    // 하지만 레이저 감시용으로는 타일은 드러내지 못합니다
    public void Init(int lightType) // 이 함수는 Instantiated하자마자 다음 줄에 바로 호출되어야만 합니다. 아주 중요합니다.
    {
        if (InitTrigger) return;
        this.lightType = lightType;
        InitTrigger = true;
    }

    public void Work()
    {
        if ((direction == null) || (direction == new Vector3(0, 0, 0)))
            Debug.Log("DEBUG_LightMaker.Work: 방향이 지정되지 않았습니다.");

        // 만약 자식 오브젝트가 레이라면:
        // 대상을 향해 빛을 쏘도록 요구

        // 만약 실린더라면 : 차일드 게임오브젝트의 위치를 바꾸도록 합니다.
        // 만약 레이라면: 레이를 쏘도록 합니다.
        if(childLightMakerChildRay != null)
        {
            if ((direction != null) && (direction != new Vector3(0, 0, 0)))
            {
                childLightMakerChildRay.ShootLight(direction);
            }
        }
    }

    //ChildTriggerEnter와 ChildTriggerExit는 자식 오브젝트가 유닛과 충돌하게 되면 호출하는 함수입니다.

    public void ChildTriggerEnter(Collider other)
    {
        if (lightType.Equals(-1))
        {
            Debug.LogError("<!> ERROR_LightMaker.OnTriggerEnter: lightType가 정의되지 않았습니다. Init함수를 실행시키거나, 인스펙터에 값을 추가해주세요.");
        }
        else if (other.gameObject.GetComponent<UnitBase>() != null)
        {
            Debug.Log("DEBUG_LightMaker.ChildTriggerEnter: 유닛을 비춥니다.");
            other.gameObject.GetComponent<UnitBase>().LightEnter(lightType);
        }
        // 타일 블럭에서도 설정
    }

    public void ChildTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<UnitBase>() != null)
        {
            other.gameObject.GetComponent<UnitBase>().LightExit(lightType);
        }
    }
}
