using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning 아이템 클래스는 ItemHelper로 이동했습니다
/// <summary>
/// 유닛의 인벤토리에 대해 정의합니다.
/// </summary>
public class UnitItemPack : 
    ItemHelper,
    BaseComponent.IDataGetableComponent<ItemHelper.Inventory>, 
    BaseComponent.IDataSetableComponent<ItemHelper.Inventory>
{
    //GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    //  내부 정보
    //      이 유닛이 가지고 있는 아이템 (Item 인스턴스 배열) (1번 아이템, 2번 아이템, 3번 아이템)
    //      이 유닛이 현재 들고 있는 아이템 번호
    //      UnitBase(나중에)

    //  추상 클래스 Item
    //      기본 작동들(use, skill, supply)
    //      + 그 아이템의 부속된 아이템(남은 탄약, 남은 머신 클래스 유닛)
    //  추상 클래스를 상속하는 아이템 클래스들

    public Inventory inventory; // 아이템을 저장하는 어레이입니다. 유닛 타입이 human이면 요소가 3개이고, 머신이면 요소가 1개입니다
    public int inventoryIndex;
    UnitBase myUnitBase;
    public float Cooldown; // 다른 행동을 하기까지 기다려야 하는 시간. (단위: 초)
    public float CooldownKnifeSkill; // 손목 잡기 스킬

    #region DEBUG용 필드
    [System.Serializable]
    public struct UnitItemPackDebugCheckBox
    {
        /// <summary>
        ///     인벤토리에 아이템 집어넣을때 콘솔창에 올릴지 여부를 넣습니다
        /// </summary>
        public bool DEBUG_InventorySetFuncSayItemType;
        public bool DEBUG_InventoryUseSayName;
    }
    public UnitItemPackDebugCheckBox debugSetting;
    //[SerializeField] 
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        inventoryIndex = 0;

        Cooldown = 0.0f;
        CooldownKnifeSkill = 0.0f;
        #region 아이템 구체화



        #endregion
        #region 인벤토리 조작

        if(gameObject.name == "HumanPlayer")
        {
            inventory = new Inventory()
            {
                self = new ItemHelper.Item[3]
                {
                    DemoItem.GetDemoKnife(),
                    DemoItem.GetDemoPistol(),
                    DemoItem.GetDemoBuildTool()
                }
            };


            //inventory = new Item[3];
            //inventory[0] = new Radios(GetComponent<GameObjectList>());
            //inventory[1] = new Pistol(GetComponent<GameObjectList>());
            //inventory[2] = new Knife(GetComponent<GameObjectList>());
            //inventory[2] = new BuildTool(GetComponent<GameObjectList>());
        }



        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        //if (Cooldown > 0.0f) { Debug.Log("아이템 사용 불가"); }
        if (Cooldown >= 0.0f) Cooldown -= Time.deltaTime;
        if (CooldownKnifeSkill >= 0.0f) CooldownKnifeSkill -= Time.deltaTime;
        if(inventory != null)
        {
            inventory.Update(gameObject, Time.deltaTime);
        }

        
    }


    #region 아이템 기본 클래스 / 아이템 클래스들
    [System.Obsolete("사용하지 않는 클래스입니다.")]
    [System.Serializable]
    public class ItemData
    {
        public string itemType; // 아이템의 종류를 입력합니다. // 사용 가능한 itemType 종류는 InventorySet(UnitItemPackData data) 함수를 참고하세요
        public bool isRealItem; // 데이터를 통해 생성된 아이템인지 체크합니다. 

        // isRealItem == true이면 아래 필드를 참고합니다. 그렇지 않다면 null이여도 생성 시 알아서 채워주니 괜찮을겁니다.
        public string[] subItemName; // 이 아이템에 서브로 사용되는 소모품의 이름들입니다.
        public int[] subItemAmount; // 이 아이템에 서브로 사용되는 소모품의 갯수들입니다.
        public AttackClassHelper.AttackInfo[] attackInfos; // 이 아이템의 공격 클래스입니다.
    }
    #region ItemData 생성자: NewItemData(), NewItemData(string itemType)
    public static ItemData NewItemData()
    {
        ItemData returnValue = new ItemData();
        returnValue.itemType = "Blank";
        returnValue.isRealItem = false;
        returnValue.subItemName = new string[] { "NotSubItem" };
        returnValue.subItemAmount = new int[] { 1 };
        returnValue.attackInfos = new AttackClassHelper.AttackInfo[] { DemoAttackInfo.GetDemoAttackInfoData() };
        return returnValue;
    }
    public static ItemData NewItemData(string itemType)
    {
        ItemData returnValue = NewItemData();
        returnValue.itemType = itemType;
        return returnValue;
    }
    #endregion

    public abstract class Item
    {
        public bool isRealItem; // 아이템이 공식 설정이나, JsonData로 입력받았다면 True, Becon으로 받는다면 False입니다.
        protected GameObjectList gameObjectList; // 아이템 인스턴스화 함수등이 담긴 컴포넌트입니다.
        public Dictionary<string, int> subItem; // 이 아이템을 사용하기 위해 기타 소모품의 이름과 그 물품의 갯수의 짝입니다.
        public List<AttackClassHelper.AttackInfo> attackInfos; // 이 아이템을 이용해 공격을 할 시, 사용되는 공격 클래스입니다.

        // float 값을 리턴하는 이유: 행동을 하고 나면 쿨다운이 있기 때문입니다.
        public abstract float Use(Vector3 vector3); // 아이템을 사용할 시 호출됩니다.
        public abstract float Skill(Vector3 vector3); // 아이템 스킬을 사용할 시 호출됩니다.
        public abstract float Supply(Vector3 vector3); // 아이템 보충용 스킬을 사용할 시 호출됩니다.
        public abstract float Update(float deltaTime); // 매 프레임마다 호출되는 함수입니다.

        // 그외 스크립트에서 사용하는 함수
        public abstract string GetItemType();
        

        public Item(GameObjectList gameObjectList)
        {
            this.gameObjectList = gameObjectList;// 컴포넌트도 일일히 떠먹여줘야 되겠냐!!!!!!!!!!!
            this.subItem = new Dictionary<string, int>();
            this.attackInfos = new List<AttackClassHelper.AttackInfo>();
        }
        public Item(GameObjectList gameObjectList, ItemData data) : this(gameObjectList)
        {
            this.subItem = new Dictionary<string, int>();
            this.attackInfos = new List<AttackClassHelper.AttackInfo>();

            if (isRealItem == true) // 데이터 불러오기 작업을 실시합니다.
            {
                for (int subItemIndex = 0; subItemIndex < data.subItemName.Length; subItemIndex++)
                {
                    this.subItem.Add(data.subItemName[subItemIndex], data.subItemAmount[subItemIndex]);
                }
                for (int atkClassIndex = 0; atkClassIndex < data.attackInfos.Length; atkClassIndex++)
                {
                    this.attackInfos.Add(data.attackInfos[atkClassIndex]);
                }
            }
            else
            {
                // 기본 아이템에서 subItem이 필요하지 않습니다.
                attackInfos.Add(DemoAttackInfo.GetDemoAttackInfoData());
                //this.attackInfos.Add(new GameManager.AttackClass(1.0f, 1.0f, 1.0f, new GameManager.chemical("unknown", 1.0f)));
            }
        }
        [System.Obsolete("사용하지 않는 메소드입니다.")]
        public virtual void GetData(ref ItemData itemData) // 이 함수는 반드시 상속해서 정의해주세요.
        {
            ItemData returnValue;
            returnValue = new ItemData();
            List<string> kList = new List<string>(subItem.Keys);
            List<int> vList = new List<int>(subItem.Values);
            returnValue.subItemName = new string[kList.Count];
            returnValue.subItemName = kList.ToArray();//.CopyTo(kList.ToArray(), 0);
            returnValue.subItemAmount = new int[vList.Count];
            returnValue.subItemAmount = vList.ToArray();//.CopyTo(vList.ToArray(), 0);
#warning 여기 고쳐야 함
            returnValue.attackInfos = this.attackInfos.ToArray();
            //returnValue.attackInfos = new AttackClassHelper.AttackInfo[attackInfos.Count];
            //returnValue.attackInfos = GameManager.AttackClass.toAttackClassDataArray(attackInfos.ToArray());
            itemData = returnValue;
        }
    }
    #region Blank
