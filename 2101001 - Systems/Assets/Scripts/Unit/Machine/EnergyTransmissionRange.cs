using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransmissionRange : MonoBehaviour
{
    public Transform myTransform;
    public Transform machineTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = machineTransform.position;
    }
}
