using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRotateToCamera : MonoBehaviour
{
    GameObject CameraGo;
    Transform quadTransform;
    Vector3 vector3;

    private void Awake()
    {
        quadTransform = transform.Find("Quad").GetComponent<Transform>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraGo = GameObject.Find("FieldCamera");

    }

    // Update is called once per frame
    void Update()
    {
        // 카메라 위치가 아니라, 카메라가 회전한 각도를 따르도록 합시다.
        quadTransform.rotation = Quaternion.Euler(
            CameraGo.transform.rotation.eulerAngles.x,
            CameraGo.transform.rotation.eulerAngles.y,
            0);
        
    }
}