#warning 올드 클래스 지우기
    //public class Blank : Item
    //{
    //    public Blank(GameObjectList gameObjectList) : base(gameObjectList)
    //    {

    //    }
    //    public Blank(GameObjectList gameObjectList, ItemData data) : base(gameObjectList, data)
    //    {

    //    }

    //    public override float Use(Vector3 vector3)
    //    {
    //        Debug.Log("이 유닛은 Blank 아이템을 사용하고 있습니다.");
    //        return 0.0f;
    //    }
    //    public override float Skill(Vector3 vector3)
    //    {
    //        return 0.0f;
    //    }
    //    public override float Supply(Vector3 vector3)
    //    {
    //        return 0.0f;
    //    }
    //    public override float Update(float deltaTime)
    //    {
    //        return 0.0f;
    //    }

    //    public override string GetItemType()
    //    {
    //        return "Blank";
    //    }

    //    public override void GetData(ref ItemData itemData)
    //    {
    //        base.GetData(ref itemData);
    //        itemData.itemType = "Blank";
    //    }
    //}
    #endregion
    #region Radios
#warning 올드 클래스 지우기
    //public class Radios : Item
    //{
    //    public Radios(GameObjectList gameObjectList) : base(gameObjectList)
    //    {

    //    }
    //    public Radios(GameObjectList gameObjectList, ItemData data) : base(gameObjectList, data)
    //    {

    //    }

    //    public override float Use(Vector3 vector3)
    //    {
    //        Debug.Log("DEBUG_UnitItemPack.Radios: Radio 아이템 사용중입니다.");
    //        return 0.0f;
    //        //UseAct(vector3);
    //    }
    //    public override float Skill(Vector3 vector3)
    //    {
    //        return 0.0f;
    //    }
    //    public override float Supply(Vector3 vector3)
    //    {
    //        return 0.0f;
    //    }
    //    public override float Update(float deltaTime)
    //    {
    //        return 0.0f;
    //    }

    //    public override string GetItemType()
    //    {
    //        return "Radios";
    //    }

    //    public override void GetData(ref ItemData itemData)
    //    {
    //        base.GetData(ref itemData);
    //        itemData.itemType = "Radios";
    //    }
    //}
    #endregion
