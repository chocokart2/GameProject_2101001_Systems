using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTurret : MonoBehaviour
{
    //component
    UnitBase myUnitBase;
    GameObjectList myGameObjectList;
    UnitItemPack myUnitItemPack;

    //component class
    GameManager.AttackClass bulletAttackClass;

    //DEBUG AttackTurretIsFunctionable
    float time;
    float angle;




    // Start is called before the first frame update
    void Start()
    {
        // component connect
        myUnitBase = GetComponent<UnitBase>();
        myGameObjectList = GetComponent<GameObjectList>();
        myUnitItemPack = GetComponent<UnitItemPack>();



        #region 아이템 조작 부분, 데이터로 로딩되는 경우 이 부분은 제거해주셔야 합니다.
        myUnitItemPack.inventory = DemoItem.GetDemoInventoryCustom(DemoItem.Name.TURRET);
        myUnitItemPack.inventory[0].subItems[0].attackInfo = DemoAttackInfo.GetDemoAttackInfoData(10000);
        time = 0.0f;
        angle = 0.0f;
        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //angle += Time.deltaTime;
        if (time > 0.2f)
        {
            time = 0.0f;
            ShotBullet();
            //SetAngleFromRight(angle);
            SetAngleFromRight(Random.Range(0, 360));
        }
        if(angle > 360.0f)
        {
            angle = 0.0f;
        }

    }

    #region [인터페이스 함수 - MachineNetMessage으로 통제될 것입니다.]

    public void SetAngleFromRight(float angle360)
    {
        // 함수 설명: vector3.right ( Vector(1,0,0) ) 기준으로 얼만큼 Y축을 중심축으로 돌아가는지 설정합니다.
        float angleRad = angle360 * Mathf.Rad2Deg;
        myUnitBase.SetUnitDirection(new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)));
    }
    public void ShotBullet()
    {
        if (myUnitItemPack == null) Hack.Say(Hack.isDebugAttackTurret, Hack.check.error, this, message: "myUnitItemPack는 null 값입니다.");

        myUnitItemPack?.inventory?.Use(gameObject, myUnitBase.unitBaseData.direction);
    }
    #endregion



}
