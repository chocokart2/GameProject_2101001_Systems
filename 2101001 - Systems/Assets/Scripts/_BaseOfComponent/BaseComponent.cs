using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��κ��� ������Ʈ�� ���ɸ��� �Լ��� �������̽��� �����մϴ�.
/// </summary>
public class BaseComponent : MonoBehaviour
{
    const int NOT_FOUND = -1;

    #region ������Ʈ�� ���Ǵ� �������̽�
    /// <summary>
    ///     �� �������̽��� ����ϴ� ������Ʈ�� ����ȭ ������ Ŭ������ ��ü�� ���� �� �ֽ��ϴ�. </summary>
    /// <typeparam name="T">
    ///     ������ ����� ���� Ŭ�����Դϴ�. </typeparam>
    public interface IDataGetableComponent<T>
    {
        T GetComponentData();
    }
    /// <summary>
    ///     �� �������̽��� ����ϴ� ������Ʈ�� ����ȭ ������ Ŭ������ ��ü�� ���� �� �ֽ��ϴ�. </summary>
    /// <typeparam name="T">
    ///     ������ �Է��� ���� Ŭ�����Դϴ�. </typeparam>
    public interface IDataSetableComponent<T>
    {
        void SetComponentData(T componentData);
    }
    #endregion

    /// <summary>
    /// �̸��� ���� ��ü�� ������ �� �ֵ��� �ϴ� �������̽��Դϴ�.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         �ݵ�� �ؾ� �� ��:
    ///     </para>
    ///     <para>
    ///         �Ӽ� Name�� �����ϴ� �ʵ�� Public�̿��� �մϴ�.
    ///     </para>
    /// </remarks>
    public interface INameKey
    {
        /// <summary>
        /// �� ��ü�� �̸��Դϴ�.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// ���� ������ �� �ִ� �������̽��Դϴ�.
    /// INameKey�� ����Ѵٸ� NamedQuantityArrayHelper�� ������ ���� �� �ֽ��ϴ�.
    /// </summary>
    public interface IMeasurable // ���� ���ְų� ������ �� �ִ� Ŭ������ �޼��带 ������!
    {
        /// <summary>
        /// �� ��ü�� ���Դϴ�.
        /// </summary>
        public float Quantity { get; set; }
    }

    // Edited Element
    /// <summary>
    /// ��ü�� �̸��� ���� �� �ְ�, �� ��ü�� ���� ������ �� �ֵ��� �ϴ� �������̽��Դϴ�.
    /// </summary>
    public interface INamedQuantity : INameKey, IMeasurable
    {

    }

    // �� TYPE[]�� ������� �ʴ°���? => �迭 Ŭ������ �Լ��� �� �߰��� �� �־����� ���ھ�. Ȯ�� �޼����� �ٸ� ����� �ֱ� �ѵ�
    /// <summary>
    /// � �༮�� �迭�Դϴ�.
    /// </summary>
    /// <typeparam name="ElementType">�� �迭�� ������ Ÿ���Դϴ�.</typeparam>
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
    /// ������ �ƴ� �����Դϴ�. ���� 0�� �� �ֽ��ϴ�.
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
    /// �̸��� ���� �� �ְ�, ���� ������ �� �ֽ��ϴ�.
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
        /// <typeparam name="T">INAmeKey�� ����ϴ� Ÿ���Դϴ�.</typeparam>
        /// <param name="population">ã������ ������ �ִٰ� �����Ǵ� ������(population)�Դϴ�.</param>
        /// <param name="sample">�����ܿ��� ã������ ǥ������(sample)�Դϴ�.</param>
        /// <returns> �������� �ε��� ���� �迭�� ��Ƴ��ϴ�.
        ///     <para>
        ///         ���� ��� sample[A]�� Name�� population[B]�� Name�� ���� ��, ��ȯ�ȹ迭[A]�� B�Դϴ�. �̶� B�� �ּڰ��Դϴ�.
        ///     </para>
        ///     <para>
        ///         ���� ���� ã�� �������� ��ȯ�� �迭[A] == -1�Դϴ�.
        ///     </para>
        /// </returns>
        public static int[] Find<ArrayClass, ElementClass>(ArrayClass population, ArrayClass sample)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : INameKey
        {
            Hack.Say(Hack.isDebugBaseComponent, $"DEBUG_BaseComponent.Find() : sample.Length = {sample.Length}");

            // �ϴ� ��� ���縦 ã�� ���ߴٰ� �����մϴ�.
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
        /// ��� ������ quantity ���� ������� �Ǵ��մϴ�.
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
        /// ��� �迭�� ������ �ϳ��� Quantity ���� ������ true�� �����մϴ�.
        /// </summary>
        /// <typeparam name="ArrayClass"></typeparam>
        /// <typeparam name="ElementClass"></typeparam>
        /// <param name="target">��� ���� ã������ �迭�Դϴ�.</param>
        /// <returns></returns>
        public static bool IsAnyElementPositive<ArrayClass, ElementClass>(ArrayClass target)
            where ArrayClass : IArray<ElementClass>
            where ElementClass : IMeasurable
        {
            for(int index = 0; index < target.Length; ++index)
            {
                if (target[index].Quantity > 0.0f) return true;
            }
            return false; // ��� ���� ã�� ���߽��ϴ�.
        }
    }