#warning 올드 클래스 지우기
    #region Pistol
    //public class Pistol : Item
    //{
    //    //int Ammo;
    //    int AmmoLoaded;
    //    GameManager.AttackClass attackType;

    //    public Pistol(GameObjectList gameObjectList) : base(gameObjectList)
    //    {
    //        //this.Ammo = 20;
    //        this.subItem.Add("Ammo", 2000);
    //        //this.Ammo = 2000;
    //        this.AmmoLoaded = 4;
    //        this.attackInfos = new List<AttackClassHelper.AttackInfo>();

    //        this.attackInfos.Add(
    //            new AttackClassHelper.AttackInfo()
    //            {
    //                chemicals = new ChemicalHelper.Chemicals()
    //                {
    //                    self = new ChemicalHelper.Chemical[]
    //                    {
    //                        new ChemicalHelper.Chemical()
    //                        {
    //                            matter = "iron",
    //                            quantity = 10.0f
    //                        }
    //                    }
    //                },
    //                energies = new EnergyHelper.Energies()
    //                {
    //                    self = new EnergyHelper.Energy[]
    //                    {
    //                        new EnergyHelper.Energy()
    //                        {
    //                            type = "kinetic",
    //                            amount = 100.0f
    //                        }
    //                    }
    //                }
    //            }
    //            );
    //    }
    //    public Pistol(GameObjectList gameObjectList, ItemData data) : base(gameObjectList, data)
    //    {
    //        if(data.isRealItem == false)
    //        {
    //            this.subItem.Add("Ammo", 24);
    //            this.AmmoLoaded = 4;
    //            this.attackInfos[0] = new GameManager.AttackClass(10.0f, 30.0f, 10.0f, new GameManager.chemical("iron1", 1.0f), new GameManager.chemical("iron2", 1.0f));
    //        }


    //        this.Supply(new Vector3(0, 0, 0));
    //    }
    //    //#region UnusingFunction
    //    //public Pistol(GameObjectList gameObjectList, int Ammo, int AmmoLoaded, GameManager.AttackClass attackClass) : base(gameObjectList)
    //    //{
    //    //    Debug.Log("(!) Warn_UnitItemPack.Pistol.Pistol(GameObjectList gameObjectList, int Ammo, int AmmoLoaded, GameManager.AttackClass attackClass) : base(gameObjectList), 이 함수는 사용하지 않습니다. 다른 함수를 사용해주세요.");
    //    //    this.subItem["Ammo"] = Ammo;
    //    //    //this.Ammo = Ammo;
    //    //    this.AmmoLoaded = AmmoLoaded;
    //    //    this.attackType = attackClass;
    //    //}
    //    //#endregion

    //    public override float Use(Vector3 vector3)
    //    {
    //        if(AmmoLoaded > 0)
    //        {
    //            gameObjectList.PistolBulletShot(attackInfos[0], vector3);
    //            AmmoLoaded--;
    //            return 0.3f;
    //        }
    //        else
    //        {
    //            return 0.0f;
    //            // 발사 안되는 소리
    //        }
    //        //Debug.Log("파이야!");
    //    }
    //    public override float Skill(Vector3 vector3) // 스킬 없음
    //    {
    //        return 0.0f;
    //    }
    //    public override float Supply(Vector3 vector3)
    //    {
    //        if(subItem.ContainsKey("Ammo") == false)
    //        {
    //            Debug.Log("<!> ERROR_UnitItemPack.Pistol.Supply() : subItem에 Ammo 키가 존재하지 않습니다.");
    //            Debug.Log("*!* FIXING_UnitItemPack.Pistol.Supply() : subItem.Add(\"Ammo\", 0);");
    //            subItem.Add("Ammo", 0);
    //        }

    //        if(subItem["Ammo"] <= 0 && AmmoLoaded <= 0)
    //        {
    //            Debug.Log("남는 탄이 없습니다");
    //            // 팀원들에게 탄약을 요청
    //        }
    //        else if(AmmoLoaded <= 0) // 완성됨
    //        {
    //            Debug.Log("재장전이 필요할것 같습니다.");
    //            if(subItem["Ammo"] < 4)
    //            {
    //                AmmoLoaded = subItem["Ammo"];
    //                subItem["Ammo"] = 0;
    //            }
    //            else
    //            {
    //                AmmoLoaded = 4;
    //                subItem["Ammo"] -= 4;
    //            }
    //        }
    //        return 3.0f;
    //    }
    //    public override float Update(float deltaTime)
    //    {
    //        return 0.0f;
    //    }

    //    public override string GetItemType()
    //    {
    //        return "Pistol";
    //    }

    //    public override void GetData(ref ItemData itemData)
    //    {
    //        subItem["Ammo"] += AmmoLoaded;
    //        base.GetData(ref itemData);
    //        itemData.itemType = "Pistol";
    //    }
    //}
    #endregion
