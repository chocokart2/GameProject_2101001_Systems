using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHelper : BaseComponent
{
    /// <summary>
    /// 에너지가 무엇인지
    /// </summary>
    [System.Serializable]
    public class Energy : INamedQuantity
    {
        public string type;
        public float amount;

        public string Name
        {
            get => type;
            set => type = value;
        }
        public float Quantity
        {
            get => amount;
            set => amount = value;
        }
    }

    [System.Serializable]
    public class Energies : INameKeyArray<Energy>, IExpandable<Energy>
    {
        public Energy this[int index]
        {
            get => self[index];
            set => self[index] = value;
        }
        public bool HasEnergy
        {
            get
            {
                bool result = false;
                for(int index = 0; index < self.Length; index++)
                {
                    result |= (self[index]?.Quantity > 0.0f);
                }
                return result;
            }
        }
        public int Length
        {
            get => self.Length;
        }

        public Energy[] self;

        public Energies() { }
        public Energies(params Energy[] energies)
        {
            self = energies;
        }
        
        public void Add(Energy element)
        {
            AddElementArray<Energy>(ref self, element);
        }
    }
}
