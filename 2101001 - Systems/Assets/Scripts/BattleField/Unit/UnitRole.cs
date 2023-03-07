using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     이 유닛의 역할 및 스쿼드속 역할에 대해서 다룹니다.
/// </summary>
/// <remarks>
///     속해있는 유닛, 스쿼드, 팀을 특정할 수 있는 정보를 가지고 있습니다. 인간 유형이 아니더라도, 특정한 역할을 맡을 수 있다면 이 컴포넌트를 주입하세요.
/// </remarks>
public class UnitRole : 
    MonoBehaviour, 
    BaseComponent.IDataGetableComponent<UnitRole.UnitRoleData>,
    BaseComponent.IDataSetableComponent<UnitRole.UnitRoleData>,
    GameManager.IComponentDataIOAble<UnitRole.UnitRoleData>
{
    #region Class Member
    #region 필드
    //public:
    public EditorField roleForEditMode;

    //private:

    private UnitRoleData mRoleData;
    
    
    #endregion
    #region Method
    #region 생성자 역할
    // Start is called before the first frame update
    void Start()
    {
        if(mRoleData == null)
        {

        }


    }
    #endregion
    #region 메인 함수 역할
    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    #region 인터페이스 함수들
    #region IComponentDataIOAble
    public void SetData(UnitRoleData input)
    {
        mRoleData = input;
    }
    public UnitRoleData GetData()
    {
        if(mRoleData == null)
        {
            // 데이터 묶음에 접근하여 아이디 값을 얻습니다.
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
        #region 구조체 설명
        // 에디터 모드로 태어난 유닛인 경우에, 인스팩터에서 이 값을 받아와

        #endregion
        [Tooltip(
            "테스트 환경에서 이 유닛이 속해있는 팀의 이름입니다. \n" +
            "TeamID 값을 대체합니다.")]
        public string teamName;
        [Tooltip(
            "테스트 환경에서 이 유닛이 속해있는 스쿼드의 이름입니다. \n" +
            "SquadID값을 대체합니다.")]
        public string squadName; // team의 squad, 이름이 같으면 같은 squad로 들어갑니다
        [Tooltip(
            "테스트 환경에서 이 유닛의 이름입니다. \n" +
            "게임매니저의 유닛 배열에도 들어가게 됩니다. \n" +
            "만약 같은 이름을 가진 유닛이 존재한다면 오류가 생길 수 있습니다 (혹은 이름이 강제로 바뀔 수 있습니다.)")]
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

        /// <summary>
        ///     자신의 팀의 번호.
        /// </summary>
        /// <remarks>
        ///     배열의 인덱스 값이 아닙니다.    
        /// </remarks>
        public int teamID; // TeamID는 인덱스 값이 아닙니다!
        /// <summary>
        ///     자신의 스쿼드의 번호.
        /// </summary>
        /// <remarks>
        ///     배열의 인덱스 값이 아닙니다.    
        /// </remarks>
        public int squadID; // 자신이 소속한 스쿼드 아이디 (!인덱스가 아님!)
        /// <summary>
        ///     자신의 유닛의 번호.
        /// </summary>
        /// <remarks>
        ///     배열의 인덱스 값이 아닙니다.    
        /// </remarks>
        public int unitID; //UnitID는 인덱스 값이 아닙니다!





        #endregion
    }
    #endregion
    #endregion
}
