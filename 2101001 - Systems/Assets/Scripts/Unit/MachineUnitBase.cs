using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUnitBase : MonoBehaviour
{
    // "MachineUnit은 고유한 기능을 가진 기계부품들의 집합입니다."
    // 이 컴포넌트를 쓰기전 필요한 것
    // <!> isCustomMachineClass을 결정합니다. 커스텀 머신은 0번으로 방치합니다.

    // Start is called before the first frame update
    void Start()
    {
        // 인스턴스화하는 과정에서 isHuman값이 바뀝니다.
        //GetComponent<UnitBase>().isHuman = false;



        Setup();
    }


    #region 이 유니티 컴포넌트 필드
    #region 이 컴포넌트에 필요한 데이터 목록, 유닛을 로드할때 JSON 파일로부터 불러오는 필드들
    public List<BaseMachineComponentData> machineComponentList; // 이 머신 유닛에 붙어있는 세부 컴포넌트들을 나열한 리스트입니다.
    public MachineArmor machineArmor;
    #endregion
    #region 유니티 인스펙터에서 결정해야 하는 값
    public int MachineUnitCode; // 머신 클래스 아이디가 무엇입니까? ex) 803 (DisturberLight을 표현하는 머신 클래스 아이디), 만약 아니라면 0번입니다.

    #endregion
    #region 그외

    #endregion
    public GameObject FixedWith;
    //public int battery = 0; // 배터리가 없으면 작동하지 않습니다. // 삭제됨: 배터리 컴포넌트가 알아서 작동해줍니다.



    // 네트워크용
    public List<MachineNetMessage> RecvMessage; // 외부로부터 들어온 메시지의 목록입니다.
    public MachineNetMessage UsingMessage; // 현재 메시지입니다.
    #endregion
    #region class 목록
    // 이 클래스는 이 컴포넌트의 필드를 담음. 데이터 입출력시 사용.
    public class MachineUnitBaseData
    {
        // "MachineUnit은 고유한 기능을 가진 기계부품들의 집합입니다."
        public BaseMachineComponentData[] machineComponentList;
        public MachineArmorData machineArmorData;

    }


    // 이 데이터 자료형은 이 머신이 특정한 머신 컴포넌트가 존재함을 알리기 위해 존재합니다.
    // UnitItemBase처럼 특별히 상속하거나 하는것은 없습니다. 대신 어떤 머신컴포넌트인가를 알려주는 이름은 TypeName에 넣어야 합니다.
    // 머신에 붙어있는 머신컴포넌트가 작동하려면 게임오브젝트에 연결된 스크립트 컴포넌트에 구현합니다.
    //public class BaseMachineComponent 
    //{
    //    // <!>

    //    public string TypeName; // 어떤 머신컴포넌트 인가? 
    //    // 수리용 데이터
    //    public Vector3 ComponentScale; // 머신의 X,Y,Z 크기 (직육면체)
    //    public Vector3 ComponentPosition; // 이 머신의 위치, x-, y-, z-의 끝쪽
    //    public bool isComponentBroken; // 컴포넌트 파괴됨 여부 (컴포넌트는 아머가 뚫리게 되면 작동되지 않습니다)
    //    public GameManager.chemicals[] ComponentMaterial;
    //}
    #region BaseMachineComponentData

    // 버튼을 만드는데 참고
    // 컴포넌트와 함꼐 작동


    [System.Serializable]
    public class BaseMachineComponentData // HumanUnitBase의 organ과 비슷한 역할을 합니다.
    {
        public string TypeName;
        // 수리용 데이터
        public Vector3 ComponentScale; // 머신의 X,Y,Z 크기 (직육면체)
        public Vector3 ComponentPosition; // 이 머신의 위치, x-, y-, z-의 끝쪽
        public bool isComponentBroken; // 컴포넌트 파괴됨 여부 (컴포넌트는 아머가 뚫리게 되면 작동되지 않습니다)

        public GameManager.chemical[] ComponentMaterial; // 내부에 존재하는 물질, 이물질이 낄 수 있습니다.
        public GameManager.chemical[] ComponentMaterialForFunction; // 작동을 위해 요구하는 물질, 완전히 100%작동하기 위한 물질의 이름과 최대 작동을 위한 최소한의 양이 적혀있어야 합니다. 이 값은 설정되고 나서(기계 설계 후) 변경되면 안됩니다.
    }
    #endregion



    // [컴포넌트:골격과 장갑판]에 도움을 주는 자료형
    public class MachineArmor
    {
        public List<ArmorDamageData> DamageHistory;
        public float armor; // 전체 각도 초기 방어력
        public float tearResistCoefficient; // 찢어짐 저항 계수
        public bool isBreaked; // 파괴됨 여부
    }
    // 데이터 입출력을 위한 클래스
    #region MachineArmorData
    [System.Serializable]
    public class MachineArmorData
    {

        public ArmorDamageData[] DamageHistory;
        public float armor;
        public bool isBreaked;
    }
    #endregion
    [System.Serializable]
    public class ArmorDamageData
    {
        public Vector3 attackedDirection;
        public float Impulse;
    }

    // [컴포넌트: 메모리 칩, 통신 장치, 연결 장치, 통제 장치]
    // 머신 간 NetWorking을 위한 자료형
    public class MachineNetMessage
    {
        // 보내려는 대상 (자신의 머신 클래스들, 다른 머신 클래스 덩어리 혹은 덩어리들)
        public string type; // string 메시지 타입 (정보의 타입 혹은 함수의 이름) swtich구문에서 메시지 처리를 결정할 분기에 사용됩니다. // 예) 감지된 유닛 목록, 이동 명령
        public string[] subDataType; // string[] 서브 데이터 타입(정보라면 필드의 타입, 함수라면 매개변수 타입) 데이터를 처리하는 장소에서 switch구문을 통해 해당하는 자료형의 변수를 만들어서, 쓸수 있도록 만듭니다.
        public string[] subData; // string[] 데이터(정보라면 필드의 데이터, 함수라면 매개변수 데이터)
    }
    // [컴포넌트: 통제 장치]
    [System.Serializable]
    public class MachineControlData
    {
        string[] controlData;


        // 들어가는 값
        // 나가는 값.
    }



    #endregion
    #region 함수 목록
    #region Start도움 함수: Component FieldSetup/ Setup
    void Setup()
    {
        #region machineComponentList의 값을 설정합니다.
        // 골격과 장갑판. 기능은 없어도 이름만 올릴까



        // machineComponentList에 컴포넌트 쓰기
        // 머신 부품으로서 존재하는 유니티 컴포넌트가 있으면 machineComponentList에 컴포넌트 이름을 Add합니다

        // 머신컴포넌트 추가하기
        // 조건 A. MachineUnitCode보고 추가하기 -> unity컴포넌트가 존재하지 않아도 되는 컴포넌트들은 이 조건으로 추가합니다.
        // 조건 B. unity Component가 존재하는지 체크하고 추가하기. -> unity 컴포넌트가 존재하는 머신 컴포넌트는 이 조건으로 추가합니다.

        machineComponentList = new List<BaseMachineComponentData>();
        BaseMachineComponentData tempMachineArmor = new BaseMachineComponentData();
        tempMachineArmor.TypeName = "MachineArmor";
        tempMachineArmor.ComponentPosition = new Vector3(0, 0, 0);
        tempMachineArmor.ComponentScale = new Vector3(0, 0, 0);
        tempMachineArmor.isComponentBroken = false;
        machineComponentList.Add(tempMachineArmor);

        // 머신 클래스 아이디 번호순대로 들어갑니다.
        if (GetComponent<EnergyStorage>() != null)
        {
            BaseMachineComponentData tempMachineComponent = new BaseMachineComponentData();
            tempMachineComponent.TypeName = "EnergyStorage";
            machineComponentList.Add(tempMachineComponent);
        }
        if (GetComponent<EnergyTransmission>() != null)
        {
            BaseMachineComponentData tempMachineComponent = new BaseMachineComponentData();
            tempMachineComponent.TypeName = "EnergyTransmission";
            machineComponentList.Add(tempMachineComponent);
        }
        if (GetComponent<EnergyGenerater>() != null)
        {
            BaseMachineComponentData tempMachineComponent = new BaseMachineComponentData();
            tempMachineComponent.TypeName = "EnergyGenerater";
            machineComponentList.Add(tempMachineComponent);
        }
        if (GetComponent<PlacementCamouflage>() != null)
        {
            BaseMachineComponentData tempMachineComponent = new BaseMachineComponentData();
            tempMachineComponent.TypeName = "PlacementCamouflage";
            machineComponentList.Add(tempMachineComponent);
        }
        if (GetComponent<RotationJoint>() != null)
        {
            BaseMachineComponentData tempMachineComponent = new BaseMachineComponentData();
            tempMachineComponent.TypeName = "RotationJoint";
            machineComponentList.Add(tempMachineComponent);
        }

        if (GetComponent<TargetAimer>() != null)
        {
            BaseMachineComponentData tempMachineComponent = new BaseMachineComponentData();
            tempMachineComponent.TypeName = "TargetAimer";
            machineComponentList.Add(tempMachineComponent);
        }
        if (GetComponent<NetworkConnector>() != null)
        {
            BaseMachineComponentData tempMachineComponent = new BaseMachineComponentData();
            tempMachineComponent.TypeName = "NetworkConnector";
            machineComponentList.Add(tempMachineComponent);
        }
        



        #endregion






    }




    #endregion





    #endregion

    // 내부에 연결된 머신 클래스
    // 연결된 다른 머신 클래스 덩어리











    // net 메시지를 받으면 머신 컨트롤 데이터를 사용하는 컴포넌트가 아닌 이상
    // 자신(그리고 만약에 설치 클래스면 자식에 있는 머신 클래스에 접근함)에 달려있는 머신 클래스 컴포넌트의 함수를 호출합니다.
    // 예를 들어, MachineUnitBase에 "Move(3,5)"라는 메시지를 받으면, 함수 이름을 (Move) 떼어내고,
    // 함수 이름이 Move이니 PlacementDrone 컴포넌트를 찾고 컴포넌트가 있으면 Move 함수를 호출합니다.
    // 아마 Send 함수도 있을거 같은데... 공통적인 함수!


    public void RecvNetMessage(MachineNetMessage netMessage)
    {
        // 외부로부터 강제로 입력받았을때 처리하는것입니다.

        // 넷메시지에 자기 컴포넌트에 해당하는 함수 이름(String)이 있는지 체크합니다.
        // 해당하는 함수가 있으면 GetComponent하고 해당 함수를 호출합니다.

        



        //



        //
    }
    public void SendNetMessage()
    {
        // 메시지
    }


    void SendTo<Component, DataType> (int destinationID, ref DataType sendData)
    {
        //근처 데이터
    }
}
