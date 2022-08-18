using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO
// DemoFieldLoader 함수 수정 필요
// FieldSetup Update: 현재 보고 있는 지역에 대한 정보를 추가해야 합니다. 그리고 유닛 인스턴스화할때, 현재 보고 있는 지역의 이름과 스쿼드에 적혀져 있는 지역이 같은 경우, 그 스쿼드의 유닛만 인스턴스화합니다.
// SetGameObjectComponentDataByFieldData 에 데이터를 주고받는 컴포넌트 다 집어넣기 (인간 유닛은 완료 / )
// RegisterUnitData 람다식을 for루프식으로 만들어 아이디 비교하기
// ComponentDataTypeToString에 컴포넌트 형식 다 넣기
// AddComponentData 비제네릭 함수에 구현 다하기

#region TIP
// 새로운 휴먼을 세팅할때는 DemoFieldLoader를 참고하세요.

#endregion

public class GameManager : MonoBehaviour
{
    #region 어디에 쓰이는 클래슨고?
    // 게임의 전반적인 통제자
    // UiManager UI 통제 기능 (타 컴포넌트)
    // FieldManager 플레이어 데이터 통제
    // 게임이 끝났는지 판정하는 GoalManager

    // UI 통제 기능
    // 키보드 입력 / 마우스 입력을 받아들여 그에 맞는 함수를 실행시켜준다.
    // [배경화면 및 버튼] [생성 및 제거] / 마우스 입력 감지
    //
    // 플레이어 데이터 통제
    // 플레이어 데이터 [파일로부터 읽어오기] / [업데이트] / [파일로 저장]
    // 각 팀의 상황들, 정보를 전부 저장한다.


    #endregion

    void Awake()
    {
        MachineWorkEvent += delegate () { };
    }

    void Start()
    {
        // 
        TileMapSetup();
        FieldSetup();



        ChemicalReactionSetup();
        //PlayerSetup(); //PlayerSetup은 FieldSetup에서 다뤄집니다.

        // 독립적인
        BuildHelperSetup();

        // 준비과정에 종속적인 함수들
        StartCoroutine("RunMachineWork");
    }

    // Update is called once per frame
    void Update()
    {
        #region Machine



        #endregion




    }

    #region Class
    public class GameManagerData
    {
        // 모든 매니저의 핵심 데이터를 저장합니다.
        // 필드 데이터
        // 연구 데이터

        FieldData fieldData;
    }


    #endregion

    #region 아무데서나 사용할 수 있는 제네릭 함수




    #endregion



    #region 길찾기 알고리즘 보조역할




    Dictionary<Vector3, int> RealTileMap;
    void TileMapSetup() { RealTileMap = new Dictionary<Vector3, int>(); }

    public void SetTileMap(Vector3 vec3, int blockTileID)
    {
        // <!> 이벤트 발생시키기 -> TileBlock으로부터 데이터를 주입하도록 호출합니다.



        // NOTE:
        // a. Dictionary에서 .Add 함수를 이용할때 ArgumentException을 이용한 코드 : 사용해도 괜찮음!
        // b. Dictionary에서 .TryAdd 함수를 이용한 코드 : 유니티에서 TryAdd 함수를 못 알아먹는듯.
        if (RealTileMap.ContainsKey(vec3) == true) // 자기 위치를 저장할때 : 메모리맵에 자기 위치에 해당하는 정보가 이미 있습니다.
        {
            RealTileMap.Remove(vec3);
            RealTileMap.Add(vec3, blockTileID);
            //Debug.Log("타일맵이 저장되었습니다. : " + vec3);
        }
        else // 자기 위치를 저장할때 : 신규 블럭 등록
        {
            RealTileMap.Add(vec3, blockTileID);
            //Debug.Log("타일맵이 저장되었습니다. : " + vec3 + ", " + blockTileID);// 작동함!
        }
    }
    // searchFloorTileVec3은 바닥 타일의 위치입니다. 바로 위에 좌표에 벽이 있는지도 계산합니다.
    public bool IsAbleToStepThere(ref Dictionary<Vector3, int> tileMap, Vector3 searchFloorTileVec3)
    {
        searchFloorTileVec3 = new Vector3(Mathf.Round(searchFloorTileVec3.x), Mathf.Round(searchFloorTileVec3.y), Mathf.Round(searchFloorTileVec3.z));
        Vector3 searchUpperTileVec3 = searchFloorTileVec3 + new Vector3(0, 1, 0);

        if (tileMap.ContainsKey(searchFloorTileVec3))
        {
            Debug.Log("바닥 벨류 값 : " + tileMap[searchFloorTileVec3]);
            if (tileMap[searchFloorTileVec3] != 101)
                return false;
        }
        else return false;
        if (tileMap.ContainsKey(searchUpperTileVec3))
        {
            Debug.Log("지상 벨류 값 : " + tileMap[searchUpperTileVec3]);
            if (tileMap[searchUpperTileVec3] != -1)
                return false;
        }
        return true;
    }
    public bool IsAbleToStepThere(Vector3 searchVec3)
    {
        return IsAbleToStepThere(ref RealTileMap, searchVec3);
    }
    #endregion
    #region BuilderHelper 머신 유닛 건설 보조역할
    public List<Vector3> FixturePosition; // Fixture가 Instantiate할 때마다 여기에 값을 넣습니다. GameManager의 책임이 없습니다.

    void BuildHelperSetup()
    {
        FixturePosition = new List<Vector3>();
    }

    #endregion
    #region 화학 물질 / 전투
    // 데이터 IO 클래스: ChemicalRuleData




    // 무기들이 사용되는 구조체입니다.

    void ChemicalReactionSetup() {
        chemicalReactionTable = new List<ChemicalReaction>();
        chemicalReactionTable = GetComponent<ChemicalReactionDemoData>().chemicalReactions;
    }
    //ChemicalReactionData
    public List<ChemicalReaction> chemicalReactionTable; // responsiveness를 기준으로 Sort됩니다.


    #region 클래스 정의: AttackClass, chemical, ChemicalReaction, AttackClassData, ChemicalElementData, ChemicalData, ChemicalReactionData, ChemicalRuleData

    #region 실제로 사용되는 클래스: AttackClass, chemical, ChemicalReaction

    public class AttackClass
    {
        public List<chemical> chemicals;
        public float mass; // 불변의 값입니다.
        public float speed; // 단순히 관통력(mess*speed)값을 얻기 위한 수입니다. 환경에 의해 변할 수 있습니다.
        public float volume; // 관통력이 커 유효 타격으로 인지할 시 얼만큼 데미지를 줄 지 계산합니다. // 혹은 충돌 이후에 계산할 수 있습니다.

        public AttackClass()
        {
            //chemicals = new chemical[chemicalIndex];
            chemicals = new List<chemical>();
            mass = 1.0f;
            speed = 1.0f;
        }
        public AttackClass(float _mass, float _speed, float _volume, params chemical[] _chemicals)
        {
            chemicals = new List<chemical>(_chemicals);
            //chemicals.CopyTo(_chemicals);
            mass = _mass;
            speed = _speed;
            volume = _volume;
        }
        public AttackClass(float _mass, float _speed, float _volume, chemical _chemicals)
        {
            chemicals = new List<chemical>(new chemical[] { _chemicals });

            //chemicals.Add(_chemicals);
            mass = _mass;
            speed = _speed;
            volume = _volume;
        }
        public AttackClass(AttackClassData data)
        {
            this.chemicals.CopyTo(chemical.toChemical(data.chemicals));
            this.mass = data.mass;
            this.speed = data.speed;
            this.volume = data.volume;
        }
        public AttackClassData GetData()
        {
            AttackClassData returnValue = new AttackClassData();
            returnValue.chemicals = chemical.toChemicalDataArray(chemicals.ToArray());
            //returnValue.chemicals.CopyTo(chemical.toChemicalDataArray(chemicals.ToArray()), 0);

            returnValue.mass = this.mass;
            returnValue.volume = this.volume;
            return returnValue;
        }
        public static AttackClass[] toAttackClassArray(AttackClassData[] data)
        {
            AttackClass[] returnValue = new AttackClass[data.Length];
            for (int index = 0; index < data.Length; index++)
            {
                returnValue[index] = new AttackClass(data[index]);
            }
            return returnValue;
        }
        public static AttackClassData[] toAttackClassDataArray(AttackClass[] instance)
        {
            AttackClassData[] returnValue = new AttackClassData[instance.Length];
            for (int index = 0; index < instance.Length; index++)
            {
                returnValue[index] = instance[index].GetData();
            }
            return returnValue;
        }
    }
    public class chemical
    {
        public string matter;
        public float quantity;

        public chemical(string _matter, float _quantity)
        {
            this.matter = _matter;
            this.quantity = _quantity;
        }
        public chemical(ChemicalData data)
        {
            this.matter = data.matter;
            this.quantity = data.quantity;
        }
        public ChemicalData GetData()
        {
            ChemicalData returnValue = new ChemicalData();
            returnValue.matter = this.matter;
            returnValue.quantity = this.quantity;
            return returnValue;
        }
        public static chemical toChemical(ChemicalData data)
        {
            return new chemical(data);
        }
        public static chemical[] toChemical(ChemicalData[] data)
        {
            chemical[] returnValue = new chemical[data.Length];
            for (int index = 0; index < data.Length; index++)
            {
                returnValue[index] = chemical.toChemical(data[index]);
            }
            return returnValue;
        }
        public static ChemicalData[] toChemicalDataArray(chemical[] instance)
        {
            ChemicalData[] returnValue = new ChemicalData[instance.Length];
            for (int index = 0; index < instance.Length; index++)
            {
                returnValue[index] = instance[index].GetData();
            }
            return returnValue;
        }
    }
    public class ChemicalReaction
    {
        public List<chemical> input;
        public List<chemical> output;
        public float responsiveness; // 값이 높을수록 다른 반응들보다 더 먼저 반응합니다.
        public float energy; // 에너지(양수면 반응 후 주변에 에너지를 방출하는 것이고, 음수면 주변의 에너지를 흡수하는 것입니다.)
        // 반응물들을 input의 화학 배열로 1번 나뉘어졌을때마다 나오는 에너지입니다. 이 발생한 에너지 값이 BaseOrganSystem의 impulseTolerance(한계값)값보다 커지면 발생한 에너지 값에 한계값을 뺀만큼 cellAmount를 깎습니다.

