using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     유닛이 살아있는지 여부를 저장할 수 있는 클래스입니다.
/// </summary>
/// <remarks>
///     <para>
///         이 컴포넌트와 UnitPart의 상관관계: 유닛의 특정한 Part에 의해 이 값이 변합니다. 
///         이 컴포넌트를 담당하는 UnitPart가 파괴되는 경우, 그 유닛의 다른 부분을 조종할 수 없습니다.
///         이 컴포넌트를 담당하는 UnitPart가 파괴되는 경우, 이 값을 바꾸는 함수를 호출해야 합니다.
///         그러면, 다른 UnitPart가 어떤 행동을 취할때마다 이 컴포넌트값을 참고하는데, 그 변경값이 반영될 것입니다.</para></remarks>
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
                message: $"사망 처리 되었습니다. 객체 이름 : {name}");

            if (Hack.TrapNull(transform)) Debug.Log("AAA!");
            GameObject instantiatedObject = Instantiate(GameObjectList.ParticlesPrefabs.DeadIcon, transform.position + new Vector3(0,1,0), Quaternion.identity, transform);
        }


        self.isLiving = false;
        // 여기에 이미지 바꾸기.
        UnitAppearance temp_Appearance = GetComponent<UnitAppearance>();
    }

    #endregion
    #region MestedClass
    /// <summary>
    /// 특정 유닛의 생존 여부를 담기 위한 클래스입니다. 대개 컴포넌트에 넣습니다.
    /// </summary>
    [System.Serializable]
    public class Life
    {
        /// <summary>
        ///     이 유닛이 살아있는지 여부를 가집니다.
        /// </summary>
        public bool isLiving;
    }
    #endregion



}
