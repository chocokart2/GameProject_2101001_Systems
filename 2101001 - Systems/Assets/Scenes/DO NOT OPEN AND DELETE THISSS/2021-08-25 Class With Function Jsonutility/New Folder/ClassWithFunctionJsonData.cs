using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassWithFunctionJsonData : MonoBehaviour
{
    // Json 파일로 변환할 수 있는 클래스를 만들어야 합니다.
    // 이 클래스는 내부에 함수가 있습니다.
    // Json 파일로 저장하도록 시도합니다.
    // 파일을 로드해보기도 합니다.

    //static public void DataSaving<DataType>(DataType data, string format, string fileName, string worldName, string Folder) // 이름은 속편하게 매게변수로 처넣어버리면 그만ㅋㅋ
    //static public bool DataLoading<DataType>(string path, ref DataType receivingInstance)

    [System.Serializable]
    public class FlyingPizza
    {
        public int numberOfpizzaSlices;
        int a;
        private int delta = 0;

        public FlyingPizza()
        {
            numberOfpizzaSlices = 0;
            a = 2;
        }
        public void Chop()
        {
            if (numberOfpizzaSlices != null) numberOfpizzaSlices++;
            a = 0;
            Debug.Log("Yee");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 파일을 찾습니다. 파일이 없으면 그냥 새로 만들어 변수에 저장합니다.
        FlyingPizza flyingPizza = new FlyingPizza();
        if (GameManager.DataLoading("Assets/Scenes/DO NOT OPEN AND DELETE THISSS/2021-08-25 Class With Function Jsonutility/pizzaData.json", ref flyingPizza) == false)
        {
            Debug.Log("파일이 로딩되지 않음.");
        }

        // 변수에 저장한 파일에 함수를 실행시킵니다.
        flyingPizza.Chop();



        // 파일을 저장합니다.
        GameManager.DataSaving("Assets/Scenes/DO NOT OPEN AND DELETE THISSS/2021-08-25 Class With Function Jsonutility/pizzaData.json", ref flyingPizza);
    }

}
