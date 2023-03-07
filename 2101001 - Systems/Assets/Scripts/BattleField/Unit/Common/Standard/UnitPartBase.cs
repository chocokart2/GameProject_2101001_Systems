using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning ���� ����. �������� �Ծ�� �ϴµ� ������ ȸ���Ǵ� ����. ������ 1�� �Ѿ�ϴ�.
/// <summary>
/// '��� ���ֵ��� ~~~Part ������Ʈ'�� �� Ŭ������ ����մϴ�.
/// </summary>
/// <remarks>
///     <para>
///         �� Ŭ������ �� �����ϴ°�?
///     </para>
///     <para>
///         ��� ���ֵ��� ������ �μ� ��ǰ���� ������ �ֽ��ϴ�.
///     </para>
/// </remarks>
public class UnitPartBase : BaseComponent
{

    /// <remarks>
    ///     <para>
    ///         ���� : Part�� ������ ���θ� �����ϴ� ������, Ư�� ����� �ϴ� �����Դϴ�.
    ///     </para>
    ///     <para>
    ///         ��� ���� : UnitPart�� ��� ������ ~~~PartŬ������ �θ� Ŭ�����Դϴ�. �ν��Ͻ��� �������, �� Ŭ������ ������ּ���.
    ///     </para>
    /// </remarks>
    [System.Serializable]
    public abstract class UnitPart
    {

        // ������
        // ȭ�� ����
        // �۵��ϴ� ��
        // �浹 ����

        protected ChangeController changeController; // ȭ�� ������ ����
#warning chemicalController�� ������ changeController�� �Űܰ� �����Դϴ�.
        ChemicalController chemicalController;

        // public
        // STATIC_VALUE
        public const float REACTION_RATIO_CHEMICAL = 0.5f;
        /// <summary>
        /// ȭ�� ������ �ϴµ� ���� �������� �����Դϴ�.
        /// </summary>
        public const float REACTION_RATIO_ENERGY = 0.5f;
        public const float PIERCE_RATIO_CHEMICAL = 0.5f;
        /// <summary>
        /// ���� �������� �ִµ� ���� �������� �����Դϴ�.
        /// </summary>
        public const float PIERCE_RATIO_ENERGY = 0.5f;
        // non Static Value
        /// <summary>
        /// �� UnitPart�� �����ϴ� ȭ�й����Դϴ�.
        /// </summary>
        /// <remarks>
        ///     �� unitPart�� ��ȣ�ϱ� ���� �߰��� �����絵 �����մϴ�. �������� ������ �ֱ� ������ other�� �߰����� �ʰ�, tagged�� chemicalWholeness�� �߰��˴ϴ�.
        /// </remarks>
        //public ChemicalHelper.Chemicals chemicals;
        public ChemicalHelper.Chemicals tagged;
        /// <summary>
        /// �� UnitPart�� "�۵�"�ϴµ� �ʿ�� �ϴ� ȭ�� �����Դϴ�.
        /// </summary>
        /// <remarks>
        /// �䱸�ϴ� ȭ�й����� ���� �ٲ�⵵ �մϴ�
        /// </remarks>
        public ChemicalHelper.Chemicals demand;
        /// <summary>
        ///     �ܺο��� ���� �����Դϴ�.
        /// </summary>
        /// <remarks>
        ///     unitpart�� ��ȭ���� ���� �����ϴ� ȭ�й����� �����մϴ�. ȭ�� ������ ���� �߻���Ű�� ���� �߰��� �͵� �ֽ��ϴ�.
        /// </remarks>
        public ChemicalHelper.Chemicals others;
#warning ChemicalWholeness�� wholeness ���� 0���� ���ɴϴ�! �̸� ��� �ذ��ؾ� ����?
#warning �Ϲ����� wholeness�� �̰��� ����Ű���� ������!
        /// <summary>
        ///     Tagged chemical�� ���� �������� ��Ÿ���ϴ�.
        /// </summary>
        /// <remarks>
        ///     �Ϲ����� wholeness�� �̸� ����ŵ�ϴ�. unitPart�� �����ϴ� ȭ�� �����̱� �����Դϴ�.
        /// </remarks>
        public ChemicalsWholeness chemicalWholeness;
        /// <summary>
        /// �� UnitPart�� �浹 ������ �� ������� ��Ÿ���ϴ�.
        /// </summary>
        public Spheres collisionRangeSphere;
        /// <summary>
        ///     �� ��ǰ�� �浹 ������ ������ü ������� ��Ÿ���ϴ�.
        /// </summary>
        public Vector3[] collisionRangeCuboid;
        /// <remarks>
        ///     <para>
        ///         �� UnitPart�� �󸶳� �ļ��� ���� �ʾҴ����� ��Ÿ���ϴ�.
        ///     </para>
        ///     <para>
        ///         �������� �ļ� / ���� / ������ �ո� / ������ ���� ���Ե˴ϴ�. �̴� �����ϰ� �ִ� chemical�� �Ǵ����� �ʰ�, ���� ���������� Ȱ���� �� ���� ������� �������� �ǹ��մϴ�.
        ///         �����ϰ�, ������ �ʰ� ������ ���� ���̸� �����Ͻø� �˴ϴ�.
        ///     </para>
        ///     <para>
        ///         �� : 0 ~ 1
        ///     </para>
        /// </remarks>
        public virtual float Wholeness
        {
            get => chemicalWholeness.Wholeness;
        }



        public UnitPart()
        {
            Hack.Say(Hack.Scope.UnitPartBase.UnitPart.Constructor, Hack.check.method, this, 
                message: "DEBUG_UnitPart.UnitPart() : �����ڰ� ȣ��Ǿ����ϴ�.");

            tagged = new ChemicalHelper.Chemicals();
            demand = new ChemicalHelper.Chemicals();
            collisionRangeSphere = new Spheres();
            collisionRangeCuboid = new Vector3[0] { };

        }
        public UnitPart(UnitPart data)
        {
#warning ���⿡ ���� �� �� ������
            chemicalController = GameObject.Find("GameManager").GetComponent<ChemicalController>();
            changeController = GameObject.Find("GameManager").GetComponent<ChangeController>();

            this.tagged = data.tagged;
            this.demand = data.demand;
            this.collisionRangeSphere = data.collisionRangeSphere;
            this.collisionRangeCuboid = data.collisionRangeCuboid;
        }

