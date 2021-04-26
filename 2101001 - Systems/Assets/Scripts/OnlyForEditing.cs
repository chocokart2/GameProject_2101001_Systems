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
        myRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
