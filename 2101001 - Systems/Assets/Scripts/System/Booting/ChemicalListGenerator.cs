using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalListGenerator : MonoBehaviour
{
    // 여러 name을 조합하여 하나의 이름을 만듭니다.




    void Start()
    {
        GenerateChemicals(16, "Busan");
    }

    #region field
    public string[] name1 = { "mono", "di", "tri", "tetra", "Pyoleemly", "null", "null", "null" };
    public string[] name2 = { "byron", "solon", "pascal", "rousseau", "locke", "null", "null", "null" };
    public string[] name3 = { "name1", "name2", "name3", "name4", "name5", "name6", "name7", "name8" };

    List<string> GeneratedNameList;
    #endregion


    [System.Serializable]
    public class ChemicalList
    {
        public string[] matterList;
    }








    void GenerateChemicals(int CountForChemicals, string worldName)
    {
        Debug.Log("무작위 화학물질을 생성하고 있습니다");
        GeneratedNameList = new List<string>();

        for (int chemicalIndex = 0; chemicalIndex < CountForChemicals; chemicalIndex++)
        {
            // 랜덤으로 이름을 생성합니다
            string generatedName = name1[UnityEngine.Random.Range(0, name1.Length - 1)];
            generatedName += name2[UnityEngine.Random.Range(0, name2.Length - 1)];
            generatedName += name3[UnityEngine.Random.Range(0, name3.Length - 1)];

            //Debug.Log("생성된 이름: " + generatedName);
            GeneratedNameList.Add(generatedName);
        }

        ChemicalList chemicalList = new ChemicalList();
        chemicalList.matterList = new string[CountForChemicals];
        GeneratedNameList.CopyTo(chemicalList.matterList);

        //GameManager.DataSaving(chemicalList, "Chemicals", "Chemicals_List", worldName);
    }

    void GenerateChemicalUsage()
    {
        // Bio에서 필요한 물질은 1~3입니다.
        // 1개 ~ 3개
        





        //weapon 재료는 외부에서 알아서 정의합니다.
        //
    }

    void GenerateChemicalReaction()
    {
        // 화학물질의 용도를 참고하여 화학물질을 제작합니다.
    }
}
