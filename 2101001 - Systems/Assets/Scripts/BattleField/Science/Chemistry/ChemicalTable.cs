using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalTable : MonoBehaviour
{
    public string[] existingChemicalNames;


    // Start is called before the first frame update
    void Start()
    {
        existingChemicalNames = makeDemoChemicalTable();
    }

    private string[] makeDemoChemicalTable()
    {
        return new string[]
        {
            "TEST_organicCarbon",
            "TEST_ATP"
        };


    }
}
