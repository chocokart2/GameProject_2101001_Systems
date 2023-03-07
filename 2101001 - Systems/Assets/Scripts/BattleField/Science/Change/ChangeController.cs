using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning 오늘 할 거 : ChemicalHelper 옮기기 -> ChemicalReaction 이사하기 / EnergyResist 이사하기

public class ChangeController : 
    ChangeHelper,
    BaseComponent.IDataGetableComponent<(ChangeHelper.ChemicalReactionTable _reactions, ChangeHelper.ChemicalEnergyResistTable _energyResists)>,
    BaseComponent.IDataSetableComponent<(ChangeHelper.ChemicalReactionTable _reactions, ChangeHelper.ChemicalEnergyResistTable _energyResists)>
{
#warning ChemicalReactionArray를 ChemicalHelper가 아니라 ChangeHelper로 이사시킵시다. 에너지도 있는데 화학 반응일 필요가 없겠지
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
            Hack.Say(Hack.isDebugChangeController, Hack.check.info, this, message: "정상적인 초기화가 진행됩니다!");

            isInited = true;
            reactions = data._reactions;
            energyResists = data._energyResists;
        }
        else
        {
            Hack.Say(Hack.isDebugChangeController, Hack.check.error, this, message: $"이미 초기화가 된 {this.GetType().Name}에 초가화를 시도했습니다.");
        }
    }
}
