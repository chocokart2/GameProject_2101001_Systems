using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning �ؾ� �� �� : ȭ�� ���� �����ϴ°� => ���� HumanUnitBase �պ���
/// <summary>
/// Ŭ������ ���� : �۾� Ŭ����
/// �⺻�� �Ǵ� Ŭ������ ������ ������ �ִ� ������Ʈ�Դϴ�.
/// ������ chemical ������Ʈ�� �� Ŭ������ ����Ͽ� ����մϴ�.
/// <para>
///     �� Ŭ������ GameManager�� Chemical���� �����ϱ� ���� Ŭ�����Դϴ�.
/// </para>
/// </summary>
public class ChemicalHelper : BaseComponent
{

    const int NOT_FOUND = -1;

    // �̷� ������ �ֽ��ϴ�
    // Ŭ���� Chemical�� ����
    // �ݷ��� Ŭ���� Chemicals�� ����


    /// <summary>
    ///     ���� ������ ȭ�� ������ ǥ���ϴ� Ŭ�����Դϴ�. ���� Region���� �������� �ʾƵ� �ǵ��Ͽ�.
    /// </summary>
    [System.Serializable]
    public class Chemical : INamedQuantity
    {
        /// <summary>
        /// �� ȭ�й����� �����Դϴ�.
        /// </summary>
        public string matter;
        /// <summary>
        /// �� ȭ�й����� ���Դϴ�
        /// </summary>
        public float quantity;

