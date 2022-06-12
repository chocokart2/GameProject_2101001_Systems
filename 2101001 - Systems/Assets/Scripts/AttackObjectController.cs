using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectController : MonoBehaviour
{
    // AttackObject는 자신의 어텍 클래스 데이터를 목표물의 유닛 베이스에 넘겨주기 위한 매개체 역할을 합니다.

    public GameManager.AttackClass property; // 어텍 클래스의 정보.
    public float speedLostPerSecond = 0; // 초당 속도 손실량.
    // 만약 HumanUnitBase와 충돌하게 된다면
    // 어디에 맞았는지 체크를 하고 -> UnitBase에 넘겨줍니다.
    // 화학 반응을 하는지도 체크해야 겠어요. -> UnitBase에 넘겨줍니다.

    // + Volume을 어떻게 처리할 지

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (property.speed > 0.0f) property.speed -= speedLostPerSecond * Time.deltaTime;
        // 프로퍼티에 시간이 지날 때마다 자신의 속도를 잃도록 할 것입니다.
    }

    // 타 컴포넌트가 이 함수를 호출하면 유닛에게 정보를 전달합니다.
    public void Attack(UnitBase target)
    {

    }
}
