using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 할일
// FieldCameraGo.GetComponent<Transform>() -> fieldCameraController 의 함수 이용하기.
// Update에서 Sub GUI가 열린 상태라면 ESC누를때 ExitMachineSettingGUI대신 이전 메인 GUI로 돌아가도록 만들기

public class UIController : MonoBehaviour
{
    // 플레이어의 마우스/ 키보드 입력을 받고 간단한 행동을 취하거나 외부 함수를 호출하는 역할을 맡습니다.



    public LayerMask unitLayerMask;
    public LayerMask tileLayerMask;
    public LayerMask FieldMouseInputLayerMask;
    //Debugging
    public GameObject DebugGameObject;

    public string openedGuiName;

    public bool isGuiOpenedAtField; // GUI가 켜져 있는 동안에는 다른 작동이 허용되지 않습니다.


    Camera FieldCamera;
    Camera UICamera;

    Vector3 FMI;
    Vector3 MouseLeftButtonDownFirstPositionOnField; // 마우스 드래그를 위한 장치 - ScreenPointToRay 값을 이용
    Vector3 MouseLeftButtonDownFirstPositionOnScreen; // 마우스 드래그 판정을 위한 장치 - ViewportPointToRay 값을 이용
    Vector3 MouseRightButtonDownFirstPositionOnField;
    Vector3 MouseRightButtonDownFirstPositionOnScreen; // Input.mousePosition값 이용
    Vector3 CamPosition; // 나중에 카메라가 2개라면
    bool isLeftButtonDragging; // 기본값은 false, Input.GetMouseButtonDown(1)이 일어나면 false, MouseLeftButtonDownFirstPositionOnScreen값과 차이가 심하면 true로 설정
    bool isRightButtonDragging;
    bool isTabKeyOn;
    bool isMouseOnField;
    bool isCameraDragMode;
    bool ishasBeenClicked;
    #region 변수: UnitLookAt 전용 변수
    bool isLookAtMode;
    bool isLookAtLock;
    Vector3 OriginalCameraPoint;
    Vector3 LockedDirection;
    #endregion
    #region 변수: UnitAutoAttack 전용 변수
    Vector3 FirstClickPoint2;
    Vector3 NextFrameDelta2;
    bool isDragOccurrence2;
    #endregion
    #region 변수: CameraMoveByDrag 전용 변수
    Vector3 FirstClickPoint;
    Vector3 NextFrameDelta;
    bool isDragOccurrence;
    #endregion


    GameObject selectedUnit;
    GameObject FieldCameraGo;
    UnitMovable selectedUnitMovable;


    void Awake()
    {
        openedGuiName = "none";
        isGuiOpenedAtField = false;


    }
    // Start is called before the first frame update
    void Start()
    {
        selectedUnit = null;
        selectedUnitMovable = null;

        FieldCamera = GameObject.Find("FieldCamera").GetComponent<Camera>();
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();

        

        isMouseOnField = false;
        isRightButtonDragging = false;
        isTabKeyOn = false;
        isCameraDragMode = false;
        ishasBeenClicked = false;

        FieldCameraGo = GameObject.Find("FieldCamera");



        #region 변수 초기화: UnitLookAt() 전용 변수
        isLookAtMode = true;
        isLookAtLock = false;

        #endregion



        // 임의 설정 값
        selectedUnit = GameObject.Find("HumanPlayer");

    }

    //클릭
    //
    //
    //
    //
    //


