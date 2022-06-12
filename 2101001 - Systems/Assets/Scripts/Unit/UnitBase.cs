using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할
// 유닛의 척추!
// 유닛에 주렁주렁 달려있는 컴포넌트에 출입구 역할을 합니다.
// 이 컴포넌트의 함수를 호출하면 해당하는 연결된 컴포넌트의 메서드를 부릅니다. 없으면 말고.
// 이벤트는 사용하지 않습니다! 이벤트를 쓰면 다른 녀석들의 이벤트도 호출되기에 유일한 객체의 이벤트여야 합니다.

public class UnitBase : MonoBehaviour
{
    #region 필드

    public bool unitNeedDefaultData = true; // 만약 트루라면 디펄트 데이터를 집어넣지 않습니다.
    #region 변수 unitNeedDefaultData에 대한 설명: 전투 중 생성과 데이터로 인한 생성시 발생하는 필드 채우기에 대한 딜레마를 극복한다.
    // 만약 데이터 로딩인 경우, unitNeedDefaultData을 false로 만듭니다. 그리고 컴포넌트에 데이터가 완벽하게 들어갔으면 unitFieldFilled를 true로 만드는 작업을 합니다.
    // 이때, 디펄트 데이터가 데이터 로딩보다 일찍 들어간 경우, 컴포넌트의 필드 정보들은 데이터 로딩에 의해 덮어씌어질것이며,
    // 만약, 데이터 로딩이 일찍 들어와 디펄트 데이터가 들어가려는 경우, unitNeedDefaultData이 true이면 데이터가 들어가지만, 그렇지 않으면 이미 데이터가 들어가 있구나 하면서 데이터를 넣지 않습니다.
    #endregion


    //public int lightCountForSensor = 0;
    //public int lightCount = 0;
    public Dictionary<int, int> lightCount; // 받은 빛마다 나뉘어집니다. Key: 빛의 종류(0: 랜더링용 가시광선, 1: 적외선, 2+: 사용자 지정 광선), Value:그 빛을 닿고 있는 갯수.
    public Dictionary<string, int> sightCount; // 팀마다 나뉘어집니다. 어레이의 길이는 게임매니저의 Team 갯수만큼 결정됩니다. 키값은 Team의 Name이고, Value는 해당 팀 Sight의 닿는 갯수입니다.
    public List<string> hierarchyGameObjectNameForRendering;

    // 이 유닛이 각도에 따라 달라지는 모습이 담긴 필드입니다.
    public Material MaterialNull; // 기본형
    public Material MachineMaterialNull; // 머신 유닛 기본형
    public Material MaterialPositiveX; // 유닛이 바라보는 방향이 양의 X일때
    public Material MaterialPositiveY; // 유닛이 바라보는 방향이 양의 Y일때
    public Material MaterialNegativeX; // 유닛이 바라보는 방향이 음의 X일때
    public Material MaterialNegativeY; // 유닛이 바라보는 방향이 음의 Y일때
    bool isMaterialSetted = false;
    // 자신과 자식 게임오브젝트의 이름을 작성합니다.
    // 이때, 자식 게임오브젝트중 UnitBase를 가지고 있는 게임오브젝트는 작성하지 않아도 됩니다.
    // 자식의 자식인경우, 폴더처럼 이름/자식/자식으로, 사이에 슬래쉬를 넣어주세요
    // 원래부터 투명한 게임오브젝트는 작성하지 않습니다.
    // 이 값은 자식 게임오브젝트에 있는 childObjectOfUnit 컴포넌트에 의해 작동됩니다

    #endregion
    public UnitBaseData unitBaseData;


    #region 컴포넌트들
    MeshRenderer myMeshRenderer;
    UnitItemPack myUnitItemPack;
    UnitMovable myUnitMovable;
    HumanUnitBase myHumanUnitBase;
    GameManager gameManager;



    // 외부에서 들어옴
    //-> 함수 호출


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("UnitBase.OnTriggerEnter");
        AttackObject otherAttackObject = other.GetComponent<AttackObject>();
        if(otherAttackObject != null)
        {
            //Debug.Log("공격받았습니다!");
            

            //this.beingAttacked(other.gameObject.transform.position, otherAttackObject.myAttackClass);


        }

