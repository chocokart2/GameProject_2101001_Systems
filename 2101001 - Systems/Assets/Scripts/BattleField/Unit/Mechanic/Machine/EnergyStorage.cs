﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStorage : MonoBehaviour
{
    public float maxEnergy = 100;
    public float energy;
    public int energyPriority; // 에너지의 우선순위


    // Start is called before the first frame update
    void Start()
    {
        energy = 0.0f;
        //감지망 생성 코드
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