        /// <summary>
        /// ���! �� ������ ����ȭ�� ���� ���� ���Դϴ�.
        /// </summary>
#warning (���� ���� �ǰ�) ���! �� ������ ����ȭ�� ���� �ʽ��ϴ�. / ������ ���� ���� change ���� ������Ʈ���� ������ �� ���Դϴ�. ���� ���⿡ �����Ѵٸ�, ���� �����̶� ������ �ٸ� �������� ������ �� �ֱ� �����Դϴ�.
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
    /// �������� ȭ�� ������ ǥ���ϴ� Ŭ�����Դϴ�.
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
        ///     �Ű������� ���� Chemicals�� self���� ã�� quantity�� �Ű������� ���� �縸ŭ ������ϴ�.
        /// </summary>
        /// <param name="removingItem"> ������� �ϴ� ȭ�� �����Դϴ�.</param>
        /// <returns>����´µ�, �Ű������� �����ߴ� self�� chemical�� quantity�� �� �����̶� �������� ���θ� �����մϴ�.</returns>
        public bool Remove(Chemicals removingItem)
        {
            // ���빰�� �����غ���
            bool result = false; // ���⸦ �����ϴ� quantity�� ������ ��, Ȥ�� �������� chemical�� �����ϴ��� ����
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
        /// �Ű������� �� ȭ�� ������ ���� ��ü�� ������ �ִ��� ���θ� �������ݴϴ�,
        /// </summary>
        /// <param name="searchingItem">ã���� �ϴ� ȭ�й����Դϴ�.</param>
        /// <returns>���� ��ü���� ã�����ϴ� �� ȭ�� ������ ��� �ε����� �ִ����� ��� �迭�Դϴ�.</returns>
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
        ///     Find(Chemicals)�� �����ϳ�, �ϳ��� ã���� ���ų�, ����� ���� ���� ���� �ƴ϶�� { -1 }�� �����մϴ�.
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
        /// <param name="divisor">������ ȭ�й����Դϴ�. �������� ������ ����մϴ�.</param>
        /// <returns>
        ///     <para>
        ///         ������ chemical�� ������ �� �� �� ���� �ּڰ��� ���մϴ�.
        ///     </para>
        /// </returns>
        public float Divide(Chemicals divisor)
        {
            return NamedQuantityArrayHelper.Divide<Chemicals, Chemical>(this, divisor);
        }
        public float GetResistance(string EnergyName)
        {
            // ���׵��� ���� ���� ����մϴ�.
            // N���� ������ ���� A�� ���� : ���� A�� �� / ��� ������ ��
            // ������ a�� ���� ���׵� SUM 1 -> A -> N(A�� ���� * A�� a�� ���� ���׵�)

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
                    // �������� �������� �ʽ��ϴ�.
                    return NOT_FOUND;
                }
            }
            return result;
        }
        public INameKey GetItem(int index) => self[index];
        public void Add(Chemical chemical) { Add(chemical); }
        public void Add(params Chemical[] newChemicals)
        {
            // ĳ������ �߰��մϴ�.
            // �� ���� �켱���� ä�����ϴ�.

            if (self == null)
            {
                self = newChemicals;
                return;
            }



            // �߰��Ϸ��� ĳ���õ��� �ϳ��� �����Ͽ� �ֽ��ϴ�.
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
        /// IEnumerable�� �������̽� �Լ�
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
        /// �̸��� ������ ĳ������ �ε����� ã���ϴ�.
        /// </summary>
        /// <param name="_matter"> ã������ ĳ������ �̸��Դϴ�. </param>
        /// <returns> ��ġ�� ã���� index�� �����մϴ�. �׷��� ������ -1�� �����մϴ�/</returns>
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

#warning ChangeHelper Ŭ������ ChemicalReaction���� �Ű���ϴ�.
    /// <summary>
    /// ȭ�� ������ ��Ÿ���� ���� Ŭ�����Դϴ�.
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

        ChemicalReaction[] reactionArray; // �׻� priority�� �������� ���ĵǾ� �־�� �Ѵ�.

        public ChemicalReactionArray() { }

#warning �׽�Ʈ ���� ����
        public void ActivateReaction(ref Chemicals chemicals, ref EnergyHelper.Energies energies)
        {
            bool foundReaction;
            int allowedLoopCount = 1024; // �� �̻� �Ѿ�� ���ѷ����ΰ����� �Ǵ��մϴ�

            do // ������ ������ �� ���������� ������ ������.
            {
                foundReaction = false;
                if(allowedLoopCount <= 0)
                {
                    Debug.LogError("ERROR_ChemicalReactionTable.ActivateReaction() : \n" +
                        "ȭ�� ������ ������ �Ͼ�� �ֽ��ϴ�. ���� ������ ������ �����մϴ�.");
                    break;
                }

                // ��� �������� Ž���մϴ�.
                for (int tableIndex = 0; tableIndex < reactionArray.Length; tableIndex++)
                {
                    ChemicalReaction reaction = reactionArray[tableIndex];  

                    int[] chemicalIndexArray = NameKeyArrayHelper.Find<Chemicals, Chemical>(chemicals, reaction.output);
                    int[] energyIndexArray = NameKeyArrayHelper.Find<EnergyHelper.Energies, EnergyHelper.Energy>(energies, reaction.requiredEnergy);

                    if( Array.Exists(chemicalIndexArray, delegate (int one) { return one == NOT_FOUND; }) &&
                        Array.Exists(energyIndexArray, delegate (int one) { return one == NOT_FOUND; }))
                    {
                        // ȭ�� ������ ã�ҽ��ϴ�!
                        // ������ �� ���� ���ϰ� / �׸�ŭ ������ �������� ���� / �׸�ŭ ������ �������� �߰��մϴ�
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




            // ���빰�� ����

        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <para>
        /// �������� ���� ������ ������ �ִ� AttackClass�� ���, �� �Լ��� ȣ���ϴ� ��� ActivateReaction(ref Chemicals chemicals, ref EnergyHelper.Energy[] energies)�� ȣ���Ͽ��� �մϴ�.
        /// �������� "����" ��쿡�� �� �Լ��� ȣ���ؾ� �մϴ�.
        /// �⺻ �Լ��� ActivateReaction(ref Chemicals chemicals, ref EnergyHelper.Energy[] energies) �Դϴ�.
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
