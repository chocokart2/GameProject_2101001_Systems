using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensorChildRay : MonoBehaviour
{
    Collider Detected;
    Collider[] DetectedUnits;
    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool LightSense(Vector3 direction, ref Collider[] colliders)
    {
        // 레이를 쏘고
        // 개발자 노트
        // 레이캐스트 올을 사용하고
        // 각 부딛힌 것들의 거리를 체크합니다.
        // 최소 거리를 넘어서는 오브젝트중에 1번째로 가까운 대상을 지목합니다.
        Debug.Log("DEBUG_LightSensorChildRay.LightSense: It Worked!");
        float MinimumDistance = 0.5f;

        // 레이를 쏘고 
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = direction;

        RaycastHit[] rayHits;
        rayHits = Physics.RaycastAll(ray, 100.0f, 1 << LayerMask.NameToLayer("Unit"));
        // DetectedUnits 거리를 통해 소팅합니다. 작은게 가장 먼저.

        List<RaycastHit> rayHitsList = new List<RaycastHit>(rayHits);
        rayHitsList.Sort(delegate (RaycastHit x, RaycastHit y)
        {
            if (x.distance < y.distance) return -1;
            else if (x.distance > y.distance) return 1;
            else return 0;
        });

        for(int rayHitsListIndex = 0; rayHitsListIndex < rayHitsList.Count; rayHitsListIndex++)
        {
            if (rayHitsList[rayHitsListIndex].distance < MinimumDistance) continue;

            // 인간을 찾아냅니다.
            if (rayHitsList[rayHitsListIndex].collider.name.StartsWith("Human"))
            {
                colliders = new Collider[] { rayHitsList[rayHitsListIndex].collider };
                return true;
            }

            // 부딛힌 오브젝트 중 머신을 찾아냅니다.
            if (rayHitsList[rayHitsListIndex].collider.name.StartsWith("Machine"))
            {
                colliders = new Collider[] { rayHitsList[rayHitsListIndex].collider };
                return true;
            }

        }

        return false;
    }
}
