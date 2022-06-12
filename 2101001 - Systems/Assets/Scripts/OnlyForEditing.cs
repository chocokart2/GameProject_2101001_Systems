using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyForEditing : MonoBehaviour
{
    Renderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        if(myRenderer != null)
        {
            myRenderer.enabled = false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<Renderer>() != null)
            {
                transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
