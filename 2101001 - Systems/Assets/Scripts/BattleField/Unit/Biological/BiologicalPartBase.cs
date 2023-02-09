using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     생명체 유닛의 기관계를 정의힙니다.
/// </summary>
public class BiologicalPartBase : UnitPartBase
{


    /// <summary>
    ///     모든 생명체 유닛의 기관을 정의한 클래스입니다.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         정의 : Organ은 생명 유닛의 내부를 구성하는 단위로, 특정 기능을 하는 구조입니다.
    ///     </para>
    ///     <para>
    ///         상속 관계 : UnitPart는 모든 유닛의 ~~~Part클래스의 부모 클래스입니다. 인스턴스를 만들려면, 이 클래스를 상속해주세요.
    ///     </para>
    /// </remarks>
    [System.Serializable]
    public class OrganPart : UnitPart
    {
        public override float wholeness
        {
            get => hp / maxHP;
            set => hp = value * maxHP;
        }
        /// <summary>
        ///     maxHP값을 먼저 제공해야 합니다.
        /// </summary>
        public float HP
        {
            get => hp;
            set => hp = Mathf.Clamp(value, 0, maxHP);
        }

        /// <summary>
        /// 이 기관의 이름입니다.
        /// </summary>
        public string Name;

        public float maxHP;
        public float RecoveryRate;
        

        /// <summary>
        /// 
        /// </summary>
        float hp;

        public OrganPart() : base()
        {
            Name = "No Name";
            maxHP = 100;
            HP = 100;
            RecoveryRate = 0.1f;
        }
        public OrganPart(OrganPart data) : base(data)
        {
            Name = data.Name;
            maxHP = data.maxHP;
            HP = data.HP;
            RecoveryRate = data.RecoveryRate;
        }

        public void Update()
        {
            // 시간이 흐름에 따라 변하게 된 값입니다.
            HP += HP * RecoveryRate;
        }
        
    }

    /// <summary>
    /// 생체 유닛의 기반 클래스입니다.
    /// </summary>
    [System.Serializable]
    public class BioUnit : BaseUnit
    {
        /// <summary>
        /// 이 생체 유닛의 종족 이름입니다.
        /// </summary>
        public string species;
        /// <summary>
        /// 이 유닛의 기관계 목록입니다.
        /// </summary>
        public OrganPart[] organParts;

        public BioUnit()
        {
            type = Species.DEFAULT;
        }

