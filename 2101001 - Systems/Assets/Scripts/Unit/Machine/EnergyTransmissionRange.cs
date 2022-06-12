using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransmissionRange : MonoBehaviour
{
    public Transform myTransform;
    //public Transform machineTransform;
    public GameObject thatEnergyTransmission;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = thatEnergyTransmission.GetComponent<Transform>().position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnergyTransmission>() != null)
        {
            Debug.Log("한 트랜스미션 디바이스가 들어옴: " + other.GetComponent<Transform>().position);
            try
            {
                //Debug.Log(myTransform.position);
                if(other.GetComponent<Transform>().position == myTransform.position)
                {
                    Debug.Log("자신의 EnergyTransmission을 감지했습니다.");
                }
            }
            catch (UnassignedReferenceException)
            {
                Debug.Log("myTransform.position 미할당 오류");
            }
        }
        else if(other.GetComponent<EnergyStorage>() != null)
        {
            Debug.Log("한 에너지 스토리지 디바이스가 들어옴");

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnergyTransmission>() != null)
        {
            
            Debug.Log("한 트랜스미션 디바이스가 나감");

        }
        else if (other.GetComponent<EnergyStorage>() != null)
        {
            Debug.Log("한 에너지 스토리지 디바이스가 나감");

        }
    }
}
