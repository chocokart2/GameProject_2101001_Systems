using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 컴포넌트 설명:
/// 이 컴포넌트를 가지고 있는 유닛은 시각 능력이 있는 유닛의 컴포넌트[UnitSight]와
/// 불빛을 낼 수 있는 유닛의 [DisturberLight/LightMaker]컴포넌트들의 상호작용을 통해
/// 자신의 모습이 플레이어 카메라에게 랜더링되어야 하는지를 결정할 수 있습니다.
/// </summary>
public class UnitVisibleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 이 함수릃 호출하는 시기
    // 적어도 UnitBase에서 유닛들의 팀 작업이 완료되었으면 합니다.
}
