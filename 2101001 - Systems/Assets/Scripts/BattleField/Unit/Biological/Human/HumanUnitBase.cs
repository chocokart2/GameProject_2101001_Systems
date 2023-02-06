using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//설명
// 필드
// 1. 기관계 클래스(BaseOrganSystem)를 저장하는 OrganSystems (List<T>)
// 1.1. 소화계
// 어떤 기관계의 부족한 화학 물질을 채워 넣습니다.
// 1.2. 순환계
// 매 1분마다 주변 기관계의 chemicalsRatioForSurvival와 chemicalsRatioForOperate를 기준으로 필요한 물질을 기관계.chemicals에 물질을 넣고 불필요한 물질들은 순환계로 배출됩니다.
// 매번의 물질교환이 일어나면 chemicalReactionCheck을 실행합니다.
// 1.3. 배출계
//
// 1.4. 감각계 
// UnitSight를 자식으로 생성하는 역할
// ㄴUnitSight는 부모의 HumanUnitBase에 정보를 전달합니다.
// 1.5. 신경계
// 유닛 통제를 할 수 있습니다.
// UnitAI와 연결합니다.
// ㄴPathFinder와 연결합니다.
// ㄴMemoryMap와 연결합니다.
// TODO - 전투 AI 컴포넌트와 연결할 수 있습니다.
// 1.6. 
// 1.+ BaseOrganSystemData
// 기관계 필드를 저장하는 클래스입니다.
// 구체적으로 Data가 더 필요하다면 파생 클래스가 만들어 질 것입니다.

