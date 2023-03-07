using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     사망한 유닛을 표시하기 위해 존재합니다.
/// </summary>
public class DeadIcon : MonoBehaviour
{

    (float angleSpeed, float timeCycle) move1 = (0.0f, 1.0f);
    (float angleSpeed, float timeCycle) move2 = (0.0f, 2.0f);

    // Update is called once per frame
    void Update()
    {
        //move1.timeCycle -= Time.deltaTime;
        //move2.timeCycle -= Time.deltaTime;

        //if (move1.timeCycle < 0.0f)
        //{
        //    move1.angleSpeed = Random.value;
        //    move1.timeCycle = Random.value * 4;
        //}
        //if (move2.timeCycle < 0.0f) move2.timeCycle = Random.value * 4;

        transform.Rotate(new Vector3(0, 180 * Time.deltaTime, 0));
    }
}
