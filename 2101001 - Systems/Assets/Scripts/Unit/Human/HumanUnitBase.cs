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

public class HumanUnitBase : MonoBehaviour
{
    #region ReadOnlyStatic Field
    public static Dictionary<int, string> HumanOrganNameList;



    #endregion

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
        organSystemsSet();


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
    



    #region 필드
    #region 1회용 필드
    //bool isEyeSightBooted = false;

    #endregion
    #region 기관계 전용 프리펩/ 필드
    // 소화계 전용
    // 순환계 전용
    // 배출계 전용
    // 감각계 전용
    //public GameObject unitSightPrefab;
    //public GameObject unitSightGo; // 여기에 이벤트를 달까? // 안돼 이벤트 극혐. 이벤트는 싱글톤에만 할거야
    public int SightRange = 6;

    #endregion


    public List<BaseOrganSystem> OrganSystems;
    public void organSystemsSet()
    {
        //Debug.Log("HumanUnitBase.organSystemsSet()"); // 호출되었습니다.
        

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        OrganSystems = new List<BaseOrganSystem>();
        OrganSystems.Add(new DigestiveSystem(gameManager, setDefaultData()));
        OrganSystems.Add(new CirculatorySystem(gameManager, setDefaultData()));
        OrganSystems.Add(new ExcretorySystem(gameManager, setDefaultData()));
        OrganSystems.Add(new SensorySystem(gameManager, GetComponent<GameObjectList>(), setDefaultData()));
        OrganSystems.Add(new NervousSystem(gameManager, setDefaultData()));
        OrganSystems.Add(new MotorSystem(gameManager, setDefaultData()));
        OrganSystems.Add(new ImmuneSystem(gameManager, setDefaultData()));
        OrganSystems.Add(new SynthesisSystem(gameManager, setDefaultData()));
        OrganSystems.Add(new IntegumentarySystem(gameManager, setDefaultData()));
    }
    public void organSystemsSet(OrganListData organData)
    {
        if (organData.organSystemDatas.Length == 9 && organData.type == "human")
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            OrganSystems = new List<BaseOrganSystem>();
            OrganSystems.Add(new DigestiveSystem(gameManager, organData.organSystemDatas[0]));
            OrganSystems.Add(new CirculatorySystem(gameManager, organData.organSystemDatas[1]));
            OrganSystems.Add(new ExcretorySystem(gameManager, organData.organSystemDatas[2]));
            OrganSystems.Add(new SensorySystem(gameManager, GetComponent<GameObjectList>(), organData.organSystemDatas[3]));
            OrganSystems.Add(new NervousSystem(gameManager, organData.organSystemDatas[4]));
            OrganSystems.Add(new MotorSystem(gameManager, organData.organSystemDatas[5]));
            OrganSystems.Add(new ImmuneSystem(gameManager, organData.organSystemDatas[6]));
            OrganSystems.Add(new SynthesisSystem(gameManager, organData.organSystemDatas[7]));
            OrganSystems.Add(new IntegumentarySystem(gameManager, organData.organSystemDatas[8]));
        }
        else organSystemsSet();
    }
    BaseOrganSystemData setDefaultData()
    {
        //Debug.Log("DEBUG_호출됨 : HumanUnitBase.setDefaultData()"); // (현재 작동됨) if It works -> instantiated

        // 기관계에 넣을 기본 데이터를 리턴하는 함수입니다.
        BaseOrganSystemData returnValue = new BaseOrganSystemData();
        returnValue.OrganSystemOperatingRate = 1.0f;
        returnValue.maxCellAmount = 100.0f;
        returnValue.cellAmount = 100.0f;
        returnValue.cellRecoveryRate = 1.48f;
        returnValue.impulseTolerance = 30.0f;
        // **position과 radius값은 생성자에 자동으로 입력됩니다.
        //returnValue.position = new List<Vector3>();
        //returnValue.position.CopyTo(positionList);
        //returnValue.radius = new List<float>();
        //returnValue.radius.CopyTo(radiusList); // 부딛힌 지점에서 0.2f 이내면 영향을 받습니다.

        //returnValue.chemicals = new GameManager.chemical[2];
        returnValue.chemicals = new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f) };
        returnValue.chemicalsRatioForSurvival = new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f) };
        returnValue.chemicalsRatioForOperate = new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f) };
        //returnValue.chemicals.CopyTo(new GameManager.chemical[] {
        //    new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f)
        //});
        //returnValue.chemicalsRatioForSurvival.CopyTo(new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f) }); // 요기 문제네
        //returnValue.chemicalsRatioForOperate.CopyTo(new GameManager.chemical[] { new GameManager.chemical("TEST_organicCarbon", 20.0f), new GameManager.chemical("TEST_ATP", 10.0f)}); // 요기도 문제일 것으로 추정된다.


        //foreach (GameManager.chemical oneOfChemicals in returnValue.chemicals)
        //{
        //    Debug.Log("HumanUnitBase.setDefaultData() " + oneOfChemicals.matter); // 작동합니다.
        //}


        return returnValue;
    }
    // 


    #endregion
    #region 외부와 약속된 함수
    public void SetComponentData(OrganListData organData)
    {
        organSystemsSet(organData);
    }



    #endregion
    #region 함수




    #endregion

    // 공격 받으면 작동하는 함수



    // 기관계에 대한 기본 정의

    //

    #region [데이터 입출력용 클래스] OrganListData, BaseOrganSystemData
    [System.Serializable]
    public class OrganListData
    {
        public string type;
        public BaseOrganSystemData[] organSystemDatas;
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

        public GameManager.chemical[] chemicals; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
        public GameManager.chemical[] chemicalsRatioForSurvival; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
        public GameManager.chemical[] chemicalsRatioForOperate; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
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

    public class BaseOrganSystem
    {
        public string TypeName;
        //bool isOrganSystemWorks; // 외부 OrganSystem이 이 값을 참고할 것입니다.
        public float OrganSystemOperatingRate; // 기관계가 얼만큼 온전하게 작동하는지 (1.0 미만이면 관련 디버프 발생, 0.0f 이하면 작동하지 않음)
        // 기관 내부의 물질이 바뀔 때마다 작동합니다.
        // 물질이 반응을 시작할때마다, 공격을 받을 때마다 이 값이 바뀝니다.

        public float maxCellAmount; // 이 기관이 최대로 회복할 수 있는 양입니다
        public float cellAmount;
        public float cellRecoveryRate; // CellAmount가 몇배로 바뀌어 있는가. 기본적으로 1.04입니다. (cellAmount = cellAmount * cellRecoveryRate * (chemicals / chemicalsRatioForsurvival))
        public float impulseTolerance; // 물리적 충돌에 대한 임계값입니다.

        public List<Vector3> position; // 자신의 기관계의 위치를 나타냅니다. 기준은 유닛이 바라보는 방향이 (0,0,-1)일때입니다.
        public List<float> radius; // 반지름, scale값의 절반

        // 이 바이오 시스템이 가지고 있는 케미컬 양
        public List<GameManager.chemical> chemicals; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
        public List<GameManager.chemical> chemicalsRatioForSurvival; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
        public List<GameManager.chemical> chemicalsRatioForOperate; // 원리와 화학은 이쪽에서 다뤄질 것 같습니다.
        protected float RatioForSurvival;
        protected float RatioForOperate;

        protected GameManager gameManager;
        
        public void ChemicalReactionCheck(GameManager.chemical[] _chemicals)
        {
            //GameManager gameManager = GetComponent<GameManager>();
            //외부에서 화학물 공격을 받았을때, 공격하는 대상이 호출하는 함수입니다.
            // chemicals에서 침입한 화합물들을 받습니다.

            // 침입한 _chemicals의 원소들을 this.chemicals에 섞는다.
            List<GameManager.chemical> InputChemicalList = new List<GameManager.chemical>(_chemicals);

            List<GameManager.chemical> cList = new List<GameManager.chemical>();
            //foreach(GameManager.chemical oneOfChemicals in chemicals)
            //{
            //    Debug.Log("ChemicalReactionCheck" + oneOfChemicals.matter); // if it works -> this.chemicals has no wrongs.
            //}
            

            cList = gameManager.MixChemical(chemicals, InputChemicalList);
            chemicals = cList;

            this.ChemicalReactionCheck();
        }
        public void ChemicalReactionCheck()
        {
            // this.chemicals에서 화학 반응이 일어나는지 검사합니다.
            float generatedEnergy = 0.0f;
            gameManager.chemicalReactionFunc(ref generatedEnergy, ref this.chemicals);

            // this.chemical를 chemicalsRatioForsurvival로 나눈 값도 얻고 chemicalsRatioForOperate로 나눈값도 얻는다.
            RatioForSurvival = Mathf.Min(1, gameManager.chemicalsDivideByChemicals(this.chemicals, this.chemicalsRatioForSurvival));
            RatioForOperate = Mathf.Min(1, gameManager.chemicalsDivideByChemicals(this.chemicals, this.chemicalsRatioForOperate));

            // 화학 반응으로 인한 에너지를 매개변수에 넣는다
            PhysicalEnergyAttack(generatedEnergy);
        }
        public void PhysicalEnergyAttack(float Energy)
        {
            // 에너지 유입으로 인한 체세포 손상값을 얻는다.
            cellAmount = Mathf.Max(0, cellAmount - Mathf.Max(Energy - impulseTolerance, 0));

            // 기관계 활동비를 얻는다.
            OrganSystemOperatingRate = (cellAmount / maxCellAmount) * RatioForSurvival * RatioForOperate;
        }
        public void PhysicalEnergyAttack(float Energy, float Volume)
        {
            if (Energy - impulseTolerance < 0.0f) return; 

            // 에너지 유입으로 인한 체세포 손상값을 얻는다.
            cellAmount = Mathf.Max(0, cellAmount - Volume, 0);

            // 기관계 활동비를 얻는다.
            OrganSystemOperatingRate = (cellAmount / maxCellAmount) * RatioForSurvival * RatioForOperate;
        }
        public void StatUpdate()
        {
            



            //OrganSystemOperatingRate = Mathf.Min(cellAmount / maxCellAmount, 1.0f) * Mathf.Min()

        }
        public BaseOrganSystem(GameManager _gameManager)
        {
            this.gameManager = _gameManager;
        }
        public BaseOrganSystem(GameManager _gameManager, BaseOrganSystemData InputData)
        {
            // 함수 설명: 데이터 클래스를 토대로 필드를 설정합니다.
            this.TypeName = "BaseOrganSystem";
            this.gameManager = _gameManager;

            this.OrganSystemOperatingRate = InputData.OrganSystemOperatingRate;
            this.maxCellAmount = InputData.maxCellAmount;
            this.cellAmount = InputData.cellAmount;
            this.cellRecoveryRate = InputData.cellRecoveryRate;
            this.impulseTolerance = InputData.impulseTolerance;
            this.position = new List<Vector3>(); // position 값은 생성자에 들어 있습니다.
            this.radius = new List<float>(); // radius 값은 생성자에 들어 있습니다.

            this.chemicals = new List<GameManager.chemical>(InputData.chemicals);
            //this.chemicals = InputData.chemicals;
            this.chemicalsRatioForOperate = new List<GameManager.chemical>(InputData.chemicalsRatioForOperate);
            //this.chemicalsRatioForOperate = InputData.chemicalsRatioForOperate;
            this.chemicalsRatioForSurvival = new List<GameManager.chemical>(InputData.chemicalsRatioForSurvival);
            //this.chemicalsRatioForSurvival = InputData.chemicalsRatioForSurvival;

            //Debug.Log("BaseOrganSystem(GameManager _gameManager, BaseOrganSystemData InputData)");
            //foreach (GameManager.chemical oneOfChemicals in InputData.chemicals) // 이로써 InputData가 텅텅 비어있음을 알게 되었습니다.
            //{
            //    Debug.Log(oneOfChemicals.matter); // it works.
            //}
            //foreach (GameManager.chemical oneOfChemicals in this.chemicals)
            //{
            //    Debug.Log(oneOfChemicals.matter); // it works.
            //}
        }
        public BaseOrganSystemData GetData()
        {
            // 함수 설명: 이 클래스의 필드를 저장 가능한 데이터 클래스로 리턴합니다.

            BaseOrganSystemData outD = new BaseOrganSystemData();
            outD.TypeName = this.TypeName;
            outD.OrganSystemOperatingRate = this.OrganSystemOperatingRate;
            outD.maxCellAmount = this.maxCellAmount;
            outD.cellAmount = this.cellAmount;
            outD.cellRecoveryRate = this.cellRecoveryRate;
            outD.impulseTolerance = this.impulseTolerance;
            //outD.position = this.position;
            //outD.radius = this.radius;
            this.chemicals.CopyTo(outD.chemicals);
            this.chemicalsRatioForOperate.CopyTo(outD.chemicalsRatioForOperate);
            this.chemicalsRatioForSurvival.CopyTo(outD.chemicalsRatioForSurvival);

            return outD;
        }
        
        // 구성 물질들
        // 작동하는 부분(bool 형식)
    }
    // 소화계
    public class DigestiveSystem : BaseOrganSystem
    {
        public DigestiveSystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public DigestiveSystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            TypeName = "DigestiveSystem";
            position.Add(new Vector3(-0.2f, 0.4f, -1.0f));
            radius.Add(0.3f);
            position.Add(new Vector3(0.2f, 0.4f, -1.0f));
            radius.Add(0.3f);
        }
    }
    // 순환계
    public class CirculatorySystem : BaseOrganSystem
    {
        public CirculatorySystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public CirculatorySystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            TypeName = "CirculatorySystem";
            position.Add(new Vector3(0.0f, 0.0f, 0.0f));
            radius.Add(10.0f); // 어디를 맞아도 데미지
        }
        public void bloodCirculation(params BaseOrganSystem[] baseOrganSystems) // 자신 기관계도 포함하세요.
        {
            /// 함수 설명 : 순환계의 고유 능력인 물질 교환을 실행하는 함수입니다.
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

            // --->>>
            // 1. 각 기관계의 수요 확인 

            // 1.1.1. 수요를 저장하는 변수. List<T>의 원소 자료형인 List<gameManager.chemical>는 기관계 하나의 요구되는 캐미컬들을 나타낸다.
            List<List<GameManager.chemical>> needList = new List<List<GameManager.chemical>>();
            
            // 1.1.2. 모든 기관계의 총 수요를 합친 값을 저장하는 변수.
            List<GameManager.chemical> needSum = new List<GameManager.chemical>();
            
            // 1.2. 각 기관계를 돌며 화학물질의 니즈를 확인
            for(int index = 0; index > baseOrganSystems.Length; index++)
            {
                //BaseOrganSystem oneOfBaseOrganSystems in baseOrganSystems
                //ChemicalDemandCheck(A, B); 를 참고하세요.

                // 1.2.1. 생존을 위한 니즈와 작동을 위한 니즈를 저장.
                List<GameManager.chemical> DemandForOperate = new List<GameManager.chemical>();
                List<GameManager.chemical> DemandForSurvival = new List<GameManager.chemical>();

                DemandForOperate = gameManager.ChemicalDemandCheck(baseOrganSystems[index].chemicalsRatioForOperate, baseOrganSystems[index].chemicals);
                DemandForSurvival = gameManager.ChemicalDemandCheck(baseOrganSystems[index].chemicalsRatioForSurvival, baseOrganSystems[index].chemicals);

                // 1.2.2. 임시로 두 값을 작동을 위한 니즈에 저장.
                DemandForOperate = gameManager.MixChemical(DemandForOperate, DemandForSurvival);

                // 1.2.3. 결과값을 needList와 needSum에 저장.
                needList[index] = gameManager.MixChemical(needList[index], DemandForOperate);
                if(baseOrganSystems[index].GetType() == this.GetType())
                {
                    continue;
                }    
                //if (index == 1) continue;
                needSum = gameManager.MixChemical(needSum, DemandForOperate);
            }

            // 디버그 파트
            foreach(GameManager.chemical oneOfNeedSum in needSum)
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
            int[,] chemicalsIndexArray = new int[baseOrganSystems.Length, needSum.Count];
            for(int index = 0; index < baseOrganSystems.Length; index++)
            {
                int[] tempIndexList = new int[needSum.Count];
                gameManager.ChemicalIndexSet(baseOrganSystems[index].chemicals, needSum, ref tempIndexList);

                for (int i = 0; i < needSum.Count; i++)
                {
                    chemicalsIndexArray[index, i] = tempIndexList[i];

                }
            }
            int[,] needListIndexArray = new int[baseOrganSystems.Length, needSum.Count];
            for(int index = 0; index < baseOrganSystems.Length; index++)
            {
                int[] tempIndexList = new int[needSum.Count];
                gameManager.ChemicalIndexSet(needList[index], needSum, ref tempIndexList);

                for (int i = 0; i < needSum.Count; i++)
                {
                    needListIndexArray[index,i] = tempIndexList[i];
                }
            }

            for (int index = 0; index < baseOrganSystems.Length; index++) // index는 몇번째 기관계인지 알려줍니다.
            {
                if (index == 1) continue; // 2번째 기관계는 순환계로 예약되어 있습니다.

                List<GameManager.chemical> inputValue = new List<GameManager.chemical>();

                for (int j = 0; j < needSum.Count; j++) // j : X번째 캐미컬
                {
                    // 순환계에 있는 물질을 순환계가 쓰고 남는지 체크합니다.
                    float Stock = chemicals[chemicalsIndexArray[1, j]].quantity - needList[1][j].quantity;
                    
                    if (Stock <= 0.0f) // 순환계에 있는 물질을 자기가 다 쓰니 남는게 없습니다.
                    {
                        continue; // 나눠 줄 수 없습니다. 다른 물질로 넘어갑니다.
                    }

                    //나눠 줄 수 있으니 자기 물질은 필요한만큼 남겨놓습니다.
                    chemicals[chemicalsIndexArray[1, j]].quantity = needList[1][needListIndexArray[index,j]].quantity;

                    // 물질을 나눠줍니다.
                    baseOrganSystems[index].chemicals[chemicalsIndexArray[index, j]].quantity += Stock * (needList[index][needListIndexArray[index, j]].quantity / needSum[j].quantity);

                    // 자신이 보유한 화학물질
                    //chemicals[chemicalsIndexArray[1, j]];
                    // 현재의 수요량
                    //needList[1][j];
                    // 전체의 수요량
                    //needSum[j];
                    // 이번 기관계의 수요량
                    //needList[index][j];
                    // 넣을 대상
                    //baseOrganSystems[index].chemicals;
                }
            }

            // 3. 기관계가 화학 반응을 일으키는지 체크합니다.
            foreach(BaseOrganSystem oneOfBaseOrganSystems in baseOrganSystems)
            {
                oneOfBaseOrganSystems.ChemicalReactionCheck();
            }

        }

    }
    // 배출계
    public class ExcretorySystem : BaseOrganSystem
    {
        public ExcretorySystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public ExcretorySystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            position.Add(new Vector3(-0.2f, -0.2f, -1.0f));
            radius.Add(0.3f);
            position.Add(new Vector3(0.2f, -0.2f, -1.0f));
            radius.Add(0.3f);
        }
    }
    // 감각계
    public class SensorySystem : BaseOrganSystem
    {
        public GameObjectList gameObjectList;
        float UnitSightRange;
        //false면 감각을 받아들일 수 없게 됩니다.
        //

        public SensorySystem(GameManager _gameManager, GameObjectList gameObjectList) : base(_gameManager)
        {
            Debug.Log("Yee");
            this.gameObjectList = gameObjectList;
            gameObjectList.UnitSightMake();
            UnitSightRange = 6.0f;
        }
        public SensorySystem(GameManager _gameManager, GameObjectList gameObjectList, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            Debug.Log("Yee");

            this.gameObjectList = gameObjectList;
            gameObjectList.UnitSightMake();
            UnitSightRange = 6.0f;

            position.Add(new Vector3(-0.2f, 1.0f, -1.0f));
            radius.Add(0.3f);
            position.Add(new Vector3(0.2f, 1.0f, -1.0f));
            radius.Add(0.3f);
        }
        public void ChemicalReactionCheck(GameManager.chemical[] _chemicals)
        {
            base.ChemicalReactionCheck(_chemicals);

            gameObjectList.UnitSightNewScale(OrganSystemOperatingRate * UnitSightRange);

            //OrganSystemOperatingRate; // 이 값으로 UnitSight의 크기를 변경합니다.
        }

        //UnitSight의 컨트롤하는 기관
        // OrganSystemOperatingRate가 작아질수록 EyeSight의 크기도 작아진다

    }
    // 신경계
    public class NervousSystem : BaseOrganSystem
    {
        //false면 통제를 할 수 없게 됩니다
        public NervousSystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public NervousSystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            // 두뇌 중추 신경계
            // 중앙
            position.Add(new Vector3(0.0f, 1.0f, 0.2f));
            radius.Add(0.6f);
            //앞쪽
            position.Add(new Vector3(0.6f, 1.0f, -0.2f));
            radius.Add(0.6f);
            position.Add(new Vector3(-0.6f, 1.0f, -0.2f));
            radius.Add(0.6f);
            //뒷쪽
            position.Add(new Vector3(0.6f, 1.0f, 0.6f));
            radius.Add(0.6f);
            position.Add(new Vector3(-0.6f, -0.2f, 0.6f));
            radius.Add(0.6f);
            // 허리 중추 신경계
            position.Add(new Vector3(0.0f, 0.8f, 1.0f));
            radius.Add(0.4f);
            position.Add(new Vector3(0.0f, 0.4f, 1.0f));
            radius.Add(0.4f);
            position.Add(new Vector3(0.0f, 0.0f, 1.0f));
            radius.Add(0.4f);
            position.Add(new Vector3(0.0f, -0.4f, 1.0f));
            radius.Add(0.4f);
        }
    }
    // 운동계
    public class MotorSystem : BaseOrganSystem
    {
        public MotorSystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public MotorSystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            // 팔
            position.Add(new Vector3(-0.8f, 0.0f, -0.2f));
            radius.Add(0.4f);
            position.Add(new Vector3(-0.8f, 0.0f, -0.6f));
            radius.Add(0.4f);
            position.Add(new Vector3(0.8f, 0.0f, -0.2f));
            radius.Add(0.4f);
            position.Add(new Vector3(0.8f, 0.0f, -0.6f));
            radius.Add(0.4f);
            // 다리
            position.Add(new Vector3(-0.4f, -1.0f, -0.4f));
            radius.Add(0.6f);
            position.Add(new Vector3(0.4f, -1.0f, -0.4f));
            radius.Add(0.6f);
            position.Add(new Vector3(-0.4f, -1.0f, 0.4f));
            radius.Add(0.6f);
            position.Add(new Vector3(0.4f, -1.0f, 0.4f));
            radius.Add(0.6f);

        }
        // 활동량이0.1 이하로 떨어지면 활동을 못하게 막습니다.
    }
    // 면역계
    public class ImmuneSystem : BaseOrganSystem
    {
        public ImmuneSystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public ImmuneSystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            //왼쪽 허리
            position.Add(new Vector3(0.6f, 0.0f, 0.6f));
            radius.Add(0.4f);
        }
    }
    // 합성계
    public class SynthesisSystem : BaseOrganSystem
    {
        public SynthesisSystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public SynthesisSystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            // 오른쪽 허리
            position.Add(new Vector3(-0.6f, 0.0f, 0.6f));
            radius.Add(0.4f);
        }
    }
    // 각질계
    public class IntegumentarySystem : BaseOrganSystem
    {
        public IntegumentarySystem(GameManager _gameManager) : base(_gameManager)
        {

        }
        public IntegumentarySystem(GameManager _gameManager, BaseOrganSystemData InputData) : base(_gameManager, InputData)
        {
            position.Add(new Vector3(0.0f, 0.0f, 0.0f));
            radius.Add(10.0f); // 어디를 맞아도 데미지
        }
    }




}
