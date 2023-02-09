using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ChangeController에 임시로 들어갈 정보입니다.
/// </summary>
public class DemoChange : ChangeHelper
{
    /// <summary>
    ///     각 Demo 화학물이 각 에너지에 저항하는 값을 가지고 있는 테이블입니다.
    /// </summary>
    /// <remarks>
    ///     만약에 새로운 에너지와 캐미컬이 만들어지면, 그것을 여기에 반영해야 합니다. 왕 하드코딩이 예상됩니다.</remarks>
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
