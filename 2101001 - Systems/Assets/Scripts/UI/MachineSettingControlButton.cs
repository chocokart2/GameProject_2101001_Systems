using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSettingControlButton : MonoBehaviour
{
    public void CallButtonFunction()
    {
        // 자신의 게임오브젝트 이름에 따라 MachineSettingGuiController의 함수를 호출하는 존재입니다.

        SelectFunction();
    }

    void SelectFunction()
    {
        MachineSettingGuiController Gui = transform.parent.GetComponent<MachineSettingGuiController>();

        if (transform.parent != null)
        {
            switch (gameObject.name)
            {
                case "EnergyStorageSettingButton":
                    transform.parent.GetComponent<MachineSettingGuiController>().DoButtonActionEnergyStorage();
                    break;
                case "TargetAimerSettingButton":
                    transform.parent.GetComponent<MachineSettingGuiController>().DoButtonActionTargetAimer();
                    break;
                case "NetworkConnectorSettingButton":
                    transform.parent.GetComponent<MachineSettingGuiController>().DoButtonActionNetworkConnector();
                    break;
                default:
                    Debug.Log("<!> ERROR_MachineSettingControlButton.SelectFunction: 반응이 지정되지 않은 이름");
                    break;
            }


        }
        else
        {
            Debug.Log("<!> ERROR_MachineSettingMainButton.OnTriggerEnter: 이 게임오브젝트의 부모 오브젝트가 지정되지 않았습니다.");
        }
    }
}
