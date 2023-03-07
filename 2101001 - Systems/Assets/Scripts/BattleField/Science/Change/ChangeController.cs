using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning ���� �� �� : ChemicalHelper �ű�� -> ChemicalReaction �̻��ϱ� / EnergyResist �̻��ϱ�

public class ChangeController : 
    ChangeHelper,
    BaseComponent.IDataGetableComponent<(ChangeHelper.ChemicalReactionTable _reactions, ChangeHelper.ChemicalEnergyResistTable _energyResists)>,
    BaseComponent.IDataSetableComponent<(ChangeHelper.ChemicalReactionTable _reactions, ChangeHelper.ChemicalEnergyResistTable _energyResists)>
{
#warning ChemicalReactionArray�� ChemicalHelper�� �ƴ϶� ChangeHelper�� �̻��ŵ�ô�. �������� �ִµ� ȭ�� ������ �ʿ䰡 ������
    static bool isInited = false;

    static private ChemicalReactionTable reactions;
    static public ChemicalReactionTable Reactions
    {
        get { return reactions; }
        private set { reactions = value; }
    }

    static private ChemicalEnergyResistTable energyResists;
    static public ChemicalEnergyResistTable EnergyResists
    {
        get { return energyResists; }
        private set { energyResists = value; }
    }

    public (ChemicalReactionTable _reactions, ChemicalEnergyResistTable _energyResists) GetComponentData()
    {
        return (reactions, energyResists);
    }

    public void SetComponentData((ChemicalReactionTable _reactions, ChemicalEnergyResistTable _energyResists) data)
    {
        if(isInited == false)
        {
            Hack.Say(Hack.isDebugChangeController, Hack.check.info, this, message: "�������� �ʱ�ȭ�� ����˴ϴ�!");

            isInited = true;
            reactions = data._reactions;
            energyResists = data._energyResists;
        }
        else
        {
            Hack.Say(Hack.isDebugChangeController, Hack.check.error, this, message: $"�̹� �ʱ�ȭ�� �� {this.GetType().Name}�� �ʰ�ȭ�� �õ��߽��ϴ�.");
        }
    }
}
