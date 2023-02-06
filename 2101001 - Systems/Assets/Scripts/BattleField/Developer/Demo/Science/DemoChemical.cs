using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     기본적인 물질에 대한 값을 가집니다. 하드코딩을 완화합니다.
/// </summary>
public class DemoChemical : MonoBehaviour
{
    /// <summary>
    ///     테스트용 기본 케미컬입니다. 일반적으로 반응하지 않습니다.
    /// </summary>
    /// <remarks>
    ///     화학 반응에도 관여하지 않습니다. 화학반응을 정의하지 않습니다. 쉽게 부서지지 않습니다.
    /// </remarks>
    public const string Default = "theousia";
    
    /// <summary>
    ///     화학 반응이 정해진 두 물질입니다. ㅠ=
    /// </summary>
    public const string Alpha = "alpha";
    /// <summary>
    ///     화학 반응이 정해진 두 물질입니다.
    /// </summary>
    public const string Beta = "beta";

    /// <summary>
    ///     테스트용 화학 반응 이후에 만들어지는 물질입니다.
    /// </summary>
    public const string Gamma = "gamma";


    //public static

    public struct Chemicals
    {
        
        
    }
}
