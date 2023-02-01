using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturberLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<UnitBase>() != null)
        {
            other.gameObject.GetComponent<UnitBase>().LightEnter(0);
        }
        // 타일 블럭에서도 설정
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<UnitBase>() != null)
        {
            other.gameObject.GetComponent<UnitBase>().LightExit(0);
        }
    }
}
