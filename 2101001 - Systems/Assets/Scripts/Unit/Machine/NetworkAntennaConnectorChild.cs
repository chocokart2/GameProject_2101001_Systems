using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAntennaConnectorChild : MonoBehaviour
{
    NetworkConnector myNetworkConnector;

    // Start is called before the first frame update
    void Start()
    {
        myNetworkConnector = transform.parent.GetComponent<NetworkConnector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 네트워크 안테나는 네트워크 안테나끼리만 통신 가능합니다.
    // 들어온 안테나 커넥터는 등록하고
    // 나간 안테나 커넥터는 제거합니다.

    //<!> 이슈: 리지드 바디가 없어서 충돌하지 않음.
    void OnTriggerEnter(Collider other)
    {
        // 함수 코멘트
        // 자신의 부모도 충돌 체크를 합니다. 자신하고는 충돌 체크를 하지 않아요.

        // 함수 설명
        // 충돌한 물체가 네트워크 관련 컴포넌트를 가지고 있으면 네트워크 연결을 시도합니다.
        // 이때 기준이 되는게
        // 1. 머신 설치일것.
        // 2. 네트워크가 있는 차일드에 네트워크 컴포넌트를 가지고 있는 머신이 있을것.

        Debug.Log("DEBUG_NetworkAntennaConnectorChild.OnTriggerEnter: " + other.name + ", 위치 : " + other.transform.position);
        if (other.name.StartsWith("MachinePlacement")) // 네트워크 머신이 설치될 수 있는 존재인지 확인합니다.
        {
            if(other.transform.Find("FixedNetworkPosition").childCount == 1) // 네트워크 자리에 유닛이 있는지 체크
            {
                if (other.transform.Find("FixedNetworkPosition").GetChild(0).name.StartsWith("MachineNetworkAntenna"))
                {
                    myNetworkConnector.TryChennelConnect(other.transform.Find("FixedNetworkPosition").GetChild(0).GetComponent<NetworkConnector>());
                }


            }
        }



        //// 만약 충돌한 머신이 안테나커넥터를 가지고 있으면, 부모의 NetworkAntennaConnector에 접근하여 등록합니다.
        //if (other.name.StartsWith("MachineNetworkAntenna")) // 리지드바디가 없어서 충돌하지 않음
        //{
        //    //myNetworkConnector.reached\
        //    if (other.gameObject.transform != transform.parent) // +ReachedClients에 동일한 유닛 아이디가 아닌 경우에만 가능(TriggerEnter로 들어온 것과 로딩한 데이터와 충돌시 데이터를 먼저 우선시합니다.)
        //    {
        //        // 연결된 대상에 등록을 합니다.
        //        myNetworkConnector.TryChennelConnect(other.GetComponent<NetworkConnector>());
                

        //        //myNetworkConnector.ReachedClients.Add(other.GetComponent<NetworkConnector>());
        //    }
        //}
    }

    void OnTriggerExit(Collider other)
    {
        // 만약 충돌한 머신이 안테나커넥터를 가지고 있으면, 부모의 NetworkAntennaConnector에 접근하여 등록합니다.
        if (other.name.StartsWith("MachineNetworkAntenna"))
        {
            //myNetworkConnector.reached\
            if (other.gameObject.transform != transform.parent) // +ReachedClients에 동일한 유닛 아이디가 아닌 경우에만 가능(TriggerEnter로 들어온 것과 로딩한 데이터와 충돌시 데이터를 먼저 우선시합니다.)
            {
                // 연결된 대상에 등록을 합니다.
                myNetworkConnector.ChennelDisconnect(other.GetComponent<NetworkConnector>());


                //myNetworkConnector.ReachedClients.Add(other.GetComponent<NetworkConnector>());
            }
        }
    }
}
