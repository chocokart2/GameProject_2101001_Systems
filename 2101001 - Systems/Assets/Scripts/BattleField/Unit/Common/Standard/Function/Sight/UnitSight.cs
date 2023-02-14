using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     이 유닛은 시각을 사용할 수 있습니다.
/// </summary>
/// <remarks>
///     디자인패턴 팁 : UnitBase를 이용하여 소통하십시오. 의존성에 의한 위험을 낮출 수 있습니다.
///     상황에 따라 시야가 좁아집니다.
/// </remarks>
public class UnitSight : MonoBehaviour
{
    #region 필드
    public int[] recvLightNums;

    private const string M_SIGHT_FOLDER = "Prefabs/BattleField/GameUnit/Original/Supporter";
    private const string M_PREFAB_NAME = "UnitSight";
    #endregion
    #region 유니티 메서드
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    #region 메소드
    //public static void UnitSightScale()
    #endregion
    #region Nested Class

    #endregion
}
