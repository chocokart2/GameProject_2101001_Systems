using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     공격에 대한 정보를 담고 있는 AttackInfo에 대하여 정의한 컴포넌트 클래스입니다.
/// </summary>
public class AttackClassHelper : MonoBehaviour
{
    /// <summary>
    /// 공격용 물제 및 투사체, 예를 들어 칼, 총알 들이 가지고 있는 공격에 대한 정보입니다.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [System.Serializable]
    public class AttackInfo
    {
        public ChemicalHelper.Chemicals chemicals;
        public EnergyHelper.Energies energies;
    }
}

// 추가적인 이야기
// + 운동 에너지에 영감을 받은 질량 / 속도는 더 포괄적인 EnergyHelper.Energies에 운동 에너지를 포함하게 되므로 표현하지 않았습니다.