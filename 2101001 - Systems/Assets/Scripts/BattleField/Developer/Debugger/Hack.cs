using System.Runtime.CompilerServices;
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
    public static bool isDebugUnitBase = true;
    public static bool isDebugUnitPartBase = false;
    public static bool isDebugUnitSightController = false;
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
    ///     Say메서드를 빠르게 설정합니다. 어떤걸 체크할지 확인합니다.
    /// </summary>
    public enum check
    {
        /// <summary>
        /// 자신의 메서드가 호출되었는지 여부를 빠르게 파악합니다.
        /// </summary>
        method = 0,
        /// <summary>
        /// 자신의 메서드가 오류가 있다고 표시합니다.
        /// </summary>
        error = 1,
    }

    public static void Say(bool isNotIgnore, check _mode, object callerClass, [CallerMemberName] string member = "알 수 없는 멤버 이름")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : 함수가 호출되었습니다.");
                    break;
                case check.error:
                    Debug.Log($"<!> ERROR_{callerClass.GetType().Name}.{member}() : 의도지 않은 실행입니다.");
                    break;
                default:
                    Debug.LogError($"알 수 없는 mode 이름");
                    break;
            }
        }
    }
    public static void Say(bool isNotIgnore, check _mode, object callerClass, object message, [CallerMemberName] string member = "알 수 없는 멤버 이름")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : 함수가 호출되었습니다.\n{message}");
                    break;
                case check.error:
                    Debug.Log($"<!> ERROR_{callerClass.GetType().Name}.{member}() : {message}");
                    break;
                default:
                    Debug.LogError($"알 수 없는 mode 이름");
                    break;
            }
        }
    }

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
