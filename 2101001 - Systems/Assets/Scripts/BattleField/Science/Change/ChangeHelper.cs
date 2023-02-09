using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHelper : BaseComponent
{
    // ������ ��Ģ�� ����Ͽ� ���� ��ȭ�� ���� ��� Ŭ�������� �����մϴ�.
    // -- ȭ�� ����
    // �� ������ ȭ�й����� �� ������ �������� ������ ȭ�� ������ �߻��մϴ�.
    // ~~~Controller�� �ش� ���뿡 ���� ǥ�� ������ �ֽ��ϴ�. ChemicalReaction
    // -- ���� �ı�
    // ȭ�� ������ � �������� ��ŭ ������ �� �ִ����� ���� ������ �ֽ��ϴ�.
    // ~~~Controller�� �ش� ���뿡 ���� ǥ�� ������ �ֽ��ϴ�.

    
    //public string Energy
    public class ChemicalReaction
    {
        /// <summary>
        /// ȭ�� ������ �Ͼ�� �����Դϴ�.
        /// </summary>
        public float priority;
        /// <summary>
        /// ������ : ȭ�� ���� ������ ȭ�� �����Դϴ�.
        /// </summary>
        public ChemicalHelper.Chemicals reactants { get; private set; }
        /// <summary>
        /// ������ : ȭ�� ���� ������ ȭ�� �����Դϴ�.
        /// </summary>
        public ChemicalHelper.Chemicals products { get; private set; }
        /// <summary>
        ///     Ȱ��ȭ �������Դϴ�.
        /// </summary>
        /// <remarks>
        ///     ȭ�� ������ �Ͼ�� ���� �ʿ��� �������Դϴ�.
        /// </remarks>
        public EnergyHelper.Energies ActivationEnergy { get; private set; }
        /// <summary>
        /// ȭ�� ���� ���Ŀ� �߻��� �������Դϴ�.
        /// </summary>
        public EnergyHelper.Energies EnergyReaction { get; private set; }

        public ChemicalReaction() { }
        public ChemicalReaction(
            float _priority,
            ChemicalHelper.Chemicals _reactants,
            ChemicalHelper.Chemicals _products,
            EnergyHelper.Energies _activationEnergy,
            EnergyHelper.Energies _energyReaction
            )
        {
            this.priority = _priority;
            this.reactants = _reactants;
            this.products = _products;
            this.ActivationEnergy = _activationEnergy;
            this.EnergyReaction = _energyReaction;
        }
    }

    [System.Serializable]
    public class ChemicalReactionTable : IArray<ChemicalReaction>, IExpandable<ChemicalReaction>
    {
        public int Length
        {
            get => reactions.Length;
        }

        public ChemicalReaction this[int index]
        {
            get
            {
                if (reactions == null) reactions = new ChemicalReaction[] { };
                return reactions[index];
            }
            set
            {
                if (reactions == null) reactions = new ChemicalReaction[] { value };
                else { reactions[index] = value; }
            }
        }

        /// <summary>
        ///     ��� �߻� ������ ȭ�� ������ �迭�Դϴ�.
        /// </summary>
        /// <remarks>
        ///     [���ĵ�] : priority�� �������� ���� ���� �ε����� ������ �ֽ��ϴ�.
        /// </remarks>
        public ChemicalReaction[] reactions;

#warning ���� Ž������ ���׷��̵� �� �� �ֽ��ϴ�.
#warning �Լ� ���� ���Դϴ�.
        /// <summary>
        /// ȭ�� ������ �߰��ϵ�, priority�� ���� ������ �������ϴ�.
        /// </summary>
        /// <param name="newElement"></param>
        public void Add(ChemicalReaction newElement)
        {
            
        }
        /// <summary>
        /// ���� ù��°�� �߰ߵ� ȭ�� ������ ã���ϴ�.
        /// </summary>
        /// <remarks>
        ///     ���, �� �Լ��� �� �۵��Ϸ���, reactions�� priority �������� ���ĵǾ� �־�� �մϴ�.
        /// </remarks>
        /// <param name="chemicals"></param>
        /// <param name="energies"></param>
        /// <returns>ȭ�� ������ ã���� �ε�����, �׷��� ������ -1�� �����մϴ�.</returns>
        public int GetIndex(ChemicalHelper.Chemicals chemicals, EnergyHelper.Energies energies)
        {
            for(int index = 0; index < reactions.Length; ++index)
            {
                // ĳ���ð� �������� �Ѵ� �����ϴ��� üũ�Ѵ�.
                int[] chemicalIndex = NamedQuantityArrayHelper.FindPositiveAll<ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (chemicals, reactions[index].reactants);
                int[] energyIndex = NamedQuantityArrayHelper.FindPositiveAll<EnergyHelper.Energies, EnergyHelper.Energy>
                    (energies, reactions[index].ActivationEnergy);

                // �������� �ʴ� ���
                if (chemicalIndex[0] == -1 || energyIndex[0] == -1)
                {
                    continue;
                }
                else
                {
                    return index;
                }
            }
            return -1; // ȭ�� ������ �������� �ʽ��ϴ�.
        }

        
    }

    // EnergyResist < ChemicalEnergyResist < ChemicalEnergyResistTable

    /// <summary>
    /// ����� ������ ���� �������� ���� ���װ�
    /// </summary>
    /// <remarks>
    /// ���ط� = Mathf.Max( energy����.amount - resistanceDefense , 0 ) * resistanceRatio
    /// </remarks>
    [System.Serializable]
    public struct EnergyResist : INameKey
    {
        /// <summary>
        /// � �������� ���� ���װ������� ��Ÿ���ϴ�.
        /// </summary>
        public string energyType;
        /// <summary>
        /// �������װ� : �������� ���� resistanceDefense��ŭ ���ε�, �� ���� ��ŭ ���ظ� �������� ���� �����Դϴ�.
        /// </summary>
        public float resistanceRatio;
        /// <summary>
        /// ������װ� : �������� ��������, �⺻������ ��� �������� ���Դϴ�. ���� ���˴ϴ�.
        /// </summary>
        public float resistanceDefense;

        public string Name
        {
            get => energyType;
            set => energyType = value;
        }
    }

    [System.Serializable]
    public class ChemicalEnergyResist : INameKey, IArray<EnergyResist>, IExpandable<EnergyResist>
    {
        public string ChemicalName;

        public int Length
        {
            get => m_self.Length;
        }
        public string Name
        {
            get => ChemicalName;
            set => ChemicalName = value;
        }
        public EnergyResist this[int index]
        {
            get => m_self[index];
            set => m_self[index] = value;
        }
        public EnergyResist this[string nameOfEnergy] // ���Ǽ� �ε���
        {
            get
            {
                int index = NameKeyArrayHelper.Find
                    <ChemicalEnergyResist, EnergyResist>
                    (this, nameOfEnergy);
                return m_self[index];
            }
            set
            {
                int index = NameKeyArrayHelper.Find
                    <ChemicalEnergyResist, EnergyResist>
                    (this, nameOfEnergy);
                m_self[index] = value;
            }
        }

        /// <summary>
        ///     �������� ������. ��
        /// </summary>
        public EnergyResist[] m_self;

        public void Add(EnergyResist element)
        {
            AddElementArray(ref m_self, element);
        }
    }

    [System.Serializable]
    public class ChemicalEnergyResistTable : IArray<ChemicalEnergyResist>, IExpandable<ChemicalEnergyResist>
    {
        public int Length
        {
            get => m_self.Length;
        }


        public ChemicalEnergyResist this[int index]
        {
            get => m_self[index];
            set => m_self[index] = value;
        }
        public ChemicalEnergyResist this[string name]
        {
            get
            {
                int index = NameKeyArrayHelper.Find
                    <ChemicalEnergyResistTable, ChemicalEnergyResist>
                    (this, name);
                return m_self[index];
            }
            set
            {
                int index = NameKeyArrayHelper.Find
                    <ChemicalEnergyResistTable, ChemicalEnergyResist>
                    (this, name);
                m_self[index] = value;
            }
        }

        public ChemicalEnergyResist[] m_self;

        public void Add(ChemicalEnergyResist element)
        {
            AddElementArray(ref m_self, element);
        }
    }
}
