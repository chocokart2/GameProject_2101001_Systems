using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� �������� �θ� Ŭ������ ������ �ֽ��ϴ�.
/// </summary>
public class ItemHelper : BaseComponent
{
    #region Interface
    public interface IItem 
    {

    }
#warning ITool�� �޼��忡 ���� GameObject �ֱ�.
    public interface ITool : IItem
    {
        /// <szummary>
        ///     ������ �� �������� ����Ҷ��� �����մϴ�. [��Ŭ��]
        /// </summary>
        /// <param name="direction">Ŭ���� �����Դϴ�.</param>
        /// <returns>�ൿ ������ ��ٿ��Դϴ�.</returns>
        public float Use(GameObject user, Vector3 direction);
        public float ESkill(GameObject user, Vector3 vector3); // ������ ��ų�� ����� �� ȣ��˴ϴ�.
        public float FSkill(GameObject user, Vector3 vector3);
        public float Supply(GameObject user, Vector3 vector3); // ������ ����� ��ų�� ����� �� ȣ��˴ϴ�.
        public float Update(GameObject user, float deltaTime); // �� �����Ӹ��� ȣ��Ǵ� �Լ��Դϴ�.

    }
    public interface IName
    {
        string TypeName { get; }
    }
    #endregion
    #region Class
    // ������ Ŭ�����鿡 ���� ������ �����Դϴ�.
    #region ItemBase : Item, SubItem, SubItemArray, Inventory
    /// <summary>
    ///     �� Ŭ������ �Ļ����� ���� �������� �����Դϴ�.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         ���� ������: �� �������� �����ϴ� �������� ��Ÿ���ϴ�.
    ///     </para>
    ///     <para>
    ///         ��κ� ����������� 0��° �ε����� ����մϴ�.
    ///     </para>
    /// 
    /// </remarks>
    [System.Serializable]
    public abstract class Item : ITool
    {
#warning ������ : ���� ������ ��Ͽ����� ��ȣ �ε����� �����ϴµ�, ��ųʸ��� �̿��� �̸����� �ٷ� �����ϴ� ����� �߰��Ѵ�. ��ųʸ� : �̸� -> �ε���
        [System.NonSerialized] public static GameObjectList gameObjectList;
        public string Name;
        /// <summary>
        ///     �������� ������ ��Ÿ���ϴ�
        /// </summary>
        public SubItemArray subItems;

        public virtual string ItemType { get => "UNDEFINED_TYPE"; }

        public abstract float Use(GameObject user, Vector3 vector3); // �������� ����� �� ȣ��˴ϴ�.
        public abstract float ESkill(GameObject user, Vector3 vector3); // ������ ��ų�� ����� �� ȣ��˴ϴ�.
        public abstract float FSkill(GameObject user, Vector3 vector3);
        public abstract float Supply(GameObject user, Vector3 vector3); // ������ ����� ��ų�� ����� �� ȣ��˴ϴ�.
        /// <summary>
        ///     �� �����Ӹ��� ȣ��Ǵ� �Լ��Դϴ�.
        /// </summary>
        /// <remarks>
        ///     ������ ������ ������ �����ս� ����� �ʷ��մϴ�.
        /// </remarks>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public abstract float Update(GameObject user, float deltaTime); // �� �����Ӹ��� ȣ��Ǵ� �Լ��Դϴ�.
    }
    /// <summary>
    /// ������ ������ ������ �ϳ��� �����մϴ�.
    /// </summary>
    /// ������ ���ο� ���� ��ǰ���� ������ �� �ֽ��ϴ�.
    /// ���� ���÷�, ���ѿ� ���� ź���� �̿� �ش��մϴ�. ������ ������ �Ҷ� ź���� �Ҹ��ϱ� �����Դϴ�.
    /// 
    [System.Serializable]
    public class SubItem : INameKey
    {
        public string name;
        public int amount;
        public AttackClassHelper.AttackInfo attackInfo; // �� �������� ���ݿ� �ش��ϴ°���Դϴ�.