        public ChemicalReaction(chemical[] _input, chemical[] _output, float _responsiveness, float _energy)
        {
            input = new List<chemical>(_input);
            output = new List<chemical>(_output);

            //input.CopyTo(_input);
            //output.CopyTo(_output);
            //input = _input;
            //output = _output;
            this.responsiveness = _responsiveness;
            this.energy = _energy;

            //Debug.Log("DEBUG_호출됨 : GameManager.ChemicalReaction(chemical[] _input, chemical[] _output, float _responsiveness, float _energy)");
            //foreach(chemical oneOfInput in input)
            //{
            //    Debug.Log("생성자: 반응물 " + oneOfInput.matter); // 작동함.
            //}
        }
        public ChemicalReaction(ChemicalReactionData data)
        {
            //List<chemical>
            //ChemicalData
            this.input.CopyTo(chemical.toChemical(data.input));
            this.output.CopyTo(chemical.toChemical(data.output));
            this.responsiveness = data.responsiveness;
            this.energy = data.energy;
        }
        // 체크하는 함수.
    }
    #endregion


    #region 직렬화에 도움을 주는 클래스: AttackClassData, ChemicalElementData, ChemicalData, ChemicalReactionData, ChemicalRuleData
    [System.Serializable]
    public class AttackClassData
    {
        public ChemicalData[] chemicals;
        public float mass;
        public float speed;
        public float volume;
    }
    #region AttackClassData생성자
    public static AttackClassData NewAttackClassData()
    {
        AttackClassData returnValue = new AttackClassData();
        returnValue.chemicals = new ChemicalData[] { new ChemicalData() };
        returnValue.chemicals[0].matter = "unknown";
        returnValue.chemicals[0].quantity = 100.0f;
        returnValue.mass = 10.0f;
        returnValue.speed = 10.0f;
        returnValue.volume = 1.0f;
        return returnValue;
    }
    #endregion
    [System.Serializable]
    public class ChemicalElementData
    {
        string[] matter;
    }
    [System.Serializable]
    public class ChemicalData
    {
        public string matter;
        public float quantity;
    }
    [System.Serializable]
    public class ChemicalReactionData
    {
        public ChemicalData[] input;
        public ChemicalData[] output;
        public float responsiveness;
        public float energy;
    }
    [System.Serializable]
    public class ChemicalRuleData
    {
        public ChemicalElementData Element;
        public ChemicalReactionData Reaction;
    }
    #endregion

    #endregion
    // 게임 월드가 시작될 때마다 chemicalReactionTable이 로딩됩니다.
    // chemicalReactionTable은 responsiveness 값이 작은 순서부터 앞에 배치됩니다.


    #region 메서드 / 보조하는 메서드를 위한 클래스
    // 처음 만들 때, 이게 뭔 지랄이냐 싶지만 자주 쓰다보면 편리할거 같아 만들어는 드립니다. 자주 쓰셔야 해요??


    // 잠재적 수정 사항 - Foreach를 for로 바꿔주세요
    public int FindChemical(List<chemical> target, string matter)
    {
        for (int index = 0; index < target.Count; index++)
        {
            if (target[index].matter == matter)
            {
                return index;
            }
        }
        return -1;
    }
    public bool isChemicalsExist(List<chemical> target, List<chemical> search, ref int[] indexArray)
    {
        // 킹고리즘 정리
        // target의 화학물 배열에서 search의 화학물 배열이 전부 있는지 확인하고, 있으면 indexArray에서 target의 index값을 입력합니다. search[n]에 있는 화학물은 target[indexArray[n]]에 존재합니다.

        int[] READYindexArray = new int[search.Count];
        bool isFound = false; // 찾았는지 여부를 저장하기 위한 장치
        int searchIndex = 0;
        int targetIndex = 0;

        foreach (chemical oneOfSearch in search)
        {
            targetIndex = 0;
            foreach (chemical oneOfTarget in target)
            {
                if (oneOfTarget.matter == oneOfSearch.matter)
                {
                    if (oneOfTarget.quantity <= 0.0f) return false; // 찾던 matter는 존재하지만 질량이 양수가 아니므로 없다고 볼 수 있습니다.


                    isFound = true;
                    READYindexArray[searchIndex] = targetIndex;
                }
            }
            if (isFound)
            {
                searchIndex++;
            }
            else
            {
                //returnValue = false;
                //READYindexArray[searchIndex] = -1;
                return false;
            }
        }

        //A) 
        //if (returnValue == true) indexArray = READYindexArray;
        //return returnValue;

        // B)
        //indexArray = READYindexArray;
        //return returnValue;

        indexArray = READYindexArray;
        return true;
    }
    public void ChemicalIndexSet(List<chemical> target, List<chemical> search, ref int[] indexArray)
    {
        // 값을 찾은경우, 그 값은 양수입니다.
        // 값을 찾지 못한 경우. 그 캐미컬의 인덱스는 -1로 리턴합니다.

        // 킹고리즘 반드시 수정
        int[] READYindexArray = new int[search.Count];
        bool isFound = false; // 찾았는지 여부를 저장하기 위한 장치
        int searchIndex = 0;
        int targetIndex = 0;

        foreach (chemical oneOfSearch in search)
        {
            targetIndex = 0;
            foreach (chemical oneOfTarget in target)
            {
                if (oneOfTarget.matter == oneOfSearch.matter)
                {
                    if (oneOfTarget.quantity <= 0.0f) return; // 찾던 matter는 존재하지만 질량이 양수가 아니므로 없다고 볼 수 있습니다.


                    isFound = true;
                    READYindexArray[searchIndex] = targetIndex;
                }
            }
            searchIndex++;
            if (isFound == false)
            {
                READYindexArray[searchIndex] = -1;
            }
        }
    }

    public List<chemical> MixChemical(List<chemical> returnValue, List<chemical> inputValue)
    {
        // <!> ERROR HUNTER <!> : returnValue, inputValue가 null인지 체크
        /*
        if (returnValue == null)
        {
            Debug.LogError("returnValue가 null입니다.");
            return;
        }
        if (inputValue == null)
        {
            Debug.LogError("inputValue가 null입니다.");
            return;
        }
        */

        bool isFound;
        for (int inputIndex = 0; inputIndex < inputValue.Count; inputIndex++)
        {
            isFound = false;
            for (int returnIndex = 0; returnIndex < returnValue.Count; returnIndex++)
            {
                if (returnValue[returnIndex].matter == inputValue[inputIndex].matter)
                {
                    isFound = true;
                    returnValue[returnIndex].quantity += inputValue[inputIndex].quantity;
                }
            }
            if (isFound == false)
            {
                returnValue.Add(inputValue[inputIndex]);
            }
        }
        return returnValue;
    }
    public bool isOkayChemicalsDeductByChemicals(ref List<chemical> target, List<chemical> removeChemicals)
    {
        // 킹고리즘 정리
        // 화학물질이 있는지 확인 및 인덱스 체크
        // removeChemicals의 각각에 접근

        int[] indexArray = new int[removeChemicals.Count];
        List<chemical> tempTarget = new List<chemical>(); // 얕은 복사가 될지 걱정입니다.
        tempTarget = target; // 얕은 복사가 될지 걱정입니다.
        if (!isChemicalsExist(target, removeChemicals, ref indexArray)) return false;

        for (int index = 0; index < removeChemicals.Count; index++)
        {

            if (target[indexArray[index]].quantity <= removeChemicals[index].quantity)
            {
                return false;
            }
            else
            {
                tempTarget[indexArray[index]].quantity -= removeChemicals[index].quantity;
            }
        }

        target = tempTarget;
        return true;
    }
    public void chemicalsMultiply(ref List<chemical> chemicals, float multiplyValue)
    {
        foreach (chemical chemical in chemicals)
        {
            chemical.quantity = chemical.quantity * multiplyValue;
        }
    }
    public List<chemical> chemicalsMultiply(List<chemical> chemicals, float multiplyValue)
    {
        List<chemical> returnValue = new List<chemical>();
        foreach (chemical chemical in chemicals)
        {
            chemical.quantity = chemical.quantity * multiplyValue;
            returnValue.Add(chemical);
        }
        return returnValue;
    }


    public float chemicalsDivideByChemicals(List<chemical> chemicals1, List<chemical> chemicals2) // chemical1을 chemical2로 나눈 값을 실수형으로 리턴합니다.
    {
        int[] indexArray = new int[chemicals2.Count];
        float[] shareArray = new float[chemicals2.Count];
        float returnValue;

        if (!isChemicalsExist(chemicals1, chemicals2, ref indexArray)) return 0.0f;

        // chemicals2의 각각의 원소에 접근하여 나눠봅니다.
        for (int index = 0; index < chemicals2.Count; index++)
        {
            shareArray[index] = chemicals1[indexArray[index]].quantity / chemicals2[index].quantity;
        }

        // 가장 작은 값을 구합니다.

        if (chemicals2.Count > 0)
            returnValue = shareArray[0];
        else // <!> ERROR HUNTER <!>
        {
            Debug.LogError("<!> ERROR : GameManager.chemicalsDivideByChemicals에 전달된 인수 중, chemicals2의 요소 수가 0입니다.");
            return -1.0f;
        }
        foreach (float oneOfShareArray in shareArray)
        {
            returnValue = Mathf.Min(returnValue, oneOfShareArray);
        }

        return returnValue;
    }
    public List<chemical> ChemicalDemandCheck(List<chemical> needChemical, List<chemical> holdingChemical)
    {
        // 함수 설명: 필요 화학물질과 보유 화학물질을 비교하여 어떤 화학물질이 얼만큼 부족한지 리턴합니다.
        // 필요 화학물질을 기준으로 합니다.

        List<chemical> returnValue = new List<chemical>();

        for (int i = 0; i < needChemical.Count; i++)
        {
            bool isFound = false;
            for (int j = 0; j < holdingChemical.Count; j++)
            {
                if (needChemical[i].matter == holdingChemical[j].matter)
                {
                    float deltaValue = needChemical[i].quantity - holdingChemical[j].quantity;
                    if (deltaValue <= 0.0f) continue; // 부족함이 없으므로 패스.

                    // 같은 물질을 찾았습니다.
                    returnValue.Add(new chemical(needChemical[i].matter, deltaValue));
                    isFound = true;
                }
            }
            if (isFound == false)
            {
                returnValue.Add(needChemical[i]);
            }
        }

        return returnValue;
    }
    public class ChemicalReactionComparerByResponsiveness : IComparer<ChemicalReaction>
    {
        public ChemicalReactionComparerByResponsiveness()
        {
            //Debug.Log("FA Q");
        }

