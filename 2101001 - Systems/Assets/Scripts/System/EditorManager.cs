using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    // Ŭ������ ����
    // �� Ŭ������ ���� ������Ʈ�� �ѿ� �����Ѵٸ� ���ӸŴ����� ������ ���� ����˴ϴ�
    // ����
    // �ν����Ϳ� ��Ʈ���� ������ �� �ִ� ĭ�� ����. -> �� ���� ������ ��� ������ �̸��Դϴ�.
    // 


    // ���� ����� ������
    #region s

    [SerializeField]
    string fileName; // �ν����ͷκ��� ���� �޽��ϴ�. // � �̸����� �����ұ��?


    #endregion

    // UI ��Ʈ
    #region s
    // �߰��ؾ� �� UI
    // ���� ��ư
    // ����[���� ���� �׽�Ʈ ���Դϴ�.]

    [SerializeField]
    GameObject buttonSave;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if(fileName == "")
        {
            fileName = "No_Title";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
