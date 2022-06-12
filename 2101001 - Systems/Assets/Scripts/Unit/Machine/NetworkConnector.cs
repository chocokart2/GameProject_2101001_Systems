using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConnector : MonoBehaviour
{
    // 클래스 설명
    // 주요 기능
    // 1. 연결된 다른 머신에게 메시지를 보내고, 받은 메시지를 자기 필드에 저장하는것.
    // 2. 컴포넌트와 연결상태 체크하기 - NetworkAntenna
    // 3. GameManager에서 같은 NetworkConnecter와 

    // WorldManager에서 UnitBaseID = WorldManager.GetNewID();
    // ㄴWorldManager에서 마지막에서 +1인 아이디를 생성
    // ㄴ그 아이디를 IDList에 저장
    // ㄴ그 아이디를 리턴합니다.


    #region Field
    // Field에 채워졌는지 여부
    bool isOkayToChennelWork = false; // 만약에 필드에 값이 채워졌으면 true로 변경합니다.

    // 나중에 쓸 정보
    int UnitBaseID; // WorldManager에 저장되는 값입니다.
    int[] ConnectedClientsID; // 채널 0번에 연결된적 있는 네트워크커넥터 데이터에 아이디 값만 모아둔것과 같은데 잘 모르겠음.

    // 필드가 살아 있을 때 쓰는 정보
    public Dictionary<int, bool> isClientsIdConnected; // 이 클라이언트 아이디를 가진 클라이언트가 연결되었는지 여부를 표시합니다. 키값은 아이디고 벨류값은 연결 여부입니다.
    Dictionary<int, List<NetworkConnector>> ClientsChennel; // 네트워크망으로 연결을 약속한 대상들입니다. 키값은 채널 아이디고 벨류값은 커넥터의 목록입니다. 네트워크망으로 연결되지 않을 수 있습니다.
                                                            // ClientsChennel의 값을 사용하기 위해서는 isClientsIdConnected을 반드시 사용해야 합니다.
                                                            // 0인 값은 연결된 적이 있는 모든 클라이언트입니다.

    public int defaultChennel; //ClientsChennel의 기본적인 Key값을 저장합니다. GUI로 변경될 수 있는 값입니다.


    // 수동으로 설정되어야 합니다. NetworkConnector들을 지정하는 패턴을 int 채널을 통해 저장합니다. 하나의 네트워크 커넥터에 여러 채널에 등록될 수 있습니다.
    // 함수 컴포넌트가 없으면 Assigned채널에서만 전달됩니다.


    // ConnectedClientsID
    // 연결된 NetworkConnector들의 UnitBaseID값을 저장하는 배열입니다.
    // WorldManager에서 (int -> ConnectedClientsID) 딕셔너리에 자신의 아이디 값을 키값으로 요청하면 ConnectedClientsID값을 리턴합니다.
    // WorldManager에서 자신의 연결관계를 저장하기 위한 용도입니다.


    #endregion
    #region Class
    public class Client
    {
        public bool isConnected; // ReachedClients에 존재하는 대상입니까?
        public NetworkConnector Component;
    }
    
    [System.Serializable]
    public class NetworkConnectorData
    {
        public int UnitBaseID;
        public ClientsChennelSinglePairData[] chennelData; // Dictionary<int, NetworkConnector[]> ClientsChennel을 만들기 위한 [구조체의 배열]입니다.
        public int defaultChennel;
    }
    [System.Serializable]
    public class ClientsChennelSinglePairData
    {
        public int chennelNumber; // Key
        public int[] clientsUnitInfoID; // Value
    }

    #endregion
    #region Function
    #region Field 채우는 함수
    void FieldSetup()
    {
        UnitBaseID = GetComponent<UnitBase>().unitBaseData.gameManagerId;


        /*
             bool isOkayToChennelWork = false; // 만약에 필드에 값이 채워졌으면 true로 변경합니다.

    // 나중에 쓸 정보
    int UnitBaseID; // WorldManager에 저장되는 값입니다.
    int[] ConnectedClientsID; // 채널 0번에 연결된적 있는 네트워크커넥터 데이터에 아이디 값만 모아둔것과 같은데 잘 모르겠음.

    // 필드가 살아 있을 때 쓰는 정보
    //NetworkConnector[] ReachedClients;
    //public List<NetworkConnector> ReachedClients; // 네트워크망으로 연결 가능한 대상입니다.
    //NetworkConnector[] ConnectedClients; // 네트워크로 연결된 대상입니다. 정보 교류가 활성화 되어 있습니다. 수동으로 연결해 줘야 합니다. 기본적으로 전체로 전달되지만, Function 컴포넌트는 숫자를 입력하면 이 배열의 인덱스로 사용하여 개별적으로 접근할 수 있습니다.
    public Dictionary<int, bool>s isClientsIdConnected; // 이 클라이언트 아이디를 가진 클라이언트가 연결되었는지 여부를 표시합니다. 키값은 아이디고 벨류값은 연결 여부입니다.
    Dictionary<int, List<NetworkConnector>> ClientsChennel; // 네트워크망으로 연결을 약속한 대상들입니다. 키값은 채널 아이디고 벨류값은 커넥터의 목록입니다. 네트워크망으로 연결되지 않을 수 있습니다.
                                                            // ClientsChennel의 값을 사용하기 위해서는 isClientsIdConnected을 반드시 사용해야 합니다.

    public int defaultChennel; //ClientsChennel의 기본적인 Key값을 저장합니다. GUI로 변경될 수 있는 값입니다.
         
         */
    }
    #region ClientsChennel 함수
    void newClientsChennel()
    {
        Debug.Log("DEBUG_NetworkConnector.newClientsChennel: Hello");
        ClientsChennel = new Dictionary<int, List<NetworkConnector>>();
        ClientsChennel.Add(0, new List<NetworkConnector>()); // 모든 채널에 존재하는 모든 네트워크 커넥터, 중복 불허
        ClientsChennel.Add(1, new List<NetworkConnector>()); // 디펄트 채널
    }

    #endregion


    #endregion
    #region 유닛 인스턴스화할때 필요한 함수(플레이어 설치, 게임매니저에서 인스턴스화 진행시 아래 함수를 호출 하시오.)
    //public void


    #endregion
    #region Connecting 함수 / 채널에 등록, 해제
    public void TryChennelConnect(NetworkConnector networkConnector) //(작성 끝, 테스트 필요)
    {
        // 이미 채널에 등록된 NetworkConnector인지 확인합니다. (채널 0에 존재하는지 체크합니다.)
        // ClientsChennel에서 0과 defaultChennel키 NetworkConnector벨류 를 등록합니다.
        // isClientsIdConnected에서 networkConnector.UnitBaseID 키 true밸류 를 등록합니다. 이미 키값이 있는지부터 체크한다음에 넣습니다.
        bool isContains = false;
        for(int index = 0; index < ClientsChennel[0].Count; index++)
        {
            if(ClientsChennel[0][index].UnitBaseID == networkConnector.UnitBaseID) // 동일 유닛인지 여부는 UnitBaseID값을 기준으로 잡습니다.
            {
                isContains = true;
                break;
            }
        }
        Debug.Log("DEBUG_NetworkConnector.TryChennelConnect: Contains 함수 테스트 " + ClientsChennel[0].Contains(networkConnector) + "for 루프 값: " + isContains);
        Debug.Log("DEBUG_NetworkConnector.TryChennelConnect: 아이디 값 " + networkConnector.GetID());

        if(isContains == false)
        {
            ClientsChennel[0].Add(networkConnector);
            ClientsChennel[defaultChennel].Add(networkConnector); // 채널 0번에는 모든 네트워크가 들어서는 곳이므로 0번에 없다면 디펄트 채널에도 없음.
        }


        // isClientsIdConnected is sus
        if(isClientsIdConnected == null)
        {
            Debug.Log("DEBUG_NetworkConnector.TryChennelConnect: isClientsIdConnected is null");
        }

        if (isClientsIdConnected.ContainsKey(networkConnector.UnitBaseID)) // NULL ERROR
        {
            isClientsIdConnected[networkConnector.UnitBaseID] = true;
        }
        else
        {
            isClientsIdConnected.Add(networkConnector.UnitBaseID, true);
        }
    }
    public void ChennelDisconnect(NetworkConnector networkConnector)
    {
        // 채널에 연결 여부는
        int networkConnectorID = networkConnector.UnitBaseID;

        if (isClientsIdConnected.ContainsKey(networkConnectorID))
        {
            isClientsIdConnected[networkConnectorID] = false;
        }
        else
        {
            isClientsIdConnected.Add(networkConnectorID, false);
        }
    }
    public List<NetworkConnector> GetClients(int key)
    {
        if (ClientsChennel == null) return null;
        return ClientsChennel[key];
    }
    // 


    #endregion
    #region Field를 Data화 하는 함수 / Data를 Field화하는 함수


    //Data를 Field화하는 함수는 이벤트로 작용합니다.
    // 1. 모든 통신 유닛은 자신의 식별 아이디값을 보유해야 합니다.
    // hint. 일단 배치되고 나면 tryChennelConnect이 작동할 것이다. 근데 내부 Field는 후에 들어갈 것이며 null 값으로 채워진 컴포넌트를 넘겨줄 수 있다.
    // hint.answer isOkayToChennelWork값을 이용한다.
    // hint. 아이디 값으로 컴포넌트를 불러올 수 있어야 합니다.
    // 
    // 2. 채널 0번에

    #endregion

    #region 퍼블릭 함수: 필드 가져오기
    public int GetID()
    {
        return UnitBaseID;
    }


    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<UnitBase>().unitNeedDefaultData == true) // 디펄트 데이터가 필요합니다.
        {
            newClientsChennel();
            isClientsIdConnected = new Dictionary<int, bool>();
        }


        // 유닛이 배치될때
        // 1. 플레이어가 막 설치한 경우
        // 2. 데이터로 로딩한경우
        // -> 일단 디펄트 값으로 만들고,
        // 1. 플레이어가 설치한 경우, BuildTool에서 네트워크 커넥터 관련 함수를 호출하여 저장된 머신의 특성을 로딩한다
        // 2. 데이터로 로딩한 경우, 게임메니저에서 관련 함수를 호출한다.
        // 결론 -> 데이터 로딩하는 함수가 필요합니다.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendMessage()
    {

    }

    public void UpdateWiredNetwork() // 무선 통신 확인은 다른 컴포넌트가 진행합니다.
    {
        // 연결 가능한 대상 목록을 체크합니다.
        // 자신의 범위 내에서 업데이트를 합니다.

        // 무선이면 -> 콜라이더를 이용해 업데이트
    }

    // GUI로 등록하는 법을 만들어라.
}
