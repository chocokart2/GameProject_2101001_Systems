using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    public GameManager.AttackClass myAttackClass;
    bool AttackClassSet = false;
    // Start is called before the first frame update
    void Start()
    {
        //myAttackClass = new GameManager.AttackClass();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
        {
            if (other.gameObject.GetComponent<UnitBase>().unitBaseData.unitType == "human")
            {
                //other.gameObject.GetComponent<HumanUnitBase>();
            }
            else
            {
                // 기계 유닛에게 접근합니다.
            }
        }
    }

    public void Attack(Collider other)
    {
        other.gameObject.GetComponent<UnitBase>().beingAttacked(transform.position, myAttackClass);
    }

    public void Set(GameManager.AttackClass attackClass)
    {
        if(AttackClassSet == false)
        {
            this.myAttackClass = attackClass;
            AttackClassSet = true;
        }
    }
    //
}
