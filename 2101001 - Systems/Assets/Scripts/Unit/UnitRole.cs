using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRole : MonoBehaviour
{
    #region 어디에 쓰이는 클래슨고?
    // 이 유닛의 직업에 대해서 다룹니다.
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
        #region 생성자

        #endregion
        #region 필드들
        public string roleName; // 이 유닛의 직업이 무엇인가



        #endregion
    }





    #region 필드들







    #endregion


    #region JobManager





    #endregion
}