        /// <summary>
        ///      ��� chemicals���� ȭ�� ������ �Ͼ���� �˻��ϰ�, ������ �� ����� this�� �����մϴ�
        ///      �������� �߻��ϸ� �߰� ���ظ� �����ϴ�.
        /// </summary>
        public void CheckChemicalReaction()
        {
            // this.chemicals���� ȭ�� ������ �Ͼ���� �˻��մϴ�.
            CheckChemicalReaction(new ChemicalHelper.Chemicals { }, new EnergyHelper.Energies { });

            // �� ������ ������ �ִ�

        }
        public void CheckChemicalReaction(ChemicalHelper.Chemicals attackChemical, EnergyHelper.Energies attackEnergy)
        {
            EnergyHelper.Energies generatedEnergies = new EnergyHelper.Energies() { };

            NamedQuantityArrayHelper.Add<ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                (ref tagged, attackChemical);
            NamedQuantityArrayHelper.Add<EnergyHelper.Energies, EnergyHelper.Energy>
                (ref generatedEnergies, attackEnergy);

            ChemicalController.reactions.ActivateReaction(ref tagged, ref generatedEnergies);
        }
        /// <summary>
        ///     �ܺο��� �����ϴ� ����� ȣ���ϴ� �Լ��Դϴ�.
        /// </summary>
        /// <param name="injectedMaterials"></param>
        public void CheckChemicalReaction(GameManager.chemical[] injectedChemicals)
        {
            List<GameManager.chemical> InputChemicals = new List<GameManager.chemical>(injectedChemicals);
            
            //chemicals = GameManager.Mix(InputChemicals, chemicals)
            CheckChemicalReaction();
        }
        /// <summary>
        ///     �� UnitPart�� ��� ��ü������ �������� �������� ���ؼ� �����մϴ�.
        /// </summary>
        /// <remarks>
        ///     �ϴ� ������ �ֽ��ϴٸ�, ���߿� ����� Ŭ������ ���ο� ��������� �ִٸ�, �������̵� �Ͻø� �˴ϴ�.
        /// </remarks>
        /// <param name="attack"></param>
        /// <param name="angle">
        ///     �� UnitPart�� ��� ������ �޾Ҵ����� ���� ���� ���Դϴ�.
        ///     �� ���� :
        ///         0.0f ~ 1.0f,
        ///         0.0f : 0��,
        ///         1.0f : 360��
        /// </param>
        public virtual void BeingAttacked(ref AttackClassHelper.AttackInfo attack, float angle)
        {
#warning DEBUG_CODE
            chemicalWholeness[0].Add(new Penetration() { angle = angle, amount = 1.0f });


            // ��� ĳ������ �� / ��ü ĳ������ ���� ������ �ִ� �迭 ����
            // ������ ���� �� �������� �����ش�. �������� �� * ��
            // ����Ʈ�� ������ ����
            // ������ ������
            // �� �������� �� ĳ������ �縸ŭ ����, tagged ĳ���ø� ������.

            // 1. ���� Ȯ���ϱ�. ���� �������ſ� ���� ����Ȱ���, �ƴϸ� �����ѵ��� �¾Ƽ� ���ؿ����� ���� üũ�մϴ�.
            if (IsPassing(angle))
            {
                Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this, message: "������ ����˴ϴ�.");
                return;
            }

            // 2. �й��Ѵ�.
            // ������ ���� �������� ������ ���� �������� �ɰ���. ĳ������ ���� ������ ���� ��ģ ���� �����صд�.
            EnergyHelper.Energies energiesForPierce = attack.energies;
            NamedQuantityArrayHelper.Multiply
                <EnergyHelper.Energies, EnergyHelper.Energy>
                (ref energiesForPierce, PIERCE_RATIO_ENERGY);
            EnergyHelper.Energies energiesForReaction = attack.energies;
            NamedQuantityArrayHelper.Multiply
                <EnergyHelper.Energies, EnergyHelper.Energy>
                (ref energiesForReaction, PIERCE_RATIO_ENERGY);
            ChemicalHelper.Chemicals chemicalsForReaction = new ChemicalHelper.Chemicals();
            NamedQuantityArrayHelper.Add
                <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                (ref chemicalsForReaction, tagged);
            NamedQuantityArrayHelper.Add
                <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                (ref chemicalsForReaction, others);
            NamedQuantityArrayHelper.Add
                <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                (ref chemicalsForReaction, attack.chemicals);

            // 3. ������ ���ݿ� ���� ���ظ� ������.
            // ���� ���̾ ���ʿ� �ִ� �༮���� ó���մϴ�.
            // �������� ���� �����Ͽ� 
            //      (�� : ������ )
            // ���� ���� Layer�� �ִ� chemical���� wholeness �������� �ݿ��մϴ�.
            for(int indexChemical = 0; indexChemical < chemicalWholeness.Length; ++indexChemical)
            {
                if (energiesForPierce.HasEnergy == false) break;

                SingleChemicalWholeness temp = chemicalWholeness[indexChemical];
                pierceOneChemical(ref temp, ref energiesForPierce, angle); //CS0206: �Ӽ� �Ǵ� �ε����� out �Ǵ� ref �Ű� ������ ������ �� �����ϴ�.
                chemicalWholeness[indexChemical] = temp;
            }

            // 3.1. �������� ��, ���� Ŭ������ ������ �ٱ����� ���������� �Ѵ�.
            // �׷��� �ʴٸ� �׳� �����Ұ�����
            if(energiesForPierce.HasEnergy == true)
            {
                NamedQuantityArrayHelper.Add<EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref energiesForPierce, energiesForReaction);
                attack.energies = energiesForPierce;
                return;
            }