    // Update is called once per frame
    void Update() // 각 유닛이 할 수 있는 행동들에 대해서 나열합니다.
    {
        isMouseOnField = false;



        // 실시간 마우스 포인트

        // 마우스 인풋 처리 + 키보드 인풋 처리
        // 키보드 인풋 처리

        // 실시간 마우스 포인트

        // UI 카메라에 Hit하는지 판단
        // if 그렇다면 마우스 클릭은 UI에 발사
        // else 마우스 클릭은 FieldCamera에 발사


        // SUPPORT - 여기 있는 if문은 밖으로 나와 있습니다.
        // 함수 안쪽에도 조건문이 들어있고, 함수 바깥에도 조건문이 들어있으면 뭔가 일관성이 떨어집니다.
        // 해결방법 1. 함수 안쪽의 조건문을 전부 밖으로 끄집어내자. // 이렇게 되면 Update 구조가 복잡해지고 읽기가 어렵습니다. 함수는 여러번 호출될 것으로 예상됩니다.
        // 해결방법 2. 바깥 조건문을 함수 안쪽으로 집어넣어보자. // 이렇게 되면 단순 반복 작업이 예상되며, 심한경우 같은 조건이 겹쳐 여러 함수가 동시에 호출될 수도 있습니다.
        if (isGuiOpenedAtField == false && true) // UnitControl (뒷부분 true는 유닛이 살아있음 여부에 대한 불리언 값입니다. 나중에 바꾸도록 합니다.)
        {
            // 나중에 또 안쪽 if문 만들건데, 이건 선택한 유닛이 존재하는가 여부를 판단합니다.

            SelectUnit();
            SelectCancel();
            UnitMove();
            UnitSneak();
            UnitLookAt();
            UnitItemUse();
            UnitItemSelect();
            UnitSelectByKey();
            UnitNextSquad();
            UnitAuto();
            UnitNoAttack();
            UnitOpenTablet();

            CameraMoveToSelectedUnit();
            CameraMoveByDrag();
        }
        else
        {
            ExitMachineSettingGUI();
            switch (openedGuiName)
            {
                case "MachineSetting":
                    InterActWithMachineSettingGUI();
                    break;
                case "MachineSettingControlTargetAimer":
                    //유닛 움직이기
                    //
                    //ESCAPE 함수
                    UnitMove();
                    TargetAimerSelectTarget();
                    InterActWithControlTargetAimerGUI();
                    ExitMachineSettingGUI(); // -> 뒤로 가는 함수 만들것.
                    UnitLookAt();
                    break;
                case "MachineSettingNetworkConnector": // 네트워크 커넥터 컨트롤
                    ExitMachineSettingGUI();
                    // 연결된 유닛 표시하기 (위치가 매번 바뀔 수 있으므로)

                    break;
                default:
                    break;
            }
        }

        DebugPrint();

        Ray RayToUi = UICamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit RayToUiHit;
        if (Physics.Raycast(RayToUi, out RayToUiHit, 100.0f, FieldMouseInputLayerMask))
        {
            // 마우스 충돌에 의한것은 버튼들이 각자 알아서 해 주는게 좋을거 같습니다
        }
        else
        {
            Ray RayToField = FieldCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit RayToFieldHit;
            if (Physics.Raycast(RayToField, out RayToFieldHit, 100.0f, FieldMouseInputLayerMask))
            {
                isMouseOnField = true; // 마우스 컨트롤할때 이 값을 반드시 이용해주세요.
            }
        }
    }



    void FieldCameraMove()
    {
        
        // 업데이트에 있습니다
        //마우스 오른클릭 드래그 한 상태
        //
    }

    #region 유닛 선택 관련
    void SelectUnit()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("DEBUG_UIController.SelectUnit: Input Get");

