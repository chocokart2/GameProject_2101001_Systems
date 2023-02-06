using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 아이템의 부모 클래스를 가지고 있습니다.
/// </summary>
public class ItemHelper : BaseComponent
{
    #region Interface
    public interface IItem 
    {

    }
#warning ITool의 메서드에 전부 GameObject 넣기.
    public interface ITool : IItem
    {
        /// <szummary>
        ///     유닛이 이 아이템을 사용할때를 정의합니다. [좌클릭]
        /// </summary>
        /// <param name="direction">클릭한 방향입니다.</param>
        /// <returns>행동 이후의 쿨다운입니다.</returns>
        public float Use(GameObject user, Vector3 direction);
        public float ESkill(GameObject user, Vector3 vector3); // 아이템 스킬을 사용할 시 호출됩니다.
        public float FSkill(GameObject user, Vector3 vector3);
        public float Supply(GameObject user, Vector3 vector3); // 아이템 보충용 스킬을 사용할 시 호출됩니다.
        public float Update(GameObject user, float deltaTime); // 매 프레임마다 호출되는 함수입니다.

    }
    public interface IName
    {
        string TypeName { get; }
    }
    #endregion
    #region Class
    // 아이템 클래스들에 대해 정의한 내용입니다.
    #region ItemBase : Item, SubItem, SubItemArray, Inventory
    /// <summary>
    ///     이 클래스의 파생형은 각자 아이템의 원본입니다.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         서브 아이템: 이 아이템을 구성하는 아이템을 나타냅니다.
    ///     </para>
    ///     <para>
    ///         대부분 서브아이템의 0번째 인덱스를 사용합니다.
    ///     </para>
    /// 
    /// </remarks>
    [System.Serializable]
    public abstract class Item : ITool
    {
#warning 개선안 : 서브 아이템 목록에서는 번호 인덱스로 접근하는데, 딕셔너리를 이용해 이름으로 바로 접근하는 방법을 추가한다. 딕셔너리 : 이름 -> 인덱스
        [System.NonSerialized] public static GameObjectList gameObjectList;
        public string Name;
        /// <summary>
        ///     아이템의 설정을 나타냅니다
        /// </summary>
        public SubItemArray subItems;

        public virtual string ItemType { get => "UNDEFINED_TYPE"; }

        public abstract float Use(GameObject user, Vector3 vector3); // 아이템을 사용할 시 호출됩니다.
        public abstract float ESkill(GameObject user, Vector3 vector3); // 아이템 스킬을 사용할 시 호출됩니다.
        public abstract float FSkill(GameObject user, Vector3 vector3);
        public abstract float Supply(GameObject user, Vector3 vector3); // 아이템 보충용 스킬을 사용할 시 호출됩니다.
        /// <summary>
        ///     매 프레임마다 호출되는 함수입니다.
        /// </summary>
        /// <remarks>
        ///     과도한 내용의 구현은 퍼포먼스 드랍을 초래합니다.
        /// </remarks>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public abstract float Update(GameObject user, float deltaTime); // 매 프레임마다 호출되는 함수입니다.
    }
    /// <summary>
    /// 아이템 내부의 아이템 하나를 정의합니다.
    /// </summary>
    /// 아이템 내부에 들어가는 물품들을 정의할 수 있습니다.
    /// 좋은 예시로, 권총에 들어가는 탄알이 이에 해당합니다. 권총은 공격을 할때 탄알을 소모하기 때문입니다.
    /// 
    [System.Serializable]
    public class SubItem : INameKey
    {
        public string name;
        public int amount;
        public AttackClassHelper.AttackInfo attackInfo; // 이 아이템이 공격에 해당하는경우입니다.

        public string Name { get; set; }
    }
    /// <summary>
    ///     아이템 내부의 아이템들을 정의합니다.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         만약 단일 아이템에 접근하고 싶다면 SubItem을 참고하십시오.
    ///     </para>
    /// </remarks>
    [System.Serializable]
    public class SubItemArray : IArray<SubItem>
    {
        public SubItem[] self;
#warning 이거 로딩했을때 키벨류 초기화해주는 함수 만들어야겠는데.
        [System.NonSerialized] public Dictionary<string, int> nameIndexPairs;

