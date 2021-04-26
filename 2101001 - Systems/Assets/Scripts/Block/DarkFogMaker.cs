using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFogMaker : MonoBehaviour
{
    public GameObject darkFog;
    Transform myTranform;

    // Start is called before the first frame update
    void Start()
    {
        //float kart = 3.99f;
        //int pee = (int)kart; //(버림)
        //Debug.Log(pee);

        // X.4999... 까지는 



        myTranform = GetComponent<Transform>();

        int startX = Mathf.RoundToInt(myTranform.position.x - (myTranform.localScale.x / 2));
        int endX = Mathf.RoundToInt(myTranform.position.x + (myTranform.localScale.x / 2));
        int startY = Mathf.RoundToInt(myTranform.position.z - (myTranform.localScale.y / 2));
        int endY = Mathf.RoundToInt(myTranform.position.z + (myTranform.localScale.y / 2));




        // 가로
        for (int _x = startX; _x <= endX; _x++)
        {
            // 트랜스폼에선 Y라고 하지만 X축 기준으로 90도 회전했기에 유니티 좌표계에선 양의 Z축 방향입니다
            for (int _z = startY; _z <= endY; _z++)
            {
                //Vector3 locate = new Vector3(1.0f, myTranform.position.y, 1.0f);
                //locate.x = (int)(myTranform.position.x - (myTranform.localScale.x / 2)) + deltaX;
                //locate.z = (int)(myTranform.position.z - (myTranform.localScale.y / 2) + 0.49f) + deltaY;

                Instantiate(darkFog, new Vector3(_x, myTranform.position.y, _z), Quaternion.identity);


                // 검은 블럭 instantiate 함수
            }
        }
        
    }
}