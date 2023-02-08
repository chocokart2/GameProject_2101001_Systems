using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning 이 클래스 리펙토링 좀 해야겠다.
// 역할
// 유닛의 척추!
// 유닛에 주렁주렁 달려있는 컴포넌트에 출입구 역할을 합니다.
// 이 컴포넌트의 함수를 호출하면 해당하는 연결된 컴포넌트의 메서드를 부릅니다. 없으면 말고.
// 이벤트는 사용하지 않습니다! 이벤트를 쓰면 다른 녀석들의 이벤트도 호출되기에 유일한 객체의 이벤트여야 합니다.

#warning 작업중 :  UnitBase 코드 클린 코드로 정리하기
/// <summary>
///     유닛 게임오브젝트에 잔뜩 달려있는 컴포넌트들의 출입구 역할을 합니다.
/// </summary>
/// <remarks>
///     <para>
///         이 컴포넌트의 함수를 호출하면, 연결된 컴포넌트의 메서드를 부릅니다.</para>
///     <para>
///         비주얼 변수도 존재합니다, 이건 UnitBase의 역할이 맞나요?</para>
///     <para>
///         빛과 시야를 담당하는 함수들도 존재합니다. 이들은 다른 컴포넌트에 담도록 합니다.</para></remarks>
public class UnitBase : 
    BaseComponent, GameManager.IComponentDataIOAble<UnitBase.UnitBaseData>
    //BaseComponent.IDataGetableComponent<>
{
    #region UnitBase member
    #region field
    // public field
    /// <summary>
    ///     변수 unitNeedDefaultData에 대한 설명: 전투 중 생성과 데이터로 인한 생성시 발생하는 필드 채우기에 대한 딜레마를 극복한다.
    /// </summary>
    /// <remarks>
    ///     만약 데이터 로딩인 경우, unitNeedDefaultData을 false로 만듭니다. 그리고 컴포넌트에 데이터가 완벽하게 들어갔으면 unitFieldFilled를 true로 만드는 작업을 합니다.
    ///     이때, 디펄트 데이터가 데이터 로딩보다 일찍 들어간 경우, 컴포넌트의 필드 정보들은 데이터 로딩에 의해 덮어씌어질것이며,
    ///     만약, 데이터 로딩이 일찍 들어와 디펄트 데이터가 들어가려는 경우, unitNeedDefaultData이 true이면 데이터가 들어가지만, 그렇지 않으면 이미 데이터가 들어가 있구나 하면서 데이터를 넣지 않습니다.
    /// </remarks>
    public bool unitNeedDefaultData = true; // 만약 트루라면 디펄트 데이터를 집어넣지 않습니다.
#warning isHuman은 필요한 함수입니까? 이건 프로퍼티로 충분할 것 같습니다.
    //public bool isHuman = true;
    //public int lightCountForSensor = 0;
    //public int lightCount = 0;
    // 어느 팀에 속해 있나? -> 팀에 속해있으면 UI에서 보여집니다.
    //public int id; // instantiate할때마다 생성됩니다.

    public Dictionary<int, int> lightCount; // 받은 빛마다 나뉘어집니다. Key: 빛의 종류(0: 랜더링용 가시광선, 1: 적외선, 2+: 사용자 지정 광선), Value:그 빛을 닿고 있는 갯수.
    public Dictionary<int, int> sightCount; // 팀마다 나뉘어집니다. 어레이의 길이는 게임매니저의 Team 갯수만큼 결정됩니다. 키값은 Team의 ID이고, Value는 해당 팀 Sight의 닿는 갯수입니다.
    /// <summary>
    /// 자신과 자식 게임오브젝트의 이름을 작성합니다.
    /// </summary>
    /// <remarks>
    /// 이때, 자식 게임오브젝트중 UnitBase를 가지고 있는 게임오브젝트는 작성하지 않아도 됩니다.
    /// 자식의 자식인경우, 폴더처럼 이름/자식/자식으로, 사이에 슬래쉬를 넣어주세요
    /// 원래부터 투명한 게임오브젝트는 작성하지 않습니다.
    /// 이 값은 자식 게임오브젝트에 있는 childObjectOfUnit 컴포넌트에 의해 작동됩니다
    /// </remarks>
    public List<string> hierarchyGameObjectNameForRendering;
    
    //public Vector3 direction = new Vector3(1,0,0);
    
    public UnitBaseData unitBaseData;
    
    public UnitAI myUnitAI;
    // public property
    /// <summary>
    ///     유닛이 바라보는 방향입니다.
    /// </summary>
    public Vector3 Direction { get => m_direction; }

    // private field
    /// <summary>
    ///     유닛이 바라보는 방향입니다.
    /// </summary>
    private Vector3 m_direction;
    private GameManager gameManager;
    // 이 게임오브젝트의 컴포넌트
    private MeshRenderer m_myMeshRenderer;
    private UnitItemPack m_myUnitItemPack;
    private UnitLife m_myUnitLife;
    private UnitMovable m_myUnitMovable;
    private UnitAppearance m_myUnitAppearance;
    private BiologicalPartBase m_myBiologicalPartBase;
    private HumanUnitBase m_myHumanUnitBase;
    #region moving member
    #region 애니메이터로 옮겨야 할 부분 (먼 훗날에 할 것)
    public Material MaterialNull; // 기본형
    public Material MachineMaterialNull; // 머신 유닛 기본형
    public Material MaterialPositiveX; // 유닛이 바라보는 방향이 양의 X일때
    public Material MaterialPositiveY; // 유닛이 바라보는 방향이 양의 Y일때
    public Material MaterialNegativeX; // 유닛이 바라보는 방향이 음의 X일때
    public Material MaterialNegativeY; // 유닛이 바라보는 방향이 음의 Y일때
    bool isMaterialSetted = false;
    #endregion


    #endregion

    #endregion
    #region unity function
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
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("UnitBase.OnTriggerEnter");
        AttackObject otherAttackObject = other.GetComponent<AttackObject>();
        if(otherAttackObject != null)
        {
            //Debug.Log("공격받았습니다!");
            

            //this.beingAttacked(other.gameObject.transform.position, otherAttackObject.myAttackClass);


        }
    }
    #endregion
    #region function
    #region interface function
    public void SetData(UnitBaseData inputData)
    {
        unitBaseData = inputData;
    }
    public UnitBaseData GetData()
    {
        return unitBaseData;
    }
    #endregion
    #region public function
