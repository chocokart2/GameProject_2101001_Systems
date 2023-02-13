using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning 아이템 클래스는 ItemHelper로 이동했습니다
/// <summary>
///     유닛의 인벤토리에 대해 정의합니다.
/// </summary>
/// <remarks>
///     데이터 : Inventory
/// </remarks>
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
    [System.Obsolete("사용하지 않는 클래스입니다.", true)]
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
    [System.Obsolete("사용되지 않는 존재입니다.", error: true)]
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
    [System.Obsolete("사용되지 않는 존재입니다.", error: true)]
    public static ItemData NewItemData(string itemType)
    {
        ItemData returnValue = NewItemData();
        returnValue.itemType = itemType;
        return returnValue;
    }
    #endregion

    [System.Obsolete]
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

    #endregion

    #region Methods
    // public methods
    public void ItemUse(Vector3 vector3)
    {
        
        if (Cooldown > 0.0f) return;
        Hack.Say(Hack.isDebugUnitItemPack, Hack.check.info, this, message: "아이템 사용됨");

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

    //[System.Obsolete("이 함수는 이전되었습니다. SetComponentData(Inventory)를 사용하세요")]
    //public void InventorySet(UnitItemPackData data)
    //{
    //    ////함수 설명: 이 컴포넌트의 데이터 클래스를 매개변수로 받아 이 컴포넌트의 데이터에 정보를 저장합니다, data의 ItemType을 받아 타입에 맞는 생성자를 호출합니다.

    //    //GameObjectList gameObjectList = GetComponent<GameObjectList>();

    //    //if (inventory == null) inventory = new Item[data.inventory.Length];

    //    //for(int i = 0; i < data.inventory.Length; i++)
    //    //{
    //    //    switch (data.inventory[i].itemType)
    //    //    {
    //    //        case "Radios":
    //    //            if(debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Radio 들어감."); }
    //    //            inventory[i] = new Radios(gameObjectList, data.inventory[i]);
    //    //            break;
    //    //        case "Pistol":
    //    //            inventory[i] = new Pistol(gameObjectList, data.inventory[i]);
    //    //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Pistol 들어감."); }
    //    //            break;
    //    //        case "Knife":
    //    //            inventory[i] = new Knife(gameObjectList, data.inventory[i]);
    //    //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Knife 들어감."); }
    //    //            break;
    //    //        case "BuildTool":
    //    //            inventory[i] = new BuildTool(gameObjectList, data.inventory[i]);
    //    //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("BuildTool 들어감."); }
    //    //            break;
    //    //        case "Turret":
    //    //            inventory[i] = new Turret(gameObjectList, data.inventory[i]);
    //    //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Turret 들어감."); }
    //    //            break;
    //    //        case "Blank": // 만약 아무것도 없는 빈 공간이 필요하다면 "Blank" 을 입력하시면 됩니다.
    //    //        default:
    //    //            inventory[i] = new Blank(gameObjectList);
    //    //            if (debugSetting.DEBUG_InventorySetFuncSayItemType) { Debug.Log("Blank 들어감."); }
    //    //            break;
    //    //    }
    //    //}
    //}
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
    //// 유닛 프리펩을 인스턴스화할때 데이터를 넣기 위한 장치입니다.
    //[System.Serializable]
    //public class UnitItemPackData
    //{


    //    public ItemData[] inventory;

    //    // 생성자
    //    public UnitItemPackData(params string[] itemTypes)
    //    {
    //        inventory = new ItemData[itemTypes.Length];

    //        for (int index = 0; itemTypes.Length > index; index++)
    //        {
    //            inventory[index] = new ItemData();
    //            inventory[index].itemType = itemTypes[index];
    //            inventory[index].isRealItem = false;
    //        }
    //    }
    //}

    //public UnitItemPackData MakeFakeItemPackData(params string[] items)
    //{
    //    UnitItemPackData returnValue = new UnitItemPackData();
    //    returnValue.inventory = new ItemData[items.Length];


    //    for (int index = 0; items.Length > index; index++)
    //    {
    //        returnValue.inventory[index].itemType = items[index];
    //        returnValue.inventory[index].isRealItem = false;
    //    }
    //    return returnValue;
    //}

    //public static UnitItemPackData NewUnitItemPackData(ItemData[] itemData)
    //{
    //    UnitItemPackData returnValue = new UnitItemPackData();
    //    returnValue.inventory = new ItemData[itemData.Length];
    //    returnValue.inventory = itemData;
    //    return returnValue;
    //}

    #endregion

 
}