    public static class NamedQuantityArrayHelper
    {
        // ��Ģ����
        public static void Add<ArrayClass, ElementClass>(ref ArrayClass target, ArrayClass addend)
            where ArrayClass : IArray<ElementClass>, IExpandable<ElementClass>
            where ElementClass : INameKey, IMeasurable
        {
#warning indexArray�� ������ ���� �� ���ƿ�!
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
                        $"DEBUG_NamedQuantityArrayHelper.Add() : target�� ���� = {target.Length}");

                    target[indexArray[index]].Quantity += addend[index].Quantity;
                }
            }
        }
        /// <summary>
        /// dividend[A].Name == divisor[B].Name�� ��, dividend[A].quantity / divisor[B].quantity�� �� ���� ���� ���� ���� ���մϴ�.
        /// </summary>
        /// <typeparam name="ArrayClass">�迭 Ŭ�����Դϴ�.</typeparam>
        /// <typeparam name="ElementClass">���� Ŭ�����Դϴ�.</typeparam>
        /// <param name="dividend">������ ���ϴ� ȭ�й����Դϴ�. �������� �������� ����մϴ�</param>
        /// <param name="divisor">������ ȭ�й����Դϴ�. �������� ������ ����մϴ�.</param>
        /// <returns>
        ///     <para>
        ///         ������ ���ҵ��� ������ �� �� �� ���� �ּڰ��� ���մϴ�.
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
        /// ���� �������� �ʴ� ����� ������� ���, �� �κ��� ���õ˴ϴ�.
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

        // ������ ����
        // �ʿ信 ���� ���ܳ� �Լ����Դϴ�. ���� �⺻�� �Ǵ� ���� �Լ��� ���޾� ȣ���Ͽ� ��ü�� �� �ְ�����, �ƹ����� �̰� �� ������ ���ٰ� �߷��ؼ� ��������ϴ�.
        /// <summary>
        /// �� ���� ����Ǵ� ���� �����մϴ�. ���� ��� 5�� 3�̶�� 2�� 0�� �����ϴ�. �������� �������� �ϱ� ������, �� ���� ������� ���� ������ �þ �� �ֽ��ϴ�.
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
        /// Subtract�� �����ϳ�, �������� �ʴ� �κп� ���ؼ��� ������� �����մϴ�.
        /// �ʿ��� ���Ұ� ���������� �������ݴϴ�.
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
                // ������ ���� �����̹Ƿ�, ���並 �߰��մϴ�.
                if (arrayIndex[index] == NOT_FOUND)
                {
                    result.Add(demand[index]);
                    continue;
                }

                // ������ ���並 �Ѿ�Ƿ�, �����մϴ�.
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
    /// ����, �߸�, ������ ���� ���� �� �ִ� �ڷ����Դϴ�.
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
        // null�� �� ������ �ľ��ϰ� nullIndex[]�� �����Ѵ�
        // element.length - nullIndex.length �� ��ŭ �迭�� ũ�⸦ �ø���
        // ���� ���ܳ� ������ �ε����� nullIndex�� �����Ѵ�.
        // �� �������� ���Ҹ� �ִ´�.
        // nullIndex�� �����Ѵ�

        int[] result = new int[elements.Length];
        int position = 0; // result�� index
        for(int index = 0; index < array.Length; ++index)
        {
            if (array[index] == null)
            {
                result[position] = index;
                position++;
            }
            if (position >= elements.Length) break;
        }
        // ���� array�� null�� ���� ������ elements.Length���� ������, �Ʒ� �ڵ尡 ����� ���Դϴ�.
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
