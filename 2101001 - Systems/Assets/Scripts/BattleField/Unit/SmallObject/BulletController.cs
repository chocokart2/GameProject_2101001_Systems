using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region 필드
    public Vector3 direction;
    public GameObject debuggerGameObject;

    //float activeCooldown = 0.1f;
    int UserUnitID;
    float activeMoveCount = 0.3f;
    float lifetime = 7.0f;
    float speed = 70.0f;
    //float speed = 0.3f; for test
    Transform myTransform;
    //public GameManager.AttackClass myAttackClass;

    public void Init(Vector3 _direction, int _UserUnitID)
    {
        direction = _direction;
        UserUnitID = _UserUnitID;
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();


        //Quaternion rotationValue = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        //myTransform.rotation = rotationValue;
        Quaternion rotationValue = Quaternion.LookRotation(direction, Vector3.forward);
        myTransform.rotation = rotationValue;
        myTransform.Rotate(90, 0, 0);

        //Debug.Log(direction);
        //myTransform.LookAt(direction + new Vector3(0,1,0));
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        transform.position += direction * speed * Time.deltaTime;
        activeMoveCount -= speed * Time.deltaTime;
        //if (activeCooldown > 0.0f)
        //{
        //    activeCooldown -= Time.deltaTime;
        //}
        if(lifetime > 0.0f)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject, 0.02f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter 호출됨 1");

        if (other.gameObject.GetInstanceID() == UserUnitID) return;
        //if (activeMoveCount > 0.0f) return;
        //Debug.Log("OnTriggerEnter 호출됨 2");

        // Debug.Log("Yee! " + other.tag + ", " + other.gameObject.name + ", " + other.gameObject.layer);
        //Instantiate(debuggerGameObject, myTransform.position, Quaternion.identity);


        //if (activeCooldown > 0.0f) return;

        if (other.tag == "Unit")
        {
            Debug.Log("Unit Hit.");


            if (other.gameObject.GetComponent<UnitBase>().unitBaseData.unitType == "human")
            {
                GetComponent<AttackObject>().Attack(other);
                //GameManager.AttackClass attackClass = GetComponent<AttackObject>().myAttackClass;
                //other.gameObject.GetComponent<UnitBase>().beingAttacked(myTransform.position, attackClass);
                Destroy(gameObject, 0.02f);
            }
            else
            {
                // 기계 유닛에게 접근합니다.
            }

            Destroy(gameObject, 0.02f);
        }
        if(other.tag == "Visible")
        {
            Debug.Log("Yee!!! ");

            Destroy(gameObject);
        }


    }

    void UnitEffect()
    {

    }
}
