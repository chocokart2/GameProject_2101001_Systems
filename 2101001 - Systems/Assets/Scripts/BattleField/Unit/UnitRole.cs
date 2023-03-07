using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     �� ������ ���� �� ������� ���ҿ� ���ؼ� �ٷ�ϴ�.
/// </summary>
/// <remarks>
///     �����ִ� ����, ������, ���� Ư���� �� �ִ� ������ ������ �ֽ��ϴ�. �ΰ� ������ �ƴϴ���, Ư���� ������ ���� �� �ִٸ� �� ������Ʈ�� �����ϼ���.
/// </remarks>
public class UnitRole : 
    MonoBehaviour, 
    BaseComponent.IDataGetableComponent<UnitRole.UnitRoleData>,
    BaseComponent.IDataSetableComponent<UnitRole.UnitRoleData>,
    GameManager.IComponentDataIOAble<UnitRole.UnitRoleData>
{
    #region Class Member
    #region �ʵ�
    //public:
    public EditorField roleForEditMode;

    //private:

    private UnitRoleData mRoleData;
    
    
    #endregion
    #region Method
    #region ������ ����
    // Start is called before the first frame update
    void Start()
    {
        if(mRoleData == null)
        {

        }


    }
    #endregion
    #region ���� �Լ� ����
    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    #region �������̽� �Լ���
    #region IComponentDataIOAble
    public void SetData(UnitRoleData input)
    {
        mRoleData = input;
    }
    public UnitRoleData GetData()
    {
        if(mRoleData == null)
        {
            // ������ ������ �����Ͽ� ���̵� ���� ����ϴ�.
            //bmRoleData.
        }
        
        return mRoleData;
    }

    public void SetComponentData(UnitRoleData data)
    {
        mRoleData = data;
    }
    public UnitRoleData GetComponentData()
    {
        return mRoleData;
    }
    #endregion


    #endregion
    #endregion
    #endregion
    #region Nested Class
    #region Structure
    [System.Serializable]
    public struct EditorField
    {
        #region ����ü ����
        // ������ ���� �¾ ������ ��쿡, �ν����Ϳ��� �� ���� �޾ƿ�

        #endregion
        [Tooltip(
            "�׽�Ʈ ȯ�濡�� �� ������ �����ִ� ���� �̸��Դϴ�. \n" +
            "TeamID ���� ��ü�մϴ�.")]
        public string teamName;
        [Tooltip(
            "�׽�Ʈ ȯ�濡�� �� ������ �����ִ� �������� �̸��Դϴ�. \n" +
            "SquadID���� ��ü�մϴ�.")]
        public string squadName; // team�� squad, �̸��� ������ ���� squad�� ���ϴ�
        [Tooltip(
            "�׽�Ʈ ȯ�濡�� �� ������ �̸��Դϴ�. \n" +
            "���ӸŴ����� ���� �迭���� ���� �˴ϴ�. \n" +
            "���� ���� �̸��� ���� ������ �����Ѵٸ� ������ ���� �� �ֽ��ϴ� (Ȥ�� �̸��� ������ �ٲ� �� �ֽ��ϴ�.)")]
        public string unitName;
    }

    public struct MemberInfo
    {
        public int teamID;
        public string teamName;

        public int squadID;

        public int unitID;
    }
    #endregion
    #region Class
    [System.Serializable]
    public class UnitRoleData
    {
        #region Ŭ������ ���� ����
        // ���� : �� ������ � ������ �ϰ� �ִ����� ���ؼ� ǥ���մϴ�.
        // �� ������ ������ ���� ������ ��� �ֽ��ϴ�.
        // �� ������ � �������� ������ �ִ����� ���� ������ ������ �ֽ��ϴ�.
        // �� ������ �� ������ � ��ġ�� ������ �ִ����� ���� ������ ������ �ֽ��ϴ�
        #region �� unitBase�� �̷��� ������ ���°�
        // unitBase������ ���� ������ ������ ������ �ִ� ������Ʈ�Դϴ�
        // �ΰ� ���� �� ���� �ʵ带 �Űܴٴϴ� ���ָ��� ������ �ִ� ������Ʈ�� �ʿ��߽��ϴ�
        // ���⼭ HumanUnitBase �� ���ؼ��� ��ü ������ ������ �ִ� ��ü �������� ��� ���� �����ϴ� ������Ʈ�Դϴ�
        // ���� ���ο� ������Ʈ�� ����� ���⿡ �㵵�� �սô�.
        #endregion
        #endregion
        #region ������

        #endregion
        #region �ʵ��        
        public string roleName; // �� ������ ������ �����ΰ�


        // �Ҽ�

        // ID���� �ܼ��� �ӽ� �������� FieldData�� ���� ����߿��� �ڽ��� Ư���ϱ� ���� ���Դϴ�.
        // ���������� ���ϴ� INDEX���� �ƴմϴ�!
        // ���� ������ ������ �� ID���� �ı�ɰ̴ϴ�.
        // WorldManager ���ؿ��� �� �� / ������ / ������ Ư���ϴ� ������ ���� ������ �մϴ�.
        MemberInfo memberInfo;

        /// <summary>
        ///     �ڽ��� ���� ��ȣ.
        /// </summary>
        /// <remarks>
        ///     �迭�� �ε��� ���� �ƴմϴ�.    
        /// </remarks>
        public int teamID; // TeamID�� �ε��� ���� �ƴմϴ�!
        /// <summary>
        ///     �ڽ��� �������� ��ȣ.
        /// </summary>
        /// <remarks>
        ///     �迭�� �ε��� ���� �ƴմϴ�.    
        /// </remarks>
        public int squadID; // �ڽ��� �Ҽ��� ������ ���̵� (!�ε����� �ƴ�!)
        /// <summary>
        ///     �ڽ��� ������ ��ȣ.
        /// </summary>
        /// <remarks>
        ///     �迭�� �ε��� ���� �ƴմϴ�.    
        /// </remarks>
        public int unitID; //UnitID�� �ε��� ���� �ƴմϴ�!





        #endregion
    }
    #endregion
    #endregion
}
