using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region field

    #region System
    GameManager gm;


    #endregion

    World world;



    #region GameObject
    public GameObject DistrictIcon;

    #endregion






    #endregion

    #region DataType
    

    public class World
    {
        public string worldName;
        public Dictionary<string, District> Area;

        public WorldData ToData()
        {
            WorldData returnValue = new WorldData();
            
            returnValue.worldName = this.worldName;
            
            returnValue.DistrictDataKey = new string[Area.Count];
            
            Area.Keys.CopyTo(returnValue.DistrictDataKey, 0);
            
            returnValue.DistrictDataValue = new DistrictData[Area.Count];
            District[] tempDistricts = new District[Area.Count];
            Area.Values.CopyTo(tempDistricts, 0);
            for (int index = 0; index < Area.Count; index++)
            {
                returnValue.DistrictDataValue[index] = tempDistricts[index].ToData();
            }

            return returnValue;
        }
        public void SetData(WorldData worldData)
        {
            // worldData의 값을 필드 변수에 넣습니다.
        }
    }
    public class District
    {
        public string name; // 이 District의 Key값
        public List<string> adjacentArea; // 연결된 지역의 Key값
        public string sceneName;
        public Vector3 IconPosition;

        public DistrictData ToData()
        {
            DistrictData returnValue = new DistrictData();
            returnValue.name = this.name;
            returnValue.adjacentArea = new string[adjacentArea.Count];
            adjacentArea.CopyTo(returnValue.adjacentArea);
            returnValue.sceneName = this.sceneName;
            returnValue.IconPosition = this.IconPosition;

            return returnValue;
        }
    }


    [System.Serializable]
    public class Header // JSON 파일의 Path를 가리킵니다.
    {
        public string WorldPath;

        public string[] UnitPath;
        public string[] SquadPath;
        public string[] TeamPath;

        // 이 값은 지워도 될 것 같습니다 -> 캠페인 여부 ,월드 이름이랑, 파일 형식, 파일이름만 알면 될 것 같은데
        //public string ChemicalList;
        //public string ChemicalUsage;
        //public string ChemicalPosition;
        //public string ChemicalReactionList;
    }

    [System.Serializable]
    public class WorldData
    {
        public string worldName;
        public string[] DistrictDataKey;
        public DistrictData[] DistrictDataValue;

    }
    [System.Serializable]
    public class DistrictData
    {
        public string name; // 이 District의 Key값
        public string[] adjacentArea; // 연결된 지역의 Key값
        public string sceneName;
        public Vector3 IconPosition;
    }
    #endregion

    #region func
    void WorldLoader()
    {
        // 전장 지도를 로딩하는 역할을 합니다.
        // 템프 폴더에서 데이터를받고
        // 루프문으로 각 디스트릭트를 로딩합니다.



        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        world = new World();
        if (!GameManager.DataLoading("Assets/GameFile/TempData/World/World.json", ref world))
        {
            Debug.Log("<!> ERROR_WorldManager.WorldLoader() : DataLoading에 들어간 파일 경로가 올바르지 않습니다.");
        }

        for (int index = 0; index < world.Area.Count; index++)
        {
            List<District> loadValue = new List<District>(world.Area.Values);
            GameObject instantiatedObject = Instantiate(DistrictIcon, loadValue[index].IconPosition, Quaternion.identity);
            instantiatedObject.GetComponent<DistrictButtonController>().DistrictName = loadValue[index].name;
        }
    }

    void WorldSaver()
    {
        




    }

    //void 


    #endregion



}
