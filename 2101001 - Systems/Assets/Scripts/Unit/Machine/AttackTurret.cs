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
        //UnitItemPack.UnitItemPackData fakeItemData = new UnitItemPack.UnitItemPackData();
        //fakeItemData.inventory = new UnitItemPack.ItemData[] { new UnitItemPack.ItemData() };
        //fakeItemData.inventory[0].itemType = "Pistol";
        //fakeItemData.inventory[0].isRealItem = false;
        //UnitItemPack.UnitItemPackData fakeItemPackData = new UnitItemPack.UnitItemPackData();
        //fakeItemPackData = UnitItemPack.NewUnitItemPackData(new UnitItemPack.ItemData[] { UnitItemPack.NewItemData("Pistol") });
        UnitItemPack.UnitItemPackData fakeItemPackData = UnitItemPack.NewUnitItemPackData(new UnitItemPack.ItemData[] { UnitItemPack.NewItemData("Turret") });

        myUnitItemPack.InventorySet(fakeItemPackData);
        time = 0.0f;
        angle = 0.0f;
        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        angle += Time.deltaTime;
        if (time > 0.1f)
        {
            time = 0.0f;
            ShotBullet();
            SetAngleFromRight(angle);
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
        //UnitDirection의 방향대로 공격 아이템 발사
        if(myUnitItemPack != null)
        {
            if(myUnitItemPack.inventory != null)
            {
                myUnitItemPack.ItemUse(myUnitBase.unitBaseData.direction);
            }
        }
    }
    #endregion



}
