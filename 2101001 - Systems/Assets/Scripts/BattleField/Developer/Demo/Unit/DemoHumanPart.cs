using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     �׽�Ʈ�� ���� OrganPart�� ����ϴ� Ŭ������ ��ü�� ���� ���� �����ϴ�.
/// </summary>
public class DemoHumanPart : HumanUnitBase
{
    // ��ü �ν��Ͻ�
    public static BioUnit GetDemoHuman()
    {
        return GetDemoHuman(null);
    }
    public static BioUnit GetDemoHuman(GameObject unit)
    {
        BioUnit result = new BioUnit();
        result.species = Species.HUMAN;
        result.organParts = new OrganPart[9]
        {
            GetDemoDigestiveSystem(),
            GetDemoCirculatorySystem(),
            GetDemoExcretorySystem(),
            GetDemoSensorySystem(),
            GetDemoNervousSystem(),
            GetDemoMotorSystem(),
            GetDemoImmuneSystem(),
            GetDemoSynthesisSystem(),
            GetDemoIntegumentarySystem()
        };
        foreach(OrganPart one in result.organParts)
        {
            one.unit = unit;
        }
        return result;
    }
    // ��� BioUnit�� OrganPart�鿡 ���� �ν��Ͻ��� ���ӿ�����Ʈ�� �ֽ��ϴ�.
    public static BioUnit AssignGameObject(BioUnit bioUnit, GameObject unit)
    {
        foreach (OrganPart one in bioUnit.organParts)
        {
            one.unit = unit;
        }
        return bioUnit;
    }

    // ����� �ν��Ͻ�    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static DigestiveSystem GetDemoDigestiveSystem()
    {
        DigestiveSystem result = DemoBiologyPart.GetDemoOrganPart<DigestiveSystem>();
        return result;
    }
    public static DigestiveSystem GetDemoDigestiveSystem(GameObject unit)
    {
        DigestiveSystem result = DemoBiologyPart.GetDemoOrganPart<DigestiveSystem>();
        result.unit = unit;
        return result;
    }

    public static CirculatorySystem GetDemoCirculatorySystem()
    {
        CirculatorySystem result = DemoBiologyPart.GetDemoOrganPart<CirculatorySystem>();
        return result;
    }
    public static CirculatorySystem GetDemoCirculatorySystem(GameObject unit)
    {
        CirculatorySystem result = DemoBiologyPart.GetDemoOrganPart<CirculatorySystem>();
        result.unit = unit;
        return result;
    }

    public static ExcretorySystem GetDemoExcretorySystem()
    {
        ExcretorySystem result = DemoBiologyPart.GetDemoOrganPart<ExcretorySystem>();
        return result;
    }
    public static ExcretorySystem GetDemoExcretorySystem(GameObject unit)
    {
        ExcretorySystem result = DemoBiologyPart.GetDemoOrganPart<ExcretorySystem>();
        result.unit = unit;
        return result;
    }

    public static SensorySystem GetDemoSensorySystem()
    {
        SensorySystem result = DemoBiologyPart.GetDemoOrganPart<SensorySystem>();
        result.UnitSightRange = 6;
        return result;
    }
#warning �۾��� : ���ӿ�����Ʈ�� ���� ������ �����ϴ�.
    public static SensorySystem GetDemoSensorySystem(GameObject unit)
    {
        SensorySystem result = GetDemoSensorySystem();
        result.unit = unit;
        return result;
    }

    public static NervousSystem GetDemoNervousSystem()
    {
        NervousSystem result = DemoBiologyPart.GetDemoOrganPart<NervousSystem>();
        return result;
    }
    public static NervousSystem GetDemoNervousSystem(GameObject unit)
    {
        NervousSystem result = DemoBiologyPart.GetDemoOrganPart<NervousSystem>();
        result.unit = unit;
        return result;
    }

    public static MotorSystem GetDemoMotorSystem()
    {
        MotorSystem result = DemoBiologyPart.GetDemoOrganPart<MotorSystem>();
        return result;
    }
    public static MotorSystem GetDemoMotorSystem(GameObject unit)
    {
        MotorSystem result = DemoBiologyPart.GetDemoOrganPart<MotorSystem>();
        result.unit = unit;
        return result;
    }

    public static ImmuneSystem GetDemoImmuneSystem()
    {
        ImmuneSystem result = DemoBiologyPart.GetDemoOrganPart<ImmuneSystem>();
        return result;
    }
    public static ImmuneSystem GetDemoImmuneSystem(GameObject unit)
    {
        ImmuneSystem result = DemoBiologyPart.GetDemoOrganPart<ImmuneSystem>();
        result.unit = unit;
        return result;
    }

    public static SynthesisSystem GetDemoSynthesisSystem()
    {
        SynthesisSystem result = DemoBiologyPart.GetDemoOrganPart<SynthesisSystem>();
        return result;
    }
    public static SynthesisSystem GetDemoSynthesisSystem(GameObject unit)
    {
        SynthesisSystem result = DemoBiologyPart.GetDemoOrganPart<SynthesisSystem>();
        result.unit = unit;
        return result;
    }

    public static IntegumentarySystem GetDemoIntegumentarySystem()
    {
        IntegumentarySystem result = DemoBiologyPart.GetDemoOrganPart<IntegumentarySystem>();
        return result;
    }
    public static IntegumentarySystem GetDemoIntegumentarySystem(GameObject unit)
    {
        IntegumentarySystem result = DemoBiologyPart.GetDemoOrganPart<IntegumentarySystem>();
        result.unit = unit;
        return result;
    }
}
