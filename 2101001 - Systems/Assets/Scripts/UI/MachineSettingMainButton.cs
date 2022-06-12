using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSettingMainButton : MonoBehaviour
{
    public void CallButtonFunction()
    {
        if(transform.parent != null)
        {
            transform.parent.GetComponent<MachineSettingGuiController>().CallButtonFunction(gameObject.name);
        }
        else
        {
            Debug.Log("<!> ERROR_MachineSettingMainButton.OnTriggerEnter: 이 게임오브젝트의 부모 오브젝트가 지정되지 않았습니다.");
        }
    }
}
