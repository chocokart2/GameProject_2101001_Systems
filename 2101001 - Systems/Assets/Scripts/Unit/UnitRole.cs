using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRole : MonoBehaviour, GameManager.IComponentDataIOAble<UnitRole.UnitRoleData>
{
    #region ��� ���̴� Ŭ������?
    // �� ������ ������ ���ؼ� �ٷ�ϴ�.
    // �����ִ� ����, ������, ���� Ư���� �� �ִ� ������ ������ �ֽ��ϴ�.
    // �ΰ� ������ �ƴϴ���, Ư���� ������ ���� �� �ִٸ� �� ������Ʈ�� �����ϼ���.

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region �ʵ�
    UnitRoleData roleData;
    #endregion



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

        public int teamID; // TeamID�� �ε��� ���� �ƴմϴ�!
        public int squadID; // �ڽ��� �Ҽ��� ������ ���̵� (!�ε����� �ƴ�!)
        public int unitID; //UnitID�� �ε��� ���� �ƴմϴ�!





        #endregion
    }
    public struct MemberInfo
    {
        public int teamID;
        public string teamName;

        public int squadID;

        public int unitID;
    }

    #region �ʵ��







    #endregion
    #region �Լ���

    #endregion
    #region �������̽� �Լ���
    #region IComponentDataIOAble
    public void SetData(UnitRoleData input)
    {
        roleData = input;
    }
    public UnitRoleData GetData()
    {
        return roleData;
    }



    #endregion


    #endregion


    #region JobManager





    #endregion
}
