using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensorChildCylinder : MonoBehaviour
{
    Dictionary<int, Collider> DetectedUnits; // 시야 범위의 물리엔진 충돌로 인해 등록되는 유닛의 목록입니다. 키 값은 인스턴스 아이디입니다.
    public float Scale = 1.0f;
    Vector3 centerPosition;

    // Start is called before the first frame update
    void Start()
    {
        DetectedUnits = new Dictionary<int, Collider>();
        transform.localScale = new Vector3(Scale, 5, Scale);
        centerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        List<int> DetectedUnitsKeys = new List<int>(DetectedUnits.Keys);

        for (int index = 0; index < DetectedUnitsKeys.Count; index++)
        {
            //Debug.Log("DEBUG_LightSensorChildCylinder.Update: " + DetectedUnitsKeys[index] + " " + DetectedUnits[DetectedUnitsKeys[index]]);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.StartsWith("MachinePlacement") || other.name.StartsWith("Human")) // 인간 혹은 머신 (설치) 유닛을 감지합니다.
        {
            // 설치 유닛이라면 이 유닛이 은신중인지 여부도 판단합니다.
            if (DetectedUnits.ContainsKey(other.gameObject.GetInstanceID()) == false)
            {
                DetectedUnits.Add(other.gameObject.GetInstanceID(), other);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // <!> 실험 요구 <!>
        // 여기에 들어가는 값은 DetectedUnit의 Collider 값이 실시간으로 변경되는지 알아봐야 합니다.
        // 만약에 Collider값의 좌표값이 변하지 않는다면 
        // Update함수에서 DetectedUnit의 Collider의 Transform.position값을 불러와라고 요구하면 값이 실시간으로 변하는지 알 수 있습니다.


        // 머신 placement로 시작하거나, 인간 유닛일경우에만 해당
        if (other.name.StartsWith("MachinePlacement") || other.name.StartsWith("Human"))
        {
            
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.StartsWith("MachinePlacement") || other.name.StartsWith("Human"))
        {
            DetectedUnits.Remove(other.gameObject.GetInstanceID());
        }
    }

    public bool tryDetect(ref Collider[] recvValue)
    {
        if(DetectedUnits.Count > 0)
        {
            recvValue = new List<Collider>(DetectedUnits.Values).ToArray();
            return true;
        }
        return false;
    }

    public void SetAngle(Vector3 direction)
    {
        // 특정한 방향으로 라이트를 설정하도록 요구합니다.
        // 방향이 미지정 되어 있으면 원점에서 퍼지도록 요구합니다

        if(direction == new Vector3(0, 0, 0))
        {
            transform.position = centerPosition;
        }
        else
        {
            transform.position = centerPosition + direction.normalized * Scale;
        }



    }
}