        public int Compare(ChemicalReaction x, ChemicalReaction y)
        {
            //Debug.Log("haha ha");
            if (x.responsiveness < y.responsiveness)
                return -1;
            else if (x.responsiveness == y.responsiveness)
                return 0;
            else
                return 1;
        }
    }
    IComparer<ChemicalReaction> chemicalReactionComparer;

    public void chemicalReactionFunc(ref float EnergyPath, ref List<chemical> chemicals)
    {
        // 킹고리즘 정리
        // 1. 있는지 확인
        // 2. 있으면 나누기 작업
        // 3. 몫을 구하고 빼는 작업
        // 4. 에너지 처리작업
        bool chemicalReactionExist = false;
        do
        {
            chemicalReactionExist = false;
            foreach (ChemicalReaction oneOfChemicalReaction in chemicalReactionTable)
            {
                // 1. 있는지 확인
                int[] indexArray = new int[oneOfChemicalReaction.input.Count];
                if (!isChemicalsExist(chemicals, oneOfChemicalReaction.input, ref indexArray)) continue;
                // 2. 있으면 나누기 작업
                float share;
                share = chemicalsDivideByChemicals(chemicals, oneOfChemicalReaction.input);
                if (share == 0.0f) continue; // 나눌 수 없는 경우입니다.

                // 3. 몫을 구하고 빼는 작업
                List<chemical> assignedChemicalsForRemove = oneOfChemicalReaction.input;
                chemicalsMultiply(ref assignedChemicalsForRemove, share);
                if (!isOkayChemicalsDeductByChemicals(ref chemicals, assignedChemicalsForRemove)) continue;

                // 4. 에너지 처리작업
                EnergyPath += share * oneOfChemicalReaction.energy;
                chemicalReactionExist = true;
            }
        }
        while (chemicalReactionExist); // 남는 화학 반응이 없어질 때까지 루프.
        // 화학 반응이 일어나는지.


        // 배열에서 나올수 있는 ChemicalReaction을 chemicalReactionTable에서 찾아냅니다. (responsiveness가 가장 큰 값부터)
        //





        // 화학 반응이 발생했다면 에너지를 리턴합니다.
    }

    #endregion


    #endregion
    #region MachineHelper : WorkEvent, NetworkHelper,




    #region WorkEvent

    public event VoidToVoid MachineWorkEvent; // 실시간 행동이 필요한 머신은 이 이벤트에 익명 메소드를 추가합니다.
    IEnumerator RunMachineWork()
    {
        while (true)
        {
            MachineWorkEvent();
            yield return new WaitForSeconds(0.1f);
        }
    }


    #endregion
    #region NetworkHelper






    #endregion



    #endregion

    #region Field Manager
    // 필드 매니저의 역할.
    // "불러오기, 수정, 저장"

    // 전체 필드에 존재하는 유닛들을 저장하고, 필드에 존재하는 것들을 끌어들입니다. 반대로 전투가 끝나면 유닛과 타일들을 저장합니다.
    // 1. 야전에 유닛들과 타일을 불러오거나, 끝나면 Json 파일로 저장하는 역할을 맡습니다.
    // (-) 유닛 연구 상태, 어떤 유닛을 생산했는지에 대한 정보는 저장하지 않습니다. 다른 매니저에게 이 일을 맡기십시오.
    // 만약 다른 정보와 상호작용을 원한다면 게임매니저 데이터를 참고하십시오.

    // SetUp
    // Assets/GameFile/Field에서 파일이 있는지 체크합니다.
    // 유닛 정보 / 팀 정보 / 스쿼드 정보를 긁어오거나 데모 데이터를 필드에 저장합니다.

    #region Class - Field Manager

    #region Base - Class-Field Manager

    //public class Field
    //{
    //    public List<BaseUnitData> unitDatas;
    //    public List<HumanUnitInfoData> humanUnitDatas;
    //    public List<MachineUnitInfoData> machineUnitDatas;
    //
    //    public List<TeamData> teamDatas;
    //}

    [System.Serializable]
    public class FieldManager
    {
        public FieldData fieldData
        {
            get; set;
        }
    }

    [System.Serializable]
    public class FieldData // <!> 아직 완벽하지 않은 데이터입니다! 야전에 추가할 정보가 있으면 더 추가해주세요.
    {
        //
        // FIELD
        //
        bool fieldLock = true; // 이 fieldLock이 설정되면 데이터가 들어가지 않습니다.

        // 현재 Field에 보여주고 있는 플레이어와 지역, 입장 등에 대한 데이터가 저장되는 곳입니다.
        public string playerTeamName; // 플레이어가 통제하는 팀의 이름입니다.
        public string[] currentPlayLocation; // 현재 플레이 하고 있는 공간의 이름입니다.
                                             // 해당 지역의 타일 배치가 실행되고, 거기에 존재하는 유닛도 배치됩니다.
                                             // <!> 스쿼드도 지역 이름따라 배치될 수있지만, 같은 장소에 다른 미션들을 받은경우가 있으면 어떻게 할지 로직이 필요합니다.
                                             // 만약 동시에 두개 이상의 공간을 보여주려면 여러 지역 이름을 나열합니다.
                                             // 현재 플레이어의 목표는 스쿼드의 미션을 확인하세요. 현재 위치에 존재하는 스쿼드의 미션이 곧 현재의 목표입니다.
                                             //

        // 여러 야전의 전반적인 데이터가 저장되는 곳입니다.
        public TeamData[] teamsAndSquads; // 분대에 대한 정보, 유닛이 존재하는 지역을 담습니다

        // 유닛에 대한 정보입니다.
        public BaseUnitData[] unitDatas;
        public HumanUnitInfoData[] humanUnitDatas;
        public MachineUnitInfoData[] machineUnitDatas;
        // 유닛 프리펩에 붙는 컴포넌트에 대한 정보입니다.
        // 휴먼 유닛
        public UnitBase.UnitBaseData[] unitBaseComponentData;
        public HumanUnitBase.OrganListData[] organListComponentData;
        public UnitItemPack.UnitItemPackData[] unitItemPackComponentData;
        // 머신 유닛 - 머신 번호 순서 -> ABC순서
        // 머신 컴포넌트에 대한 어레이입니다.



        // 타일에 대한 정보라던가
        public BaseTileData[] tileDatas;

        // 미션에 대한 정보라던가
        public MissionData[] missionDatas;
        public string GameCurrentLocation; // 현재 플레이하고 있는 장소입니다.




        //
        // FUNCTION
        //

        public void AddUnitData(BaseUnitData baseUnitData, HumanUnitInfoData humanUnitInfoData, params object[] componentDatas)
        {

        }
        public void AddUnitData(BaseUnitData baseUnitData, MachineUnitInfoData machineUnitInfoData, params object[] componentDatas)
        {

        }

        public BaseUnitData TryGetBaseUnitData(int id)
        {
            if (unitDatas == null) return null;

            for (int index = 0; index < unitDatas.Length; index++)
            {
                if (unitDatas[index].ID == id)
                {
                    return unitDatas[index];
                }
            }

            return null;
        }
    }


    #endregion
    #region Unit - Class-Field Manager

    #region 공통: 유닛
    [System.Serializable]
    public class BaseUnitData
    {
        // 기본 정보
        public int ID; // WorldManager에서 저장되는아이디입니다. 유닛을 구분하는 유니크한 식별번호입니다. // WorldManager.GetNewUnitID를 호출해야 합니다. // 0부터 시작하여 1씩 늘어납니다.
        public string unitType; // Human인지, Machine인지 구분합니다.
        public string gameObjectName; // 게임오브젝트 이름입니다.
        public bool isDummyData;

        // 위치
        public string nameOfLocation; // 유닛이 존재하는 위치
        public bool isUnitStayedThatPlace; // 유닛이 주둔군인지 여부
        // isUnitStayedThatPlace == true이면 사용되는 필드입니다.
        public Vector3 position;
        public Vector3 direction;
        // isUnitStayedThatPlace == false이면 사용되는 필드입니다.
        public string nameOfLocationPast; // 이전 턴에 위치한 지역의 이름. 어느 비콘에 배치시킬지 결정합니다.

        // 컴포넌트 정보
        public string[] usingComponentNames; // 이 유닛이 사용하는 컴포넌트 이름 배열입니다. 이를 통해 컴포넌트 데이터를 찾을 수 있습니다.
        public int[] usingComponentNameIndex; // usingComponentNames로 찾은 컴포넌트 배열중 몇 번째가 자신의 것인지 체크합니다.
        // 1. 자신의 컴포넌트를 배열에 저장, 2. usingComponentNames에 컴포넌트 이름을 올리고 usingComponentNameIndex에 해당 인덱스를 찾아 넣습니다.
        // 컴포넌트 데이터 타입, Type을 String으로 기록한 매개변수, 그리고 필드 데이터 매개변수, 그리고 배이스 유닛 데이터 매개변수가 필요합니다.

        public int FieldInstanceID; //필드에서만 사용될 데이터입니다.
        // <이 클래스의 필드는 유니티 컴포넌트로부터 자유롭습니다. 추가적으로 필요한 컴포넌트 데이터가 있으면 그 컴포넌트가 직접 알아서 불러오도록 합니다.>
        // + 유닛에 들어가는 컴포넌트 데이터들은 아이디와 데이터의 목록이 들어있는 Json파일을 각각 구해옵니다
        // 데이터는 ID에 맞게 불러옵니다. 자신의 ID가 저장되어 있으면 불러오고 그렇지 못하면 디폴트 데이터를 불러옵니다.
        public BaseUnitData()
        {

        }
        public BaseUnitData(string gameObjectName)
        {

        }
    }

