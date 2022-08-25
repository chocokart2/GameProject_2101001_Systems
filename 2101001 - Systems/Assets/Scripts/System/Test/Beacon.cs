using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    // 캐릭터가 소환되도록 게임매니저에 소환 준비 데이터에 자신을 억지로 집어넣습니다


    #region Field
    #region UnitInfoData
    public string nameID = "Default Name";

    #endregion

    // GameManager
    #region BaseUnit
    //title BaseUnitData
    public string unitType = "human";
    public string gameObjectName = "HumanDummy";
    public Vector3 direction = new Vector3(0, 0, -1);
    #endregion
    #region HumanUnitInfo
    //title HumanUnitInfo
    string charactor = "kart";
    #endregion

    // Component
    #region UnitBase
    public string UnitBaseUnitType = "human";
    //public int UnitBaseID = -256;
    public string UnitBaseTeam = "unassigned";
    public string UnitBaseSquad = "unassigned";
    #endregion
    #region HumanUnitBase
    public string[] chemicals;


    #endregion
    #region UnitItemPack
    // public string[] ItemList = new string[3] { "Knife", "Pistol", "BuildTool" };
    public string item1 = "Knife";
    public string item2 = "Pistol";
    public string item3 = "BuildTool";

    #endregion

    #region Component
    GameManager gameManager;
    #endregion
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("DEBUG_Beacon.Start:저 작동 중입니다! " + transform.position);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // 이벤트에 등록
        GameObject.Find("GameManager").GetComponent<GameManager>().BeconEventForUnit += delegate (ref GameManager.FieldData myData)
        {
            Debug.Log("*** info_Beacon_Start : Beacon " + gameObject.name + " added Fake UnitInfo " + transform.position);
            //RegisterWithGameManagerCallBack(ref myData);
        };
        //Debug.Log("DEBUG_Beacon.Start:저 작동 중입니다! 2 " + transform.position);
    }
    void Start()
    {


    }


    /*
    public void RegisterWithGameManagerCallBack(ref GameManager.FieldData targetField)
    {
        #region 함수 설명
        // 나도 뭔지 모르겠다.
        #endregion


        Debug.Log("DEBUG_Beacon: 야호! " + transform.position);
        // 1. teamDatas

        // 알고리즘 수정
        // 만약에 targetField에서 UnitBaseTeam의 내용을 이름으로 하고 있는 팀이 있는지 여부를 판단.
        // 있으면 그 팀에 집어넣고 그렇지 않으면 팀을 따로 파서 넣는다.

        // 자신의 팀이 어디 있는지 찾는 배열입니다.
        bool isTeamFound = false; int myTeamIndex = 0; // 자신의 팀이 어느 인덱스에 존재하는지 알려줍니다.
        bool isSquadFound = false; int mySquadIndex = 0; // 자신의 스쿼드가 어느 인덱스에 존재하는지 알려줍니다.
        for(int teamIndex = 0; targetField.teamDatas.Length > teamIndex; teamIndex++) // 팀 배열의 머리부터 발끝까지 뒤집니다.
        {
            #region Error Hunter: Null cacher
            if(targetField.teamDatas[teamIndex] == null)
            {
                Debug.Log("DEBUG_Beacon.RegisterWithGameManagerCallBack: targetField의 teamDatas가 null입니다.");
                continue;
            }

            #endregion

            if (targetField.teamDatas[teamIndex].name == UnitBaseTeam) // 만약 이번 팀이 찾는 팀의 이름과 동일하다면
            {
                //팀을 찾은거고,
                isTeamFound = true;
                myTeamIndex = teamIndex;
                break;
            }
        }
        if (isTeamFound)
        {
            // 팀을 찾았으면 찾은 팀에다 넣습니다.
            // 해당 팀에 자신의 스쿼드가 존재하는지 체크합니다.
            // 스쿼드가 있으면 넣고 스쿼드가 없으면 만듭니다.

            // 자신의 팀에 자신의 유닛을 집어넣습니다.

            // 스쿼드를 이름따라 찾고 인덱스를 찾습니다.
            for(int squadIndex = 0; targetField.teamDatas[myTeamIndex].squads.Length > squadIndex; squadIndex++)
            {
                if(targetField.teamDatas[myTeamIndex].squads[squadIndex].name == UnitBaseSquad)
                {
                    isSquadFound = true; mySquadIndex = squadIndex;
                    break;
                }
            }
            if (isSquadFound)
            {
                // 스쿼드를 찾았습니다. 끝입니다.
            }
            else
            {
                GameManager.SquadData dummySquad = new GameManager.SquadData();

                dummySquad.SquadID = GameManager.SquadData.GetNewSquadID(targetField);
                dummySquad.name = UnitBaseSquad;
                // locaction, pastlocaction은 null로 둡니다.
                ///dummySquad.memberID = new int[1] { fakeBaseUnitData.ID };
                // assignedMission은 null로 둡니다. 목표가 현재 없습니다.
                gameManager.AddElementInArray(ref targetField.teamDatas[myTeamIndex].squads, dummySquad);
            }


        }
        else // 완료
        {
            // 팀을 못 찾았으면 팀을 넣습니다. 유닛을 담을 스쿼드도 준비합ㄴ디ㅏ.
            
            GameManager.TeamData beaconTeam = new GameManager.TeamData();
            #region 여기에 추가할 팀 만들기

            beaconTeam.name = UnitBaseTeam;
            beaconTeam.ID = GameManager.TeamData.GetNewTeamID(targetField);

            
            GameManager.SquadData dummySquad = new GameManager.SquadData();
            #region 이 팀에 추가할 스쿼드 만들기.
            dummySquad.SquadID = GameManager.SquadData.GetNewSquadID(targetField);// gameManager.GetNewSquadID(targetField);
            dummySquad.name = UnitBaseSquad;
            // locaction, pastlocaction은 null로 둡니다.
            //dummySquad.memberID = new int[1] { fakeBaseUnitData.ID };
            // assignedMission은 null로 둡니다. 목표가 현재 없습니다.

            #endregion
            gameManager.AddElementInArray(ref beaconTeam.squads, dummySquad);
            #endregion
            gameManager.AddElementInArray(ref targetField.teamDatas, beaconTeam);
        }



        // 해야 할 것. 팀에다가 자신의 아이디를 집어넣기
        #region 숨기기
        // 2. BaseUnitData
        //GameManager.BaseUnitData fakeBaseUnitData = new GameManager.BaseUnitData();
        //fakeBaseUnitData.ID = GameManager.UnitInSquadData.GetNewUnitID(targetField);
        //fakeBaseUnitData.unitType = unitType;
        //fakeBaseUnitData.gameObjectName = gameObjectName;
        //fakeBaseUnitData.isUnitStayedThatPlace = true;
        //fakeBaseUnitData.position = transform.position;
        //fakeBaseUnitData.direction = direction;
        //fakeBaseUnitData.isDummyData = true;
        #endregion
        // 2. UnitInSquadData 추가하기
        GameManager.UnitInSquadData fakeUnitData = new GameManager.UnitInSquadData();

        


        // 컴포넌트 정보 추가하기
        gameManager.AddComponentData(new UnitBase.UnitBaseData(fakeBaseUnitData, UnitBaseTeam), ref targetField, ref fakeBaseUnitData);
        gameManager.AddComponentData(new HumanUnitBase.OrganListData(), ref targetField, ref fakeBaseUnitData);
        gameManager.AddComponentData(new UnitItemPack.UnitItemPackData(item1, item2, item3), ref targetField, ref fakeBaseUnitData);
        // Add this to there
        gameManager.AddElementInArray(ref targetField.unitDatas, fakeBaseUnitData);
        gameManager.AddElementInArray(ref targetField.teamDatas[myTeamIndex].squads[mySquadIndex].memberID, fakeBaseUnitData.ID);

        // 3. HumanUnitInfoData
        GameManager.HumanUnitInfoData fakeHumanUnitInfoData = new GameManager.HumanUnitInfoData();
        fakeHumanUnitInfoData.SquadID = targetField.teamDatas[myTeamIndex].squads[mySquadIndex].SquadID;
        fakeHumanUnitInfoData.BaseUnitDataID = fakeBaseUnitData.ID;
        fakeHumanUnitInfoData.charactor = "kart";
        fakeHumanUnitInfoData.TeamID = targetField.teamDatas[myTeamIndex].ID;
        gameManager.AddElementInArray(ref targetField.humanUnitDatas, fakeHumanUnitInfoData);

        Debug.Log("Debug_Becon.RegisterWithGameManagerCallBack: 자신의 주둔군 여부 : " + fakeBaseUnitData.isUnitStayedThatPlace);


        //// 베이스 유닛 데이터
        //GameManager.BaseUnitData fakeBaseUnitData = new GameManager.BaseUnitData();
        //int thisUnitID = gameManager.GetNewUnitID(targetField);
        //fakeBaseUnitData.ID = thisUnitID;
        //fakeBaseUnitData.unitType = unitType;
        //fakeBaseUnitData.gameObjectName = gameObjectName;
        //fakeBaseUnitData.isUnitStayedThatPlace = true;
        //fakeBaseUnitData.position = transform.position;
        //fakeBaseUnitData.direction = direction;
        //gameManager.AddElementInArray(ref targetField.unitDatas, fakeBaseUnitData);

        //// 휴먼 유닛 데이터
        //GameManager.HumanUnitInfoData fakeHumanUnitData = new GameManager.HumanUnitInfoData();
        //fakeHumanUnitData.BaseUnitDataID = thisUnitID;
        //fakeHumanUnitData.charactor = charactor;
        //fakeHumanUnitData.unitBaseData = new UnitBase.UnitBaseData(UnitBaseUnitType, UnitBaseID, UnitBaseTeam);
        //fakeHumanUnitData.unitItemPackData = new UnitItemPack.UnitItemPackData();
        //fakeHumanUnitData.unitItemPackData.inventory = new UnitItemPack.ItemData[3];
        //fakeHumanUnitData.unitItemPackData.inventory[0] = new UnitItemPack.ItemData();
        //fakeHumanUnitData.unitItemPackData.inventory[1] = new UnitItemPack.ItemData();
        //fakeHumanUnitData.unitItemPackData.inventory[2] = new UnitItemPack.ItemData();
        //fakeHumanUnitData.unitItemPackData.inventory[0].itemType = item1;
        //fakeHumanUnitData.unitItemPackData.inventory[1].itemType = item2;
        //fakeHumanUnitData.unitItemPackData.inventory[2].itemType = item3;
        //fakeHumanUnitData.unitItemPackData.inventory[0].isRealItem = false;
        //fakeHumanUnitData.unitItemPackData.inventory[1].isRealItem = false;
        //fakeHumanUnitData.unitItemPackData.inventory[2].isRealItem = false;
        //gameManager.AddElementInArray(ref targetField.humanUnitDatas, fakeHumanUnitData);

        //GameManager.UnitInfoData fakeUnitData = new GameManager.UnitInfoData();
        //fakeUnitData.ID = nameID;
        //fakeUnitData.charactor = "Kart";
        //fakeUnitData.unitBaseData = new UnitBase.UnitBaseData(UnitBaseUnitType, UnitBaseID, UnitBaseTeam);
        //fakeUnitData.organListData = new HumanUnitBase.OrganListData();
        //fakeUnitData.unitItemPackData = new UnitItemPack.UnitItemPackData();
        //fakeUnitData.unitItemPackData.inventory = new UnitItemPack.ItemData[3];
        //fakeUnitData.unitItemPackData.inventory[0] = new UnitItemPack.ItemData();
        //fakeUnitData.unitItemPackData.inventory[1] = new UnitItemPack.ItemData();
        //fakeUnitData.unitItemPackData.inventory[2] = new UnitItemPack.ItemData();
        //fakeUnitData.unitItemPackData.inventory[0].itemType = item1;
        //fakeUnitData.unitItemPackData.inventory[1].itemType = item2;
        //fakeUnitData.unitItemPackData.inventory[2].itemType = item3;
        //fakeUnitData.unitItemPackData.inventory[0].isRealItem = false;
        //fakeUnitData.unitItemPackData.inventory[1].isRealItem = false;
        //fakeUnitData.unitItemPackData.inventory[2].isRealItem = false;
        //fakeUnitData.isGarrisonForHoldingPosition = true;
        //fakeUnitData.HoldingPosition = transform.position;
        //targetField.Add(fakeUnitData);
    }
    /**/


    // 1. Public으로 외부에서 데이터를 입력
    // 2. 입력받은 데이터를 기반으로 유닛 정보를 완성
    // 3. 유닛 정보를 바탕으로 FieldBooter에게 이 데이터를 삽입
    // FieldBooter는 데이터를 기반으로 유닛을 생성합니다.
}