            // 4. ȭ�� ���� ������ �����Ѵ�. ����üũ -> �������� -> ����üũ -> ������ / ������ ���� �� ������ ���� ������ �ݺ�
            // ����� �������� ������ �ٱ����� �ű��.
            // 4-a. ���������� ���� Ŭ������ ������ �ٱ����� ���������� �Ѵ�, �����ϰ� ���� �������� �ٱ����� ��������.
            // 4-b. �������� �ʾ��� ��, ���� ��Ʈ�� ���� Ŭ������ ������ �㵵�� �Ѵ�.

            bool isThereChemicalReaction = false; // ȭ�� ������ �߻��Ѵٸ� true�� �ٲ�ϴ�.
            do
            {
                // ȭ�� ���� üũ
                int reactionIndex = ChangeController.Reactions.GetIndex(chemicalsForReaction, energiesForReaction);
                if (reactionIndex == -1) break;
                // ���� ���� ȹ��
                float reactionRatioChemical =
                    NamedQuantityArrayHelper.Divide
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (chemicalsForReaction, ChangeController.Reactions[reactionIndex].reactants);
                float reactionRatioEnergy =
                    NamedQuantityArrayHelper.Divide
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (energiesForReaction, ChangeController.Reactions[reactionIndex].ActivationEnergy);
                float reactionRatio = MathF.Min(reactionRatioChemical, reactionRatioEnergy);

                // ���� ����
                // ĳ���� ����
                // ���� �� ����
                ChemicalHelper.Chemicals reactants = ChangeController.Reactions[reactionIndex].reactants;
                NamedQuantityArrayHelper.Multiply
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, reactionRatio);
                NamedQuantityArrayHelper.Subtract
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref chemicalsForReaction, reactants);

                // ���� �� ���� attackClass -> others -> tagged
                NamedQuantityArrayHelper.RemoveIntersection
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, ref attack.chemicals);
                NamedQuantityArrayHelper.RemoveIntersection
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, ref others);
                NamedQuantityArrayHelper.RemoveIntersection
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, ref tagged);

                // ĳ���� ���ϱ�
                // ���� �� �߰�
                ChemicalHelper.Chemicals products = ChangeController.Reactions[reactionIndex].products;
                NamedQuantityArrayHelper.Multiply
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref products, reactionRatio);
                NamedQuantityArrayHelper.Add
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref chemicalsForReaction, products);

                // ���� �� �߰�
                NamedQuantityArrayHelper.Add
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref others, products);

                // ������ ����
                EnergyHelper.Energies activation = ChangeController.Reactions[reactionIndex].ActivationEnergy;

                NamedQuantityArrayHelper.Multiply
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref activation, reactionRatio);
                NamedQuantityArrayHelper.Subtract
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref energiesForReaction, activation);
                NamedQuantityArrayHelper.Subtract
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref attack.energies, activation);

                // ������ ���ϱ�
                EnergyHelper.Energies generatedEnergy = ChangeController.Reactions[reactionIndex].EnergyReaction;
                NamedQuantityArrayHelper.Multiply
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref generatedEnergy, reactionRatio);

                EnergyHelper.Energies generatedEnergyReaction = generatedEnergy;
                NamedQuantityArrayHelper.Multiply
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref generatedEnergyReaction, REACTION_RATIO_ENERGY);
                EnergyHelper.Energies generatedEnergyPierce = generatedEnergy;
                NamedQuantityArrayHelper.Multiply
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref generatedEnergyReaction, PIERCE_RATIO_ENERGY);

                NamedQuantityArrayHelper.Add
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref attack.energies, generatedEnergy);
                NamedQuantityArrayHelper.Add
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref energiesForReaction, generatedEnergyReaction);
                NamedQuantityArrayHelper.Add
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref energiesForPierce, generatedEnergyPierce);


                // �߻��� �������� ������Ѻ���.
                for(int index = 0; index < chemicalWholeness.Length; ++index)
                {
                    SingleChemicalWholeness temp = chemicalWholeness[index];
                    pierceOneChemical(ref temp, ref energiesForPierce, angle);
                    chemicalWholeness[index] = temp;
                }
                
                // ĳ������ ����Ǿ����� üũ�Ѵ�.
                if(MeasurableArrayHelper.IsAnyElementPositive
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (energiesForPierce))
                {
                    break;
                }

                // �ݺ� ����
                // 1. ȭ�� ������ �ְ�
                // And
                // 2. ĳ������ ������� �ʾ��� ��
            } while (isThereChemicalReaction && true);

            // ȭ�� ������ �������� �����ִ� ���� ���������� �����ϵ��� �Ѵ�.
            for(int index = 0; index < chemicalWholeness.Length; ++index)
            {
                SingleChemicalWholeness temp = chemicalWholeness[index];
                pierceOneChemical(ref temp, ref energiesForReaction, angle);
                chemicalWholeness[index] = temp;
            }

            // ���� / �� ���뿡 ���� �޶����ϴ�.
            if (chemicalWholeness.IsBroken(angle))
            {
                // �����
                // attack.energies = ���뿡���� + ����������
                // ���� => ���ڰ� ���� �˾Ƽ� ���α� / ���� ������ �׳� ���۳� ������ �������� �Ѵ�.

                NamedQuantityArrayHelper.Add
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref attack.energies, energiesForPierce);
                NamedQuantityArrayHelper.Add
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref attack.energies, energiesForPierce);
            }
            else
            {
                // �� �����
                // ������ => �������� �� �����ϱ�..
                // ���� => ���ݿ� ���� ������ ���Ѱ��� ��Ʈ�� ��´�.

                NamedQuantityArrayHelper.Add
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref others, attack.chemicals);
            }
        }
        /// <summary>
        ///     �� ������Ʈ�� �� �������� ������ ���������� üũ�Ͽ� ������ UnitPart�� ������ ��ġ���� üũ�մϴ�.
        /// </summary>
        /// <remarks>
        ///     ��� ĳ������ AngleWholeness�� taggedChemical�� ������ �����ϴ�
        ///     <para>
        ///         BeingAttacked ���ο��� ȣ������ �ʽ��ϴ�.
        ///         �ֳ��ϸ� BeingAttacked�� �����ε��� �� ���� �� �Լ��� �Ź� ȣ���ؾ� �ϴϱ� ���� ������ ������
        ///         ȣ���ڰ� ȣ���ϵ��� �մϴ�.
        ///     </para>
        /// </remarks>
        /// <param name="angle"> �����Ϸ��� �����Դϴ�.</param>
        /// <returns></returns>
        public virtual bool IsPassing(float angle)
        {
            bool isTaggedExist = false;
            // ��� chemical��
            for(int index = 0; index < tagged.Length; ++index)
            {
                if (tagged[index].quantity > 0)
                {
                    isTaggedExist = true;
                    break;
                }
            }
            if (isTaggedExist == false) return false;

            // ��� chemicalWholeness�� üũ�մϴ�.
            Hack.Say(Hack.Scope.UnitPartBase.UnitPart.IsPassing, Hack.check.info, this,
                message: $"chemicalWholeness ���� Null ���� {chemicalWholeness == null}");
            for(int index = 0; index < chemicalWholeness.Length; ++index)
            {
                if (chemicalWholeness[index].GetAngleWholeness(angle) <= 0)
                {
                    return true;
                }
            }

            return false;
        }


