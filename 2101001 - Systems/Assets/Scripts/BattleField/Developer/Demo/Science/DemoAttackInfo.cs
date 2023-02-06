using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     AttackInfo�� �ν��Ͻ��� ������ִ� Ŭ�����Դϴ�.
/// </summary>
public class DemoAttackInfo : AttackClassHelper
{
    /// <summary>
    ///     ����� ���� Ŭ�����Դϴ�.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         ���� �����Դϴ�. ���� ������ �
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
