using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ����� ������ �ý��ϴ�. �ٸ� Ŭ�������� Debug�� �̹� �ٸ�������� ���Ѱ屺��. ª�� ������ Ŭ�������� �ʿ��߽��ϴ�. �������ּ���.
/// </summary>
public class Hack : MonoBehaviour
{
    // ����
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
    ///     Say�޼��带 ������ �����մϴ�. ��� üũ���� Ȯ���մϴ�.
    /// </summary>
    public enum check
    {
        /// <summary>
        /// �ڽ��� �޼��尡 ȣ��Ǿ����� ���θ� ������ �ľ��մϴ�.
        /// </summary>
        method = 0,
        /// <summary>
        /// �ڽ��� �޼��尡 ������ �ִٰ� ǥ���մϴ�.
        /// </summary>
        error = 1,
    }

    public static void Say(bool isNotIgnore, check _mode, object callerClass, [CallerMemberName] string member = "�� �� ���� ��� �̸�")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : �Լ��� ȣ��Ǿ����ϴ�.");
                    break;
                case check.error:
                    Debug.Log($"<!> ERROR_{callerClass.GetType().Name}.{member}() : �ǵ��� ���� �����Դϴ�.");
                    break;
                default:
                    Debug.LogError($"�� �� ���� mode �̸�");
                    break;
            }
        }
    }
    public static void Say(bool isNotIgnore, check _mode, object callerClass, object message, [CallerMemberName] string member = "�� �� ���� ��� �̸�")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : �Լ��� ȣ��Ǿ����ϴ�.\n{message}");
                    break;
                case check.error:
                    Debug.Log($"<!> ERROR_{callerClass.GetType().Name}.{member}() : {message}");
                    break;
                default:
                    Debug.LogError($"�� �� ���� mode �̸�");
                    break;
            }
        }
    }

    /// <summary>
    ///     Debug.Log�� ������ �����մϴ�. ��� �տ� �ִ� �Ű������� �� �޽����� ȣ������ ���θ� �Ǵ��մϴ�.
    /// </summary>
    /// <param name="isNotIgnore">�޽����� ǥ������ �����Դϴ�. Hack Ŭ������ ������ ����ϱ� �ٸ��ϴ�.</param>
    /// <param name="message">�޽����Դϴ�.</param>
    public static void Say(bool isNotIgnore, object message)
    {
        if (isNotIgnore)
        {
            Debug.Log(message);
        }
    }
}
