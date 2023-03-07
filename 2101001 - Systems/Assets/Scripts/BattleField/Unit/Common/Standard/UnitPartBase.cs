using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#warning 버그 있음. 데미지를 입어야 하는데 오히려 회복되는 사태. 심지어 1을 넘어섭니다.
/// <summary>
/// '모든 유닛들의 ~~~Part 컴포넌트'는 이 클래스를 상속합니다.
/// </summary>
/// <remarks>
///     <para>
///         이 클래스는 왜 존재하는가?
///     </para>
///     <para>
///         모든 유닛들은 각각의 부속 부품들을 가지고 있습니다.
///     </para>
/// </remarks>
public class UnitPartBase : BaseComponent
{

    /// <remarks>
    ///     <para>
    ///         정의 : Part는 유닛의 내부를 구성하는 단위로, 특정 기능을 하는 구조입니다.
    ///     </para>
    ///     <para>
    ///         상속 관계 : UnitPart는 모든 유닛의 ~~~Part클래스의 부모 클래스입니다. 인스턴스를 만들려면, 이 클래스를 상속해주세요.
    ///     </para>
    /// </remarks>
    [System.Serializable]
    public abstract class UnitPart
    {

        // 온전성
        // 화학 물질
        // 작동하는 것
        // 충돌 범위

        protected ChangeController changeController; // 화학 반응시 참고
#warning chemicalController의 역할은 changeController로 옮겨갈 예정입니다.
        ChemicalController chemicalController;

        // public
        // STATIC_VALUE
        public const float REACTION_RATIO_CHEMICAL = 0.5f;
        /// <summary>
        /// 화학 반응을 하는데 들어가는 에너지의 비율입니다.
        /// </summary>
        public const float REACTION_RATIO_ENERGY = 0.5f;
        public const float PIERCE_RATIO_CHEMICAL = 0.5f;
        /// <summary>
        /// 관통 데미지를 주는데 들어가는 에너지의 비율입니다.
        /// </summary>
        public const float PIERCE_RATIO_ENERGY = 0.5f;
        // non Static Value
        /// <summary>
        /// 이 UnitPart를 구성하는 화학물질입니다.
        /// </summary>
        /// <remarks>
        ///     이 unitPart를 보호하기 위해 추가된 충전재도 포함합니다. 내구도를 가지고 있기 때문에 other에 추가되지 않고, tagged와 chemicalWholeness에 추가됩니다.
        /// </remarks>
        //public ChemicalHelper.Chemicals chemicals;
        public ChemicalHelper.Chemicals tagged;
        /// <summary>
        /// 이 UnitPart가 "작동"하는데 필요로 하는 화학 물질입니다.
        /// </summary>
        /// <remarks>
        /// 요구하는 화학물질이 종종 바뀌기도 합니다
        /// </remarks>
        public ChemicalHelper.Chemicals demand;
        /// <summary>
        ///     외부에서 들어온 물질입니다.
        /// </summary>
        /// <remarks>
        ///     unitpart를 강화히기 위해 존재하는 화학물질도 포함합니다. 화학 반응을 먼저 발생시키기 위해 추가된 것도 있습니다.
        /// </remarks>
        public ChemicalHelper.Chemicals others;
#warning ChemicalWholeness는 wholeness 값이 0으로 나옵니다! 이를 어떻게 해결해야 하죠?
#warning 일반적인 wholeness가 이것을 가리키도록 만들자!
        /// <summary>
        ///     Tagged chemical에 대한 온전도를 나타냅니다.
        /// </summary>
        /// <remarks>
        ///     일반적인 wholeness는 이를 가리킵니다. unitPart를 구성하는 화학 물질이기 때문입니다.
        /// </remarks>
        public ChemicalsWholeness chemicalWholeness;
        /// <summary>
        /// 이 UnitPart의 충돌 범위를 구 모양으로 나타냅니다.
        /// </summary>
        public Spheres collisionRangeSphere;
        /// <summary>
        ///     이 부품의 충돌 범위를 직육면체 모양으로 나타냅니다.
        /// </summary>
        public Vector3[] collisionRangeCuboid;
        /// <remarks>
        ///     <para>
        ///         이 UnitPart가 얼마나 파손을 입지 않았는지를 나타냅니다.
        ///     </para>
        ///     <para>
        ///         물리적인 파손 / 변형 / 구멍이 뚫림 / 끊어짐 등이 포함됩니다. 이는 함유하고 있는 chemical은 판단하지 않고, 오직 정상적으로 활동할 수 없는 모양으로 변형됨을 의미합니다.
        ///         간단하게, 멀쩡한 옷과 찢어진 옷의 차이를 생각하시면 됩니다.
        ///     </para>
        ///     <para>
        ///         값 : 0 ~ 1
        ///     </para>
        /// </remarks>
        public virtual float Wholeness
        {
            get => chemicalWholeness.Wholeness;
        }



