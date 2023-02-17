using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     디버거 역할을 맡습니다. 다만 클래스명중 Debug는 이미 다른사람에게 빼앗겼군요. 짧고 간단한 클래스명이 필요했습니다. 이해해주세요.
/// </summary>
public class Hack : BaseComponent
{
    /// <summary>
    ///     중립의 의미를 가질 수 있는 불리언 타입입니다.
    /// </summary>
    public enum EStat
    {
        neutral = 0,
        disable,
        enable
    }

    /// <summary>
    ///     디버그할 범위입니다.
    /// </summary>
    public static class Scope
    {
        /// <summary>
        ///     모든 메시지 무조건 출력 / 무조건 침묵을 결정합니다. 일반적으로는 null 값입니다.
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

    // 변수
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
        /// <summary>
        /// 함수에 대한 정보를 알려주고 싶을때 표시합니다.
        /// </summary>
        info = 2,
    }
    public static void Say(bool isNotIgnore, check _mode, Type type, [CallerMemberName] string member = "알 수 없는 멤버 이름")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{type.Name}.{member}() : 함수가 호출되었습니다.");
                    break;
                case check.info:
                    Debug.Log($"DEBUG_{type.Name}.{member}() : 지정한 부분을 실행했습니다.");
                    break;
                case check.error:
                    Debug.Log($"<!> ERROR_{type.Name}.{member}() : 의도지 않은 실행입니다.");
                    break;
                default:
                    Debug.LogError($"알 수 없는 mode 이름");
                    break;
            }
        }
        else if(isMustReceiveErrorMessage && _mode == check.error)
        {
            Debug.Log($"<!> ERROR_{type.Name}.{member}() : 의도지 않은 실행입니다.");
        }
    }
    /// <summary>
    ///     정보를 출력하는 메소드입니다.
    /// </summary>
    /// <remarks>
    ///     만약 함수가 출력되지 않는다면 우선 가장 첫번째 매개변수 값이 fasle인지 확인하십시오.</remarks>
    /// <param name="isNotIgnore"></param>
    /// <param name="_mode"></param>
    /// <param name="callerClass"></param>
    /// <param name="member"></param>
    public static void Say(bool isNotIgnore, check _mode, object callerClass, [CallerMemberName] string member = "알 수 없는 멤버 이름")
    {
        if (isNotIgnore)
        {
            switch (_mode)
            {
                case check.method:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : 함수가 호출되었습니다.");
                    break;
                case check.info:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : 지정한 부분을 실행했습니다.");
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
                case check.info:
                    Debug.Log($"DEBUG_{callerClass.GetType().Name}.{member}() : {message}");
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

    public static void Error(string callerClassName, object message, [CallerMemberName] string member = "알 수 없는 멤버 이름")
    {
        Debug.Log($"<!> ERROR_{callerClassName}.{member}() : {message}");
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
#nullable enable
    /// <summary>
    ///     대상이 Null인지 체크합니다.
    /// </summary>
    /// <param name="target">Null인지 체크할 대상입니다.</param>
    /// <param name="isNotIgnore">Null일때 메시지를 출력하는지 여부를 출력합니다.</param>
    /// <returns></returns>
    public static bool TrapNull(object? target, bool isNotIgnore = false)
    {
        if(target == null)
        {
            Say(isNotIgnore, $"<!> WARNING_NullCatch: 객체가 Null입니다.");
            //Say(isNotIgnore, $"<!> WARNING_NullCatch: {target.GetType().Name} 형식의 객체 혹은 NullAble {nameof(target)}은 Null입니다.");
            return true;
        }
        else
        {
            return false;
        }
    }
#nullable disable
}
