using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCameraController : MonoBehaviour
{
    // 고정값
    Vector3 DefaultDeltaCameraAndPoint { get { return new Vector3(0, 7, -6); } } // 카메라가 바라보고 있는 대상과 카메라의 위치 차이의 기본값입니다.
    
    string theGameObject;
    //Transform myTransform;
    
    Vector3 deltaCameraAndPoint;
    /*Vector3 targetPosition;*/ GameObject targetObject; // 바라보아야 하는 대상입니다. 게임오브젝트를 담은 이유는 벡터 정보를 그대로 담았다간 깊은 복사가 발생할 것을 우려하여, 게임오브젝트는 참조로 정보가 업데이트
    Dictionary<string, GameObject> objectMemory; // 여러 오브젝트를 임시로 저장할 수 있는 곳입니다.

    // Start is called before the first frame update
    void Start()
    {
        objectMemory = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        LiveValue();
    }

    void LiveValue()
    {

    }

    #region Public Function




    #region 필드 변경
    public void SetTargetObject(GameObject newTargetObject)
    {
        targetObject = newTargetObject;
    }
    public bool TryChangeTargetObject(string index)
    {
        if (objectMemory.ContainsKey(index) == true)
        {
            targetObject = objectMemory[index];
            return true;
        }
        else return false;
    }
    public void SaveObject(GameObject newObject, string index)
    {
        // 함수 설명
        // 오브젝트 메모리에 인덱스에 뉴오브젝트를 저장합니다.

        if(objectMemory.ContainsKey(index) == false)
        {
            objectMemory.Add(index, newObject);
        }
        else
        {
            objectMemory[index] = newObject;
        }
    }


    #endregion
    #region 카메라 포지션 변경하는 것

    // 무브
    #region 기본 위치로 옮김 : public void MoveToDefaultPosition()
    public void MoveToDefaultPosition()
    {
        if(targetObject != null)
        {
            transform.position = targetObject.transform.position + DefaultDeltaCameraAndPoint;
        }
        else
        {
            transform.position = new Vector3(0, 1, 0) + DefaultDeltaCameraAndPoint;
        }
    }
    #endregion
    #region 특정 위치로 옮김 : public void MoveToCustomPosition(Vector3 position)
    public void MoveToCustomPosition(Vector3 position)
    {
        transform.position = position;
    }
    #endregion
    #region 타겟을 기준으로 특정 각도 + 반지름으로 움직임
    public void MoveToTargetDelta(Vector3 deltaVec)
    {
        targetObject.transform.position = targetObject.transform.position + deltaVec;
    }

    #endregion



    // 줌아웃
    public void ZoomOut(Vector3 deltaPosition)
    {
        if(targetObject != null)
        {
            transform.position = targetObject.transform.position + deltaPosition;
        }
        else
        {
            Debug.Log("DEBUG_FieldCameraController.ZoomOut: 타겟 오브젝트가 없습니다.");
        }

        //Vector3 DeltaPlayerToCamera = new Vector3(0, 7, -6);

    }

    #endregion

    #endregion
}
