// 플레이어 프리펩에 넣길 바랍니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public int SightRange;
    public LayerMask tileLayerMask; // 초당 몇 타일만큼 움
    //public float moveSpeed;
    //public Dictionary<Vector3, int> memoryMap; // 좌표
    public List<Vector3> directionList;
    Transform myTransform;

    PathFinder myPathFinder;
    UnitMovable myUnitMovable;

    List<Vector3> playerPoint; //플레이어가 지정한 경유지들입니다.
    List<Vector3> goPoint; // 패스파인더를 통해 만들어진 루트입니다.


    //List<Vector3> routePoint; // 패스파인더를 통해 만들어진 꺾는 곳들입니다.
    //List<myNode> goPoint;

    // Start is called before the first frame update
    void Start()
    {

        SightRange = 6;
        //memoryMap = new Dictionary<Vector3, int>();
        myTransform = GetComponent<Transform>();
        myPathFinder = GetComponent<PathFinder>();
        myUnitMovable = GetComponent<UnitMovable>();

        directionList = GetComponent<UnitMovable>().directionList;
        //directionList = new List<Vector3>();
        //openNodeDic = new Dictionary<Vector3, myNode>();
        //closedNodeDic = new Dictionary<Vector3, myNode>();
        //moveSpeed = 3;

    }

    // Update is called once per frame
    void Update()
    {

        // 0움직이기 위한 조작
        // 1마우스 클릭하면 카메라에서 래이를 발사합니다
        // 2레이저는 벽을 무시합니다.
        // 3레이저는 바닥에 닿으면 체크합니다.
        // 4 2,3줄은 태그를 이용해서 벽과 바닥을 구분하도록 합니다.
        // 바닥에 닿은 레이저는 그 바닥이 벽 아래 바닥인지, 그냥 바닥인지 체크합니다.

        // UIController가 등장한 버전
        // (0. UI컨트롤러가 자신이 선택한 게임오브젝트에게 컨트롤 메시지를 던집니다.)
        // 1. UI컨트롤러로부터 컨트롤 메시지를 받습니다
        // 2. 컨트롤 메시지를 받으면 움직입니다.


        /*
        if (Input.GetMouseButtonDown(1)) // 마우스 클릭을 받았습니다!
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //UnityEngine.Debug.Log("DEBUG_Update_마우스 클릭이 감지되었습니다.");

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, tileLayerMask))
            {


                float x = Mathf.RoundToInt(hit.point.x);
                float z = Mathf.RoundToInt(hit.point.z);

                UnityEngine.Debug.Log("DEBUG_Update_마우스 클릭 좌표 : " + x + " and " + z);
                MoveToTile(x, z);
            }
        }*/
    }

    void MoveToTile(float _x, float _z)
    {
        //목표: 매개변수로 알려준 목표지점을 / 알아서 / 발로 걸어가 / 최대한 가깝게 찾아갑니다.

        // 0.1 버전 : 그냥 텔레포트합니다.
        //transform.position = new Vector3(_x, 1, _z);

        // 0.2 버전 : 경로를 만들고 가능한 경로가 있으면 한칸씩 움직입니다.
        //myUnitMovable.MoveCommand(_x, _z);

        //PathFinder(_x, _z);
        //bool isPathFinderMakeit = false;
        //myPathFinder.PathFinderFunction(_x, _z, myTransform.position, ref directionList, ref isPathFinderMakeit);

        //foreach(Vector3 bois in directionList)
        //{
        //    Debug.Log("첫번째 디렉션의 방향은 (" + bois.x + ", " + bois.z + ") 입니다.");
        //    //break;
        //    //1. 예상한 이동값을 만들기
        //    //2. 업데이트에 움직이기
        //    //3. 자신의 위치가 예상한 이동값과 일치한지 확인하기 1초마다 a타일
        //}
        //if (isPathFinderMakeit)
        //{
        //    myUnitMovable.SendToUnitMovable(_x, _z, ref directionList);
        //}




        // 0.21버전 : 경로가 중간에 막히는지 여부를 판단합니다.


        //+ 두 포인트를 잇는 루트에 클릭을 하면 중간에 새 체크포인트를 집어넣는 함수를 만듭니다.
        //+
    }

    void yee()
    {



    }
}
