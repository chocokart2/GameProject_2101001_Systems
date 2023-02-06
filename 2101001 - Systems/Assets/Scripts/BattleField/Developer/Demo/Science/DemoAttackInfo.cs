using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     AttackInfo의 인스턴스를 만들어주는 클래스입니다.
/// </summary>
public class DemoAttackInfo : AttackClassHelper
{
    /// <summary>
    ///     데모용 공격 클래스입니다.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         암흑 물질입니다. 암흑 물질은 어떤
    ///     </para>
    /// </remarks>
    public static AttackInfo GetDemoAttackInfoData()
    {
        AttackInfo result = new AttackInfo();
        result.energies = new EnergyHelper.Energies(
            new EnergyHelper.Energy()
            {
                type = DemoEnergy.Default,
                amount = 10.0f
            }
            );
        result.chemicals = new ChemicalHelper.Chemicals(
            new ChemicalHelper.Chemical()
            {
                matter = DemoChemical.Default,
                quantity = 100.0f
            }
            );
        return result;
    }
}