        public UnitPart()
        {
            Hack.Say(Hack.Scope.UnitPartBase.UnitPart.Constructor, Hack.check.method, this, 
                message: "DEBUG_UnitPart.UnitPart() : 생성자가 호출되었습니다.");

            tagged = new ChemicalHelper.Chemicals();
            demand = new ChemicalHelper.Chemicals();
            collisionRangeSphere = new Spheres();
            collisionRangeCuboid = new Vector3[0] { };

        }
        public UnitPart(UnitPart data)
        {
#warning 여기에 에러 뜰 것 같은데
            chemicalController = GameObject.Find("GameManager").GetComponent<ChemicalController>();
            changeController = GameObject.Find("GameManager").GetComponent<ChangeController>();

            this.tagged = data.tagged;
            this.demand = data.demand;
            this.collisionRangeSphere = data.collisionRangeSphere;
            this.collisionRangeCuboid = data.collisionRangeCuboid;
        }

        /// <summary>
        ///      멤버 chemicals에서 화학 반응이 일어나는지 검사하고, 있으면 그 결과를 this에 번영합니다
        ///      에너지가 발생하면 추가 피해를 입힙니다.
        /// </summary>
        public void CheckChemicalReaction()
        {
            // this.chemicals에서 화학 반응이 일어나는지 검사합니다.
            CheckChemicalReaction(new ChemicalHelper.Chemicals { }, new EnergyHelper.Energies { });

            // 이 유닛이 가지고 있는

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
        ///     외부에서 공격하는 대상이 호출하는 함수입니다.
        /// </summary>
        /// <param name="injectedMaterials"></param>
        public void CheckChemicalReaction(GameManager.chemical[] injectedChemicals)
        {
            List<GameManager.chemical> InputChemicals = new List<GameManager.chemical>(injectedChemicals);
            
            //chemicals = GameManager.Mix(InputChemicals, chemicals)
            CheckChemicalReaction();
        }
        /// <summary>
        ///     이 UnitPart가 어떻게 구체적으로 데미지를 입을지에 대해서 정의합니다.
        /// </summary>
        /// <remarks>
        ///     일단 로직은 있습니다만, 나중에 상속한 클래스가 새로운 고려사항이 있다면, 오버라이드 하시면 됩니다.
        /// </remarks>
        /// <param name="attack"></param>
        /// <param name="angle">
        ///     이 UnitPart가 어디에 공격을 받았는지에 대한 각도 값입니다.
        ///     값 범위 :
        ///         0.0f ~ 1.0f,
        ///         0.0f : 0도,
        ///         1.0f : 360도
        /// </param>
        public virtual void BeingAttacked(ref AttackClassHelper.AttackInfo attack, float angle)
        {
#warning DEBUG_CODE
            chemicalWholeness[0].Add(new Penetration() { angle = angle, amount = 1.0f });


            // 대상 캐미컬의 양 / 전체 캐미컬의 양을 가지고 있는 배열 생성
            // 비율에 따라 각 에너지를 나눠준다. 에너지의 양 * 몫
            // 포인트를 가지고 있음
            // 공격을 받으면
            // 각 에너지를 각 캐미컬의 양만큼 나눔, tagged 캐미컬만 한정함.

            // 1. 상태 확인하기. 완전 망가진거에 쏴서 관통된건지, 아니면 멀쩡한데다 맞아서 피해연산을 할지 체크합니다.
            if (IsPassing(angle))
            {
                Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this, message: "공격이 관통됩니다.");
                return;
            }

            // 2. 분배한다.
            // 관통을 위한 에너지와 반응을 위한 에너지로 쪼갠다. 캐미컬은 반응 연산을 위해 합친 값을 저장해둔다.
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

            // 3. 에너지 공격에 대해 피해를 입힌다.
            // 가장 레이어가 윗쪽에 있는 녀석부터 처리합니다.
            // 에너지는 전부 연산하여 
            //      (합 : 에너지 )
            // 가장 윗쪽 Layer에 있는 chemical부터 wholeness 데미지를 반영합니다.
            for(int indexChemical = 0; indexChemical < chemicalWholeness.Length; ++indexChemical)
            {
                if (energiesForPierce.HasEnergy == false) break;

                SingleChemicalWholeness temp = chemicalWholeness[indexChemical];
                pierceOneChemical(ref temp, ref energiesForPierce, angle); //CS0206: 속성 또는 인덱서는 out 또는 ref 매개 변수로 전달할 수 없습니다.
                chemicalWholeness[indexChemical] = temp;
            }

            // 3.1. 관통했을 때, 공격 클래스의 물질을 바깥으로 내보내도록 한다.
            // 그렇지 않다면 그냥 진행할거지만
            if(energiesForPierce.HasEnergy == true)
            {
                NamedQuantityArrayHelper.Add<EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref energiesForPierce, energiesForReaction);
                attack.energies = energiesForPierce;
                return;
            }