        public int Length
        {
            get => self.Length;
        }
        /// <summary>
        ///     이름으로 접근하는 인덱서입니다.
        /// </summary>
        /// <param name="name">
        ///     접근하기 위한 이름입니다.
        /// </param>
        /// <returns>
        ///     <para>
        ///         접근 값과 성공 여부를 묶은 튜플 형식을 리턴합니다.
        ///     </para>
        ///     <para>
        ///         result : 찾으려는 객체입니다.
        ///     </para>
        ///     <para>
        ///         isAccessed : 접근 성공했는지를 나타내는 불리언 값입니다.
        ///     </para>
        /// </returns>
        public (SubItem result, bool isAccessed) this[string name]
        {
            get
            {
                if (self[nameIndexPairs[name]].name != name)
                {
                    UpdateIndex();
                    if (self[nameIndexPairs[name]].name != name)
                    {
                        Debug.LogWarning("<!>ERROR_SubItemArray.this[string] : 존재하지 않는 이름에 대해 접근하였습니다.");
                        return (new SubItem(), false);
                    }
                }
                return (self[nameIndexPairs[name]], true);
            }
        }
        public SubItem this[int index]
        {
            get => self[index];
            set => self[index] = value;
        }

        public SubItemArray() { }
        public SubItemArray(params SubItem[] subItems) { self = subItems; }

#warning lv 1 func 이 함수 테스트 안함.
        /// <summary>
        ///     딕셔너리의 값을 업데이트 합니다. 이름 -> 그 이름을 가진 원소들중 가장 첫번째의 원소의 인덱스
        /// </summary>
        /// <remarks>
        ///     생각보다 가벼운 함수입니다.
        /// </remarks>
        public void UpdateIndex()
        {
            // 딕셔너리에 그 키 값이 없음, 추가함
            // 딕셔너리에 그 키 값이 있음, 무시함.

            for(int index = 0; index < self.Length; index++)
            {
                if (nameIndexPairs.ContainsKey(self[index].name)) continue;
                nameIndexPairs.Add(self[index].name, index);
            }
        }
    }
    /// <summary>
    /// 아이템을 보관할 수 있는 존재입니다. 또한 액션을 취할 때, 내재된 인덱스를 통해 선택한 아이템으로 바로 참조할 수 있게 해줍니다.
    /// </summary>
    /// 아무래도 이 존재는 인간 뿐만 아니라,
    /// 아이템을 사용하기를 바라는 존재, 즉 로봇도 이것을 사용할 수 있을 것이다.
    [System.Serializable]
    public class Inventory : IArray<Item>, ITool
    {
        //  field
        public GameObject gameObject;
        public Item[] self; // 이 클래스의 핵심 클래스입니다.

        public Item this[int index]
        {
            get => self[index];
            set => self[index] = value;
        }
        public int Length
        {
            get => self.Length;
        }

        private int selectIndex = 0;
        //  constructor
        public Inventory() { }
        public Inventory(int inventorySize) { self = new Item[inventorySize]; }

        //  method
        public void Select(int index) => selectIndex = index;
        public float Use(GameObject user, Vector3 direction) => self[selectIndex].Use(user, direction);
        public float ESkill(GameObject user, Vector3 direction) => self[selectIndex].ESkill(user, direction);
        public float FSkill(GameObject user, Vector3 direction) => self[selectIndex].FSkill(user, direction);
        public float Supply(GameObject user, Vector3 direction) => self[selectIndex].Supply(user, direction);
        public float Update(GameObject user, float deltaTime)
        {
            for(int index = 0; index < (self?.Length ?? 0); ++index)
            {
                self[index].Update(user, deltaTime);
            }
            return 0.0f;
        }

    }
    #endregion
    #region ItemList
    #region item - Nothing
    [System.Serializable]
    public class Blank : Item
    {
        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
    }
    #endregion

