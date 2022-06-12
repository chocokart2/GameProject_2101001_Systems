using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransmission : MonoBehaviour
{
    public List<EnergyNetElement> EnergyNet;
    public GameObject EnergyTransmissionRangeGO;
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
        EnergyTransmissionRangeInstantiate = Instantiate(EnergyTransmissionRangeGO, myTransform.position, Quaternion.identity);
        EnergyTransmissionRangeInstantiate.GetComponent<EnergyTransmissionRange>().thatEnergyTransmission = gameObject; // 오류 가능성
    }

    // Update is called once per frame
    void Update()
    {
        //EnergyTransmissionRangeInstantiate.GetComponent<EnergyTransmissionRange>().myTransform.position = myTransform.position;
        EnergyTransmissionRangeInstantiate.GetComponent<EnergyTransmissionRange>().thatEnergyTransmission = gameObject;
    }
}
