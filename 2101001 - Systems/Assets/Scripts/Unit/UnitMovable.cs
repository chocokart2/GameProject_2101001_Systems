// 움직일 수 있는 유닛인 경우 이 컴포넌트를 넣길 바랍니다.
// 외부 컴포넌트로부터 움직임 목록을 수정받습니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovable : MonoBehaviour
{
    
    public List<Vector3> directionList;
    float cumulativeTime; // 누적 시간
    float moveSpeed;
    bool isNeedNewVector;
    bool isUnitMovingNow;
    bool isNewListComes;
    GameManager thatGameManager;
    Transform myTransform;
    PathFinder myPathfinder;
    Vector3 pastPosition;
    Vector3 futurePosition;
    Vector3 destinationPosition;
    Vector3 currentVector;

    //Vector3 DEBUG_oldVector;

    float deltaMove; // only Using at Update Func
    // 매 움직임마다 객체를 소환하여 블럭이 존재하는지 확인하고,
    // 블럭이 존재하면 패스파인더를 통해 루트를 다시 짜도록 요청한다.

    // Start is called before the first frame update
    void Start()
    {
        deltaMove = 0.0f;
        cumulativeTime = 0.0f;
        directionList = new List<Vector3>(); // 초반에 아무것도 없음
        currentVector = new Vector3(0, 0, 0);
        thatGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        myTransform = GetComponent<Transform>();
        myPathfinder = GetComponent<PathFinder>();
        //moveSpeed = 3.0f;
        moveSpeed = 4.0f; // 움직임 리스트
        moveSpeed = 1.0f; // 움직임 리스트
        isNeedNewVector = false;
        //isNewList = false;
        isUnitMovingNow = false;
        isNewListComes = false;
        //DEBUG_oldVector = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && (directionList.Count > 0))
        {
            Debug.Log("directionList[0] : " + directionList[0]);
            if(directionList.Count > 1)
                Debug.Log("directionList[1] : " + directionList[1]);
        }

        //(currentVector != new Vector3(0, 0, 0) && deltaMove >= currentVector.magnitude


        isUnitMovingNow = (directionList.Count > 0) || (currentVector != new Vector3(0, 0, 0)); // 방향리스트에 벡터가 존재하거나, 현 벡터가 영벡터가 아니면 움직이는것으로 판단
        // 2021-04-25 16:42 예상되는 허점 없음.

        if (isUnitMovingNow) // 실패할 가능성 있음 (첫번째 리스트 처리후 다음 리스트 처리할때 부분이 잘못됨)
        {

            // 처리할 벡터가 있습니다. 1. 지금 쓰는 벡터가 있음 2. 방향리스트에 벡터가 있음
            //Debug.Log("유닛 이동 중!");

            // 목록에 뜬 벡터만 계속 처리해주면 됩니다.
            
            //if (!isNeedNewVector) // 현재 백터를 처리하는 것입니다. // 조건문이 중요하네
            if (currentVector != new Vector3(0, 0, 0)) // 현재 백터를 처리하는 것입니다. // 조건문이 중요하네
            {
                cumulativeTime += Time.deltaTime;
                deltaMove = cumulativeTime * moveSpeed;
                myTransform.position = pastPosition + currentVector.normalized * deltaMove;            

                if (deltaMove >= currentVector.magnitude) // 현재 이동중인 벡터를 다 썼습니다.
                {
                    deltaMove = 0.0f;
                    cumulativeTime = 0.0f;
                    isNeedNewVector = true;
                    myTransform.position = futurePosition;

                    //Debug.Log("다음 벡터로 넘어갑니다");
                    
                    //if(isNewListComes)
                    //{
                    //    directionList.Insert(0, directionList[0]);
                    //    isNewListComes = false;
                    //}
                    
                }
            }
            if(isNeedNewVector)
            {
                // 현 백터를 다 쓴 상태일 때, 혹은 노는 상태에서 외부입력이 들어올 때, isNeedNewVector가 활성화됩니다.
                if(directionList.Count == 0)
                {
                    currentVector = new Vector3(0, 0, 0);
                    isNeedNewVector = false;


                    Debug.Log("yee");
                }
                else if (directionList.Count > 0)// directionList.Count이 1보다 큽니다.
                {
                    Debug.Log("myTransform.position + directionList[0] " + directionList[0] + "  = " + (myTransform.position + directionList[0]));
                    if (thatGameManager.IsAbleToStepThere(myTransform.position + new Vector3(0, -1, 0) + directionList[0]))
                    {
                        currentVector = directionList[0];
                        directionList.RemoveAt(0);
                        isNeedNewVector = false;
                        //Debug.Log(currentVector);
                        pastPosition = myTransform.position;
                        futurePosition = pastPosition + currentVector;
                        Debug.Log("새 벡터를 설치했습니다 : " + currentVector);
                    }
                    else
                    {
                        // 긴급상황.
                        currentVector = new Vector3(0, 0, 0);
                        isUnitMovingNow = false;

                        bool isNewPathfindingOkay = false;
                        directionList = new List<Vector3>();
                        List<Vector3> tempDirectionList = new List<Vector3>();
                        myPathfinder.PathFinderFunction(destinationPosition.x, destinationPosition.y, myTransform.position, ref tempDirectionList, ref isNewPathfindingOkay);
                        if (directionList.Count == 0)
                        {
                            //directionList.Add(new Vector3(0, 0, 0)); // 이게 왜 있는거지?

                        }

                        Debug.Log("막혔습니다.");
                        if (isNewPathfindingOkay)
                        {
                            directionList = tempDirectionList;
                            Debug.Log("경로 발견, 다시 갑니다.");
                        }
                        // 만약 다시 찾더라도 길이 또 막히면 못찾는다고 멈추게 한다.
                    }
                }
                else
                {
                    Debug.LogError("아ㅏㅏ 응쓰겠싸아_directionList이 0보다 작습니다.");
                }


            }
            /**/
        }
    }


    public void MoveCommand(float destinationPositionX, float destinationPositionZ)
    {
        Mathf.Round(destinationPositionX);
        Mathf.Round(destinationPositionZ);
        Vector3 startPosition = new Vector3(0, 0, 0);
        destinationPosition = new Vector3(destinationPositionX, 1, destinationPositionZ);

        List<Vector3> tempDirectionList = new List<Vector3>();
        bool isFoundPath = false;

        //startPosition을 찾습니다.
        if(isUnitMovingNow) // UnitMovable 이 일하고 있습니다.
        {
            // 정상 작동
            startPosition = futurePosition;
            //Debug.Log("출발예상위치 : " + startPosition);
        }
        else // UnitMovable이 놀고 있습니다.
        {
            // 정상 작동
            //Debug.Log("놀고 있는 유닛이동");            
            startPosition = myTransform.position;
        }

        myPathfinder.PathFinderFunction(destinationPositionX, destinationPositionZ, startPosition, ref tempDirectionList, ref isFoundPath);
        
        if (isFoundPath) // 길을 찾았습니다.
        {
            directionList = tempDirectionList;
            foreach(Vector3 DbugVec in directionList)
            {
                Debug.Log("받은 벡터 : " + DbugVec);
            }
        }

        isNeedNewVector = !isUnitMovingNow;
        // 이 컴포넌트가 놀고 있으면 바로 새 백터를 끼웁니다.
        // 그렇지 않다면, 업데이트 함수에 현재 쓰고 있는 벡터를 다 쓸 때까지 기다리도록 합니다.
    }

    void RePathFinding()
    {

    }
}


