using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerMain : MonoBehaviour
{
    Camera buttonCamera;

    // Start is called before the first frame update
    void Start()
    {
        buttonCamera = GameObject.Find("ButtonCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray RayToMousePoint = buttonCamera.ScreenPointToRay(Input.mousePosition); // 지역변수 RayToMousePoint
            RaycastHit RayToMousePointHit;
            if(Physics.Raycast(RayToMousePoint, out RayToMousePointHit, 100.0f))
            {
                CampainButton cb = RayToMousePointHit.collider.GetComponent<CampainButton>();


                if (cb != null)
                {
                    cb.ButtonPressed();
                }
                    

            }

        }




    }
}
