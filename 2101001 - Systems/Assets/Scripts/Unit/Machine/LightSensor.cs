using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    // 콜라이더를 가지는 자식 게임오브젝트를 만들고,
    // 콜라이더에 충돌한 게임오브젝트중 LightMaker에 의해 특정한 파장에 닿은 게임오브젝트가 있으면 이를 감지합니다.
    // 유닛 분석기 컴포넌트가 없으면 바로 네트워크로 전송합니다

    public GameObject LightSensingGameObject; // 이하 주석에는 LSG라고 표기합니다.
    public Vector3 direction; // 외부에 의해 설정되는 값입니다!

    UnitAnalyzer myUnitAnalyzer;
    bool isUnitAnalyzerExist;
    LightSensorChildCylinder childLightSensorChildCylinder;
    LightSensorChildRay childLightSensorChildRay;

    // LSG에서 ChildTriggerEnter을 실행합니다


    // Start is called before the first frame update
    void Start()
    {
        GameObject InstantiatedObject = Instantiate(LightSensingGameObject, transform.position, Quaternion.identity);
        InstantiatedObject.transform.parent = transform;
        if(transform.Find("LightSensorChildCylinderRange") != null)
        {
            childLightSensorChildCylinder = transform.Find("LightSensorChildCylinderRange").GetComponent<LightSensorChildCylinder>();
        }
        if(transform.Find("LightSensorChildRay(Clone)") != null)//
        {
            childLightSensorChildRay = transform.Find("LightSensorChildRay(Clone)").GetComponent<LightSensorChildRay>();
        }

        myUnitAnalyzer = GetComponent<UnitAnalyzer>();
        isUnitAnalyzerExist = (myUnitAnalyzer != null);
    }

    public MachineUnitBase.MachineNetMessage Work()
    {
        // 자식 게임오브젝트의 컴포넌트에 접근하여 함수를 호출합니다.
        // 발견된 값이 있으면 목록에 등록합니다.
        // 유닛 분석기에 정보를 저장합니다.

        Collider[] colliders = new Collider[1];

        MachineUnitBase.MachineNetMessage returnValue = new MachineUnitBase.MachineNetMessage();
        bool isDetected = false;

        // 빛을 감지하도록 합니다.
        if (childLightSensorChildCylinder != null)
        {
            // 특정한 방향으로 라이트를 설정하도록 요구합니다.
            // 방향이 미지정 되어 있으면 원점에서 퍼지도록 요구합니다
            if (direction == null) direction = new Vector3(0, 0, 0);
            childLightSensorChildCylinder.SetAngle(direction);
            isDetected = childLightSensorChildCylinder.tryDetect(ref colliders);
        }
        else if (childLightSensorChildRay != null)
        {
            Debug.Log("DEBUG_LightSensor.Work: childLightSensorChildRay컴포넌트가 Null이 아닙니다.");
            // 특정한 방향으로 레이를 쏘도록 요구합니다
            // 방향이 미지정 되어 있다면 레이를 쏘지 않습니다.

            if ((direction != null) && (direction != new Vector3(0, 0, 0)))
            {
                Debug.Log("DEBUG_LightSensor.Work: direction가 방향을 가집니다.");
                isDetected = childLightSensorChildRay.LightSense(direction, ref colliders);
            }
        }

        if (isDetected)
        {
            if (myUnitAnalyzer != null)
            {
                // UnitAnalyzer에서 콜라이더 어레이를 보내고, 메시지를 받는다.
                returnValue = myUnitAnalyzer.Analyze(colliders);
            }
            else
            {
                returnValue.type = "SenseAboutUnit";
                returnValue.subDataType = new string[] { "bool" };
                returnValue.subData = new string[] { "true" };
            }
        }
        else
        {
            returnValue.type = "TrashMessage";
            returnValue.subDataType = new string[] { "DUMMY" };
            returnValue.subData = new string[] { "DUMMY" };
        }
        return returnValue;
    }
}
