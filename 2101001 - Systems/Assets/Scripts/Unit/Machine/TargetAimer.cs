using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAimer : MonoBehaviour
{
    // 주변 컴포넌트들에게 방향 값을 제공합니다
    // 유닛 베이스에게 각도를 설정합니다.


    bool ConnectedPlacementAssigned;
    
    GameObject ConnectedPlacement; // 이 게임오브젝트를 향헤 센서스트링이 레이저를 발사할 것입니다.
    float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        ConnectedPlacementAssigned = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(time >= 0.1f)
        {
            time = 0.0f;
            if (ConnectedPlacementAssigned)
            {
                Debug.DrawRay(transform.position, ConnectedPlacement.transform.position - transform.position, Color.green, 0.05f);

                // 라이트메이커 (레이)가 있으면, 라이트 메이커 레이에게 방향을 알려줍니다.
                // 라이트 라이트 센서(레이)가 있으면 방향을 알려줍니다.
            }
        }
        time += Time.deltaTime;

        // 대상을 항해 레이저 발사
        // 만약 한번이라도 맞았으면 데이터 목록에 그 결과를 저장.
    }
    // 상호작용용 함수
    // 커넥티드 플레이스먼트 등록
    // 등록 해제
    public void SetConnectedPlacement(GameObject target)
    {
        ConnectedPlacement = target;
        ConnectedPlacementAssigned = true;


        // 주변 컴포넌트에게 방향을 지정해줍니다.
        // 방향을 찾습니다
        Vector3 direction = target.transform.position - transform.position;

        // 컴포넌트들을 찾습니다.
        UnitBase thisUnitBase = GetComponent<UnitBase>();
        LightMaker thisLightMaker = GetComponent<LightMaker>();
        LightSensor thisLightSensor = GetComponent<LightSensor>();

        if (thisUnitBase != null) thisUnitBase.SetUnitDirection(direction);
        if (thisLightMaker != null) thisLightMaker.direction = direction;
        if (thisLightSensor != null) thisLightSensor.direction = direction;
    }
    public void ClearConnectedPlacement()
    {
        ConnectedPlacement = null;
        ConnectedPlacementAssigned = false;
    }


    void ClockWork()
    {
        //연결된 대상을 향해 데이터 목록을 보냄
        // 데이터 목록을 치움.

        

    }
}
