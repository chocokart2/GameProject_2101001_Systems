using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRole : MonoBehaviour, GameManager.IComponentDataIOAble<UnitRole.UnitRoleData>
{
    #region 어디에 쓰이는 클래슨고?
    // 이 유닛의 직업에 대해서 다룹니다.
    // 속해있는 유닛, 스쿼드, 팀을 특정할 수 있는 정보를 가지고 있습니다.
    // 인간 유형이 아니더라도, 특정한 역할을 맡을 수 있다면 이 컴포넌트를 주입하세요.

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 필드
    UnitRoleData roleData;
    #endregion



    [System.Serializable]
    public class UnitRoleData
    {
        #region 클래스에 대한 설명
        // 역할 : 이 유닛이 어떤 역할을 하고 있는지에 대해서 표시합니다.
        // 이 유닛의 직업에 대한 정보를 담고 있습니다.
        // 이 유닛이 어떤 아이템을 가지고 있는지에 대한 정보를 가지고 있습니다.
        // 이 유닛이 팀 내에서 어떤 위치를 가지고 있는지에 대한 정보를 가지고 있습니다
        #region 왜 unitBase에 이러한 정보가 없는가
        // unitBase에서는 여러 종류의 유닛이 가지고 있는 컴포넌트입니다
        // 인간 유닛 등 여러 필드를 옮겨다니는 유닛만이 가지고 있는 컴포넌트가 필요했습니다
        // 여기서 HumanUnitBase 에 대해서는 생체 유닛이 가지고 있는 생체 정보들을 담기 위해 존재하는 컴포넌트입니다
        // 따라서 새로운 컴포넌트를 만들어 여기에 담도록 합시다.
        #endregion
        #endregion
        #region 생성자

        #endregion
        #region 필드들        
        public string roleName; // 이 유닛의 직업이 무엇인가


        // 소속

        // ID값은 단순히 임시 데이터인 FieldData의 유닛 목록중에서 자신을 특정하기 위한 값입니다.
        // 유동적으로 변하는 INDEX값은 아닙니다!
        // 또한 게임이 끝나면 이 ID값은 파기될겁니다.
        // WorldManager 수준에서 각 팀 / 스쿼드 / 유닛을 특정하는 정보는 따로 만들어야 합니다.
        MemberInfo memberInfo;

        public int teamID; // TeamID는 인덱스 값이 아닙니다!
        public int squadID; // 자신이 소속한 스쿼드 아이디 (!인덱스가 아님!)
        public int unitID; //UnitID는 인덱스 값이 아닙니다!





        #endregion
    }
    public struct MemberInfo
    {
        public int teamID;
        public string teamName;

        public int squadID;

        public int unitID;
    }

    #region 필드들







    #endregion
    #region 함수들

    #endregion
    #region 인터페이스 함수들
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
