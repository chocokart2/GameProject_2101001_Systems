﻿using System.Collections;
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
    MemoryBlock memoryBlockComponent; // 이 변수를 가지고 있는 이유는 유닛을 납치했을때 지역 정보도 얻을 수 있기 때문입니다.
    bool isInitCalled = false;

    public GameObject FollowingObject = null;

    void OnTriggerEnter(Collider other)
    {
        #region 함수 설명

        // 타일 블럭이 충돌함
        // 이벤트 발생?

        #endregion



        //Debug.Log("Ouch");
        //if(other.gameObject.layer == 8)
        if (other.gameObject.layer == 8)
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
            Debug.Log($"DEBUG_UnitSight.OnTriggerEnter : {other.gameObject.name}");
            UnitBase recvUnitBase = other.gameObject.GetComponent<UnitBase>();

            if (recvUnitBase == null) Debug.Log("Error_UnitSight.OnTriggerEnter : 널 값입니다.");
            if (transform.parent == null) Debug.Log("Error_UnitSight.OnTriggerEnter : transform.parent 널 값입니다.");
            if (transform.parent.gameObject == null) Debug.Log("Error_UnitSight.OnTriggerEnter : transform.parent.gameObject널 값입니다.");
            if (transform.parent.gameObject.GetComponent<UnitRole>() == null) Debug.Log("Error_UnitSight.OnTriggerEnter : UnitRole이 널 값입니다.");
            if (transform.parent.gameObject.GetComponent<UnitRole>().GetData() == null) Debug.Log("Error_UnitSight.OnTriggerEnter : UnitRole.GetData()이 널 값입니다.");
            if (transform.parent.gameObject.GetComponent<UnitRole>().GetData().teamID == null) Debug.Log("Error_UnitSight.OnTriggerEnter : UnitRole.GetData().teamID이 널 값입니다.");
            Debug.Log($"DEBUG_UnitSight.OnTriggerEnter : {recvUnitBase.name}");

            //other.gameObject.GetComponent<UnitBase>().
            recvUnitBase.
                SightEnter(
                transform.parent.gameObject.GetComponent<UnitRole>().GetData().teamID);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<UnitBase>() != null)
        {
            other.gameObject.GetComponent<UnitBase>().SightExit(transform.parent.gameObject.GetComponent<UnitRole>().GetData().teamID);
        }
    }

}
