using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePlacement : MonoBehaviour
{
    // 게임오브젝트를 설치할 수 있다면 됩니다.

    public GameObject emptyObject; // 이 오브젝트의 자식에 다른 머신을 담을 단순한 빈 공간입니다.
    public bool IsMachineConnected; // 메인 머신이 설치되었음을 알리는 변수입니다.
    public bool IsNetworkConnected; // IsMachineConnected와 역할은 비슷.


    // Start is called before the first frame update
    void Start()
    {

        GameObject instantiatedObject1 = Instantiate(emptyObject, transform.position, Quaternion.identity);
        instantiatedObject1.transform.parent = gameObject.transform;
        instantiatedObject1.name = "FixedMachinePosition";
        GameObject instantiatedObject2 = Instantiate(emptyObject, transform.position, Quaternion.identity);
        instantiatedObject2.transform.parent = gameObject.transform;
        instantiatedObject2.name = "FixedNetworkPosition";
        IsMachineConnected = false;
        IsNetworkConnected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
