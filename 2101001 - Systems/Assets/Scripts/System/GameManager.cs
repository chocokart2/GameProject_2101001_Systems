using System;
using System.Linq;
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

        // 게임 모드에 따라 달라지는 행동
        GameMode gameMode = GetGameMode();
        if (gameMode == GameMode.GamePlay)
        {
            ReadyPlayMode();
        }
        else if (gameMode == GameMode.EditAndTest)
        {
            ReadyTestMode();
        }
        else
        {
            Console.WriteLine("ERROR_GameManager.Start() : 알 수 없는 게임모드입니다.");
        }





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


    #region Start 함수 호출 루틴
    
    // 1. 자신이 어떤 모드인지 결정합니다.
    GameMode GetGameMode()
    {
        EditorManager em = GameObject.Find("EditorManager")?.GetComponent<EditorManager>();

        if (em == null)
            return GameMode.GamePlay;
        else
            return GameMode.EditAndTest;
    }

    // 2. 해당하는 모드에 대해서 설정합니다.
    void ReadyPlayMode()
    {





        // 1. 파일을 읽습니다.
        FieldData _fieldData = new FieldData();
        string _filePath = "Assets/GameFile/Field/fieldData.json";
        if (DataLoading(_filePath, ref _fieldData) == false)
        {
            // 파일이 없으면 파일이 없다고 에러 메시지를 보냅니다.
            throw new CannotReeachFileException($"파일 경로{_filePath} 위치에 파일을 찾을 수 없습니다.");
        }

        // 2. 작전 목표를 읽습니다.
        // 자신의 팀이 누구인지 파악합니다.
        // 각 팀의 작전 목표를 파악합니다.
        //_fieldData.teamDatas[0].goal





    }
    void ReadyTestMode()
    {
        // (패스) UI Controller -> UI Controller가 알아서 해 줘야 해

        // 이 파트는
        if(currentFieldData != null)
        {
            currentFieldData = new FieldData();
        }
        UnitRoleInitate();



        RealTileMap = new Dictionary<Vector3, int>();
        ReadyRealTileMap(ref RealTileMap);


        ActiveUnitSight();
    }



    #region 열거형
    enum GameMode
    {
        GamePlay,
        EditAndTest
    }
    enum SceneMode
    {
        Field,
        WorldMap
    }
    #endregion


    #endregion
    #region 길찾기 알고리즘 보조역할
    public event TileMap2Void ReadyRealTileMap = delegate (ref Dictionary<Vector3, int> myMap) { };



    Dictionary<Vector3, int> RealTileMap;
    void TileMapSetup() { RealTileMap = new Dictionary<Vector3, int>(); }

    public void SetTileMap(ref Dictionary<Vector3, int> _map , Vector3 vec3, int blockTileID)
    {
        #region 이 함수는 작전 필드의 에디터용 함수입니다.
        // 이 함수는 필요한 존재인가?
        // 게임 맵에서는 객체가 먼저 존재하고, 게임오브젝트는 존재하지 않을 것이다
        // 따라서 게임오브젝트의 블럭 정보를 저장하는 일은 없을 것이다. 왜냐하면 파일에서 따온 정보를 이용하면 되니까.
        // 이 함수는 게임 맵에서 필요하지 않다
        // 그러면 에디터 맵에 필요한 존재인가? 그렇다!
        #endregion
        // <!> 이벤트 발생시키기 -> TileBlock으로부터 데이터를 주입하도록 호출합니다.

        



        // NOTE:
        // a. Dictionary에서 .Add 함수를 이용할때 ArgumentException을 이용한 코드 : 사용해도 괜찮음!
        // b. Dictionary에서 .TryAdd 함수를 이용한 코드 : 유니티에서 TryAdd 함수를 못 알아먹는듯.
        if (_map.ContainsKey(vec3) == true) // 자기 위치를 저장할때 : 메모리맵에 자기 위치에 해당하는 정보가 이미 있습니다.
        {
            _map.Remove(vec3);
            _map.Add(vec3, blockTileID);
            //Debug.Log("타일맵이 저장되었습니다. : " + vec3);
        }
        else // 자기 위치를 저장할때 : 신규 블럭 등록
        {
            _map.Add(vec3, blockTileID);
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

#warning 아래 클래스의 내용을 science/attackclass로 옮기기.
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
            AttackClassData returnValue = new()
            {
                chemicals = chemical.toChemicalDataArray(chemicals.ToArray()),
                //returnValue.chemicals.CopyTo(chemical.toChemicalDataArray(chemicals.ToArray()), 0);

                mass = this.mass,
                volume = this.volume
            };
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
    public class ChemicalReaction // 만들었습니다.
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
    public List<chemical> Mix(List<chemical> returnValue, List<chemical> inputValue)
    {
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
    #region 이 클래스가 가지고 있는 공통적인 내용
    // 한 순간 이외에 사용되지 않음
    // - 씬을 로딩하거나 성과를 저장할 때만 사용.
    // 나머지는 컴포넌트에 저장되어 업데이트됨.
    // 스쿼드 및
    #endregion


    #region Base - Class-Field Manager
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
        #region 클래스 설명 및 자주 묻는 질문



        // 전투 장소에 대한 전반적인 정보를 담고 있습니다.
        // 게임 플레이 상황을 저장할 수 있고 JSON으로 만들어 직렬화 할 수 있어야 합니다.
        // 또 게임이 재개되면 이 인스턴스를 이용해 상황을 재구현할 수 있어야 합니다.

        #region 자주 묻는 질문
            #region 이 클래스의 일부 멤버들만 지역의 이름을 제한적으로 가지고 있는 이유
                // "굳이 필요하지 않아서"
                // FieldData는 멤버 변수로 이미 자신의 지역 이름을 가지고 있습니다.
                // 지역이 정해지고, 미션이 정해지면 구체적인 작전 장소가 정해지게 됩니다.
                // 이때 그 멤버들도 자신이 위치하고 있는 지역의 이름을 가지려고 한다면, 생성자를 생성할때, 데이터를 할당할 때, 제공해야 하는 정보는 많아지게 됩니다.
                // 예외적으로 SquadData는 분대 단위로 지역을 이곳저곳 움직이고, 바깥에서도 쓸만한 클래스이므로 지역의 이름을 가지고 있습니다.
            #endregion

            #region 유닛들에 대한 정보를 항상 동기화하지 않는 이유
                // "이미 유닛을 찾아서 저장하면 되기 때문"


            #endregion

            #region 이 씬의 전투 장소에 대해서는 어디서 저장해야 하는가? : FieldData
                // 필드데이터는 실제 작전을 위한 필드데이터입니다.
            #endregion
            #region 언제 / 어디서 개별 필드 데이터가 게임용 필드 데이터로 합쳐지는가?
                // 월드 지도에서는 개별 필드 데이터로 지정이 된다
                // 게임을 로딩하는 중에서는 사용자가 기다려 줄 수 있으므로 여기서 일을 몰빵해보자
                // 그 외에서는 즉각즉각 처리해야 해서 좀 약해보인다
            #endregion
        #endregion
        #endregion
        #region 생성자
        public FieldData()
        {
            currentPlayLocation = new string[2];
            playerTeamName = "Player";
            teamDatas = new TeamDataArray(1);
            teamDatas.Add(new TeamData[1]
            {
                new TeamData()
                {
                    name = "Neutral"
                }
            });
            machineUnitDatas = new MachineUnitData[0];
            blocksData = new BlocksData();
        }
        #endregion
        #region 프로퍼티

        #endregion
        #region 필드
        bool fieldLock = true; // 이 fieldLock이 설정되면 데이터가 들어가지 않습니다.

        #region currentPlayLocation 추가적인 설명들
        // 현재 플레이 하고 있는 공간의 이름입니다.
        // 해당 지역의 타일 배치가 실행되고, 거기에 존재하는 유닛도 배치됩니다.
        // <!> 스쿼드도 지역 이름따라 배치될 수있지만, 같은 장소에 다른 미션들을 받은경우가 있으면 어떻게 할지 로직이 필요합니다.
        // 만약 동시에 두개 이상의 공간을 보여주려면 여러 지역 이름을 나열합니다.
        // 현재 플레이어의 목표는 스쿼드의 미션을 확인하세요. 현재 위치에 존재하는 스쿼드의 미션이 곧 현재의 목표입니다.
        //
        #endregion
        public string[] currentPlayLocation; // 불러온 지역의 이름들입니다. 아마 키 값으로 쓸만할텐데

        #region 유닛에 대한 필드
        // 팀에 대한 정보들
        [Tooltip("플레이어가 통제할 팀입니다. \n" +
            "만약 멀티플레이라면 이 값이 다르개 설정될 수 있습니다.")]
        public string playerTeamName; // 플레이어가 통제하는 팀의 이름입니다.
        public int playerTeamID
        {
            get
            {
                int returnValue = -1;

                if(teamDatas != null)
                {
                    for (int teamIndex = 0; teamIndex < teamDatas.Length; teamIndex++)
                    {
                        if (playerTeamName == teamDatas[teamIndex].name)
                            return teamDatas[teamIndex].ID;
                    }
                }


                return returnValue;
            }
        }
        public TeamDataArray teamDatas; // 분대에 대한 정보, 유닛이 존재하는 지역을 담습니다

        // 머신 유닛에 대한 필드들
        public MachineUnitData[] machineUnitDatas; // 이런 클래스 만들기
        #endregion        
        // 블럭에 대한 정보
        public BlocksData blocksData;

        // 거점에 대한 정보

        #endregion
        #region Method
        #region public Method
        public void AddUnitData(BaseUnitData baseUnitData, HumanUnitInfoData humanUnitInfoData, params object[] componentDatas)
        {
            
        }
        public void AddUnitData(MachineUnitData machineUnitData, params object[] componentDatas) // 에러가 있습니다!
        {
            AddElementInArrayStatic(ref machineUnitDatas, machineUnitData);
        }
        public int GetPlayerTeamID()
        {
            #region 함수 설명
            // FieldData에서 playerTeamName와 동일한 팀 이름을 가진 존재를 찾는다.
            // 만약 찾으면 팀의 아이디 값을 리턴한다.

            #endregion

            return -1;
        }

        #endregion



        #endregion
    }


    #endregion


    #region 1. TeamData : Squads(Unit) / Goal / BlockMemory
    #region 1.0. Team Informations
    [System.Serializable]
    public class TeamDataArray : IEnumerable, IEnumerator
    {
        #region     < 필드 / 프로퍼티
        #region     << public 필드 / 프로퍼티
        #region     <<< public 필드


        #endregion  <<< public 필드 >
        #region     <<< public 프로퍼티
        public int Length
        {
            get
            {
                return self.Length;
            }
        }
        public TeamData this[int index]
        {
            get
            {
                return self[index];
            }
            set
            {
                value = self[index];
                UpdateIndexUnitReacher();
            }
        }
        //public TeamDataUnitArray Unit
        //{
        //    get; set;
        //}
        #endregion  <<< public 프로퍼티 >
        #endregion  << public 필드 / 프로퍼티 >
        #region     << private 필드 / 프로퍼티
        private int position = -1;
        [Tooltip(
            "이 클래스의 핵심 내용이 되는 필드입니다. (self라고 지은 이유도 여기에 있고요.) \n" +
            "이 필드의 값에 접근하기 위해 인덱서가 준비되어 있으니 이를 이용하시길 바랍니다."
            )]
        private TeamData[] self;
        /// <summary>
        /// 팀 배열 [스쿼드 배열 [유닛 배열]]같이 3차원적인 배열을
        /// 1차원적인 배열로 접근하기 위해 존재합니다.
        /// </summary>
        [Tooltip(
            "GetUnit(int)을 지원하기 위해 존재하는 딕셔너리입니다. \n" +
            "팀 배열,스쿼드 배열, 유닛 배열로 된 3차원 배열을 \n" +
            "유닛 배열만 존재하는 1차원적인 배열로 접근하는 것을 도와주기 위해 존재합니다. \n" +
            "이것은 값에 get하기 위해서만 존재하며, set 등으로 수정할 일이 있으면 여기 값도 바뀌어야 합니다. \n" +
            "이때, 관련 함수를 호출해 주세요."
            )]
        private List<UnitIndex> indexUnitReacher;

        #endregion < private 필드 / 프로퍼티 >
        #endregion 필드 / 프로퍼티
        #region < 구조체
        // 이 값의 멤버 변수를 통해, self의 유닛의 인덱스를 구할 수 있습니다.
        private struct UnitIndex
        {
            public int teamIndex;
            public int squadIndex;
            public int unitIndex;
            public UnitIndex(int _teamIndex, int _squadIndex, int _unitIndex)
            {
                this.teamIndex = _teamIndex;
                this.squadIndex = _squadIndex;
                this.unitIndex = _unitIndex;
            }
        } // 팀, 스쿼드. 유닛의 인덱스를 각각 가지고 있어 유닛을 특정하기 쉽도록 합니다.
          //public class TeamDataUnitArray : IEnumerable, IEnumerator
          //{
          //    // 클래스 설명 : TeamDataArray의 모든 유닛의 목록을 1차원적 배열처럼 접근할 수 있도록 합니다.
          //    // 단점 : 이 클래스의 외부의 필드에 접근할 수 없습니다. 중첩 클래스가 원래 그런 녀석이니깐요.
          //    // 어쩔 수 없습니다. 생성자로 바깥을 참조해야 하지 않을까요
          //    // 의문점. 여기 있는 클래스의 배열이 바깥의 배열을 = 연산을 사용하면 참조가 될까?
          //    // public
          //    public UnitInSquadData this[int index]
          //    {
          //        get
          //        {
          //            return self[index];
          //        }
          //    }            
          //    public TeamDataUnitArray(TeamData[] target)
          //    {
          //    }
          //    // private
          //    private UnitInSquadData[] self;
          //}
        #endregion < 구조체 >
        #region < 생성자
        public TeamDataArray() { }
        public TeamDataArray(int size) : base()
        {
            self = new TeamData[size];
        }
        #endregion < 생성자 >
        #region     < 메서드
        #region     << public 메서드
        #region     <<< 인터페이스 메서드
        public void Reset()
        {
            position = -1;
        }
        public bool MoveNext()
        {
            if(position < self.Length - 1)
            {
                position++;
                return true;
            }
            else
            {
                return false;
            }
        }
        public System.Object Current
        {
            get
            {
                return self[position];
            }
        }
        public IEnumerator GetEnumerator()
        {
            for(int index = 0; index < self.Length; index++)
            {
                yield return self[index];
            }
        }
        #endregion
        public void Add(params TeamData[] _newItems)
        {
            foreach(TeamData item in _newItems)
            {
                AddElementInArrayStatic(ref self, item);
            }
        }
        public UnitInSquadData GetUnit(int index)
        {
            // get unit -> indexUnitReacher + unitIndex -> UpdateIndexUnitReacher()

            return self[indexUnitReacher[index].teamIndex]
                .squads[indexUnitReacher[index].squadIndex]
                .units[indexUnitReacher[index].unitIndex];
        } // 배열처럼 컨트롤 하기 위해 존재
        public UnitInSquadData[] GetAllUnits()
        {
            UnitInSquadData[] result = new UnitInSquadData[0];
            foreach (UnitIndex _unitIndex in indexUnitReacher)
            {
                AddElementInArrayStatic(ref result, self[_unitIndex.teamIndex].
                    squads[_unitIndex.squadIndex].
                    units[_unitIndex.unitIndex]);
            }
            return result;
        }
        #endregion // public 메서드
        #region private 메서드
        private void UpdateIndexUnitReacher()
        {
            // 함수 설명 : 이 함수는 indexUnitReacher를 업데이트 하기 위해 존재합니다.
            // 
            // 이 함수가 호출되어야 하는 시기: self 값중 유닛이 새로 생겨나거나 없어질 때.
            indexUnitReacher = new List<UnitIndex>();
            if (self == null) return;
            for(int teamIndex = 0; teamIndex < self?.Length; teamIndex++)
            {
                for(int squadIndex = 0; squadIndex < self[teamIndex]?.squads?.Length; squadIndex++)
                {
                    for(int unitIndex = 0; unitIndex < self[teamIndex]?.squads[squadIndex]?.units?.Length; squadIndex++)
                    {
                        if (self[teamIndex]?.squads[teamIndex]?.units[unitIndex] != null)
                        {
                            indexUnitReacher.Add(new UnitIndex(teamIndex, squadIndex, unitIndex));
                        }
                    }
                }
            }
        }
        #endregion // private 메서드






        #endregion




        // 최하단 정보
        //배경
        // 팀 -> 스쿼드 -> 유닛으로 묶으려는 요구사항과
        // 모든 유닛들을 하나의 배열처럼 접근하려는 요구사항 두개 다를 맞추기 위해
        // 다양한 종류의 인덱서 서비스를 제공합니다.
        // 그 외에 이 유닛이 속해있는 팀에 대해서도 접근할때 유용합니다.
    }


    [System.Serializable]
    public class TeamData
    {
        #region 생성자 : void / int
        public TeamData() { }
        public TeamData(int id)
        {
            this.ID = id;
        }
        #endregion

        // 팀을 특정하기 위한 클래스입니다.
        public string name; // 유저에게 표시할 이름
        public int ID; // 이름이 같더라도 컴퓨터가 팀을 특정할 수 있도록 합니다.
        public UnitID unitID;


        public FieldData fieldData
        {
            get
            {
                return GameObject.Find("GameMananger").GetComponent<GameManager>().currentFieldData;
            }
        }

        // 팀 내부에 대한 정보입니다.
        public SquadData[] squads; // 팀 내부에 어떤 분대가 있는가
        #region 인덱서 - squad 참조
        public SquadData this[int index]
        {
            get { return squads[index]; }
            set { this[index] = squads[index]; }
        }
        public SquadData this[string name]
        {
            get
            {
                if (squads == null) return null;
                for(int index = 0; index < squads.Length; index++)
                {
                    if(squads[index].name == name) return squads[index];
                }
                return null;
            }
            set
            {
                if (squads == null)
                {
                    Debug.Log("<!>FunctionWillNotWorking_TeamData.this[string] : 접근하려는 squad가 Null입니다.");
                    return;
                }
                for (int index = 0; index < squads.Length; index++)
                {
                    if (squads[index].name == name)
                    {
                        squads[index] = value;
                    }
                }
            }
        }
        #endregion

        public GoalData goal; // 이 팀의 승리 조건
        public BlockMemoryData[] blockMemorys; // 이 팀이 기억하고 있는 지도
        public DiplomacyData[] relations; // 다른 팀과의 관계

        public string[] DataTag; // 추가적인 데이터입니다. 무슨 일이 생겨서 가라로 추가해야 하는 정보가 있을 때 사용합니다.

        // 자신을 포함하는 필드데이터를 참조하기 위한 포인터 프로퍼티입니다.

        #region static method
        public static int GetNewTeamID(FieldData fieldData)
        {
            #region 함수 설명
            // 필드데이터의 팀 목록을 확인하고, 새로 만들려는 팀이 몇 번째인가를 리턴합니다.
            #endregion
            int returnValue = -1;
            if (fieldData.teamDatas == null) // 필드데이터에 팀이 없으면 이 팀이 첫번째 팀입니다.
            {
                return 0;
            }
            for (int index = 0; index < fieldData.teamDatas.Length; index++)
            {
                if (fieldData.teamDatas[index] == null) continue;

                if (fieldData.teamDatas[index].ID > returnValue) returnValue = fieldData.teamDatas[index].ID;
            }
            return returnValue + 1;
        }

        #endregion
        #region public instance method

        #region factory method
        public TeamData CreateTeamData()
        {
            TeamData returnValue = new TeamData();
            returnValue.unitID = new UnitID()
            {
                //team = GetNewTeamID()
                squad = new int[0], // 만약 생성하게 된다면
                unit = new int[0]
            };
            return returnValue;
        }
        #endregion
        


        #endregion
    }
    #region MyRegion

    #endregion
    #region 레거시
    public class Team
    {
        public string name;
        public List<Squad> squads;
        public Dictionary<Vector3, int> memoryMap; // 휘발성 메모리 맵입니다.
    }
    #endregion 레거시



    #endregion
    #region 1.1. Squads
    #region 1.1.0. Squad Information


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

    [System.Serializable]
    public class SquadData
    {
        #region 생성자
        public SquadData()
        {

        }
        public SquadData(int TeamID)
        {
            TeamID = this.TeamID;
        }
        #endregion

        public int TeamID;

        // 스쿼드의 정보
        public UnitID UnitID;
        public int SquadID; // 0부터 시작하여 생성된 순서대로 배정받습니다
        public string name;

        public string nameOfLocation;
        public string nameOfPastLocation;
        //public int[] memberID;
        public string assingedMission;
        public bool isDummyData;



        // 분대원의 정보
        public UnitInSquadData[] units;


        #region static member method
        public static int GetNewSquadID(FieldData fieldData)
        {
            #region 함수 설명
            // 필드데이터를 뒤져 가장 큰 스쿼드 아이디값 + 1을 리턴합니다.
            #endregion
            int returnValue = -1;
            if (fieldData.teamDatas == null) return 0; // 팀 데이터가 없으니 스쿼드도 존재하지 않습니다. 따라서 첫번째입니다.
            for (int teamIndex = 0; teamIndex < fieldData.teamDatas.Length; teamIndex++) // 모든 팀을 뒤집니다.
            {
                if (fieldData.teamDatas[teamIndex] == null) continue;
                if (fieldData.teamDatas[teamIndex].squads == null) continue;
                for (int squadIndex = 0; squadIndex < fieldData.teamDatas[teamIndex].squads.Length; squadIndex++)
                {
                    // 선택한 대상이 여태껏 찾은 아이디값보다 더 크면 더 큰 값을 선택합니다.
                    if (fieldData.teamDatas[teamIndex].squads[squadIndex].SquadID > returnValue) returnValue = fieldData.teamDatas[teamIndex].squads[squadIndex].SquadID;
                }
            }
            return returnValue + 1;
        }

        #endregion
    }
    #endregion
    #region 1.1.1. Units
    /// <summary>
    /// 팀의 Squad 내부에 존재하는 유닛에 대한 정보입니다.
    /// ? : Machine 유닛은 통제 가능한 중립이므로, TeamData에 존재하지 않고, FieldData에 존재합니다.
    /// </summary>
    [System.Serializable]
    public class UnitInSquadData
    {


        #region Help
        // 스쿼드 내부에 있는 유닛에 대한 정보입니다.

        // 멤버
        // A. 컴포넌트
        // UnitBase 컴포넌트 속 데이터
        // HumanUnitBase 컴포넌트 속 데이터
        // UnitItemPack 컴포넌트 속 데이터
        // UnitRoleData 컴포넌트 속 데이터

        #endregion


        #region 생성자
        public UnitInSquadData()
        {

        }


        #endregion
        #region 팩토리 메서드
        public static UnitInSquadData MakeUnit(ref FieldData fieldData)
        {
            UnitInSquadData returnValue = new UnitInSquadData();

            return returnValue;
        }
        #endregion
        #region 필드



        #region 필드 : 컴포넌트 정보
        // 유닛의 종류에 따라서 Null인 값이 존재할 수도 있습니다.
        public UnitBase.UnitBaseData unitBaseData;
        public HumanUnitBase.HumanUnitBaseData organListData;
        public UnitItemPack.UnitItemPackData unitItemPackData;
        public UnitRole.UnitRoleData unitRoleData;
        #endregion
        // 이 유닛을 설명하는 데이터들
        public int memberID;
        public int squadID; // 그냥 가리키기만 하면 됩니다.
        public int teamID; // 그냥 가리키기만 하면 됩니다.
        #endregion
        #region 메서드

        #endregion
        #region Static 메서드
        public static int GetNewUnitID(FieldData fieldData)
        {
            #region 함수 설명
            // 모든 스쿼드 값 내부 유닛의 아이디값을 전부 뒤져 가장 큰 아이디값을 찾아 그 값의 +1한 값을 리턴합니다.
            #endregion
            int returnValue = -1;

            if (fieldData.teamDatas == null) return 0;
            for (int teamIndex = 0; teamIndex < fieldData.teamDatas.Length; teamIndex++)
            {
                if (fieldData.teamDatas[teamIndex] == null) continue;
                if (fieldData.teamDatas[teamIndex].squads == null) continue;
                for (int squadIndex = 0; squadIndex < fieldData.teamDatas[teamIndex].squads.Length; squadIndex++)
                {
                    if (fieldData.teamDatas[teamIndex].squads[squadIndex] == null) continue;
                    if (fieldData.teamDatas[teamIndex].squads[squadIndex].units == null) continue;
                    for (int unitIndex = 0; unitIndex < fieldData.teamDatas[teamIndex].squads[squadIndex].units.Length; unitIndex++)
                    {
                        if (fieldData.teamDatas[teamIndex].squads[squadIndex].units[unitIndex] == null) continue;
                        if (returnValue < fieldData.teamDatas[teamIndex].squads[squadIndex].units[unitIndex].memberID)
                            returnValue = fieldData.teamDatas[teamIndex].squads[squadIndex].units[unitIndex].memberID;
                    }
                }
            }

            return returnValue + 1;
        }

        #endregion



    }


    [System.Serializable]
    public class BaseUnitData // 구식 클래스
    {
        // 기본 정보
        public int ID; // WorldManager에서 저장되는아이디입니다. 유닛을 구분하는 유니크한 식별번호입니다. // WorldManager.GetNewUnitID를 호출해야 합니다. // 0부터 시작하여 1씩 늘어납니다.
        public string unitType; // Human인지, Machine인지 구분합니다.
        //-> 유닛 베이스에서 다룹니다.
        public string gameObjectName; // 게임오브젝트 이름입니다.
        //-> 유닛 베이스에서 다룹니다.
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





    #endregion




    #endregion
    #region 1.2. Goal
    #region 1.2.0. Goal Infomation
    [System.Serializable]
    public class GoalData
    {
        #region help
        // 이 팀이 가지고 있는 목표에 대해서 다룹니다.
        

        #endregion
        #region 생성자

        #endregion
        #region 필드
        // 작전 필드에 사용할 정보
        public string objective;

        //
        




        // 월드 지도에 사용할 정보
        public string missisonLocation;

        // 공통 정보

        #endregion
    }
    #endregion
    #region 1.2.1. Which things can be Goal
    [System.Serializable]
    public class GoalObjectiveGeneric<T>
    {

    }

    #endregion



    #endregion
    #region 1.3. BlockMemory
    [System.Serializable]
    public class BlockMemoryData
    {
        public Vector3 position;
        public int blockID; // 어떤 블럭인가?
        public BlockStatusData[] blockStatusDatas;
    }
    #endregion
    #region 1.4. Relations
    [System.Serializable]
    public class DiplomacyData
    {
        #region Help
        // 다른 상대와 관계 상태를 저장하기 위한 정보입니다.

        #endregion
        public int targetTeamID;
        public float status;
        public bool IsAllay { get { return status > 1; } }
        public bool IsEnemy { get { return status < -1; } }
        void foo()
        {
            MachineUnitData aaa = new();
            aaa.machineTypeID = 1;
        }
    }
    #endregion
    #region 1.x Univarsal class
    [System.Serializable]
    public class UnitID
    {
        // IdAddressPack이라고 바꾸는건 어떨까

        #region 클래스 설명
        #region 클래스의 용도
        // 자신이 속한 팀 / 스쿼드 및 자신의 아이디를 알리는 역할을 합니다.
        // 마치 자신의 명함이자 조직의 전화번호부(혹은 연관된 친구의 몋암을 저장하는 곳)같은 역할을 해요.
        // 또한 팀 / 스쿼드가 자신에 속한 스쿼드 / 유닛을 알리기 위한 용도도 있습니다.\
        #region 더 멍청한 나를 위한 설명
        // 팀인 경우
        // team : 자신의 팀 아이디를 넣음
        // squad : 자신에게 속한 스쿼드 값을 넣음
        // unit : 자신에게 속한 유닛의 아이디 값을 넣음
        // 스쿼드인 경우
        // team : 자신이 속한 팀 아이디를 넣음
        // squad : 자신의 스쿼드 아이디 값을 넣음
        // unit : 자신에게 속한 유닛의 아이디 값을 넣음
        // 유닛인 경우
        // team : 자신이 속한 팀 아이디 값을 넣음
        // squad : 자신이 속한 스쿼드 아이디 값을 넣음
        // unit : 자신의 유닛 아이디 값을 넣음

        #endregion
        #endregion

        #endregion

        public int team;
        public int[] squad;
        public int[] unit;
    }


    #endregion
    #endregion
    #region 2. BlockData
    [System.Serializable]
    public class BlocksData
    {
        #region 클래스 설명
        // 블럭들에 대한 데이터를 담고 있습니다.
        // 블럭들에 대한 정보를 저장하고 불러오는 메소드를 담기 위해 따로 클래스를 만들었습니다.
        #endregion
        #region 생성자
        public BlocksData()
        {
            blocks = new BlockData[0];
        }
        #endregion
        #region 필드
        public BlockData[] blocks;
        #endregion
        #region 메서드
        public void Add(BlockData blockData)
        {
            #region 메소드 설명

            #endregion
        }
        /// <summary>
        /// 매개변수로 받은 블럭을 blocks에 넣습니다. 이때, [해당 위치에 블럭이 없음 : 그 위치에 블럭을 추가합니다.] / [해당 위치에 블럭이 있음 : 그 위치의 블럭을 지우고 덮어씁니다.]
        /// </summary>
        /// <param name="NewBlock">새로 끼워넣을 블럭입니다.</param>
        public void Change(BlockData newBlock)
        {

        }
        public void Delete(Vector3 blockPos)
        {

        }
        /// <summary>
        /// 해당하는 좌표에 있는 블럭을 찾습니다.
        /// </summary>
        /// <param name="blockPos"></param>
        /// <returns> 블럭을 찾았으면 : 해당 좌표에 위치하고 있는 BlockData,
        /// 만약 못 찾았으면 : null
        /// </returns>
        public BlockData Get(Vector3 blockPos)
        {
            for(int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].position.x == blockPos.x &&
                    blocks[i].position.y == blockPos.y &&
                    blocks[i].position.z == blockPos.z)
                {
                    return blocks[i];
                }
            }
            return null;
        }
        public void Sort()
        {

        }
        #endregion
    }


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

        #region 필드

        public Vector3 position; // 거의 키 값 취급을 하는 변수입니다.
        public int blockID; // 어떤 블럭인가?
        public BlockStatusData[] blockStatusDatas;
        #endregion
        #region 메서드
        // BlockData[]는 dictionary역할을 해 줄수 있는 녀석이여야 합니다.
        // 키 값을 매개변수로

        public static void Sort(ref BlockData[] target)
        {
            
        }


        #endregion
        
    }
    [System.Serializable]
    public class BlockStatusData
    {
        string name; // 수치의 이름
        float value; // 얼만큼인지
    }
    #region 레거시



    //[System.Serializable]
    //[Obsolete("이 데이터는 구식입니다. BlockData 형식을 이용하세요.")]
    //public class BaseTileData
    //{
    //    public int TileID;
    //    public string PrefabName;

    //    public Vector3 position;
    //}
    #endregion

    #endregion
    #region 3. TerritoryData
    [System.Serializable]
    public class TerritoryData
    {
        // 생성자
        public TerritoryData()
        {
            owner = "None";
            radius = 5.0f;
        }

        public string owner;
        public float radius;
        public Vector3[] edgePosition;
    }
    #endregion
    #region 4. MachineData
    [System.Serializable]
    public class MachineUnitData
    {
        #region 클래스 설명
        // 이 필드에 존재하는 머신들에 대해서 다룹니다.
        // 
        //
        #endregion
        #region 필드
        public UnitBase.UnitBaseData unitBaseData;
        public string prefabName;

        /// <summary>
        /// 이 머신이 어떤 종류의 머신인가요?
        /// </summary>
        public int machineTypeID;
        public int machineID; // 이 필드에 존재하는 몇 번째 머신인지 판단
        #region machineID에 대한 정보 및 질문
        // 필드를 재 로딩했을때, 이 머신이 어디에 위치해 있는지를 알려줍니다.
        // 이 머신과 다른 머신에 상호관계가 놓여 있을때, 필드를 재 로딩했을때, 그 관계가 지워지지 않게 만들기 위함입니다.
        // 아이디 값은 인덱스 값이 아니라, 아이디 값을 발급할 당시 게임 매니저에서 여태껏 머신 아이디를 제공한 값 중에서 가장 큰 값 + 1을 받습니다.
        #endregion

        #region 머신 유닛이 "가질 수 있는" 컴포넌트 필드의 데이터들

        #endregion

        #endregion







    }


    #endregion
    #region 5. Location
    [System.Serializable]
    public class SceneLocate
    {
        #region 클래스 설명


        #endregion

        public string[] location;
        
    }




	#endregion

    #region Interface
    public interface IComponentDataIOAble<T>
    {
        void SetData(T inputData);
        T GetData();
    }




    #endregion

    #region Unit - Class-Field Manager

    #region 공통: 유닛

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
    public class UnitInfo // 버려질 클래스입니다. // 좀 역겨운데
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

    //[System.Serializable]
    //public class TeamData
    //{
    //    public string name;
    //    public int ID;
    //
    //    public SquadData[] squads;
    //    public GoalData goal;
    //    //public BlockData
    //}
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



    #region TerritoryData - Class-FieldManager

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

    // 에디터 전용
    [Tooltip(
        "이 변수는 에디터 모드에서 유닛들을 호출하여 배열에 담아주는 역할을 합니다. \n" +
        "로봇 유닛도 포함되어 있습니다.")]
    public GameObject[] unitInEditor;
    [Tooltip(
    "이 변수는 에디터 모드에서 유닛들을 호출하여 배열에 담아주는 역할을 합니다. \n" +
    "인간 유닛만 목록에 들어 있습니다.")]
    public GameObject[] humanUnitInEditor;





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
    [SerializeField]
    public GameObject WarningIcon;

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
    public static int AddElementInArrayStatic<T>(ref T[] array, T element)
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
    #region 퍼블릭 메서드




    #endregion
    #region 프라이빗  메서드

    #region 초기값 설정

    #endregion
    #region UnitRole 초기화 inEditorMode
    /// <summary>
    /// 에디터용 씬에서 하이어라키에 존재하는 유닛 게임오브젝트들을 UnitRole(주로 이름,스쿼드,팀 이름에 기반)과 FieldData의 유닛 목록에 넣습니다.
    /// 같은 팀/스쿼드 이름이 적힌 유닛은 같은 소속에 들어가게 됩니다. (팀이 다르면 스쿼드 이름이 같아도 다른 소속입니다.)
    /// </summary>
    void UnitRoleInitate()
    {
        // 함수 설명 : 이 씬에 존재하는 모든 유닛들의 UnitRole의 정보들을 초기화합니다.
        humanUnitInEditor = GetHumanUnitArrayInEditorMode();
        CheckUnitNameOverlap(ref humanUnitInEditor, WarningIcon);
        int[][] recvIdArray = SetCurrentUnitRoleToFieldData(humanUnitInEditor);
        SetUnitRoleID(ref humanUnitInEditor, recvIdArray);
        

        //내가 할 것
        //(완료)1.게임매니저에 유닛의 목록을 담는 변수 만들기
        //(테스트 안함 / 완료)2.에디터 모드에서(인간)유닛을 모두 호출하여 배열에 담는 기능 만들기
        //(테스트 안함 / 완료)2.1.이름이 동일한 대상은 쪼개는 기능 만들기
        //3.1.받아온 정보를 필드데이터에 집어넣는 기능 만들기
        //3.2. UnitRole에게 각 아이디 값 할당.
        //4.구체적으로는 잘 모르겠지만 UnitSight를 활성화시켜 충돌시 정보를 집어넣는 기능을 활성화하기
        //4.1.UnitSight를 준비가 된 상태에서 instantiate
        //4.1.A.UnitSight를 생성하는 함수에서 instantiate하는 함수를 블럭으로 막기
        //4.1.B.unitSight를 생성하는 부분을 잘라와, 게임매니저에서 3번이 완료된 다음 부분에 삽입하기.
        //4.1.B.hint 1 : 2번 혹은 3번 정보 참조, 아마 게임오브젝트를 배열로 가지는 변수가 있을 것이다
        //5.
    }
    #region UnitRole <--> FieldData 유닛 멤버 변수

    #endregion

    /// <summary>
    /// UnitRole을 위해 존재합니다.
    /// 유닛의 게임오브젝트 정보를 배열에 저장합니다.
    /// </summary>
    /// <returns> GameObject[] 인간 유닛들을 담은 배열을 리턴합니다.</returns>
    GameObject[] GetHumanUnitArrayInEditorMode()
    {
        GameObject[] unitInEditor = GameObject.FindGameObjectsWithTag("Unit");
        GameObject[] returnValue = new GameObject[0];

        // unitInEditor의 원소를 하나씩 탐색합니다.
        for (int index = 0; index < unitInEditor.Length; index++)
        {
            // 만약 선택한 원소가 인간 유닛임이 판명나면
            if (unitInEditor[index].GetComponent<HumanUnitBase>() != null)
            {
                // 인간 유닛 배열에 집어넣습니다.
                AddElementInArray(ref returnValue, unitInEditor[index]);
            }
        }
        return returnValue;
    }
    /// <summary>
    /// 
    /// UnitRole에 동일한 유닛의 이름이 있다면 알림을 보내고 이름을 바꿉니다
    /// </summary>
    /// <remarks>
    /// 원소 중에서 UnitRole을 열어 동일한 이름을 가진 존재가 둘 이상 있으면 그 존재들에게 WarningIcon을 인스턴스화하고, 이름을 바꿉니다.
    /// </remarks>
    void CheckUnitNameOverlap(ref GameObject[] _targetArray, GameObject NoticeIcon)
    {
        Dictionary<string, GameObject> nameSet = new Dictionary<string, GameObject>(); // 중복을 허용하지 않는 이름의 목록.
        for(int index = 0; index < _targetArray.Length; index++)
        {
            string inputName = _targetArray[index].GetComponent<UnitRole>().roleForEditMode.unitName;
            try
            {
                // 이름을 넣음
                nameSet.Add(inputName, _targetArray[index]);
            }
            catch (ArgumentException)
            {
                // 이미 값이 존재함
                // 존재하는 녀석에게 ICON 소환
                // 이름을 바꿈

                // 존재하는 녀석에게 ICON 소환
                // 이미 존재하는 녀석에게 아이콘 소환
                Instantiate(WarningIcon, nameSet[inputName].transform.position, Quaternion.identity);

                // 넣을려는 녀석에게도 아이콘 소환
                Instantiate(WarningIcon, _targetArray[index].transform.position, Quaternion.identity);

                // 이름을 바꿈
                string oldName = inputName;
                while (nameSet.ContainsKey(inputName))
                {
                    inputName = $"{inputName}#";
                }
                _targetArray[index].GetComponent<UnitRole>().roleForEditMode.unitName = inputName;

                Console.WriteLine($"경고_잘못된_인스펙터_값_경고 : 이미 존재하는 유닛의 이름이 존재합니다. 따라서 예전 이름 {oldName}을 {inputName}으로 바꾸었습니다.");
            }
        }

    }
    /// <summary>
    /// 이 함수는 에디터용 함수입니다.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    int[][] SetCurrentUnitRoleToFieldData(GameObject[] target)
    {
        // 함수 설명
        // 인간 유닛의 UnitRole 컴포넌트의 인스펙터에 입력한 이름에 기반하여 CurrentFieldData에 값을 넣어봅니다.
        // 1. 목록에 이름이 있는지 체크합니다.
        // 2. A. 이름이 있다 -> 해당 팀으로 넣음
        // 2. B. 못보던 이름이다 -> 만들어서 넣음 (새로운 아이디와 함께)
        // 3. 아이디 값을 result 값에 넣습니다.
        // 배열의 유닛 롤에 기반하여 현재 필드데이터의 값에 아이디 

        // currentFieldData는 teamdata가 비어 있는 FieldData입니다
        int[][] result = new int[0][];
        Dictionary<string, int> _teamNameToID = new Dictionary<string, int>(); // 딕셔너리의 크기 -> 새 아이디의 값
        Dictionary<string[], int> _squadNameToID = new Dictionary<string[], int>(); // {팀 이름, 스쿼드 이름} 을 키 값으로 받습니다,
        Dictionary<string, int> _unitNameToID = new Dictionary<string, int>();

        Dictionary<String, Dictionary<String, int>> _teamAndSquadName2index = new Dictionary<string, Dictionary<string, int>>();


#warning 다 비슷한 로직이니까 다음에 위 3개를 배열로 만든 다음에, for 문 돌려서 각각 처리하자

        for (int index = 0; index < target.Length; index++)
        {
            UnitRole _targetRole = target[index].GetComponent<UnitRole>();
            int _teamID = 0, _squadID = 0, _unitID = 0; // 비어있는 TeamDataArray를 채워주므로
            int _teamIndex = -1, _squadIndex = 0, _unitIndex = -1; // 미할당인데, 일단 컴파일러 에러는 피하면서, 사용하면 예외를 내뿜도록 합니다. // 스쿼드 인덱스는 팀이 바뀔때마다, 0부터 다시 시작합니다.
            // 팀 작업
            string _inputTeamName = _targetRole.roleForEditMode.teamName;
            if (_teamNameToID.ContainsKey(_inputTeamName))
            {
                // 이름이 있음
                // 아이디 값을 구한다.
                _teamID = _teamNameToID[_inputTeamName];
                _teamIndex = _teamID + 1;
            }
            else
            {
                // 이름이 없음
                // 팀을 새로 판다
                // 이름을 입력한다.
                // 아이디 값을 새로 할당한다.
                // UnitID(팀,스쿼드,유닛 아이디 값 저장소)에 할당
                // 끝. 그 이후 작업은 if문 아래에 진행.
                TeamData _newTeam = new TeamData();

                _newTeam.name = _inputTeamName;

                _teamID = _teamNameToID.Count;
                _newTeam.ID = _teamID;

                _newTeam.unitID = new UnitID();
                _newTeam.unitID.team = _teamID;
                _teamNameToID.Add(_inputTeamName, _teamID);

                _newTeam.squads = new SquadData[0];

                _teamIndex = currentFieldData.teamDatas.Length;
                currentFieldData.teamDatas.Add(_newTeam);
            }
            
            // 스쿼드 작업
            string _inputSquadName = _targetRole.roleForEditMode.squadName;
            // 해당 스쿼드 이름이 있는지 확인한다
            bool _squadNameFound = false;
            foreach (KeyValuePair<string[], int> one in _squadNameToID)
            {
                // 이름이 있는 조건은 팀 이름과 스쿼드 이름 모두가 동일한지 체크한다.
                if (one.Key.SequenceEqual(new String[] { _inputTeamName, _inputSquadName }))
                {
                    _squadNameFound = true; // 이름이 존재하는거 확인
                    _squadID = one.Value;
                    break;
                }
            }

            // 인덱스 값 찾기
            if(_teamAndSquadName2index.ContainsKey(_inputTeamName))
            {
                if (_teamAndSquadName2index[_inputTeamName].ContainsKey(_inputSquadName) == false)
                {
                    _teamAndSquadName2index[_inputTeamName].Add(_inputSquadName, _teamAndSquadName2index[_inputTeamName].Count);
                }
            }
            else
            {
                _teamAndSquadName2index.Add(_inputTeamName, new Dictionary<string, int>());
                _teamAndSquadName2index[_inputTeamName].Add(_inputSquadName, _teamAndSquadName2index[_inputTeamName].Count);
            }
            _squadID = _teamAndSquadName2index[_inputTeamName][_inputSquadName];

            // 팀으로 작업 했을때랑 비슷한 알고리즘입니다. 윗부분 주석 읽으시면 됩니다.
            if (_squadNameFound == false)
            {
                SquadData _newSquad = new SquadData();

                _newSquad.name = _inputSquadName;

                // 마무리작업
                _newSquad.TeamID = _teamID;

                _squadID = _squadNameToID.Count();
                _newSquad.SquadID = _squadID;

#warning NullReferenceException: Object reference not set to an instance of an object.
                if (currentFieldData == null) { Debug.Log("DEBUG_GameManager.SetCurrentUnitRoleToFieldData : currentFieldData 널 값입니다."); }
                if (currentFieldData.teamDatas == null) { Debug.Log("DEBUG_GameManager.SetCurrentUnitRoleToFieldData : currentFieldData.teamDatas 널 값입니다."); }
                if (currentFieldData.teamDatas[_teamIndex] == null) { Debug.Log("DEBUG_GameManager.SetCurrentUnitRoleToFieldData : currentFieldData.teamDatas[_teamIndex] 널 값입니다."); }
                if (currentFieldData.teamDatas[_teamIndex].unitID == null) { Debug.Log("DEBUG_GameManager.SetCurrentUnitRoleToFieldData : currentFieldData.teamDatas[_teamIndex].unitID 널 값입니다."); } // 여기가 널 값이였네

                AddElementInArray(ref currentFieldData.teamDatas[_teamIndex].unitID.squad, _squadID);
                // 스쿼드 데이터에도 존재 하겠죠?
                _newSquad.UnitID = new UnitID();
                _newSquad.UnitID.team = _teamID;

                _squadNameToID.Add(new string[] { _inputTeamName, _inputSquadName }, _squadID);

                AddElementInArray(ref currentFieldData.teamDatas[_teamIndex].squads, _newSquad);
            }

            // 유닛 작업
            string _inputUnitName = _targetRole.roleForEditMode.unitName;
            if (_unitNameToID.ContainsKey(_inputUnitName))
            {
#warning 존재할 리가 없잖아
                _unitID = _unitNameToID[_inputUnitName];
            }
            else
            {
                // 저번에도 그랬듯이. 이것도 똑같습니다.
                UnitInSquadData _newUnit = new UnitInSquadData();

                _unitID = _unitNameToID.Count;
                _newUnit.memberID = _unitID;
                _unitNameToID.Add(_inputUnitName, _unitID);

                // _squadIndex와 length를 구해본다.
                Debug.Log($"DEBUG_GameManager.SetCurrentUnitRoleToFieldData : _squadIndex : {_squadIndex}, Length : {currentFieldData.teamDatas[_teamIndex].squads.Length}");
#warning IndexOutOfRangeException: Index was outside the bounds of the array. currentFieldData.teamDatas[_teamIndex].squads가 작아서 늘려야 할지도,

                AddElementInArray(ref currentFieldData.teamDatas[_teamIndex].unitID.unit, _unitID); // 전화번호부 설정

                _newUnit.squadID = _squadID;

                _newUnit.teamID = _teamID;

                // UnitRole 내용 채우기
                _newUnit.unitRoleData = new UnitRole.UnitRoleData();
                _newUnit.unitRoleData.teamID = _teamID;
                _newUnit.unitRoleData.squadID = _squadID;
                _newUnit.unitRoleData.unitID = _unitID;

                // 
                AddElementInArray(ref currentFieldData.teamDatas[_teamIndex]
                    .squads[_squadIndex].units, _newUnit);
                AddElementInArray(ref currentFieldData.teamDatas[_teamIndex].squads[_squadIndex].UnitID.unit, _unitID);
            }

            AddElementInArray(ref result, new int[] { _teamID, _squadID, _unitID });
        }

        
        return result;
    }
    /// <summary>
    /// q
    /// </summary>
    /// <param name="target"> UnitRole을 가지고 있는 게임오브젝트입니다. </param>
    /// <param name="inputID"> 설정할 아이디 값을 가지고 있는 배열입니다. "{팀 아이디, 스쿼드 아이디, 유닛 아이디}"를 원소로 가지고 있는 배열입니다.</param>
    void SetUnitRoleID(ref GameObject[] target, int[][] inputID)
    {
        for(int index = 0; index < target.Length; index++)
        {
            target[index].GetComponent<UnitRole>().SetData(
                new UnitRole.UnitRoleData()
                {
                    teamID = inputID[index][0],
                    squadID = inputID[index][1],
                    unitID = inputID[index][2]
                });
        }
    }
    #endregion


    #region UnitSight 활성화 메서드
    /// <summary>
    /// 이 세계에 존재하는 모든 인간 유닛들에 접근하여 각자 시야를 활성화시켜줍니다.
    /// </summary>
    void ActiveUnitSight()
    {
        GameObject[] _humans = GetHumanUnitArrayInEditorMode();
        for(int index = 0; index < _humans.Length; index++)
        {
            _humans[index].GetComponent<GameObjectList>().UnitSightMake();
        }
    }
    #endregion

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


    //public void AddComponentData(UnitBase.UnitBaseData componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    //{
    //    AddComponentDataHelper(ref targetFieldData.unitBaseComponentData, ref baseUnitData, componentData, "UnitBase");
    //}
    //public void AddComponentData(HumanUnitBase.OrganListData componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    //{
    //    AddComponentDataHelper(ref targetFieldData.organListComponentData, ref baseUnitData, componentData, "HumanUnitBase");
    //}
    //public void AddComponentData(UnitItemPack.UnitItemPackData componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    //{
    //    AddComponentDataHelper(ref targetFieldData.unitItemPackComponentData, ref baseUnitData, componentData, "UnitItemPack");
    //}



    public void AddComponentData<ComponentDataClass>(ComponentDataClass componentData, ref FieldData targetFieldData, ref BaseUnitData baseUnitData)
    {
        // 컴포넌트의 데이터를 FieldData에 집어넣고 BaseUnitData에 컴포넌트 이름과 자기 컴포넌트가 위치한 인덱스를 기록합니다.
        // AddComponentDataHelper는 반복되는 부분을 줄인 부분입니다. 윗부분 Region을 참고하세요

        Dictionary<Type, int> typeIntPairs = new Dictionary<Type, int>();
        typeIntPairs.Add(typeof(UnitBase.UnitBaseData), 100);
        typeIntPairs.Add(typeof(HumanUnitBase.HumanUnitBaseData), 201);
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
        #region help
        // 펙토리 메서드입니다.
        // fieldData에 임시 데이터를 집어넣습니다.

        // Team PlayerTeam
        // -자신의 스쿼드 2부대: 엔지니어, 공격대
        // --엔지니어: 감시자, 공병2
        // --공격대: 사수 4명
        // Team EnemyTeam
        // -자신의 스쿼드 1부대: 공격대
        // Team Neutral

        // 어떻게 만들 것인가
        // 팀을 만든다
        // 스쿼드를 만든다
        // 유닛을 만든다
        // 스쿼드 내부에 유닛을 집어넣는다
        // 팀 내부에 스쿼드를 집어넣는다.

        #endregion

        string location = "Demo";
        string PlayerTeam = "Player";
        string EnemyTeam = "EnemyTeam";

        //Debug.Log("DEBUG_GameManager.DemoFieldLoader() : 그 곳에 있습니다.");

        FieldData returnValue = new()
        {
            currentPlayLocation = new string[1] { location },

            playerTeamName = PlayerTeam,

            machineUnitDatas = new MachineUnitData[0],

            blocksData = new()
            
        };
        returnValue.teamDatas.Add(new TeamData[3]
            {
                new(0)
                {
                    name = PlayerTeam,
                    //ID = dataBase.GetNewTeamID(); // 만들어야 할 것들 // 아무래도 여기서도 불가능 할 것 같다\.
                    // 할당이 이미 끝난 상태에서 아이디값을 나눠주도록 하자.

                    squads = new SquadData[2]
                    {
                        new()
                        {
                            SquadID = 0, // <!하드코딩!>
                            name = "엔지니어 팀",

                            nameOfLocation = location,
                            nameOfPastLocation = "PlayerTeamHQ",
                            assingedMission = "none",
                            isDummyData = true,

                            units = new UnitInSquadData[3] // 감시자 하나, 엔지니어 둘
                            {
                                new()
                                {
                                    unitBaseData = new("human")
                                    {
                                        direction = new Vector3(0, 0, -1),
                                        teamName = PlayerTeam
                                    },
                                    organListData = new(),
                                    unitItemPackData = new("Radios", "Pistol", "Knife")
                                }, // 감시자
                                new()
                                {
                                    unitBaseData = new("human")
                                    {
                                        direction = new Vector3(0, 0, -1),
                                        teamName = PlayerTeam
                                    },
                                    organListData = new(),
                                    unitItemPackData = new("BuildTool", "Pistol", "Knife")
                                }, // 엔지니어
                                new()
                                {
                                    unitBaseData = new("human")
                                    {
                                        direction = new Vector3(0, 0, -1),
                                        teamName = PlayerTeam
                                    },
                                    organListData = new(),
                                    unitItemPackData = new("BuildTool", "Pistol", "Knife")
                                } // 엔지니어
                            }
                        },
                        new()
                        {
                            SquadID = 1,
                            name = "공격팀",

                            nameOfLocation = location,
                            nameOfPastLocation = "PlayerTeamHQ",
                            assingedMission = "none",
                            isDummyData = true,

                            units = new UnitInSquadData[4]
                            {
                                new(),
                                new(),
                                new(),
                                new()
                            }
                        } // 공격대
                    },
                    goal = new()
                }, // 플레이어 팀
                new(1)
                {
                    name = "EnemyTeam",
                    ID = 1,

                    squads = new SquadData[1]
                    {
                        new()
                        {
                            SquadID = 2,
                            name = "적 공격대",

                            nameOfLocation = location,
                            nameOfPastLocation = "EnemyTeamHQ",
                            assingedMission = "none",
                            isDummyData = true,

                            units = new UnitInSquadData[4]
                            {
                                new()
                                {
                                    unitBaseData = new()
                                    {
                                        direction = new Vector3(0, 0, -1),
                                        teamName = EnemyTeam
                                    },
                                    organListData = new(),
                                    unitItemPackData = new("Radios", "Pistol", "Knife")
                                },
                                new()
                                {
                                    unitBaseData = new()
                                    {
                                        direction = new Vector3(0, 0, -1),
                                        teamName = EnemyTeam
                                    },
                                    organListData = new(),
                                    unitItemPackData = new("Pistol", "Pistol", "Knife")
                                },
                                new()
                                {
                                    unitBaseData = new()
                                    {
                                        direction = new Vector3(0, 0, -1),
                                        teamName = EnemyTeam
                                    },
                                    organListData = new(),
                                    unitItemPackData = new("Pistol", "Pistol", "Knife")
                                },
                                new()
                                {
                                    unitBaseData = new()
                                    {
                                        direction = new Vector3(0, 0, -1),
                                        teamName = EnemyTeam
                                    },
                                    organListData = new(),
                                    unitItemPackData = new("Pistol", "Pistol", "Knife")
                                }
                            }
                        } // 적 공격팀
                    }
                }, // 적 팀에 대한 정보
                new(2)
                {
                    name = "Neutral",
                    ID = 2,

                    squads = new SquadData[0]
                } // 중립 팀
            });
        //returnValue.playerTeamName = "Player";
        //returnValue.teamDatas = new TeamData[3]; // 플레이어 팀 / 적 팀 / 중립 팀
        #region teamDatas


        // 0번째 스쿼드
        //TeamData playerTeam = new TeamData();
        //playerTeam.name = "Player";
        //playerTeam.ID = TeamData.GetNewTeamID(returnValue);
        //playerTeam.squads = new SquadData[2] { new SquadData(), new SquadData() };
        #region Player

        #endregion
        #region Blocks

        #endregion
        #region Territory

        #endregion

        #region Player Squad 1


        //playerTeam.squads[0].SquadID = SquadData.GetNewSquadID(returnValue);
        //playerTeam.squads[0].name = "PlayerStriker001";
        //playerTeam.squads[0].nameOfLocation = location;
        //playerTeam.squads[0].nameOfPastLocation = "AlphaHeadquarter";
        // 바깥에서 멤버 아이디 추가해 줄 것입니다.
        //playerTeam.squads[0].memberID = new int[4]; // 멤버도 추가할 것

        #region Squad Member 1 감시자 1
        //// BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        //// BaseUnitData 정보 채우기
        //UnitInSquadData PlayerStriker01Attacker = new UnitInSquadData();
        //// unitBaseData
        //PlayerStriker01Attacker.unitBaseData = new UnitBase.UnitBaseData("Human");
        //PlayerStriker01Attacker.unitBaseData.direction = new Vector3(0, 0, -1);
        //PlayerStriker01Attacker.unitBaseData.teamName = "Player";
        //// OrganData 추가하기
        //PlayerStriker01Attacker.organListData = new HumanUnitBase.HumanUnitBaseData();
        //// ItemData 추가하기
        //PlayerStriker01Attacker.unitItemPackData = new UnitItemPack.UnitItemPackData("Radios", "Pistol", "Knife");



        //PlayerStriker01Attacker.memeberID = GetNewUnitID(returnValue);

        //BaseUnitData PlayerStriker001_Attacker = new BaseUnitData();
        //PlayerStriker001_Attacker.direction = new Vector3(0, 0, -1);
        //PlayerStriker001_Attacker.ID = GetNewUnitID(returnValue);
        //PlayerStriker001_Attacker.unitType = "human";

        //// UnitBase, OrganData, ItemData 추가하기
        //AddComponentData(new UnitBase.UnitBaseData(PlayerStriker001_Attacker, "Player"), ref returnValue, ref PlayerStriker001_Attacker);
        //AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker001_Attacker);
        //AddComponentData(new UnitItemPack.UnitItemPackData("Radios", "Pistol", "Knife"), ref returnValue, ref PlayerStriker001_Attacker);
        //// Add this to there
        //AddElementInArray(ref returnValue.unitDatas, PlayerStriker001_Attacker);
        //AddElementInArray(ref playerTeam.squads[0].units[1].memberID, PlayerStriker001_Attacker.ID);

        //HumanUnitInfoData PlayerStriker001_AttackerHuman = new HumanUnitInfoData();
        //PlayerStriker001_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        //PlayerStriker001_AttackerHuman.BaseUnitDataID = PlayerStriker001_Attacker.ID;
        //PlayerStriker001_AttackerHuman.charactor = "kart";
        //PlayerStriker001_AttackerHuman.TeamID = playerTeam.ID;
        //AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker001_AttackerHuman);
        #endregion
        #region Squad Member 2 공격대 1
        //// BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        //// BaseUnitData 정보 채우기
        //BaseUnitData PlayerStriker002_Attacker = new BaseUnitData();
        //PlayerStriker002_Attacker.direction = new Vector3(0, 0, -1);
        //PlayerStriker002_Attacker.unitType = "human";

        //// Add this to there
        //AddElementInArray(ref returnValue.unitDatas, PlayerStriker002_Attacker);
        //AddElementInArray(ref playerTeam.squads[0].memberID, PlayerStriker002_Attacker.ID);

        //// UnitBase, OrganData, ItemData 추가하기
        //AddComponentData(new UnitBase.UnitBaseData(PlayerStriker002_Attacker, "Player"), ref returnValue, ref PlayerStriker002_Attacker);
        //AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker002_Attacker);
        //AddComponentData(new UnitItemPack.UnitItemPackData("Blank", "Pistol", "Knife"), ref returnValue, ref PlayerStriker002_Attacker);

        //HumanUnitInfoData PlayerStriker002_AttackerHuman = new HumanUnitInfoData();
        //PlayerStriker002_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        //PlayerStriker002_AttackerHuman.BaseUnitDataID = PlayerStriker002_Attacker.ID;
        //PlayerStriker002_AttackerHuman.charactor = "kart";
        //PlayerStriker002_AttackerHuman.TeamID = playerTeam.ID;
        //AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker002_AttackerHuman);
        #endregion
        #region Squad Member 2 공격대 2
        //// BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        //// BaseUnitData 정보 채우기
        //BaseUnitData PlayerStriker003_Attacker = new BaseUnitData();
        //PlayerStriker003_Attacker.direction = new Vector3(0, 0, -1);
        //PlayerStriker003_Attacker.ID = GetNewUnitID(returnValue);
        //PlayerStriker003_Attacker.unitType = "human";

        //// Add this to there
        //AddElementInArray(ref returnValue.unitDatas, PlayerStriker003_Attacker);
        //AddElementInArray(ref playerTeam.squads[0].memberID, PlayerStriker003_Attacker.ID);

        //// UnitBase, OrganData, ItemData 추가하기
        //AddComponentData(new UnitBase.UnitBaseData(PlayerStriker003_Attacker, "Player"), ref returnValue, ref PlayerStriker003_Attacker);
        //AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker003_Attacker);
        //AddComponentData(new UnitItemPack.UnitItemPackData("Blank", "Pistol", "Knife"), ref returnValue, ref PlayerStriker003_Attacker);

        //HumanUnitInfoData PlayerStriker003_AttackerHuman = new HumanUnitInfoData();
        //PlayerStriker003_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        //PlayerStriker003_AttackerHuman.BaseUnitDataID = PlayerStriker003_Attacker.ID;
        //PlayerStriker003_AttackerHuman.charactor = "kart";
        //PlayerStriker003_AttackerHuman.TeamID = playerTeam.ID;
        //AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker003_AttackerHuman);
        #endregion
        #region Squad Member 2 공격대 3
        // BaseUnitData, 컴포넌트 정보(UnitBaseData, OrganData, ItemData), HumanUnitInfoData

        // BaseUnitData 정보 채우기
        //BaseUnitData PlayerStriker004_Attacker = new BaseUnitData();
        //PlayerStriker004_Attacker.direction = new Vector3(0, 0, -1);
        //PlayerStriker004_Attacker.ID = GetNewUnitID(returnValue);
        //PlayerStriker004_Attacker.unitType = "human";

        //// Add this to there
        //AddElementInArray(ref returnValue.unitDatas, PlayerStriker004_Attacker);
        //AddElementInArray(ref playerTeam.squads[0].memberID, PlayerStriker004_Attacker.ID);

        //// UnitBase, OrganData, ItemData 추가하기
        //AddComponentData(new UnitBase.UnitBaseData(PlayerStriker004_Attacker, "Player"), ref returnValue, ref PlayerStriker004_Attacker);
        //AddComponentData(new HumanUnitBase.OrganListData(), ref returnValue, ref PlayerStriker004_Attacker);
        //AddComponentData(new UnitItemPack.UnitItemPackData("Blank", "Pistol", "Knife"), ref returnValue, ref PlayerStriker004_Attacker);

        //HumanUnitInfoData PlayerStriker004_AttackerHuman = new HumanUnitInfoData();
        //PlayerStriker004_AttackerHuman.SquadID = playerTeam.squads[0].SquadID;
        //PlayerStriker004_AttackerHuman.BaseUnitDataID = PlayerStriker004_Attacker.ID;
        //PlayerStriker004_AttackerHuman.charactor = "kart";
        //PlayerStriker004_AttackerHuman.TeamID = playerTeam.ID;
        //AddElementInArray(ref returnValue.humanUnitDatas, PlayerStriker004_AttackerHuman);
        #endregion

        #endregion
        #region Player Squad 2

        #endregion
        //returnValue.teamDatas[0] = playerTeam;

        #region Enemy Squad 1

        #endregion

        TeamData enemyTeam = new TeamData();
        enemyTeam.name = "enemyTeam";
        enemyTeam.squads = new SquadData[1] { new SquadData() };
        enemyTeam.squads[0].name = "EnemyStriker001";

        #endregion

        //returnValue.
        //returnValue.humanUnitDatas = new HumanUnitInfoData[3];

        
        returnValue.currentPlayLocation = new string[] { location };

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


        for (int teamIndex = 0; teamIndex < data.teamDatas.Length; teamIndex++)
        {
            for (int squadIndex = 0; squadIndex < data.teamDatas[teamIndex]?.squads?.Length; squadIndex++)
            {
                for(int unitIndex = 0; unitIndex < data.teamDatas[teamIndex].squads[squadIndex].units.Length; unitIndex++)
                {
                    // 매개변수로 들어간 유닛을 하나하나 골라서 인스턴스화합니다.
                    UnitInSquadData unit = data.teamDatas[teamIndex].squads[squadIndex].units[unitIndex];
                    if (unit == null) continue;
                    if (unit.unitBaseData == null) continue;
                    #region 1.1. 인스턴스화 - 프리펩 결정
                    // 유닛 타입 체크
                    // 타입에 따라 해당하는 유닛 타입 배열에 자신과 아이디가 같은 녀석을 찾음
                    // 세부 프리펩을 찾는다.
                    GameObject prefab;

                    // 일단 스위치 문으로 구현한 다음에
                    // 클래스와 상속을 이용한 다형성을 사용해서 다시 구현해보자.
                    
                    switch (unit.unitBaseData.prefabName)
                    {
                        // 인간 타입
                        case null:
                        case "HumanUnitDefault":        
                            prefab = HumanPrefabDefaultSkin;
                            break;
                        case "HumanUnitKart":
                            prefab = HumanPrefabKartSkin;
                            break;
                        // 머신 타입
                        // 머신 유닛의 프리펩은 머신 종류에 따라 나뉩니다. ex)103
                        default:
                            // 어떤 프리펩인지 모르므로 일단 기본 프리펩을 던져줍시다.


                            switch (unit.unitBaseData.unitType)
                            {
                                case "human":
                                    prefab = HumanPrefabDefaultSkin;
                                    break;
                                default:
                                    Debug.Log("DEBUG_GameManager.UnitInstantiate: 이 유닛의 타입(" + unit.unitBaseData.unitType + ")을 통해 프리펩을 결정할 수 없습니다.");
                                    continue;
                            }


                            break;
                    }


                    #endregion
                    #region 1.2. 인스턴스화 - 위치 결정

                    #endregion
                    #region 1.3. 인스턴스화 - 함수 호출/
                    GameObject InstantiatedObject = Instantiate(prefab);
                    #endregion
                    #region 2. 인스턴스화 한 대상들 컴포넌트 데이터 집어넣기

                    #endregion
                }
            }
        }
        


        //for (int unitIndex = 0; unitIndex < data.unitDatas.Length; unitIndex++) // 필드 데이터에 있는 모든 유닛Array의 원소들을 인스턴스화하도록 시도합니다.
        //{
        //    int humanUnitDataIndex = 0;
        //    #region 1.1. 인스턴스화 - 프리펩 결정
        //    // 유닛 타입 체크
        //    // 타입에 따라 해당하는 유닛 타입 배열에 자신과 아이디가 같은 녀석을 찾음
        //    // 세부 프리펩을 찾는다.
        //    GameObject prefab; // 프리펩입니다.
        //    switch (data.unitDatas[unitIndex].unitType) // 유닛의 타입을 찾습니다. 머신 / 인간 / +모더들이 추가한 유닛의 타입
        //    {
        //        case "human": // 인간 타입일때.
        //            // 인간 프리펩을 찾습니다

        //            int humanIndex = 0;
        //            // 휴먼 데이터가 몇번째 인덱스가 존재하는지 찾는 루프문입니다. (나중에 인덱스 어레이를 집어넣도록 하자.)
        //            for (; humanIndex < data.humanUnitDatas.Length; humanIndex++)
        //            {
        //                if (data.humanUnitDatas[humanIndex] == null) continue;

        //                if (data.humanUnitDatas[humanIndex].BaseUnitDataID == data.unitDatas[unitIndex].ID)
        //                {
        //                    humanUnitDataIndex = humanIndex;
        //                    break; // 휴먼 유닛 인덱스를 찾았습니다.
        //                }
        //            }
        //            // 유닛 데이터의 캐릭터 이름으로 프리펩을 찾습니다.
        //            switch (data.humanUnitDatas[humanUnitDataIndex].charactor)
        //            {
        //                case "kart":
        //                    prefab = HumanPrefabKartSkin;
        //                    break;
        //                default:
        //                    prefab = HumanPrefabDefaultSkin;
        //                    break;
        //            }
        //            break;
        //        case "machine": // 머신 타입일때.
        //            // 유닛의 프리펩은 머신 종류에 따라 나뉩니다. ex)103
        //            prefab = HumanPrefabDefaultSkin;
        //            break;
        //        default:
        //            Debug.Log("DEBUG_GameManager.UnitInstantiate: 이 유닛의 타입(" + data.unitDatas[unitIndex].unitType + ")을 통해 프리펩을 결정할 수 없습니다.");
        //            prefab = HumanPrefabDefaultSkin;
        //            break;
        //    }






        //    #endregion
        //    #region 1.2. 인스턴스화 - 위치 결정
        //    Vector3 position; // 유닛이 존재할 위치입니다.
        //    // 1. 데이터에 주둔군인지 여부를 결정합니다.
        //    // 1.a. 주둔군인경우 저장된 위치를 값으로 합니다.
        //    // 1.b. 주둔군이 아닌경우 스쿼드 / 혹은 유닛의 과거 위치를 찾아 과거 지역의 이름을 가지고있는 첫번째 비콘이 존재하는 위치에 넣습니다. // 이미 유닛이 배치되어있으면 그 비콘의 지역 이름이 같은 다음 비콘의 위치에 넣습니다.
        //    // 1.a.1. 
        //    // 
        //    if (data.unitDatas[unitIndex].isUnitStayedThatPlace) // 유닛이 주둔군인지 여부를 판별합니다.
        //    {
        //        position = data.unitDatas[unitIndex].position;
        //    }
        //    else
        //    {
        //        // 
        //        position = new Vector3(0, 1, 0);
        //    }

        //    #endregion
        //    // 1.3. 인스턴스 함수 호출
        //    GameObject instantiatedObject = Instantiate(prefab, position, Quaternion.identity);
        //}



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
    #region UnitInstantiate Helper Classes
    private class UnitInstantiaterBase
    {
        public virtual void MakeUnit()
        {

        }
    }
    private class HumanInstantiater : UnitInstantiaterBase
    {
        public override void MakeUnit()
        {
            base.MakeUnit();
            
        }
    }


    #endregion
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
    public void SetGameObjectComponentData(ref GameObject instantiatedGameObject, FieldData fieldData, int teamIndex, int squadIndex, int unitIndex)
    {
        #region 함수 설명
        // 생성된 게임오브젝트에 fieldData에 저장된 컴포넌트의 데이터들을 집어넣어주는 함수.
        // 입력 : 인스턴스된 게임오브젝트, 데이터가 저장된 필드데이터, 소속(팀, 스쿼드, 유닛)
        // 출력 : 없음. 대신 ref로 언급한 게임오브젝트의 컴포넌트의 데이터가 채워질 것입니다.

        // 만약 게임오브젝트에 컴포넌트가 있는데 fieldData에 해당하는 컴포넌트 값이 Null인경우, 무시합니다.
        // 대신 log를 내뿜습니다.
        // 같은 맥락으로, 게임오브젝트에 컴포넌트가 있는데 fieldData에 컴포넌트 데이터가 있는경우에도 무시합니다.
        // 이 역시 log를 내뿜습니다.
        #endregion


        UnitInSquadData unit = fieldData.teamDatas[teamIndex].squads[squadIndex].units[unitIndex];

        // UnitBase 컴포넌트 집어넣기.
        UnitBase unitBase = instantiatedGameObject.GetComponent<UnitBase>();
        if (unitBase == null && unit.unitBaseData != null)
        {
            Debug.Log("DEBUG_GameManager.SetGameObjectComponentData: fieldData 속에 이 게임오브젝트를 위한 Unit Base 컴포넌트가 존재하지 않습니다.");
        }
        else if (unitBase != null && unit.unitBaseData == null)
        {
            Debug.Log("DEBUG_GameManager.SetGameObjectComponentData: 이 게임오브젝트는 Unit Base 라는 컴포넌트를 가지고 있지 않습니다. 위치 : " + instantiatedGameObject.transform.position);
        }
        else if (unitBase != null && unit.unitBaseData == null)
        {
            unitBase.unitBaseData = unit.unitBaseData;
        }

        // HumanUnitBase 컴포넌트 집어넣기.
        HumanUnitBase humanUnitBase = instantiatedGameObject.GetComponent<HumanUnitBase>();
        if (humanUnitBase == null && unit.organListData != null)
        {
            Debug.Log("DEBUG_GameManager.SetGameObjectComponentData: fieldData 속에 이 게임오브젝트를 위한 Human Unit Base 컴포넌트가 존재하지 않습니다.");
        }
        else if (humanUnitBase != null && unit.organListData == null)
        {
            Debug.Log("DEBUG_GameManager.SetGameObjectComponentData: 이 게임오브젝트는 Human Unit Base 라는 컴포넌트를 가지고 있지 않습니다. 위치 : " + instantiatedGameObject.transform.position);
        }
        else if (humanUnitBase != null && unit.organListData == null)
        {
            humanUnitBase.initiateIndividual(unit.organListData);
        }

        // UnitItemPack 컴포넌트 집어넣기.
        UnitItemPack unitItemPack = instantiatedGameObject.GetComponent<UnitItemPack>();
        if (unitItemPack == null && unit.unitItemPackData != null)
        {
            Debug.Log("DEBUG_GameManager.SetGameObjectComponentData: fieldData 속에 이 게임오브젝트를 위한 Human Unit Base 컴포넌트가 존재하지 않습니다.");
        }
        else if (unitItemPack != null && unit.unitItemPackData == null)
        {
            Debug.Log("DEBUG_GameManager.SetGameObjectComponentData: 이 게임오브젝트는 Human Unit Base 라는 컴포넌트를 가지고 있지 않습니다. 위치 : " + instantiatedGameObject.transform.position);
        }
        else if (unitItemPack != null && unit.unitItemPackData == null)
        {
            unitItemPack.InventorySet(unit.unitItemPackData);
        }
    }
    #endregion
    #region 없던 유닛을 생성시키는 함수
    // 중간에 유닛이 생성하기를원하는 경우 이 함수를 호출해둬야 합니다.
    #region RegisterUnitHelper
    //public void RegisterUnitHelper_NewUnitData(ref BaseUnitData baseUnitData, Vector3 position/*, params object[] componentDatas */)
    //{

    //    // 아이디값 설정
    //    // 아이디가 0이 아니라도 일단 아무거나면 됐죠 으이?
    //    // 여기 람다쓰지말고 for루프 사용하자
    //    int temp = baseUnitData.ID;
    //    //baseUnitData.ID = GetNewUnitID(currentFieldData);
    //    if (temp == -1) temp = UnitInSquadData.GetNewUnitID(currentFieldData);
    //    else
    //    {
    //        for (int tIndex = 0; tIndex < currentFieldData.teamDatas.Length; tIndex++)
    //        {
    //            bool isFound = false;
    //            for (int sIndex = 0; sIndex < currentFieldData.teamDatas[tIndex].squads.Length; sIndex++)
    //            {
    //                for (int uIndex = 0; uIndex < currentFieldData.teamDatas[tIndex].squads[sIndex].units.Length; uIndex++)
    //                {
    //                    if (temp == currentFieldData.teamDatas[tIndex].squads[sIndex].units[uIndex].memberID)
    //                    {
    //                        baseUnitData.ID = UnitInSquadData.GetNewUnitID(currentFieldData);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    // 게임오브젝트 이름은 패스

    //    // 위치
    //    if (currentFieldData.currentPlayLocation != null)
    //    {
    //        if (currentFieldData.currentPlayLocation.Length > 0)
    //        {
    //            baseUnitData.nameOfLocation = currentFieldData.currentPlayLocation[0];
    //        }
    //    }
    //    baseUnitData.isUnitStayedThatPlace = true;
    //    baseUnitData.position = position;
    //    baseUnitData.direction = new Vector3(0, 0, -1);

    //    // 컴포넌트 정보
    //    //baseUnitData.usingComponentNames = ComponentDataTypeToString(componentDatas);
    //    //AddComponentData(componentDatas, ref currentFieldData, ref baseUnitData);

    //    //AddElementInArray(ref currentFieldData.unitDatas, baseUnitData);
    //}
    #endregion
    //public void RegisterUnit(ref BaseUnitData baseUnitData, HumanUnitInfoData humanUnitInfoData, Vector3 position, params object[] componentsDatas)
    //{
    //    // 휴먼 유닛을 필드 데이터에 넣어줍니다
    //    // 베이스 유닛 데이터를 집어넣습니다.
        
    //}
    //public void RegisterUnit(ref BaseUnitData baseUnitData, MachineUnitInfoData machineUnitInfoData, Vector3 position, params object[] componentsDatas)
    //{
    //    // 머신 유닛을 필드 데이터에 넣어줍니다.
    //    // EX) 유닛을 설치할때 사용합니다.
    //    // 필드 데이터에 유닛의 정보를 추가합니다.
    //    // 만약 매개변수로 들어온 값이 완전하지 않으면, 알아서 디펄트값을 채워줍니다.

    //    RegisterUnitHelper_NewUnitData(ref baseUnitData, position);
    //    baseUnitData.unitType = "machine";

    //    if (currentFieldData.teamDatas == null) { currentFieldData.teamDatas = new TeamData[0] { }; }
    //    int index = Array.FindIndex(currentFieldData.teamDatas, a => "Machine" == a.name);
    //    if (index == -1)
    //    {
    //        TeamData teamData = new TeamData();
    //        teamData.name = "Machine";
    //        AddElementInArray(ref currentFieldData.teamDatas, teamData);
    //    }



    //    AddElementInArray(ref currentFieldData.unitDatas, baseUnitData);

    //    Debug.Log("DEBUG_GameManager.RegisterUnit: 새로운 유닛 아이디가 배치됨 " + baseUnitData.ID);
    //}




    #endregion
    #region 유닛 인스턴스화
    //





    #endregion
    #region 외부 컴포넌트 관련 함수
    public string ComponentDataTypeToString(object componentDatas)
    {
        System.Type dataType = componentDatas.GetType();
        if (dataType == typeof(HumanUnitBase.HumanUnitBaseData))
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
    #region 서포터 클래스

    public class FunctionTransister
    {
        // 클래스 설명
        // 클래스 외부에서 실행해야 하는 함수가 있는데,
        // 해당 조건이 만족하지 못한다면 대리자를 걸고,
        // 조건이 만족한 상태라면 걸린 대리자를 즉시 실행한다.

        // 필드
        public event Void2Void caller;

        // 프라이빗 필드
        bool isReady = false; // 걸려있는 대리자가 실행할 것인가
        

    }





    #endregion
    #region Delegate
    public delegate void Void2Void();
    public delegate void TileMap2Void(ref Dictionary<Vector3, int> TileMap);
    #endregion
    #region 예외 Exception


    class CannotReeachFileException : Exception
    {
        public CannotReeachFileException() : base () { }
        public CannotReeachFileException(string message) : base(message) { }
    }
    #endregion


    public void PingPong()
    {
        Debug.Log("PONG! : GameManager의 함수를 호출 할 수 있습니다.");
    }
}