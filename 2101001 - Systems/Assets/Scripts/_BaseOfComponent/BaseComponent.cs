using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대부분의 컴포넌트에 사용될만한 함수와 인터페이스를 포함합니다.
/// </summary>
public class BaseComponent : MonoBehaviour
{
    const int NOT_FOUND = -1;

    #region 컴포넌트에 사용되는 인터페이스
    /// <summary>
    ///     이 인터페이스를 상속하는 컴포넌트는 직렬화 가능한 클래스의 객체를 받을 수 있습니다. </summary>
    /// <typeparam name="T">
    ///     데이터 출력을 위한 클래스입니다. </typeparam>
    public interface IDataGetableComponent<T>
    {
        T GetComponentData();
    }
    /// <summary>
    ///     이 인터페이스를 상속하는 컴포넌트는 직렬화 가능한 클래스의 객체를 받을 수 있습니다. </summary>
    /// <typeparam name="T">
    ///     데이터 입력을 위한 클래스입니다. </typeparam>
    public interface IDataSetableComponent<T>
    {
        void SetComponentData(T componentData);
    }
    #endregion

    /// <summary>
    /// 이름을 통해 객체를 구별할 수 있도록 하는 인터페이스입니다.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         반드시 해야 할 것:
    ///     </para>
    ///     <para>
    ///         속성 Name이 접근하는 필드는 Public이여야 합니다.
    ///     </para>
    /// </remarks>
    public interface INameKey
    {
        /// <summary>
        /// 이 객체의 이름입니다.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 양을 측정할 수 있는 인터페이스입니다.
    /// INameKey도 상속한다면 NamedQuantityArrayHelper의 도움을 받을 수 있습니다.
    /// </summary>
    public interface IMeasurable // 양을 빼주거나 연산할 수 있는 클래스의 메서드를 만들자!
    {
        /// <summary>
        /// 이 객체의 양입니다.
        /// </summary>
        public float Quantity { get; set; }
    }

    // Edited Element
    /// <summary>
    /// 객체의 이름을 붙일 수 있고, 이 객체의 양을 측정할 수 있도록 하는 인터페이스입니다.
    /// </summary>
    public interface INamedQuantity : INameKey, IMeasurable
    {

    }

    // 왜 TYPE[]를 사용하지 않는거지? => 배열 클래스에 함수를 더 추가할 수 있었으면 좋겠어. 확장 메서드라는 다른 방법도 있긴 한데
    /// <summary>
    /// 어떤 녀석의 배열입니다.
    /// </summary>
    /// <typeparam name="ElementType">이 배열의 원소의 타입입니다.</typeparam>
    public interface IArray<ElementType>
    {
        public ElementType this[int index] { get; set; }
        public int Length { get; }
    }

    public interface IExpandable<ElementType>
    {
        public void Add(ElementType data);
    }

