using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     테스트를 위한 OrganPart를 상속하는 클래스의 객체에 대한 값을 가집니다.
/// </summary>
public class DemoHumanPart : HumanUnitBase
{
    // 개체 인스턴스
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
    // 대상 BioUnit의 OrganPart들에 각각 인스턴스한 게임오브젝트를 넣습니다.
    public static BioUnit AssignGameObject(BioUnit bioUnit, GameObject unit)
    {
        foreach (OrganPart one in bioUnit.organParts)
        {
            one.unit = unit;
        }
        return bioUnit;
    }

    // 기관계 인스턴스    
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
#warning 작업중 : 게임오브젝트를 넣을 변수가 없습니다.
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
