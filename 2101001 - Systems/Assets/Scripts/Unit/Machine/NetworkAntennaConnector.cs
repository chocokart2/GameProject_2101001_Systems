using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAntennaConnector : MonoBehaviour
{
    // 네트워크 커넥터를 자신의 필드처럼 사용합니다.
    // 구체적으로 네트워크 연결하는 것은 물리층에서 다루어지며, 차일드 오브젝트의 스크립트들을 참조하세요.

    [SerializeField] GameObject AntennaRangeObject;

    // Start is called before the first frame update
    void Start()
    {
        GameObject instantiatedObject = Instantiate(AntennaRangeObject, transform.position, Quaternion.identity);
        instantiatedObject.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