            Ray ray = FieldCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50.0f, unitLayerMask))
            {
                if (hit.transform.CompareTag("Unit"))
                {
                    Debug.Log("DEBUG_UIController.SelectUnit: Unit Hit");

                    if (hit.collider.gameObject.GetComponent<UnitBase>().unitBaseData.unitType == "human")
                    {
                        Debug.Log("DEBUG_UIController.SelectUnit: humanUnit Hit : " + GameObject.Find("GameManager").GetComponent<GameManager>().playerTeam + ", " + hit.collider.gameObject.GetComponent<UnitBase>().unitBaseData.teamID);


                        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentFieldData.playerTeamName == hit.collider.gameObject.GetComponent<UnitBase>().unitBaseData.teamID) // 자신의 팀과 게임오브젝트의 팀이 똑같은지 체크
                        {
                            selectedUnit = hit.collider.gameObject;

                            FieldCameraGo.transform.position = selectedUnit.transform.position + new Vector3(0, 7, -6);
                            isCameraDragMode = false;
                        }
                    }
                    // 아래엔 머신 클래스 유닛



                    // 통신을 통해 접근할 수 있는지 확인합니다.
                    //  통제권이 있는지 확인합니다.
                    //  통제권이 있으면 현 통제권이 줄 수 있는 인풋, 아웃풋에 접근하게 됩니다
                    //      해당 디바이스가 가지고 있는 각각의 인풋, 혹은 아웃풋에 접근할 수 있는지 확인합니다.
                    //      확인이 끝났으면 플레이어는 통제권이 줄 수 있는 인풋과 아웃풋에만 접근할 수 있습니다.
                }
                else
                {
                    Debug.Log(hit.transform.tag);
                }
            }


        }//클릭시 작동



        // 부딛힌 게임오브젝트 확인




    }
    void SelectCancel() // 완성됨 (Key: ESC)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selectedUnit = null;
            isLookAtLock = false;
        }
    }
    #endregion
    #region 선택된 유닛 컨트롤
    #region GUI가 일반 상태일때나 다른 상태일 때 사용할 수 있는것.

    void UnitMove() // 완성됨 (Key: W, A, S, D)
    {
        if (selectedUnit != null)
        {
            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                Vector3 WalkDirection = new Vector3(0, 0, 0);
                if (Input.GetKey(KeyCode.W))
                {
                    WalkDirection += new Vector3(0, 0, 1);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    WalkDirection += new Vector3(-1, 0, 0);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    WalkDirection += new Vector3(0, 0, -1);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    WalkDirection += new Vector3(1, 0, 0);
                }
                selectedUnit.GetComponent<UnitBase>().Walk(WalkDirection);
            }
        }
    }
    void UnitSneak()
    {
        if (selectedUnit != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                selectedUnit.GetComponent<UnitBase>().Sneak();
            }
        }
    }
    void UnitLookAt() // 완성됨 > 이슈 발생 (Mouse 움직임 + 마우스 휠 클릭, Key: C)
                      // 이슈 1. 마우스랑 임계 거리와 비슷한 위치에 있을때 버벅거리는 현상.
                      // + 버벅거릴때 마우스를 애매한 거리에 두면 더 심해진다.
    {
        if (selectedUnit == null)
        {
            return;
        }

        // 필수 변수 조정 단계
        float CameraMoveRadus = 4.0f; // PlayerSight의 절반 가량이 적당
        Vector3 DeltaPlayerToCamera = new Vector3(0, 7, -6);
        OriginalCameraPoint = selectedUnit.transform.position + DeltaPlayerToCamera;
        float BoundaryRadAngle = 1.0f / 18.0f;
        if (Input.GetKeyDown(KeyCode.C))
        {
            isLookAtMode = !isLookAtMode;
        }

        // 조건 단계
        if (isCameraDragMode || (selectedUnit == null))
        {
            return;
        }

        // 1. 예상 카메라 위치에서 플레이어를 향한 레이를 만들고 관련한 벡터를 만듭니다
        Ray RayToPlayer = new Ray();
        RayToPlayer.origin = OriginalCameraPoint;
        RayToPlayer.direction = -DeltaPlayerToCamera;
        RaycastHit RayToPlayerHit;
        if (!Physics.Raycast(RayToPlayer, out RayToPlayerHit, 100.0f, FieldMouseInputLayerMask)) return;
        Vector3 VectorToPlayer = RayToPlayerHit.point - OriginalCameraPoint;

        // 2.
        Ray RayToMousePoint = FieldCamera.ScreenPointToRay(Input.mousePosition); // 지역변수 RayToMousePoint
        RaycastHit RayToMousePointHit;
        if (!Physics.Raycast(RayToMousePoint, out RayToMousePointHit, 100.0f, FieldMouseInputLayerMask)) return;
        Vector3 VectorToMousePoint = RayToMousePointHit.point - FieldCameraGo.transform.position;

        // 3.
        float DotValue = Vector3.Dot(VectorToPlayer, VectorToMousePoint);
        float BoundaryValue = Vector3.Magnitude(VectorToPlayer) * Vector3.Magnitude(VectorToMousePoint) * Mathf.Cos(BoundaryRadAngle * Mathf.PI);

        // 4.
        Vector3 LookDirection = VectorToMousePoint - VectorToPlayer;
        LookDirection.Normalize();

        if (DotValue < BoundaryValue) // 내적은 사이각 값이 늘어날수록 작아지는 경향이 있습니다
        {
            // 4.2.


            if (Input.GetMouseButtonDown(2))
            {
                FieldCameraGo.GetComponent<Transform>().position = OriginalCameraPoint + LookDirection * CameraMoveRadus;
                selectedUnit.GetComponent<UnitBase>().LookAtLock(LookDirection);
                isLookAtLock = true;
                LockedDirection = LookDirection;
                Debug.Log("락 모드");
            }
            else if (isLookAtMode)
            {
                if (isLookAtLock)
                {
                    FieldCameraGo.GetComponent<Transform>().position = OriginalCameraPoint + LockedDirection * CameraMoveRadus;
                }
                else
                {
                    // 4.3.
                    FieldCameraGo.GetComponent<Transform>().position = OriginalCameraPoint + LookDirection * CameraMoveRadus;

                    // 4.4.
                    selectedUnit.GetComponent<UnitBase>().LookAt(LookDirection);
                }
            }
            else
            {
                if (isLookAtLock) // 바라보기가 허용되지 않는데 락 모드
                {
                    FieldCameraGo.GetComponent<Transform>().position = OriginalCameraPoint + LockedDirection * CameraMoveRadus;
                }
                else // 락 모드도 아니고 바라보기도 허용하지 않음
                {
                    FieldCameraGo.GetComponent<Transform>().position = OriginalCameraPoint;
                    selectedUnit.GetComponent<UnitBase>().LookAtStop();
                }
            }
        }
        else
        {
            if (isLookAtLock)
            {
                FieldCameraGo.GetComponent<Transform>().position = OriginalCameraPoint + LockedDirection * CameraMoveRadus;
            }
            else
            {
                FieldCameraGo.GetComponent<Transform>().position = OriginalCameraPoint;
                selectedUnit.GetComponent<UnitBase>().LookAtStop();
            }
            if (Input.GetMouseButtonDown(2))
            {
                isLookAtLock = false;
            }
        }

        // 마우스가 갖다대는 방향대로 유닛이 몸을 틉니다.
        // 필요 조건: 카메라가 선택한 유닛을 바라보고 있는가(드래그 상태이면 꺼집니다.), 선택한 유닛이 하나만 있는가
        // 추가 필요 조건: c버튼을 누르면 멀리보기가 해제됩니다.
        // 0. 플레이어를 기준으로, UnitLookAt에 의해 움직이기 전 원래 카메라가 있을 위치를 저장해둡니다. OriginalCameraPoint
        // 1. 레이저를 쏩니다(시작: OriginalCameraPoint, 방향: 플레이어 위치, 끝: FieldMouseInputLayerMask과 부딛히는 자리) RayToPlayer이라고 명명합니다. VectorToPlayer 벡터로 만들어 저장합니다.
        // 2. 레이저를 쏩니다(시작: 카메라 위치, 방향: 마우스가 가리키는 방향대로, 끝: FieldMouseInputLayerMask과 부딛히는 자리) RayToMousePoint이라고 명명합니다. VectorToMousePoint 벡터로 만들어 저장합니다.
        // 3. 각도를 구합니다. VectorToPlayer와 VectorToMousePoint
        // 4. 만약 3번에서 어느정도 각도가 나왔다면 아래를 진행합니다. + is
        // (앞으로 미룹니다.)4.1. 플레이어를 기준으로, UnitLookAt에 의해 움직이기 전 원래 카메라가 있을 위치를 저장해둡니다. OriginalCameraPoint
        // 4.2. RayToPlayer의 끝점으로 시작하여 RayToMousePoint의 끝을 끝점으로 하는 벡터를 만들고 방향벡터로 만듭니다. LookDirection
        // 4.3. 카메라의 위치를 OriginalCameraPoint + LookDirection * CameraMoveRadus로 합니다.
        // 4.4. 함수를 호출합니다. selectedUnit.GetComponent<UnitBase>().LookAt(LookDirection);
    }
    void UnitItemUse()
    {
        bool isMouseClickedUnit = false; // false면 공격방향은 y축과 평행하게, 그렇지 않으면 클릭해 부딛힌 방향대로 날아갑니다.
        Vector3 AttackDirection; // 카메라 위치에서 클릭한 지점까지를 날로 표현한 벡터입니다.
        if (selectedUnit == null) return;
        if (Input.GetKey(KeyCode.Space)) return; // Space는 유닛 선택용 키입니다.

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
        {
            // 함수 내 지역변수 RayToMousePoint
            Ray RayToMousePoint = FieldCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit RayToMousePointHit;
            if (!Physics.Raycast(RayToMousePoint, out RayToMousePointHit, 100.0f, FieldMouseInputLayerMask)) return;
            //Debug.Log("0" + RayToMousePointHit.point);
            RaycastHit RayToMousePointHitForUnitHit;
            if (Physics.Raycast(RayToMousePoint, out RayToMousePointHitForUnitHit, 100.0f, unitLayerMask))
            {
                //Debug.DrawRay(FieldCameraGo.transform.position, RayToMousePoint, Color.cyan, 1.0f);
                Debug.Log("유닛에 맞았습니다");
                // 만약 맞은 대상이 selectedUnit이 아닌경우에만
                RayToMousePointHit = RayToMousePointHitForUnitHit;
                isMouseClickedUnit = true;



            }


            // 만약 유닛이 엎드려 엄폐로 인해 콜라이더가 아래로 내려간경우
            // 제대로 클릭했을때, 충돌 지점이 자연스럽게 아래로 내려가집니다
            // 만약 제대로 클릭하지 않은 경우 각도는 표준 방향대로 y축과 평행하게 날아가는데, 클릭이 엎드린 유닛에 만난경우 자연스럽게 아래로 내려갑니다.

            if (isMouseClickedUnit)
            {
                AttackDirection = RayToMousePointHit.point - selectedUnit.transform.position;

                //Debug.Log("1" + AttackDirection); // 작동됨
            }
            else
            {
                AttackDirection = new Vector3(
                    RayToMousePointHit.point.x - selectedUnit.transform.position.x,
                    0,
                    RayToMousePointHit.point.z - selectedUnit.transform.position.z);
                //Debug.Log("2" + AttackDirection);
                //Debug.Log("2" + RayToMousePointHit.point);
                //Debug.Log("2" + selectedUnit.transform.position);
            }
            Debug.DrawRay(selectedUnit.transform.position, AttackDirection, Color.cyan, 1.0f);
            if (Input.GetMouseButtonDown(0) && (!Input.GetKey(KeyCode.LeftShift)) && (!Input.GetKey(KeyCode.Space)))
            {
                Debug.DrawRay(FieldCameraGo.transform.position, (RayToMousePointHit.point - FieldCameraGo.transform.position), Color.red, 1.0f);

                selectedUnit.GetComponent<UnitBase>().ItemUse(AttackDirection);
                //Debug.Log("0 " + AttackDirection);
                Debug.Log("ItemUse");

            }
            else if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift)) && (!Input.GetKey(KeyCode.Space)))
            {
                selectedUnit.GetComponent<UnitBase>().ItemSkill(AttackDirection);
                Debug.Log("ItemSkill");
            }
            else if (Input.GetKeyDown(KeyCode.R) && (!Input.GetKey(KeyCode.Space)))
            {
                selectedUnit.GetComponent<UnitBase>().ItemSupply(AttackDirection);
                Debug.Log("ItemSupply");
            }
        }
    }
    #region ItemUse도와주는 함수 / 변수

    void UnitItemUseMachineGUI()
    {
        // 현재 유닛이 들고 있는 머신 클래스들을 보여주거나 숨겨주는 작업을 합니다.
        // 만약에 선택한 유닛의 아이템이 빌드 툴이고 And UI 입력이 스킬 사용이라면
        // ItemPack을 받아 머신을 표시합니다.
        // 만약 타임이 지났거나
        // OR
        // 선택한 유닛이 바뀌었거나,
        // or
        // 선택한 유닛의 운동계, 신경계가 박살난경우
        // 표시를 해제합니다.

        // 중요한 것: 지금 현재 GUI가 표시되고 있습니까


        // 호출 시점: 아마 아이템을 스킬 사용으로 결정한 순간에 이 함수를 호출시키면 됩니다.

        UnitItemPack thatInventory = selectedUnit.GetComponent<UnitItemPack>();

        if (thatInventory == null)
        {
            return; // 컴포넌트가 존재하지 않습니다.
        }
        
        if (thatInventory.inventory[thatInventory.inventoryIndex].GetItemType() == "BuildTool")
        {
            // 빌드툴이 맞습니다.
        }
        

    }


    #endregion
    void UnitItemSelect() //완성됨 (Key: 1,2,3 (` 키 안 눌렀을 때))
    {
        if (selectedUnit == null) return;
        if (Input.GetKey(KeyCode.BackQuote)) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedUnit.GetComponent<UnitBase>().ItemSelect(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedUnit.GetComponent<UnitBase>().ItemSelect(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedUnit.GetComponent<UnitBase>().ItemSelect(2);
    }
    void UnitSelectByKey()
    {
        if (selectedUnit == null) return;
        if (!Input.GetKey(KeyCode.BackQuote)) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedUnit.GetComponent<UnitBase>().Select(1);
            // 외부에서 SetSelectUnit 함수를 호출할 것.
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedUnit.GetComponent<UnitBase>().Select(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedUnit.GetComponent<UnitBase>().Select(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedUnit.GetComponent<UnitBase>().Select(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedUnit.GetComponent<UnitBase>().Select(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedUnit.GetComponent<UnitBase>().Select(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedUnit.GetComponent<UnitBase>().Select(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedUnit.GetComponent<UnitBase>().Select(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectedUnit.GetComponent<UnitBase>().Select(9);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectedUnit.GetComponent<UnitBase>().Select(10);
        }
    }
    void UnitNextSquad()// (Key: Q)
    {
        if (selectedUnit == null) return;
        if (Input.GetKeyDown(KeyCode.Q))
            selectedUnit.GetComponent<UnitBase>().NextSquad();
    }
    void UnitAuto()
    {
        if (selectedUnit == null) return;

        if (Input.GetMouseButton(1))
        {
            Ray MouseRay = FieldCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit MouseRayHit;
            if (!Physics.Raycast(MouseRay, out MouseRayHit, 100.0f, FieldMouseInputLayerMask)) return;
            NextFrameDelta2 = FieldCameraGo.transform.position - MouseRayHit.point;

            if (Input.GetMouseButtonDown(1))
            {
                FirstClickPoint2 = MouseRayHit.point;
                isDragOccurrence2 = false;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (isDragOccurrence2 == false)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(MouseRay, out hit, 100.0f, tileLayerMask)) return;
                    float x = Mathf.Round(hit.point.x);
                    float z = Mathf.Round(hit.point.z);

                    if (Input.GetKey(KeyCode.F))
                    {
                        selectedUnit.GetComponent<UnitBase>().AutoAttack(new Vector3(x, 0.0f, z));
                    }
                    else
                    {
                        selectedUnit.GetComponent<UnitBase>().AutoWalk(new Vector3(x, 0.0f, z));
                    }
                }
            }
            else
            {
                if (isDragOccurrence2 == false)
                {
                    // 아직 드래그 모드가 아니므로, NextFrameDelta가 요건을 충 주시합니다.

                    if (Vector3.Magnitude(MouseRayHit.point - FirstClickPoint2) > 0.3f)
                    {
                        isDragOccurrence2 = true;
                    }
                }
            }




            if (Input.GetKey(KeyCode.F))
            {
                // 오른클릭하면 작동
            }
        }
    }
    void UnitNoAttack()
    {
        if (selectedUnit == null) return;
        if (Input.GetKeyDown(KeyCode.X)) selectedUnit.GetComponent<UnitBase>().NoAttack();
    }
    void UnitOpenTablet() // 미완성 (Key: Tab)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (selectedUnit == null)
            {
                // just open tablet
                // 코드 추가하세요.
            }
            else
            {
                // openTablet with Inventory
                selectedUnit.GetComponent<UnitBase>().InventoryOpen();
                // 코드 추가하세요.

            }
        }


    }
    void CameraMoveToSelectedUnit()
    {
        if (selectedUnit == null) return;

        if (Input.GetKey(KeyCode.Space) && Input.GetMouseButtonDown(0))
            ishasBeenClicked = true;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (ishasBeenClicked == false)
            {
                FieldCameraGo.GetComponent<Transform>().position = selectedUnit.transform.position + new Vector3(0, 7, -6);
                isCameraDragMode = false;
            }


            ishasBeenClicked = false;
        }







        // 스페이스 키가 막 눌러질때 시작 Input.GetKey(KeyCode.Space)
        //      그동안 마우스 클릭을 받은 적이 있었는가? ishasBeenClicked; (false로 시작, Input.GetKey(KeyCode.Space)일때, 클릭 이벤트 발생 시 true, Input.GetKey(KeyCode.Space) == false일때 false로 리셋)
        //      



    }
    #endregion
    #region 표적 조준기 조작 상태일때 사용하는 것
    void TargetAimerSelectTarget()
    {
        bool isWorked = false;
        if (selectedUnit != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray RayToTarget = FieldCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit RayToTargetHit;
                if(!Physics.Raycast(RayToTarget, out RayToTargetHit, 100.0f, unitLayerMask)) { Debug.Log("TargetAimerSelectTarget 실패 - 선택되지 않은 머신"); return; }
                Ray RayToTargetFromSelectedUnit = new Ray();
                RaycastHit RayToTargetFromSelectedUnitHit;
                RayToTargetFromSelectedUnit.origin = selectedUnit.transform.position;
                RayToTargetFromSelectedUnit.direction = RayToTargetHit.point - selectedUnit.transform.position;
                if(!Physics.Raycast(RayToTargetFromSelectedUnit, out RayToTargetFromSelectedUnitHit, 100.0f, unitLayerMask)) { Debug.Log("TargetAimerSelectTarget 실패 - 플레이어가 닿지 않음"); return; }
                
                isWorked = GameObject.Find("MachineSettingGUI(Clone)").GetComponent<MachineSettingGuiController>().SetTargetAimerTarget(RayToTargetFromSelectedUnitHit.collider);
            }
        }

        // 클릭을 받는다.
        // 클릭한 대상을 향해 레이저를 발사한다.
        // 발사한 대상을 향해 



    }
    void InterActWithControlTargetAimerGUI()
    {
        // 클릭을 하면, 함수를 호출시키도록 한다.

        // 확인 버튼
        // 취소 버튼


    }


    #endregion

    #endregion
    #region 카메라 움직임
    void CameraMoveByDrag()
    {
        //Vector3 FirstClickPoint; 처음 클릭했을때 위치입니다. 드래그 하는 동안 변하지 않는 값입니다.
        //Vector3 NextFrameDelta; 마우스가 움직이고 나서


        if (Input.GetMouseButton(1))
        {
            Ray MouseRay = FieldCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit MouseRayHit;
            if (!Physics.Raycast(MouseRay, out MouseRayHit, 100.0f, FieldMouseInputLayerMask)) return;
            NextFrameDelta = FieldCameraGo.transform.position - MouseRayHit.point;

            if (Input.GetMouseButtonDown(1))
            {
                FirstClickPoint = MouseRayHit.point;
                isDragOccurrence = false;
            }
            else
            {
                if (isDragOccurrence == false)
                {
                    // 아직 드래그 모드가 아니므로, NextFrameDelta가 요건을 충 주시합니다.

                    if (Vector3.Magnitude(MouseRayHit.point - FirstClickPoint) > 0.3f)
                    {
                        isDragOccurrence = true;
                        isCameraDragMode = true;
                    }
                }
                else
                {
                    FieldCameraGo.transform.position = FirstClickPoint + NextFrameDelta;
                    // 드래그가 발생했으므로, 매 프레임마다 카메라를 옮깁니다.
                }
            }
        }

        // 추가할 기능
        // 카메라가 움직일 수 있는 범위를 설정할 것.

        /**/


        //로직
        //2.마우스 오른 버튼을 누르고, 드래그하면 반응
        //  마우스 오른 버튼을 눌렸는지 확인
        //  오른 버튼이 내려가면 기준이 시작됩니다.
        //3.첫루프
        //  항상 마우스를 통해 Ray를 발사합니다.
        //
        //isCameraDragMode를 킵니다.

    }    
    #endregion
    #region 디버거
    void DebugPrint()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            DebuggerComponent ddee = GameObject.Find("GameManager").GetComponent<DebuggerComponent>();
            ddee.PrintLog();
        }
    }
    #endregion
    #region GuiFunctionList - GUI 계열 스크립트에서 호출하는 함수 + GUI계열 스크립트를 호출하도록 하는 Input 인식 함수.
    #region MachineSettingGUI
    void InterActWithMachineSettingGUI()
    {
        // 함수 설명.
        // UI 카메라에 마우스 포인트따라 레이를 발사해, 레이와 충돌한 UI버튼에 함수를 호출하도록 합니다.
        // 만약 새로운 오브젝트에 새로운 스크립트가 추가되었다면 여기에 추가하세요.
        // 이 함수는 MachineSettingGUI가 살아있는 때에만 매번 호출되어야 합니다.



        if (Input.GetMouseButtonDown(0))
        {
            Ray RayToGui = UICamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit RayToGuiHit;
            if(!Physics.Raycast(RayToGui, out RayToGuiHit, 100.0f)) { return; }

            // 충돌체 버튼과 연관
            if(RayToGuiHit.collider.GetComponent<MachineSettingMainButton>() != null)
            {
                GameObject.Find("MachineSettingGUI(Clone)").GetComponent<MachineSettingGuiController>().CallButtonFunction(RayToGuiHit.collider.gameObject.name);
            }
            if(RayToGuiHit.collider.gameObject.name == "Quad")
            {
                // 컴포넌트 이름이 MachineSettingControlButton인경우
                if (RayToGuiHit.collider.transform.parent.GetComponent<MachineSettingControlButton>() != null)
                {
                    RayToGuiHit.collider.transform.parent.GetComponent<MachineSettingControlButton>().CallButtonFunction();
                }


            }


            Debug.Log("DEBUG_UIController: GUI와 interact가 감지되었습니다: " + RayToGuiHit.collider.gameObject.name);
        }


        // 클릭을 받으면
        // UICamera에 레이저를 발사합니다.
    }

    void ExitMachineSettingGUI()
    {
        bool NeedToStopOrCantWork = Input.GetKeyDown(KeyCode.Escape) || (false); // 두번째 False는 플레이어가 해당 GUI를 사용할 수 없는경우를 판단합니다. HumanBaseData에서 저장해둔 (행동 가능한지 여부를 판단하는) 정보를 꺼내옵니다.
        // 만약에 선택한 인간 유닛이 무력화되었다면 바로 GUI에서 나가도록 합니다.
        if (NeedToStopOrCantWork)
        {
            if(selectedUnit != null)
            {
                FieldCameraGo.transform.position = selectedUnit.transform.position + new Vector3(0, 7, -6);
            }
            openedGuiName = "none";
            isGuiOpenedAtField = false;
            GameObjectList.CloseMachineSettingGUI();
        }




    }

    #region NetworkConnector




    #endregion



    #endregion
    #region GUI에서 호출하는 퍼블릭 함수
    #region 필드 체인저

    //public void SetOpenedGuiName(string value)
    //{
    //    if(openedGuiName == null)
    //}



    #endregion
    #region 카메라 컨트롤하는 함수

    public void FieldCameraZoomOut(Vector3 deltaPosition)
    {
        // 카메라가 줌아웃한 상태입니다.
        

        // 조건: GUI가 명목상으로 오픈된 상태여야 합니다.
        if(openedGuiName == "MachineSettingNetworkConnector" && selectedUnit != null) // 만약 SelectedUnit이 null일때 이 함수를 호출시키려면 fieldCamera에서 Vector3 CameraTarget이 존재해야 합니다.
        {


            //FieldCameraGo.transform.position = selectedUnit.transform.position + deltaPosition;
        }


        //Vector3 DeltaPlayerToCamera = new Vector3(0, 7, -6);

    }
    public void FieldCameraZoomReset()
    {

    }
    #endregion

    #endregion

    #endregion





    public void SetSelectUnit(ref GameObject Go)
    {
        selectedUnit = Go;
    }

}