        //if(otherBulletController.)





    }

    public void beingAttacked(Vector3 attackerPos, GameManager.AttackClass attackClass)
    {
        Debug.Log("beingAttacked");


        // 공격을 적용하는 함수입니다.
        // 사람인 경우 -> 공격부위에 가장 가까운 기관계에 작용.
        // 공격을 받은 후, 휴먼 유닛이라면 자신이 죽었는지 체크합니다
        // 자신이 사망했는지 여부를 판단해주는 함수를 만듭니다.
        // 유닛이 사망했으면 UIController에서 SelectedUnit이 사망했던건지 체크하게 되고, 만약 그렇게 되면 모든 UI를 제거하고, isGuiOpenedAtField와 openedGuiName을 기본값으로 바꿉니다.

        switch (this.unitBaseData.unitType) // 유닛 타입에 따라 유닛의 기관계 유형을 구분짓습니다.
        {
            case "human": // 인간인 경우
                //Debug.Log("human 반응 잡힘");
                Debug.Log("DEBUG_UnitBase.BeingAttacked: 공격을 받은 유닛의 이름 - " + gameObject.name + ", 유닛의 인스턴스 아이디 - " + GetInstanceID());

                // 방향 잡기(attackerpos, 일반화)
                // 어디를 맞았는지 일일히 체크합니다
                // 걸렸다면 그곳에 화학물질을 부여합니다.
                
                // 1. 방향잡기
                Vector3 AttackDirection = new Vector3(0, 0, 0);
                AttackDirection = attackerPos - transform.position;
                AttackDirection.Normalize(); // 공격 지점과 자신의 거리가 1인 벡터로 만듭니다.

                // 2. 어디에 맞았는지 체크
                List<int> reachedOrganIndex = new List<int>(); // 닿은 OrganSystems의 Index를 기록합니다. (사실 순서는 상관없는 값입니다.)
                for(int organIndex = 0; organIndex < myHumanUnitBase.OrganSystems.Count; organIndex++) // 각 기관계마다 체크합니다.
                {
                    // <!> ERROR HUNTER <!>
                    if(myHumanUnitBase.OrganSystems[organIndex].position.Count != myHumanUnitBase.OrganSystems[organIndex].radius.Count)
                    {
                        Debug.Log("<!> ERROR_UnitBase.beingAttacked: HumanUnitBase 컴포넌트의 필드 OrganSystems의 " + (organIndex + 1) + "번째 원소의 position의 원소의 개수와 radius의 원소의 개수가 다릅니다.");
                        continue;
                    }
                    // 정상적인 루트
                    for (int posIndex = 0; posIndex < myHumanUnitBase.OrganSystems[organIndex].position.Count; posIndex++) // 이 기관계의 위치값마다 체크합니다.
                    {
                        Vector3 DeltaVector = myHumanUnitBase.OrganSystems[organIndex].position[posIndex] - AttackDirection;
                        if(DeltaVector.sqrMagnitude < Mathf.Pow(myHumanUnitBase.OrganSystems[organIndex].radius[posIndex], 2)) // 위치로 인해 기관계가 닿는 경우
                        {
                            reachedOrganIndex.Add(organIndex); // 피해를 입은 기관계 목록에 등록
                            Debug.Log("DEBUG_UnitBase.beingAttacked: 한 기관계에 닿았습니다!"); // 디버그로 알림은 덤.
                            continue; // 중복되지 않도록 다음 기관계로 넘어갑니다.
                        }
                    }
                }

                // 3. 화학물질 적용
                for(int organIndex = 0; organIndex < reachedOrganIndex.Count; organIndex++)
                {
                    // 0. 재료 준비
                    int tempValue = reachedOrganIndex[organIndex]; // 기관계 인덱스 번호
                    List<GameManager.chemical> tempChemicals = new List<GameManager.chemical>(); // 집어넣을 캐미컬
                    tempChemicals = gameManager.chemicalsMultiply(attackClass.chemicals, (1 / reachedOrganIndex.Count));

                    // 1. 물질을 넣고 화학반응을 체크합니다.
                    //myHumanUnitBase.OrganSystems[tempValue].chemicals = gameManager.MixChemical(myHumanUnitBase.OrganSystems[tempValue].chemicals, tempChemicals);
                    myHumanUnitBase.OrganSystems[tempValue].ChemicalReactionCheck(tempChemicals.ToArray());

                    // 2. 물리 데미지를 체크합니다.
                    myHumanUnitBase.OrganSystems[tempValue].PhysicalEnergyAttack(attackClass.mass * attackClass.speed, attackClass.volume);

                    // Debug. 데미지 상태 출력
                    Debug.Log(" DEBUG_UnitBase.beingAttacked: " + HumanUnitBase.HumanOrganNameList[tempValue + 1] + "의 작동비율 : " + myHumanUnitBase.OrganSystems[tempValue].OrganSystemOperatingRate);
                }

                Debug.Log("맞췄습니다!");
                


                break;
            case "machine":
                Debug.Log("준비 중");





                break;
            default:
                // <!> ERROR HUNTER <!>
                Debug.Log("<!> ERROR : UnitBase.beingAttacked: 알 수 없는 unitType입니다. 새로 case를 등록해주세요. unitBaseData.unitType " + unitBaseData.unitType);
                break;
        }
    }


    void ComponentSetup()
    {
        myMeshRenderer = transform.Find("Quad").GetComponent<MeshRenderer>();

        myUnitItemPack = null;
        myUnitItemPack = GetComponent<UnitItemPack>();
        myUnitMovable = null;
        myUnitMovable = GetComponent<UnitMovable>();
        myHumanUnitBase = null;
        myHumanUnitBase = GetComponent<HumanUnitBase>();
    }
    #endregion

    #region
    public UnitAI myUnitAI;


    #endregion



    [System.Serializable]
    public class UnitBaseData // GameManager에서 사용하는 클래스입니다. 변할 수 있는 값들을 여기에 저장합니다.(이벤트는 기본적으로 설정됨.)
    {
        public string unitType = "human"; // 소문자로 작성합니다. 두 단어 이상인 경우 두번째 단어부터 대문자를 쓰고 붙여씁니다.
        public bool isUnitTypeSet = false;
        public Vector3 direction;
        public int instanceId; // 1) instantiate할때마다 생성됩니다. 2) GameManager에서 유닛을 찾는데 이용합니다.
        public int gameManagerId;
        public string teamID; // Unassigned값은

        public UnitBaseData()
        {
            unitType = "human";
            isUnitTypeSet = false;
            direction = new Vector3(1, 0, 0);
            instanceId = -1;
            teamID = "Player";
        }
        public UnitBaseData(string _unitType)
        {
            this.unitType = _unitType;
            this.isUnitTypeSet = true;
            this.direction = new Vector3(0, 1, 0);
            this.instanceId = -1;
            teamID = "Player";
        }
        public UnitBaseData(string _unitType, int _instanceId)
        {
            this.unitType = _unitType;
            this.isUnitTypeSet = true;
            this.direction = new Vector3(0, 1, 0);
            this.instanceId = _instanceId;
            teamID = "Player";
        }
        public UnitBaseData(string _unitType, int _instanceId, string _team) : this(_unitType, _instanceId)
        {
            teamID = _team;
        }
        public UnitBaseData(GameManager.BaseUnitData unitData, string team)
        {
            unitType = unitData.unitType;
            isUnitTypeSet = true;
            direction = unitData.direction;
            instanceId = -1;
            gameManagerId = unitData.ID;
            teamID = team;
        }
    }


    //public bool isHuman = true;
    //public Vector3 direction = new Vector3(1,0,0);
    // 어느 팀에 속해 있나? -> 팀에 속해있으면 UI에서 보여집니다.
    //public int id; // instantiate할때마다 생성됩니다.

    #region 정보 삽입 메서드
    public void UnitBaseDataNewSet(string _unitType, int _id)
    {
        unitBaseData = new UnitBaseData(_unitType, _id);
    }

    #endregion

    #region 이벤트로 통제되는 유닛의 행동들
    // 1. 이벤트에 들어갈 형식을 생각해둡니다.
    public delegate void EventHandlerVoid();
    public delegate void EventHandlerVector3(Vector3 vector3);
    //public delegate void EventHandlerBool(bool boolValue);
    public delegate void EventHandlerInt(int intValue);

    // 2. 유닛들이 할 수 있는것들을 여기에 나열합니다.
    public event EventHandlerVoid UnitSneak;
    public event EventHandlerInt UnitItemSelect;
    public event EventHandlerInt UnitSelect;
    public event EventHandlerVoid UnitNextSquad;
    public event EventHandlerVector3 UnitAutoAttack;
    public event EventHandlerVoid UnitNoAttack;
    public event EventHandlerVector3 UnitAutoWalk;
    public event EventHandlerVoid UnitInventoryOpen;

    // 3. 외부 컴포넌트가 이 이벤트들의 대리자 체인에 개입할 것입니다.
    void EventSetup()
    {
        // 3. 이벤트에 비어있는 익명 메소드를 집어넣는다.
        UnitSneak += delegate () { };
        UnitItemSelect += delegate (int value) { };
        UnitSelect += delegate (int value) { };
        UnitNextSquad += delegate () { };
        UnitAutoAttack += delegate (Vector3 vector3) { };
        UnitNoAttack += delegate () { };
        UnitAutoWalk += delegate (Vector3 vector3) { };
        UnitInventoryOpen += delegate () { };
    }

    // 4. 각 이벤트를 실행시키는 public 함수를 만듭니다.
    public void Walk(Vector3 Direction)
    {
        if(myUnitMovable != null)
        {
            switch (unitBaseData.unitType)
            {
                case "human":
                    // <!> ERROR HUNTER <!>
                    if (myHumanUnitBase == null)
                    {
                        Debug.Log("<!> ERROR_UnitBase.Walk(Vector3 Direction) : 이 게임오브젝트에는 HumanUnitBase 컴포넌트가 존재하지 않습니다!");
                        break;
                    }
                    if (myHumanUnitBase.OrganSystems == null)
                    {
                        Debug.Log("<!> ERROR_UnitBase.Walk(Vector3 Direction) : HumanUnitBase의 필드 organSystems가 null값입니다.");
                        break;
                    }
                    myUnitMovable.Move(Direction, myHumanUnitBase.OrganSystems[5].OrganSystemOperatingRate);



                    break;
                default:
                    
                    break;
            }

            
        }
    }
    public void Sneak()
    {
        UnitSneak();
    }
    public void LookAt(Vector3 Direction)
    {
        //Debug.Log("Called LookAt" + Direction);
        GetComponent<GameObjectList>().UnitSightDirectionSet(Direction);

    }
    public void LookAtStop()
    {
        GetComponent<GameObjectList>().UnitSightDirectionSet(new Vector3(0, 0, 0));
        //Debug.Log("Called LookAtStop()"); // 작동함
    }
    public void LookAtLock(Vector3 Direction)
    {
        GetComponent<GameObjectList>().UnitSightDirectionSet(Direction);
    }
    public void ItemUse(Vector3 Direction)
    {
        if(myUnitItemPack != null)
        {
            myUnitItemPack.ItemUse(Direction);
        }
    }
    public void ItemSkill(Vector3 Direction)
    {
        if (myUnitItemPack != null)
        {
            myUnitItemPack.ItemSkill(Direction);
        }
    }
    public void ItemSupply(Vector3 Direction)
    {
        if (myUnitItemPack != null)
        {
            myUnitItemPack.ItemSupply(Direction);
        }
    }
    public void ItemSelect(int value)
    {
        if(myUnitItemPack != null)
        {
            myUnitItemPack.inventoryIndex = value;

        }

        //UnitItemSelect(value);
    }
    public void Select(int value)
    {
        UnitSelect(value); // GameManager의 유닛 목록에 접근하여
    }
    public void NextSquad()
    {
        UnitNextSquad();
    }
    public void AutoAttack(Vector3 Direction)
    {
        UnitAutoAttack(Direction);
    }
    public void NoAttack()
    {
        UnitNoAttack();
    }
    public void AutoWalk(Vector3 Direction)
    {
        UnitAutoWalk(Direction);
    }
    public void InventoryOpen()
    {
        UnitInventoryOpen();
    }

    //이후 UI Controller로부터 위 함수들을 호출 받으면 이벤트를 발생시킵니다.
    //이벤트가 발생하면 있는 컴포넌트는 일하고 컴포넌트가 없으면 아무 일도 일어나지 않습니다.
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // 독립적인 정보
        isMaterialSetted = (MaterialPositiveX != null) && (MaterialPositiveY != null) && (MaterialNegativeX != null) && (MaterialNegativeY != null);


        // 빛과 관계된 것은 함수가 불러질때 실행됩니다.
        //lightCount = new Dictionary<int, int>();
        //lightCount.Add(0, 0);
        //sightCount = new Dictionary<string, int>();
        //sightCount.Add("Player", 0);
        ComponentSetup();
        EventSetup();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        unitBaseData = new UnitBaseData();
        ForceSetup();


        //GameManager의 팀 값을 가져옵니다.


    }
    // Update is called once per frame
    void Update()
    {

    }

    void ForceSetup()
    {
        if(gameObject.name == "Player")
        {
            unitBaseData.teamID = "Player";
        }
    }

    // 외부에서 호출하는 함수들





    #region 빛과 시야
    #region 퍼블릭 메서드 - checkRenderingCondition, SightEnter, SightExit, LightEnter, LightExit, LightEnterForSensor, LightExitForSensor
    public void checkRenderingCondition() // 예외, 자기 팀원은 밝기에 상관없이 그대로 표시됩니다.
    {
        string myTeamName = "Player";//GameObject.Find("GameManager").GetComponent<GameManager>().playerTeam;

        // 1. lightCount가 1보다 높을 것
        // 2. 자기편 UnitSight에 닿을 것 sightCount[myTeamName] > 0을 체크
        // 이런경우 랜더링됩니다.

        bool isTeamLooking = lightCount[0] > 0 && sightCount[myTeamName] > 0;

        for (int index = 0; index < hierarchyGameObjectNameForRendering.Count; index++)
        {
            string[] nameList = hierarchyGameObjectNameForRendering[index].Split('/');
            int lastLevelCount = nameList.Length;// = int.Parse(hierarchyGameObjectNameForRendering[index][0]);
            GameObject targetObject = gameObject;

            // hierarchyGameObjectNameForRendering을 기준으로 게임오브젝트를 찾습니다.
            for (int nameListIndex = 1; nameListIndex < lastLevelCount; nameListIndex++)
            {
                targetObject = targetObject.transform.Find(nameList[nameListIndex]).gameObject;
            }

            if (targetObject.GetComponent<ChildObjectOfUnit>().isRenderingChangable == true)
            {
                targetObject.GetComponent<Renderer>().enabled = isTeamLooking;
                if (unitBaseData.teamID == gameManager.playerTeam)
                {
                    //targetObject.GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }
    public void SightEnter(string teamID)
    {
        if (sightCount == null)
        {
            sightCount = new Dictionary<string, int>();
            sightCount.Add("Player", 1);
        }
        if (sightCount.ContainsKey(teamID) == true)
        {
            sightCount[teamID] += 1;
        }
        else
        {
            sightCount.Add(teamID, 1);
        }
        if (lightCount == null)
        {
            lightCount = new Dictionary<int, int>();
            lightCount.Add(0, 0);
        }
        checkRenderingCondition();
    }
    public void SightExit(string teamID)
    {
        sightCount[teamID] -= 1;
        checkRenderingCondition();
    }
    public void LightEnter(int lightNumber)
    {
        if (sightCount == null)
        {
            sightCount = new Dictionary<string, int>();
            sightCount.Add("Player", 1);
        }
        if(lightCount == null)
        {
            lightCount = new Dictionary<int, int>();
            if(lightNumber != 0) lightCount.Add(0, 0); // 기본 빛에 대한 정보도 추가해줍니다.
        }
        if (lightCount.ContainsKey(lightNumber) == false)
        {
            lightCount.Add(lightNumber, 0); // 바로 두 줄 아래 코드에서 1 추가하고 있습니다.
        }
        lightCount[lightNumber]++; // ERROR - lightNumber 키가 없음
        checkRenderingCondition();
    }
    public void LightExit(int lightNumber)
    {
        lightCount[lightNumber]--;
        checkRenderingCondition();
    }

    #endregion

    #endregion
    #region 유닛의 각도와 외형
    #region 퍼블릭 메서드
    public void SetUnitDirection(Vector3 direction)
    {
        //Debug.Log("DEBUG_UnitBase.Setdirection: It Worked!");

        // 자신의 각도를 변경합니다.
        unitBaseData.direction = direction;
        Vector3 vecDirection = direction.normalized;

        // 자신의 외형을 변경합니다.
        if (isMaterialSetted && (myMeshRenderer != null))
        {
            //
            if(direction.z > Mathf.Cos(45 * Mathf.Deg2Rad)) // 북쪽
            {
                myMeshRenderer.material = MaterialPositiveY;
            }
            else if (direction.z < Mathf.Cos(135 * Mathf.Deg2Rad)) // 남쪽
            {
                myMeshRenderer.material = MaterialNegativeY;
            }
            else if (direction.x >= Mathf.Cos(45 * Mathf.Deg2Rad)) // 동쪽
            {
                myMeshRenderer.material = MaterialPositiveX;
            }
            else
            {
                myMeshRenderer.material = MaterialNegativeX;
            }
        }
    }
    #endregion
    #region 프라이빗 메서드

    #endregion


    #endregion


}