        // 0.2 버전입니다.
        public override void DamagePart(Vector3 position, Vector3 direction, AttackClassHelper.AttackInfo attackInfo) // XML 주석이 존재합니다.
        {
            // S는 organParts[organIndex]의 위치 값입니다.
            // A는 Position
            // D는 Direction


            //함수의 진행 순서
            // 어떤 기관계에 맞았는지 체크하고 결과를 boolean 배열로 나타낸다.
            // bool 배열중에서 true인 기관계를 상대로 어느 각도에 맞앗는지 


            // 어느 기관계에 해당 공격이 맞았는지 체크한다
            //bool[] isOrganCollided = Enumerable.Repeat<bool>(false, organParts.Length).ToArray(); // 편하긴 한데 함수 호출이 참 무겁겠는데?

            bool[] isOrganCollided = new bool[organParts.Length];
            float[] priority = new float[organParts.Length]; // 어느 기관계가 먼저 맞았는지에 대해 알아보기 위해, 출발지점부터 날아간 이동거리를 저장하는 변수입니다.
            float[] angles = new float[organParts.Length];

            for (int index = 0; index < organParts.Length; ++index)
            {
                isOrganCollided[index] = false;
                priority[index] = -1.0f;
                angles[index] = 0.0f;
            }

            for (int organIndex = 0; organIndex < organParts.Length; organIndex++)
            {
                // 모든 충돌 부위를 체크합니다.
                Spheres CollisionSpheres = organParts[organIndex].collisionRangeSphere;

                bool isHit = false; // 해당 공격이 기관계를 맞았는지 저장하는 변수입니다.
                float minAT = float.MaxValue; // 기관계에 맞았을때, 공격출발지점과 맞은 지점간의 가장 짧은 거리를 저장하는 변수입니다.
                float angleMinAT = 0.0f; // 기관계에 맞았을때, 가장 짧은 거리일 경우에 저장하는 각도의 값입니다.

                for (int collisionIndex = 0; collisionIndex < CollisionSpheres.Length; collisionIndex++)
                {
                    #region 구질구질한 설명
                    // 각 모서리의 길이가 1인 정육면체
                    // 그리고 그 정육면제에 있는 무작위의 구, (OrganSphere라고 하자.)
                    // 그리고 정육면체의 겉면에서 출발하여(attack Position), 정육면체의 안쪽으로 향하며(attack Direction), 무작위의 위치의 무작위의 방향이지만 OrganSphere를 반드시 지나치는 반직선 AttackLine이 있다.
                    // Organ Sphere의 반지름 값은 R이라고 하자.
                    // Attack Position의 점은 A, Attack Direcion의 방향벡터는 D, OrganSphere의 중심점은 S라고 하자
                    // 이때 AttackLine와 OrganSphere가 만나는 두 점 중, attack Position에 가장 가까운 점은 T라고 하자. T : (Tx, Ty, Tz)
                    // 이때 OrganSphere의 중심점에서 AttackLine으로 내리는 수선의 발을 H라고 하자.
                    #endregion
                    // AS벡터 = 부딛힌 부분 -> 기관의 센터
                    Vector3 attack2organVector = CollisionSpheres[collisionIndex].position - position;
                    // AS 벡터 내적 D벡터
                    float attack2organ_dot_direction = Vector3.Dot(attack2organVector, direction);
                    // R의 제곱- AS 벡터의 크기의 제곱 + 벡터AS 내적 벡터D 값 제곱 = R의 제곱 - 선분 AH의 제곱 = TH의 제곱
                    float ThSquare
                        = Mathf.Pow(CollisionSpheres[collisionIndex].radius, 2)
                        - Mathf.Pow(attack2organVector.x, 2)
                        - Mathf.Pow(attack2organVector.y, 2)
                        - Mathf.Pow(attack2organVector.z, 2)
                        + Mathf.Pow(attack2organ_dot_direction, 2);

                    if (ThSquare > 0) // 기관계에 해당 공격이 부딛혔습니다.
                    {
                        // 방향을 구합니다.
                        float lineAT = attack2organ_dot_direction - Mathf.Sqrt(ThSquare);
                        Vector3 resultDirection = position + direction * lineAT - CollisionSpheres[collisionIndex].position;
                        resultDirection.y = 0;

                        isHit = true;
                        if (lineAT < minAT)
                        {
                            angleMinAT = Mathf.Atan2(resultDirection.y, resultDirection.x);
                        }
                        isOrganCollided[organIndex] = true;
                    }
                }

                isOrganCollided[organIndex] = true;
                if (isHit)
                {
                    priority[organIndex] = minAT;
                    angles[organIndex] = angleMinAT;
                }
            }

            // 요구 사항 : 어떤 기관계가 먼저 데미지를 입는지
            // priority의 값이 가장 적은 녀석이 우선권을 가집니다.
            // isOrganCollided 값이 없는 경우 배열의 원소에 들어가지 않습니다.
            // 배열의 원소는 organParts의 인덱스 번호입니다.
            // 우선권을 가진 인덱스 번호는 배열의 앞에 옵니다.
            // forEach를 이용해서 인덱스 번호를 가지고 접근하여 각 함수를 호출할 것입니다.
            
            List<int> collidedOrganIndexes = new List<int>();
            for (int index = 0; index < organParts.Length; index++)
            {
                if (isOrganCollided[index]) collidedOrganIndexes.Add(index);
            }
            collidedOrganIndexes.Sort(delegate (int first, int second)
            {
                if (priority[first] < priority[second]) return 1;
                else if (priority[first] > priority[second]) return -1;
                else return 0;
            });

            for(int index = 0; index < collidedOrganIndexes.Count; index++)
            {
                organParts[collidedOrganIndexes[index]].BeingAttacked(ref attackInfo, angles[collidedOrganIndexes[index]]); 
            }
        }
    }

    /// <summary>
    ///     종의 이름을 명시한 구간입니다. </summary>
    /// <remarks>
    ///     만약 새로운 종을 만들 일이 있다면, 여기에 새로운 종족 이름을 추가하시면 됩니다.</remarks>
    public static class Species
    {
        public const string DEFAULT = "creature";
        public const string HUMAN = "human";
    }

    
}