#warning 올드 클래스 지우기
    #region Knife
    //public class Knife : Item
    //{
    //    float AttackCoolDown;


    //    public Knife(GameObjectList gameObjectList) : base(gameObjectList)
    //    {
    //        this.attackInfos = new List<AttackClassHelper.AttackInfo>();
    //        this.attackInfos.Add(DemoAttackInfo.GetDemoAttackInfoData());
    //    }
    //    public Knife(GameObjectList gameObjectList, ItemData data) : base(gameObjectList, data)
    //    {

    //    }

    //    public override float Use(Vector3 vector3)
    //    {
    //        gameObjectList.KnifeBladeStab(this.attackInfos[0], vector3);


    //        // 특정한 방향으로 칼 게임 오브젝트를 띄웁니다
    //        // punch
    //        return 0.8f;
    //    }
    //    public override float Skill(Vector3 vector3) // 손목 잡기
    //    {


    //        // 짧은 시간동안 동안 경계 상태를 유지하며,
    //        // 경계 상태에선 자신을 공격하려는 상대에게 반격할 수 있습니다.
    //        // 스킬을 쓰고 나면


    //        return 2.0f;
    //    }
    //    public override float Supply(Vector3 vector3)
    //    {
    //        return 0.0f;
    //    }
    //    public override float Update(float deltaTime)
    //    {
    //        return 0.0f;
    //    }

    //    public override string GetItemType()
    //    {
    //        return "Knife";
    //    }

    //    public override void GetData(ref ItemData itemData)
    //    {
    //        base.GetData(ref itemData);
    //        itemData.itemType = "Knife";
    //    }
    //}
    #endregion
