using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     프리펩을 게임 월드에 손으로 직접 배치한 경우, 정보를 입력하는 컴포넌트입니다.
/// </summary>
public class ManualUnitData : BaseComponent
{
    // 받을 정보
    // 유닛의 타입(Biological, Mechanical), 종류(종족, 모델명)
    // -> 그에 맞는 컴포넌트 접근

    // UI 정보
    [SerializeField, Tooltip("이 유닛의 큰 종류입니다.")] string unitType;
    [SerializeField, Tooltip("이 종류의 세부 종류입니다.")] string unitSpecies;

    [SerializeField] bool isDemoHumanPartBase;

    UnitBase unitBase;
    // type
    HumanUnitBase humanPartBase;

    // function
    UnitLife unitLife;
    


    void Awake()
    {
        // 컴포넌트 할당.
        unitBase = GetComponent<UnitBase>();
        if(unitBase == null)
            Hack.Error(this.GetType().ToString(), "이 게임오브젝트는 UnitBase 컴포넌트가 존재하지 않습니다.");

        switch (unitType, unitSpecies)
        {
            case ("biological", BiologicalPartBase.Species.HUMAN):
                humanPartBase = GetComponent<HumanUnitBase>();
                if (humanPartBase == null)
                    Hack.Error(this.GetType().ToString(), "이 게임오브젝트는 HumanUnitBase 컴포넌트가 존재하지 않습니다.");

                break;
            case ("biological", _):
                Hack.Error(this.GetType().ToString(), "알 수 없는 유닛 종류");
                break;
            case (MechanicPartBase.MechanicType.DEFAULT, _):
                Hack.Error(this.GetType().ToString(), "미구현");
                break;
            default:
                Hack.Error(this.GetType().ToString(), "알 수 없는 상태");
                break;
        }

        unitLife = GetComponent<UnitLife>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
