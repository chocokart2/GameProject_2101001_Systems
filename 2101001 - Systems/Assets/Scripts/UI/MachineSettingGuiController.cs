using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineSettingGuiController : MonoBehaviour
{

    // Field 구간입니다.

    #region GameObject
    public GameObject MainSceneButton;
    public GameObject TextBox;
    
    [Header ("MachineSetting GameObject")]

    public GameObject MachineSettingBackground;
    public GameObject MachineSettingTitle;
    public GameObject MachineSettingDeco;
    public GameObject MachineSettingButton;

    [Header("MachineSettingControl GameObject")]
    [Header("MachineSettingControlSubGUI GameObject")]
    public GameObject MachineSettingSubGuiBackground;

    public GameObject MachineSettingUpButton;
    public GameObject MachineSettingDownButton;


    public GameObject EnergyPriorityButton;
    [Header("MachineSettingNetworkConnector GameObject")]
    public GameObject NetworkConnectorSelfIcon;
    public GameObject NetworkConnectorDisconnectedIcon;
    public GameObject NetworkConnectorPartiallyConnectedIcon;
    public GameObject NetworkConnectorConnectedIcon;
    public GameObject NetworkConnectorMenu;



    #endregion
    #region TextMesh

    public TextMesh MachineSettingTitleText;

    #endregion
    #region 외부 Singleton Component


    // UI Controller
    UIController UIControllerComponent;
    FieldCameraController FieldCameraControllerComponent;

    // Camera
    Camera fieldCamera;
    Camera uiCamera;


    #endregion
    #region 특정한 함수 내에서만 사용하는 준 지역변수

    #region ArrangeIconForNetworkConnectorGUI()

    Dictionary<int, GameObject> networkClientsIcon;

    #endregion

    #endregion

    // 뒷 배경화면의 X크기와 Y크기입니다.
    float BackgroundScaleX;
    float BackgroundScaleY;

    // 외부 컴포넌트에서 머신 선택을 위한 레이캐스팅으로 충돌한 오브젝트를 들고왔습니다.
    public Collider TargetMachine;
    // 설치 / 메인 / 통신 머신들이 한 세트를 이루는데, 설치 머신이 기준을 이루기 때문입니다.
    Transform TargetPlacementMachineTransform;

    // Function 구간입니다.

    // Start is called before the first frame update
    void Awake()
    {
        #region 외부 Singleton Component SetUp
        UIControllerComponent = GameObject.Find("UIController").GetComponent<UIController>();
        FieldCameraControllerComponent = GameObject.Find("FieldCamera").GetComponent<FieldCameraController>();

        fieldCamera = GameObject.Find("FieldCamera").GetComponent<Camera>();
        uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        #endregion
        networkClientsIcon = new Dictionary<int, GameObject>();
    }


    void Start()
    {
        Debug.Log("DEBUG_MachineSettingGuiController.Start: START!");


    }

    // Update is called once per frame
    void Update()
    {
        // 열려있는 GUI에 따라 달라지는 Update에 호출되는 함수들;
        if (UIControllerComponent.openedGuiName != null)
        {
            switch (UIControllerComponent.openedGuiName)
            {
                case "MachineSettingNetworkConnector":
                    //ArrangeIconForNetworkConnectorGUI();
                    break;
                default:
                    break;
            }



        }


                //MachineSettingNetworkConnector
    }

    // DEV TIP: 이 함수는 MachineSettingGUI 인스턴스화 이후에 인스턴스의 컴포넌트에게 이 함수를 호출하도록 해주세요!;
    public void ReadyGUI(Collider collider)
    {
        if (collider == null)
        {
            // 타겟 머신이 없다는 에러
            Debug.Log("DEBUG_MachineSettingGuiController.ReadyGUI: TargetMachine이 지정되지 않아 정상적인 작동이 되지 않습니다.");
            return;
        }
        TargetMachine = collider;
        Debug.Log("DEBUG_MachineSettingGuiController.ReadyGUI: TargetMachine의 이름: " + TargetMachine.gameObject.name);
        FieldCameraControllerComponent.SaveObject(TargetMachine.gameObject, "MachineSettingGUI");

        UIControllerComponent.openedGuiName = "MachineSetting";


        TargetPlacementMachineTransform = GameObjectList.ReachPlacementMachineTransform(TargetMachine);

        Transform testTransform;
        if (TryGetPlacementMachineTransform(collider, out testTransform))
        {
            Debug.Log("DEBUG_MachineSettingGuiController.ReadyGUI: 찾았습니다! TargetPlacementMachineTransform의 이름: " + testTransform.gameObject.name);
        }


        ArrangeMainButton();


    }
    #region ReadyGUI 도와주는 함수: TryGetPlacementMachineTransform
    bool TryGetPlacementMachineTransform(Collider thatCollider, out Transform getTransform)
    {
        if(thatCollider.gameObject.GetComponent<MachinePlacement>() != null)
        {
            getTransform = thatCollider.transform;
            return true;
        }
        else if (thatCollider.name == "PlacementColliderBox")
        {
            getTransform = thatCollider.transform.parent;
            return true;
        }
        else if (thatCollider.transform.parent != null)
        {
            // 설치용 머신에 붙은 머신인 경우
            if ((thatCollider.transform.parent.name == "FixedMachinePosition") && (thatCollider.transform.parent.name == "FixedNetworkPosition"))
            {
                getTransform = thatCollider.transform.parent.parent;
                return true;
            }
            // 설치용 머신에 붙은 머신의 쿼드인경우
            if(thatCollider.gameObject.name == "Quad" && thatCollider.transform.parent.parent != null)
            {
                if ((thatCollider.transform.parent.parent.name == "FixedMachinePosition") && (thatCollider.transform.parent.parent.name == "FixedNetworkPosition"))
                {
                    getTransform = thatCollider.transform.parent.parent.parent;
                    return true;
                }
            }
        }
        getTransform = transform;
        return false;
    }
    #endregion

    //


    #region GUI Arrange: GUI 배치하기
    #region 비어있는 GUI만들기
    void CleanGUI() // 배경 빼고 모든 게임오브젝트를 삭제합니다.
    {
        //Debug.Log("DEBUG_MachineSettingGuiController_CleanGUI: 자식 오브젝트의 갯수는 다음과 같습니다. " + transform.childCount);
        for (int a = transform.childCount - 1; a > 0; a--) // 배경 빼고 자식 게임오브젝트가 남아있는동안 지웁니다.
        {
            //Debug.Log("DEBUG_MachineSettingGuiController_CleanGUI: 자식의 갯수: " + transform.childCount + " 자식 오브젝트 이름: " + transform.GetChild(0).name);
            Destroy(transform.GetChild(a).gameObject); // 첫번째 자식 게임오브젝트를 지웁니다.
        }

    }
    void CleanGUIAll() // 배경 포함 모든 게임오브젝트를 삭제합니다.
    {
        //Debug.Log("DEBUG_MachineSettingGuiController_CleanGUI: 자식 오브젝트의 갯수는 다음과 같습니다. " + transform.childCount);
        for (int a = transform.childCount - 1; a > -1; a--) // 배경 빼고 자식 게임오브젝트가 남아있는동안 지웁니다.
        {
            //Debug.Log("DEBUG_MachineSettingGuiController_CleanGUI: 자식의 갯수: " + transform.childCount + " 자식 오브젝트 이름: " + transform.GetChild(0).name);
            Destroy(transform.GetChild(a).gameObject); // 첫번째 자식 게임오브젝트를 지웁니다.
        }

    }
    #endregion

    #region 1. 메인 화면 / 수리 화면 / 컨트롤 설정 화면 UI만들어주는 함수


    void ArrangeMainButton()
    {
        // 자신보다 앞쪽에 버튼들을 세워 자식 오브젝트로 표현
        // 각 버튼들은 눌릴때마다 다른 페이지로 가게 되어 있는데, 이는 숫자로 표현합니다.

        // 최대 6개의 버튼이 존재합니다.
        //왼쪽: 설치 클래스 머신
        //중앙: 설치 클래스에 붙은 머신
        //오른쪽: 전달 클래스 머신
        //(머신이 붙어있지 않았다면, 빈 상태로 합니다)

        // 위쪽: 수리
        // 아래: 조작

        GameObject InstantiatedBackground = Instantiate(MachineSettingBackground, transform.position, Quaternion.identity);
        InstantiatedBackground.transform.parent = transform;
        InstantiatedBackground.name = "MachineSettingGuiBackground";
        BackgroundScaleX = transform.Find("MachineSettingGuiBackground").localScale.x;
        BackgroundScaleY = transform.Find("MachineSettingGuiBackground").localScale.y;


        CleanGUI();



        float positionSpacingX = BackgroundScaleX / 3;
        float positionSpacingY = BackgroundScaleY / 2;
        Vector3 baseIconPos = transform.position + new Vector3(-(BackgroundScaleX * 2 / 6), -(BackgroundScaleY / 4), -0.1f);

        for (int yAxis = 0; yAxis < 2; yAxis++)
        {
            for (int xAxis = 0; xAxis < 3; xAxis++)
            {
                // 설치한 머신이 존재하지 않으면 패스합니다.
                if (xAxis == 1)
                {
                    if (TargetPlacementMachineTransform.Find("FixedMachinePosition").childCount == 0)
                    {
                        Debug.Log("DEBUG_MachineSettingGuiController.ArrangeMainButton: 이 머신에는 추가로 설치한 메인 머신이 존재하지 않습니다.");
                        continue;
                    }
                }
                if (xAxis == 2)
                {
                    if (TargetPlacementMachineTransform.Find("FixedNetworkPosition").childCount == 0)
                    {
                        Debug.Log("DEBUG_MachineSettingGuiController.ArrangeMainButton: 이 머신에는 추가로 설치한 네트워크 머신이 존재하지 않습니다.");
                        continue;
                    }
                }


                GameObject instantiatedObject = Instantiate(MainSceneButton, baseIconPos + new Vector3(positionSpacingX * xAxis, positionSpacingY * yAxis, 0), Quaternion.identity);
                instantiatedObject.transform.localScale = new Vector3(positionSpacingX * 0.9f, positionSpacingY * 0.9f, 1.0f);
                instantiatedObject.name = "MachineSettingGui_Button_Main" + ((xAxis + 1) * 100 + yAxis + 1).ToString();
                instantiatedObject.transform.parent = transform;
            }
        }
    }
    public void ShowRepairPage(int buttonNumX) // 미완성 // buttonNumX: 왼쪽에서 오른쪽으로 설치, 머신, 통신
    {
        Debug.Log("YEE1");
        //
        CleanGUI();


        // n X n 격자들을 배경으로 보여주고
        // 격자 위에 머신 컴포넌트의 배치를 보여줌,
        // ㄴ머신 컴포넌트가 올려져 있는것을 클릭하게 되면 점검할 수 있음.



    }
    public void ShowControlPage(int buttonNumX) // buttonNumX: 왼쪽에서 오른쪽으로 1: 설치, 2: 머신, 3: 통신
    {
        // 기본적인 UI를 준비시킵니다.
        // 임시 머신 타이틀을 생성시킵니다. (연결중...)
        // 머신 클래스마다 다릅니다.
        // 해당 번호의 머신을 찾습니다.
        // 그 머신 게임오브젝트의 MachineUnitBase컴포넌트를 찾고, switch로 해당하는 머신클래스 arrange함수를 실행시킵니다.
        // - 머신의 미니 컴포넌트들의 조작 버튼들을 나열합니다.
        // - title의 이름을 변경시킵니다.
        


        Debug.Log("YEE2");

        // 빈 GUI 만들기
        CleanGUI();

        // 파라메터 준비
        float DecoRectSpace = 0.5f;
        float DecoRectThicknessHalf = 0.1f;
        float TitleHeightHalf = 0.5f;
        float CurrentCursorForButtonArrange = 0.0f; // 마지막 버튼의 아래바닥의 Y좌표입니다. 기준은 블러 배경의 맨 윗부분을 시작으로 합니다. 스케일 수준은 World와 같습니다. 맨 아래 버튼의 마진은 포함하지 않습니다.
        float ButtonToButtonMargin = 0.2f; /// 버튼과 버튼 사이에 존재하는 공간입니다.
        Vector3 UpperRightPosition = transform.position + new Vector3(-BackgroundScaleX / 2, BackgroundScaleY / 2, -0.5f); // GUI 백그라운드의 오른쪽 위의 좌표를 기록합니다.
        GameObject InstantiatedDecoRect = Instantiate(MachineSettingDeco, transform.position + new Vector3(-BackgroundScaleX / 2 + DecoRectSpace + DecoRectThicknessHalf, 0.0f, -0.5f), Quaternion.identity);
        InstantiatedDecoRect.transform.parent = transform;
        InstantiatedDecoRect.transform.localScale = new Vector3(DecoRectThicknessHalf * 2, (BackgroundScaleY - DecoRectSpace * 2), 1.0f); // (현 비율 + 빼고 싶은 값) / 현 비율

        GameObject InstantiatedTitle = Instantiate(MachineSettingTitle, transform.position + new Vector3(-BackgroundScaleX / 2 + DecoRectSpace + DecoRectThicknessHalf * 2, BackgroundScaleY / 2 - (DecoRectSpace) - (TitleHeightHalf), -0.5f), Quaternion.identity);
        InstantiatedTitle.transform.parent = transform;
        TextMesh titleTextObj = InstantiatedTitle.GetComponent<TextMesh>();
        CurrentCursorForButtonArrange = -(DecoRectSpace + TitleHeightHalf * 2 + ButtonToButtonMargin);

        //titleTextObj.text = "연결중...";

        // MachineUnitBase 컴포넌트를 참고합니다.
        //TargetPlacementMachineTransform

        MachineUnitBase TargetMachineUnitBase;
        #region 대상 머신의 MachineUnitBase 컴포넌트를 찾아 TargetMachineUnitBase에 넣습니다.
        if (buttonNumX == 1)
        {
            TargetMachineUnitBase = TargetPlacementMachineTransform.GetComponent<MachineUnitBase>();
        }
        else if(buttonNumX == 2)
        {
            TargetMachineUnitBase = TargetPlacementMachineTransform.Find("FixedMachinePosition").GetChild(0).GetComponent<MachineUnitBase>();
        }
        else
        {
            TargetMachineUnitBase = TargetPlacementMachineTransform.Find("FixedNetworkPosition").GetChild(0).GetComponent<MachineUnitBase>();
        }
        #endregion

        // title의 이름을 변경합니다.
        string titleText;
        #region titleText에 머신의 클래스 네임을 넣고, titleTextObj에 표시합니다.
        switch (TargetMachineUnitBase.MachineUnitCode)
        {
            case 0:
                titleText = "Unassigned Type or CustomType";
                break;
            case 101:
                titleText = "EnergyStorage";
                break;
            case 102:
                titleText = "EnergyTransmission";
                break;
            case 103:
                titleText = "EnergyFarm";
                break;
            case 201:
                titleText = "PlacementFixture";
                break;
            case 202:
                titleText = "PlacementDrone";
                break;
            case 203:
                titleText = "PlacementRcCar";
                break;
            case 301:
                titleText = "SensorString";
                break;
            case 302:
                titleText = "SensorCamera";
                break;
            case 501:
                titleText = "NetworkCable";
                break;
            case 502:
                titleText = "NetworkAntenna";
                break;
            case 601:
                titleText = "ControlFunction";
                break;
            case 602:
                titleText = "ControlMemory";
                break;
            case 701:
                titleText = "AttackBomb";
                break;
            case 702:
                titleText = "AttackTurret";
                break;
            case 703:
                titleText = "AttackHolder";
                break;
            case 801:
                titleText = "DisturberWall";
                break;
            case 802:
                titleText = "DisturberDummy";
                break;
            case 803:
                titleText = "DisturberLight";
                break;
            default:
                titleText = "알 수 없는 유닛 코드";
                break;
        }
        titleTextObj.text = titleText;
        #endregion

        // 버튼을 만드는 함수에 들어갈 매개변수를 준비해줍니다
        float DecoRectThickness = DecoRectThicknessHalf * 2;
        var GuiData = (DecoRectSpace, DecoRectThicknessHalf * 2, ButtonToButtonMargin, UpperRightPosition);
        //var GuiData = (DecoRectSpace, DecoRectThickness, ButtonToButtonMargin, UpperRightPosition);


        // 목표물 머신의 머신 유닛 베이스에서 컴포넌트 목록에 따라 버튼들을 나열합니다.
        if (TargetMachineUnitBase.machineComponentList.Exists(component => component.TypeName == "EnergyStorage"))
        {
            AddButtonInGUI("EnergyStorageSettingButton", "Set Battery Priority", ref CurrentCursorForButtonArrange, GuiData);
        }
        if (TargetMachineUnitBase.machineComponentList.Exists(component => component.TypeName == "TargetAimer"))
        {
            AddButtonInGUI("TargetAimerSettingButton", "Set lasor direction", ref CurrentCursorForButtonArrange, GuiData);
        }
        if (TargetMachineUnitBase.machineComponentList.Exists(component => component.TypeName == "NetworkConnector"))
        {
            AddButtonInGUI("NetworkConnectorSettingButton", "Set Network option", ref CurrentCursorForButtonArrange, GuiData);
        }




    }
    #endregion
    #region 2. GUI 제작시 도움이 되는 함수: 세팅컨트롤 "버튼 생성" 함수

    void AddButtonInGUI(string ButtonName, string ButtonText, ref float CursorY, (float DecoRectSpace, float DecoRectThickness, float ButtonToButtonMargin, Vector3 UpperRightPosition) GuiData )
    {
        float InstantiatedBatteryButtonPosX = GuiData.DecoRectSpace + GuiData.DecoRectThickness + MachineSettingButton.transform.GetChild(0).localScale.x / 2 + GuiData.ButtonToButtonMargin;
        float InstantiatedBatteryButtonPosY = CursorY - MachineSettingButton.transform.GetChild(0).localScale.y / 2;
        GameObject InstantiatedBatteryButton = Instantiate(MachineSettingButton, GuiData.UpperRightPosition + new Vector3(InstantiatedBatteryButtonPosX, InstantiatedBatteryButtonPosY, 0.0f), Quaternion.identity);

        InstantiatedBatteryButton.transform.parent = transform;
        InstantiatedBatteryButton.name = ButtonName;
        InstantiatedBatteryButton.GetComponent<TextMesh>().text = ButtonText;

        CursorY -= MachineSettingButton.transform.GetChild(0).localScale.y + GuiData.ButtonToButtonMargin;
    }

    #endregion
    #region 3. 세팅 컨트롤 버튼이 눌러졌을때 실행되는 함수. + Sub GUI의 배치를 도와주는 함수,
    public void DoButtonActionEnergyStorage()
    {
        GameObject InstantiatedBackground = Instantiate(MachineSettingSubGuiBackground, transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);
        InstantiatedBackground.transform.parent = transform;

        GameObject InstantiatedNumber = Instantiate(MachineSettingButton, transform.position + new Vector3(0.0f, 0.0f, -1.5f), Quaternion.identity);
        InstantiatedNumber.transform.parent = InstantiatedBackground.transform;
    }
    //TargetAimer
    public void DoButtonActionTargetAimer()
    {


        // 페이지를 삭제하고, UI 컨트롤러에 접근합니다
        // 버튼을 누릅니다.


        UIControllerComponent.openedGuiName = "MachineSettingControlTargetAimer";

        CleanGUIAll();
    }
    //NetworkConnector
    public void DoButtonActionNetworkConnector()
    {
        // GUI를 모두 제거합니다.
        // 네트워크 커넥팅 GUI를 엽니다.
        // 버튼들을 배치합니다
        // 채널 버튼
        // 
        // UIController Update에 양도할 일들: 연결된 유닛 표시(취소: 이것은 이 클래스의 Update 함수에 집어넣자. 굳이 여기서 처리할 일을 다른데에 처리를 시키면 일관성이 낮아진다.)
        
        // (UI: 연결된 대상 채널에 넣기, 빼기/ 선택한 채널 변경하기 / GUI 나가기)
        // ()


        CleanGUIAll();
        UIControllerComponent.openedGuiName = "MachineSettingNetworkConnector";
        FieldCameraControllerComponent.TryChangeTargetObject("MachineSettingGUI");
        FieldCameraControllerComponent.ZoomOut(new Vector3(0, 14, -12));
        //UIControllerComponent.FieldCameraZoomOut(new Vector3(0, 14, -12));

        // 

        ArrangeIconForNetworkConnectorGUI();

    }
    #region MachineSetting - NetworkConnector

    void ArrangeIconForNetworkConnectorGUI()
    {

        // 딕셔너리에 해당 아이디에 설정된 아이콘이 존재하는지 체크
        // 아니요: 아이콘이 없었거나, 새롭게 연결된 클라이언트 등장 -> 인스턴스화
        // 예: 기존 클라이언트 위치 바뀜 -> 그 유닛 아이디를 참고하여 해당 아이콘 위치 변경

        // 수정사항 1. 0번째 채널에는 연결이 끊긴 것도 카운트됩니다.
        // 해결방안 1. isClientsIdConnected에 아이디 값을 넣어봅시다.


        // 네트워크를 지정할 것.
        NetworkConnector networkConnector = TargetPlacementMachineTransform.Find("FixedNetworkPosition").GetChild(0).GetComponent<NetworkConnector>();
        if(networkConnector == null) // 네트워크 커넥터가 널값인가요
        {
            Debug.Log("<!> ERROR_MachineSettingGuiController.ArrangeIconForNetworkConnectorGUI: 컴포넌트를 찾을 수 없습니다.");
        }
        List<NetworkConnector> listNetworkConnector = new List<NetworkConnector>();
        listNetworkConnector = networkConnector.GetClients(0); // 이 네트워크 커넥터가 연결한 기록이 있는
        

        // 네트워크 커넥터가 위치하는 것에 화면 위에 띄웁니다.
        // 자신이 위치하는 것
        // 다른 네트워크 컨트롤러가 위치하는 것
        // -> 1. 충돌
        // -> 2. 게임매니저 활용
        // (선택)-> 3. 유닛 네트워크 커넥터 컴포넌트 필드 활용, 디펄트 채널에 존재하는 유닛들만.

        // 아이콘의 위치를 구하고, 아이콘의 상태를 결정하고, 해당 아이콘이 존재하는지 체크하고, 새로 만들거나 위치를 변경합니다.
        for (int index = 0; index < networkConnector.GetClients(0).Count; index++)
        {
            Debug.Log("DEBUG_MachineSettingGuiController.ArrangeIconForNetworkConnectorGUI: ICON Count " + networkConnector.GetClients(0).Count);

            // 아이콘의 위치를 구합니다.
            Vector3 iconPos = fieldCamera.WorldToScreenPoint(networkConnector.GetClients(0)[index].transform.position);
            Debug.Log("DEBUG_MachineSettingGuiController.ArrangeIconForNetworkConnectorGUI: ICON scene pos " + iconPos);
            Vector3 iconWorldPos = uiCamera.ScreenToWorldPoint(iconPos);
            Debug.Log("DEBUG_MachineSettingGuiController.ArrangeIconForNetworkConnectorGUI: ICON world pos " + iconWorldPos);
            //ScreenToWorldPoint
            // 

            // 나중에 구현할 것으로 연기됨: 아이콘 선택하기 해당 네트워크 커넥터가 현재 선택한 채널에 맞는지 찾습니다.
            // 100퍼센트 포함됨: 아이콘 >= 선택 -> 선택됨
            // 일부 포함됨: 아이콘 < 선택 -> 일부 선택됨 + 색상
            // 선택 안됨: -> 회색 아이콘
            // 자신: 자신의 아이콘

            // 해당 아이콘이 존재하는지 체크합니다.
            // 아이콘의 위치는 GUI의 자식에 위치합니다
            if(networkClientsIcon.ContainsKey(listNetworkConnector[index].GetID()) == true)
            {
                networkClientsIcon[listNetworkConnector[index].GetID()].transform.position = iconWorldPos;
            }
            else // 새로운 아이콘을 배치하세요
            {
                // 임시로 연결됨 아이콘을 사용합니다.
                GameObject instantiatedObject = Instantiate(NetworkConnectorConnectedIcon, iconWorldPos, Quaternion.identity);
                instantiatedObject.transform.parent = transform;
            }


        }
        





    }
    #endregion
    #endregion

    #region 4. SubGUI Action: GUI 안에 GUI의 버튼들의 상호작용. 
    public void ChangeNumber(int amount)
    {

    }
    #region MachineSetting - TargetAimer
    public bool SetTargetAimerTarget(Collider target)
    {
        Transform targetTransform;
        bool returnValue = TryGetPlacementMachineTransform(target, out targetTransform);
        if(returnValue)
        {
            TargetPlacementMachineTransform.Find("FixedMachinePosition").GetChild(0).GetComponent<TargetAimer>().SetConnectedPlacement(targetTransform.gameObject);
            Debug.Log("DEBUG_MachineSettingGuiController.SetTargetAimerTarget: It Worked");
        }
        return returnValue;
    }
    #endregion
    #region MachineSetting - NetworkConnector
    // 네트워크 클릭
    




    #endregion


    #endregion


    //




    //101
    void ArrangeRepairPageEnergyStorage()
    {
        
    }
    void ArrangeControlPageEnergyStorage()
    {
        
    }
    


    void ArrangeRepairPageSensorString()
    {

    }
    void ArrangeControlPageSensorString()
    {

    }
    //702
    void ArrangeRepairPageAttackTurret()
    {

    }
    void ArrangeControlPageAttackTurret()
    {
        // 배터리 관리 버튼 생성
        // 회전관절 관리 겸 터렛포 관리 버튼 생성.
        
        



    }

    #endregion
    // 리지드바디를 갖고 있는 버튼 게임오브젝트가 충돌하면 이 함수를 호출합니다.



    #region Button
    public void CallButtonFunction(string name)
    {
        Debug.Log("DEBUG_MachineSettingGuiController.CallButtonFunction: Called " + name);

        if (name.StartsWith("MachineSettingGui_Button_Main"))
        {
            int target = 0;
            if (name.StartsWith("MachineSettingGui_Button_Main1")) target = 1;
            else if (name.StartsWith("MachineSettingGui_Button_Main2")) target = 2;
            else if (name.StartsWith("MachineSettingGui_Button_Main3")) target = 3;
            if (name.EndsWith("1")) ShowRepairPage(target);
            else if (name.EndsWith("2")) ShowControlPage(target);
        }
    }

    #endregion
}
