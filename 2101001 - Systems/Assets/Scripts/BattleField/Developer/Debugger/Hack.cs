using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     디버거 역할을 맡습니다. 다만 클래스명중 Debug는 이미 다른사람에게 빼앗겼군요. 짧고 간단한 클래스명이 필요했습니다. 이해해주세요.
/// </summary>
public class Hack : MonoBehaviour
{
    // 변수
    // Unit.Common.Standard
    public static bool isDebugUnitPartBase = false;
    // Unit.Biological
    // Unit.Biological.Human
    public static bool isDebugHumanUnitBase = false;

    // Developer.Demo.Unit
    public static bool isDebugDemoBiologyPart = false;

    // others

    public static bool isDebugChemicalReactionDemoData = false;
    public static bool isDebugUnitSight = false;
    public static bool isDebugBeacon = false;
    // gamemamager
    public static bool isDebugGameManager_SetCurrentUnitRoleToFieldData = false;



    /// <summary>
    ///     Debug.Log와 역할이 동일합니다. 대신 앞에 있는 매개변수는 이 메시지를 호출할지 여부를 판단합니다.
    /// </summary>
    /// <param name="isNotIgnore">메시지를 표시할지 여부입니다. Hack 클래스의 변수를 사용하길 바립니다.</param>
    /// <param name="message">메시지입니다.</param>
    public static void Say(bool isNotIgnore, object message)
    {
        if (isNotIgnore)
        {
            Debug.Log(message);
        }
    }
}
