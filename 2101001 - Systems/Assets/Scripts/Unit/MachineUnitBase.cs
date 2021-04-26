using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUnitBase : MonoBehaviour
{
    public int MachineUnitCode;
    public int battery = 0; // 배터리가 없으면 작동하지 않습니다.
    public GameObject FixedWith;




    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnitBase>().isHuman = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
