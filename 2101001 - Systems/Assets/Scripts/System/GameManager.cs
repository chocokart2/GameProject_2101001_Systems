using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<Vector3, int> RealTileMap;
    //public bool isDarkFogSetNow;

    // Start is called before the first frame update
    void Start()
    {
        RealTileMap = new Dictionary<Vector3, int>();
        //isDarkFogSetNow = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTileMap(Vector3 vec3, int blockTileID)
    {
        // NOTE:
        // a. Dictionary에서 .Add 함수를 이용할때 ArgumentException을 이용한 코드 : 사용해도 괜찮음!
        // b. Dictionary에서 .TryAdd 함수를 이용한 코드 : 유니티에서 TryAdd 함수를 못 알아먹는듯.
        if (RealTileMap.ContainsKey(vec3) == true) // 자기 위치를 저장할때 : 메모리맵에 자기 위치에 해당하는 정보가 이미 있습니다.
        {
            RealTileMap.Remove(vec3);
            RealTileMap.Add(vec3, blockTileID);
            //Debug.Log("타일맵이 저장되었습니다. : " + vec3);
        }
        else // 자기 위치를 저장할때 : 신규 블럭 등록
        {
            RealTileMap.Add(vec3, blockTileID);
            //Debug.Log("타일맵이 저장되었습니다. : " + vec3 + ", " + blockTileID);// 작동함!
        }
    }

    // searchFloorTileVec3은 바닥 타일의 위치입니다. 바로 위에 좌표에 벽이 있는지도 계산합니다.
    public bool IsAbleToStepThere(ref Dictionary<Vector3, int> tileMap, Vector3 searchFloorTileVec3)
    {
        searchFloorTileVec3 = new Vector3(Mathf.Round(searchFloorTileVec3.x), Mathf.Round(searchFloorTileVec3.y), Mathf.Round(searchFloorTileVec3.z));
        Vector3 searchUpperTileVec3 = searchFloorTileVec3 + new Vector3(0, 1, 0);

        if (tileMap.ContainsKey(searchFloorTileVec3))
        {
            Debug.Log("바닥 벨류 값 : " + tileMap[searchFloorTileVec3]);
            if (tileMap[searchFloorTileVec3] != 101)
                return false;
        }
        else return false;
        if (tileMap.ContainsKey(searchUpperTileVec3))
        {
            Debug.Log("지상 벨류 값 : " + tileMap[searchUpperTileVec3]);
            if (tileMap[searchUpperTileVec3] != -1)
                return false;
        }
        return true;
    }
    public bool IsAbleToStepThere(Vector3 searchVec3)
    {
        return IsAbleToStepThere(ref RealTileMap, searchVec3);
    }
}