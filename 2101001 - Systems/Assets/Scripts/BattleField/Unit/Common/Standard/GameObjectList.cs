using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Instantiate 관련하여 작업을 도와주는 컴포넌트입니다.
// 게임오브젝트와 관련한 작업을 돕습니다.

/// <summary>
///     Instantiate 관련한 작업을 도와주는 함수입니다. </summary>
/// <remarks>
///     <para>
///         게임에 사용되는 다양한 프리펩을 static 변수로 소유하고 있으며, 프리펩들을 인스턴스화하는데 도움을 주기도 합니다. </para>
///     <para>
///         만약 어떤 클래스의 메서드가 Instantiate를 요구하는경우, 이 클래스를 사용할 수 있습니다. </para>
///     <para>
///         [!] 이젠 Load 함수를 이용해서 로딩을 합니다.</para></remarks>
public class GameObjectList : MonoBehaviour
{
    int unitLayerMaskIndex = 1;
    HumanActingGuiController humanActingGuiController;
    GameManager gm;
    public static UnitPrefabs unitPrefabs;
    public static BlockPrefabs blockPrefabs;
    public static AttackObjectPrefabs attackObjectPrefabs;
    /// <summary>
    ///     유닛 프리펩이 존재하는 폴더의 위치입니다. </summary>
    private const string M_UNIT_HUMAN_FOLDER = "Prefabs/BattleField/GameUnit/Original/Biological/Human/Charactor";
    /// <summary>
    ///     유닛 프리펩이 존재하는 폴더의 위치입니다. </summary>
    private const string M_UNIT_MACHINE_FOLDER = "Prefabs/BattleField/GameUnit/Original/Mechanic/MachineUnit";
    /// <summary>
    ///     블럭 프리펩이 존재하는 폴더의 위치입니다. </summary>
    private const string M_BLOCK_FOLDER = "";
    /// <summary>
    ///     AttackObject 프리펩이 존재하는 폴더의 위치입니다. </summary>
    private const string M_ATTACK_OBJECT_FOLDER = "Prefabs/BattleField/AttackObject";
    /// <summary>
    ///     Particles 프리펩이 존재하는 폴더의 위치입니다.</summary>
    private const string M_3D_ICON_FOLDER = "Prefabs/BattleField/Particles";
#warning todo : 여기에 게임오브젝트 옮겨담기.
    // 변수 묶음
    /// <summary>
    ///     유닛 게임오브젝트입니다. </summary>
    /// <remarks>
    ///     <para>
    ///         종류 : 인간 유닛, 머신 유닛</para></remarks>
    public struct UnitPrefabs
    {
        #region Property : which access to private member


        // biological
        // biological human
        public GameObject BasicHuman
        {
            get
            {
                if (m_basicHuman == null)
                    m_basicHuman = Resources.Load<GameObject>($"{M_UNIT_HUMAN_FOLDER}/HumanUnitDefault");
                return m_basicHuman;
            }
        }
        //public GameObject batez;
        //public GameObject nakita;
        //public GameObject claire;