            // 4. 화학 반응 루프를 진행한다. 반응체크 -> 반응적용 -> 관통체크 -> 비관통시 / 마지막 루프 내 반응이 없을 때까지 반복
            // 관통시 에너지와 물질을 바깥으로 옮긴다.
            // 4-a. 관통했을때 공격 클래스의 물질을 바깥으로 내보내도록 한다, 반응하고 남은 에너지도 바깥으로 내보낸다.
            // 4-b. 관통하지 않았을 때, 유닛 파트에 공격 클래스의 물질을 담도록 한다.

            bool isThereChemicalReaction = false; // 화학 반응이 발생한다면 true로 바뀝니다.
            do
            {
                // 화학 반응 체크
                int reactionIndex = ChangeController.Reactions.GetIndex(chemicalsForReaction, energiesForReaction);
                if (reactionIndex == -1) break;
                // 반응 비율 획득
                float reactionRatioChemical =
                    NamedQuantityArrayHelper.Divide
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (chemicalsForReaction, ChangeController.Reactions[reactionIndex].reactants);
                float reactionRatioEnergy =
                    NamedQuantityArrayHelper.Divide
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (energiesForReaction, ChangeController.Reactions[reactionIndex].ActivationEnergy);
                float reactionRatio = MathF.Min(reactionRatioChemical, reactionRatioEnergy);

                // 반응 적용
                // 캐미컬 빼기
                // 계산용 값 제거
                ChemicalHelper.Chemicals reactants = ChangeController.Reactions[reactionIndex].reactants;
                NamedQuantityArrayHelper.Multiply
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, reactionRatio);
                NamedQuantityArrayHelper.Subtract
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref chemicalsForReaction, reactants);

                // 실제 값 제거 attackClass -> others -> tagged
                NamedQuantityArrayHelper.RemoveIntersection
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, ref attack.chemicals);
                NamedQuantityArrayHelper.RemoveIntersection
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, ref others);
                NamedQuantityArrayHelper.RemoveIntersection
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref reactants, ref tagged);

                // 캐미컬 더하기
                // 계산용 값 추가
                ChemicalHelper.Chemicals products = ChangeController.Reactions[reactionIndex].products;
                NamedQuantityArrayHelper.Multiply
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref products, reactionRatio);
                NamedQuantityArrayHelper.Add
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref chemicalsForReaction, products);

                // 실제 값 추가
                NamedQuantityArrayHelper.Add
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref others, products);

                // 에너지 빼기
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

                // 에너지 더하기
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


                // 발생한 에너지를 관통시켜본다.
                for(int index = 0; index < chemicalWholeness.Length; ++index)
                {
                    SingleChemicalWholeness temp = chemicalWholeness[index];
                    pierceOneChemical(ref temp, ref energiesForPierce, angle);
                    chemicalWholeness[index] = temp;
                }
                
                // 캐미컬이 관통되었는지 체크한다.
                if(MeasurableArrayHelper.IsAnyElementPositive
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (energiesForPierce))
                {
                    break;
                }

                // 반복 조건
                // 1. 화학 반응이 있고
                // And
                // 2. 캐미컬을 관통되지 않았읗 때
            } while (isThereChemicalReaction && true);

            // 화학 반응이 끝났으면 남아있는 반응 에너지에게 관통하도록 한다.
            for(int index = 0; index < chemicalWholeness.Length; ++index)
            {
                SingleChemicalWholeness temp = chemicalWholeness[index];
                pierceOneChemical(ref temp, ref energiesForReaction, angle);
                chemicalWholeness[index] = temp;
            }

            // 관통 / 비 관통에 따라 달라집니다.
            if (chemicalWholeness.IsBroken(angle))
            {
                // 관통시
                // attack.energies = 관통에너지 + 반응에너지
                // 물질 => 각자가 각자 알아서 냅두기 / 공격 물질은 그냥 구멍난 곳으로 나가도록 한다.

                NamedQuantityArrayHelper.Add
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref attack.energies, energiesForPierce);
                NamedQuantityArrayHelper.Add
                    <EnergyHelper.Energies, EnergyHelper.Energy>
                    (ref attack.energies, energiesForPierce);
            }
            else
            {
                // 비 관통시
                // 에너지 => 에너지는 다 썼으니깐..
                // 물질 => 공격에 쓰인 물질에 대한것은 파트에 담는다.

                NamedQuantityArrayHelper.Add
                    <ChemicalHelper.Chemicals, ChemicalHelper.Chemical>
                    (ref others, attack.chemicals);
            }
        }
        /// <summary>
        ///     이 유닛파트가 이 각도에서 완전히 망가졌는지 체크하여 공격이 UnitPart에 영향을 끼치는지 체크합니다.
        /// </summary>
        /// <remarks>
        ///     모든 캐미컬의 AngleWholeness와 taggedChemical의 유무를 따집니다
        ///     <para>
        ///         BeingAttacked 내부에서 호출하지 않습니다.
        ///         왜냐하면 BeingAttacked가 오버로딩할 때 마다 그 함수를 매번 호출해야 하니깐 차라리 구현이 고정된
        ///         호출자가 호출하도록 합니다.
        ///     </para>
        /// </remarks>
        /// <param name="angle"> 공격하려는 각도입니다.</param>
        /// <returns></returns>
        public virtual bool IsPassing(float angle)
        {
            bool isTaggedExist = false;
            // 모든 chemical이
            for(int index = 0; index < tagged.Length; ++index)
            {
                if (tagged[index].quantity > 0)
                {
                    isTaggedExist = true;
                    break;
                }
            }
            if (isTaggedExist == false) return false;

            // 모든 chemicalWholeness를 체크합니다.
            Hack.Say(Hack.Scope.UnitPartBase.UnitPart.IsPassing, Hack.check.info, this,
                message: $"chemicalWholeness 값이 Null 여부 {chemicalWholeness == null}");
            for(int index = 0; index < chemicalWholeness.Length; ++index)
            {
                if (chemicalWholeness[index].GetAngleWholeness(angle) <= 0)
                {
                    return true;
                }
            }

            return false;
        }


