using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMakerChildRay : MonoBehaviour
{

    RaycastHit[] RayHits;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootLight(Vector3 direction) // 호출되지 않음
    {
        // 퍼블릭 함수를 호출할때마다 모든 오브젝트를 관통시키는 레이를 발사시키고
        // 충돌했다면 부모 오브젝트의 라이트메이커 컴포넌트의 ChildTriggerEnter 함수를 호출시킨다.
        // 차일드 트리거 엔터 함수를 호출한 지 0.1초 후에 OnTriggerExit 함수를 호출시킨다.

        // 퍼블릭 함수를 호출할때마다 모든 오브젝트를 관통시키는 레이를 발사시키고
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = direction;
        RayHits = Physics.RaycastAll(ray.origin, ray.direction, ray.direction.magnitude, 1 << LayerMask.NameToLayer("Unit"));
        Debug.DrawRay(transform.position, direction, Color.magenta, 0.05f);

        // 충돌했다면 부모 오브젝트의 라이트메이커 컴포넌트의 ChildTriggerEnter 함수를 호출시킨다.
        LightMaker lightMaker = transform.parent.GetComponent<LightMaker>();
        for (int rayHitsIndex = 0; rayHitsIndex < RayHits.Length; rayHitsIndex++)
        {
            lightMaker.ChildTriggerEnter(RayHits[rayHitsIndex].collider);
        }

        // 차일드 트리거 엔터 함수를 호출한 지 0.1초 후에 OnTriggerExit 함수를 호출시킨다.
        //IEnumerator coroutine;
        //coroutine = TakeOverUnitLightCount(RayHits);

        StartCoroutine(TakeOverUnitLightCount(RayHits));
    }

    IEnumerator TakeOverUnitLightCount(RaycastHit[] hits)
    {
        //Debug.Log("TakeOverUnitLightCount 첫번째");
        yield return new WaitForSeconds(0.1f);
        //yield return new WaitForSeconds(1);
        //Debug.Log("TakeOverUnitLightCount 두번째");

        LightMaker lightMaker = transform.parent.GetComponent<LightMaker>();
        for (int rayHitsIndex = 0; rayHitsIndex < hits.Length; rayHitsIndex++)
        {
            lightMaker.ChildTriggerExit(hits[rayHitsIndex].collider);
        }
    }
}
