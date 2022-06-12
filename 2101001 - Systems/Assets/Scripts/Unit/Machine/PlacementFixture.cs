using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementFixture : MonoBehaviour
{
    Vector3 vector3;

    // Start is called before the first frame update
    void Start()
    {
        vector3 = transform.position;
        //GameObject.Find("GameManager").GetComponent<GameManager>().aaa = 3;
        GameObject.Find("GameManager").GetComponent<GameManager>().FixturePosition.Add(vector3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Delete()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().FixturePosition.Remove(vector3);
    }
}
