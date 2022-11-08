using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectController : MonoBehaviour
{
    [SerializeField] GameObject BorderQuad;


    // Start is called before the first frame update
    void Start()
    {
        // �ӽ÷� ������
        ReadyObject();
    }

    public void ReadyObject()
    {
        #region �̰� ���ϴ� �����ΰ�?
        // ���ӸŴ����� ã�ư��� �ش� �̼��� ���� �˾Ƴ�
        // - ���⿣ �ӽ÷� ������ �������̶�� ��
        // ���� ���� ���� ��ǥ�̸� Ȱ��ȭ�� ��


        #endregion

        Transform[] transforms = FindEdge();
        if (transforms.Length >= 3)
        {
            // ���� ��� ����

            // ù��°�� �������� �մ�
            InstantiateQuad(transforms[0].position, transforms[transforms.Length - 1].position);
            for(int index = 0; index < transforms.Length - 1; index++)
            {
                InstantiateQuad(transforms[index].position, transforms[index + 1].position);
            }
        }
        else
        {
            // ���� ��� ����
        }
    }

    // �ϼ��� �Լ�
    Transform[] FindEdge() // �ڽ� ���ӿ�����Ʈ�߿��� Edge�� ã�� �迭�� �����մϴ�. 
    {
        #region �̰� ���ϴ� �Լ��ΰ�?
        // �ڽ��� �ڽ� ���ӿ�����Ʈ�߿���
        // Limit ���ο� �����ϸ鼭 // �Ÿ��� Scale���� ������ �Ǵ��ϸ� �˴ϴ�.
        // Edge�� // ������ �ڽ� ���ӿ�����Ʈ�� ������Ʈ EdgeController�� �����ϰ� �ִ°��� �Ǵ��ϸ� �ȴ�.
        // 

        // �� �Լ��� ���� ���� : 3�� �̻��̸� �ٰ��� ��ǥ�� �Ǵ��մϴ�.
        #endregion

        Transform limit = transform.Find("MissionLimitRange");
        if (limit == null)
        {
            Debug.LogError("<!>_MissionObjectController : " + transform.name + "�� MissionLimitRange�� �������� �ʽ��ϴ�");
            return null;
        }

        List<Transform> returnValue = new List<Transform>();

        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            Transform selected = transform.GetChild(childIndex);

            // ���� ������ child�� Edge�� �ƴϸ� �������� �Ѿ�ϴ�.
            if (selected.GetComponent<EdgeController>() == null) continue;

            // ���� ������ child�� Limit�� �Ѿ�ٸ� �������� �Ѿ�ϴ�.
            if (Vector3.Distance(selected.position, limit.position) > limit.localScale.x) // ���͸� �̿��ؼ� �ڽ��� ��ġ-limit �߽ɰ� �Ÿ��� limit�� scale/2���� ũ�ٸ� �������� �Ѿ�ϴ�.
                continue;

            returnValue.Add(selected);
        }

        return returnValue.ToArray();
    }
    void InstantiateQuad(Vector3 pos1, Vector3 pos2) // �� ������ �޾� �� ������ �غ����� �ϴ� Quad�� �ν��Ͻ�ȭ�մϴ�.
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
