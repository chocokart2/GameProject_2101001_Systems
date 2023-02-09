using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBiologyPart : BiologicalPartBase
{
    public const string DEFAULT_ORGAN_NAME = "Default_Organ";

    /// <summary>
    ///     모든 생물 유닛의 Organ에 들어갈 수 있는 객체를 리턴합니다 </summary>
    /// <remarks>
    ///     OrganPart를 상속하는 클래스의 객체에 넣을때, 부족한 값이 존재할 수 있습니다. </remarks>
    /// <returns>
    ///     OrganPart의 객체를 리턴합니다. </returns>
    public static OrganPart GetDemoOrganPart()
    {
        Hack.Say(Hack.isDebugDemoBiologyPart, "DEBUG_DemoBiologyPart.GetDemoOrganPart() : 함수가 호출되었습니다. 의도한 호출이 아님");

        OrganPart result = new OrganPart();
        SetMemberDemoOrganPart(ref result);
        return result;
    }

    public static T GetDemoOrganPart<T>()
        where T : OrganPart, new()
    {
        T result = new T();
        SetMemberDemoOrganPart(ref result);
        return result;
    }
    /// <summary>
    ///     OrganPart 객체의 멤버 변수들을 초기화해줍니다. 다만 매개변수를 넘길 때, 생성자를 미리 호출해줘야 합니다. 왜냐하면 이는 파생 클래스에게 이런 서비스를 제공하기 때문입니다.
    /// </summary>
    /// <param name="organPart"></param>
    public static void SetMemberDemoOrganPart<T>(ref T organPart)
        where T : OrganPart, new()
    {
        Hack.Say(Hack.isDebugDemoBiologyPart, "DEBUG_DemoBiologyPart.SetMemberDemoOrganPart(T) : 함수가 호출되었습니다.");
        organPart.chemicalWholeness = new ChemicalsWholeness()
        {
            m_self = new SingleChemicalWholeness[0] { }
        };
        // base value
        organPart.tagged = new ChemicalHelper.Chemicals()
        {
            new ChemicalHelper.Chemical()
            {
                matter = DemoChemical.Beta,
                quantity = 10.0f
            }
        };
        organPart.demand = new ChemicalHelper.Chemicals()
        {
            new ChemicalHelper.Chemical()
            {
                matter = DemoChemical.Beta,
                quantity = 10.0f
            }
        };
        organPart.others = new ChemicalHelper.Chemicals()
        {
            new ChemicalHelper.Chemical()
            {
                matter = DemoChemical.Default,
                quantity = 10.0f
            }
        };
        organPart.chemicalWholeness = new ChemicalsWholeness()
        {
            m_self = new SingleChemicalWholeness[]
            {
                new SingleChemicalWholeness()
                {
                    damages = new Penetration[0],
                    layer = 0,
                    name = DemoChemical.Beta
                }
            }
        };
        organPart.chemicalWholeness.Update();
        // derivedValue
        organPart.maxHP = 100.0f;
        organPart.HP = 100.0f;
        organPart.RecoveryRate = 0.4f;
        organPart.Name = DEFAULT_ORGAN_NAME;
    }
}
