// 플레이어 입장에서 미확인된 지역을 나타내는 프리펩에 넣는 컴포넌트입니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFog : MonoBehaviour
{
    Transform myTransform;
    PlayerController playerController;
    Dictionary<Vector3, int> playerMemoryMap;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        playerMemoryMap = GameObject.Find("Player").GetComponent<UnitMemory>().memoryMap;
        SaveMapData(100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Memory")
        {
            Destroy(gameObject);


            // 메모리 타일이 들어왔으므로 나가야 합니다
        }
    }

    void SaveMapData(int _blockTypeID)
    {
        if (playerMemoryMap.ContainsKey(myTransform.position) == true)
        {
            playerMemoryMap.Remove(myTransform.position);
            playerMemoryMap.Add(myTransform.position, _blockTypeID);
            Debug.Log("메모리맵이 갱신되었습니다. x:" + myTransform.position.x);
        }
        else // 신규 블럭 등록
        {
            playerMemoryMap.Add(myTransform.position, _blockTypeID);
            //Debug.Log("메모리맵이 저장되었습니다.");
        }
    }
}
