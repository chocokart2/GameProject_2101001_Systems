using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    // 클래스의 설명
    // 이 클래스를 가진 컴포넌트가 싼에 존재한다면 게임매니저가 에디터 모드로 실행됩니다
    // 역할
    // 인스팩터에 스트링을 저장할 수 있는 칸이 있음. -> 이 씬을 저장할 경우 저장할 이름입니다.
    // 


    // 파일 입출력 서포터
    #region s

    [SerializeField]
    string fileName; // 인스펙터로부터 값을 받습니다. // 어떤 이름으로 저장할까요?


    #endregion

    // UI 파트
    #region s
    // 추가해야 할 UI
    // 저장 버튼
    // 문장[현재 씬은 테스트 중입니다.]

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
