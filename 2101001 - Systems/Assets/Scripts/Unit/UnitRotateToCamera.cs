using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRotateToCamera : MonoBehaviour
{
    GameObject CameraGo;
    Vector3 vector3;

    // Start is called before the first frame update
    void Start()
    {
        CameraGo = GameObject.Find("FieldCamera");

    }

    // Update is called once per frame
    void Update()
    {
        //vector3 = CameraGo.transform.position;
        //vector3.y = transform.position.y;

        //transform.GetChild(0).LookAt(vector3, new Vector3(0, 1, 0));
        // 카메라 위치가 아니라, 카메라가 회전한 각도를 따르도록 합시다.
    }
}
