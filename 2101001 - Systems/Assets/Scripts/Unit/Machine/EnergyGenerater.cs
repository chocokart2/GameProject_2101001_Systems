using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerater : MonoBehaviour
{
    //public 
    public float energyPerSecond; // 모든 에너지 생산 부위가 표준적으로(오버클럭X) 활성화될때 초당 에너지 생산량입니다.
    public float energyEnvironmental; // 주변 환경의 영향으로 변할 수 있는 값입니다. 최소: 0.0 / 기본: 1.0 / 최대: 1.0.
    //myEnergyGenerater Value
    bool isPartAvailable_energyLine;
    //myGameObject Component
    EnergyStorage myEnergyStorage;


    // Start is called before the first frame update
    void Start()
    {
        energyEnvironmental = 1.0f;
        isPartAvailable_energyLine = true;
        myEnergyStorage = GetComponent<EnergyStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        myEnergyStorage.energy = myEnergyStorage.energy + energyPerSecond * energyEnvironmental * Time.deltaTime;
        if(myEnergyStorage.energy > myEnergyStorage.maxEnergy)
        {
            if (isPartAvailable_energyLine)
            {
                myEnergyStorage.energy = myEnergyStorage.maxEnergy;
            }
        }
    }
}
