using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    //나이프블레이드는 자신의 리지드바디 컴포넌트가 존재해야 합니다.
    // 아무래도 패런트의 트랜스폼을 이용해야 하다 보니까.
    //칼의 위치 = 기본 위치 + 움직임 위치
    // 기본위치 = 플레이어 위치
    // 움직임 위치 = 주입한 방향 * func이동값(시간)

    // 유닛 게임오브젝트와 닿으면:
    // 닿은 오브젝트의 고유 ID가 UserUnitID와 동일한가?
    // 그렇다면 무시
    // 아니라면 데미지 적용

    #region field
    int UserUnitID;
    float time; // 누적 시간
    float lifeTime;
    Vector3 direction;
    Vector3 StartPos;
    #endregion
    public void Init(Vector3 _direction, int _UserUnitID, Vector3 _StartPos)
    {
        direction = _direction;
        UserUnitID = _UserUnitID;
        StartPos = _StartPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        lifeTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        lifeTime -= Time.deltaTime;

        //transform.position = transform.parent.position;
        transform.position += direction * Time.deltaTime * 6 * MoveAnimationValue(time);

        if (lifeTime < 0.0f) Destroy(gameObject);

    }

    float MoveAnimationValue(float time)
    {
        //return (1 - Mathf.Pow(time - 1.0f, 8)) * 0.5f;
        return 0.5f - time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == UserUnitID) return;

        if (other.tag == "Unit")
        {
            if (other.gameObject.GetComponent<UnitBase>().unitBaseData.unitType == "human")
            {
                GetComponent<AttackObject>().Attack(other);
            }
            else
            {
                // 기계 유닛에게 접근합니다.
            }
        }


    }
}