//error 이슈 : 0.5 벡터
// 이동중에 클릭을 하면 1칸 더 움직이는 상황이 존재함


/*
isUnitMovingNow = (directionList.Count > 0 && (directionList[0] != new Vector3(0, 0, 0))) || (firstVecInMoveList != new Vector3(0, 0, 0));
if (isUnitMovingNow)
{
    if (isNewOrder)
    {
        Debug.Log("DEBUG_Update if(isNewOrder), firstVecInMoveList = (" + firstVecInMoveList.x + ", " + firstVecInMoveList.z + ")");
        isNewOrder = false;
        firstVecInMoveList = directionList[0];

        cumulativeTime = 0.0f;

        pastPosition = myTransform.position;
        futurePosition = pastPosition + firstVecInMoveList;
    } // 목록에서 새로운 벡터가 들어왔을때 작동합니다.
    else
    {
        //Debug.Log("i'm doing my job for my best");
        //cumulativeTime += Time.deltaTime;
        //deltaMove = cumulativeTime * moveSpeed;
        //myTransform.position = pastPosition + firstVecInMoveList.normalized * deltaMove;

        if(deltaMove >= firstVecInMoveList.magnitude)
        {
            isNewOrder = true;
            myTransform.position = futurePosition;

            // 다음 벡터를 부릅니다.
            if (directionList.Count == 2) // 남은게 하나밖에 없음.
            {
                directionList.RemoveAt(0);
                Debug.Log("DEBUG_AllCleared " + directionList[0]);
                //directionList[0] = new Vector3(0, 0, 0);
                //firstVecInMoveList = new Vector3(0, 0, 0);
                //if(directionList[0] == new Vector3(0, 0, 0))
                //{
                //    directionList[0] = new Vector3(0, 0, 0);
                //    firstVecInMoveList = new Vector3(0, 0, 0);
                //}
                //else
                //{
                //    directionList.Insert(1, new Vector3(0, 0, 0));
                //}
            }
            else if(directionList.Count > 2)
            {
                directionList.RemoveAt(0);// 다음 벡터로!
            }
            else if (directionList.Count == 1)
            {
                Debug.Log("directionList의 count이 1입니다. 저희는 이를 처리해줄 수 없습니다.");
            }
            else
            {
                Debug.LogError("directionList의 count이 0 이하입니다. 저희는 이를 처리해줄 수 없습니다.");
            }
        }
    }

} // 유닛이 움직이고 있을때 작동합니다.
/**/

//if (isUnitMovingNow == false) // 현재 움직이고 있지 않을때
//{
//    if (isNewList) // 새 목록이 떴다.
//    {
//        currentVector = directionList[0];
//        isNewList = false;
//        // 근데 0,0,0이면 어쩌지?
//    }
//}
//else // isUnitMovingNow == true
//{
//    if(isNewList) // 움직이고 있을 때, 새 목록이 떴다.
//    {
//        // 현재 처리하고 있는 벡터 먼저 처리하고,
//        // 패스파인더에게 루트를 짜도록 요청한다.
//        List<Vector3> tempDirectionList = new List<Vector3>();
//        bool isFoundPath = false;
//        myPathfinder.PathFinderFunction(destinationPosition.x, destinationPosition.z, futurePosition, ref tempDirectionList, ref isFoundPath);
//        if (isFoundPath)
//        {
//            directionList = tempDirectionList;
//        }


//        // newList를 false로
//        // 
//    }
//    else // 아마 방향리스트를 처리할 거야
//    {
//        if (isNeedNewVector)
//        {
//            //
//        }
//        else// 움직이도록 하자
//        {

//        }

//        //
//    }
//}