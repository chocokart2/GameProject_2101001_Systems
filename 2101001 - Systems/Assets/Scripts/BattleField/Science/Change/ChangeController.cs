using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning ���� �� �� : ChemicalHelper �ű�� -> ChemicalReaction �̻��ϱ� / EnergyResist �̻��ϱ�

public class ChangeController : ChangeHelper
{
#warning ChemicalReactionArray�� ChemicalHelper�� �ƴ϶� ChangeHelper�� �̻��ŵ�ô�. �������� �ִµ� ȭ�� ������ �ʿ䰡 ������
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
        // reactions�� energyResists�� �ν��Ͻ�ȭ�մϴ�.`
        // �̷� �ϵ� �ڵ��� ���� ��ģ ���̾�. ������ ���� ������� �ϼ��ɶ����� �� ���ƾ߰ھ�.

        energyResists = new ChemicalEnergyResistTable();
        ChemicalEnergyResist chemicalResist_TEST_ATP = new ChemicalEnergyResist();
        #region init about chemicalResist_TEST_ATP
        chemicalResist_TEST_ATP.ChemicalName = "TEST_ATP";
        chemicalResist_TEST_ATP.Add(new EnergyResist() { energyType = "A", resistanceDefense = 0.0f, resistanceRatio = 3.0f });
        #endregion
        energyResists.Add(chemicalResist_TEST_ATP);
    }
}
