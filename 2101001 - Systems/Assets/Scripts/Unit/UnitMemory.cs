// 기억 능력을 사용하는 유닛인경우 이 컴포넌트를 넣습니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMemory : MonoBehaviour
{
    public Dictionary<Vector3, int> memoryMap;

    // Start is called before the first frame update
    void Start()
    {
        memoryMap = new Dictionary<Vector3, int>();
    }
    
        

    // Update is called once per frame
    void Update()
    {
        
    }
}