        public string Name { get; set; }
    }
    /// <summary>
    ///     ������ ������ �����۵��� �����մϴ�.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         ���� ���� �����ۿ� �����ϰ� �ʹٸ� SubItem�� �����Ͻʽÿ�.
    ///     </para>
    /// </remarks>
    [System.Serializable]
    public class SubItemArray : IArray<SubItem>
    {
        public SubItem[] self;
#warning �̰� �ε������� Ű���� �ʱ�ȭ���ִ� �Լ� �����߰ڴµ�.
        [System.NonSerialized] public Dictionary<string, int> nameIndexPairs;

        public int Length
        {
            get => self.Length;
        }
        /// <summary>
        ///     �̸����� �����ϴ� �ε����Դϴ�.
        /// </summary>
        /// <param name="name">
        ///     �����ϱ� ���� �̸��Դϴ�.
        /// </param>
        /// <returns>
        ///     <para>
        ///         ���� ���� ���� ���θ� ���� Ʃ�� ������ �����մϴ�.
        ///     </para>
        ///     <para>
        ///         result : ã������ ��ü�Դϴ�.
        ///     </para>
        ///     <para>
        ///         isAccessed : ���� �����ߴ����� ��Ÿ���� �Ҹ��� ���Դϴ�.
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
                        Debug.LogWarning("<!>ERROR_SubItemArray.this[string] : �������� �ʴ� �̸��� ���� �����Ͽ����ϴ�.");
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

#warning lv 1 func �� �Լ� �׽�Ʈ ����.
        /// <summary>
        ///     ��ųʸ��� ���� ������Ʈ �մϴ�. �̸� -> �� �̸��� ���� ���ҵ��� ���� ù��°�� ������ �ε���
        /// </summary>
        /// <remarks>
        ///     �������� ������ �Լ��Դϴ�.
        /// </remarks>
        public void UpdateIndex()
        {
            // ��ųʸ��� �� Ű ���� ����, �߰���
            // ��ųʸ��� �� Ű ���� ����, ������.

            for(int index = 0; index < self.Length; index++)
            {
                if (nameIndexPairs.ContainsKey(self[index].name)) continue;
                nameIndexPairs.Add(self[index].name, index);
            }
        }
    }
    /// <summary>
    /// �������� ������ �� �ִ� �����Դϴ�. ���� �׼��� ���� ��, ����� �ε����� ���� ������ ���������� �ٷ� ������ �� �ְ� ���ݴϴ�.
    /// </summary>
    /// �ƹ����� �� ����� �ΰ� �Ӹ� �ƴ϶�,
    /// �������� ����ϱ⸦ �ٶ�� ����, �� �κ��� �̰��� ����� �� ���� ���̴�.
    [System.Serializable]
    public class Inventory : IArray<Item>, ITool
    {
        //  field
        public GameObject gameObject;
        public Item[] self; // �� Ŭ������ �ٽ� Ŭ�����Դϴ�.

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
            Debug.Log("DEBUG_UnitItemPack.Radios: Radio ������ ������Դϴ�.");
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
                if (subItems[0].attackInfo == null) Debug.Log("DEBUG_Pistol.Use(GameObject,Vector3) : subItem�� null �Դϴ�.");
                GameObjectList.PistolBulletShot(user, subItems[0].attackInfo, vector3);
                subItems[0].amount--;
                // �߻� �Ҹ�
                return 0.3f;
            }
            else
            {
                // �߻� �� �Ǵ� �Ҹ�
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
#warning �����
            Debug.Log($"DEBUG_ItemHelper.Use(Vector3) : subItems�� Null ���ΰ���? {subItems[0].attackInfo == null}");
#warning GameObjectList�� �䱸�ϴ� �Լ��� ����ƽ�� �ƴϰų�, �ۺ��� �ƴմϴ�.
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
            Debug.Log("Turret �������� ����");
            if (subItems["ammo"].isAccessed == false)
            {
                // �������� �����ϴ�.
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
                Debug.Log("Turret �������� ����");

                return 0.0f;
                // �߻� �ȵǴ� �Ҹ�
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
        ///     ��ȿ�� �ӽ��� ������ �迭�Դϴ�. ���� ���ں��� ū ���� ������ ���ĵǾ� �ֽ��ϴ�.
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

#warning �������� �ۺ� ������Ƽ�� �����̺� �ʵ� �̸��� ��ġ���� �ʽ��ϴ�.
        public int CursorIndex { get => selectedIndex; }
        public int CursorMachineID { get => selectedMachineClassID; }
        public int[] EachClassMachineID { get => EachClassIndex; }
#warning �뵵�� �𸣴� ����
        bool isFirstSkill = true;
        float UiLifeTime = 0.0f;
        /// <summary>
        /// ���� ������ �ӽ� Ŭ���� ������ ���� ���Դϴ�.
        /// </summary>
        int selectedMachineClassID = 101;
        /// <summary>
        ///     EachClassIndex������Ű�� Ŀ���Դϴ�.
        /// </summary>
        int selectedIndex = 0; //EachClassIndex�� ���° �ε����� ����Ű�� �ִ°�
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
            if (selectedMachineClassID != 0) // ���� ��� �ִ� �������� ������ 0 �ʰ��ΰ�� And ���� ��� �ִ� �������� ���Ⱑ �ƴѰ��
            {
                string holdingItem = "MachineUnit" + selectedMachineClassID.ToString();
                if (subItems[holdingItem].result.amount <= 0)
                {
                    Debug.Log("<!> Warning_UnitItemPack.BuildTool.Use() �ӽ� Ŭ����ID��" + selectedMachineClassID + " �� �ӽ��� �����ϰ� ���� �ʽ��ϴ�.");
                    SelectNextUnit(selectedIndex);
                }
                else
                {
                    bool isBuildWorks = false;
                    gameObjectList.Build(vector3, selectedMachineClassID, ref isBuildWorks);
                    if (isBuildWorks)
                    {
                        Debug.Log("�ӽ��� ��ġ�Ǿ����ϴ�");
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

#warning lv 1 �׽�Ʈ ����
        /// <summary>
        ///     ���� ������ �ӽ��� ���� �ӽ��� �����ϵ��� �մϴ�.
        /// </summary>
        /// <param name="buildClassIndex">����� ��ȣ�� EachClassIndex�� �ε��� ��ȣ�Դϴ�.</param>
        private void SelectNextUnit(int buildClassIndex)
        {
            // machineIdArray[buildClassIndex][]�� �ش��ϴ� �迭�� ���� ���̸鼭
            // selectedMachineClassID���� ū ���� ���� ���� ���� �����Ѵ�
            // ���� ã�� �� ���ٸ�, �迭���� ���� ���� ���� �����ϰų�,
            // �׸��� ��ȿ���� �ʴٸ� 0�� �����Ѵ�.
            // machineIdArray

            int result = selectedMachineClassID;
            int prev = -1;
            
            subItems.UpdateIndex(); // �ϴ� ��Ȯ�� ������ �� �ֵ��� �մϴ�.
            for(int index = 0; index < machineIdArray[buildClassIndex].Length; index++)
            {
                // ���� ���� ���� ���� �����̰�, ���� ���� �� < ���� �� < selectedMachineClassID���̸� �����մϴ�.
                if (prev != -1 && machineIdArray[buildClassIndex][index] < selectedMachineClassID) continue;

                string searchingKey = $"MachineUnit{machineIdArray[buildClassIndex][index]}";
                if (subItems.nameIndexPairs.ContainsKey(searchingKey))
                {
                    // Ű ���� �����ϴ� ���̵� ���� ã�ҽ��ϴ�.

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
            // �� �ܿ��� result���� Ư�� ���� ������ �ǹ�
            selectedMachineClassID = result;
            EachClassIndex[buildClassIndex] = selectedMachineClassID;
        }
    }
    #endregion
    #endregion

    #region item - debug
    #region item - debug - fieldStick
    /// <summary>
    ///     ���� ������ ������ UI�� ǥ���մϴ�.
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
