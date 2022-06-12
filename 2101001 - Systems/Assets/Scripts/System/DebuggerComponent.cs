using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerComponent : MonoBehaviour
{
    public List<string> ErrorLog;

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        ErrorLog = new List<string>();
        Debug.Log("tee");
        ErrorLog.Add("log start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ErrLog(string log)
    {
        ErrorLog.Add(log);
    }

    public void PrintLog()
    {
        foreach (string oneOfErrorLog in ErrorLog)
        {
            Debug.Log(oneOfErrorLog);
        }
    }
}