    public class BaseUnitReal
    {
        public BaseUnitData unitData;
        public GameObject thisGameObject;
        public int instanceID;
    }


    #endregion
    #region 인간: 유닛, 스쿼드, 팀
    [System.Serializable]
    public class HumanUnitInfoData
    {
        // 0. 발생 시에서만 만들어지는 값, 이후 수정이 불가능한 값.
        public int BaseUnitDataID;

        // 1. 야전이 시작하면서 외부에서 삽입받는 정보입니다.
        // 1.1. 보유한 컴포넌트, 정보 (생성되면서 필요한 정보입니다.)
        public string charactor; // 프리펩 외형 이름입니다.

        // 프리펩의 컴포넌트 데이터는 상위 클래스인 필드데이터에 존재합니다. // 아래 3개의 필드는 사용하지 않습니다.
        //public UnitBase.UnitBaseData unitBaseData;
        //public HumanUnitBase.OrganListData organListData;
        //public UnitItemPack.UnitItemPackData unitItemPackData;

        // 1.2. 주변 객체와 관계를 담은 정보
        public int SquadID;
        public int TeamID;

        public HumanUnitInfoData()
        {

        }

        public HumanUnitInfoData(BaseUnitData baseUnitData, SquadData squadData, TeamData teamData, string charactor)
        {
            BaseUnitDataID = baseUnitData.ID;
            SquadID = squadData.SquadID;
            TeamID = teamData.ID;
        }
    }

    /*
    public class UnitInfo // 버려질 클래스입니다.
    {
        // 0. 발생 시에서만 만들어지는 값, 이후 수정이 불가능한 값.
        public string ID; // 세계가 발생할 때, 몇 번째로 생성된 유닛인지 알립니다.
        // 1. 야전이 시작하면서 외부에서 삽입받는 정보입니다.
        // 1.1. 보유한 컴포넌트, 정보 (생성되면서 필요한 정보입니다.)
        public string charactor; // 프리펩의 외형이 달라집니다.
        public UnitBase.UnitBaseData unitBaseData;
        public HumanUnitBase.OrganListData organListData;
        public UnitItemPack.UnitItemPackData unitItemPackData;
        public bool isGarrisonForHoldingPosition; // 주둔군인지 여부 판단.
        public Vector3 HoldingPosition; // 주둔군이라면 어디에 위치하는지에 대한 정보

        // 1.2. 주변 객체와 관계를 담은 정보
        public string SquadName; // 스쿼드 이름(아이디) - 스쿼드 아이디는 플레이어가 스쿼드를 결성할 때마다 결정됩니다.(무소속 및 스쿼드 배치가 없을경우 -1)
        public string TeamName; // 팀 정보 - 세계가 시작하면서 결정됩니다.(무소속일경우 -1)

        // 2. 게임 플레이 중간에 필요한 정보(생성되면서 결정되는 정보.)
        public GameObject unitGameObject; // 필드에 등장하는 게임오브젝트에 대한 정보는 유일하게 여기에 저장하도록 합시다.
        public int InstanceID; // 유닛의 GameObject의 InstanceID를 저장합니다. UnitID를 Key로 저장하고 UnitInfo를 Value로 저장할때도 유용합니다.
    }
    /**/

    public class Squad
    {
        // 멤버에 대한 정보 저장.
        // 분대의 위치 저장.
        // 분대의 이름
        // 분대 아이콘(?)

        public int squadID; // 스쿼드의 아이디 // 이 세계에서 몇 번째로 생성된 스쿼드인지 기록합니다.
        public string name; // 표시될 스쿼드의 이름 // 플레이어가 지정한 이름입니다.
        public string nameOfLocation; // 위치한 지역의 이름
        //public int squadID;
        //public List<UnitInfo> member;

        public List<int> memberID; //


    }
    public class Team
    {
        public string name;
        public List<Squad> squads;
        public Dictionary<Vector3, int> memoryMap; // 휘발성 메모리 맵입니다.
    }


    /*
    [System.Serializable]
    public class UnitInfoData
    {
        // 0. 발생 시에서만 만들어지는 값, 이후 수정이 불가능한 값.
        public string ID; // 세계가 발생할 때, 몇 번째로 생성된 유닛인지 알립니다.
        // 1. 야전이 시작하면서 외부에서 삽입받는 정보입니다.
        // 1.1. 보유한 컴포넌트, 정보 (생성되면서 필요한 정보입니다.)
        public string charactor; // 프리펩의 외형이 달라집니다.
        public string[] usingComponentNames; // 이 유닛이 사용하는 컴포넌트 이름 배열입니다. 이를 통해 컴포넌트 데이터를 찾을 수 있습니다.
        public int[] usingComponentNameIndex; // usingComponentNames로 찾은 컴포넌트 배열중 몇 번째가 자신의 것인지 체크합니다.

        public UnitBase.UnitBaseData unitBaseData;
        public HumanUnitBase.OrganListData organListData;
        public UnitItemPack.UnitItemPackData unitItemPackData;
        public bool isGarrisonForHoldingPosition; // 주둔군인지 여부 판단.
        public Vector3 HoldingPosition; // 주둔군이라면 어디에 위치하는지에 대한 정보

        // 1.2. 주변 객체와 관계를 담은 정보
        public string SquadName; // 스쿼드 이름(아이디) - 스쿼드 아이디는 플레이어가 스쿼드를 결성할 때마다 결정됩니다.(무소속 및 스쿼드 배치가 없을경우 -1)
        public string TeamName; // 팀 정보 - 세계가 시작하면서 결정됩니다.(무소속일경우 -1)

        // 1.3. 부스터

    }
    /**/
    [System.Serializable]
    public class SquadData
    {
        public int SquadID; // 0부터 시작하여 생성된 순서대로 배정받습니다
        public string name;
        public string nameOfLocation;
        public string nameOfPastLocation;
        public int[] memberID;
        public string assingedMission; // 명령받은 미션입니다.
        public bool isDummyData;
        public bool isMachineSquad;
    }
    [System.Serializable]
    public class TeamData
    {
        public string name;
        public int ID;
        public SquadData[] squads;
    }
    #endregion
    #region 머신: 머신유닛인포메이션
    public class MachineUnitInfo
    {
        // 1. 야전이 시작하면서 외부에서 삽입받는 정보입니다.
        // 1.1. 보유한 컴포넌트, 정보 (생성되면서 필요한 정보입니다.)
        public int BaseUnitDataID; // 유닛 데이터의 아이디 값입니다.
        public string machinePrefabName; // 프리펩의 이름입니다.
        public int MachineUnitID; // 머신의 식별번호입니다.

        // 2. 게임 플레이 중간에 필요한 정보(생성되면서 결정되는 정보.)
        public GameObject unitGameObject; // 필드에 등장하는 게임오브젝트에 대한 정보는 유일하게 여기에 저장하도록 합시다.
        public int InstanceID; // 유닛의 GameObject의 InstanceID를 저장합니다. UnitID를 Key로 저장하고 UnitInfo를 Value로 저장할때도 유용합니다.
    }
    [System.Serializable]
    public class MachineUnitInfoData
    {
        // 1. 야전이 시작하면서 외부에서 삽입받는 정보입니다.
        // 1.1. 보유한 컴포넌트, 정보 (생성되면서 필요한 정보입니다.)
        public string machinePrefabName; // 프리펩의 이름입니다.
        public int MachineUnitID; // 머신의 식별번호입니다.

    }



    #endregion



    #endregion
    #region Tile - Class-Field Manager
    [System.Serializable]
    public class BlockData
    {
        #region 어디에 쓰이는 클래슨고?
        // 필드를 로딩하거나 저장할때, 블럭들이 어디에 어떤 블럭이 있었는지를 저장하는 용도의 클래스입니다.
        // 또는 유니티를 이용해 자기만의 맵을 만들었을때도 저장하는 용도입니다.

        #region 필드 설명
        // BlockID
        // 어떤 블럭인가
        // 블럭의 형태뿐 만 아니라 어떤 텍스쳐의 블럭을 사용하였는가도 저장하는 용도입니다.
        #endregion
        #endregion
        public int blockID;
        public Vector3 blockPosition;
    }


    [System.Serializable]
    [Obsolete("이 데이터는 구식입니다. BlockData 형식을 이용하세요.")]
    public class BaseTileData
    {
        public int TileID;
        public string PrefabName;

        public Vector3 position;
    }
    #endregion
    #region Mission - Class-Field Manager

    [System.Serializable]
    public class MissionData
    {
        public string missionLocation;
        public string objective;
        // 팀마다 목표가 다릅니다.
    }
    #endregion

    #endregion



    #region TestCode
    bool TestMode; // true이면 프로그램이 실행할 때마다 테스트를 진행합니다.


    //1. 폴더에 있는 JSON 파일을 가져오거나, 중간에 삽입되어 얻은 값들로, 유닛을 생성합니다.

    // 1.1. 파일을 가져와 변수에 저장하는 것
    // 조건. JsonString을 변수로 저장했을때, 변수에 미할당 기본값이 남아있는지 체크 할 것.
    bool testLoading(FieldData field)
    {
        return false;
    }
    // 1.2. 변수의 값으로 유닛을 생성하는 것.
    // 조건. 유닛을 생성하고 이벤트를 호출하고 그 유닛이 응답해주는 함수를 호출하였을때, 누락된 유닛이 있는지 체크할 것. 유닛의 목록과 응답한 유닛의 목록을 비교하라.

    // 1.2.1. 유닛의 컴포넌트와 세부 사항.
    // 조건. 컴포넌트에 값이 들어갔느냐 기본값이 아니라 할당된 값 여부를 판단.

    // 2. 게임매니저가 필드에 존재하는 유닛들에게 정보를 긁어와 Json 파일로 만드는 것

    // 




    #endregion
    #region Delegate
    public delegate void VoidToVoid();

    #endregion
    #region Event

    #endregion
    #region Field - Field Manager



    #region 호출 순서가 중요한 필드

    // 필드는 호출 순서대로 넣습니다.



    // 로딩시 넣는 임시 데이터




