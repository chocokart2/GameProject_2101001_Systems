using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransmission : MonoBehaviour
{
    public List<EnergyNetElement> EnergyNet;
    public GameObject EnergyTransmissionRange;
    GameObject EnergyTransmissionRangeInstantiate;
    Transform myTransform;



    public class EnergyNetElement
    {
        GameObject linkedGameObject;
        List<EnergyTransmission> linkedBy; // 어떤 에너지전달탑과 연결되었나
    }


    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        EnergyTransmissionRangeInstantiate = Instantiate(EnergyTransmissionRange, myTransform.position, Quaternion.identity);
        EnergyTransmissionRangeInstantiate.GetComponent<EnergyTransmissionRange>().machineTransform = myTransform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
