using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalSeed_Busan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ChemicalManager.ChemicalSeed seed = new ChemicalManager.ChemicalSeed();
        seed.seed = new ChemicalManager.ChemicalSeedSeed[3];
        seed.seed[0] = new ChemicalManager.ChemicalSeedSeed();
        seed.seed[1] = new ChemicalManager.ChemicalSeedSeed();
        seed.seed[2] = new ChemicalManager.ChemicalSeedSeed();
        seed.seed[0].seed = new string[] { "mono", "di", "tri", "tetra", "Pyoleemly"};
        seed.seed[1].seed = new string[] { "byron", "solon", "pascal", "rousseau", "locke"};
        seed.seed[2].seed = new string[] { "name1", "name2", "name3", "name4", "name5", "name6", "name7", "name8" };

        GameManager.DataSaving(seed, "Chemicals", "ChemicalSeed_Busan", "Busan", "Scenario");
    }
}