    // 
    public delegate void UnitInfoDataHandler(ref FieldData myData);
    public event UnitInfoDataHandler BeconEventForUnit = delegate (ref FieldData myData) { };
    //public event UnitInfoDataHandler BeconEventForUnit = delegate (ref List<UnitInfoData> myData) { };

    // 팀/스쿼드/유닛

    public FieldData currentFieldData; // 현재의 필드 데이터입니다. 데이터 로딩으로 먼저 채워진 후, 비콘으로 데이터가 추가로 채워지고 난 뒤, 게임 도중에 다시 수정 될 수 있습니다.
    //public List<Team> teamList;
    //public List<Squad> squadList;
    //public List<UnitInfo> unitInfoList;// 지워질 데이터,
    public List<BaseUnitData> baseUnitList;
    public List<HumanUnitInfoData> humanUnitList;
    public List<MachineUnitInfoData> machineUnitList;
    // 주석한 부분 인스턴스 아이디를 키값으로 하고, 데이터 상으로 존재하는 유닛 정보를 벨류값으로 하는 딕셔너리 아이디어였습니다.
    //public Dictionary<int, int> InstanceIdToUnitId; // gameObject.GetInstanceID()값을 세계 내부의 고정값인 UnitInfoID값으로 바꿔줍니다.
    //public Dictionary<int, BaseUnitData> UnitDictionary; // 필드에 존재하는 유닛들
    public Dictionary<int, SquadData> SquadDictionary;
    public Dictionary<int, Team> TeamDictionary;

    // 플레이어가 조종할 팀 설정
    public string playerTeam;

    #endregion
    #region 유닛 필드



    //public List<Team> teams;
    Team Alpha;
    Team Beta;
    Team Neutral;

    int LastSpawnedUnitId;



    #endregion


    #region Prefabs
    #region UnitPrefabs
    #region Human Prefabs And Skin
    public GameObject HumanPrefabDefaultSkin;
    public GameObject HumanPrefabKartSkin;

    // charactor 따라서 prefab를 다른걸로 설정하여 instantiate의 첫번째 매개변수를 결정합니다.
    #endregion
    #region Machine Prefabs
    public GameObject EnergyStorage; // 101
    public GameObject EnergyTransmission; // 102
    public GameObject EnergyFarm; // 103
    public GameObject PlacementFixture; // 201
    public GameObject PlacementDrone; // 202
    // spider had been terminated
    public GameObject PlacementRcCar; // 204
    public GameObject SensorString; // 301
    public GameObject SensorCamera; // 302
    public GameObject NetworkSircuit; // 501
    public GameObject NetworkAntenna; // 502
    public GameObject AttackBomb; // 701
    public GameObject Attackturret; // 702
    public GameObject DisturberWall; // 801
    public GameObject DisturberCamo_hologram; // 802
    public GameObject DisturberLight; // 803
    public GameObject ProductMine; // 901
    public GameObject ProductMove; // 902
    public GameObject ProductCraft; // 903
    public GameObject ProductSave; // 904
    #endregion
    #endregion
    #region TilePrefabs
    #region Floor

    #endregion
    #region Wall

    #endregion


    #endregion

    #endregion




    #endregion
    
    #region Function - Field Manager

    #region generic Func
    public int AddElementInArray<T>(ref T[] array, T element)
    {
        // array에 null이 있는곳을 찾습니다.
        // null이 있다면 그곳에 element을 집어넣습니다.
        // null이 없다면 크기를 늘린 뒤 그곳에 element를 집어넣습니다.
        // 넣는 것에 성공했다면 해당 인덱스를 리턴합니다.

        if (array == null)
        {
            array = new T[] { element };
            return 0;
        }
        for (int index = 0; index < array.Length; index++)
        {
            if (array[index] == null)
            {
                array[index] = element;
                return index;
            }
        }
        Array.Resize(ref array, array.Length + 1);
        array[array.Length - 1] = element;
        return array.Length - 1;
    }
    #endregion

