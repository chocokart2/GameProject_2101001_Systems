using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHelper : BaseComponent
{
    // 과학적 규칙에 기반하여 생긴 변화에 대한 기반 클래스들을 정의합니다.
    // -- 화학 반응
    // 두 덩이의 화학물질과 두 덩이의 에너지가 합쳐져 화학 반응이 발생합니다.
    // ~~~Controller는 해당 내용에 대한 표를 가지고 있습니다. ChemicalReaction
    // -- 물질 파괴
    // 화학 물질이 어떤 에너지를 얼만큼 저항할 수 있는지에 대한 서술이 있습니다.
    // ~~~Controller는 해당 내용에 대한 표를 가지고 있습니다.

    
    //public string Energy
    public class ChemicalReaction
    {
        /// <summary>
        /// 화학 반응이 일어나는 순서입니다.
        /// </summary>
        public float priority;
        /// <summary>
        /// 반응물 : 화학 반응 이전의 화학 물질입니다.
        /// </summary>
        public ChemicalHelper.Chemicals reactants { get; private set; }
        /// <summary>
        /// 생성물 : 화학 반응 이후의 화학 물질입니다.
        /// </summary>
        public ChemicalHelper.Chemicals products { get; private set; }
        /// <summary>
        ///     활성화 에너지입니다.
        /// </summary>
        /// <remarks>
        ///     화학 반응이 일어나기 위해 필요한 에너지입니다.
        /// </remarks>
        public EnergyHelper.Energies ActivationEnergy { get; private set; }
        /// <summary>
        /// 화학 반응 이후에 발생된 에너지입니다.
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
        ///     모든 발생 가능한 화학 반응의 배열입니다.
        /// </summary>
        /// <remarks>
        ///     [정렬됨] : priority가 높을수록 낮은 값의 인덱스를 가지고 있습니다.
        /// </remarks>
        public ChemicalReaction[] reactions;

#warning 이진 탐색으로 업그레이드 할 수 있습니다.
#warning 함수 구현 중입니다.
        /// <summary>
        /// 화학 반응을 추가하되, priority에 따라 순서가 정해집니다.
        /// </summary>
        /// <param name="newElement"></param>
        public void Add(ChemicalReaction newElement)
        {
            
        }
        /// <summary>
        /// 가장 첫번째로 발견된 화학 반응을 찾습니다.
        /// </summary>
        /// <remarks>
        ///     경고, 이 함수가 잘 작동하려면, reactions이 priority 기준으로 정렬되어 있어야 합니다.
        /// </remarks>
        /// <param name="chemicals"></param>
        /// <param name="energies"></param>
        /// <returns>화학 반응을 찾으면 인덱스를, 그렇지 않으면 -1을 리턴합니다.</returns>
        public int GetIndex(ChemicalHelper.Chemicals chemicals, EnergyHelper.Energies energies)
        {
            for(int index = 0; index < reactions.Length; ++index)
            {
                // 캐미컬과 에너지가 둘다 존재하는지 체크한다.
                int[] chemicalIndex = NamedQuantityArrayHelper.FindPositiveAll<ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (chemicals, reactions[index].reactants);
                int[] energyIndex = NamedQuantityArrayHelper.FindPositiveAll<EnergyHelper.Energies, EnergyHelper.Energy>
                    (energies, reactions[index].ActivationEnergy);

                // 존재하지 않는 경우
                if (chemicalIndex[0] == -1 || energyIndex[0] == -1)
                {
                    continue;
                }
                else
                {
                    return index;
                }
            }
            return -1; // 화학 반응이 존재하지 않습니다.
        }

        
    }

    // EnergyResist < ChemicalEnergyResist < ChemicalEnergyResistTable

    /// <summary>
    /// 대상이 가지는 단일 에너지에 대한 저항값
    /// </summary>
    /// <remarks>
    /// 피해량 = Mathf.Max( energy객제.amount - resistanceDefense , 0 ) * resistanceRatio
    /// </remarks>
    [System.Serializable]
    public struct EnergyResist : INameKey
    {
        /// <summary>
        /// 어떤 에너지에 대한 저항값인지를 나타냅니다.
        /// </summary>
        public string energyType;
        /// <summary>
        /// 비율저항값 : 에너지가 들어와 resistanceDefense만큼 깎인뒤, 이 대상과 얼만큼 피해를 입을지에 대한 비율입니다.
        /// </summary>
        public float resistanceRatio;
        /// <summary>
        /// 방어저항값 : 에너지가 들어왔을때, 기본적으로 깎는 에너지의 양입니다. 먼저 계산됩니다.
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
        public EnergyResist this[string nameOfEnergy] // 편의성 인덱서
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
        ///     수정하지 마세요. 프
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
