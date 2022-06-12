using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSightController : MonoBehaviour
{
    int UnitId;
    PlayerController playerController;
    Transform myTf;
    Transform playerTf;
    GameObject memoryBlockPrefab;
    MemoryBlock memoryBlockComponent;

    // Start is called before the first frame update
    void Start()
    {
        //




        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        myTf = GetComponent<Transform>();
        playerTf = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myTf.position = playerTf.position;
        myTf.localScale = new Vector3(playerController.SightRange, 1, playerController.SightRange);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ouch");
        if(other.gameObject.layer == 8)
        {
            // 충돌한 물체의 타일을 저장.
            TileBlock tileBlock = other.GetComponent<TileBlock>();
            Transform transform = other.GetComponent<Transform>();
            Vector3 locate = new Vector3();
            locate.x = transform.position.x;
            locate.y = transform.position.y + tileBlock.deltaYForCloneGO;
            locate.z = transform.position.z;

        
        // GetComponent?
        //Transform other.GameOb

        //Instantiate(/*콜라이더의 메모리 GO*/, /*콜라이더의 위치 값*/, Quaternion.identity) ;
            //저장한 값을 토대로 프리펩 생성
            memoryBlockPrefab = Instantiate(tileBlock.cloneGO, locate, Quaternion.identity);
            memoryBlockComponent = memoryBlockPrefab.GetComponent<MemoryBlock>();
            memoryBlockComponent.blockTypeID = tileBlock.blockTypeID;
        }

    }
}
