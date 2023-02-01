using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning 오늘 할 거 : ChemicalHelper 옮기기 -> ChemicalReaction 이사하기 / EnergyResist 이사하기

public class ChangeController : ChangeHelper
{
#warning ChemicalReactionArray를 ChemicalHelper가 아니라 ChangeHelper로 이사시킵시다. 에너지도 있는데 화학 반응일 필요가 없겠지
    public ChemicalReactionTable reactions
    {
        get { return reactions; }
        private set { reactions = value; }
    }
    public ChemicalEnergyResistTable energyResists
    {
        get { return energyResists; }
        private set { energyResists = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#warning hardcoding
    public void setData_demo()
    {
        // reactions와 energyResists을 인스턴스화합니다.`
        // 이런 하드 코딩은 정말 미친 짓이야. 하지만 파일 입출력이 완성될때까지 좀 참아야겠어.

        energyResists = new ChemicalEnergyResistTable();
        ChemicalEnergyResist chemicalResist_TEST_ATP = new ChemicalEnergyResist();
        #region init about chemicalResist_TEST_ATP
        chemicalResist_TEST_ATP.ChemicalName = "TEST_ATP";
        chemicalResist_TEST_ATP.Add(new EnergyResist() { energyType = "A", resistanceDefense = 0.0f, resistanceRatio = 3.0f });
        #endregion
        energyResists.Add(chemicalResist_TEST_ATP);
    }
}