    /// <summary>
    /// 음수가 아닌 존재입니다. 값이 0일 수 있습니다.
    /// </summary>
    public interface INotNegativeValue
    {
        public bool IsPositive { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ElementType"></typeparam>
    public interface INameKeyArray<ElementType> : IArray<ElementType> where ElementType : INameKey
    {
        //public ElementType this[int index] { get; set; }
        //public int Length { get; }
    }



    /// <summary>
    /// 이름을 붙일 수 있고, 양을 측정할 수 있습니다.
    /// </summary>


    public interface INamedQuantityArray<ElementType> : IArray<ElementType> where ElementType : INamedQuantity
    {

    }

    public static class NameKeyArrayHelper
    {
        public static int Find<ArrayClass, ElementClass>(ArrayClass array, string key)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : INameKey
        {
            for(int index = 0; index < array.Length; ++index)
            {
                if (array[index].Name.Equals(key))
                    return index;
            }
            return NOT_FOUND;
        }
        /// <summary>
        /// find sample from population
        /// </summary>
        /// <typeparam name="T">INAmeKey를 상속하는 타입입니다.</typeparam>
        /// <param name="population">찾으려는 집단이 있다고 추정되는 모집단(population)입니다.</param>
        /// <param name="sample">모집단에서 찾으려는 표본집단(sample)입니다.</param>
        /// <returns> 모집단의 인덱스 값을 배열로 담아냅니다.
        ///     <para>
        ///         예를 들어 sample[A]의 Name과 population[B]의 Name이 같을 때, 반환된배열[A]는 B입니다. 이때 B는 최솟값입니다.
        ///     </para>
        ///     <para>
        ///         만약 값을 찾지 못했으면 반환된 배열[A] == -1입니다.
        ///     </para>
        /// </returns>
        public static int[] Find<ArrayClass, ElementClass>(ArrayClass population, ArrayClass sample)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : INameKey
        {
            Hack.Say(Hack.isDebugBaseComponent, $"DEBUG_BaseComponent.Find() : sample.Length = {sample.Length}");

            // 일단 모든 존재를 찾지 못했다고 가정합니다.
            int[] result = Enumerable.Repeat(NOT_FOUND, sample.Length).ToArray();

            for (int sampleIndex = 0; sampleIndex < sample.Length; sampleIndex++)
            {
                bool found = false;
                for(int populationIndex = 0; populationIndex < population.Length; populationIndex++)
                {
                    if (population[populationIndex].Name.Equals(sample[sampleIndex].Name))
                    {
                        result[sampleIndex] = sampleIndex;                
                        found = true;
                    }
                }
                if(found == false)
                {
                    result[sampleIndex] = NOT_FOUND;
                }
            }
            return result;
        }
    }
    public static class MeasurableArrayHelper
    {
        /// <summary>
        /// 모든 원소의 quantity 값이 양수인지 판단합니다.
        /// </summary>
        /// <typeparam name="ArrayClass"></typeparam>
        /// <typeparam name="ElementClass"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsAllElementPositive<ArrayClass, ElementClass>(ArrayClass target)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : IMeasurable
        {
            for(int index = 0; index < target.Length; ++index)
            {
                if (target[index].Quantity <= 0.0f) return false;
            }

            return true;
        }
        /// <summary>
        /// 대상 배열의 원소중 하나라도 Quantity 값리 양수라면 true를 리턴합니다.
        /// </summary>
        /// <typeparam name="ArrayClass"></typeparam>
        /// <typeparam name="ElementClass"></typeparam>
        /// <param name="target">양수 값을 찾으려는 배열입니다.</param>
        /// <returns></returns>
        public static bool IsAnyElementPositive<ArrayClass, ElementClass>(ArrayClass target)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : IMeasurable
        {
            for(int index = 0; index < target.Length; ++index)
            {
                if (target[index].Quantity > 0.0f) return true;
            }
            return false; // 양수 값을 찾지 못했습니다.
        }
    }

    public static class NamedQuantityArrayHelper
    {
        // 사칙연산
        public static void Add<ArrayClass, ElementClass>(ref ArrayClass target, ArrayClass addend)
            where ArrayClass : IArray<ElementClass>, IExpandable<ElementClass>
            where ElementClass : INameKey, IMeasurable
        {
#warning indexArray에 내용이 없는 것 같아요!
            int[] indexArray = NameKeyArrayHelper.Find<ArrayClass, ElementClass>(target, addend);
            
            for(int index = 0; index < indexArray.Length; ++index)
            {
                if (indexArray[index] == NOT_FOUND)
                {
                    target.Add(addend[index]);
                }
                else
                {
                    Hack.Say(Hack.Scope.BaseComponent.NamedQuantityArrayHelper.Add,
                        $"DEBUG_NamedQuantityArrayHelper.Add() : indexArray[{index}] = {indexArray[index]}");
                    Hack.Say(Hack.Scope.BaseComponent.NamedQuantityArrayHelper.Add,
                        $"DEBUG_NamedQuantityArrayHelper.Add() : target의 길이 = {target.Length}");

                    target[indexArray[index]].Quantity += addend[index].Quantity;
                }
            }
        }
        /// <summary>
        /// dividend[A].Name == divisor[B].Name일 때, dividend[A].quantity / divisor[B].quantity를 한 값중 가장 작은 값을 구합니다.
        /// </summary>
        /// <typeparam name="ArrayClass">배열 클래스입니다.</typeparam>
        /// <typeparam name="ElementClass">원소 클래스입니다.</typeparam>
        /// <param name="dividend">나눔을 당하는 화학물질입니다. 나눗셈의 피제수와 비슷합니다</param>
        /// <param name="divisor">나누는 화학물질입니다. 나눗셈의 제수와 비슷합니다.</param>
        /// <returns>
        ///     <para>
        ///         각각의 원소들을 나누기 한 값 중 가장 최솟값을 구합니다.
        ///     </para>
        /// </returns>
        public static float Divide<ArrayClass, ElementClass>(ArrayClass dividend, ArrayClass divisor)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : INameKey, IMeasurable
        {
            float[] quotients = new float[divisor.Length];

            int[] indexArray = NameKeyArrayHelper.Find<ArrayClass, ElementClass>(dividend, divisor);

            for(int index = 0; index < indexArray.Length; ++index)
            {
                if (indexArray[index] == NOT_FOUND) continue;
                quotients[index] = dividend[indexArray[index]].Quantity / divisor[index].Quantity;
            }
            return Mathf.Min(quotients);
        }
        public static void Multiply<ArrayClass, ElementClass>(ref ArrayClass multiplier, float multiplicand)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : IMeasurable
        {
            for(int index = 0; index < multiplier.Length; ++index)
            {
                multiplier[index].Quantity *= multiplicand;
            }
        }
        /// <summary>
        /// minuend(at after function) == minuend(in argument) - subtrahend
        /// </summary>
        /// <remarks>
        /// 만약 존재하지 않는 대상을 지우려는 경우, 그 부분은 무시됩니다.
        /// </remarks>
        /// <typeparam name="ArrayClass"></typeparam>
        /// <typeparam name="ElementClass"></typeparam>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        public static void Subtract<ArrayClass, ElementClass>(ref ArrayClass minuend, ArrayClass subtrahend)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : INameKey, IMeasurable
        {
            int[] indexArray = NameKeyArrayHelper.Find<ArrayClass, ElementClass>(minuend, subtrahend);

            for(int index = 0; index < indexArray.Length; ++index)
            {
                if (indexArray[index] == NOT_FOUND) continue;
                minuend[indexArray[index]].Quantity -= subtrahend[index].Quantity;
            }
        }

        // 개선된 연산
        // 필요에 의해 생겨난 함수들입니다. 물론 기본이 되는 여러 함수를 연달아 호출하여 대체할 수 있겠지만, 아무래도 이게 더 빠를것 같다고 추론해서 만들었습니다.
        /// <summary>
        /// 두 값의 공통되는 값을 제거합니다. 예를 들어 5와 3이라면 2와 0이 남습니다. 차이점을 기준으로 하기 때문에, 한 쪽이 음수라면 값이 오히려 늘어날 수 있습니다.
        /// </summary>
        /// <typeparam name="ArrayClass"></typeparam>
        /// <typeparam name="ElementClass"></typeparam>
        /// <param name="firstArray"></param>
        /// <param name="secondArray"></param>
        public static void RemoveIntersection<ArrayClass, ElementClass>(ref ArrayClass firstArray, ref ArrayClass secondArray)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : INameKey, IMeasurable
        {
            int[] indexArray = NameKeyArrayHelper.Find<ArrayClass, ElementClass>(firstArray, secondArray);

            for(int index = 0; index < indexArray.Length; ++index)
            {
                if (indexArray[index] == NOT_FOUND) continue;

                float differenceValue = firstArray[indexArray[index]].Quantity - secondArray[index].Quantity;
                if (differenceValue > 0.0f)
                {
                    firstArray[indexArray[index]].Quantity = differenceValue;
                    secondArray[index].Quantity = 0.0f;
                }
                else
                {
                    firstArray[indexArray[index]].Quantity = 0.0f;
                    secondArray[index].Quantity = -differenceValue;
                }
            }
        }

        public static int[] FindPositiveAll<ArrayClass, ElementClass>(ArrayClass population, ArrayClass sample)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : INameKey, IMeasurable
        {
            int[] result = NameKeyArrayHelper.Find<ArrayClass, ElementClass>(population, sample);
            if (Array.Exists(result, i => i == -1)) return new int[] { -1 };
            return result;
        }

        /// <summary>
        /// Subtract와 유사하나, 존재하지 않는 부분에 대해서도 결과값에 포함합니다.
        /// 필요한 원소가 무엇인지를 리턴해줍니다.
        /// </summary>
        /// <typeparam name="ArrayClass"></typeparam>
        /// <typeparam name="ElementClass"></typeparam>
        /// <param name="demand"></param>
        /// <param name="supply"></param>
        /// <returns></returns>
        public static ArrayClass GetDemand<ArrayClass, ElementClass>(ArrayClass demand, ArrayClass supply)
            where ArrayClass : IArray<ElementClass>, IExpandable<ElementClass>, new()
            where ElementClass : INameKey, IMeasurable, new()
        {
            ArrayClass result = new ArrayClass();

            int[] arrayIndex = NameKeyArrayHelper.Find<ArrayClass, ElementClass>(demand, supply);
            
            for (int index = 0; index < arrayIndex.Length; index++)
            {
                // 공급이 없는 수요이므로, 수요를 추가합니다.
                if (arrayIndex[index] == NOT_FOUND)
                {
                    result.Add(demand[index]);
                    continue;
                }

                // 공급이 수요를 넘어서므로, 무시합니다.
                if (supply[arrayIndex[index]].Quantity >= demand[index].Quantity)
                {
                    continue;
                }

                ElementClass newDemand = demand[index];
                newDemand.Quantity -= supply[arrayIndex[index]].Quantity;

                result.Add(newDemand);
            }

            return result;
        }
    }

    /// <summary>
    /// 긍정, 중립, 부정의 값을 가질 수 있는 자료형입니다.
    /// </summary>
    public class myBool
    {

    }

    public static int AddElementArray<ElementType>(ref ElementType[] array, ElementType element)
    {
        if (array == null)
        {
            array = new ElementType[] { element };
            return 0;
        }
        for (int index = 0; index < array.Length; index++)
        {
            if (array[index] == null)
            {
                array[index] = element;
                return index;
            }
        }
        Array.Resize(ref array, array.Length + 1);
        array[array.Length - 1] = element;
        return array.Length - 1;
    }
    public static int[] AddElementsArray<ElementType>(ref ElementType[] array, params ElementType[] elements)
    {
        // null이 몇 개인지 파악하고 nullIndex[]에 저장한다
        // element.length - nullIndex.length 한 만큼 배열의 크기를 늘린다
        // 새로 생겨난 공간의 인덱스도 nullIndex에 저장한다.
        // 빈 공간마다 원소를 넣는다.
        // nullIndex를 리턴한다

        int[] result = new int[elements.Length];
        int position = 0; // result의 index
        for(int index = 0; index < array.Length; ++index)
        {
            if (array[index] == null)
            {
                result[position] = index;
                position++;
            }
            if (position >= elements.Length) break;
        }
        // 만약 array의 null인 곳의 갯수가 elements.Length보다 작으면, 아래 코드가 실행될 것입니다.
        if(position < elements.Length)
        {
            for (int index = position, newIndex = array.Length; index < elements.Length; index++, newIndex++)
            {
                result[position] = newIndex;
            }

            Array.Resize(ref array, array.Length + Mathf.Max(0, elements.Length - position));
        }

        for (int index = 0; index < result.Length; index++)
        {
            array[result[index]] = elements[index];
        }

        return result;
    }
    //public static int FindIndexBinarySearch<ElementType>(ElementType[] array, ElementType element, IComparable compare)
    //{

    //}

    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }
}
