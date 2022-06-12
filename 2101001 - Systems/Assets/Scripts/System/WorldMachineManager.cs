using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMachineManager : MonoBehaviour
{
    #region Event And Delegate
    // Delegate
    public delegate void VoidToVoid();
    public delegate void DataListToVoid(ref List<WorldMachineManagerData> data);

    // Event
    public event DataListToVoid WorkForSave; // 아마 네트워크 커넥터에게 클라이언트 데이터를 채워넣도록 요구할 수 있습니다.

    // EventSetup
    void EventSetup()
    {
        WorkForSave += delegate (ref List<WorldMachineManagerData> INeedMoreGlucose) { };
    }
    #endregion
    #region Field
    List<int> NetworkIdList; //ClientData의 ID값만 빼와서 배열로 만들었습니다.
    // 이벤트 - 네트워크 커넥터에게 데이터 요구하는 함수를 위한.
    #endregion
    #region Class
    [System.Serializable]
    public class WorldMachineManagerData
    {
        public NetworkConnector.NetworkConnectorData[] ClientData;
    }
    #endregion
    #region Function
    #region Public Function
    public void SaveComponent()
    {
        // WorldMachineManagerData를 불러옴

        // 그 값을 리스트로 만들어 이 함수의 지역변수(ReadyValueForSaving)로 만듦
        List<WorldMachineManagerData> readyValueForSaving = new List<WorldMachineManagerData>();

        // 이벤트를 실행시켜 ReadyValueForSaving에 저장 및 갱신시킴
        WorkForSave(ref readyValueForSaving);

        // WorldMachineManagerData로 데이터 변환

        // Json 파일에 저장
    }

    public void LoadComponent() // 이 컴포넌트는 데이터를 로딩합니다. 머신 유닛이 로딩될때 이 함수가 실행되어 있어야 합니다.
    {
        //WorldManager에서 경로를 얻는다.

        // 경로를 이용해서 Json파일이 있을 곳을 찾는다.

        // 직렬화된 데이터를 이 컴포넌트에 로드합니다.
    }

    public void RequestNewID()
    {

    }
    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        EventSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
