using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAmSearchable : MonoBehaviour
{
    // 1 자신의 위치에 검은 안개 4개를 깔아놓습니다.
    // 2 근처에 지상 눈 유닛이 있으면 인접한 부분의 검은 안개를 지웁니다.
    // 자신이 벽이라면 인접한 벽면의 검은 안개를 지웁니다.(전후좌우만 인접한다고 판단합니다.)
    // 3 근처에 공중 눈 유닛이 있으면 
    /*
    bool darkFogNorthWest;
    bool darkFogNorthEast;
    bool darkFogSouthWest;
    bool darkFogSouthEast;
    */
    // Start is called before the first frame update
    void Start()
    {
        /*
        darkFogNorthWest = true;
        darkFogNorthEast = true;
        darkFogSouthWest = true;
        darkFogSouthEast = true;
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
