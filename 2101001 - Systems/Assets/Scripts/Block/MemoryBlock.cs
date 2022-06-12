// 플레이어 기억속에 존재하는 허상의 맵 타일입니다.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBlock : MonoBehaviour
{
    public bool functionA = true; // 우선권에 따라 밀려나는 함수 켜기/끄기 여부
    public float preference;
    public int blockTypeID = 0; // 외부에 의해 정의됩니다. // 패스파인더를 위한 데이터입니다
    public string blockName = "undefined";
    bool isNoBlock; // 플레이어 시야에 기억속지도가 업데이트 했는데 블럭이 없는 경우
    bool isNewMemory = true;
    bool isStartWithFogBlock = true;
    Transform myTransform;
    Vector3 positionForMemoryMap;
    Dictionary<Vector3, int> playerMemoryMap;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Hello Hamger!");

        myTransform = GetComponent<Transform>();
        positionForMemoryMap = myTransform.position;
        float fixedX = Mathf.Round(myTransform.position.x);
        float fixedY = Mathf.Round(myTransform.position.y);
        float fixedZ = Mathf.Round(myTransform.position.z);
        positionForMemoryMap = new Vector3(fixedX, fixedY, fixedZ);


        //myTransform.translate()

        playerMemoryMap = GameObject.Find("HumanPlayer").GetComponent<UnitMemory>().memoryMap;

        if (isStartWithFogBlock)
        {
            preference = 10.0f; // 저는 갓 태어난 아이에요
        }
        isNoBlock = false;

        

        SaveMapData();
    }

    // Update is called once per frame
    void Update()
    {
        if (preference > 0.0f)
        {
            preference -= Time.deltaTime; // 그리고 늙어요
        }
    }


    /// 맨 처음 새로 생성된 블럭은 뒤에 메모리 블럭이 없기 때문에 자신을 공기 취급함.

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tile")
        {
            //Debug.Log("타일 블럭과 접촉함"); // <임시주석>
            isNoBlock = false;
        }
        else if (other.tag == "Memory")
        {
            //Debug.Log("DEBUG_MemoryBlock_메모리 블럭과 접촉함");
            isNoBlock = false; // 블럭이 있구나~
            MemoryBlock otherMB = other.GetComponent<MemoryBlock>();
            
            //--
            /*
            if (this.preference < otherMB.preference) // 새 메모리 블럭으로 교체하는 코드입니다.
            {
                Debug.Log("I'm die.");
                // 만약에 데이터블럭이 정보가 있다면 여기에 새 데이터 블럭으로 정보를 넘겨주는 코드를 작성합니다.
                Destroy(gameObject); // 그리고 교체되면 죽어요.
            }/**/

        }
        else if (other.tag == "Sight")
        {
            if (isNewMemory)
            {
                isNewMemory = false;
            }
            else
            {
                //Debug.Log("I'm die." + this.myTransform.position.x + ", " + this.myTransform.position.y + ", " + this.myTransform.position.z);
                if (isNoBlock) // 뒤를 이을 메모리 블럭이 없으므로 
                {
                    SaveMapData(-1);
                }
                // 만약에 데이터블럭이 정보가 있다면 여기에 새 데이터 블럭으로 정보를 넘겨주는 코드를 작성합니다.
                Destroy(gameObject); // 그리고 교체되면 죽어요.
            }

        }
    }



    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Memory")
        {
            Transform otherTransform = GetComponent<Transform>();
            if(otherTransform.position == myTransform.position)
            {
                isNoBlock = true;
            }
        }
    }

    void SaveMapData(int _blockTypeID)        
    {
        // NOTE:
        // a. Dictionary에서 .Add 함수를 이용할때 ArgumentException을 이용한 코드 : 사용해도 괜찮음!
        // b. Dictionary에서 .TryAdd 함수를 이용한 코드 : 유니티에서 TryAdd 함수를 못 알아먹는듯.
        if (playerMemoryMap.ContainsKey(positionForMemoryMap) == true) // 자기 위치를 저장할때 : 메모리맵에 자기 위치에 해당하는 정보가 이미 있습니다.
        {
            playerMemoryMap.Remove(positionForMemoryMap);
            playerMemoryMap.Add(positionForMemoryMap, _blockTypeID);
            //Debug.Log("메모리맵이 갱신되었습니다. x:" + positionForMemoryMap.x);
        }
        else // 자기 위치를 저장할때 : 신규 블럭 등록
        {
            playerMemoryMap.Add(positionForMemoryMap, _blockTypeID);
            //Debug.Log("메모리맵이 저장되었습니다.");
        }
    }
    void SaveMapData()
    {
        SaveMapData(blockTypeID);
    }

    public void SetPreference(float _value)
    {
        preference = _value;
        isStartWithFogBlock = false;
    }
    public void BeginBlock()
    {
        isNewMemory = false;
    }

}

// 뒤에 블럭이 없는 경우 -1을 저장합니다
// 뒤에 블럭이 있는경우 시작할때의 블럭을 저장합니다.