// 클래스 설명
/// <summary>
/// 생물의 기관계중 인간의 기관계를 정의하고 상태를 저장합니다.
/// </summary>
public class HumanUnitBase :
    BiologicalPartBase, 
    GameManager.IComponentDataIOAble<HumanUnitBase.HumanUnitBaseData>
{
    // 필드
    public int SightRange = 6;
    /// <summary>
    /// 기관계에 대한 이름을 담고 있습니다.
    /// </summary>
    public static Dictionary<int, string> HumanOrganNameList;
    /// <summary>
    /// 개체에 대한 정보입니다
    /// </summary>
    public BioUnit individual;



    #region 유니티 함수


    void Awake()
    {
        HumanOrganNameList = new Dictionary<int, string>();
        HumanOrganNameList.Add(1, "소화계");
        HumanOrganNameList.Add(2, "순환계");
        HumanOrganNameList.Add(3, "배출계");
        HumanOrganNameList.Add(4, "감각계");
        HumanOrganNameList.Add(5, "신경계");
        HumanOrganNameList.Add(6, "운동계");
        HumanOrganNameList.Add(7, "면역계");
        HumanOrganNameList.Add(8, "합성계");
        HumanOrganNameList.Add(9, "피부계");

    }
    // Start is called before the first frame update
    void Start()
    {
        #region 1회용 필드 초기화


        //isEyeSightBooted = false;
        initiateIndividual();
        #endregion



        //unitSightGo = Instantiate(unitSightPrefab, transform.position, Quaternion.identity);
        //unitSightGo.GetComponent<UnitSight>().Init(gameObject);

        //GameObject myGo = gameObject.GetInstanceID();

        //unitSightGo.GetComponent<PlayerSightController>
    }

    // Update is called once per frame
    void Update()
    {




        //unitSightGo.GetComponent<UnitSight>().FollowingObject = gameObject;



        //unitSightGo.transform.position = transform.position; //(잘 작동함!)
        // GetInstanceID는 어려울 것 같습니다 -> 일일히 컴포넌트를 참조시키는건 어떨까

    }
    #endregion



    #region 필드
    #region 파일 입출력용 데이터
    HumanUnitBaseData humanUnitBaseData;


    #endregion

    #region 기관계 전용 프리펩/ 필드
    // 소화계 전용
    // 순환계 전용
    // 배출계 전용
    // 감각계 전용
    //public GameObject unitSightPrefab;
    //public GameObject unitSightGo; // 여기에 이벤트를 달까? // 안돼 이벤트 극혐. 이벤트는 싱글톤에만 할거야
    

    #endregion






    //public List<OrganPart> OrganSystems;
    public void initiateIndividual()
    {
        individual = DemoHumanPart.GetDemoHuman(gameObject);
    }
    public void initiateIndividual(HumanUnitBaseData data)
    {
        individual = new BioUnit();
        individual.species = data.bioUnit.species;
        individual.organParts = new OrganPart[9];

        if (data.bioUnit.organParts.Length == 9 && data.bioUnit.species == "human")
        {
            individual.organParts[0] = new DigestiveSystem(data.bioUnit.organParts[0]);
            individual.organParts[1] = new CirculatorySystem(data.bioUnit.organParts[1]);
            individual.organParts[2] = new ExcretorySystem(data.bioUnit.organParts[2]);
            individual.organParts[3] = new SensorySystem(data.bioUnit.organParts[3], GetComponent<GameObjectList>());
            individual.organParts[4] = new NervousSystem(data.bioUnit.organParts[4]);
            individual.organParts[5] = new MotorSystem(data.bioUnit.organParts[5]);
            individual.organParts[6] = new ImmuneSystem(data.bioUnit.organParts[6]);
            individual.organParts[7] = new SynthesisSystem(data.bioUnit.organParts[7]);
            individual.organParts[8] = new IntegumentarySystem(data.bioUnit.organParts[8]);
        }
        else initiateIndividual();
    }
    //BaseOrganSystemData setDefaultData()
    //{
    //    //Debug.Log("DEBUG_호출됨 : HumanUnitBase.setDefaultData()"); // (현재 작동됨) if It works -> instantiated

    //    // 기관계에 넣을 기본 데이터를 리턴하는 함수입니다.
    //    BaseOrganSystemData returnValue = new BaseOrganSystemData();
    //    returnValue.OrganSystemOperatingRate = 1.0f;
    //    returnValue.maxCellAmount = 100.0f;
    //    returnValue.cellAmount = 100.0f;
    //    returnValue.cellRecoveryRate = 1.48f;
    //    returnValue.impulseTolerance = 30.0f;
    //    // **position과 radius값은 생성자에 자동으로 입력됩니다.
    //    //returnValue.position = new List<Vector3>();
    //    //returnValue.position.CopyTo(positionList);
    //    //returnValue.radius = new List<float>();
    //    //returnValue.radius.CopyTo(radiusList); // 부딛힌 지점에서 0.2f 이내면 영향을 받습니다.

    //    //returnValue.chemicals = new GameManager.chemical[2];
    //    returnValue.chemicals = new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f) };
    //    returnValue.chemicalsRatioForSurvival = new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f) };
    //    returnValue.chemicalsRatioForOperate = new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f) };
    //    //returnValue.chemicals.CopyTo(new GameManager.chemical[] {
    //    //    new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f)
    //    //});
    //    //returnValue.chemicalsRatioForSurvival.CopyTo(new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f) }); // 요기 문제네
    //    //returnValue.chemicalsRatioForOperate.CopyTo(new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f)}); // 요기도 문제일 것으로 추정된다.


    //    //foreach (GameManager.chemical oneOfChemicals in returnValue.chemicals)
    //    //{
    //    //    Debug.Log("HumanUnitBase.setDefaultData() " + oneOfChemicals.matter); // 작동합니다.
    //    //}


    //    return returnValue;
    //}
    // 


    #endregion
    #region IComponentDataIOAble 함수
    public void SetData(HumanUnitBaseData input)
    {
        initiateIndividual(input);
    }
    public HumanUnitBaseData GetData()
    {
        HumanUnitBaseData returnValue = new HumanUnitBaseData(individual);
        return returnValue;
    }
    #endregion
    #region 함수

    private BioUnit getDefaultBioUnit()
    {
        BioUnit result = new BioUnit();

        result.species = "human";

#warning 작업중 : getDefaultOrganPart 함수 대신에 그에 맞는 부모 클래스를 호출해야 합니다.
        AddElementArray(ref result.organParts, new DigestiveSystem(DemoHumanPart.GetDemoDigestiveSystem()));
        //AddElementArray(ref result.organParts, new DigestiveSystem(getDefaultOrganPart()));
#warning 매개변수에 게임매니저 컴포넌트를 넣을 수도 있습니다.
        AddElementArray(ref result.organParts, new CirculatorySystem(DemoHumanPart.GetDemoCirculatorySystem()));
        AddElementArray(ref result.organParts, new ExcretorySystem(getDefaultOrganPart()));
        AddElementArray(ref result.organParts, new SensorySystem(getDefaultOrganPart(), GetComponent<GameObjectList>()));
        AddElementArray(ref result.organParts, new NervousSystem(getDefaultOrganPart()));
        AddElementArray(ref result.organParts, new MotorSystem(getDefaultOrganPart()));
        AddElementArray(ref result.organParts, new ImmuneSystem(getDefaultOrganPart()));
        AddElementArray(ref result.organParts, new SynthesisSystem(getDefaultOrganPart()));
        AddElementArray(ref result.organParts, new IntegumentarySystem(getDefaultOrganPart()));

        return result;
    }
#warning 언젠간 삭제할 코드
    /// <summary>
    /// [임시] OrganPart의 필드를 임시로 정의합니다.
    /// </summary>
    /// <remarks>
    ///     [임시] 코드라 약간의 하드 코딩이 있습니다.
    /// </remarks>
    /// <returns></returns>
    private OrganPart getDefaultOrganPart()
    {
        OrganPart result = DemoBiologyPart.GetDemoOrganPart();

        //result.tagged = new ChemicalHelper.Chemicals(
        //    new ChemicalHelper.Chemical() { Name = "TEST_organicCarbon", Quantity = 20.0f },
        //    new ChemicalHelper.Chemical() { Name = "TEST_ATP", Quantity = 10.0f }
        //    );
        //result.demand = new ChemicalHelper.Chemicals(
        //    new ChemicalHelper.Chemical() { Name = "TEST_organicCarbon", Quantity = 20.0f },
        //    new ChemicalHelper.Chemical() { Name = "TEST_ATP", Quantity = 10.0f }
        //    );

        //result.Name = "이름없음";
        //result.RecoveryRate = 1.04f;
        //result.maxHP = 100.0f;
        //result.HP = 100.0f;

        return result;
    }



    #endregion

    // 공격 받으면 작동하는 함수



    // 기관계에 대한 기본 정의

    //

    #region [데이터 입출력용 클래스] HumanUnitBaseData, BaseOrganSystemData
    /// <summary>
    /// 데이터 입출력 용 클래스입니다.
    /// </summary>
    [System.Serializable]
    public class HumanUnitBaseData
    {
        public BioUnit bioUnit;

        public HumanUnitBaseData() { }
        public HumanUnitBaseData(BioUnit _inputBioUnit) // baseOrganSystems -> organList
        {
            bioUnit.species = "human";
            bioUnit.organParts.CopyTo(_inputBioUnit.organParts, 0);
        }
    }
    [System.Serializable]
    public class BaseOrganSystemData
    {
        public string TypeName; // GameManager에서 이 값을 토대로 OrganSystem에 .Add(new TypeName(GameManager, InputData)); 할 것입니다.
        public float OrganSystemOperatingRate;
        public float maxCellAmount;
        public float cellAmount;
        public float cellRecoveryRate;
        public float impulseTolerance;
        //public Vector3[] position;
        //public float[] radius;


        //public ChemicalHelper.Chemicals chemicals; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
        ///// <summary>
        /// 이 Organ이 제대로 작동하기 위해 요구하는 Chemicals 입니다.
        /// </summary>
        //public ChemicalHelper.Chemicals requiringChemicals; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
        //public BaseOrganSystemData()
        //{
        //    //position = new List<Vector3>();
        //    //radius = new List<float>();
        //    chemicals = new List<GameManager.chemical>();
        //    chemicalsRatioForSurvival = new List<GameManager.chemical>();
        //    chemicalsRatioForOperate = new List<GameManager.chemical>();
        //}
    }
    #endregion

    // 소화계
    public class DigestiveSystem : OrganPart
    {
        public DigestiveSystem() : base()
        {
            Name = "DigestiveSystem";

            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(-0.2f, 0.4f, -1.0f), radius = 0.3f },
                new Sphere() { position = new Vector3(0.2f, 0.4f, -1.0f), radius = 0.3f }
                );
        }
        public DigestiveSystem(OrganPart data) : base(data)
        {
            Name = "DigestiveSystem";

            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(-0.2f, 0.4f, -1.0f), radius = 0.3f },
                new Sphere() { position = new Vector3(0.2f, 0.4f, -1.0f), radius = 0.3f }
                );
        }
    }
    // 순환계
    public class CirculatorySystem : OrganPart
    {
        public CirculatorySystem() : base()
        {
            Name = "CirculatorySystem";

            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(0.0f, 0.0f, 0.0f), radius = 10.0f });
        }


        public CirculatorySystem(OrganPart data, GameManager _gameManager) : base()
        {

        }
        public CirculatorySystem(OrganPart data) : base(data)
        {
            Name = "CirculatorySystem";

            // 모세혈관이 곳곳에 퍼져 있기 때문에 어디를 맞아도 데미지를 받습니다.
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(0.0f, 0.0f, 0.0f), radius = 10.0f });
        }
        public void bloodCirculation(params OrganPart[] organs) // 자신 기관계도 포함하세요. // baseOrganSystems이 매개변수였음.
        {
            /// 함수 설명 : 순환계의 고유 능력인 물질 교환을 실행하는 함수입니다.
            /// 
            /// 순환계는 에너지를 소모하여 전달하는 경우는 일부이고, 대부분 에너지를 사용하지 않습니다.
            /// 물론 능동적 수송(매개 수송 / 소낭 수송)에는 에너지를 소모할 수 있습니다.
            ///
            /// 러프 디자인
            /// 1분마다 이 함수가 호출되며
            /// 주변 기관계에게 필요한 물질을 나눠 준 다음
            /// 후속 화학반응도 체크합니다.
            ///
            /// 킹고리즘 정리
            /// 1. 각 기관계의 수요 확인(물질이 부족하면 needList에 등록, 물질이 넘쳐나면)
            /// 2.1. 각 기관계마다 물질을 하나하나 나눠줌(이중 For문:기관계, 물질), 수식((순환계 보유 - 순환계 수요량) * (이 기관계의 수요량 / Sum(모든 기관계의 이 물질 수요량)))
            /// 바깥 루프가 기준이 기관계라면 한꺼번에 줄 수 있겠다.
            /// 2.2.(삭제) 각 기관계마다 남는 물질을 반 나눠서 한쪽은 기관계에 남기고 한쪽은 순환계에 남김. (물질 분비는 섭식, 면역, 합성 시스템에서만 가능합니다.)
            /// 3. 주고 받은 물질이 각 기관계에서 반응하는지 체크합니다.

            // 1. 각 기관계의 수요 확인 

            // 1.1.1. 수요를 저장하는 변수. List<T>의 원소 자료형인 List<gameManager.chemical>는 기관계 하나의 요구되는 캐미컬들을 나타낸다.
            //List<List<GameManager.chemical>> needList = new List<List<GameManager.chemical>>();

            /// <remarks> 요구하는 화학 물질입니다. </remarks>
            // 단일 기관계가 요구하는 화학 물질입니다.
            List<ChemicalHelper.Chemicals> demand = new List<ChemicalHelper.Chemicals>();
            ChemicalHelper.Chemicals demandSum = new ChemicalHelper.Chemicals();

            //List<ChemicalHelper.Chemicals> needList = new List<ChemicalHelper.Chemicals>();
            
            // 1.1.2. 모든 기관계의 총 수요를 합친 값을 저장하는 변수.
            //List<GameManager.chemical> needSum = new List<GameManager.chemical>();
            
            // 1.2. 각 기관계를 돌며 화학물질의 니즈를 확인
            for(int index = 0; index > organs.Length; index++)
            {
                // 1.2.1. 니즈 저장하기
                ChemicalHelper.Chemicals demandChemical = new ChemicalHelper.Chemicals();

                demandChemical = NamedQuantityArrayHelper.GetDemand<ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (organs[index].demand, organs[index].tagged);

                // 1.2.2. 임시로 두 값을 작동을 위한 니즈에 저장.
                //DemandForOperate = gameManager.MixChemical(DemandForOperate, DemandForSurvival);

                // 1.2.3. 결과값을 needList와 needSum에 저장.
                demand.Add(this.demand);
                demandSum.Add(this.demand);
            }

            // 디버그 파트
            foreach(ChemicalHelper.Chemical oneOfNeedSum in demandSum)
            {
                Debug.Log("총 수요 물질 [" + oneOfNeedSum.matter + "] : " + oneOfNeedSum.quantity);
            }
            // <<<---
            // 2. 각 기관계마다 물질을 하나하나 나눠줍니다.
            // 바깥 포문은 기관계, 안쪽 포문은 물질마다(needSum)

            // 예상 이슈
            // 캐미컬이 질량이 0인 캐미컬이 존재한다면
            // 중복된 이름의 캐미컬이 있습니다.

            // chemicalsIndexArray 대상 기관계의 필드 chemicals에 인덱스로 사용됩니다.
            // 첫번째 인덱스는 몇번째 기관계인지입니다.
            // 두번째 인덱스는 needSum의 몇번째 캐미컬인지입니다.
            int[,] chemicalsIndexArray = new int[organs.Length, demandSum.Length];
            for(int index = 0; index < organs.Length; index++)
            {
                int[] indexArray = NameKeyArrayHelper.Find<ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (organs[index].tagged, demandSum);

                //int[] tempIndexList = new int[needSum.Count]; // -> IndexArray
                //gameManager.ChemicalIndexSet(baseOrganSystems[index].chemicals, needSum, ref tempIndexList);

                for (int i = 0; i < demandSum.Length; i++)
                {
                    chemicalsIndexArray[index, i] = indexArray[i];

                }
            }
            int[,] needListIndexArray = new int[organs.Length, demandSum.Length];
            for(int index = 0; index < organs.Length; index++)
            {
                int[] indexArray = NameKeyArrayHelper.Find<ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (demand[index], demandSum);

                //int[] tempIndexList = new int[needSum.Count];
                //gameManager.ChemicalIndexSet(needList[index], needSum, ref tempIndexList);

                for (int i = 0; i < demandSum.Length; i++)
                {
                    needListIndexArray[index,i] = indexArray[i];
                }
            }

            for (int organIndex = 0; organIndex < organs.Length; organIndex++) // organIndex은 몇번째 기관계인지 알려줍니다.
            {
                if (organIndex == 1) continue; // 2번째 기관계는 순환계로 예약되어 있습니다.

                for (int j = 0; j < demandSum.Length; j++) // j : X번째 캐미컬
                {
                    // 순환계에 있는 물질을 순환계가 쓰고 남는지 체크합니다.
                    float Stock = tagged[chemicalsIndexArray[1, j]].quantity - demand[1][j].quantity;
                    
                    if (Stock <= 0.0f) // 순환계에 있는 물질을 자기가 다 쓰니 남는게 없습니다.
                    {
                        continue; // 나눠 줄 수 없습니다. 다른 물질로 넘어갑니다.
                    }

                    //나눠 줄 수 있으니 자기 물질은 필요한만큼 남겨놓습니다.
                    tagged[chemicalsIndexArray[1, j]].quantity = demand[1][needListIndexArray[organIndex, j]].quantity;

                    // 물질을 나눠줍니다.
                    organs[organIndex].tagged[chemicalsIndexArray[organIndex, j]].quantity += Stock * (demand[organIndex][needListIndexArray[organIndex, j]].quantity / demandSum[j].quantity);

                    // 자신이 보유한 화학물질
                    //chemicals[chemicalsIndexArray[1, j]];
                    // 현재의 수요량
                    //needList[1][j];
                    // 전체의 수요량
                    //needSum[j];
                    // 이번 기관계의 수요량
                    //needList[organIndex][j];
                    // 넣을 대상
                    //baseOrganSystems[organIndex].chemicals;
                }
            }

            // 3. 기관계가 화학 반응을 일으키는지 체크합니다.
            foreach (OrganPart _organ in organs)
            {
                _organ.CheckChemicalReaction();
            }
        }

    }
    // 배출계
    public class ExcretorySystem : OrganPart
    {

        public ExcretorySystem()
        {
            Name = "excretorySystem";
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(-0.2f, -0.2f, -1.0f), radius = 0.3f },
                new Sphere() { position = new Vector3(0.2f, -0.2f, -1.0f), radius = 0.3f }
                );
        }
        public ExcretorySystem(GameManager _gameManager) : base()
        {

        }
        public ExcretorySystem(OrganPart data) : base(data)
        {
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(-0.2f, -0.2f, -1.0f), radius = 0.3f },
                new Sphere() { position = new Vector3(0.2f, -0.2f, -1.0f), radius = 0.3f }
                );
        }
    }
