using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : AttackClassHelper
{
    // 필드
    public AttackInfo attackInfo;
    // 공격 방향을 정의합니다.
    public Vector3 direction; bool isNullDirection = true;
    
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
        if(isNullDirection == true)
        {
            Debug.Log("<!>WARNING_AttackObject.Attack() : 이 공격체의 공격 방향은 정해지지 않았습니다!");
        }
        other.gameObject.GetComponent<UnitBase>().beingAttacked(transform.position, direction, attackInfo);
    }

    public void Set(AttackInfo _attackInfo)
    {
        if(AttackClassSet == false)
        {
            attackInfo = _attackInfo;
            AttackClassSet = true;
        }
        else Debug.Log("WARNING_AttackObject.Set() 이미 초기화된 대상입니다.");
    }
    public void Set(AttackInfo _attackInfo, Vector3 _direction)
    {
        if (AttackClassSet == false)
        {
            attackInfo = _attackInfo;
            direction = _direction; isNullDirection = false;
            AttackClassSet = true;
        }
        else Debug.Log("WARNING_AttackObject.Set() 이미 초기화된 대상입니다.");
    }

    //
}