    #region FieldData 관련 함수
    #region region: AddComponentData에 쓰이는 함수
    void AddComponentDataHelper<ComponentDataClass>(ref ComponentDataClass[] ComponentDataArray, ref BaseUnitData baseUnitData, ComponentDataClass componentData, string componentName)
    {
        // 함수 설명
        //

        int index1 = AddElementInArray(ref ComponentDataArray, componentData);
        int index2 = AddElementInArray(ref baseUnitData.usingComponentNames, componentName);
        int index3 = AddElementInArray(ref baseUnitData.usingComponentNameIndex, index1);
        //Weak Error Hnuter
        if (index2 != index3)
        {
            Debug.Log("WARNING_GameManager.AddComponentDataHelper: 컴포넌트 이름을 저장하는 인덱스와 인덱스 위치를 저장하는 인덱스가 다릅니다.");
        }
    }
    #endregion
    public void AddComponentData(UnitBase.UnitBaseData componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    {
        AddComponentDataHelper(ref targetFieldData.unitBaseComponentData, ref baseUnitData, componentData, "UnitBase");
    }
    public void AddComponentData(HumanUnitBase.OrganListData componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    {
        AddComponentDataHelper(ref targetFieldData.organListComponentData, ref baseUnitData, componentData, "HumanUnitBase");
    }
    public void AddComponentData(UnitItemPack.UnitItemPackData componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    {
        AddComponentDataHelper(ref targetFieldData.unitItemPackComponentData, ref baseUnitData, componentData, "UnitItemPack");
    }

    public void AddComponentData<ComponentDataClass>(ComponentDataClass componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    {
        // 컴포넌트의 데이터를 FieldData에 집어넣고 BaseUnitData에 컴포넌트 이름과 자기 컴포넌트가 위치한 인덱스를 기록합니다.
        // AddComponentDataHelper는 반복되는 부분을 줄인 부분입니다. 윗부분 Region을 참고하세요

        Dictionary<Type, int> typeIntPairs = new Dictionary<Type, int>();
        typeIntPairs.Add(typeof(UnitBase.UnitBaseData), 100);
        typeIntPairs.Add(typeof(HumanUnitBase.OrganListData), 201);
        typeIntPairs.Add(typeof(UnitItemPack.UnitItemPackData), 202);


        // Switch문을 이용하면 중괄호가 하나로 뭉쳐지는 점으로 인해 제네릭 함수를 사용하기 곤란한 이유입니다.
        
        switch (typeIntPairs[componentData.GetType()])
        {

            case 100:
                {
                    //AddComponentDataHelper<UnitBase.UnitBaseData>(ref targetFieldData.unitBaseComponentData, ref baseUnitData, componentData, "UnitBase"); // ERROR
                    //AddComponentDataHelper(ref targetFieldData.unitBaseComponentData, ref baseUnitData, componentData, "UnitBase"); // ERROR
                }
                break;
            case 201:
                {
                    //AddComponentDataHelper<HumanUnitBase.OrganListData>(ref targetFieldData.organListComponentData, ref baseUnitData, componentData, "HumanUnitBase"); // ERROR
                    //AddComponentDataHelper(ref targetFieldData.organListComponentData, ref baseUnitData, componentData, "HumanUnitBase"); // ERROR
                }
                break;
            case 202:
                {
                    if (componentData.GetType() == typeof(UnitItemPack.UnitItemPackData))
                    {
                        //AddComponentDataHelper<UnitItemPack.UnitItemPackData>(ref targetFieldData.unitItemPackComponentData, ref baseUnitData, componentData, "UnitItemPack"); // ERROR

                    }

                    //AddComponentDataHelper<UnitItemPack.UnitItemPackData>(ref targetFieldData.unitItemPackComponentData, ref baseUnitData, componentData, "UnitItemPack"); // ERROR
                }
                break;
            default:
                Debug.Log("DEBUG_GameManager.AddComponentData: 매개변수로 받은 componentData는 FieldData에 추가할 수 있는 클래스가 아닙니다.");
                break;



        }
    }

    #endregion

    // 이 함수는 내수용 함수인듯 합니다.
    void SetUp() // 기본적인 세팅에 대한 설명입니다. FieldManager의 핵심 함수입니다.
    {
        // 함수 설명: 파일에 있는 정보를 통해 UnitDictionary, SquadDictionary, TeamDictionary에 값을 넣습니다.
        // 아이디어 게시판
        // 파일이 있는 주체가 직접 데이터를 넣습니다. -> 쉽지만 일일히 넣어야 함
        // 이 프로그램이 파일에 직접 접근해 데이터를 넣습니다. -> 편하지만 어려움 -> 이것을 선택

        // 킹고리즘 정리
        //

        if (GetComponent<BattleField_Demo_Demo>() != null)
        {

        }

        // 팀 설정


        // 스쿼드 설정



        //TeamDictionary.Add(0, new Team())

        //teams = new Team[3];

        //teams = new List<Team>();
        //teams.Add(Neutral);
        //teams.Add(Alpha);
        //teams.Add(Beta);
        //teams[0] = Neutral;
        //teams[0] = Neutral;
        //teams[1] = Alpha;
        //teams[2] = Beta;



        LastSpawnedUnitId = 0;

        //
        //for(int i = 0; i)


        //foreach (Team team in teams) // 필요한 것 - 각 컴포넌트마다 데이터를 저장하는 클래스를 만들 것
        //{
        //    team.memoryMap = new Dictionary<Vector3, int>();
        //    foreach (Squad squad in team.squads)
        //    {
        //        foreach (UnitInfo info in squad.member)
        //        {
        //            // 유닛 인스턴스화
        //            GameObject tempUnitPrefab;
        //            if (info.charactor == "kart") tempUnitPrefab = HumanPrefabKartSkin;
        //            else tempUnitPrefab = HumanPrefabDefaultSkin;
        //            info.unitGameObject = Instantiate(tempUnitPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        //            // 인스턴스된 게임오브젝트의 각 컴포넌트마다 데이터를 집어넣습니다. 컴포넌트의 정보를 저장하는 ~Data 클래스를 참고하세요
        //            // 유닛 기반 정보 넣기 작업 id 값으로는 LastSpawnedUnitId 사용
        //            info.unitGameObject.GetComponent<UnitBase>().UnitBaseDataNewSet(info.unitBaseData.UnitType, LastSpawnedUnitId); LastSpawnedUnitId++;
        //            // 유닛 건강 상태 넣기 작업
        //            // 아이템 넣기 작업
        //            info.unitGameObject.GetComponent<UnitItemPack>().InventorySet(info.unitItemPackData.inventory[0], info.unitItemPackData.inventory[1], info.unitItemPackData.inventory[2]);
        //            //info.unitGameObject.GetComponent<>
        //            //
        //        }
        //    }
        //}


    }
    #region Field Booting Function
    void FieldSetup() // 핵심 함수
    {
        PlayerSetup("Player");

        // 1. 변수 준비
        // 2. JSON 파일을 받음
        // 3. 유닛 비콘이벤트, 스폰포인트 비콘이벤트 등으로 변수에 추가 정보를 주입
        // 4. 유닛 인스턴스화 작업


        // Field 파일에 값이 존재하는지 체크합니다
        FieldData fieldData = new FieldData();

        // 파일 로딩 -> FieldData fieldData에 저장,
        // 만약 실패했다면 더미 데이터를 집어넣습니다.
        if (DataLoading("Assets/GameFile/Field/fieldData.json", ref fieldData) == false)
        {
            fieldData = DemoFieldLoader();
        }


        //if (fieldData.units != null) // 필드 데이터에 쓸 수 있는 값을 찾았습니다!
        //{
        //    LoadedUnitData = new List<UnitInfoData>(fieldData.units);
        //}



        // 이벤트로 외부에 있는 비콘에 유닛 인포를 추가로 넣어달라고 요청합니다.
        BeconEventForUnit(ref fieldData);

        //fieldData.units = LoadedUnitData.ToArray();



        // UnitInfoList로 인스턴스화 하고 값 넣기
        UnitInstantiate(ref fieldData);

        // 필드데이터에 값을 넣기
        currentFieldData = fieldData;
    }
    FieldData DemoFieldLoader()
    {
        // fieldData에 임시 데이터를 집어넣습니다.

        // Team PlayerTeam
        // -자신의 스쿼드 2부대: 엔지니어, 공격대
        // --엔지니어: 감시자, 공병2
        // --공격대: 사수 4명
        // Team EnemyTeam
        // -자신의 스쿼드 1부대: 공격대

        FieldData returnValue = new FieldData();
        returnValue.playerTeamName = "Player";
        returnValue.teamsAndSquads = new TeamData[2];
        #region teamsAndSquads


        // teamsAndSquads[0]
        TeamData playerTeam = new TeamData();
        playerTeam.name = "Player";
        playerTeam.ID = GetNewTeamID(returnValue);
        playerTeam.squads = new SquadData[2] { new SquadData(), new SquadData() };
        #region Player Squad 1


        playerTeam.squads[0].SquadID = GetNewSquadID(returnValue);
        playerTeam.squads[0].name = "PlayerStriker001";
        playerTeam.squads[0].nameOfLocation = "Demo";
        playerTeam.squads[0].nameOfPastLocation = "AlphaHeadquarter";
        // 바깥에서 멤버 아이디 추가해 줄 것입니다.
        //playerTeam.squads[0].memberID = new int[4]; // 멤버도 추가할 것

        #region Squad Member 1 감시자 1
        // BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        // BaseUnitData 정보 채우기
        BaseUnitData PlayerStriker001_Attacker = new BaseUnitData();
        PlayerStriker001_Attacker.direction = new Vector3(0, 0, -1);
        PlayerStriker001_Attacker.ID = GetNewUnitID(returnValue);
        PlayerStriker001_Attacker.unitType = "human";
        // UnitBase, OrganData, ItemData 추가하기
        AddComponentData(new UnitBase.UnitBaseData(PlayerStriker001_Attacker, "Player"), ref returnValue, ref PlayerStriker001_Attacker);
        AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker001_Attacker);
        AddComponentData(new UnitItemPack.UnitItemPackData("Radios", "Pistol", "Knife"), ref returnValue, ref PlayerStriker001_Attacker);
        // Add this to there
        AddElementInArray(ref returnValue.unitDatas, PlayerStriker001_Attacker);
        AddElementInArray(ref playerTeam.squads[0].memberID, PlayerStriker001_Attacker.ID);

        HumanUnitInfoData PlayerStriker001_AttackerHuman = new HumanUnitInfoData();
        PlayerStriker001_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        PlayerStriker001_AttackerHuman.BaseUnitDataID = PlayerStriker001_Attacker.ID;
        PlayerStriker001_AttackerHuman.charactor = "kart";
        PlayerStriker001_AttackerHuman.TeamID = playerTeam.ID;
        AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker001_AttackerHuman);
        #endregion
        #region Squad Member 2 공격대 1
        // BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        // BaseUnitData 정보 채우기
        BaseUnitData PlayerStriker002_Attacker = new BaseUnitData();
        PlayerStriker002_Attacker.direction = new Vector3(0, 0, -1);
        PlayerStriker002_Attacker.ID = GetNewUnitID(returnValue);
        PlayerStriker002_Attacker.unitType = "human";

        // Add this to there
        AddElementInArray(ref returnValue.unitDatas, PlayerStriker002_Attacker);
        AddElementInArray(ref playerTeam.squads[0].memberID, PlayerStriker002_Attacker.ID);

        // UnitBase, OrganData, ItemData 추가하기
        AddComponentData(new UnitBase.UnitBaseData(PlayerStriker002_Attacker, "Player"), ref returnValue, ref PlayerStriker002_Attacker);
        AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker002_Attacker);
        AddComponentData(new UnitItemPack.UnitItemPackData("Blank", "Pistol", "Knife"), ref returnValue, ref PlayerStriker002_Attacker);

        HumanUnitInfoData PlayerStriker002_AttackerHuman = new HumanUnitInfoData();
        PlayerStriker002_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        PlayerStriker002_AttackerHuman.BaseUnitDataID = PlayerStriker002_Attacker.ID;
        PlayerStriker002_AttackerHuman.charactor = "kart";
        PlayerStriker002_AttackerHuman.TeamID = playerTeam.ID;
        AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker002_AttackerHuman);
        #endregion
        #region Squad Member 2 공격대 2
        // BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        // BaseUnitData 정보 채우기
        BaseUnitData PlayerStriker003_Attacker = new BaseUnitData();
        PlayerStriker003_Attacker.direction = new Vector3(0, 0, -1);
        PlayerStriker003_Attacker.ID = GetNewUnitID(returnValue);
        PlayerStriker003_Attacker.unitType = "human";

        // Add this to there
        AddElementInArray(ref returnValue.unitDatas, PlayerStriker003_Attacker);
        AddElementInArray(ref playerTeam.squads[0].memberID, PlayerStriker003_Attacker.ID);

        // UnitBase, OrganData, ItemData 추가하기
        AddComponentData(new UnitBase.UnitBaseData(PlayerStriker003_Attacker, "Player"), ref returnValue, ref PlayerStriker003_Attacker);
        AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker003_Attacker);
        AddComponentData(new UnitItemPack.UnitItemPackData("Blank", "Pistol", "Knife"), ref returnValue, ref PlayerStriker003_Attacker);