#warning 올드 클래스 지우기
    #region BuildTool

    //public class BuildTool : Item // 왠만하면 한 유닛에 이 아이템 2개 이상 들게 하지 마세요(머신을 담는 인벤토리 등에 어색함이 남습니다.)
    //{
    //    float UiLifetime; // 기본 0.0f, 활동시 2.0f

    //    //Dictionary<int, int> howMuchHave; // 각 머신클래스 번호와 얼만큼 갖고 있는지에 대한 정보, 키: 머신 ID, 벨류: 얼마나 갖고 있는지
    //    //Dictionary<int, /* 머신 클래스의 class */> // 지금 사용하고 있는 머신ID가 어떤 머신을 가리키는지에 대한 정보 (같은 ID라도 내부 화학물질 등이 다르면 다른걸로 취급) // 있으면 좋을 것이라 판단했음()
    //    int selectedMachineClassID; // 지금 선택한 머신 클래스 유닛
    //    int selectedIndex; //EachClassIndex의 몇번째 인덱스를 가리키고 있는가
    //    int[] EachClassIndex; // 현재 9개의 분류가 대표로 어떤 머신을 선택하고 있는지에 대한 인덱스 번호(머신 ID를 사용합니다. dictionary의  key 값이기도 하고요), howMuchHave 값이 변경될때마다 체크합니다.

    //    // Trigger
    //    bool isFirstSkill;



    //    public BuildTool(GameObjectList gameObjectList) : base(gameObjectList)
    //    {
    //        UiLifetime = 0.0f;

    //        // 강제 아이템 인벤토리 세팅
    //        // 정확한 정보를 전달하기 위해서는 이것만을 위한 클래스가 필요합니다
    //        selectedMachineClassID = 101;
    //        EachClassIndex = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //        selectedIndex = 0;
    //        this.subItem.Add("MachineUnit101", 3);
    //        this.subItem.Add("MachineUnit102", 3);
    //        this.subItem.Add("MachineUnit103", 3);
    //        this.subItem.Add("MachineUnit201", 3);
    //        //
    //        //this.subItem.Add(202, 3);
    //        //this.subItem.Add(204, 3);
    //        this.subItem.Add("MachineUnit301", 3);
    //        //this.subItem.Add(302, 3);
    //        //this.subItem.Add("MachineUnit402", 3);
    //        //this.subItem.Add(501, 3);
    //        this.subItem.Add("MachineUnit502", 3);
    //        //this.subItem.Add(503, 3);
    //        //this.subItem.Add(701, 3);
    //        //this.subItem.Add(702, 3);
    //        //this.subItem.Add(801, 3);
    //        //this.subItem.Add(802, 3);
    //        this.subItem.Add("MachineUnit702", 10);
    //        this.subItem.Add("MachineUnit803", 10);
    //        //this.subItem.Add(901, 3);
    //        //this.subItem.Add(902, 3);
    //        //this.subItem.Add(903, 3);
    //        //this.subItem.Add(904, 3);

    //        isFirstSkill = true;
    //    }
    //    public BuildTool(GameObjectList gameObjectList, ItemData data) : base(gameObjectList, data)
    //    {

    //    }
    //    public override float Use(Vector3 vector3)
    //    {
    //        //SubItem에서 MachineUnit이라는 글자를 빼고, 숫자를 추출하는 작업이 필요.


    //        //Debug.Log("BuildTool.Use()");
    //        if (selectedMachineClassID != 0) // 현재 들고 있는 아이템의 갯수가 0 초과인경우 And 현재 들고 있는 아이템이 공기가 아닌경우
    //        {
    //            string holdingItem = "MachineUnit" + selectedMachineClassID.ToString();
    //            if (subItem[holdingItem] <= 0)
    //            {
    //                Debug.Log("<!> Warning_UnitItemPack.BuildTool.Use() 머신 클래스ID가" + selectedMachineClassID + " 인 머신을 보유하고 있지 않습니다.");
    //                SelectNextUnit(selectedIndex);
    //            }
    //            else
    //            {
    //                bool isBuildWorks = false;
    //                gameObjectList.Build(vector3, selectedMachineClassID, ref isBuildWorks);
    //                if (isBuildWorks)
    //                {
    //                    Debug.Log("머신이 설치되었습니다");
    //                    subItem[holdingItem]--;
    //                    if (subItem[holdingItem] <= 0)
    //                    {
    //                        SelectNextUnit(selectedIndex);
    //                    }
    //                }
    //            }
    //        }

    //        return 0.0f;
    //    }
    //    public override float Skill(Vector3 vector3) // 손목 잡기
    //    {
    //        if (isFirstSkill == true)
    //        {
    //            for (int index = 0; index < 9; index++)
    //            {
    //                SelectNextUnit(index);
    //            }


    //            isFirstSkill = false;
    //        }


    //        //Debug.Log("마우스 위치 값 : " + Input.mousePosition);

    //        // GameObjectList를 통해 UI Controller와 소통합니다.
    //        // 만약 라이프타임이 0 이상 올라가면 UI Controller에게 함수를 호출하도록 요구하게 되며
    //        // 이때 이 함수는 머신 인벤토리 상태를 보여주도록 합니다.
    //        // 만약 빠져나가는 경우에는 (타임아웃, 운동계 신경계 박살, 다른 아이템 선택, 다른 유닛 선택) UI를 감춥니다




    //        if (UiLifetime <= 0.0f)
    //        {
    //            // UI를 생성합니다.
    //            Debug.Log("빌드 표시");



    //        }
    //        else
    //        {
    //            // 방향을 따라 선택한 유닛을 결정합니다
    //            Debug.Log("빌드 선택");

    //            // 마우스 위치를 이용해 어떤 카테고리의 머신 클래스를 선택하는지 번호를 구합니다.
    //            Vector3 mouseDirection = (Input.mousePosition - new Vector3((Screen.width / 2), (Screen.height / 2), 0)).normalized;
    //            int buildClassIndex = 0;
    //            // vector3를 이용해 각도를 구합니다.
    //            if(mouseDirection.x > 0.0f)
    //            {
    //                buildClassIndex = -8; // 8,7,6,5,4
    //            }
    //            if (mouseDirection.y > Mathf.Cos(40 * Mathf.Deg2Rad))
    //            {
    //                buildClassIndex += 0;
    //            }
    //            else if (mouseDirection.y > Mathf.Cos(80 * Mathf.Deg2Rad))
    //            {
    //                buildClassIndex += 1;
    //            }
    //            else if (mouseDirection.y > Mathf.Cos(120 * Mathf.Deg2Rad))
    //            {
    //                buildClassIndex += 2;
    //            }
    //            else if (mouseDirection.y > Mathf.Cos(160 * Mathf.Deg2Rad))
    //            {
    //                buildClassIndex += 3;
    //            }
    //            else
    //            {
    //                buildClassIndex += 4;
    //            }

    //            if (buildClassIndex < 0) buildClassIndex *= -1;
    //            Debug.Log("얻어진 빌드 클래스 인덱스 : " + buildClassIndex);
    //            if (buildClassIndex == selectedIndex)
    //            {
    //                SelectNextUnit(buildClassIndex);
    //                #region 주석
    //                /*
    //                // 이 부류의 다음 머신을 선택합니다.
    //                // 인벤토리의 모든 키 값을 가져옴
    //                // 키 값중에 selectedIndex 라인에 들어갈 수 있는 키값들로 솎아냄

    //                // 현재 selectedMachineClassID값보다 큰 아이디 값을 찾는다.

    //                // 못 찾았으면 selectedIndex 라인에 들어갈 수 있는 키값중 가장 첫번째 값을 찾는다.
    //                // (만약 찾은 키 값에 BuildMachineInventory[키] 값이 0보다 크지 않으면 다시 찾는다)
    //                // 못 찾았으면(이번 라인에 존재하는 머신이 없는경우) 유닛은 공기를 들게 하여 건축을 못하도록 막아둔다.
    //                int[] BuildMachineInventoryKeyList = new int[BuildMachineInventory.Count];
    //                BuildMachineInventory.Keys.CopyTo(BuildMachineInventoryKeyList, 0);
    //                int FirstOfThisLine = -1; // -1은 가짜로 넣어둔 값, 만약 -1이 들어간다면 다른곳에서 오류를 일으킬 것입니다.
    //                bool isFoundFirst = false;
    //                bool isFoundNext = false;
    //                for (int index = 0; index < BuildMachineInventory.Keys.Count; index++)
    //                {
    //                    int oneOfKeys = BuildMachineInventoryKeyList[index];
    //                    if(oneOfKeys >= (buildClassIndex + 1) * 100 &&
    //                        oneOfKeys < (buildClassIndex + 2) * 100 &&
    //                        BuildMachineInventory[oneOfKeys] > 0) // 범위 내의 키값인지 체크 && 값이 있는지도 체크합니다
    //                    {
    //                        if(isFoundFirst == false) // 첫번째로 해당하는 값이 있으면 일단 주머니에 넣어둡니다.
    //                        {
    //                            FirstOfThisLine = oneOfKeys;
    //                            isFoundFirst = true;
    //                        }
    //                        if(oneOfKeys > selectedMachineClassID) // 진짜로 찾던 값을 찾았습니다
    //                        {
    //                            selectedMachineClassID = oneOfKeys; // 다음에 해당하는 값을 찾았습니다.
    //                            isFoundNext = true;
    //                            break;
    //                        }
    //                    }
    //                }

    //                // isFoundNext == true && isFoundFirst == true인 경우는 For문 안에 있습니다 (false true인 경우는 존재하지 않음)
    //                if (isFoundNext == false && isFoundFirst == true) // 현재 선택한 아이디 다음에 해당하는 값이 없다면 주머니에 넣어둔 아이디값을 저장합니다
    //                {
    //                    selectedMachineClassID = FirstOfThisLine;
    //                }
    //                else if (isFoundNext == false && isFoundFirst == false)
    //                {
    //                    selectedMachineClassID = 0;
    //                }
    //                EachClassIndex[buildClassIndex] = selectedMachineClassID; /**/
    //                #endregion
    //            }
    //            else
    //            {
    //                selectedMachineClassID = EachClassIndex[buildClassIndex];
    //                selectedIndex = buildClassIndex;
    //            }

    //            Debug.Log("선택한 머신 클래스 아이디 : " + selectedMachineClassID);
    //        }


    //        UiLifetime = 2.0f;

    //        // 유닛 선택 창을 띄우게 됩니다.

    //        // 설치할 머신 클래스 유닛 선택
    //        // 스킬을 사용하면, UI 화면처럼 3X3 격자가 나옵니다 + 오른쪽 상단에 닫기 표시 있음 - 윈도우형
    //        // 스킬을 쓸 때마다 케이크형 UI를 보여줍니다. UI가 보이지 않을때는 UI만 보이게 하지만, UI가 보이는 상태에서 스킬을 쓰면 머신을 선택했다고 간주합니다. 각도에 따라서 결과값이 달라집니다. UI는 소환된지 2초 후 반응이 없으면 사라집니다, 스킬을 발동할때마다 UI 라이프타임이 2초로 설정됩니다. - 간편형
    //        // 
    //        // 선택하지 않은 격자를 클릭하면 그 클래스의 대표 머신을 선택하게 됩니다. - 공통
    //        // 선택한 격자를 다시 클릭하면 그 격자의 다음 머신을 선택하게 됩니다(대표가 바뀜). - 공통



    //        gameObjectList.ShowSubItem(this);
    //        return 0.0f;
    //    }
    //    public override float Supply(Vector3 vector3)
    //    {


    //        gameObjectList.TryTouchMachine(vector3);

    //        // 설치한 게임오브젝트에 수리를 진행하거나, MachineNet에 개입하거나 기타 설정들을 손봅니다.
    //        // OpenMachine



    //        return 0.0f;
    //    }
    //    public override float Update(float deltaTime)
    //    {
    //        if (UiLifetime > 0.0f)
    //        {
    //            UiLifetime -= deltaTime;
    //            if(UiLifetime <= 0.0f)
    //            {
    //                gameObjectList.HideSubItem();
    //            }
    //        }




    //        return 0.0f;
    //    }

    //    public override string GetItemType()
    //    {
    //        return "BuildTool";
    //    }

    //    // 외부 함수
    //    public override void GetData(ref ItemData itemData)
    //    {
    //        itemData.itemType = "BuildTool";
    //        base.GetData(ref itemData);

    //    }


    //    // 클래스 내부 전용 함수
    //    private void SelectNextUnit(int buildClassIndex)
    //    {
    //        // 이 부류의 다음 머신을 선택합니다.
    //        // 인벤토리의 모든 키 값을 가져옴
    //        // 키 값중에 selectedIndex 라인에 들어갈 수 있는 키값들로 솎아냄

    //        // 현재 selectedMachineClassID값보다 큰 아이디 값을 찾는다.

    //        // 못 찾았으면 selectedIndex 라인에 들어갈 수 있는 키값중 가장 첫번째 값을 찾는다.
    //        // (만약 찾은 키 값에 BuildMachineInventory[키] 값이 0보다 크지 않으면 다시 찾는다)
    //        // 못 찾았으면(이번 라인에 존재하는 머신이 없는경우) 유닛은 공기를 들게 하여 건축을 못하도록 막아둔다.

    //        List<string> subItemKeys = new List<string>(subItem.Keys); // subItem의 키값들을 전부 저장할 값을 준비합니다.
    //        List<int> RealMachineClassIdList = new List<int>(); // subItem의 키값중 여기에 머신 클래스 유닛을 가리키는 키값을 저장할겁니다.
    //        // 머신 클래스 유닛들을 가리키는 키값을 솎아냅니다.
    //        for(int keyIndex = 0; keyIndex < subItemKeys.Count; keyIndex++)
    //        {
    //            if (subItemKeys[keyIndex].StartsWith("MachineUnit"))
    //            {
    //                int a;
    //                if(int.TryParse(subItemKeys[keyIndex].Remove(0,11), out a))
    //                {
    //                    RealMachineClassIdList.Add(a);
    //                }
    //            }
    //        }

    //        int[] BuildMachineInventoryKeyList = new int[RealMachineClassIdList.Count];
    //        BuildMachineInventoryKeyList = RealMachineClassIdList.ToArray();





    //        //BuildMachineInventory.Keys.CopyTo(BuildMachineInventoryKeyList, 0);
    //        int FirstOfThisLine = -1; // -1은 가짜로 넣어둔 값, 만약 -1이 들어간다면 다른곳에서 오류를 일으킬 것입니다.
    //        bool isFoundFirst = false;
    //        bool isFoundNext = false;
    //        for (int index = 0; index < BuildMachineInventoryKeyList.Length; index++) // 
    //        {
    //            int oneOfKeys = BuildMachineInventoryKeyList[index];
    //            if (oneOfKeys >= (buildClassIndex + 1) * 100 && // 자기 부류의 아이템이고
    //                oneOfKeys < (buildClassIndex + 2) * 100 && // 다음 부류의 아이템은 예외로 합니다.
    //                subItem["MachineUnit" + oneOfKeys.ToString()] > 0) // 범위 내의 키값인지 체크 && 값이 있는지도 체크합니다
    //            {
    //                if (isFoundFirst == false) // 첫번째로 해당하는 값이 있으면 일단 주머니에 넣어둡니다.
    //                {
    //                    FirstOfThisLine = oneOfKeys;
    //                    isFoundFirst = true;
    //                }
    //                if (oneOfKeys > selectedMachineClassID) // 진짜로 찾던 값을 찾았습니다
    //                {
    //                    selectedMachineClassID = oneOfKeys; // 다음에 해당하는 값을 찾았습니다.
    //                    isFoundNext = true;
    //                    break;
    //                }
    //            }
    //        }

    //        // isFoundNext == true && isFoundFirst == true인 경우는 For문 안에 있습니다 (false true인 경우는 존재하지 않음)
    //        if (isFoundNext == false && isFoundFirst == true) // 현재 선택한 아이디 다음에 해당하는 값이 없다면 주머니에 넣어둔 아이디값을 저장합니다
    //        {
    //            selectedMachineClassID = FirstOfThisLine;
    //        }
    //        else if (isFoundNext == false && isFoundFirst == false)
    //        {
    //            selectedMachineClassID = 0;
    //        }
    //        EachClassIndex[buildClassIndex] = selectedMachineClassID;

    //    }

    //    // 필드 가져오는 함수
    //    public int GetSelectedIndex()
    //    {
    //        return selectedIndex;
    //    }
    //    public bool IsEachClassIndexNull()
    //    {
    //        return EachClassIndex == null;
    //    }
    //    public int GetEachClassIndexElement(int _index)
    //    {
    //        return EachClassIndex[_index];
    //    }

    //}
    #endregion
