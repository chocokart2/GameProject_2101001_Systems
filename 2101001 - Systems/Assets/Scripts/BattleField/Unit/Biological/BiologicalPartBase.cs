using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     ����ü ������ ����踦 �������ϴ�.
/// </summary>
public class BiologicalPartBase : UnitPartBase
{


    /// <summary>
    ///     ��� ����ü ������ ����� ������ Ŭ�����Դϴ�.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         ���� : Organ�� ���� ������ ���θ� �����ϴ� ������, Ư�� ����� �ϴ� �����Դϴ�.
    ///     </para>
    ///     <para>
    ///         ��� ���� : UnitPart�� ��� ������ ~~~PartŬ������ �θ� Ŭ�����Դϴ�. �ν��Ͻ��� �������, �� Ŭ������ ������ּ���.
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
        ///     maxHP���� ���� �����ؾ� �մϴ�.
        /// </summary>
        public float HP
        {
            get => hp;
            set => hp = Mathf.Clamp(value, 0, maxHP);
        }

        /// <summary>
        /// �� ����� �̸��Դϴ�.
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
            // �ð��� �帧�� ���� ���ϰ� �� ���Դϴ�.
            HP += HP * RecoveryRate;
        }
        
    }

    /// <summary>
    /// ��ü ������ ��� Ŭ�����Դϴ�.
    /// </summary>
    [System.Serializable]
    public class BioUnit : BaseUnit
    {
        /// <summary>
        /// �� ��ü ������ ���� �̸��Դϴ�.
        /// </summary>
        public string species;
        /// <summary>
        /// �� ������ ����� ����Դϴ�.
        /// </summary>
        public OrganPart[] organParts;

        public BioUnit()
        {
            type = Species.DEFAULT;
        }

        // 0.2 �����Դϴ�.
        public override void DamagePart(Vector3 position, Vector3 direction, AttackClassHelper.AttackInfo attackInfo) // XML �ּ��� �����մϴ�.
        {
            // S�� organParts[organIndex]�� ��ġ ���Դϴ�.
            // A�� Position
            // D�� Direction


            //�Լ��� ���� ����
            // � ����迡 �¾Ҵ��� üũ�ϰ� ����� boolean �迭�� ��Ÿ����.
            // bool �迭�߿��� true�� ����踦 ���� ��� ������ �¾Ѵ��� 


            // ��� ����迡 �ش� ������ �¾Ҵ��� üũ�Ѵ�
            //bool[] isOrganCollided = Enumerable.Repeat<bool>(false, organParts.Length).ToArray(); // ���ϱ� �ѵ� �Լ� ȣ���� �� ���̰ڴµ�?

            bool[] isOrganCollided = new bool[organParts.Length];
            float[] priority = new float[organParts.Length]; // ��� ����谡 ���� �¾Ҵ����� ���� �˾ƺ��� ����, ����������� ���ư� �̵��Ÿ��� �����ϴ� �����Դϴ�.
            float[] angles = new float[organParts.Length];

            for (int index = 0; index < organParts.Length; ++index)
            {
                isOrganCollided[index] = false;
                priority[index] = -1.0f;
                angles[index] = 0.0f;
            }

            for (int organIndex = 0; organIndex < organParts.Length; organIndex++)
            {
                // ��� �浹 ������ üũ�մϴ�.
                Spheres CollisionSpheres = organParts[organIndex].collisionRangeSphere;

                bool isHit = false; // �ش� ������ ����踦 �¾Ҵ��� �����ϴ� �����Դϴ�.
                float minAT = float.MaxValue; // ����迡 �¾�����, ������������� ���� �������� ���� ª�� �Ÿ��� �����ϴ� �����Դϴ�.
                float angleMinAT = 0.0f; // ����迡 �¾�����, ���� ª�� �Ÿ��� ��쿡 �����ϴ� ������ ���Դϴ�.

                for (int collisionIndex = 0; collisionIndex < CollisionSpheres.Length; collisionIndex++)
                {
                    #region ���������� ����
                    // �� �𼭸��� ���̰� 1�� ������ü
                    // �׸��� �� ���������� �ִ� �������� ��, (OrganSphere��� ����.)
                    // �׸��� ������ü�� �Ѹ鿡�� ����Ͽ�(attack Position), ������ü�� �������� ���ϸ�(attack Direction), �������� ��ġ�� �������� ���������� OrganSphere�� �ݵ�� ����ġ�� ������ AttackLine�� �ִ�.
                    // Organ Sphere�� ������ ���� R�̶�� ����.
                    // Attack Position�� ���� A, Attack Direcion�� ���⺤�ʹ� D, OrganSphere�� �߽����� S��� ����
                    // �̶� AttackLine�� OrganSphere�� ������ �� �� ��, attack Position�� ���� ����� ���� T��� ����. T : (Tx, Ty, Tz)
                    // �̶� OrganSphere�� �߽������� AttackLine���� ������ ������ ���� H��� ����.
                    #endregion
                    // AS���� = �ε��� �κ� -> ����� ����
                    Vector3 attack2organVector = CollisionSpheres[collisionIndex].position - position;
                    // AS ���� ���� D����
                    float attack2organ_dot_direction = Vector3.Dot(attack2organVector, direction);
                    // R�� ����- AS ������ ũ���� ���� + ����AS ���� ����D �� ���� = R�� ���� - ���� AH�� ���� = TH�� ����
                    float ThSquare
                        = Mathf.Pow(CollisionSpheres[collisionIndex].radius, 2)
                        - Mathf.Pow(attack2organVector.x, 2)
                        - Mathf.Pow(attack2organVector.y, 2)
                        - Mathf.Pow(attack2organVector.z, 2)
                        + Mathf.Pow(attack2organ_dot_direction, 2);

                    if (ThSquare > 0) // ����迡 �ش� ������ �ε������ϴ�.
                    {
                        // ������ ���մϴ�.
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

            // �䱸 ���� : � ����谡 ���� �������� �Դ���
            // priority�� ���� ���� ���� �༮�� �켱���� �����ϴ�.
            // isOrganCollided ���� ���� ��� �迭�� ���ҿ� ���� �ʽ��ϴ�.
            // �迭�� ���Ҵ� organParts�� �ε��� ��ȣ�Դϴ�.
            // �켱���� ���� �ε��� ��ȣ�� �迭�� �տ� �ɴϴ�.
            // forEach�� �̿��ؼ� �ε��� ��ȣ�� ������ �����Ͽ� �� �Լ��� ȣ���� ���Դϴ�.
            
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
    ///     ���� �̸��� ����� �����Դϴ�. </summary>
    /// <remarks>
    ///     ���� ���ο� ���� ���� ���� �ִٸ�, ���⿡ ���ο� ���� �̸��� �߰��Ͻø� �˴ϴ�.</remarks>
    public static class Species
    {
        public const string DEFAULT = "creature";
        public const string HUMAN = "human";
    }

    
}
