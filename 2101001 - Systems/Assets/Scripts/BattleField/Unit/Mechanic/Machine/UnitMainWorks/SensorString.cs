using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorString : MonoBehaviour
{
    // SensorString은 머신 네트워크랑 자신 정도만 책임 지면 됩니다
    // 여러 함수를 호출하는 역할을 맡습니다.

    void Start()
    {
        // ComponentFind
        myLightMaker = GetComponent<LightMaker>();
        myLightSensor = GetComponent<LightSensor>();
        //myConnecter

        GameObject.Find("GameManager").GetComponent<GameManager>().MachineWorkEvent += new GameManager.VoidToVoid(Work);
    }

    #region Field

    LightMaker myLightMaker;
    LightSensor myLightSensor;
    #endregion


    // 이벤트로 주기적으로 호출되는 함수입니다.

    public void Work() // 함수가 호출되었습니다.
    {
        // 1. 작업
        // 라이트메이커에게 빛을 발사시키도록 함
        // 라이트센서에게 빛을 감지시키도록 함
        // 감지한 빛을 필터하도록 함
        // 라이트 센서가 빛을 감지하여 메시지를 리턴하도록 합니다.

        // 2. 데이터 작업
        // Send 함수를 호출시킨다.

        MachineUnitBase.MachineNetMessage gotMessage;



        myLightMaker.Work();
        gotMessage = myLightSensor.Work();
        Debug.Log(gotMessage.type);
        foreach(string subData in gotMessage.subData)
        {
            Debug.Log(subData);
        }



    }

    public void Sensor() // 이벤트로 인해 UnitBase로부터 호출
    {
        // 만약에

    }
}
