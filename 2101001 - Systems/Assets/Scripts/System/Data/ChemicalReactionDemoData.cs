using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalReactionDemoData : MonoBehaviour
{
    // 사용법
    // 1. 원하는 대로 이 값을 수정해주세요.
    // 2. GameManager에 이것을 끌어놓으세요

    public List<GameManager.ChemicalReaction> chemicalReactions;



    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        chemicalReactions = new List<GameManager.ChemicalReaction>();
        chemicalReactions.Add(new GameManager.ChemicalReaction(
            new GameManager.chemical[] { new GameManager.chemical("testAlpha", 1.0f), new GameManager.chemical("testBeta", 1.0f) },
            new GameManager.chemical[] { new GameManager.chemical("testGamma", 1.0f) },
            20000.0f,
            1.0f
            ));
        chemicalReactions.Add(new GameManager.ChemicalReaction(
            new GameManager.chemical[] { new GameManager.chemical("testAlpha", 1.0f), new GameManager.chemical("testBeta", 1.0f) },
            new GameManager.chemical[] { new GameManager.chemical("testGamma", 1.0f) },
            20001.0f,
            1.0f
            ));
        chemicalReactions.Add(new GameManager.ChemicalReaction(
            new GameManager.chemical[] { new GameManager.chemical("testDnaBase", 1.0f), new GameManager.chemical("testDnaAnti", 1.0f) },
            new GameManager.chemical[] { new GameManager.chemical("testGamma", 0.5f) },
            10100.0f,
            1.0f
            ));

        //의문: 왜 배열을 쓰는 List<T>.CopyTo가 잘 안먹힐까?

        // responsiveness가 낮은값이 먼저 앞으로 가도록 정렬합니다.
        chemicalReactions.Sort(new GameManager.ChemicalReactionComparerByResponsiveness());

        // Debugger
        Hack.Say(Hack.isDebugChemicalReactionDemoData, "DEBUG_ChemicalReactionData.Setup(): 호출됨");
        for(int CrIndex = 0; CrIndex < chemicalReactions.Count; CrIndex++)
        {
            Hack.Say(Hack.isDebugChemicalReactionDemoData, $"{CrIndex + 1}번째 화학반응 테이블");
            foreach(GameManager.chemical oneOfInput in chemicalReactions[CrIndex].input)
            {
                Hack.Say(Hack.isDebugChemicalReactionDemoData, $"반응물 : {oneOfInput.matter}");
            }
            foreach(GameManager.chemical oneOfOutput in chemicalReactions[CrIndex].output)
            {
                Hack.Say(Hack.isDebugChemicalReactionDemoData, $"생성물 : {oneOfOutput.matter}");
            }
        }


    }

    
}
