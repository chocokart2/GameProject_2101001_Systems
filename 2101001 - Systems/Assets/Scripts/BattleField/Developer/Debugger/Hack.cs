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
