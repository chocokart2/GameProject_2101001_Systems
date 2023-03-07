using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     �������� ���� ���忡 ������ ���� ��ġ�� ���, ������ �Է��ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class ManualUnitData : BaseComponent
{
    // ���� ����
    // ������ Ÿ��(Biological, Mechanical), ����(����, �𵨸�)
    // -> �׿� �´� ������Ʈ ����

    // UI ����
    [SerializeField, Tooltip("�� ������ ū �����Դϴ�.")] string unitType;
    [SerializeField, Tooltip("�� ������ ���� �����Դϴ�.")] string unitSpecies;

    [SerializeField] bool isDemoHumanPartBase;

    UnitBase unitBase;
    // type
    HumanUnitBase humanPartBase;

    // function
    UnitLife unitLife;
    


    void Awake()
    {
        // ������Ʈ �Ҵ�.
        unitBase = GetComponent<UnitBase>();
        if(unitBase == null)
            Hack.Error(this.GetType().ToString(), "�� ���ӿ�����Ʈ�� UnitBase ������Ʈ�� �������� �ʽ��ϴ�.");

        switch (unitType, unitSpecies)
        {
            case ("biological", BiologicalPartBase.Species.HUMAN):
                humanPartBase = GetComponent<HumanUnitBase>();
                if (humanPartBase == null)
                    Hack.Error(this.GetType().ToString(), "�� ���ӿ�����Ʈ�� HumanUnitBase ������Ʈ�� �������� �ʽ��ϴ�.");

                break;
            case ("biological", _):
                Hack.Error(this.GetType().ToString(), "�� �� ���� ���� ����");
                break;
            case (MechanicPartBase.MechanicType.DEFAULT, _):
                Hack.Error(this.GetType().ToString(), "�̱���");
                break;
            default:
                Hack.Error(this.GetType().ToString(), "�� �� ���� ����");
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
