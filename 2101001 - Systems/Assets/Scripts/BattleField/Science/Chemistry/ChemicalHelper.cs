using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning 해야 할 일 : 화학 반응 실행하는거 => 이후 HumanUnitBase 손보기
/// <summary>
/// 클래스의 역할 : 작업 클래스
/// 기본이 되는 클래스의 묶음을 가지고 있는 컴포넌트입니다.
/// 웬만한 chemical 컴포넌트는 이 클래스를 상속하여 사용합니다.
/// <para>
///     이 클래스는 GameManager의 Chemical에서 독립하기 위한 클래스입니다.
/// </para>
/// </summary>
public class ChemicalHelper : BaseComponent
{

    const int NOT_FOUND = -1;

    // 이런 내용이 있습니다
    // 클래스 Chemical의 정의
    // 콜랙션 클래스 Chemicals의 정의


    /// <summary>
    ///     단일 종류의 화학 물질을 표현하는 클래스입니다. 괜히 Region으로 분할하지 않아도 되도록요.
    /// </summary>
    [System.Serializable]
    public class Chemical : INamedQuantity
    {
        /// <summary>
        /// 이 화학물질의 종류입니다.
        /// </summary>
        public string matter;
        /// <summary>
        /// 이 화학물질의 양입니다
        /// </summary>
        public float quantity;

        /// <summary>
        /// 경고! 이 변수는 직렬화가 되지 않을 것입니다.
        /// </summary>
#warning (수정 강력 권고) 경고! 이 변수는 직렬화가 되지 않습니다. / 에너지 저항 값은 change 관련 컴포넌트에서 조정해 줄 것입니다. 만약 여기에 존재한다면, 같은 물질이라 할지라도 다른 반응값을 가져올 수 있기 때문입니다.
        public List<EnergyResistPair> energyResistList;

        public float Quantity
        {
            get => quantity;
            set => quantity = value;
        }
        public string Name
        {
            get => matter;
            set => matter = value;
        }

        public bool TryGetResistant(string energyName, out float receveVariable)
        {
            for(int index = 0; index < energyResistList.Count; index++)
            {
                if (energyResistList[index].energyType.Equals(energyName))
                {
                    receveVariable = energyResistList[index].resistance;
                    return true;
                }
            }
            receveVariable = 0.0f;
            return false;
        }
    }
    /// <summary>
    /// 여러개의 화학 물질을 표현하는 클래스입니다.
    /// </summary>
    public class Chemicals : IEnumerable, IEnumerator, IArray<Chemical>, IExpandable<Chemical>
    {
        public Chemical[] self;

        public int Length { get => self.Length; }
        public float Quantity
        {
            get
            {
                float result = 0.0f;
                for (int index = 0; index < self.Length; index++)
                    result += self[index].quantity;
                return result;
            }
        }
        public System.Object Current
        {
            get => self[mPosition];
        }
        public Chemical this[int _index]
        {
            get => self[_index];
            set => self[_index] = value;
        }
        public Chemical this[string _matter]
        {
            get
            {
                int index = findIndex(_matter);
                if (index == NOT_FOUND)
                {
                    return null;
                }
                else
                {
                    return self[index];
                }
            }
            set
            {
                int index = findIndex(_matter);
                if (index == NOT_FOUND)
                {
                    self[index] = value;
                }
                else
                {
                    Add(value);
                }
            }
        }

        int mPosition = -1;

        public Chemicals()
        {
            self = new Chemical[] { };
        }
        public Chemicals(params Chemical[] chemicals)
        {
            Add(chemicals);
        }


