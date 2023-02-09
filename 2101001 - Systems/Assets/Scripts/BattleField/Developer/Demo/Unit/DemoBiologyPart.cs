using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBiologyPart : BiologicalPartBase
{
    public const string DEFAULT_ORGAN_NAME = "Default_Organ";

    /// <summary>
    ///     ��� ���� ������ Organ�� �� �� �ִ� ��ü�� �����մϴ� </summary>
    /// <remarks>
    ///     OrganPart�� ����ϴ� Ŭ������ ��ü�� ������, ������ ���� ������ �� �ֽ��ϴ�. </remarks>
    /// <returns>
    ///     OrganPart�� ��ü�� �����մϴ�. </returns>
    public static OrganPart GetDemoOrganPart()
    {
        Hack.Say(Hack.isDebugDemoBiologyPart, "DEBUG_DemoBiologyPart.GetDemoOrganPart() : �Լ��� ȣ��Ǿ����ϴ�. �ǵ��� ȣ���� �ƴ�");

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
    ///     OrganPart ��ü�� ��� �������� �ʱ�ȭ���ݴϴ�. �ٸ� �Ű������� �ѱ� ��, �����ڸ� �̸� ȣ������� �մϴ�. �ֳ��ϸ� �̴� �Ļ� Ŭ�������� �̷� ���񽺸� �����ϱ� �����Դϴ�.
    /// </summary>
    /// <param name="organPart"></param>
    public static void SetMemberDemoOrganPart<T>(ref T organPart)
        where T : OrganPart, new()
    {
        Hack.Say(Hack.isDebugDemoBiologyPart, "DEBUG_DemoBiologyPart.SetMemberDemoOrganPart(T) : �Լ��� ȣ��Ǿ����ϴ�.");
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
