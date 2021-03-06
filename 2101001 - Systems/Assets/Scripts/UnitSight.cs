using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    /// <summary>
    /// 11
    /// </summary>

    int UnitId;
    //PlayerController playerController;
    HumanUnitBase myHumanUnitBase;
    //Transform playerTf;
    GameObject memoryBlockPrefab;
    MemoryBlock memoryBlockComponent;
    bool isInitCalled = false;

    public GameObject FollowingObject = null;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ouch");
        //if(other.gameObject.layer == 8)
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
        if(other.gameObject.GetComponent<UnitBase>() != null)
        {
            other.gameObject.GetComponent<UnitBase>().SightEnter(transform.parent.gameObject.GetComponent<UnitBase>().unitBaseData.teamID);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<UnitBase>() != null)
        {
            other.gameObject.GetComponent<UnitBase>().SightExit(transform.parent.gameObject.GetComponent<UnitBase>().unitBaseData.teamID);
        }
    }

}
