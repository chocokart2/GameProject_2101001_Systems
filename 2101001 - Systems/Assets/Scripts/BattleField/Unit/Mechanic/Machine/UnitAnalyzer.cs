using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnalyzer : MonoBehaviour
{
    float[] allowedFrequency; // 허용된 주파수

    public MachineUnitBase.MachineNetMessage Analyze(Collider[] colliders)
    {
        #region 함수 설명



        #endregion


        // 머신 설치 유닛만 분석합니다.
        // 만약 콜라이더에 인간을 발견하면 등록합니다 - gameObjects와gameObjectsInstanceIdList에 올립니다.
        // 만약 콜라이더에 머신을 발견하면, 그 머신을 기반으로 설치 유닛 게임오브젝트를 발견합니다.
        // 만약 동일한 설치 유닛이 둘 있다면 제거합니다.


        // + 설치 머신은 0.15만큼 y축으로 내려가 있습니다

        // Message 필드
        MachineUnitBase.MachineNetMessage returnValue = new MachineUnitBase.MachineNetMessage();
        returnValue.type = "SenseAboutUnitAnalyzed";
        List<string> returnValueSubData = new List<string>(); // 나중에 들어갈
        List<string> returnValueSubDataType = new List<string>(); // 나중에 들어갈

        // 동일 머신인지 판단하기 위한 필드입니다.
        List<GameObject> gameObjects = new List<GameObject>();
        List<int> gameObjectsInstanceIdList = new List<int>();

        // colliders을 gameObjects에 옮기는 작업을 진행합니다.
        for (int collidersIndex = 0; collidersIndex < colliders.Length; collidersIndex++)
        {
            // 만약 콜라이더에 머신을 발견하면, 그 머신을 기반으로 설치 유닛 게임오브젝트를 탐색합니다.
            if (colliders[collidersIndex].name.StartsWith("Machine"))
            {
                GameObject targetObject = colliders[collidersIndex].gameObject;
                
                // 아래 포 문은 발견할 수도, 못할 수도 있습니다.
                for(int attempt = 0; attempt < 10; attempt++)
                {
                    if (targetObject.name.StartsWith("MachinePlacement")
                        && (gameObjectsInstanceIdList.Contains(targetObject.GetInstanceID()) == false))
                    {
                        // 발견했으므로 올립니다.
                        gameObjects.Add(targetObject);
                        gameObjectsInstanceIdList.Add(targetObject.GetInstanceID());
                        break;
                    }
                    if (targetObject.transform.parent != null)
                    {
                        targetObject = targetObject.transform.parent.gameObject;
                    }
                }
            }

            if (colliders[collidersIndex].name.StartsWith("Human"))
            {
                gameObjects.Add(colliders[collidersIndex].gameObject);
                gameObjectsInstanceIdList.Add(colliders[collidersIndex].gameObject.GetInstanceID());

                // 추가할 것: allowedFrequency가 추가되면, 해당 주파수를 체크하도록 합니다.
            }
        }

        for(int gameObjectsIndex = 0; gameObjectsIndex < gameObjects.Count; gameObjectsIndex++)
        {
            // 정보 추가하기
            // 1. 인스턴스 아이디
            // 2. 팀의 정보
            // 3. 좌표
            // 매 유닛마다 array에 추가
            
            // 1.
            returnValueSubData.Add(gameObjects[gameObjectsIndex].GetInstanceID().ToString());

            // 2. GameObject의 UnitBase를 참조합니다.
            // 아무래도 팀이 무엇인지를 참조하는 것 같네.
            string Team = "Unknown";
            if (gameObjects[gameObjectsIndex].name.StartsWith("Machine"))
            {
                Team = "Machine";
            }
            else if (gameObjects[gameObjectsIndex].name.StartsWith("Human"))
            {
                Team = gameObjects[gameObjectsIndex].GetComponent<UnitBase>().unitBaseData.teamName;
            }
            returnValueSubData.Add(Team);

            // 3. 좌표
            Vector3 position = gameObjects[gameObjectsIndex].transform.position;
            if (gameObjects[gameObjectsIndex].name.StartsWith("Machine"))
            {
                position += new Vector3(0.0f, 0.15f, 0.0f);
            }
                
            returnValueSubData.Add(position.x.ToString());
            returnValueSubData.Add(position.y.ToString());
            returnValueSubData.Add(position.z.ToString());

            returnValueSubDataType.Add("int");
            returnValueSubDataType.Add("string");
            returnValueSubDataType.Add("float");
            returnValueSubDataType.Add("float");
            returnValueSubDataType.Add("float");
        }
        returnValue.subDataType = returnValueSubDataType.ToArray();
        returnValue.subData = returnValueSubData.ToArray();

        return returnValue;
    }
}
