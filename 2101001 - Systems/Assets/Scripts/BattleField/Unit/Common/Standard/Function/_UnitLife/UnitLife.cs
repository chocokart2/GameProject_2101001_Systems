using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ������ ����ִ��� ���θ� ������ �� �ִ� Ŭ�����Դϴ�.
/// </summary>
/// <remarks>
///     <para>
///         �� ������Ʈ�� UnitPart�� �������: ������ Ư���� Part�� ���� �� ���� ���մϴ�. 
///         �� ������Ʈ�� ����ϴ� UnitPart�� �ı��Ǵ� ���, �� ������ �ٸ� �κ��� ������ �� �����ϴ�.
///         �� ������Ʈ�� ����ϴ� UnitPart�� �ı��Ǵ� ���, �� ���� �ٲٴ� �Լ��� ȣ���ؾ� �մϴ�.
///         �׷���, �ٸ� UnitPart�� � �ൿ�� ���Ҷ����� �� ������Ʈ���� �����ϴµ�, �� ���氪�� �ݿ��� ���Դϴ�.</para></remarks>
public class UnitLife : MonoBehaviour,
    BaseComponent.IDataGetableComponent<UnitLife.Life>,
    BaseComponent.IDataSetableComponent<UnitLife.Life>
{
    #region UnitLifeClass
    // field
    public Life self;
    // property
    public bool IsAlive
    {
        get
        {
            if (self == null) return false;
            else return self.isLiving;
        }
    }

    // unity function
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // function
#warning DemoFunction;
    public static Life GetDemoLife()
    {
        return new Life() { isLiving = true };
    }
    // interface
    public Life GetComponentData() { return self; }
    public void SetComponentData(Life data) { self = data; }
    // just public
    public void Kill()
    {
        if (self.isLiving)
        {
            Hack.Say(Hack.Scope.UnitLife.Kill, Hack.check.info, this,
                message: $"��� ó�� �Ǿ����ϴ�. ��ü �̸� : {name}");

            if (Hack.TrapNull(transform)) Debug.Log("AAA!");
            GameObject instantiatedObject = Instantiate(GameObjectList.ParticlesPrefabs.DeadIcon, transform.position + new Vector3(0,1,0), Quaternion.identity, transform);
        }


        self.isLiving = false;
        // ���⿡ �̹��� �ٲٱ�.
        UnitAppearance temp_Appearance = GetComponent<UnitAppearance>();
    }

    #endregion
    #region MestedClass
    /// <summary>
    /// Ư�� ������ ���� ���θ� ��� ���� Ŭ�����Դϴ�. �밳 ������Ʈ�� �ֽ��ϴ�.
    /// </summary>
    [System.Serializable]
    public class Life
    {
        /// <summary>
        ///     �� ������ ����ִ��� ���θ� �����ϴ�.
        /// </summary>
        public bool isLiving;
    }
    #endregion



}