#warning 올드 클래스 지우기
    #region Cannon - Machine
    /*
    public class Turret : Item
    {
        public Turret(GameObjectList gameObjectList) : base(gameObjectList)
        {
            Debug.Log("터릿 기본 생성자 1");
            this.subItem.Add("Ammo", 2000);
            this.attackInfos = new List<GameManager.AttackClass>();
            this.attackInfos.Add(new GameManager.AttackClass(10.0f, 30.0f, 10.0f, new GameManager.chemical("iron1", 1.0f), new GameManager.chemical("iron2", 1.0f)));
        }
        public Turret(GameObjectList gameObjectList, ItemData data) : base(gameObjectList, data)
        {
            Debug.Log("터릿 기본 생성자 2");

            if (data.isRealItem == false)
            {
                this.subItem.Add("Ammo", 2000);
                this.attackInfos[0] = new GameManager.AttackClass(10.0f, 30.0f, 10.0f, new GameManager.chemical("iron1", 1.0f), new GameManager.chemical("iron2", 1.0f));
            }
        }

        public override float Use(Vector3 vector3)
        {
            Debug.Log("Turret 아이템이 사용됨");

            if (this.subItem["Ammo"] > 0)
            {
                gameObjectList.PistolBulletShot(attackInfos[0], vector3);
                this.subItem["Ammo"]--;
                return 0.3f;
            }
            else
            {
                Debug.Log("Turret 아이템이 사용됨");

                return 0.0f;
                // 발사 안되는 소리
            }
        }
        public override float Skill(Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(float deltaTime)
        {
            return 0.0f;
        }

        public override string GetItemType()
        {
            return "Turret";
        }

        public override void GetData(ref ItemData itemData)
        {
            itemData.itemType = "Turret";
            base.GetData(ref itemData);

        }
    }
    //*/
    #endregion

    #endregion

    #region Methods
    // public methods
    public void ItemUse(Vector3 vector3)
    {
        
        if (Cooldown > 0.0f) return;
        Debug.Log("아이템 사용됨");

        if (inventory == null) return;

        // 현재 인벤토리의 아이템 번호 인덱스에 접근해서 use 함수를 호출합니다.
        Cooldown = inventory[inventoryIndex].Use(gameObject, vector3);

    }
    public void ItemSkillE(Vector3 vector3)
    {
        if (Cooldown > 0.0f) return;
        Cooldown = inventory[inventoryIndex].ESkill(gameObject, vector3);
    }
    public void ItemSkillF(Vector3 vector3)
    {
        if (Cooldown > 0.0f) return;
        Cooldown = inventory[inventoryIndex].FSkill(gameObject, vector3);
    }
    public void ItemSupply(Vector3 vector3)
    {
        if (Cooldown > 0.0f) return;
        Cooldown = inventory[inventoryIndex].Supply(gameObject, vector3);
    }
    public void ItemSelect(int i)
    {
        if (Cooldown > 0.0f) return;
        inventoryIndex = i;
        Cooldown = 0.5f;
    }
    #region interface methods
    public Inventory GetComponentData()
    {
        return inventory;
    }
    public void SetComponentData(Inventory data)
    {
        inventory = data;
    }
    #endregion


    #region InventorySet

    [System.Obsolete("이 함수는 이전되었습니다. SetComponentData(Inventory)를 사용하세요")]
    public void InventorySet()
    {
        inventory[0] = new ItemHelper.Pistol();
        inventory[1] = new ItemHelper.Knife();
        inventory[2] = new ItemHelper.BuildTool();
    }
    [System.Obsolete("이 함수는 이전되었습니다. SetComponentData(Inventory)를 사용하세요")]
    public void InventorySet(ItemHelper.Item item1, ItemHelper.Item item2, ItemHelper.Item item3)
    {
        inventory[0] = item1;
        inventory[1] = item2;
        inventory[2] = item3;
    }

    public void InventorySetForMachine(ItemHelper.Item item)
    {
        inventory = new Inventory(1);

        inventory[0] = item;
    }
    #endregion

    [System.Obsolete("이 함수는 이전되었습니다. SetComponentData(Inventory)를 사용하세요")]
    public void InventorySet(UnitItemPackData data)
    {
        ////함수 설명: 이 컴포넌트의 데이터 클래스를 매개변수로 받아 이 컴포넌트의 데이터에 정보를 저장합니다, data의 ItemType을 받아 타입에 맞는 생성자를 호출합니다.

        //GameObjectList gameObjectList = GetComponent<GameObjectList>();

        //if (inventory == null) inventory = new Item[data.inventory.Length];

        //for(int i = 0; i < data.inventory.Length; i++)
        //{
        //    switch (data.inventory[i].itemType)
        //    {
        //        case "Radios":
        //            if(debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Radio 들어감."); }
        //            inventory[i] = new Radios(gameObjectList, data.inventory[i]);
        //            break;
        //        case "Pistol":
        //            inventory[i] = new Pistol(gameObjectList, data.inventory[i]);
        //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Pistol 들어감."); }
        //            break;
        //        case "Knife":
        //            inventory[i] = new Knife(gameObjectList, data.inventory[i]);
        //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Knife 들어감."); }
        //            break;
        //        case "BuildTool":
        //            inventory[i] = new BuildTool(gameObjectList, data.inventory[i]);
        //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("BuildTool 들어감."); }
        //            break;
        //        case "Turret":
        //            inventory[i] = new Turret(gameObjectList, data.inventory[i]);
        //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Turret 들어감."); }
        //            break;
        //        case "Blank": // 만약 아무것도 없는 빈 공간이 필요하다면 "Blank" 을 입력하시면 됩니다.
        //        default:
        //            inventory[i] = new Blank(gameObjectList);
        //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Blank 들어감."); }
        //            break;
        //    }
        //}
    }
    public void InventorySet(int index, ItemHelper.Item item)
    {
        if (index >= 0 && index < inventory.Length)
        {
            inventory[index] = item;
            return;
        }
        Debug.Log("<!> ERROR_UnitItemPack.InventorySet : UnitItemPack.InventorySet에 전달된 인덱스가 옳지 않습니다.");
    }

    public void InventoryIndexSet(int value)
    {
        if(value >= 0 && value < inventory.Length)
        {
            inventory.Select(value);
            return;
        }
        Debug.Log("<!> ERROR_UnitItemPack.InventoryIndexSet : UnitItemPack.InventoryIndexSet에 전달된 매개변수가 옳지 않습니다.");
    }
    #endregion

    #region UnitItemPackData 클래스
    // 유닛 프리펩을 인스턴스화할때 데이터를 넣기 위한 장치입니다.
    [System.Serializable]
    public class UnitItemPackData
    {


        public ItemData[] inventory;

        // 생성자
        public UnitItemPackData(params string[] itemTypes)
        {
            inventory = new ItemData[itemTypes.Length];

            for (int index = 0; itemTypes.Length > index; index++)
            {
                inventory[index] = new ItemData();
                inventory[index].itemType = itemTypes[index];
                inventory[index].isRealItem = false;
            }
        }
    }

    public UnitItemPackData MakeFakeItemPackData(params string[] items)
    {
        UnitItemPackData returnValue = new UnitItemPackData();
        returnValue.inventory = new ItemData[items.Length];


        for (int index = 0; items.Length > index; index++)
        {
            returnValue.inventory[index].itemType = items[index];
            returnValue.inventory[index].isRealItem = false;
        }
        return returnValue;
    }

    public static UnitItemPackData NewUnitItemPackData(ItemData[] itemData)
    {
        UnitItemPackData returnValue = new UnitItemPackData();
        returnValue.inventory = new ItemData[itemData.Length];
        returnValue.inventory = itemData;
        return returnValue;
    }

    #endregion

 
}
