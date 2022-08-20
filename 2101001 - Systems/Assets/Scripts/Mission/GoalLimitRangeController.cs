using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLimitRangeController : MonoBehaviour
{
    // 


    // Start is called before the first frame update
    void Start()
    {
        // 자신의 랜더링을 끄는 것이다
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