        HumanUnitInfoData PlayerStriker003_AttackerHuman = new HumanUnitInfoData();
        PlayerStriker003_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        PlayerStriker003_AttackerHuman.BaseUnitDataID = PlayerStriker003_Attacker.ID;
        PlayerStriker003_AttackerHuman.charactor = "kart";
        PlayerStriker003_AttackerHuman.TeamID = playerTeam.ID;
        AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker003_AttackerHuman);
        #endregion
        #region Squad Member 2 공격대 3
        // BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        // BaseUnitData 정보 채우기
        BaseUnitData PlayerStriker004_Attacker = new BaseUnitData();
        PlayerStriker004_Attacker.direction = new Vector3(0, 0, -1);
        PlayerStriker004_Attacker.ID = GetNewUnitID(returnValue);
        PlayerStriker004_Attacker.unitType = "human";

        // Add this to there
        AddElementInArray(ref returnValue.unitDatas, PlayerStriker004_Attacker);
        AddElementInArray(ref playerTeam.squads[0].memberID, PlayerStriker004_Attacker.ID);

        // UnitBase, OrganData, ItemData 추가하기
        AddComponentData(new UnitBase.UnitBaseData(PlayerStriker004_Attacker, "Player"), ref returnValue, ref PlayerStriker004_Attacker);
        AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker004_Attacker);
        AddComponentData(new UnitItemPack.UnitItemPackData("Blank", "Pistol", "Knife"), ref returnValue, ref PlayerStriker004_Attacker);

        HumanUnitInfoData PlayerStriker004_AttackerHuman = new HumanUnitInfoData();
        PlayerStriker004_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        PlayerStriker004_AttackerHuman.BaseUnitDataID = PlayerStriker004_Attacker.ID;
        PlayerStriker004_AttackerHuman.charactor = "kart";
        PlayerStriker004_AttackerHuman.TeamID = playerTeam.ID;
        AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker004_AttackerHuman);
        #endregion

        #endregion
        #region Player Squad 2

        #endregion
        returnValue.teamsAndSquads[0] = playerTeam;

        #region Enemy Squad 1

        #endregion

        TeamData enemyTeam = new TeamData();
        enemyTeam.name = "enemyTeam";
        enemyTeam.squads = new SquadData[1] { new SquadData() };
        enemyTeam.squads[0].name = "EnemyStriker001";

        #endregion


        returnValue.humanUnitDatas = new HumanUnitInfoData[3];


        returnValue.GameCurrentLocation = "Demo";

        PlayerSetup("Player");

        return returnValue;
    }

    //FieldData FieldLoad(string path)
    //{
    // 파일을 대상


    //}


    //Note
    // 프리펩 결정하는 스위치문이 전부 휴먼 유닛을 인스턴스화하도록 되어 있습니다.
    // 침입 유닛일때, 자신의 이전 위치와, 비콘의 위치를 참조해야 하지만, 아직 구현이 되어 있지 않습니다.
    void UnitInstantiate(ref FieldData data)
    {
        // 자신의 지역에 존재하는 녀석들만 필드에 등록하도록 합시다.
        // 0. 유닛의 정보를 추가하지 않도록 합니다.
        // 1. 일단 유닛이 있으면 인스턴스화하도록 준비합니다.
        // 1.1. 유닛의 프리펩을 결정합니다. 유닛 타입을 먼저 확인하고, 세부 사항을 확인합니다.
        // 1.2. 유닛의 위치를 결정합니다.
        // 1.3. 인스턴스 함수를 호출합니다.
        // 1.4. 컴포넌트 데이터를 집어넣습니다.
        // 2. 이제 데이터를 집어넣을 수 있도록 currentFielddata에 잠금을 끕니다
        for (int unitIndex = 0; unitIndex < data.unitDatas.Length; unitIndex++) // 필드 데이터에 있는 모든 유닛Array의 원소들을 인스턴스화하도록 시도합니다.
        {
            int humanUnitDataIndex = 0;
            #region 1.1. 인스턴스화 - 프리펩 결정
            // 유닛 타입 체크
            // 타입에 따라 해당하는 유닛 타입 배열에 자신과 아이디가 같은 녀석을 찾음
            // 세부 프리펩을 찾는다.
            GameObject prefab; // 프리펩입니다.
            switch (data.unitDatas[unitIndex].unitType) // 유닛의 타입을 찾습니다. 머신 / 인간 / +모더들이 추가한 유닛의 타입
            {
                case "human": // 인간 타입일때.
                    // 인간 프리펩을 찾습니다

                    int humanIndex = 0;
                    // 휴먼 데이터가 몇번째 인덱스가 존재하는지 찾는 루프문입니다. (나중에 인덱스 어레이를 집어넣도록 하자.)
                    for (; humanIndex < data.humanUnitDatas.Length; humanIndex++)
                    {
                        if (data.humanUnitDatas[humanIndex] == null) continue;

                        if (data.humanUnitDatas[humanIndex].BaseUnitDataID == data.unitDatas[unitIndex].ID)
                        {
                            humanUnitDataIndex = humanIndex;
                            break; // 휴먼 유닛 인덱스를 찾았습니다.
                        }
                    }
                    // 유닛 데이터의 캐릭터 이름으로 프리펩을 찾습니다.
                    switch (data.humanUnitDatas[humanUnitDataIndex].charactor)
                    {
                        case "kart":
                            prefab = HumanPrefabKartSkin;
                            break;
                        default:
                            prefab = HumanPrefabDefaultSkin;
                            break;
                    }
                    break;
                case "machine": // 머신 타입일때.
                    // 유닛의 프리펩은 머신 종류에 따라 나뉩니다. ex)103
                    prefab = HumanPrefabDefaultSkin;
                    break;
                default:
                    Debug.Log("DEBUG_GameManager.UnitInstantiate: 이 유닛의 타입(" + data.unitDatas[unitIndex].unitType + ")을 통해 프리펩을 결정할 수 없습니다.");
                    prefab = HumanPrefabDefaultSkin;
                    break;
            }






            #endregion
            #region 1.2. 인스턴스화 - 위치 결정
            Vector3 position; // 유닛이 존재할 위치입니다.
            // 1. 데이터에 주둔군인지 여부를 결정합니다.
            // 1.a. 주둔군인경우 저장된 위치를 값으로 합니다.
            // 1.b. 주둔군이 아닌경우 스쿼드 / 혹은 유닛의 과거 위치를 찾아 과거 지역의 이름을 가지고있는 첫번째 비콘이 존재하는 위치에 넣습니다. // 이미 유닛이 배치되어있으면 그 비콘의 지역 이름이 같은 다음 비콘의 위치에 넣습니다.
            // 1.a.1. 
            // 
            if (data.unitDatas[unitIndex].isUnitStayedThatPlace) // 유닛이 주둔군인지 여부를 판별합니다.
            {
                position = data.unitDatas[unitIndex].position;
            }
            else
            {
                // 
                position = new Vector3(0, 1, 0);
            }

            #endregion
            // 1.3. 인스턴스 함수 호출
            GameObject instantiatedObject = Instantiate(prefab, position, Quaternion.identity);
        }



        #region 휴먼유닛 인스턴스화 // 참고용으로 주석화


        /*
        
        // HumanUnit Instantiate
        for (int unitIndex = 0; unitIndex < data.humanUnitDatas.Length; unitIndex++)
        {
            // 인스턴스한 대상에다가 












            #region 인스턴스 작업.1. 유닛의 프리펩 결정
            GameObject prefab;
            switch (data.humanUnitDatas[unitIndex].charactor) // 유닛의 외형
            {
                case "kart":
                    prefab = HumanPrefabKartSkin;
                    break;
                default:
                    prefab = HumanPrefabDefaultSkin;
                    break;
            }
            #endregion

            #region (work in process) 인스턴스 작업.2. 소환 위치 결정.
            Vector3 SpawnPosition;
            if (true) // 이 유닛이 주둔군이면
            {
                SpawnPosition = new Vector3(0, 1, 0);
                //SpawnPosition = data.units[unitIndex].HoldingPosition;
            }
            else
            {
                // DEMO - 0,1,0에서 소환
                SpawnPosition = new Vector3(0, 1, 0);

                // 원래버전 - SquadDictionary[units[unitIndex].SquadName].position의 값에 따라 결정됩니다.
                // Squadname.position에서 oldPosition도 들어 있음
            }
            #endregion


            // 인스턴스 작업.3. 인스턴스화
            GameObject instancedUnit = Instantiate(prefab, SpawnPosition, Quaternion.identity);

            // 인스턴스 게임오브젝트의 컴포넌트에서 필드 넣기
            if (data.units[unitIndex].unitBaseData != null)
            {
                instancedUnit.GetComponent<UnitBase>().unitBaseData = data.units[unitIndex].unitBaseData;
            }
            else
            {
                Debug.Log("<!> ERROR_GameManager.UnitInstantiate : this Unit unitBaseData is null.");
                Debug.Log("-!- FIXING_GameManager.UnitInstantiate : instancedUnit.GetComponent<UnitBase>().unitBaseData = new UnitBase.UnitBaseData();");
                instancedUnit.GetComponent<UnitBase>().unitBaseData = new UnitBase.UnitBaseData();
            }
            if (data.units[unitIndex].organListData != null)
            {
                instancedUnit.GetComponent<HumanUnitBase>().organSystemsSet(data.units[unitIndex].organListData);
            }
            else
            {
                Debug.Log("<!> ERROR_GameManager.UnitInstantiate : this Unit organListData is null.");
                Debug.Log("-!- FIXING_GameManager.UnitInstantiate : instancedUnit.GetComponent<HumanUnitBase>().organSystemsSet();");
                instancedUnit.GetComponent<HumanUnitBase>().organSystemsSet();
            }
            if (data.units[unitIndex].unitItemPackData != null)
            {
                instancedUnit.GetComponent<UnitItemPack>().InventorySet(data.units[unitIndex].unitItemPackData);
            }
            else
            {
                Debug.Log("<!> ERROR_GameManager.UnitInstantiate : this Unit unitItemPackData is null.");
                Debug.Log("-!- FIXING_GameManager.UnitInstantiate : instancedUnit.GetComponent<UnitItemPack>().InventorySet();");
                instancedUnit.GetComponent<UnitItemPack>().InventorySet();

            }

            // 인스턴스했으니 그 오브젝트의 아이디 붙이기
            instancedUnit.GetComponent<UnitBase>().unitBaseData.id = instancedUnit.GetInstanceID();
        }

        */
        #endregion
    }
    void PlayerSetup()
    {
        playerTeam = "Player";
    }
    void PlayerSetup(string Value)
    {
        playerTeam = Value;
    }
    #endregion
    #region Field Ending Function
    public void ZippingWorld()
    {



    }
    #endregion
    #region 데이터 로딩을 통한 유닛 인스턴스화에 도움을 주는 함수
    public void SetGameObjectComponentDataByFieldData(ref GameObject instantiatedGameObject, FieldData fieldData, string componentType, int index)
    {
        // 함수 설명: 컴포넌트 타입에 따라 FieldData에 
        switch (componentType)
        {
            case "UnitBase":
                UnitBase unitBase = instantiatedGameObject.GetComponent<UnitBase>();
                if (unitBase != null)
                {
                    if (fieldData.unitBaseComponentData.Length > index && index > -1)
                    {
                        unitBase.unitBaseData = fieldData.unitBaseComponentData[index];
                    }
                }
                else
                {
                    Debug.Log("DEBUG_GameManager.SetGameObjectComponentDataByFieldData: 이 게임오브젝트는 " + componentType + "이란 컴포넌트를 가지고 있지 않습니다.");
                }
                break;
            case "HumanUnitBase":
                HumanUnitBase humanUnitBase = instantiatedGameObject.GetComponent<HumanUnitBase>();
                if (humanUnitBase != null)
                {
                    if (fieldData.organListComponentData.Length > index && index > -1)
                    {
                        humanUnitBase.organSystemsSet(fieldData.organListComponentData[index]);
                    }
                }
                else
                {
                    Debug.Log("DEBUG_GameManager.SetGameObjectComponentDataByFieldData: 이 게임오브젝트는 " + componentType + "이란 컴포넌트를 가지고 있지 않습니다.");
                }
                break;
            case "UnitItemPack":
                UnitItemPack unitItemPack = instantiatedGameObject.GetComponent<UnitItemPack>();
                if (unitItemPack != null)
                {
                    if (fieldData.organListComponentData.Length > index && index > -1)
                    {
                        unitItemPack.InventorySet(fieldData.unitItemPackComponentData[index]);
                    }
                }
                else
                {
                    Debug.Log("DEBUG_GameManager.SetGameObjectComponentDataByFieldData: 이 게임오브젝트는 " + componentType + "이란 컴포넌트를 가지고 있지 않습니다.");
                }
                break;
            default:
                Debug.Log("DEBUG_GameManager.SetGameObjectComponentDataByFieldData: 이 함수는 " + componentType + "이라는 컴포넌트에 데이터를 넣도록 설정되지 않았습니다.");
                break;
        }



    }
    #endregion
    #region 없던 유닛을 생성시키는 함수
    // 중간에 유닛이 생성하기를원하는 경우 이 함수를 호출해둬야 합니다.
    #region RegisterUnitHelper
    public void RegisterUnitHelper_NewUnitData(ref BaseUnitData baseUnitData, Vector3 position/*, params object[] componentDatas */)
    {
        // 아이디가 0이 아니라도 일단 아무거나면 됐죠 으이?
        // 여기 람다쓰지말고 for루프 사용하자
        int temp = baseUnitData.ID;
        //baseUnitData.ID = GetNewUnitID(currentFieldData);
        if (Array.FindIndex(currentFieldData.unitDatas, thing => thing.ID == temp) == -1 || baseUnitData.ID == 0)
        {
            baseUnitData.ID = GetNewUnitID(currentFieldData); // 여기선 호출되지 않았더라
        }
        // 게임오브젝트 이름은 패스

        // 위치
        if (currentFieldData.currentPlayLocation != null)
        {
            if (currentFieldData.currentPlayLocation.Length > 0)
            {
                baseUnitData.nameOfLocation = currentFieldData.currentPlayLocation[0];
            }
        }
        baseUnitData.isUnitStayedThatPlace = true;
        baseUnitData.position = position;
        baseUnitData.direction = new Vector3(0, 0, -1);

        // 컴포넌트 정보
        //baseUnitData.usingComponentNames = ComponentDataTypeToString(componentDatas);
        //AddComponentData(componentDatas, ref currentFieldData, ref baseUnitData);

        //AddElementInArray(ref currentFieldData.unitDatas, baseUnitData);
    }
    #endregion
    public void RegisterUnit(ref BaseUnitData baseUnitData, HumanUnitInfoData humanUnitInfoData, Vector3 position, params object[] componentsDatas)
    {
        // 휴먼 유닛을 필드 데이터에 넣어줍니다
        // 베이스 유닛 데이터를 집어넣습니다.
        
    }
    public void RegisterUnit(ref BaseUnitData baseUnitData, MachineUnitInfoData machineUnitInfoData, Vector3 position, params object[] componentsDatas)
    {
        // 머신 유닛을 필드 데이터에 넣어줍니다.
        // EX) 유닛을 설치할때 사용합니다.
        // 필드 데이터에 유닛의 정보를 추가합니다.
        // 만약 매개변수로 들어온 값이 완전하지 않으면, 알아서 디펄트값을 채워줍니다.

        RegisterUnitHelper_NewUnitData(ref baseUnitData, position);
        baseUnitData.unitType = "machine";

        AddElementInArray(ref currentFieldData.unitDatas, baseUnitData);

        Debug.Log("DEBUG_GameManager.RegisterUnit: 새로운 유닛 아이디가 배치됨 " + baseUnitData.ID);
    }




    #endregion
    #region 유닛 인스턴스화
    //





    #endregion
    #region 외부 컴포넌트 관련 함수
    public string ComponentDataTypeToString(object componentDatas)
    {
        System.Type dataType = componentDatas.GetType();
        if (dataType == typeof(HumanUnitBase.OrganListData))
        {
            return "HumanUnitBase";
        }
        else
        {
            return "Null";
        }
    }
    public string[] ComponentDataTypeToString(params object[] componentDatas)
    {
        // 매개변수로 받은 컴포넌트 데이터들을 컴포넌트 타입 이름으로 리턴합니다.
        string[] returnValue = new string[0];
        for(int index = 0; index < componentDatas.Length; index++)
        {
            System.Type dataType = componentDatas[index].GetType();
            AddElementInArray<string>(ref returnValue, ComponentDataTypeToString(componentDatas[index]));
        }

        return returnValue;
    }


    #endregion


    #region Class Helper
    #region FieldData Helper



    #endregion


    #endregion

    #region Function - Unit
    public int GetNewUnitID(FieldData fieldData) 
    {
        // field값 중에서 가장 높은 아이디를 가져옵니다.
        // 가장 높은 아이디 값의 +1한 값을 리턴합니다.

        int returnValue = -1;
        if(fieldData.unitDatas == null)
        {
            Debug.Log("DEBUG_GameManager.GetNewUnitID: 새로운 유닛 아이디! 0");
            return 0;
        }
        for(int index = 0; index < fieldData.unitDatas.Length; index++)
        {
            if (fieldData.unitDatas[index].ID > returnValue) returnValue = fieldData.unitDatas[index].ID;
        }
        Debug.Log("DEBUG_GameManager.GetNewUnitID: 새로운 유닛 아이디! " + (returnValue + 1));
        return returnValue + 1;


        // 만약 returnValue가 int의 최대값이면 두번째로 낮은 값을 불러올 것
    }
    public int GetNewSquadID(FieldData fieldData)
    {
        int returnValue = -1;
        if (fieldData.teamsAndSquads == null) return 0;
        for (int teamIndex = 0; teamIndex < fieldData.teamsAndSquads.Length; teamIndex++)
        {
            if (fieldData.teamsAndSquads[teamIndex] == null) continue;
            if (fieldData.teamsAndSquads[teamIndex].squads == null) continue;
            for(int squadIndex = 0; squadIndex < fieldData.teamsAndSquads[teamIndex].squads.Length; squadIndex++)
            {
                if (fieldData.teamsAndSquads[teamIndex].squads[squadIndex].SquadID > returnValue) returnValue = fieldData.teamsAndSquads[teamIndex].squads[squadIndex].SquadID;
            }
        }
        return returnValue + 1;
    }
    public int GetNewTeamID(FieldData fieldData)
    {
        int returnValue = -1;
        if(fieldData.teamsAndSquads == null)
        {
            return 0;
        }
        for (int index = 0; index < fieldData.teamsAndSquads.Length; index++)
        {
            if (fieldData.teamsAndSquads[index] == null) continue;

            if (fieldData.teamsAndSquads[index].ID > returnValue) returnValue = fieldData.teamsAndSquads[index].ID;
        }
        return returnValue + 1;
    }
    #endregion

    #region File I/O
    // save


    //void SaveData(UnitInfo unitInfo)
    //{
    //    string JsonData = JsonUtility.ToJson(unitInfo);
    //
    //    //string Path = string.Format("Assets/Data")
    //
    //}
    //void LoadData


    #endregion

    #endregion // Function End


    #endregion // Field Manager End
    #region Research Manager
    // 연구 정보를 저장합니다.
    // 연구 목록을 만듭니다.

    
    
    #region Class - Research Manager
    
    
    public class RessearchManager
    {

    }


    #endregion

    #endregion
    #region Goal Manager
    // 설명: 필드에 일어나는 전투와 전투 목적에 대해 관리하는 녀석입니다.
    // 전투 목표에 대해 체크합니다. (능동적으로 다른 변수를 감시하는 것이 아니라 함수로 호출되어 작동합니다.)
    // 전투 목표 달성 여부에 대해 체크합니다. (이 함수도 호출되어 작동됩니다.)

    #region TestCode
    // 


    #endregion
    #region Field

    #endregion
    #region Class
    public class GoalManager
    {
        #region 이건 어디에 쓰이는 클래슨고?
        // 싱글톤
        // 1. 이 작전 필드에서 플레이어 입장에서 내려진 작전 목표에 대해서 저장하고
        // 2. 플레이어가 작전에 성공했는지 Update 함수에서 자기 맴버 변수를 호출하여 항시 확인합니다.
        // 3. 만약 작전에 대해서 성공했다면 이벤트를 호출합니다. (니들이 필요하면 알아서 쓰세요.)
        #endregion

        #region Field

        #endregion
        #region Class

        #endregion
        #region Function
        public void CheckGoalStatus()
        {
            // 팀이 목표를 달성했는지 여부를 판단합니다.
        }




        #endregion

    }


    public class MissionGoal
    {
        // 플레이어가 가지고 있는 목표에 대해서 다룹니다.
        // 1. 지역형
        // 
        // 
        // 2. 캐릭터형
        // 



    }




    #endregion
    #region Function

    #endregion

    #endregion



    #region DataSaving / Loading
    // 아마도 이 함수는 전투가 종료할때나, 예제 컴포넌트에서 강제로 데이터를 넣을때 사용합니다.
    static public void DataSaving<DataType>(string path, ref DataType data)
    {
        string JsonData = JsonUtility.ToJson(data);

        System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
        fileInfo.Directory.Create();
        System.IO.File.WriteAllText(path, JsonData);
    }
    static public void DataSaving<DataType>(DataType data, string format, string fileName, string worldName, string Folder) // 이름은 속편하게 매게변수로 처넣어버리면 그만ㅋㅋ
    {
        string JsonData = JsonUtility.ToJson(data);

        string FilePath = string.Format("Assets/GameFile/{3}/{0}/{1}/{2}.json", worldName, format, fileName, Folder);
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(FilePath);
        fileInfo.Directory.Create();
        System.IO.File.WriteAllText(fileInfo.FullName, JsonData);
    }
    static public void DataSaving<DataType>(DataType data, string format, string name, string worldName) // 이름은 속편하게 매게변수로 처넣어버리면 그만ㅋㅋ
    {
        DataSaving(data, format, name, worldName, "World");
    }
    static public void DataSaving<DataType>(DataType data, string path)
    {
        string JsonData = JsonUtility.ToJson(data);

        string FilePath = string.Format(path);
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(FilePath);
        fileInfo.Directory.Create();
        System.IO.File.WriteAllText(fileInfo.FullName, JsonData);
    }
    static public void CampainSaving<DataType>(DataType data, string format, string name)
    {
        DataSaving(data, string.Format("Assets/GameFile/Campain/{0}/{1}.json", format, name));
    }





    static public bool DataLoading<DataType>(string path, ref DataType receivingInstance)
    {
        // 파일 데이터를 읽기만 하는 프로그램입니다.
        // 클래스에 이 데이터를 저장하려면 다른 함수에서 이 함수를 호출하는게 좋겠습니다.
        System.IO.FileInfo loadFile = new System.IO.FileInfo(path);
        if(loadFile.Exists == false)
        {
            Debug.LogWarning("파일이 없습니다. path : " + path);
            return false;
        }
        string JsonData = System.IO.File.ReadAllText(loadFile.FullName);
        receivingInstance = JsonUtility.FromJson<DataType>(JsonData);
        return true;
    }
    
    #endregion


    #region Camera
    bool isCamera1stPerson = false;
    #endregion

    public void PingPong()
    {
        Debug.Log("PONG! : GameManager의 함수를 호출 할 수 있습니다.");
    }
}