        public bool MoveNext()
        {
            if (mPosition >= self.Length - 1) return false;
            else
            {
                mPosition++;
                return true;
            }
        }
        /// <summary>
        ///     매개변수로 받은 Chemicals를 self에서 찾아 quantity를 매개변수로 받은 양만큼 덜어냅니다.
        /// </summary>
        /// <param name="removingItem"> 덜어내고자 하는 화학 물질입니다.</param>
        /// <returns>덜어냈는데, 매개변수가 지정했던 self의 chemical의 quantity가 한 번만이라도 음수인지 여부를 리턴합니다.</returns>
        public bool Remove(Chemicals removingItem)
        {
            // 내용물을 제거해본다
            bool result = false; // 빼기를 진행하다 quantity가 음수가 된, 혹은 음수였던 chemical이 존재하는지 여부
            int[] indexArray = NameKeyArrayHelper.Find<Chemicals, Chemical>(this, removingItem);
            for (int index = 0; index < indexArray.Length; ++index)
            {
                self[indexArray[index]].quantity -= removingItem[index].quantity;
                if (self[indexArray[index]].quantity < 0.0f)
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 매개변수에 들어간 화학 물질을 현재 객체가 가지고 있는지 여부를 리턴해줍니다,
        /// </summary>
        /// <param name="searchingItem">찾고자 하는 화학물질입니다.</param>
        /// <returns>현재 객체에서 찾고자하는 각 화학 물질이 어느 인덱스에 있는지를 담는 배열입니다.</returns>
        public int[] Find(Chemicals searchingItem)
        {
            int[] result = new int[searchingItem.Length];
            for (int paramIndex = 0; paramIndex < searchingItem.Length; paramIndex++)
            {
                bool isFound = false;
                for (int selfIndex = 0; selfIndex < this.Length; selfIndex++)
                {
                    if (searchingItem[paramIndex].matter.Equals(self[selfIndex].matter))
                    {
                        result[paramIndex] = selfIndex;
                        isFound = true;
                        break;
                    }
                }
                if (isFound == false)
                {
                    result[paramIndex] = NOT_FOUND;
                }
            }
            return result;
        }
        /// <summary>
        ///     Find(Chemicals)와 동일하나, 하나라도 찾던게 없거나, 멤버의 값이 양의 값이 아니라면 { -1 }을 리턴합니다.
        /// </summary>
        /// <param name="searchingItem"></param>
        /// <returns></returns>
        /// <seealso cref="Find(Chemicals)">sss</seealso>/>
        public int[] FindPositiveAll(Chemicals searchingItem)
        {
            int[] result = new int[searchingItem.Length - 1];
            for (int paramIndex = 0; paramIndex < searchingItem.Length; ++paramIndex)
            {
                bool isFound = false;
                for(int selfIndex = 0; selfIndex < self.Length; ++selfIndex)
                {
                    if (searchingItem[paramIndex].matter.Equals(self[selfIndex].matter))
                    {
                        if(self[selfIndex].Quantity <= 0.0f)
                        {
                            return new int[] { NOT_FOUND };
                        }

                        isFound = true;
                        result[paramIndex] = selfIndex;
                        break;
                    }
                }
                if(isFound == false)
                {
                    return new int[] { NOT_FOUND };
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisor">나누는 화학물질입니다. 나눗셈의 제수와 비슷합니다.</param>
        /// <returns>
        ///     <para>
        ///         각각의 chemical을 나누기 한 값 중 가장 최솟값을 구합니다.
        ///     </para>
        /// </returns>
        public float Divide(Chemicals divisor)
        {
            return NamedQuantityArrayHelper.Divide<Chemicals, Chemical>(this, divisor);
        }
        public float GetResistance(string EnergyName)
        {
            // 저항도는 비율 따라 계산합니다.
            // N개의 물질중 물질 A의 비율 : 물질 A의 양 / 모든 물질의 양
            // 에너지 a에 대한 저항도 SUM 1 -> A -> N(A의 비율 * A의 a에 대한 저항도)

            float result = 0.0f;
            for (int index = 0; index < self.Length; index++)
            {
                float resistance;
                if (self[index].TryGetResistant(EnergyName, out resistance))
                {

                    result += self[index].quantity * resistance / Quantity;
                }
                else
                {
                    // 에너지가 존재하지 않습니다.
                    return NOT_FOUND;
                }
            }
            return result;
        }
        public INameKey GetItem(int index) => self[index];
        public void Add(Chemical chemical) { Add(chemical); }
        public void Add(params Chemical[] newChemicals)
        {
            // 캐미컬을 추가합니다.
            // 빈 공간 우선으로 채워집니다.

            if (self == null)
            {
                self = newChemicals;
                return;
            }



            // 추가하려는 캐미컬들을 하나씩 선택하여 넣습니다.
            for (int newChemIndex = 0; newChemIndex < newChemicals.Length; newChemIndex++)
            {
                int foundIndex = findIndex(newChemicals[newChemIndex].matter);
                if (foundIndex == NOT_FOUND)
                {
                    this[foundIndex].quantity += newChemicals[newChemIndex].quantity;
                }
                else
                {
                    int selfIndex = 0;
                    bool isFound = false;
                    for (; selfIndex < self.Length; selfIndex++)
                    {
                        if (self[selfIndex] == null)
                        {
                            self[selfIndex] = newChemicals[newChemIndex];
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound) continue;

                    Array.Resize(ref self, self.Length + 1);
                    self[self.Length - 1] = newChemicals[newChemIndex];
                }


            }
        }
        public void Add(Chemicals newChemicals)
        {
            if (self == null)
            {
                foreach(Chemical one in newChemicals)
                    Add(one);
                return;
            }

            int[] indexArray = NameKeyArrayHelper.Find<Chemicals, Chemical>(this, newChemicals);

            for(int index = 0; index < indexArray.Length; index++)
            {
                if (indexArray[index] == NOT_FOUND)
                {
                    AddElementArray(ref self, newChemicals[index]);
                }
                else
                {
                    self[indexArray[index]].Quantity = newChemicals[index].Quantity;
                }
            }
        }
        /// <summary>
        /// IEnumerable의 인터페이스 함수
        /// </summary>
        public void Reset()
        {
            mPosition = -1;
        }
        public IEnumerator GetEnumerator()
        {
            for(int index = 0; index < self.Length; index++)
            {
                yield return self[index];
            }
        }
        /// <summary>
        /// 이름을 가지고 캐미컬의 인덱스를 찾습니다.
        /// </summary>
        /// <param name="_matter"> 찾으려는 캐미컬의 이름입니다. </param>
        /// <returns> 위치를 찾으면 index를 리턴합니다. 그렇지 않으면 -1을 리턴합니다/</returns>
        int findIndex(string _matter)
        {
            for(int index = 0; index < self.Length; index++)
                if (self[index].matter.Equals(_matter))
                    return index;
            return NOT_FOUND;
        }
    }
    [System.Serializable]
    public struct EnergyResistPair
    {
        public string energyType;
        public float resistance;
        //
    }

#warning ChangeHelper 클래스의 ChemicalReaction으로 옮겼습니다.
    /// <summary>
    /// 화학 반응을 나타내기 위한 클래스입니다.
    /// </summary>
    [System.Serializable]
    public class ChemicalReaction
    {
        public float priority;
        public Chemicals input;
        public Chemicals output;
        public EnergyHelper.Energies requiredEnergy;
        public EnergyHelper.Energies generatedEnergy;
    }

    public class ChemicalReactionArray
    {

        ChemicalReaction[] reactionArray; // 항상 priority를 기준으로 정렬되어 있어야 한다.

        public ChemicalReactionArray() { }

#warning 테스트 하지 않음
        public void ActivateReaction(ref Chemicals chemicals, ref EnergyHelper.Energies energies)
        {
            bool foundReaction;
            int allowedLoopCount = 1024; // 이 이상 넘어가면 무한루프인것으로 판단합니다

            do // 가능한 반응이 다 끝날때까지 루프를 돌린다.
            {
                foundReaction = false;
                if(allowedLoopCount <= 0)
                {
                    Debug.LogError("ERROR_ChemicalReactionTable.ActivateReaction() : \n" +
                        "화학 반응이 무한정 일어나고 있습니다. 반응 루프를 강제로 종료합니다.");
                    break;
                }

                // 모든 반응식을 탐색합니다.
                for (int tableIndex = 0; tableIndex < reactionArray.Length; tableIndex++)
                {
                    ChemicalReaction reaction = reactionArray[tableIndex];  

                    int[] chemicalIndexArray = NameKeyArrayHelper.Find<Chemicals, Chemical>(chemicals, reaction.output);
                    int[] energyIndexArray = NameKeyArrayHelper.Find<EnergyHelper.Energies, EnergyHelper.Energy>(energies, reaction.requiredEnergy);

                    if( Array.Exists(chemicalIndexArray, delegate (int one) { return one == NOT_FOUND; }) &&
                        Array.Exists(energyIndexArray, delegate (int one) { return one == NOT_FOUND; }))
                    {
                        // 화학 반응을 찾았습니다!
                        // 나누기 한 값을 구하고 / 그만큼 물질과 에너지를 빼고 / 그만큼 물질과 에너지를 추가합니다
                        float quotientOfChemical =
                            NamedQuantityArrayHelper.Divide<Chemicals, Chemical>(chemicals, reaction.input);
                        float quotientOfEnergy =
                            NamedQuantityArrayHelper.Divide<EnergyHelper.Energies, EnergyHelper.Energy>(energies, reaction.requiredEnergy);
                        float coefficient = Mathf.Min(quotientOfChemical, quotientOfEnergy);

                        if(coefficient < 0)
                        {
                            foundReaction = true;

                            Chemicals removingChemials = reaction.input;
                            NamedQuantityArrayHelper.Multiply<Chemicals, Chemical>(ref removingChemials, coefficient);
                            EnergyHelper.Energies removingEnergies = reaction.requiredEnergy;
                            NamedQuantityArrayHelper.Multiply<EnergyHelper.Energies, EnergyHelper.Energy>(ref removingEnergies, coefficient);
                            NamedQuantityArrayHelper.Subtract<Chemicals, Chemical>(ref chemicals, removingChemials);
                            NamedQuantityArrayHelper.Subtract<EnergyHelper.Energies, EnergyHelper.Energy>(ref energies, removingEnergies);

                            Chemicals addingChemicals = reaction.output;
                            NamedQuantityArrayHelper.Multiply<Chemicals, Chemical>(ref addingChemicals, coefficient);
                            EnergyHelper.Energies addingEnergy = reaction.generatedEnergy;
                            NamedQuantityArrayHelper.Multiply<EnergyHelper.Energies, EnergyHelper.Energy>(ref addingEnergy, coefficient);
                            NamedQuantityArrayHelper.Add<Chemicals, Chemical>(ref chemicals, addingChemicals);
                            NamedQuantityArrayHelper.Add<EnergyHelper.Energies, EnergyHelper.Energy>(ref energies, addingEnergy);
                        }
                    }
                }
                allowedLoopCount--;
            }
            while (foundReaction);




            // 내용물을 빼기

        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <para>
        /// 에너지에 대한 정보를 가지고 있는 AttackClass인 경우, 이 함수를 호출하는 대신 ActivateReaction(ref Chemicals chemicals, ref EnergyHelper.Energy[] energies)를 호출하여야 합니다.
        /// 에너지가 "없는" 경우에만 이 함수를 호출해야 합니다.
        /// 기본 함수는 ActivateReaction(ref Chemicals chemicals, ref EnergyHelper.Energy[] energies) 입니다.
        /// </para>
        /// </remarks>
        /// <param name="target"></param>
        /// <returns></returns>
        public EnergyHelper.Energies ActivateReaction(ref Chemicals chemicals)
        {
            EnergyHelper.Energies result = new EnergyHelper.Energies() { };
            ActivateReaction(ref chemicals, ref result);
            return result;   
        }
    }
}