        // mechanic
        // mechanic machine
        public GameObject EnergyStorage // 101
        {
            get
            {
                if (m_energyStorage == null)
                    m_energyStorage = Resources.Load<GameObject>($"{M_UNIT_MACHINE_FOLDER}/100_EnergyClass/MachineEnergyStorage");
                return m_energyStorage;
            }
        }
        public GameObject EnergyTransmission // 102
        {
            get
            {
                if (m_energyTransmission == null)
                    m_energyTransmission = Resources.Load<GameObject>($"{M_UNIT_MACHINE_FOLDER}/100_EnergyClass/MachineEnergyTransmission");
                return m_energyTransmission;
            }
        }
        public GameObject EnergyFarm // 103
        {
            get
            {
                if (m_energyFarm == null)
                    m_energyFarm = Resources.Load<GameObject>($"{M_UNIT_MACHINE_FOLDER}/100_EnergyClass/MachineEnergyTransmission");
                return m_energyFarm;
            }
        }
#warning 작업중입니다.
        public GameObject PlacementFixture; // 201
        public GameObject PlacementDrone; // 202
                                          // spider had been terminated
        public GameObject PlacementRcCar; // 204
        public GameObject SensorString; // 301
        public GameObject SensorCamera; // 302
        public GameObject NetworkCircuit; // 501
        public GameObject NetworkAntenna; // 502
        public GameObject NetworkRouter; // 502
        public GameObject AttackBomb; // 701
        public GameObject AttackTurret; // 702
        public GameObject DisturberWall; // 801
        public GameObject DisturberCamo_hologram; // 802
        public GameObject DisturberLight; // 803
        public GameObject ProductMine; // 901
        public GameObject ProductMove; // 902
        public GameObject ProductCraft; // 903
        public GameObject ProductSave; // 904
        #endregion
        // 신경 끝!
        #region private member : which contains real prefabs and uneditable
        // biological
        #region Biological Human
        private GameObject m_basicHuman;
        //public GameObject batez;
        //public GameObject nakita;
        //public GameObject claire;
        #endregion
        // mechanic
        // mechanic machine
        private GameObject m_energyStorage; // 101
        private GameObject m_energyTransmission; // 102
        private GameObject m_energyFarm; // 103
        private GameObject m_placementFixture; // 201
        private GameObject m_placementDrone; // 202
                                             // spider had been terminated
        private GameObject m_placementRcCar; // 204
        private GameObject m_sensorString; // 301
        private GameObject m_sensorCamera; // 302
        private GameObject m_networkCircuit; // 501
        private GameObject m_networkAntenna; // 502
        private GameObject m_networkRouter; // 502
        private GameObject m_attackBomb; // 701
        private GameObject m_attackTurret; // 702
        private GameObject m_disturberWall; // 801
        private GameObject m_disturberCamo_hologram; // 802
        private GameObject m_disturberLight; // 803
        private GameObject m_productMine; // 901
        private GameObject m_productMove; // 902
        private GameObject m_productCraft; // 903
        private GameObject m_productSave; // 904
        #endregion

    }
    /// <summary>
    ///     블럭에 사용되는 게임오브젝트입니다. </summary>
    public struct BlockPrefabs
    {

    }
    /// <summary>
    ///     공격에 사용되는 게임오브젝트를 묶어놓은 존재입니다. </summary>
    public struct AttackObjectPrefabs
    {
        public static GameObject BulletFlying
        {
            get
            {
                if (m_bulletFlying == null)
                    m_bulletFlying = Resources.Load<GameObject>($"{M_ATTACK_OBJECT_FOLDER}/Bullet");
                return m_bulletFlying;
            }
        }
        public static GameObject KnifeBlade
        {
            get
            {
                if (m_knifeBlade == null)
                    m_knifeBlade = Resources.Load<GameObject>($"{M_ATTACK_OBJECT_FOLDER}/KnifeBlade");
                return m_knifeBlade;
            }
        }

        private static GameObject m_bulletFlying; // 날아다니는 총알입니다.
        private static GameObject m_knifeBlade;
    }
    
    public static class ParticlesPrefabs
    {
        public static GameObject DeadIcon
        {
            get
            {
                if (m_deadIcon == null)
                {
                    m_deadIcon = Resources.Load<GameObject>($"{M_3D_ICON_FOLDER}/DeadIcon");
                }
                return m_deadIcon;
            }
        }

        private static GameObject m_deadIcon;
    }

