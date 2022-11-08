using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectController : MonoBehaviour
{
    [SerializeField] GameObject BorderQuad;


    // Start is called before the first frame update
    void Start()
    {
        // 임시로 설정됨
        ReadyObject();
    }

    public void ReadyObject()
    {
        #region 이게 뭐하는 물건인고?
        // 게임매니저를 찾아가서 해당 미션이 뭔지 알아냄
        // - 여기엔 임시로 무조건 점령전이라고 함
        // 만약 영토 관련 목표이면 활성화가 됨


        #endregion

        Transform[] transforms = FindEdge();
        if (transforms.Length >= 3)
        {
            // 쿼드 경계 생성

            // 첫번째와 마지막을 잇는
            InstantiateQuad(transforms[0].position, transforms[transforms.Length - 1].position);
            for(int index = 0; index < transforms.Length - 1; index++)
            {
                InstantiateQuad(transforms[index].position, transforms[index + 1].position);
            }
        }
        else
        {
            // 구형 경계 생성
        }
    }

    // 완성된 함수
    Transform[] FindEdge() // 자식 게임오브젝트중에서 Edge를 찾아 배열로 리턴합니다. 
    {
        #region 이게 뭐하는 함수인고?
        // 자신의 자식 게임오브젝트중에서
        // Limit 내부에 존재하면서 // 거리가 Scale보다 작은지 판단하면 됩니다.
        // Edge인 // 조건은 자식 게임오브젝트가 컴포넌트 EdgeController를 보유하고 있는가를 판단하면 된다.
        // 

        // 이 함수를 쓰는 이유 : 3개 이상이면 다각형 목표로 판단합니다.
        #endregion

        Transform limit = transform.Find("MissionLimitRange");
        if (limit == null)
        {
            Debug.LogError("<!>_MissionObjectController : " + transform.name + "의 MissionLimitRange가 존재하지 않습니다");
            return null;
        }

        List<Transform> returnValue = new List<Transform>();

        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            Transform selected = transform.GetChild(childIndex);

            // 현재 선택한 child가 Edge가 아니면 다음으로 넘어갑니다.
            if (selected.GetComponent<EdgeController>() == null) continue;

            // 현재 선택한 child가 Limit를 넘어간다면 다음으로 넘어갑니다.
            if (Vector3.Distance(selected.position, limit.position) > limit.localScale.x) // 벡터를 이용해서 자신의 위치-limit 중심과 거리가 limit의 scale/2보다 크다면 다음으로 넘어갑니다.
                continue;

            returnValue.Add(selected);
        }

        return returnValue.ToArray();
    }
    void InstantiateQuad(Vector3 pos1, Vector3 pos2) // 두 지점을 받아 그 지점을 밑변으로 하는 Quad를 인스턴스화합니다.
    {
        Vector3 position = new Vector3((pos1.x + pos2.x) / 2, 2, (pos1.z + pos2.z) / 2);
        Vector3 scale = new Vector3(
            Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.z - pos2.z, 2)), 3, 1);
        Quaternion quaternion = Quaternion.Euler(0,
            Mathf.Atan2(pos2.x - pos1.x, pos2.z - pos1.z), 0);
        GameObject go = Instantiate(BorderQuad, position, Quaternion.identity);
        go.transform.localScale = scale;
        go.transform.Rotate(0, Mathf.Atan2(pos2.z - pos1.z, -pos2.x + pos1.x) * Mathf.Rad2Deg, 0);
        //go.transform.Rotate(0, 30, 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