#warning �۾���
        /// <summary>
        ///     wholeness�� ������ �ִ� ĳ���ÿ� ���� �������� �����Ű�� �̸� �����մϴ�.
        ///     �ʹ� ������ ������ �ݿ��Ҷ��� �� �Լ��� ����մϴ�
        /// </summary>
        /// <param name="targetChemical">���ظ� ���� ĳ������ �������Դϴ�.</param>
        /// <param name="energies">���� �������Դϴ�. </param>
        /// <param name="angle">���ظ� ���� �����Դϴ�.</param>
        /// <returns>
        ///     �����Ѵٸ� true�� �����մϴ�.
        /// </returns>
        private void pierceOneChemical(ref SingleChemicalWholeness targetChemical, ref EnergyHelper.Energies energies, float angle)
        {
            Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this); // �� �Լ��� ȣ��˴ϴ�.
            // �� ��� ���� ���ط��� ����մϴ�.
            // ��������(angle) / ������ = k�� ���մϴ�.
            // k�� 1.0 �����̸� ���������� 0���� ���鵵�� �ϰ� energies�� �ϳ��� �� (���� ��) = (���� ��) * (1 - k) ��
            // k�� 1.0 �ʰ��� ���� 1�� �����Ͽ� ���������� ����ϴ�

            // ���ط� ���
            float expectedDamages = 0.0f; // ����Ǵ� ������Դϴ�,
            for(int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
            {
                // �ʹ� �� �������.
                ChangeHelper.EnergyResist energyResist = ChangeController.EnergyResists[targetChemical.Name][energies[indexEnergy].Name];

                // �Ӱ� ���ط����� ���� üũ�մϴ�.
                if (energyResist.resistanceDefense < energies[indexEnergy].Quantity)
                {
                    expectedDamages +=
                        (energies[indexEnergy].Quantity - energyResist.resistanceDefense) /
                        (energyResist.resistanceRatio * tagged[targetChemical.Name].Quantity);
                    Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this, message: $"��ȿ ���ظ� �Ծ����ϴ�!\n ���ط� : {(energies[indexEnergy].Quantity - energyResist.resistanceDefense) / (energyResist.resistanceRatio * tagged[targetChemical.Name].Quantity)}");
                }
            }

            // �������� / ������ �� ���� ���մϴ�.
            if(expectedDamages == 0.0f) // 0���� ���� ���� �� �����ϴ�.
            {
                for(int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
                {
                    energies[indexEnergy].Quantity = 0.0f;
                }
                return;
            }
            float angleWholeness = targetChemical.GetAngleWholeness(angle);
            float energyRatio = angleWholeness / expectedDamages;

            // �б⸦ �����ϴ�.
            if (energyRatio <= 1.0) // �������� �ʹ� �����ؼ� ����� �� �����ϴ�.
            {
                Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this, message: $"������ ����˴ϴ�.");
                // �������� 0���� �����
                targetChemical.Add(
                    new Penetration() { amount = angleWholeness, angle = angle }); // amount�� expectedDamages * energyRatio �ε� angleWholeness ���� ����

                // energies �� ���
                for (int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
                    energies[indexEnergy].amount *= 1 - energyRatio;
            }
            else // ���� �������� ���� ��� �� �� �ֽ��ϴ�.
            {
                Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this, message: $"������ ����˴ϴ�.");
                targetChemical.Add(
                    new Penetration() { amount = expectedDamages, angle = angle });

                // energies �� ���
                for (int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
                {
                    energies[indexEnergy].amount = 0.0f;
                }
            }
        }
    }

    /// <summary>
    /// � ����� ���ο� �����ϴ� ĳ���� �ټ��� �󸶳� ���������� ��Ÿ���ϴ�.
    /// </summary>
    /// <remarks>
    ///     �ݵ�� tagged Chemical�� 1��1�� �����ϵ��� �־�� �մϴ�!
    /// </remarks>
    [System.Serializable]
    public class ChemicalsWholeness : IArray<SingleChemicalWholeness>
    {
        public int Length
        {
            get => m_self.Length;
        }
        public float Wholeness
        {
            get
            {
                Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this);
#warning ���⼭ wholeness ������Ʈ �غ���
                Update();
                float result = 0.0f;
                for(int index = 0; index < m_self.Length; index++)
                {
                    Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this, message:$"m_self[{index}].Wholeness = {m_self[index].Wholeness}");
                    result += m_self[index].Wholeness;
                }
                if (m_self.Length <= 0) return 0.0f;
                else return result / m_self.Length;
            }
        }
        public SingleChemicalWholeness this[int index]
        {
            get => m_self[index];
            set => m_self[index] = value;
        }

        /// <summary>
        ///     �������� ������. ����ȭ�� �������� �ʾ� private�� public���� �ٲ��� ���Դϴ�.
        /// </summary>
        public SingleChemicalWholeness[] m_self;

        public bool IsBroken(float angle)
        {
            for(int index = 0; index < m_self.Length; ++index)
            {
                if(m_self[index].GetAngleWholeness(angle) > 0.0f)
                    return false;
            }
            return true;
        }
        public void Update()
        {
            for(int index = 0; index < m_self.Length; index++)
            {
                m_self[index].IntegrateWholeness();
            }
        }
    }
    /// <summary>
    /// � ����� ���ο� �����ϴ� ���� "�Ѱ�"�� �󸶳� ���������� ��Ÿ���ϴ�.
    /// </summary>
    /// <remarks>
    ///     �ַ� � �������� ��ŭ ���ظ� �Ծ������� ǥ���� �� �ֽ��ϴ�.
    /// </remarks>
    [System.Serializable]
    public class SingleChemicalWholeness : INameKey
    {
        /// <summary>
        /// wholeness�� ������ ����� ����� ���ذ��Դϴ�.
        /// </summary>
        /// <remarks>
        /// �� �������� ������ ������ �������� �����������ϴ�.
        /// </remarks>
        public static float PIERCE_VALUE
        {
            get => 0.0f;
        }
        /// <summary>
        /// ������ �ֺ��� ��ġ�� ������ �����Դϴ�.
        /// </summary>
        /// <remarks>
        /// �� �κп� �������� ������ �׷��� ����� V�ڷ� ���̰� �˴ϴ�,
        /// �� ���� 2�谡 ��ǫ ���̱� �����ϴ� ���� ���ΰ� ������ ���� �Ÿ��Դϴ�.
        /// 0.5 �̻��̸� �Ƹ� �������� ������ ����ų ���Դϴ�.
        /// </remarks>
        public static float EFFECT_RANGE
        {
            get => 0.1f;
        }
        /// <summary>
        ///     �׷����� �ִ� ������ ����ȭ �۾��� �պ��ؾ� �ϴ� �����Դϴ�. �ʹ� �۾Ƽ� ������ ������ �����Դϴ�.
        /// </summary>
        /// <remarks>
        ///     ����ȭ�� ���� �����ϴ� ����Դϴ�.
        /// </remarks>
        public static float MERGE_LIMIT_RANGE
        {
            get => 0.0078125f;
        }
        /// <summary>
        /// ��ü���� �������� �����մϴ�.
        /// </summary>
        /// <remarks>
        /// �����ϴ� ������ ���� �䱸�Ҷ��� �ƴ϶�, ��ȭ�� �Ͼ ������ ������ �����Ͽ� �ݿ��մϴ�.
        /// </remarks>
        public float Wholeness
        {
            get => lazyWholeness;
        }
        public int layer; // ������ ������ ���� ��, ���� �켱�����Դϴ�. �尩 � ���� �����մϴ�.
        public string name;
        /// <summary>
        /// ĳ������ �̸��Դϴ�.
        /// </summary>
        public string Name { get => name; set => name = value; }
        public Penetration[] damages; // chemical�� ���� �����Դϴ�.

        private float lazyWholeness = 0.0f;

        // ���� �䱸�ϴ� �������� ������? (���� ���� / �䱸 ����) * �� ������ wholeness

        /// <summary>
        /// ���ݹ��� ��, �� �Լ��� ȣ���մϴ�. Damage�� ����Ϸ��� �õ��ϰ�, �ش������ ����Ǿ�����, �� ���� �����մϴ�.
        /// </summary>
        /// <remarks>
        ///     ���� ���ݽ� ������ �Ǿ� add�� ���� �ʾƾ� �� ���� �ֽ����. �� ��쿡�� �ܺ� �Լ����� ó���ؾ� �մϴ�.
        ///     1. ������ �� �����, �ٸ� ĳ���ÿ��� ó���ؾ� �ϴ°�?
        /// </remarks>
        /// <param name="penetration">
        /// ((���� �������� �� - ������ ������װ�) / (������ �������װ� * ������ �� * wholeness(angle))
        /// </param>
        /// <returns>
        /// ���Ƴ��� ���ϰ� �շ����� �������� �������ݴϴ�. �� ���� ������ �������װ� * ������ �� * wholeness�� ���Ͽ� �����մϴ�.
        /// </returns>
        public float Add(Penetration penetration)
        {
            Hack.Say(Hack.Scope.UnitPartBase.SingleChemicalWholeness.Add, Hack.check.method, this);
            // ������ ���Դϴ�.
            float result = 0.0f;

            // damages�� reflect�� penetration�Դϴ�.
            Penetration reflectedPenetration = new Penetration()
            {
                angle = penetration.angle
            };
            // delta�� �����̸� ����Ǿ����� �ǹ��մϴ�.
            float angleWholeness = GetAngleWholeness(penetration.angle);
            float remainingWholeness = angleWholeness - penetration.amount;
            
            if (angleWholeness <= PIERCE_VALUE)
            {
                // ���ۿ��� ���ƿ� �������̹Ƿ� �״�� �������ݴϴ�.
                result = penetration.amount;
                return result;
            }
            else if (remainingWholeness <= PIERCE_VALUE)
            {
                // �������� �԰�, ����Ǿ����ϴ�.
                result = MathF.Abs(remainingWholeness);
                reflectedPenetration.amount = angleWholeness;

                AddElementArray(ref damages, reflectedPenetration);
                sort();
                return result;
            }
            else
            {
                // �������� �԰�, �������� ���߽��ϴ�.
                result = 0.0f;

                AddElementArray(ref damages, penetration);
                sort();
                return result;
            }
        }
        /// <summary>
        /// �ش��ϴ� ������ �������� �������ݴϴ�.
        /// </summary>
        /// <param name="angle">�����Դϴ� "���� ���� / 360" �� ���� �Է��ؾ� �ϸ�, 0�� 1 ������ ���̿��� �մϴ�.</param>
        /// <returns> �ش��ϴ� ������ �������Դϴ�. </returns>
        public float GetAngleWholeness(float angle)
        {
            Hack.Say(Hack.Scope.UnitPartBase.SingleChemicalWholeness.GetAngleWholeness, Hack.check.method, this);
            // ��ó�ϼ��� ���� ��´�.
            float result = 1.0f;

            for(int value = 1; value >= -1; value--) // value ���� ������ 0�� 1�� ����� ��ġ�� �ɷ� �־� �ǳ��� ������ ������ ������� �����ϴ� ����Դϴ�.
            {
                for (int index = 0; index < damages.Length; index++) // 
                {
                    // ���н��� �̹��� �����ϼ���.
                    Hack.Say(Hack.Scope.UnitPartBase.SingleChemicalWholeness.GetAngleWholeness, Hack.check.info, this,
                        message: $"damages[index].amount = {damages[index].amount},\tvalue : {value},\tangle : {angle}");

                    result -= MathF.Max(0, damages[index].amount - (damages[index].amount / EFFECT_RANGE) * MathF.Abs(angle + value - damages[index].angle));

                }
            }

            if (result < 0) return 0.0f;
            else return result;
        }
#warning �ִ��� ����ȭ�� �����ؾ� �Ѵ�.
        /// <summary>
        ///     �׷����� �����Ͽ� ���� ����ϴ�.
        /// </summary>
        /// <remarks>
        /// ���ظ� ������ ��, �߾�, �� ��Ʈ���� �������� ����µ� �̸� �������� ������ �մϴ�.
        /// ����� ���� ������ �� ������ �����˴ϴ�. lazyWholeness�� ������Ʈ �˴ϴ�.
        /// </remarks>
        /// <returns></returns>
        public float IntegrateWholeness()
        {
            // 360������ �Ѿ�� �༮�� �ִ��� üũ�մϴ�.
            // � �༮.angle - EFFECT_RANGE < 0�̰ų�
            // � �༮.angle + EFFECT_RANGE > 1�� ���� ������ �װ��� pick�մϴ�.

            // 1�ܰ迡�� �ɸ��� �༮�� ������ �༮�� �н��� �ϳ� ������ݴϴ�. y���� ���� ���� �����ϱ� 360 == 0�̹Ƿ� �����Ͽ� �����մϴ�.

            // �� ����Ʈ�� ���Ѵ�.
            // �ʹ� ���� ����Ʈ(x��� y�� ��� �ſ� ���� ���̿��� ��)�� merge�ұ�?

            // ���̸� ���Ѵ�

            // ���� ������ �Ѿ�� ���� �ִ��� Ȯ���մϴ�.
            Penetration[] realDamages = new Penetration[0]{ };
            // ���� ���� �迭 realDamages�� damages���� �ٿ��ֱ� �մϴ�.
            foreach (Penetration one in damages) AddElementArray(ref realDamages, one);

#warning DEBUG_CODE
            for(int index = 0; index < damages.Length; index++) Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this, message: $"position : x = {damages[index].angle}, y = {damages[index].Quantity}");

            for(int index = 0; index < damages.Length; index++)
            {
#warning bugable : damages[index].angle - EFFECT_RANGE�� ���� -1���� �� ���� ��쵵 �ֽ��ϴ�!
                if (damages[index].angle - EFFECT_RANGE < 0.0f) // -1.0f < damages[index].angle - EFFECT_RANGE < 0.0f
                {
                    AddElementArray(ref realDamages,
                        new Penetration()
                        {
                            amount = damages[index].amount,
                            angle = damages[index].angle + 1.0f,
                        });
                }
                if (damages[index].angle + EFFECT_RANGE > 1.0f)
                {
                    AddElementArray(ref realDamages,
                        new Penetration()
                        {
                            amount = damages[index].amount,
                            angle = damages[index].angle - 1.0f
                        });
                }
            }

            // �� ����Ʈ�� ���Ѵ�.
            float[] pointKey = new float[1] { 0.0f }; // ������ wholeness�� ����� �������Դϴ�. �׷����� ���̴� �����Դϴ�.
            for(int index = 0; index < realDamages.Length; index++)
            {
                float[] angleValue = new float[3]
                {
                    realDamages[index].angle - EFFECT_RANGE,
                    realDamages[index].angle,
                    realDamages[index].angle + EFFECT_RANGE
                };

                for (int angleIndex = 0; angleIndex < angleValue.Length; angleIndex++)
                {
                    // 0 <= x < 360�̸� �迭�� ���� ���� �߰��Ѵ�.
                    if (angleValue[angleIndex] >= 0.0f &&
                        angleValue[angleIndex] < 1.0f)
                    {
#warning IndexOutOfRangeException
                        // pointKey�� ������ �����?
                        // �ƴϸ� angleValue�� ������ ����� index�� �����ϱ�?
                        // angleValue�� ����?
                        AddElementArray(ref pointKey, angleValue[angleIndex]);
                    }
                }
            }
            AddElementArray(ref pointKey, 1.0f);
            // ����
            Array.Sort(pointKey);

            // ������ �����ұ�?
            // pointKey�� �����ϼ���.

            // ������ ũ�⸦ ���Ѵ�
            float result = 0.0f;

            for(int index = 0; index < pointKey.Length - 1; ++index)
            {
                float x1 = pointKey[index];
                float y1 = GetAngleWholeness(pointKey[index]);
                float x2 = pointKey[index + 1];
                float y2 = GetAngleWholeness(pointKey[index + 1]);

                // �� ���� y��ǥ�� ����� ��
                if (y1 > 0.0f && y2 > 0.0f)
                {
                    float addValue = getTrapezoid(x1, x2, y1, y2);
                    Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this,
                        message: $"wholeness �� : [{index} of {pointKey.Length - 1}] {result},\t�߰� �� : {addValue}\n���� : {x2-x1},\t���� {y1}, {y2}\t{x1}, {x2}\t{y1}, {y2}");
                    result += addValue;
                    continue;
                }
                // �� ���� �ϳ��� y��ǥ�� ����̰� �ϳ��� ������ ��
                else if (y1 > 0.0f || y2 > 0.0f)
                {
                    float addValue = getTriangle(x1, x2, y1, y2);
                    Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this,
                        message: $"wholeness �� : [{index} of {pointKey.Length - 1}] {result},\t�߰� �� : {addValue}\n���� : {x2 - x1},\t ���� {MathF.Abs(y2 - y1)}\t{x1}, {x2}\t{y1}, {y2}");
                    result += addValue;
                    continue;
                }
                // �� ���� ����� �ƴϹǷ� ������� �ʽ��ϴ�.
            }
            Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this,
                message: $"wholeness �� : {result}");

            lazyWholeness = result;
            return result;
        }
        /// <summary>
        /// ���� / ��ǰ ��ü ������, Damage�� ���� ���� �Ű����� ����ŭ ��ȭ���ݴϴ�.
        /// </summary>
        /// <remarks>
        /// ���� �ɰ��� ���̴� �κк��� ġ���մϴ�, ���� ġ���� ���� ������ ����� ��ó�� �������� �� ���, �� ��ó�� ������ ġ���մϴ�.
        /// </remarks>
        /// <param name="amount">
        /// 0�� 1 ���� ���� �Է��ؾ� �մϴ�.
        /// </param>
        public void Recovory(float amount)
        {

        }
        private void sort()
        {
            Array.Sort<Penetration>(damages,
                delegate (Penetration a, Penetration b)
                {
                    return b.angle.CompareTo(a.angle);
                }
                );
        }
        /// <summary>
        /// target�� ������ ������� �� ��ġ�� ���ϰ�, �װ��� recvArray�� �߰��մϴ�.
        /// </summary>
        /// <param name="recvArray"></param>
        /// <param name="target"></param>
        private void addMergedPoint(ref Vector2[] recvArray, Vector2[] target)
        {
            if (target.Length == 0) return;

            Vector2 result = new Vector2(0.0f, 0.0f);

            for(int index = 0; index < target.Length; ++index)
            {
                result += target[index];
            }
            result /= target.Length;
            AddElementArray(ref recvArray, result);
        }
        /// <summary>
        ///     �ش� ������ ���� wholeness�� ����ϴ�.
        /// </summary>
        /// <remarks>
        /// GetAngleWholeness���� ������ :
        ///     �� �Լ��� �ش� ���� (0 ~ 360)�� �Ѿ ���� �������� �ʽ��ϴ�. ���� �� �Լ��� ȣ�� �� ��,
        ///     �Ű������� �ѱ� ����� �ش� ó���� �Ϸ��ؾ� �մϴ�.
        /// </remarks>
        /// <param name="targetDamages">������ ������ ���� ����Դϴ�.</param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private float getRawAngleWholeness(Penetration[] targetDamages, float angle)
        {
            float result = 1.0f;

            for (int index = 0; index < targetDamages.Length; index++) // 
            {
                // ���н��� �̹��� �����ϼ���.
                result -= MathF.Max(0, targetDamages[index].amount - (targetDamages[index].amount / EFFECT_RANGE) * MathF.Abs(angle - targetDamages[index].angle));
            }

            return result;
        }
        /// <summary>
        /// y1 * y2 < 0�� ��, ����(x1, y1), (x2, y2)���̸� �մ� ����, y = 0, x = (y1�� ����̸� x1 Ȥ�� y2�� ����̸� x2)�� ����� �����ﰢ���� ���̸� �����ݴϴ�.
        /// </summary>
        /// <param name="x1">ù��° ���� x��ǥ�Դϴ�.</param>
        /// <param name="x2">�ι�° ���� x��ǥ�Դϴ�. x1���� �� ū ���� ������ �־�� �մϴ�.</param>
        /// <param name="y1">ù��° ���� y��ǥ�Դϴ�.</param>
        /// <param name="y2">�ι�° ���� y��ǥ�Դϴ�.</param>
        /// <returns>
        /// �ﰢ���� ���� ������ ���մϴ�. ���� �ﰢ���� �ƴϰų�, y1 == y2�̸� -1.0f�� �����մϴ�.
        /// </returns>
        private float getTriangle(float x1, float x2, float y1, float y2)
        {
            // �� ���� ���� �Ű� ������ ���� ��쿡 ���� ��ó����Դϴ�.
            if (y1 * y2 > 0) return -1.0f;
            if (y1 - y2 == 0) return -1.0f; // 0���� ���� �� �����ϴ�.

            if (y1 > 0)
            {
                return y1 * y1 * (x2 - x1) / ((y1 - y2) * 2);
            }
            else if (y2 > 0)
            {
                return y2 * y2 * (x2 - x1) / ((y2 - y1) * 2);
            }
            else // y1 * y2 == 0�� ���
            {
                return MathF.Abs(x2 - x1) * MathF.Abs(y2 - y1) / 2;
            }
        }
        /// <summary>
        /// ��ٸ����� ���̸� �����ݴϴ�.
        /// </summary>
        /// <param name="x1">ù��° ���� x��ǥ�Դϴ�.</param>
        /// <param name="x2">�ι�° ���� x��ǥ�Դϴ�. x1���� �� ū ���� ������ �־�� �մϴ�.</param>
        /// <param name="y1">ù��° ���� y��ǥ�Դϴ�.</param>
        /// <param name="y2">�ι�° ���� y��ǥ�Դϴ�.</param>
        /// <returns></returns>
        private float getTrapezoid(float x1, float x2, float y1, float y2)
        {
            return (y1 + y2) * MathF.Abs(x2 - x1) / 2;
        }
        private Vector2[] mergeSimilarPoints(Vector2[] points)
        {
            // x��ǥ�� ���̿� y��ǥ�� ���̰� ��� MERGE_LIMIT_RANGE���� �۰ų� ������,
            // pivots�� ���� ��Ͽ� ����ϰ�,
            // �׷��� �ʰų�, ������ �������� �ȴٸ� pivot�� �ִ� ������ ��հ��� ���Ѵ�����,
            // result�� ����մϴ�.

            Vector2[] result = new Vector2[0] { };

            if(points.Length <= 0) return result;
            Vector2[] pivots = new Vector2[0] {};
            Vector2 addingPoint = points[0];

            // �ݺ������� ����� �����Դϴ�.
            for (int index = 1; index < points.Length - 1; ++index) // ù��° ����Ʈ�� ������ ǥ��Ʈ�� �������� �ʽ��ϴ�.
            {
                #region ���� ����
                //  ù��° �ǹ��̶��
                //      ������ �������� �ʰ� �ϴ� pivots�� ����ֱ�
                //      ���� ������ ����Ʈ���
                //          result�� ����ֱ�
                //          ���� ����
                //  �׷��� ������
                //      x,y ��ǥ ������ ���̰� �ش� ���� ���̸�
                //          result�� ����ֱ�
                //      �׷��� ������
                //          �ǹ��� ����ֱ�
                //          continue
                //      ���� ������ ����Ʈ��� result�� ����ֱ�
                //          ���� ����
                #endregion
                if (pivots.Length == 0)
                {
                    AddElementArray(ref pivots, points[index]);
                }
                else
                {
                    // ���� ���� ���̶��
                    if (points[index].x - pivots[0].x <= MERGE_LIMIT_RANGE &&
                        MathF.Abs(points[index].y - pivots[0].y) <= MERGE_LIMIT_RANGE)
                    {
                        AddElementArray(ref pivots, points[index]);
                    }
                    else
                    {
                        // ��� ����Ʈ�� ���� ��, ����Ʈ�� �߰�
                        addMergedPoint(ref result, pivots);

                        // ���� ���� ���̹Ƿ�, ���� �Ǻ��� �ļ� �߰��մϴ�.
                        pivots = new Vector2[1] { points[index] };                        
                    }
                }
            }
            // ������ ����Ʈ�Դϴ�.
            addMergedPoint(ref result, pivots);

            return result;
        }
    }

    [System.Serializable]
    public class Penetration : IMeasurable
    {
        /// <summary>
        /// ��� �������� �������� �Ծ����� ǥ���մϴ�. 0�� 1 ������ ���̿��� �մϴ�.
        /// </summary>
        public float angle;
        /// <summary>
        /// ��ŭ ���ظ� ���������� ����մϴ�. 0�� 1 ������ ���̿��� �մϴ�. Quantity�̱⵵ �մϴ�.
        /// </summary>
        public float amount;

        public float Quantity
        {
            get => amount;
            set => amount = value;
        }
    }
    [System.Serializable]
    public struct Sphere
    {
        public Vector3 position;
        public float radius;

        public Sphere(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }
    }
    [System.Serializable]
    public class Spheres : IArray<Sphere>
    {
        public Sphere[] self;

        public int Length
        {
            get => self.Length;
        }
        public Sphere this[int index]
        {
            get => self[index];
            set => self[index] = value;
        }

        public Spheres(params Sphere[] spheres)
        {
            self = spheres;
        }
        public void Add(params Sphere[] spheres)
        {
            foreach(Sphere one in spheres)
            {
                AddElementArray(ref self, one);
            }
        }
    }

    /// <summary>
    /// �� ������ UnitPart�� ���� �� �ִ� �����Դϴ�.
    /// </summary>
    [System.Serializable]
    public abstract class BaseUnit
    {
        /// <summary>
        /// �� ������ �����Դϴ�.
        /// </summary>
        public string type;

        /// <summary>
        ///     �ش� ������ �� ������ Part�鿡�� � ������ ��ĥ�� ����մϴ�.
        /// </summary>
        /// <remarks>
        ///     �� �Լ��� �������̵� �Ǿ�� �մϴ� : �ν��Ͻ��� ����� Ŭ�������� Part�� �ٸ��� ���ư� �� �����ϱ��.
        ///     �� �Լ��� �� �������� ��� ó���Ǿ�� �մϴ�.
        ///     <para>
        ///         ���� : �˾Ƽ� �ϼ���. ������ ����Ǿ����� ���γ� ���...
        ///     </para>
        ///     <para>
        ///         �� �Լ��� "�" unitPart�� ���ظ� �������� ����մϴ�.
        ///         �׷��� �� �Լ��� �� ������Ʈ�� ���� ���ظ� ������� �ʰ�, �� ���� ���� �Լ����� ȣ���մϴ�.
        ///         ��, �������� ���� �ڵ� ��Ÿ���� �����غ�����,
        ///         �� ��ü�� ���������� ���� ������ �� ��ũ��Ʈ�� ���ǵ� ������ �ǴܵǱ⿡,
        ///         ���� ���� ��Ʈ�� ������ ��ġ�� �� �Լ��� �� ��ũ��Ʈ�� �߰��߽��ϴ�
        ///     </para>
        /// </remarks>
        /// <param name="position"> ��� ������ �¾Ҵ��� �����մϴ�,.</param>
        /// <param name="direction">�¾��� �� ����� ���� ��ü�� �������� �����Դϴ�.. ��� �������� ���ȴ°��Դϴ�.</param>
        /// <param name="attackInfo">���� ������Ʈ�� ���� �����Դϴ�.</param>
        public abstract void DamagePart(Vector3 position, Vector3 direction, AttackClassHelper.AttackInfo attackInfo);
    }
}

// Unit -> Organism -> Human
// Unit -> Machine -> IeuMachineStandard