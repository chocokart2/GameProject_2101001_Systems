using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ChangeController�� �ӽ÷� �� �����Դϴ�.
/// </summary>
public class DemoChange : ChangeHelper
{
    /// <summary>
    ///     �� Demo ȭ�й��� �� �������� �����ϴ� ���� ������ �ִ� ���̺��Դϴ�.
    /// </summary>
    /// <remarks>
    ///     ���࿡ ���ο� �������� ĳ������ ���������, �װ��� ���⿡ �ݿ��ؾ� �մϴ�. �� �ϵ��ڵ��� ����˴ϴ�.</remarks>
    /// <returns></returns>
    public static ChemicalEnergyResistTable GetDemoChemicalEnergyResistTable()
    {
        ChemicalEnergyResistTable result = new ChemicalEnergyResistTable()
        {
            m_self = new ChemicalEnergyResist[]
            {
                new ChemicalEnergyResist()
                {
                    ChemicalName = DemoChemical.Default,
                    m_self = new EnergyResist[]
                    {
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Default,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        },
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Special,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        }
                    }
                },
                new ChemicalEnergyResist()
                {
                    ChemicalName = DemoChemical.Alpha,
                    m_self = new EnergyResist[]
                    {
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Default,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        },
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Special,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        }
                    }
                },
                new ChemicalEnergyResist()
                {
                    ChemicalName = DemoChemical.Beta,
                    m_self = new EnergyResist[]
                    {
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Default,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        },
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Special,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        }
                    }
                },
                new ChemicalEnergyResist()
                {
                    ChemicalName = DemoChemical.Gamma,
                    m_self = new EnergyResist[]
                    {
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Default,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        },
                        new EnergyResist()
                        {
                            energyType = DemoEnergy.Special,
                            resistanceDefense = 30.0f,
                            resistanceRatio = 0.3f,
                        }
                    }
                }
            }
        };

        return result;
    }

    public static ChemicalReactionTable GetDemoChemicalReactionTable()
    {
        ChemicalReactionTable result = new ChemicalReactionTable()
        {
            reactions = new ChemicalReaction[]
            {
                new ChemicalReaction(
                    _priority: 1.0f,
                    _reactants: new ChemicalHelper.Chemicals()
                    {
                        new ChemicalHelper.Chemical()
                        {
                            matter = DemoChemical.Alpha,
                            quantity = 1.0f
                        },
                        new ChemicalHelper.Chemical()
                        {
                            matter = DemoChemical.Beta,
                            quantity = 2.0f
                        }
                    },
                    _products: new ChemicalHelper.Chemicals()
                    {
                        new ChemicalHelper.Chemical()
                        {
                            matter = DemoChemical.Gamma,
                            quantity = 0.5f
                        }
                    },
                    _activationEnergy: new EnergyHelper.Energies()
                    {
                        self = new EnergyHelper.Energy[0]
                    },
                    _energyReaction: new EnergyHelper.Energies()
                    {
                        self = new EnergyHelper.Energy[]
                        {
                            new EnergyHelper.Energy()
                            {
                                type = DemoEnergy.Default,
                                amount = 3.0f
                            }
                        }
                    }
                    )
            }
        };
        return result;
    }
}