#warning 이 함수는 수정되어야 합니다. 그러니까 이 함수를 호출받았을 때, UnitPart로 정보를 옮겨야 합니다.
#warning (진행중) 2차 버전 : UnitPartBase의 함수를 호출합니다.
    /// <summary>
    /// 공격을 받았을 때 호출하는 함수입니다.
    /// </summary>
    /// <remarks>
    ///     이 게임에 존재하는 유닛의 종류는 다양하고, 또 모더에 의해 확장될 수 있어서, 각 Part 컴포넌트가 다형성을 이용해 각 피해 로직에 대한 구현을 가지고 있고,
    ///     이 녀석은 그냥 호출하기만 하면 됩니다. 대 저택의 여러 노동자(요리사, 청소부 등등)와 네트워킹(집사)하는 친구죠.
    ///     <para>
    ///         UnitPartBase.BeingAttacked에도 비슷한 내용의 함수가 있는 것 같은데요?
    ///     </para>
    /// </remarks>
    /// <param name="attackerDirection">
    ///     공격 방향입니다 </param>
    /// <param name="attackClass"></param>
    /// <exception cref="NotImplementedException"></exception>
    //public void beingAttacked(Vector3 attackerPosition, Vector3 attackerDirection, GameManager.AttackClass attackClass)
    public void BeingAttacked(Vector3 attackerPosition, Vector3 attackerDirection, AttackClassHelper.AttackInfo attackClass)
    {
        Hack.Say(Hack.isDebugUnitBase, Hack.check.method, this);

        

        // 하위 컴포넌트가 해야 할 일.
        // 한 Unit으로 Attack이 들어옴
        // 



        // 공격을 적용하는 함수입니다.
        // 사람인 경우 -> 공격부위에 가장 가까운 기관계에 작용.
        // 공격을 받은 후, 휴먼 유닛이라면 자신이 죽었는지 체크합니다
        // 자신이 사망했는지 여부를 판단해주는 함수를 만듭니다.
        // 유닛이 사망했으면 UIController에서 SelectedUnit이 사망했던건지 체크하게 되고, 만약 그렇게 되면 모든 UI를 제거하고, isGuiOpenedAtField와 openedGuiName을 기본값으로 바꿉니다.
        


        switch (this.unitBaseData.unitType) // 유닛 타입에 따라 유닛의 기관계 유형을 구분짓습니다.
        {
#warning 머신 유닛도 this.unitBaseData.unitType이 human으로 입력된 것으로 추론됩니다. 버그 같습니다.
            case BiologicalPartBase.Species.HUMAN: // 인간인 경우
                //Debug.Log("human 반응 잡힘");
                Debug.Log("DEBUG_UnitBase.BeingAttacked: 공격을 받은 유닛의 이름 - " + gameObject.name + ", 유닛의 인스턴스 아이디 - " + GetInstanceID());
#warning 새로운 버전
                // HumanUnitBase 값이 있을 것입니다.
                m_myHumanUnitBase.individual.DamagePart(attackerPosition, attackerDirection, attackClass);



#warning 올드 버전
//                // 방향 잡기(attackerpos, 일반화)
//                // 어디를 맞았는지 일일히 체크합니다
//                // 걸렸다면 그곳에 화학물질을 부여합니다.

//                // 1. 방향잡기
//                Vector3 AttackDirection = new Vector3(0, 0, 0);
//                AttackDirection = attackerPosition - transform.position;
//                AttackDirection.Normalize(); // 공격 지점과 자신의 거리가 1인 벡터로 만듭니다.

//                // 2. 어디에 맞았는지 체크
//                List<int> reachedOrganIndex = new List<int>(); // 닿은 OrganSystems의 Index를 기록합니다. (사실 순서는 상관없는 값입니다.)
//                for(int organIndex = 0; organIndex < m_myHumanUnitBase.individual.organParts.Length; organIndex++) // 각 기관계마다 체크합니다.
//                {
//                    // 정상적인 루트
//                    for (int posIndex = 0; posIndex < m_myHumanUnitBase.individual.organParts[organIndex].collisionRangeSphere.Length; posIndex++) // 이 기관계의 위치값마다 체크합니다.
//                    {
//                        Vector3 DeltaVector = m_myHumanUnitBase.individual.organParts[organIndex].collisionRangeSphere[posIndex].position - AttackDirection;
//                        if(DeltaVector.sqrMagnitude < Mathf.Pow(m_myHumanUnitBase.individual.organParts[organIndex].collisionRangeSphere[posIndex].radius, 2)) // 위치로 인해 기관계가 닿는 경우
//                        {
//                            reachedOrganIndex.Add(organIndex); // 피해를 입은 기관계 목록에 등록
//                            Debug.Log("DEBUG_UnitBase.beingAttacked: 한 기관계에 닿았습니다!"); // 디버그로 알림은 덤.
//                            continue; // 중복되지 않도록 다음 기관계로 넘어갑니다.
//                        }
//                    }
//                }

//                // 3. 화학물질 적용
//                for(int organIndex = 0; organIndex < reachedOrganIndex.Count; organIndex++)
//                {
//                    // 0. 재료 준비
//                    int tempValue = reachedOrganIndex[organIndex]; // 기관계 인덱스 번호
//                    List<GameManager.chemical> tempChemicals = new List<GameManager.chemical>(); // 집어넣을 캐미컬
//                    tempChemicals = gameManager.chemicalsMultiply(ref attackClass.chemicals, (1 / reachedOrganIndex.Count));

//                    // 1. 물질을 넣고 화학반응을 체크합니다.
//                    //m_myHumanUnitBase.OrganSystems[tempValue].chemicals = gameManager.MixChemical(m_myHumanUnitBase.OrganSystems[tempValue].chemicals, tempChemicals);
//#warning 아래 코드를 대체할 것을 입력하세요.
//                    //m_myHumanUnitBase.OrganSystems[tempValue].ChemicalReactionCheck(tempChemicals.ToArray());



//                    // 2. 물리 데미지를 체크합니다.
//#warning 아래 줄은 고치려고 했던 줄입니다.
//#warning 이 코드 고치기 <- 각 UnitPart마다 충돌할때 생기는 데미지 처리 함수를 만들기
//                    //m_myHumanUnitBase.OrganSystems[tempValue].PhysicalEnergyAttack(attackClass.mass * attackClass.speed, attackClass.volume);
//                    //m_myHumanUnitBase.individual.organParts[tempValue].(attackClass.mass * attackClass.speed, attackClass.volume);


//                    // Debug. 데미지 상태 출력
//                    Debug.Log(" DEBUG_UnitBase.beingAttacked: " + HumanUnitBase.HumanOrganNameList[tempValue + 1] + "의 작동비율 : " + m_myHumanUnitBase.individual.organParts[tempValue].HP);
//                }

//                Debug.Log("맞췄습니다!");
                


                break;
            case "machine":
                Debug.Log("준비 중");


                


                break;
            default:
                throw new NotImplementedException($"알 수 없는 UnitType입니다. 입력된 유닛 타입은 \"{unitBaseData.unitType}\" 입니다.");
                //break;
        }
    }
    /// <summary>
    ///     유닛의 각도를 변경합니다.
    /// </summary>
    /// <param name="direction">
    ///     유닛이 바라보는 각도입니다.</param>
    public void SetUnitDirection(Vector3 direction)
    {
        unitBaseData.direction = direction;

        if(m_myUnitAppearance != null)
        {
            m_myUnitAppearance.SetUnitDirection(direction);
        }
    }
    #endregion
    #region private function
    void ComponentSetup()
    {
        m_myMeshRenderer = transform.Find("Quad").GetComponent<MeshRenderer>();

        m_myUnitAppearance = GetComponent<UnitAppearance>();
        m_myUnitItemPack = GetComponent<UnitItemPack>();
        m_myUnitMovable = GetComponent<UnitMovable>();

        m_myBiologicalPartBase = GetComponent<BiologicalPartBase>();
        m_myHumanUnitBase = GetComponent<HumanUnitBase>();


    }
    void ForceSetup()
    {
        if(gameObject.name == "Player")
        {
            unitBaseData.teamName = "Player";
        }
    }
    #endregion
    #endregion
    #endregion
    #region Nasted Class
    [System.Serializable]
    public class UnitBaseData // GameManager에서 사용하는 클래스입니다. 변할 수 있는 값들을 여기에 저장합니다.(이벤트는 기본적으로 설정됨.)
    {
        #region 생성자 목록
        public UnitBaseData()
        {
            unitType = "human";
            direction = new Vector3(1, 0, 0);
            instanceId = -1;
            teamName = "Player";
        }
        public UnitBaseData(string _unitType)
        {
            this.unitType = _unitType;
            this.direction = new Vector3(0, 1, 0);
            this.instanceId = -1;
            teamName = "Player";
        }
        // 매개변수에 _instanceId를 사용하는 경우는 이미 존재하는 게임오브젝트를 데이터에 집어넣을 때 입니다.
        public UnitBaseData(string _unitType, int _instanceId)
        {
            this.unitType = _unitType;
            this.direction = new Vector3(0, 1, 0);
            this.instanceId = _instanceId;
            teamName = "Player";
        }
        public UnitBaseData(string _unitType, int _instanceId, string _team) : this(_unitType, _instanceId)
        {
            teamName = _team;
        }
        public UnitBaseData(GameManager.BaseUnitData unitData, string team)
        {
            unitType = unitData.unitType;
            direction = unitData.direction;
            instanceId = -1;
            //gameManagerId = unitData.ID;
            teamName = team;
        }
        #endregion

        public string prefabName;

        public string unitType;
        
        public string teamName { get; set; } // 자신을 포함하는 인스턴스가 누구인지를 가리킵니다.
        //public bool isUnitTypeSet = false;
        public Vector3 position;
        public Vector3 direction;
        public int instanceId; // 1) instantiate할때마다 생성됩니다. 2) GameManager에서 유닛을 찾는데 이용합니다.
        public int gameManagerID;

        //public int gameManagerId; // 어레이에 없어짐으로서 더이상 사용하지 않음


        #region 함수들

        




        #endregion

    }


    #endregion





    public void UnitBaseDataNewSet(string _unitType, int _id)
    {
        unitBaseData = new UnitBaseData(_unitType, _id);
    }
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
        Hack.Say(Hack.isDebugUnitBase, Hack.check.method, this);
        if(m_myUnitMovable != null)
        {
            switch (unitBaseData.unitType)
            {
                //case "creature":
                case "human":
                    // <!> ERROR HUNTER <!>
                    if (m_myHumanUnitBase == null)
                    {
                        Debug.Log("<!> ERROR_UnitBase.Walk(Vector3 Direction) : 이 게임오브젝트에는 HumanUnitBase 컴포넌트가 존재하지 않습니다!");
                        break;
                    }
                    if (m_myHumanUnitBase.individual == null)
                    {
                        Debug.Log("<!> ERROR_UnitBase.Walk(Vector3 Direction) : HumanUnitBase의 필드 individual가 null값입니다.");
                        break;
                    }

                    if(m_myHumanUnitBase != null)
                    {
                        m_myUnitMovable.Move(Direction, m_myHumanUnitBase.individual.organParts[5].wholeness);
                        Debug.Log($"DEBUG_UnitBase.Walk : wholeness = {m_myHumanUnitBase.individual.organParts[5].wholeness}");
                    }



                    break;
                default:
                    Debug.Log($"<!> ERROR_UnitBase.Walk : 알 수 없는 unitBaseData.unitType 값입니다 :{unitBaseData.unitType}");
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
        if(m_myUnitItemPack != null)
        {
            m_myUnitItemPack.ItemUse(Direction);
        }
    }
    public void ItemSkillE(Vector3 Direction)
    {
        if (m_myUnitItemPack != null)
        {
            m_myUnitItemPack.ItemSkillE(Direction);
        }
    }
    public void ItemSkillF(Vector3 Direction)
    {
        if (m_myUnitItemPack != null)
        {
            m_myUnitItemPack.ItemSkillF(Direction);
        }
    }
    public void ItemSupply(Vector3 Direction)
    {
        if (m_myUnitItemPack != null)
        {
            m_myUnitItemPack.ItemSupply(Direction);
        }
    }
    public void ItemSelect(int value)
    {
        if(m_myUnitItemPack != null)
        {
            m_myUnitItemPack.inventoryIndex = value;

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






    // 외부에서 호출하는 함수들




#warning 여기 좀 손보자.
#warning 여기 좀 손보자.
    #region 빛과 시야
    #region 퍼블릭 메서드 - checkRenderingCondition, SightEnter, SightExit, LightEnter, LightExit, LightEnterForSensor, LightExitForSensor
    public void checkRenderingCondition() // 예외, 자기 팀원은 밝기에 상관없이 그대로 표시됩니다.
    {
        int myTeamID = gameManager.currentFieldData.GetPlayerTeamID();
        //string myTeamName = "Player";//GameObject.Find("GameManager").GetComponent<GameManager>().playerTeam;

        // 1. lightCount가 1보다 높을 것
        // 2. 자기편 UnitSight에 닿을 것 sightCount[myTeamName] > 0을 체크
        // 이런경우 랜더링됩니다.

        bool isTeamLooking = lightCount[0] > 0 && sightCount[myTeamID] > 0;

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
                if (unitBaseData.teamName == gameManager.playerTeam)
                {
                    //targetObject.GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }
    /// <summary>
    /// 어떤 유닛A의 시야 범위에 이 유닛이 들어온 경우 A의 컴포넌트가 이 함수를 호출하여
    /// 이 유닛이 어떤 유닛들에게 볼 "수" 있는지 기록합니다
    /// </summary>
    /// <param name="teamID"> 이 유닛을 발견한 유닛의 teamID입니다. teamID는 월드매니저에서 구합니다.</param>
    /// 
    public void SightEnter(int teamID)
    {
        #region 함수 설명
        // 이 유닛이 어느 유닛의 시야에 들어왔는가?
        // 입력 : 이 컴포넌트를 발견한 다른 유닛의 팀 이름(아군 적군 상관없이)
        // 출력 : 없음
        // 결과 : 이 유닛을 발견한 팀 X의 멤버수()가 증가합니다.

        #endregion

        if (sightCount == null)
        {
            sightCount = new Dictionary<int, int>();
            sightCount.Add(gameManager.currentFieldData.GetPlayerTeamID(), 1);
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
    public void SightExit(int teamID)
    {
        sightCount[teamID] -= 1;
        checkRenderingCondition();
    }
    public void LightEnter(int lightNumber)
    {
        if (sightCount == null)
        {
            sightCount = new Dictionary<int, int>();
            sightCount.Add(gameManager.currentFieldData.GetPlayerTeamID(), 1);
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


}