    //LayerMask UnitLayerMask;
    // 뭔지 모르겠으면 awake를 보세요.
    void Awake()
    {
        unitLayerMaskIndex = 1 << LayerMask.NameToLayer("Unit");
        //UnitLayerMask = LayerMask.NameToLayer("Unit");
        //Debug.Log("DEBUG_GameObjectList.Awake: 유닛 레이어 마스크 설정 여부 확인. 이름 출력: " + LayerMask.LayerToName(UnitLayerMask));
    }
    void Start()
    {
        humanActingGuiController = GameObject.Find("HumanActionGUI").GetComponent<HumanActingGuiController>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    #region 1. 게임오브젝트 리스트
    #region MyRegion

    [Header("Item GameObject Prefab")]

    public GameObject KnifeBlade;
    // 필드 int: KnifeBlade소유자의 게임오브젝트ID(자해방지) (+ 이게임오브젝트가 충돌감지시, 부딛힌 대상의 고유ID스캔할 것.)
    public GameObject KnifeHand;

    public static GameObject PistolBullet;

    [Header("Unit Sight")]
    public GameObject UnitSightPrefab;
    //public GameObject UnitSightInstance;
    float UnitSightRadius = 6.0f;
    #endregion


    #region Machine 프리펩
    [Header("Machine Unit Prefab")]
    public GameObject EnergyStorage; // 101
    public GameObject EnergyTransmission; // 102
    public GameObject EnergyFarm; // 103
    public GameObject PlacementFixture; // 201
    public GameObject PlacementDrone; // 202
    // spider had been terminated
    public GameObject PlacementRcCar; // 204
    public GameObject SensorString; // 301
    public GameObject SensorCamera; // 302
    public GameObject NetworkCircuit; // 501
    public GameObject NetworkAntenna; // 502
    public GameObject NetworkRouter; // 502
    public GameObject AttackBomb; // 701
    public GameObject AttackTurret; // 702
    public GameObject DisturberWall; // 801
    public GameObject DisturberCamo_hologram; // 802
    public GameObject DisturberLight; // 803
    public GameObject ProductMine; // 901
    public GameObject ProductMove; // 902
    public GameObject ProductCraft; // 903
    public GameObject ProductSave; // 904
    #endregion
    #region GUI 프리펩
    [Header ("GUI Prefab")]
    public GameObject MachineSettingGUI;



    #endregion



    #endregion
    #region 2. 게임오브젝트 관련 작업 (주로 인스턴스화)
    // 순서: 오브젝트의 ABC순서대로.
    #region UnitSight
    public void UnitSightMake([CallerFilePath] string caller = "")
    {
        Debug.Log($"DEBUG_GameObjectList.UnitSightMake : 호출자 : {caller}");

        // UnitSight 프리펩을 이 인스턴스의 자식으로서 생성
        GameObject UnitSightInstance;
        UnitSightInstance = Instantiate(UnitSightPrefab, transform.position, Quaternion.identity) as GameObject;
        UnitSightInstance.transform.parent = gameObject.transform;
        UnitSightNewScale(3.0f);
        //transform.GetChild(1).localScale = new Vector3(6, 2, 6);
    }
    public void UnitSightNewScale(float radius)
    {
        // UnitSight 인스턴스에게 접근합니다.
        transform.Find("UnitSight(Clone)").localScale = new Vector3(radius * 2, 2, radius * 2);
        UnitSightRadius = radius;
    }
#warning 함수 구현 작업중
    public static void UnitSightScale(GameObject unit, float radius)
    {
        Hack.TrapNull(unit, Hack.isDebugGameObjectList);
        Hack.TrapNull(unit.GetComponent<Transform>(), Hack.isDebugGameObjectList);
        Hack.TrapNull(unit.GetComponent<Transform>().Find("UnitSight(Clone)"), Hack.isDebugGameObjectList);
        Transform targetTransform = unit.GetComponent<Transform>().Find("UnitSight(Clone)");
        if(targetTransform == null)
        {
            Hack.Error("GameObjectList", "targetTransform이 Null 값입니다.");
            return;
        }
        targetTransform.localScale = new Vector3(radius * 2, 2, radius * 2);
#warning 여기에 UnitSightRadius = radius; 관련 작업이 있어야 합니다.
    }
    /// <summary>
    /// 유닛이 바라보는 방향에 따라서 UnitSight의 위치를 변경합니다.
    /// (추론)연속적으로 호출됩니다
    /// </summary>
    /// <param name="vector3">이 유닛이 바라보는 방향입니다. 호출자가 인자로 넣어줍니다.</param>
    public void UnitSightDirectionSet(Vector3 vector3)
    {
        vector3 = vector3 * UnitSightRadius;
        Transform _unitSight = transform.Find("UnitSight(Clone)");
        if (_unitSight != null)
        {
            _unitSight.position = new Vector3(transform.position.x + vector3.x, 0, transform.position.z + vector3.z);
        }
    }
    #endregion
#warning ==== 여기까지가 유닛사이트 ====

    #region Knife: 사용, 스킬(미완)
    public static void KnifeBladeStab(GameObject attacker, AttackClassHelper.AttackInfo attackType, Vector3 direction)
    {
        direction.Normalize(); // 더 이상 이 벡터의 크기는 중요하지 않습니다.
        GameObject instantiatedGameObject = Instantiate(AttackObjectPrefabs.KnifeBlade, attacker.transform.position, Quaternion.LookRotation(direction, Vector3.up));
        instantiatedGameObject.transform.parent = attacker.transform;

        instantiatedGameObject.GetComponent<KnifeController>().Init(direction, attacker.GetInstanceID(), attacker.transform.position);
        instantiatedGameObject.GetComponent<AttackObject>().Set(attackType);
    }

    public void KnifeHandCatch(Vector3 direction, int UserUnitID)
    {
        direction.Normalize(); // 더 이상 이 벡터의 크기는 중요하지 않습니다.
        GameObject instantiatedGameObject = Instantiate(KnifeHand, transform.position, Quaternion.LookRotation(direction, Vector3.up));

    }
    #endregion
    #region Pistol: 사용
    public static void PistolBulletShot(GameObject attacker, AttackClassHelper.AttackInfo attackInfo, Vector3 direction)
    {
        direction.Normalize(); // 더 이상 이 벡터의 크기는 중요하지 않습니다.
        GameObject instantiatedGameObject = Instantiate(AttackObjectPrefabs.BulletFlying, attacker.transform.position, Quaternion.LookRotation(direction, Vector3.up));
        instantiatedGameObject.GetComponent<BulletController>().Init(direction, attacker.GetInstanceID());
        instantiatedGameObject.GetComponent<AttackObject>().Set(attackInfo, direction);
    }
    #endregion

#warning 프리펩 교체하기
    #region BuildTool: 사용, 보충(미완)
    // BuildTool
    public void Build(Vector3 Direction, int machineClassID, ref bool isSetable)
    {
        isSetable = false; // 의미없지만 혹시 모르니 false로
        // 설치 위치 확인
        Vector3 BuildPos = new Vector3();
        BuildPos = Direction + transform.position;
        Vector3 instantiatePosition = new Vector3();
        GameObject BuildObject;

        // GameObject 세팅
        switch (machineClassID)
        {
            case 101:
                BuildObject = EnergyStorage;
                break;
            case 102:
                BuildObject = EnergyTransmission;
                break;
            case 103:
                BuildObject = EnergyFarm;
                break;
            case 201:
                BuildObject = PlacementFixture;
                break;
            case 202:
                BuildObject = PlacementDrone;
                break;
            case 204:
                BuildObject = PlacementRcCar;
                break;
            case 301:
                BuildObject = SensorString;
                break;
            case 302:
                BuildObject = SensorCamera;
                break;
            case 501:
                BuildObject = NetworkCircuit;
                break;
            case 502:
                BuildObject = NetworkAntenna;
                break;
            case 503:
                BuildObject = NetworkRouter;
                break;
            case 701:
                BuildObject = AttackBomb;
                break;
            case 702:
                BuildObject = AttackTurret;
                break;
            case 801:
                BuildObject = DisturberWall;
                break;
            case 802:
                BuildObject = DisturberCamo_hologram;
                break;
            case 803:
                BuildObject = DisturberLight;
                break;
            case 901:
                BuildObject = ProductMine;
                break;
            case 902:
                BuildObject = ProductMove;
                break;
            case 903:
                BuildObject = ProductCraft;
                break;
            case 904:
                BuildObject = ProductSave;
                break;
                


            //C-1케이스

            default:
                Debug.Log("GameObjectList.Build : 머신 클래스 아이디에 대응하는 작업이 없습니다");
                return;
        }

        // machineClassID 확인 및 인스턴스할 게임오브젝트 장전
        // a. 일반 머신
        // b. 배치 머신
        // c. 통신 머신
        switch (machineClassID)
        {
            // A 케이스
            case 101:
            case 102:
            case 103:
            case 301:
            case 302:
            case 701:
            case 702:
            case 801:
            case 802:
            case 803:
            case 901:
            case 902:
            case 903:
            case 904:
                Ray RayToFindPlacement = new Ray();
                RayToFindPlacement.origin = transform.position;
                RayToFindPlacement.direction = Direction;
                RaycastHit RayToFindPlacementHit;
                Debug.DrawRay(RayToFindPlacement.origin, RayToFindPlacement.direction.normalized * 3.0f, Color.white, 1.2f);
                //if (!Physics.Raycast(RayToFindPlacement, out RayToFindPlacementHit, 3.0f, UnitLayerMask)) { Debug.Log("유닛에 닿지 못함"); isSetable = false; return; } // 3미터 이내에 존재하는가 ok
                if (!Physics.Raycast(RayToFindPlacement, out RayToFindPlacementHit, 3.0f, unitLayerMaskIndex)) { Debug.Log("유닛에 닿지 못함"); isSetable = false; return; } // 3미터 이내에 존재하는가 ok


                Debug.Log("망치에 부딛혔습니다! " + RayToFindPlacementHit.collider.gameObject.name); 
                // 부모 클래스로 간다음에 자식을 찾을까
                if (RayToFindPlacementHit.collider.gameObject.name != "PlacementColliderBox") { Debug.Log("DEBUG_GameObjectList.Build: 이 유닛에는 머신 유닛을 설치할 수 없습니다, 유닛의 이름: " + RayToFindPlacementHit.collider.gameObject.name); isSetable = false; return; }
                if (RayToFindPlacementHit.collider.gameObject.transform.parent.Find("FixedMachinePosition") == null) { Debug.Log("DEBUG_GameObjectList.Build: 설치할 자식 오브젝트가 없음"); isSetable = false; return; } // PlacementClass로서 자식 게임오브젝트가 있는지 확인합니다.
                if (RayToFindPlacementHit.collider.gameObject.transform.parent.GetComponent<MachinePlacement>().IsMachineConnected == true) { Debug.Log("DEBUG_GameObjectList.Build: 자식 오브젝트의 자리가 없음"); isSetable = false; return; } // 이미 머신을 설치했는지 체크합니다.
                instantiatePosition = RayToFindPlacementHit.collider.gameObject.transform.parent.position;
                // 아직 더 부족합니다 이미 배치된 것이 있는지도 확인해야합니다.
                Debug.Log("아이템 설치");
                GameObject instantiatedObject = Instantiate(BuildObject, instantiatePosition, Quaternion.identity);
                instantiatedObject.transform.parent = RayToFindPlacementHit.collider.gameObject.transform.parent.Find("FixedMachinePosition").transform;
                isSetable = true;
                // Collider은 아마 네모 상자일 것입니다.



                GameManager.BaseUnitData baseUnitDataMachine = new GameManager.BaseUnitData();
                GameManager.MachineUnitInfoData machineUnitInfoDataMachine = new GameManager.MachineUnitInfoData();
                //gm.RegisterUnit(ref baseUnitDataMachine, machineUnitInfoDataMachine, instantiatePosition);
                //

                //if (RayToFindPlacementHit.collider.gameObject.transform.Find("FixedMachinePosition(clone)") == null) return; // 부딛힌 목표물의 자식에 끼울 수있는 오브젝트가 있는가


                //RayToFindPlacementHit.collider.gameObject; -> // 

                break;
                // B-1 케이스 : 고정 설치 클래스 - 빈 타일에 설치 할 수 있습니다.
            case 201:


                break;
            // B-2 케이스 : 동적 설치 클래스 - 타일에 설치 할 수 있습니다.
            case 202:
            case 204:

                break;

            //C-1케이스
            case 501: // 타일에서도 설치할 수 있고, 설치 유닛에게도 설치할 수 있습니다.
                break;
            case 502: // 설치 유닛에게 설치합니다
                Ray RayToFindPlacement2 = new Ray();
                RayToFindPlacement2.origin = transform.position;
                RayToFindPlacement2.direction = Direction;
                RaycastHit RayToFindPlacement2Hit;
                if(!Physics.Raycast(RayToFindPlacement2, out RayToFindPlacement2Hit, 3.0f, unitLayerMaskIndex)) { Debug.Log("유닛에 닿지 못함"); isSetable = false; return; }

                if (RayToFindPlacement2Hit.collider.gameObject.name != "PlacementColliderBox") { Debug.Log("DEBUG_GameObjectList.Build: 이 유닛에는 네트워크 유닛을 설치할 수 없습니다, 유닛의 이름: " + RayToFindPlacement2Hit.collider.gameObject.name); isSetable = false; return; }
                if (RayToFindPlacement2Hit.collider.gameObject.transform.parent.Find("FixedNetworkPosition") == null) { Debug.Log("DEBUG_GameObjectList.Build: 설치할 자식 오브젝트가 없음"); isSetable = false; return; } // PlacementClass로서 자식 게임오브젝트가 있는지 확인합니다.
                if (RayToFindPlacement2Hit.collider.gameObject.transform.parent.GetComponent<MachinePlacement>().IsNetworkConnected == true) { Debug.Log("DEBUG_GameObjectList.Build: 자식 오브젝트의 자리가 없음"); isSetable = false; return; } // 이미 머신을 설치했는지 체크합니다.

                GameObject instantiatedObject2 = Instantiate(BuildObject, RayToFindPlacement2Hit.collider.gameObject.transform.parent.position, Quaternion.identity);
                instantiatedObject2.transform.parent = RayToFindPlacement2Hit.collider.gameObject.transform.parent.Find("FixedNetworkPosition").transform;
                isSetable = true;

                GameManager.BaseUnitData baseUnitDataNetworkAntenna = new GameManager.BaseUnitData();
                GameManager.MachineUnitInfoData machineUnitInfoDataNetworkAntenna = new GameManager.MachineUnitInfoData();
                //gm.RegisterUnit(ref baseUnitDataNetworkAntenna, machineUnitInfoDataNetworkAntenna, instantiatePosition);

                // 이거 함수화시키면 좋지 않을까
                break;
            case 503:
                break;
            default:
                Debug.Log("GameObjectList.Build : 머신 클래스 아이디에 대응하는 작업이 없습니다");
                return;
        }


        // A. 방향대로 레이저 발사
        // 머신에 닿았다면 그 머신의 3번째 자식의 자식이 있는지 체크
        // 없다면 인스턴스화
        // 나머지 머신갯수 줄이는 일은 유닛 아이템 팩에서 알아서 해 주겠지?
        //

        // B.
    }
    
    //
    public void ShowSubItem(UnitItemPack.BuildTool buildTool)
    {
        humanActingGuiController.ShowSubItem(buildTool);
    }
    public Collider AccessToMachine(Vector3 vector3)
    {
        // 자신의 위치에서 벡터의 방향으로 레이를 쏴 머신에 접근합니다.
        Ray rayToMachine = new Ray();
        rayToMachine.origin = transform.position;
        rayToMachine.direction = vector3;
        RaycastHit rayToMachineHit;
        if (Physics.Raycast(rayToMachine, out rayToMachineHit, 100.0f, unitLayerMaskIndex))
        {
            if (rayToMachineHit.collider.name == "PlacementColliderBox")
            {
                return rayToMachineHit.collider;
            }
            else if (rayToMachineHit.collider.GetComponent<MachineUnitBase>() == null)
            {
                Debug.Log("DEBUG_GameObjectList.TryTouchMachine: 닿은 유닛은 머신클래스 유닛이 아닙니다. 머신 이름: " + rayToMachineHit.collider.name);
                return null;
            }
            else
            {
                return rayToMachineHit.collider;
            }
        }
        return null;
    }


    public void TryTouchMachine(Vector3 vector3)
    {
        Ray rayToMachine = new Ray();
        rayToMachine.origin = transform.position;
        rayToMachine.direction = vector3;
        RaycastHit rayToMachineHit;
        if (Physics.Raycast(rayToMachine, out rayToMachineHit, 100.0f, unitLayerMaskIndex))
        {
            if (rayToMachineHit.collider.name == "PlacementColliderBox")
            {
                OpenMachineSettingGUI(rayToMachineHit.collider);
                return;
            }
            else if (rayToMachineHit.collider.GetComponent<MachineUnitBase>() == null)
            {
                Debug.Log("DEBUG_GameObjectList.TryTouchMachine: 닿은 유닛은 머신클래스 유닛이 아닙니다. 머신 이름: " + rayToMachineHit.collider.name);
                return;
            }
            else
            {
                OpenMachineSettingGUI(rayToMachineHit.collider);
            }


            // 유닛에 맞은 경우
            // 1. 사람에 맞음 ->패스
            // 2. 머신에 맞음 ->OpenMachineSettingGUI



        }


        // 자신의 위치에서 클릭한 방향대로 Ray 발사
        // 만약 Ray가 Unit 머신에 닿으면 OpenMachineSettingGUI 함수 호출



    }
    
    #region TryTouchMachine의 보조 함수: OpenMachineSettingGUI
    void OpenMachineSettingGUI(Collider collider)
    {
        GameObject.Find("UIController").GetComponent<UIController>().isGuiOpenedAtField = true;


        Vector3 GuiPosition;

        if (GameObject.Find("UICamera") == null)
        {
            Debug.Log("DEBUG_GameObjectList.OpenMachineSettingGUI: UICamera가 존재하지 않거나 찾을 수 없어서, GUI를 배치할 수 없습니다.");
            return;
        }
        if (GameObject.Find("UIHelper") == null)
        {
            Debug.Log("DEBUG_GameObjectList.OpenMachineSettingGUI: UICamera가 존재하지 않거나 찾을 수 없어서, GUI를 배치할 수 없습니다.");
            return;
        }

        GuiPosition = GameObject.Find("UIHelper").transform.position + new Vector3(0.0f, -0.75f, 0.0f);
        GameObject instantiatedGameObject = Instantiate(MachineSettingGUI, GuiPosition, Quaternion.identity);
        instantiatedGameObject.transform.parent = GameObject.Find("UIHelper").transform;
        instantiatedGameObject.GetComponent<MachineSettingGuiController>().ReadyGUI(collider);

        //Debug.Log("DEBUG_GameObjectList.OpenMachineSettingGUI: 짜잔! 함수가 호출되었습니다.");



    }

    public static void CloseMachineSettingGUI()
    {
        Destroy(GameObject.Find("MachineSettingGUI(Clone)"));
    }


    #endregion
    #endregion
    #region 공통
    public void HideSubItem()
    {
        humanActingGuiController.HideSubItem();
    }


    #endregion
    #endregion
    
    #region Static Function
    #region Machine
    #region Machine Class ID -> Machine Class Name
    static public string MachineIdToNameKR(int ID)
    {
        switch (ID)
        {
            case 0:
                return "비어있음";
            case 101:
                return "에너지 스토리지";
            case 102:
                return "에너지 전달탑";
            case 103:
                return "에너지 팜";
            case 201:
                return "고정형 거치대";
            case 202:
                return "드론형 거치대";
            case 203:
                return "거미형 로봇(삭제됨)";
            case 204:
                return "RC카 거치대";
            case 301:
                return "센서 스트링";
            case 302:
                return "센서 카메라";
            case 303:
                return "센서 마이크(삭제됨)";
            case 501:
                return "유선 통신기";
            case 502:
                return "무선 통신기";
            case 503:
                return "중추 통신기";
            case 601:
                return "컨트롤 함수";
            case 602:
                return "컨트롤 메모리";
            case 701:
                return "확산용 폭탄";
            case 702:
                return "발사용 터렛";
            case 703:
                return "가시 집게";
            case 801:
                return "바리게이트";
            case 802:
                return "위장용 홀로그램";
            case 803:
                return "라이트블럽";
            case 901:
                return "채취 디바이스";
            case 902:
                return "전달 디바이스";
            case 903:
                return "가공 디바이스";
            case 904:
                return "저장 디바이스";
            default:
                return "ERROR_GameObjectList";
        }
    }


    #endregion
    #region Random Machine Collider -> Machine Placement Trasnform
    static public Transform ReachPlacementMachineTransform(Collider TargetMachine)
    {
        // 함수 설명: TargetMachine을 기반으로 설치 클래스 머신 트랜스폼을 찾아 TargetPlacementMachineTransform에 저장합니다.
        // 닿은 물체가 설치용 머신인지 체크,
        // 아니라면 부모의 설치용 머신을 찾는다.

        if (TargetMachine.gameObject.GetComponent<MachinePlacement>() != null) // 닿은 머신은 설치용 머신입니다.
        {
            Debug.Log("DEBUG_MachineSettingGuiController.ReachPlacementMachine: 닿은 머신은 설치용 머신입니다.");
            return TargetMachine.transform;
        }
        else if (TargetMachine.name == "PlacementColliderBox") // 닿은 머신은 설치용 머신의 자식입니다.
        {
            Debug.Log("DEBUG_MachineSettingGuiController.ReachPlacementMachine: 닿은 머신은 설치용 머신의 자식입니다.");
            return TargetMachine.transform.parent;

        }
        else // 닿은 머신은 설치용 머신에 붙은 머신입니다.
        {
            Debug.Log("DEBUG_MachineSettingGuiController.ReachPlacementMachine: 닿은 머신은 설치용 머신에 붙은 머신입니다.");
            return TargetMachine.transform.parent.parent;
        }

        //Debug.Log("DEBUG_MachineSettingGuiController.ReachPlacementMachine: TargetPlacementMachineTransform의 이름: " + TargetPlacementMachineTransform.gameObject.name);
    }


    #endregion

    #endregion


    #endregion
    #region Function


    // public method



    #endregion

}