#warning 작업중 : UnitSightScale 함수가 호출될 시점에는 UnitSightRange의 값도 따라 변해야 합니다
    // 감각계
    /// <summary>
    ///     김각계 </summary>
    /// <remarks>
    ///     시각을 담당하는 유닛 파트입니다.</remarks>
    public class SensorySystem : OrganPart
    {
        public GameObject unit;
        public GameObjectList gameObjectList;
        public float UnitSightRange;
        //false면 감각을 받아들일 수 없게 됩니다.
        //

        public SensorySystem() : base()
        {

        }
        public SensorySystem(GameObjectList gameObjectList) : base()
        {
            Hack.Say(Hack.isDebugHumanUnitBase, "Yee");

            this.gameObjectList = gameObjectList;
            //gameObjectList.UnitSightMake();
            UnitSightRange = 6.0f;
        }
        public SensorySystem(OrganPart data, GameObjectList gameObjectList) : base(data)
        {
            Hack.Say(Hack.isDebugHumanUnitBase, "Yee");

            this.gameObjectList = gameObjectList;
            //gameObjectList.UnitSightMake();
            UnitSightRange = 6.0f;

            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(-0.2f, 1.0f, -1.0f), radius = 0.3f },
                new Sphere() { position = new Vector3(0.2f, 1.0f, -1.0f), radius = 0.3f }
                );
        }
        public override void BeingAttacked(ref AttackClassHelper.AttackInfo attack, float angle)
        {
            base.BeingAttacked(ref attack, angle);
            GameObjectList.UnitSightScale(unit, wholeness * UnitSightRange);
        }

        //[System.Obsolete("이 함수는 사용되지 않습니다.")]
        //public void ChemicalReactionCheck(GameManager.chemical[] _chemicals)
        //{
        //    Debug.Log("DEBUG_SensorySystem.ChemicalReactionCheck : 종료 -> 이 함수는 사용하지 않습니다");

        //    //base.ChemicalReactionCheck(_chemicals);

        //    gameObjectList.UnitSightNewScale(wholeness * UnitSightRange);

        //    //OrganSystemOperatingRate; // 이 값으로 UnitSight의 크기를 변경합니다.
        //}

        //UnitSight의 컨트롤하는 기관
        // OrganSystemOperatingRate가 작아질수록 EyeSight의 크기도 작아진다

    }
    // 신경계
    public class NervousSystem : OrganPart
    {
        //false면 통제를 할 수 없게 됩니다
        public NervousSystem() : base()
        {
            // 두뇌 중추 신경계
            collisionRangeSphere.Add(
                // 중앙
                new Sphere() { position = new Vector3(0.0f, 1.0f, 0.2f), radius = 0.6f },
                // 앞 쪽
                new Sphere() { position = new Vector3(0.6f, 1.0f, -0.2f), radius = 0.6f },
                new Sphere() { position = new Vector3(-0.6f, 1.0f, -0.2f), radius = 0.6f },
                // 뒷 쪽
                new Sphere() { position = new Vector3(0.6f, 1.0f, 0.6f), radius = 0.6f },
                new Sphere() { position = new Vector3(-0.6f, -0.2f, 0.6f), radius = 0.6f },
                // 허리 중추 신경계
                new Sphere() { position = new Vector3(0.0f, 0.8f, 1.0f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.0f, 0.4f, 1.0f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.0f, 0.0f, 1.0f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.0f, -0.4f, 1.0f), radius = 0.4f }
                );
        }
        public NervousSystem(OrganPart data) : base(data)
        {
            // 두뇌 중추 신경계
            collisionRangeSphere.Add(
                // 중앙
                new Sphere() { position = new Vector3(0.0f, 1.0f, 0.2f), radius = 0.6f },
                // 앞 쪽
                new Sphere() { position = new Vector3(0.6f, 1.0f, -0.2f), radius = 0.6f },
                new Sphere() { position = new Vector3(-0.6f, 1.0f, -0.2f), radius = 0.6f },
                // 뒷 쪽
                new Sphere() { position = new Vector3(0.6f, 1.0f, 0.6f), radius = 0.6f },
                new Sphere() { position = new Vector3(-0.6f, -0.2f, 0.6f), radius = 0.6f },
                // 허리 중추 신경계
                new Sphere() { position = new Vector3(0.0f, 0.8f, 1.0f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.0f, 0.4f, 1.0f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.0f, 0.0f, 1.0f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.0f, -0.4f, 1.0f), radius = 0.4f }
                );
        }
    }
    // 운동계
    public class MotorSystem : OrganPart
    {
        public MotorSystem() : base()
        {
            collisionRangeSphere.Add(
                // 팔
                new Sphere() { position = new Vector3(-0.8f, 0.0f, -0.2f), radius = 0.4f },
                new Sphere() { position = new Vector3(-0.8f, 0.0f, -0.6f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.8f, 0.0f, -0.2f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.8f, 0.0f, -0.6f), radius = 0.4f },
                // 다리
                new Sphere() { position = new Vector3(-0.4f, -1.0f, -0.4f), radius = 0.6f },
                new Sphere() { position = new Vector3(0.4f, -1.0f, -0.4f), radius = 0.6f },
                new Sphere() { position = new Vector3(-0.4f, -1.0f, 0.4f), radius = 0.6f },
                new Sphere() { position = new Vector3(0.4f, -1.0f, 0.4f), radius = 0.6f }
                );
        }
        public MotorSystem(OrganPart data) : base(data)
        {
            collisionRangeSphere.Add(
                // 팔
                new Sphere() { position = new Vector3(-0.8f, 0.0f, -0.2f), radius = 0.4f },
                new Sphere() { position = new Vector3(-0.8f, 0.0f, -0.6f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.8f, 0.0f, -0.2f), radius = 0.4f },
                new Sphere() { position = new Vector3(0.8f, 0.0f, -0.6f), radius = 0.4f },
                // 다리
                new Sphere() { position = new Vector3(-0.4f, -1.0f, -0.4f), radius = 0.6f },
                new Sphere() { position = new Vector3(0.4f, -1.0f, -0.4f), radius = 0.6f },
                new Sphere() { position = new Vector3(-0.4f, -1.0f, 0.4f), radius = 0.6f },
                new Sphere() { position = new Vector3(0.4f, -1.0f, 0.4f), radius = 0.6f }
                );
        }
        // 활동량이0.1 이하로 떨어지면 활동을 못하게 막습니다.
    }
    // 면역계
    public class ImmuneSystem : OrganPart
    {
        public ImmuneSystem() : base()
        {

        }
        public ImmuneSystem(OrganPart data) : base(data)
        {
            //왼쪽 허리
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(0.6f, 0.0f, 0.6f), radius = 0.4f });
        }
    }
    // 합성계
    public class SynthesisSystem : OrganPart
    {
        public SynthesisSystem() : base()
        {
            // 오른쪽 허리
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(-0.6f, 0.0f, 0.6f), radius = 0.4f });
        }
        public SynthesisSystem(GameManager _gameManager) : base()
        {

        }
        public SynthesisSystem(OrganPart data) : base(data)
        {
            // 오른쪽 허리
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(-0.6f, 0.0f, 0.6f), radius = 0.4f });
        }
    }
    // 각질계
    public class IntegumentarySystem : OrganPart
    {
        public IntegumentarySystem() : base()
        {
            // 어디를 맞아도 데미지
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(0.0f, 0.0f, 0.0f), radius = 10.0f });
        }
        public IntegumentarySystem(GameManager _gameManager) : base()
        {

        }
        public IntegumentarySystem(OrganPart data) : base(data)
        {
            // 어디를 맞아도 데미지
            collisionRangeSphere.Add(
                new Sphere() { position = new Vector3(0.0f, 0.0f, 0.0f), radius = 10.0f });
        }
    }




}
