using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ����� ������ �ý��ϴ�. �ٸ� Ŭ�������� Debug�� �̹� �ٸ�������� ���Ѱ屺��. ª�� ������ Ŭ�������� �ʿ��߽��ϴ�. �������ּ���.
/// </summary>
public class Hack : BaseComponent
{
    /// <summary>
    ///     �߸��� �ǹ̸� ���� �� �ִ� �Ҹ��� Ÿ���Դϴ�.
    /// </summary>
    public enum EStat
    {
        neutral = 0,
        disable,
        enable
    }

    /// <summary>
    ///     ������� �����Դϴ�.
    /// </summary>
    public static class Scope
    {
        /// <summary>
        ///     ��� �޽��� ������ ��� / ������ ħ���� �����մϴ�. �Ϲ������δ� null ���Դϴ�.
        /// </summary>
        private static bool? all = null;

        #region System.Common
        public static class BaseComponent
        {
            private static bool? _all = null;

            public static class NamedQuantityArrayHelper
            {
                private static bool? __all = null;

                private static bool add = false;

                public static bool Add
                {
                    get => ((all ?? _all) ?? __all) ?? add;
                    set => add = value;
                }
            }
        }

        #endregion
        #region Units.Common.Standard
        public static class UnitBase
        {
            private static bool? _all = null;

            private static bool walk = false;

            public static bool Walk
            {
                get => (all ?? _all) ?? walk;
                set => walk = value;
            }
        }
        public static class UnitPartBase
        {
            private static bool? _all = null;

            public static class UnitPart
            {
                private static bool? __all = null;

                private static bool constructor = false;
                private static bool isPassing = false;

                public static bool Constructor
                {
                    get => ((all ?? _all) ?? __all) ?? constructor;
                    set => constructor = value;
                }
                public static bool IsPassing
                {
                    get => ((all ?? _all) ?? __all) ?? isPassing;
                    set => isPassing = value;
                }
            }
            public static class SingleChemicalWholeness
            {
                private static bool? __all = null;

                private static bool getAngleWholeness = true;

                public static bool GetAngleWholeness
                {
                    get => ((all ?? _all) ?? __all) ?? getAngleWholeness;
                }
            }
        }
        #endregion



    }

    // ����
    public static bool isMustReceiveErrorMessage = true;
    // System.Common
    public static bool isDebugBaseComponent = false;
    // Unit.Common.Standard
    public static bool isDebugUnitBase = true;
    public static bool isDebugUnitPartBase = true;
    public static bool isDebugUnitSightController = false;
    // unit.common.addable
    public static bool isDebugItemHelper = true;
    public static bool isDebugUnitItemPack = true;
    // Unit.Biological
    // Unit.Biological.Human
    public static bool isDebugHumanUnitBase = true;
    #region isDebugHumanUnitBase
    public static bool isDebugHumanUnitBase_Nerv = true;
    #endregion

    // Developer.Demo.Unit
    public static bool isDebugDemoBiologyPart = false;

    // others
    public static bool isDebugGameObjectList = true;
    public static bool isDebugAttackTurret = true;
    public static bool isDebugChangeController = true;
    public static bool isDebugChemicalReactionDemoData = false;
    public static bool isDebugUnitSight = false;
    public static bool isDebugBeacon = false;
    // gamemamager
    public static bool isDebugGameManager_Awake = true;
    public static bool isDebugGameManager_Start = true;
    public static bool isDebugGameManager_SetCurrentUnitRoleToFieldData = false;
    public static bool isDebugGameManager_UnitInstantiate = true;

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
        /// <summary>
        /// �Լ��� ���� ������ �˷��ְ� ������ ǥ���մϴ�.
        /// </summary>
        info = 2,
    }
    public static void Say(bool isNotIgnore, check _mode, Type type, [CallerMemberName] string member = "�� �� ���� ��� �̸�")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{type.Name}.{member}() : �Լ��� ȣ��Ǿ����ϴ�.");
                    break;
                case check.info:
                    Debug.Log($"DEBUG_{type.Name}.{member}() : ������ �κ��� �����߽��ϴ�.");
                    break;
                case check.error:
                    Debug.Log($"<!> ERROR_{type.Name}.{member}() : �ǵ��� ���� �����Դϴ�.");
                    break;
                default:
                    Debug.LogError($"�� �� ���� mode �̸�");
                    break;
            }
        }
        else if(isMustReceiveErrorMessage && _mode == check.error)
        {
            Debug.Log($"<!> ERROR_{type.Name}.{member}() : �ǵ��� ���� �����Դϴ�.");
        }
    }
    /// <summary>
    ///     ������ ����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <remarks>
    ///     ���� �Լ��� ��µ��� �ʴ´ٸ� �켱 ���� ù��° �Ű����� ���� fasle���� Ȯ���Ͻʽÿ�.</remarks>
    /// <param name="isNotIgnore"></param>
    /// <param name="_mode"></param>
    /// <param name="callerClass"></param>
    /// <param name="member"></param>
    public static void Say(bool isNotIgnore, check _mode, object callerClass, [CallerMemberName] string member = "�� �� ���� ��� �̸�")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : �Լ��� ȣ��Ǿ����ϴ�.");
                    break;
                case check.info:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : ������ �κ��� �����߽��ϴ�.");
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
                case check.info:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : {message}");
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

    public static void Error(string callerClassName, object message, [CallerMemberName] string member = "�� �� ���� ��� �̸�")
    {
        Debug.Log($"<!> ERROR_{callerClassName}.{member}() : {message}");
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
#nullable enable
    /// <summary>
    ///     ����� Null���� üũ�մϴ�.
    /// </summary>
    /// <param name="target">Null���� üũ�� ����Դϴ�.</param>
    /// <param name="isNotIgnore">Null�϶� �޽����� ����ϴ��� ���θ� ����մϴ�.</param>
    /// <returns></returns>
    public static bool TrapNull(object? target, bool isNotIgnore = false)
    {
        if(target == null)
        {
            Say(isNotIgnore, $"<!> WARNING_NullCatch: ��ü�� Null�Դϴ�.");
            //Say(isNotIgnore, $"<!> WARNING_NullCatch: {target.GetType().Name} ������ ��ü Ȥ�� NullAble {nameof(target)}�� Null�Դϴ�.");
            return true;
        }
        else
        {
            return false;
        }
    }
#nullable disable
}
