using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTileBlockMaker : MonoBehaviour
{
    //초기에 공기 준타일 블럭을 만듭니다.
    //일단 맨 처음 네모 구간만큼 체크 루프를 돕니다
    //타일 블럭이 없으면 공기 블럭을 놓습니다
    //타일 블럭이 있으면 공기 블럭을 놓지 않습니다
    //
    //공기 블럭은 플레이어 시야에 닿으면 메모리 블럭을 만듭니다.
    Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();


        int startX = Mathf.RoundToInt(myTransform.position.x - (myTransform.localScale.x / 2));
        int endX = Mathf.RoundToInt(myTransform.position.x + (myTransform.localScale.x / 2));
        int startY = Mathf.RoundToInt(myTransform.position.z - (myTransform.localScale.y / 2));
        int endY = Mathf.RoundToInt(myTransform.position.z + (myTransform.localScale.y / 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