    #region item - watcher
    #region item - watcher - radio
    public class Radio : Item
    {
        public override string ItemType { get => "Radio"; }

        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
            Debug.Log("DEBUG_UnitItemPack.Radios: Radio 아이템 사용중입니다.");
            return 0.0f;
        }
    }
    #endregion
    #region item - watcher - eyebeacon
    public class EyeBeacon : Item
    {
        public override string ItemType { get => "EyeBeacon"; }

        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
    }
    #endregion
    #region item - watcher - smoke

    #endregion
    #endregion
    #region item - weapon
    #region item - weapon - pistol
    public class Pistol : Item
    {
        public override string ItemType { get => "Pistol"; }

        protected int MaxAmmo;

        public Pistol() { }
        public Pistol(int Max)
        {

        }

        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
            if (subItems[0].amount > 0)
            {
                if (subItems[0].attackInfo == null) Debug.Log("DEBUG_Pistol.Use(GameObject,Vector3) : subItem이 null 입니다.");
                GameObjectList.PistolBulletShot(user, subItems[0].attackInfo, vector3);
                subItems[0].amount--;
                // 발사 소리
                return 0.3f;
            }
            else
            {
                // 발사 안 되는 소리
                return 0.0f;
            }
        }
    }
    #endregion
    #region item - weapon - knife
    public class Knife : Item
    {
        public override string ItemType { get => "Knife"; }

        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
#warning 디버깅
            Debug.Log($"DEBUG_ItemHelper.Use(Vector3) : subItems은 Null 값인가요? {subItems[0].attackInfo == null}");
#warning GameObjectList의 요구하는 함수가 스테틱이 아니거나, 퍼블릭이 아닙니다.
            GameObjectList.KnifeBladeStab(user, subItems[0].attackInfo, vector3);
            //gameObjectList.KnifeBladeStab(subItems[0].attackInfo, vector3);

            return 0.8f;
        }
    }
    #endregion
    #region item - weapon - cannon
    public class Turret : Item
    {
        public override string ItemType { get => "Turret"; }

        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
            Debug.Log("Turret 아이템이 사용됨");
            if (subItems["ammo"].isAccessed == false)
            {
                // 아이템이 없습니다.
                return 0.0f;
            }

            if (subItems["ammo"].result.amount > 0)
            {
                GameObjectList.PistolBulletShot(user, subItems["ammo"].result.attackInfo, vector3);
                this.subItems["ammo"].result.amount--;
                return 0.3f;
            }
            else
            {
                Debug.Log("Turret 아이템이 사용됨");

                return 0.0f;
                // 발사 안되는 소리
            }
        }
    }
    #endregion
    #endregion
    #region item - build
    #region item - build - buildtool
    public class BuildTool : Item
    {
        public override string ItemType { get => "BuildTool"; }

        /// <summary>
        ///     유효한 머신의 값들의 배열입니다. 작은 숫자부터 큰 숫자 순으로 정렬되어 있습니다.
        /// </summary>
        public readonly int[][] machineIdArray = new int[][]
        {
            new int[] { 101, 102, 103 },
            new int[] { 201 },
            new int[] { 301 },
            new int[] { 0 },
            new int[] { 501 },
            new int[] { 0 },
            new int[] { 702 },
            new int[] { 803 },
            new int[] { 0 }
        };

#warning 엑세스용 퍼블릭 프로퍼티랑 프라이빗 필드 이름이 일치하지 않습니다.
        public int CursorIndex { get => selectedIndex; }
        public int CursorMachineID { get => selectedMachineClassID; }
        public int[] EachClassMachineID { get => EachClassIndex; }
#warning 용도를 모르는 변수
        bool isFirstSkill = true;
        float UiLifeTime = 0.0f;
        /// <summary>
        /// 지금 선택한 머신 클래스 유닛의 종류 값입니다.
        /// </summary>
        int selectedMachineClassID = 101;
        /// <summary>
        ///     EachClassIndex를가리키는 커서입니다.
        /// </summary>
        int selectedIndex = 0; //EachClassIndex의 몇번째 인덱스를 가리키고 있는가
        int[] EachClassIndex = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };



        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
            if (selectedMachineClassID != 0) // 현재 들고 있는 아이템의 갯수가 0 초과인경우 And 현재 들고 있는 아이템이 공기가 아닌경우
            {
                string holdingItem = "MachineUnit" + selectedMachineClassID.ToString();
                if (subItems[holdingItem].result.amount <= 0)
                {
                    Debug.Log("<!> Warning_UnitItemPack.BuildTool.Use() 머신 클래스ID가" + selectedMachineClassID + " 인 머신을 보유하고 있지 않습니다.");
                    SelectNextUnit(selectedIndex);
                }
                else
                {
                    bool isBuildWorks = false;
                    gameObjectList.Build(vector3, selectedMachineClassID, ref isBuildWorks);
                    if (isBuildWorks)
                    {
                        Debug.Log("머신이 설치되었습니다");
                        subItems[holdingItem].result.amount--;
                        if (subItems[holdingItem].result.amount <= 0)
                        {
                            SelectNextUnit(selectedIndex);
                        }
                    }
                }
            }
            return 0.0f;
        }