#warning 작업중
        /// <summary>
        ///     wholeness를 가지고 있는 캐미컬에 대해 에너지를 관통시키고 이를 적용합니다.
        ///     초반 에너지 공격을 반영할때도 이 함수를 사용합니다
        /// </summary>
        /// <param name="targetChemical">피해를 입을 캐미컬의 온전도입니다.</param>
        /// <param name="energies">여러 에너지입니다. </param>
        /// <param name="angle">피해를 입힐 각도입니다.</param>
        /// <returns>
        ///     관통한다면 true를 리턴합니다.
        /// </returns>
        private void pierceOneChemical(ref SingleChemicalWholeness targetChemical, ref EnergyHelper.Energies energies, float angle)
        {
            Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this); // 이 함수는 호출됩니다.
            // 한 대상에 대한 피해량을 계산합니다.
            // 각온전성(angle) / 피해합 = k을 구합니다.
            // k가 1.0 이하이면 각온전성을 0으로 만들도록 하고 energies중 하나의 값 (원래 값) = (원래 값) * (1 - k) 값
            // k가 1.0 초과면 비율 1로 설정하여 각온전성을 깎습니다

            // 피해량 계산
            float expectedDamages = 0.0f; // 예상되는 관통력입니다,
            for(int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
            {
                // 너무 길어서 끊었어요.
                ChangeHelper.EnergyResist energyResist = ChangeController.EnergyResists[targetChemical.Name][energies[indexEnergy].Name];

                // 임계 피해량보다 센지 체크합니다.
                if (energyResist.resistanceDefense < energies[indexEnergy].Quantity)
                {
                    expectedDamages +=
                        (energies[indexEnergy].Quantity - energyResist.resistanceDefense) /
                        (energyResist.resistanceRatio * tagged[targetChemical.Name].Quantity);
                    Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this, message: $"유효 피해를 입었습니다!\n 피해량 : {(energies[indexEnergy].Quantity - energyResist.resistanceDefense) / (energyResist.resistanceRatio * tagged[targetChemical.Name].Quantity)}");
                }
            }

            // 각온전성 / 피해합 의 값을 구합니다.
            if(expectedDamages == 0.0f) // 0으로 값을 나눌 수 없습니다.
            {
                for(int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
                {
                    energies[indexEnergy].Quantity = 0.0f;
                }
                return;
            }
            float angleWholeness = targetChemical.GetAngleWholeness(angle);
            float energyRatio = angleWholeness / expectedDamages;

            // 분기를 가집니다.
            if (energyRatio <= 1.0) // 에너지가 너무 강력해서 관통될 것 같습니다.
            {
                Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this, message: $"공격이 관통됩니다.");
                // 각온전성 0으로 만들기
                targetChemical.Add(
                    new Penetration() { amount = angleWholeness, angle = angle }); // amount는 expectedDamages * energyRatio 인데 angleWholeness 값과 동일

                // energies 값 깎기
                for (int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
                    energies[indexEnergy].amount *= 1 - energyRatio;
            }
            else // 공격 에너지를 전부 흡수 할 수 있습니다.
            {
                Hack.Say(Hack.isDebugUnitPartBase, Hack.check.method, this, message: $"공격이 흡수됩니다.");
                targetChemical.Add(
                    new Penetration() { amount = expectedDamages, angle = angle });

                // energies 값 깎기
                for (int indexEnergy = 0; indexEnergy < energies.Length; ++indexEnergy)
                {
                    energies[indexEnergy].amount = 0.0f;
                }
            }
        }
    }

    /// <summary>
    /// 어떤 대상의 내부에 존재하는 캐미컬 다수가 얼마나 온전한지를 나타냅니다.
    /// </summary>
    /// <remarks>
    ///     반드시 tagged Chemical과 1대1로 대응하도록 넣어야 합니다!
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
#warning 여기서 wholeness 업데이트 해보자
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
        ///     수정하지 마세요. 직렬화를 지원하지 않아 private를 public으로 바꿨을 뿐입니다.
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
    /// 어떤 대상의 내부에 존재하는 물질 "한개"가 얼마나 온전한지를 나타냅니다.
    /// </summary>
    /// <remarks>
    ///     주로 어떤 각도에서 얼만큼 피해를 입었는지를 표기할 수 있습니다.
    /// </remarks>
    [System.Serializable]
    public class SingleChemicalWholeness : INameKey
    {
        /// <summary>
        /// wholeness의 값에서 관통된 경우의 기준값입니다.
        /// </summary>
        /// <remarks>
        /// 이 각도에서 공격이 들어오면 구멍으로 지나가버립니다.
        /// </remarks>
        public static float PIERCE_VALUE
        {
            get => 0.0f;
        }
        /// <summary>
        /// 관통이 주변에 미치는 영향의 범위입니다.
        /// </summary>
        /// <remarks>
        /// 한 부분에 데미지가 들어오면 그래프 모양이 V자로 깎이게 됩니다,
        /// 이 값의 2배가 움푹 패이기 시작하는 곳과 패인게 나오는 곳의 거리입니다.
        /// 0.5 이상이면 아마 논리적으로 오류를 일으킬 것입니다.
        /// </remarks>
        public static float EFFECT_RANGE
        {
            get => 0.1f;
        }
        /// <summary>
        ///     그래프에 있는 점들이 최적화 작업중 합병해야 하는 기준입니다. 너무 작아서 무시할 정도의 기준입니다.
        /// </summary>
        /// <remarks>
        ///     최적화를 위해 존재하는 상수입니다.
        /// </remarks>
        public static float MERGE_LIMIT_RANGE
        {
            get => 0.0078125f;
        }
        /// <summary>
        /// 전체적인 온전도를 리턴합니다.
        /// </summary>
        /// <remarks>
        /// 참조하는 변수는 값을 요구할때가 아니라, 변화가 일어날 때마다 연산을 진행하여 반영합니다.
        /// </remarks>
        public float Wholeness
        {
            get => lazyWholeness;
        }
        public int layer; // 에너지 공격을 받을 때, 적용 우선순위입니다. 장갑 등를 위해 존재합니다.
        public string name;
        /// <summary>
        /// 캐미컬의 이름입니다.
        /// </summary>
        public string Name { get => name; set => name = value; }
        public Penetration[] damages; // chemical에 입힌 피해입니다.

        private float lazyWholeness = 0.0f;

        // 만약 요구하는 물질보다 적으면? (현재 물질 / 요구 물질) * 이 각도의 wholeness

        /// <summary>
        /// 공격받을 때, 이 함수를 호출합니다. Damage를 기록하려고 시도하고, 해당공격이 관통되었을때, 그 값을 리턴합니다.
        /// </summary>
        /// <remarks>
        ///     만약 공격시 관통이 되어 add를 하지 않아야 할 수가 있스비다. 이 경우에는 외부 함수에서 처리해야 합니다.
        ///     1. 관통이 된 경우라면, 다른 캐미컬에서 처리해야 하는가?
        /// </remarks>
        /// <param name="penetration">
        /// ((들어온 에너지의 양 - 물질의 방어저항값) / (물질의 비율저항값 * 물질의 양 * wholeness(angle))
        /// </param>
        /// <returns>
        /// 막아내지 못하고 뚫려버린 비율값을 리턴해줍니다. 이 값을 물질의 비율저항값 * 물질의 양 * wholeness과 곱하여 전달합니다.
        /// </returns>
        public float Add(Penetration penetration)
        {
            Hack.Say(Hack.Scope.UnitPartBase.SingleChemicalWholeness.Add, Hack.check.method, this);
            // 리턴할 값입니다.
            float result = 0.0f;

            // damages에 reflect할 penetration입니다.
            Penetration reflectedPenetration = new Penetration()
            {
                angle = penetration.angle
            };
            // delta가 음수이면 관통되었음을 의미합니다.
            float angleWholeness = GetAngleWholeness(penetration.angle);
            float remainingWholeness = angleWholeness - penetration.amount;
            
            if (angleWholeness <= PIERCE_VALUE)
            {
                // 구멍에다 날아온 에너지이므로 그대로 관통해줍니다.
                result = penetration.amount;
                return result;
            }
            else if (remainingWholeness <= PIERCE_VALUE)
            {
                // 데미지를 입고, 관통되었습니다.
                result = MathF.Abs(remainingWholeness);
                reflectedPenetration.amount = angleWholeness;

                AddElementArray(ref damages, reflectedPenetration);
                sort();
                return result;
            }
            else
            {
                // 데미지를 입고, 관통하지 못했습니다.
                result = 0.0f;

                AddElementArray(ref damages, penetration);
                sort();
                return result;
            }
        }
        /// <summary>
        /// 해당하는 각도의 온전성을 리턴해줍니다.
        /// </summary>
        /// <param name="angle">각도입니다 "원래 각도 / 360" 한 값을 입력해야 하며, 0과 1 사이의 값이여야 합니다.</param>
        /// <returns> 해당하는 각도의 온전성입니다. </returns>
        public float GetAngleWholeness(float angle)
        {
            Hack.Say(Hack.Scope.UnitPartBase.SingleChemicalWholeness.GetAngleWholeness, Hack.check.method, this);
            // 근처일수록 값을 깎는다.
            float result = 1.0f;

            for(int value = 1; value >= -1; value--) // value 값은 각도가 0과 1에 비슷한 위치에 걸려 있어 건너편 각도로 각도의 영향력을 제공하는 경우입니다.
            {
                for (int index = 0; index < damages.Length; index++) // 
                {
                    // 수학식은 이미지 참고하세요.
                    Hack.Say(Hack.Scope.UnitPartBase.SingleChemicalWholeness.GetAngleWholeness, Hack.check.info, this,
                        message: $"damages[index].amount = {damages[index].amount},\tvalue : {value},\tangle : {angle}");

                    result -= MathF.Max(0, damages[index].amount - (damages[index].amount / EFFECT_RANGE) * MathF.Abs(angle + value - damages[index].angle));

                }
            }

            if (result < 0) return 0.0f;
            else return result;
        }
#warning 최대한 최적화를 진행해야 한다.
        /// <summary>
        ///     그래프를 적분하여 값을 얻습니다.
        /// </summary>
        /// <remarks>
        /// 피해를 입으면 좌, 중앙, 우 파트마다 꼭지점이 생기는데 이를 기준으로 연산을 합니다.
        /// 상당히 많은 연산을 할 것으로 추정됩니다. lazyWholeness가 업데이트 됩니다.
        /// </remarks>
        /// <returns></returns>
        public float IntegrateWholeness()
        {
            // 360각도를 넘어가는 녀석이 있는지 체크합니다.
            // 어떤 녀석.angle - EFFECT_RANGE < 0이거나
            // 어떤 녀석.angle + EFFECT_RANGE > 1인 것이 있으면 그것을 pick합니다.

            // 1단계에서 걸리는 녀석이 있으면 녀석의 분신을 하나 만들어줍니다. y값은 원래 원형 각도니까 360 == 0이므로 복제하여 구현합니다.

            // 각 포인트를 구한다.
            // 너무 좁은 포인트(x축과 y축 모두 매우 작은 차이여야 함)는 merge할까?

            // 넓이를 구한다

            // 각도 범위를 넘어가는 점이 있는지 확인합니다.
            Penetration[] realDamages = new Penetration[0]{ };
            // 피해 저장 배열 realDamages에 damages값을 붙여넣기 합니다.
            foreach (Penetration one in damages) AddElementArray(ref realDamages, one);

#warning DEBUG_CODE
            for(int index = 0; index < damages.Length; index++) Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this, message: $"position : x = {damages[index].angle}, y = {damages[index].Quantity}");

            for(int index = 0; index < damages.Length; index++)
            {
#warning bugable : damages[index].angle - EFFECT_RANGE의 값이 -1보다 더 작은 경우도 있습니다!
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

            // 각 포인트를 구한다.
            float[] pointKey = new float[1] { 0.0f }; // 각도의 wholeness를 계산할 각도값입니다. 그래프가 꺾이는 구간입니다.
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
                    // 0 <= x < 360이면 배열에 각도 값을 추가한다.
                    if (angleValue[angleIndex] >= 0.0f &&
                        angleValue[angleIndex] < 1.0f)
                    {
#warning IndexOutOfRangeException
                        // pointKey가 범위를 벗어날까?
                        // 아니면 angleValue의 범위를 벗어나는 index가 문제일까?
                        // angleValue는 뭘까?
                        AddElementArray(ref pointKey, angleValue[angleIndex]);
                    }
                }
            }
            AddElementArray(ref pointKey, 1.0f);
            // 정렬
            Array.Sort(pointKey);

            // 머지를 진행할까?
            // pointKey를 변경하세요.

            // 각자의 크기를 구한다
            float result = 0.0f;

            for(int index = 0; index < pointKey.Length - 1; ++index)
            {
                float x1 = pointKey[index];
                float y1 = GetAngleWholeness(pointKey[index]);
                float x2 = pointKey[index + 1];
                float y2 = GetAngleWholeness(pointKey[index + 1]);

                // 두 점이 y좌표가 양수일 때
                if (y1 > 0.0f && y2 > 0.0f)
                {
                    float addValue = getTrapezoid(x1, x2, y1, y2);
                    Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this,
                        message: $"wholeness 값 : [{index} of {pointKey.Length - 1}] {result},\t추가 값 : {addValue}\n가로 : {x2-x1},\t세로 {y1}, {y2}\t{x1}, {x2}\t{y1}, {y2}");
                    result += addValue;
                    continue;
                }
                // 두 점중 하나의 y좌표가 양수이고 하나는 음수일 때
                else if (y1 > 0.0f || y2 > 0.0f)
                {
                    float addValue = getTriangle(x1, x2, y1, y2);
                    Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this,
                        message: $"wholeness 값 : [{index} of {pointKey.Length - 1}] {result},\t추가 값 : {addValue}\n가로 : {x2 - x1},\t 세로 {MathF.Abs(y2 - y1)}\t{x1}, {x2}\t{y1}, {y2}");
                    result += addValue;
                    continue;
                }
                // 두 점이 양수가 아니므로 계산하지 않습니다.
            }
            Hack.Say(Hack.isDebugUnitPartBase, Hack.check.info, this,
                message: $"wholeness 값 : {result}");

            lazyWholeness = result;
            return result;
        }
        /// <summary>
        /// 수리 / 부품 교체 등으로, Damage의 피해 값을 매개변수 값만큼 완화해줍니다.
        /// </summary>
        /// <remarks>
        /// 가장 심각해 보이는 부분부터 치료합니다, 만약 치료중 피해 수준이 비슷한 상처가 여러개가 된 경우, 그 상처를 나눠서 치료합니다.
        /// </remarks>
        /// <param name="amount">
        /// 0과 1 사이 값을 입력해야 합니다.
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
        /// target의 점들의 평균적인 점 위치를 구하고, 그것을 recvArray에 추가합니다.
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
        ///     해당 각도에 대한 wholeness를 얻습니다.
        /// </summary>
        /// <remarks>
        /// GetAngleWholeness와의 차이점 :
        ///     이 함수는 해당 각도 (0 ~ 360)를 넘어간 경우는 생각하지 않습니다. 따라서 이 함수를 호출 할 때,
        ///     매개변수로 넘길 대상은 해당 처리를 완료해야 합니다.
        /// </remarks>
        /// <param name="targetDamages">연산을 진행할 피해 기록입니다.</param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private float getRawAngleWholeness(Penetration[] targetDamages, float angle)
        {
            float result = 1.0f;

            for (int index = 0; index < targetDamages.Length; index++) // 
            {
                // 수학식은 이미지 참고하세요.
                result -= MathF.Max(0, targetDamages[index].amount - (targetDamages[index].amount / EFFECT_RANGE) * MathF.Abs(angle - targetDamages[index].angle));
            }

            return result;
        }
        /// <summary>
        /// y1 * y2 < 0일 때, 두점(x1, y1), (x2, y2)사이를 잇는 선분, y = 0, x = (y1이 양수이면 x1 혹은 y2가 양수이면 x2)가 만드는 직각삼각형의 넓이를 구해줍니다.
        /// </summary>
        /// <param name="x1">첫번째 점의 x좌표입니다.</param>
        /// <param name="x2">두번째 점의 x좌표입니다. x1보다 더 큰 값을 가지고 있어야 합니다.</param>
        /// <param name="y1">첫번째 점의 y좌표입니다.</param>
        /// <param name="y2">두번째 점의 y좌표입니다.</param>
        /// <returns>
        /// 삼각형중 양의 면적을 구합니다. 만약 삼각형이 아니거나, y1 == y2이면 -1.0f를 리턴합니다.
        /// </returns>
        private float getTriangle(float x1, float x2, float y1, float y2)
        {
            // 영 좋지 못한 매개 변수가 들어온 경우에 대한 대처방법입니다.
            if (y1 * y2 > 0) return -1.0f;
            if (y1 - y2 == 0) return -1.0f; // 0으로 나눌 수 없습니다.

            if (y1 > 0)
            {
                return y1 * y1 * (x2 - x1) / ((y1 - y2) * 2);
            }
            else if (y2 > 0)
            {
                return y2 * y2 * (x2 - x1) / ((y2 - y1) * 2);
            }
            else // y1 * y2 == 0인 경우
            {
                return MathF.Abs(x2 - x1) * MathF.Abs(y2 - y1) / 2;
            }
        }
        /// <summary>
        /// 사다리꼴의 넓이를 구해줍니다.
        /// </summary>
        /// <param name="x1">첫번째 점의 x좌표입니다.</param>
        /// <param name="x2">두번째 점의 x좌표입니다. x1보다 더 큰 값을 가지고 있어야 합니다.</param>
        /// <param name="y1">첫번째 점의 y좌표입니다.</param>
        /// <param name="y2">두번째 점의 y좌표입니다.</param>
        /// <returns></returns>
        private float getTrapezoid(float x1, float x2, float y1, float y2)
        {
            return (y1 + y2) * MathF.Abs(x2 - x1) / 2;
        }
        private Vector2[] mergeSimilarPoints(Vector2[] points)
        {
            // x좌표의 차이와 y좌표의 차이가 모두 MERGE_LIMIT_RANGE보다 작거나 같으면,
            // pivots의 점의 목록에 등록하고,
            // 그렇지 않거나, 마지막 루프문이 된다면 pivot에 있던 점들의 평균값을 구한다음에,
            // result에 등록합니다.

            Vector2[] result = new Vector2[0] { };

            if(points.Length <= 0) return result;
            Vector2[] pivots = new Vector2[0] {};
            Vector2 addingPoint = points[0];

            // 반복적으로 사용할 로직입니다.
            for (int index = 1; index < points.Length - 1; ++index) // 첫번째 포인트와 마지막 표인트는 머지하지 않습니다.
            {
                #region 로직 정리
                //  첫번째 피벗이라면
                //      묻지도 따지지도 않고 일단 pivots에 집어넣기
                //      만약 마지막 포인트라면
                //          result에 집어넣기
                //          루프 종료
                //  그렇지 않으면
                //      x,y 좌표 각각의 차이가 해당 범위 밖이면
                //          result에 집어넣기
                //      그렇지 않으면
                //          피벗에 집어넣기
                //          continue
                //      만약 마지막 포인트라면 result에 집어넣기
                //          루프 종료
                #endregion
                if (pivots.Length == 0)
                {
                    AddElementArray(ref pivots, points[index]);
                }
                else
                {
                    // 머지 범위 안이라면
                    if (points[index].x - pivots[0].x <= MERGE_LIMIT_RANGE &&
                        MathF.Abs(points[index].y - pivots[0].y) <= MERGE_LIMIT_RANGE)
                    {
                        AddElementArray(ref pivots, points[index]);
                    }
                    else
                    {
                        // 평균 포인트를 만든 뒤, 리절트에 추가
                        addMergedPoint(ref result, pivots);

                        // 머지 범위 밖이므로, 새로 피봇을 파서 추가합니다.
                        pivots = new Vector2[1] { points[index] };                        
                    }
                }
            }
            // 마지막 포인트입니다.
            addMergedPoint(ref result, pivots);

            return result;
        }
    }

    [System.Serializable]
    public class Penetration : IMeasurable
    {
        /// <summary>
        /// 어느 각도에서 데미지를 입었는지 표현합니다. 0과 1 사이의 값이여야 합니다.
        /// </summary>
        public float angle;
        /// <summary>
        /// 얼만큼 피해를 입혔는지를 기록합니다. 0과 1 사이의 값이여야 합니다. Quantity이기도 합니다.
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
    /// 이 유닛의 UnitPart를 담을 수 있는 정보입니다.
    /// </summary>
    [System.Serializable]
    public abstract class BaseUnit
    {
        /// <summary>
        /// 이 유닛의 종류입니다.
        /// </summary>
        public string type;

        /// <summary>
        ///     해당 공격이 이 유닛의 Part들에게 어떤 영향을 미칠지 계산합니다.
        /// </summary>
        /// <remarks>
        ///     이 함수는 오버라이딩 되어야 합니다 : 인스턴스를 만드는 클래스마다 Part가 다르게 돌아갈 수 있으니까요.
        ///     이 함수는 한 순간만에 즉시 처리되어야 합니다.
        ///     <para>
        ///         구현 : 알아서 하세요. 적당히 관통되었는지 여부나 등등...
        ///     </para>
        ///     <para>
        ///         이 함수는 "어떤" unitPart가 피해를 입을지를 계산합니다.
        ///         그래서 이 함수는 한 유닛파트가 입을 피해를 계산하지 않고, 그 일을 해줄 함수들을 호출합니다.
        ///         또, 앞으로의 유닛 코딩 스타일을 생각해봤을때,
        ///         한 개체의 여러가지의 유닛 기관계는 한 스크립트에 정의될 것으로 판단되기에,
        ///         여러 유닛 파트에 영향을 끼치는 이 함수는 이 스크립트에 추가했습니다
        ///     </para>
        /// </remarks>
        /// <param name="position"> 어느 부위로 맞았는지 정의합니다,.</param>
        /// <param name="direction">맞았을 때 당시의 공격 물체가 움직였던 방향입니다.. 어느 방향으로 때렸는가입니다.</param>
        /// <param name="attackInfo">공격 오브젝트의 공격 정보입니다.</param>
        public abstract void DamagePart(Vector3 position, Vector3 direction, AttackClassHelper.AttackInfo attackInfo);
    }
}

// Unit -> Organism -> Human
// Unit -> Machine -> IeuMachineStandard