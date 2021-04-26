// 실제 게임 맵에 존재하는 타일입니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlock : MonoBehaviour
{
    public GameObject cloneGO;
    public float deltaYForCloneGO = 0.0f;
    public int blockTypeID;
    Transform myTransform;
    bool triggerDarkfog = true;
    bool triggerGameManager = true;
    bool isStartWithDarkFog = true; // 0: 모르겠다 1: 아니다 2:그렇다
    //외부 컴포넌트
    GameManager thatGameManager;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myTransform.position = new Vector3(Mathf.RoundToInt(myTransform.position.x), Mathf.RoundToInt(myTransform.position.y), Mathf.RoundToInt(myTransform.position.z));
    }

    /// <summary>
    ///
    /// </summary>

    // Update is called once per frame
    void Update()
    {
        if (triggerDarkfog)
        {
            if(isStartWithDarkFog == true)
            {
                Vector3 locate = new Vector3();
                locate.x = myTransform.position.x;
                locate.y = myTransform.position.y + deltaYForCloneGO;
                locate.z = myTransform.position.z;

                // preference
                GameObject tempCloneGO = Instantiate(cloneGO, locate, Quaternion.identity);
                MemoryBlock tempCloneGOMemoryBlock = tempCloneGO.GetComponent<MemoryBlock>();
                tempCloneGOMemoryBlock.SetPreference(0.0f);
                tempCloneGOMemoryBlock.BeginBlock();
            }
            triggerDarkfog = false;
        }
        if (triggerGameManager)
        {
            thatGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            thatGameManager.SetTileMap(myTransform.position, blockTypeID);
            triggerGameManager = false;
            //Debug.Log("yee"); // 작동함!
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "StartDarkFog")
        {
            isStartWithDarkFog = false;
            // 컴파일 오류는 없지만 논리 오류가 있을 수 있음.
            // 만약에 instantiate가 먼저 이루어지고 DarkFog가 있는걸 감지한다면 논리적 오류가 발생할 수 있음.
        }
    }
}