#warning lv 1 테스트 없음
        /// <summary>
        ///     같은 종류의 머신의 다음 머신을 선택하도록 합니다.
        /// </summary>
        /// <param name="buildClassIndex">썸네일 번호의 EachClassIndex의 인덱스 번호입니다.</param>
        private void SelectNextUnit(int buildClassIndex)
        {
            // machineIdArray[buildClassIndex][]에 해당하는 배열의 원소 값이면서
            // selectedMachineClassID보다 큰 값중 가장 작은 값을 선택한다
            // 값을 찾을 수 없다면, 배열에서 가장 작은 값을 리턴하거나,
            // 그마저 유효하지 않다면 0을 리턴한다.
            // machineIdArray

            int result = selectedMachineClassID;
            int prev = -1;
            
            subItems.UpdateIndex(); // 일단 정확히 접근할 수 있도록 합니다.
            for(int index = 0; index < machineIdArray[buildClassIndex].Length; index++)
            {
                // 가장 작은 값을 구한 상태이고, 가장 작은 값 < 지금 값 < selectedMachineClassID값이면 무시합니다.
                if (prev != -1 && machineIdArray[buildClassIndex][index] < selectedMachineClassID) continue;

                string searchingKey = $"MachineUnit{machineIdArray[buildClassIndex][index]}";
                if (subItems.nameIndexPairs.ContainsKey(searchingKey))
                {
                    // 키 값이 존재하는 아이디 값을 찾았습니다.

                    if (prev == -1) prev = machineIdArray[buildClassIndex][index];
                    if (result < machineIdArray[buildClassIndex][index])
                    {
                        result = machineIdArray[buildClassIndex][index];
                        break;
                    }
                }
            }
            if (prev == -1) result = 0;
            else if (result == selectedMachineClassID) result = prev;
            // 그 외에는 result에서 특정 값이 들어갔음을 의미
            selectedMachineClassID = result;
            EachClassIndex[buildClassIndex] = selectedMachineClassID;
        }
    }
    #endregion
    #endregion

    #region item - debug
    #region item - debug - fieldStick
    /// <summary>
    ///     닿은 유닛의 정보를 UI로 표현합니다.
    /// </summary>
    [System.Serializable]
    public class FieldStick : Item
    {
        public override float ESkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float FSkill(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Supply(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
        public override float Update(GameObject user, float deltaTime)
        {
            return 0.0f;
        }
        public override float Use(GameObject user, Vector3 vector3)
        {
            return 0.0f;
        }
    }
    #endregion
    #endregion
    #endregion
    #endregion
}
