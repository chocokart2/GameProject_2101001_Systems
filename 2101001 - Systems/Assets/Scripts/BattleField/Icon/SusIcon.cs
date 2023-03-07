using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusIcon : MonoBehaviour
{
    #region Field
    Vector3 NowXYZ;
    Vector3 NextXYZ; // xyz는 각각 랜덤한 값을 가집니다.
    Vector3 NowXYZ2;
    Vector3 NextXYZ2;
    float nowTime; // 0부터 시작해서 TimeCicle으로,
    float nowTime2; // 0부터 시작해서 TimeCicle으로,
    float timeCycle; // 타임이 이 값에 도달하면 타임을 0으로 만드는 기준입니다.
    float timeCycle2; // 타임이 이 값에 도달하면 타임을 0으로 만드는 기준입니다.
    float LifeCycle;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        NowXYZ = new Vector3(0, 0, 0); NowXYZ2 = new Vector3(0, 0, 0);
        NextXYZ = newNextXYZ(); NextXYZ2 = newNextXYZ();
        nowTime = 0.0f;
        timeCycle = 1.0f;
        timeCycle2 = 2.0f;

        LifeCycle = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;
        nowTime2 += Time.deltaTime;
        LifeCycle -= Time.deltaTime;
        if (nowTime > timeCycle)
        {
            nowTime = 0.0f;
            NextXYZ = newNextXYZ();
            timeCycle = 2.0f;//Random.Range(2.0f, 3.0f);
        }
        if (nowTime2 > timeCycle2)
        {
            nowTime2 = 0.0f;
            NextXYZ2 = newNextXYZ();
            timeCycle2 = 2.0f;//Random.Range(2.0f, 3.0f);
        }

        if (LifeCycle < 0.0f)
        {
            transform.localScale /= (1 + Time.deltaTime);
            Destroy(gameObject, 1.0f);
        }
        //float x = value(timeCycle, nowTime) * (NextXYZ.x - NowXYZ.x) + NowXYZ.x;
        float x = value(timeCycle, nowTime) * (NextXYZ.x - NowXYZ.x) + NowXYZ.x + value(timeCycle2, nowTime2) * (NextXYZ2.x - NowXYZ2.x) + NowXYZ2.x;
        //float y = value(timeCycle, nowTime) * (NextXYZ.y - NowXYZ.y) + NowXYZ.y;
        float y = value(timeCycle, nowTime) * (NextXYZ.y - NowXYZ.y) + NowXYZ.y + value(timeCycle2, nowTime2) * (NextXYZ2.y - NowXYZ2.y) + NowXYZ2.y;
        //float z = value(timeCycle, nowTime) * (NextXYZ.z - NowXYZ.z) + NowXYZ.z;
        float z = value(timeCycle, nowTime) * (NextXYZ.z - NowXYZ.z) + NowXYZ.z + value(timeCycle2, nowTime2) * (NextXYZ2.z - NowXYZ2.z) + NowXYZ2.z;

        transform.Rotate(new Vector3(x, y, z));
    }
    Vector3 newNextXYZ() {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }
    float value(float cycle, float time)
    {
        return Mathf.Pow(time, 2.0f) - cycle * time;
    }
}
