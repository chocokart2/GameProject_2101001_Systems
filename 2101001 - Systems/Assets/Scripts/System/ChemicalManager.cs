using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalManager : MonoBehaviour
{

    #region 클래스
    #region 000 캐미컬 창조 시 필요한 것
    [System.Serializable]
    public class ChemicalSeed
    {
        public ChemicalSeedSeed[] seed;
    }
    [System.Serializable]
    public class ChemicalSeedSeed
    {
        public string[] seed;
    }
    [System.Serializable]
    public class elementList // 생성된 캐미컬을 만들기 위한 데이터입니다.
    {
        public string[] matter;
    }

    #endregion
    #region 100 항상 필요한 클래스

    #endregion
    #endregion
    #region 함수
    static public elementList ChemicalMake(int count, ref ChemicalSeed inputSeed)
    {
        if(count == 0)
        {
            Debug.Log("ChemicalManager.ChemicalMake : count가 양수가 아님.");
        }
        if(inputSeed == null)
        {
            Debug.Log("ChemicalManager.ChemicalMake : inputSeed가 null값입니다.");
        }
        if(inputSeed.seed == null)
        {
            Debug.Log("ChemicalManager.ChemicalMake : inputSeed.seed가 null값입니다.");
        }

        elementList returnValue = new elementList();
        returnValue.matter = new string[count];
        for(int i = 0; i < count; i++) // 몇 번 만드는지에 대한 인덱스입니다.
        {
            returnValue.matter[i] = "";

            bool isFound = false;
            do
            {
                isFound = false;

                for (int index = 0; index < inputSeed.seed.Length; index++)
                {
                    returnValue.matter[i] += inputSeed.seed[index].seed[UnityEngine.Random.Range(0, inputSeed.seed[index].seed.Length - 1)];
                }

                // 중복 체크 i부터 0까지 체크
                for (int j = 0; j > i; j++)
                {
                    if (returnValue.matter[i] == returnValue.matter[j]) // 이미 존재하는 캐미컬
                    {
                        isFound = true;
                        break;
                    }
                }

            } while (isFound);
        }

        return returnValue;
    }


    #endregion




}
