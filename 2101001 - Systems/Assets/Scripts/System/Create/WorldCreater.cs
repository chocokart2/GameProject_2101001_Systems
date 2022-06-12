using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    static public void Yee()
    {
        Debug.Log("Tee");
    }

    static public void CreateWorld(string path, string scenarioName)
    {
        // 시나리오의 데이터를 따라 데모 월드를 만듭니다. 폴더를 가리키고 있어야 합니다 끝에 /가 붙으면 안됩니다.

        // path를 따라서 만듭니다.


        // 1. scenarioName을 참고하여 JSON파일이 있는 경로를 만듭니다.
        // 2. 해당하는 파일의 인스턴스를 만들고. 생성자를 호출합니다.
        // 3. 게임매니저를 통해 파일 로드를 호출합니다(1번과 2번을 매개변수로)
        // 4.A. 파일이 로딩되지 않으면 오류 메시지 찔끔 흘리고 리턴합니다. 어느 함수에 문제가 생겼는지 흘립니다.
        // 4.B. 파일이 로딩되었으면 로딩한 인스턴스를 약간의 가공을 거친 뒤, path에 올립니다.
        // 4.B.A. 월드 파일은 그냥 냅다 올리면 됩니다.


        Debug.Log("WorldCreater : 시나리오에서 월드를 복사하고 있습니다.");
        // World 폴더 참조
        string WorldLoadPath = string.Format("Assets/GameFile/Scenario/{0}/World/World_{0}.json", scenarioName);
        WorldManager.WorldData worldData = new WorldManager.WorldData();
        if(GameManager.DataLoading(WorldLoadPath, ref worldData) == false)
        {
            Debug.Log("WorldCreater.CreateWorld 에서 오류 발생");
            return;
        }
        else
        {
            //Debug.Log("파일 로딩 완료");

            string WorldSavePath = string.Format("{0}/World/World_{1}.json", path, scenarioName);
            GameManager.DataSaving(worldData, WorldSavePath);
        }

        // 아이템 생성


        #region ChemicalCreate
        // 캐미컬 생성
        Debug.Log("WorldCreater : 화학물질을 생성하고 있습니다.");

        string ChemicalLoadPath = string.Format("Assets/GameFile/Scenario/{0}/Chemicals/ChemicalSeed_{0}.json", scenarioName);
        ChemicalManager.ChemicalSeed chemicalSeedData = new ChemicalManager.ChemicalSeed();
        if (GameManager.DataLoading(ChemicalLoadPath, ref chemicalSeedData) == false)
        {
            Debug.Log("WorldCreater.CreateWorld 에서 오류 발생");
            return;
        }
        else
        {
            ChemicalManager.elementList GeneratedChemical = new ChemicalManager.elementList();
            GeneratedChemical = ChemicalManager.ChemicalMake(20, ref chemicalSeedData);

            string ChemicalListSavePath = string.Format("{0}/Chemicals/ChemicalList_{1}.json", path, scenarioName);
            GameManager.DataSaving(GeneratedChemical, ChemicalListSavePath);
        }
        Debug.Log("WorldCreater : 화학물질을 생성하고 있습니다. - done");

        // 캐미컬 용도 생성


        // 캐미컬의 위치 생성


        // 캐미컬 반응 생성
        #endregion
        // 유닛 생성



        // 업그레이드 생성
    }



    // 만약에 CreateWorld에 패턴이 발견된다면 이 함수로 간결하게 만듭시다.
    void CreateWorld_Load()
    {

    }
}
