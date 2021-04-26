// 길을 찾아 능동적으로 움직일 수 있는 유닛 프리펩에 넣는 컴포넌트입니다.
// 이 컴포넌트가 있는 프리펩에 UnitMemory라는 컴포넌트도 같이 있어야 합니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public GameObject PathFinderGuide;
    Dictionary<Vector3, int> memoryMap;
    Dictionary<Vector3, myNode> openNodeDic;
    Dictionary<Vector3, myNode> closedNodeDic;

    class myNode
    {

        public Vector3 locate; // 자기 위치
        public int moveCount; // G 이동한 갯수 이전 노드의 MoveCount의 +1
        public int deltaXZ; // H (맨헤튼 거리)
        public int F; // F값을 토대로 Sorting 될 것입니다.
        public Vector3 perv; //이전 위치
        

        public myNode(float _MyLocateX, float _MyLocateZ, float _EndLocateX, float _EndLocateZ, int _moveCount, Vector3 _perv)
        {
            locate = new Vector3(_MyLocateX, 1, _MyLocateZ);
            moveCount = _moveCount;
            deltaXZ = (int)(Mathf.Abs(_EndLocateX - _MyLocateX) + Mathf.Abs(_EndLocateZ - _MyLocateZ));
            F = moveCount + deltaXZ; // moveCount와 deltaXZ의 값을 합친 값입니다. 값이 낮을수록 최적의 노드입니다.
            perv = _perv;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //openNodeDic = new Dictionary<Vector3, myNode>();
        //closedNodeDic = new Dictionary<Vector3, myNode>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PathFinderFunction(float _x, float _z, Vector3 unitPosition, ref List<Vector3> directionList, ref bool isAbleToReach) //_x와 _z는 목표점의 transform.position의 값들입니다.
    {
        // be returned value
        //List<Vector3> 
        directionList = new List<Vector3>();

        float myTx = Mathf.Round(unitPosition.x);
        float myTz = Mathf.Round(unitPosition.z);
        Vector3 myUnitPosition = new Vector3(myTx, 1, myTz);
        openNodeDic = new Dictionary<Vector3, myNode>();
        closedNodeDic = new Dictionary<Vector3, myNode>();

        //0.1   보이는 패쓰만 만듭니다.
        //0.11  보이는 패쓰만으로도 만들 수 없는지 여부를 판단합니다.
        //0.12  알 수없는 타일만으로 갈 수 있는지 여부도 판단합니다.
        //0.2   꺾는 곳마다 패쓰포인트에 기록합니다.


        // 열린 목록

        //0.1.
        // A* 알고리즘을 사용합니다.
        // 4개의 방향으로 리스트 만들기
        // 일단 좌표만 찍고, 각 부분에 벽이 있는지 바닥이 있는지 검사합니다.
        // 만약 바닥인 경우, 그 좌표는 이용 가능한 것으로 판정됩니다.
        // 이용 가능한 좌표들중에 이동한 타일 갯수, 목표지점과 현재 타일의 Dy+Dx의 값을 각각 기록합니다.
        // 가는 길이 없는 경우 100번 블럭도 허용합니다.
        //for(int i = 0, i > 1, i++)

        bool isUnitKnowRoute = true; // 이 변수는 초반에 아는 길로 PathFind하도록 합니다.

        // Pop
        if (checkBlockForPath(new Vector3(myTx + 1, 1, myTz), false))
            openNodeDic.Add(new Vector3(myTx + 1, 1, myTz), new myNode(myTx + 1, myTz, _x, _z, 1, myUnitPosition));
        if (checkBlockForPath(new Vector3(myTx - 1, 1, myTz), false))
            openNodeDic.Add(new Vector3(myTx - 1, 1, myTz), new myNode(myTx - 1, myTz, _x, _z, 1, myUnitPosition));
        if (checkBlockForPath(new Vector3(myTx, 1, myTz + 1), false))
            openNodeDic.Add(new Vector3(myTx, 1, myTz + 1), new myNode(myTx, myTz + 1, _x, _z, 1, myUnitPosition));
        if (checkBlockForPath(new Vector3(myTx, 1, myTz - 1), false))
            openNodeDic.Add(new Vector3(myTx, 1, myTz - 1), new myNode(myTx, myTz - 1, _x, _z, 1, myUnitPosition));
        
        while (true)
        {
            // 선택된 것은 가장 낮은 값이여야 합니다!
            // F값이 가장 낮은 값을 선정 <입력 완료>
            var tempMyNodeList = new List<myNode>(openNodeDic.Values);
            //myNode tempMyNode = tempMyNodeList.Find(tempval => tempval != null); // <<<< 문제
            myNode tempMyNode = tempMyNodeList[tempMyNodeList.FindIndex(LambdaMyNode => LambdaMyNode.deltaXZ >= 0)]; // 와일 문에서 사용되는 노드.

            foreach (myNode NodeInList in new List<myNode>(openNodeDic.Values))
            {
                if (tempMyNode.F > NodeInList.F)
                {
                    tempMyNode = NodeInList;
                }
            }
            // 열린 구간에서 빼내서 닫힌 구간에 넣기
            openNodeDic.Remove(tempMyNode.locate);
            closedNodeDic.Add(tempMyNode.locate, tempMyNode);////////////////////

            //if (tempMyNodeList.FindIndex(node => node.deltaXZ == 0) != -1) // 길을 찾았습니다. 목록 중에 DeltaXZ값이 0인 노드가 있습니다.
            if (tempMyNode.deltaXZ == 0)
            {
                //Debug.Log("길을 찾았습니다!");
                // 패스파인더를 통해 만들어진 이동 목록 만들기
                // a-1 deltaXZ가 0인 노드를 찾기
                // a-2 그 노드를 이동목록의 맨 첫번째에 넣기

                // a-3 perv가 (myTx, 1, myTy)인지 확인하기
                // a-4 그 노드의 멤버 perv를 키값으로 하여 closedNodeDic에서 찾아보기
                // 이 데이터를 단순한 방향을 배열한 정보로 변환한다 디렉션어레이라고 부르자

                List<myNode> moveList = new List<myNode>();
                for (myNode pickedMyNode = tempMyNodeList[tempMyNodeList.FindIndex(node => node.deltaXZ == 0)]; true;)
                {
                    directionList.Insert(0, pickedMyNode.locate - pickedMyNode.perv);
                    if (pickedMyNode.perv == myUnitPosition) break; // 이것은 맨 마지막 노드입니다.
                    try
                    {
                        pickedMyNode = closedNodeDic[pickedMyNode.perv]; // 노드의 과거를 따라 짚어갑니다.
                        //Debug.Log("헤헤 길 찾았아요!");
                    }
                    catch (KeyNotFoundException)
                    {
                        Debug.Log("ERROR_PathFinderFunction_값을 찾을 수 없습니다!");
                        break;
                    }
                }
                isAbleToReach = true;
                break;
                // 야호 드디어 길을 찾았어요!
                // 경로를 생성합니다.

                // 이동 명령은 클릭한 지 1초 뒤에 움직이도록 합니다.
                // 아마 스레드로 움직일테니 이벤트를 쓸 수 있으면 쓸 수 있도록 합니다.
                // 이벤트 활성화가 되지 않았으면 업데이트 함수에 이동 코드가 작동하지 않음
            }
            if ((openNodeDic.Count == 0) && isUnitKnowRoute) // 초기에는 모르는 길을 선택하지 않으니 pop위쪽에 놔둠.
            {
                Debug.Log("알고 있는 길을 찾을 수 없습니다!");
                isUnitKnowRoute = false;
                //break;
            }

            // Pop(가장 낮은)
            if (checkBlockForPath(new Vector3(tempMyNode.locate.x + 1, 1, tempMyNode.locate.z), isUnitKnowRoute))
            {
                openNodeDic.Add(new Vector3(tempMyNode.locate.x + 1, 1, tempMyNode.locate.z), new myNode(tempMyNode.locate.x + 1, tempMyNode.locate.z, _x, _z, tempMyNode.moveCount + 1, tempMyNode.locate));
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x + 1, tempMyNode.moveCount, tempMyNode.locate.z), Quaternion.identity);
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x + 1, 1.5f, tempMyNode.locate.z), Quaternion.identity);
            }
            if (checkBlockForPath(new Vector3(tempMyNode.locate.x - 1, 1, tempMyNode.locate.z), isUnitKnowRoute))
            {
                openNodeDic.Add(new Vector3(tempMyNode.locate.x - 1, 1, tempMyNode.locate.z), new myNode(tempMyNode.locate.x - 1, tempMyNode.locate.z, _x, _z, tempMyNode.moveCount + 1, tempMyNode.locate));
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x - 1, tempMyNode.moveCount, tempMyNode.locate.z), Quaternion.identity);
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x + 1, 1.5f, tempMyNode.locate.z), Quaternion.identity);
            }
            if (checkBlockForPath(new Vector3(tempMyNode.locate.x, 1, tempMyNode.locate.z + 1), isUnitKnowRoute))
            {
                openNodeDic.Add(new Vector3(tempMyNode.locate.x, 1, tempMyNode.locate.z + 1), new myNode(tempMyNode.locate.x, tempMyNode.locate.z + 1, _x, _z, tempMyNode.moveCount + 1, tempMyNode.locate));
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x, tempMyNode.moveCount, tempMyNode.locate.z + 1), Quaternion.identity);
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x + 1, 1.5f, tempMyNode.locate.z), Quaternion.identity);
            }
            if (checkBlockForPath(new Vector3(tempMyNode.locate.x, 1, tempMyNode.locate.z - 1), isUnitKnowRoute))
            {
                openNodeDic.Add(new Vector3(tempMyNode.locate.x, 1, tempMyNode.locate.z - 1), new myNode(tempMyNode.locate.x, tempMyNode.locate.z - 1, _x, _z, tempMyNode.moveCount + 1, tempMyNode.locate));
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x, tempMyNode.moveCount, tempMyNode.locate.z - 1), Quaternion.identity);
                //Instantiate(PathFinderGuide, new Vector3(tempMyNode.locate.x + 1, 1.5f, tempMyNode.locate.z), Quaternion.identity);
            }


            // 길이 없는 경우

            if ((openNodeDic.Count == 0) && !isUnitKnowRoute)
            {
                Debug.LogWarning("길을 찾을 수 없습니다!");
                isAbleToReach = false;
                break;
            }
        }
    }

    bool checkBlockForPath(Vector3 _searchVec3, bool isPassDarkFog)
    {
        memoryMap = GetComponent<UnitMemory>().memoryMap;
        Vector3 parameterVec3_up = new Vector3(_searchVec3.x, 1, _searchVec3.z);
        Vector3 parameterVec3_down = new Vector3(_searchVec3.x, 0, _searchVec3.z);

        //Debug.Log("DEBUG_PathFinder_블럭 확인 : (" + _searchVec3.x + ", " + _searchVec3.z + ")");

        // 0. 열린 목록과 닫힌 목록에 존재하는지 체크합니다.
        // 1. 그 위치의 y1과 y0를 체크합니다.
        // 2. 각각이 딕셔너리에 존재하는지 체크합니다.
        // 바닥이 없으면 그 위치는 사용할 수 없습니다.
        // 지상이 있으나 없으나 상관하지 않습니다. 다만 지상이 없으면 없다고 미리 알려주는 데이터를 마련합니다.
        // 3. 딕셔너리에서 바닥이 이용 가능한 타일타입인지 확인합니다, 이때 isPassDarkFog을 확인합니다.
        // 만약 isPassDarkFog이 true이면 100과 101은 통과시켜줍니다
        // 만약 isPassDarkFog이 false이면 101만 통과시켜 줍니다
        // 4. 딕셔너리에서 지상이 있으면 지상의 값이 -1인지 확인합니다.
        // 5. 다 통과가 되면 지정받은 위치를 OpenList에 넣고 평가합니다. -> 외부에서 작동하는 부분입니다.

        foreach (myNode NodeInList in new List<myNode>(closedNodeDic.Values))
        {
            if ((_searchVec3.x == NodeInList.locate.x) && (_searchVec3.z == NodeInList.locate.z))
            {
                
                return false;
            }
        }
        foreach (myNode NodeInList in new List<myNode>(openNodeDic.Values))
        {
            if ((_searchVec3.x == NodeInList.locate.x) && (_searchVec3.z == NodeInList.locate.z))
            {
                return false;
            }
        }
        if (memoryMap.ContainsKey(parameterVec3_down))
        {
            if (isPassDarkFog)
            {
                if (!((memoryMap[parameterVec3_down] == 101) || (memoryMap[parameterVec3_down] == 100)))
                    return false;
            }
            else
            {
                if (memoryMap[parameterVec3_down] != 101)
                    return false;
            }
        }
        else
        {
            return false;
        }

        if (memoryMap.ContainsKey(parameterVec3_up))
        {
            //Debug.Log("This Block info : " + memoryMap[parameterVec3_up]);
            if (memoryMap[parameterVec3_up] != -1) // 공기가 아니라면
            {
                return false; // 공기가 아닌 지상 타일은 통과할 수 없습니다.
            }
        }
        //Instantiate(PathFinderGuide, new Vector3(_searchVec3.x, 1, _searchVec3.z), Quaternion.identity);
        return true;
    }
}




//checkBlockForPath
//  memoryMap에 102가 존재합니다. (2021-04-13)