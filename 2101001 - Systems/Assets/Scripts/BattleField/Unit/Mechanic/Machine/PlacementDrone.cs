using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementDrone : MonoBehaviour
{
    #region field
    //public:



    //private:
    Vector3 destinationPosition; bool isStop;
    float moveSpeed;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);

        #region field init
        destinationPosition = transform.position;
        isStop = true;
        moveSpeed = 2.0f;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {


        #region Move() 보조장치
        if(isStop == false)
        {
            //만약에 현재 위치와 목표 지점이 근접해있다면 바로 자신의 위치를 목표 지점으로 옮기고, isStop을 true로 놓는다.
            if ((destinationPosition - transform.position).sqrMagnitude < 0.01f)
            {
                transform.position = destinationPosition;
                isStop = true;
            }
            else
            {
                transform.Translate((destinationPosition - transform.position).normalized * moveSpeed * Time.deltaTime); // 목적지로 가는 단위벡터 * speed * Time.deltaTime
            }
        }


        #endregion
    }



    #region [인터페이스 함수 - MachineNetMessage으로 통제될 것입니다.]
    public void Move(float x, float z)
    {
        //특정 위치로 일직선으로 움직이도록 만듭니다.
        destinationPosition = new Vector3(x, 2.5f, z);
        isStop = false;
    }
    #endregion


}
