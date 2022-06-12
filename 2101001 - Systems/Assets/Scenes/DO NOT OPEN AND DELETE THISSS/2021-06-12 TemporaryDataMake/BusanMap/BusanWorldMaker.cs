using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusanWorldMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WorldSet();
    }


    //GeumJeong

    void WorldSet()
    {
        #region Area(District)
        WorldManager.District GeumJeong = new WorldManager.District();
        GeumJeong.name = "GeumJeong";
        GeumJeong.adjacentArea = new List<string>();
        GeumJeong.adjacentArea.Add("GiJang");
        GeumJeong.adjacentArea.Add("HaeUnDae");
        GeumJeong.adjacentArea.Add("DongNae");
        GeumJeong.adjacentArea.Add("Buk");
        GeumJeong.sceneName = "BattleField_Busan_GeumJeong";
        GeumJeong.IconPosition = new Vector3(3, 0, 4);

        WorldManager.District GiJang = new WorldManager.District();
        GiJang.name = "GiJang";
        GiJang.adjacentArea = new List<string>();
        GiJang.adjacentArea.Add("GeumJeong");
        GiJang.adjacentArea.Add("HaeUnDae");
        GiJang.sceneName = "BattleField_Busan_GiJang";
        GiJang.IconPosition = new Vector3(6, 0, 4);

        WorldManager.District GangSeo = new WorldManager.District();
        GangSeo.name = "GangSeo";
        GangSeo.adjacentArea = new List<string>();
        GangSeo.adjacentArea.Add("Buk");
        GangSeo.adjacentArea.Add("SaSang");
        GangSeo.adjacentArea.Add("SaHa");
        GangSeo.sceneName = "BattleField_Busan_GangSeo";
        GangSeo.IconPosition = new Vector3(-3, 0, 2);

        WorldManager.District Buk = new WorldManager.District();
        Buk.name = "Buk";
        Buk.adjacentArea = new List<string>();
        Buk.adjacentArea.Add("GeumJeong");
        Buk.adjacentArea.Add("DongNae");
        Buk.adjacentArea.Add("BuSanJin");
        Buk.adjacentArea.Add("SaSang");
        Buk.adjacentArea.Add("GangSeo");
        Buk.sceneName = "BattleField_Busan_Buk";
        Buk.IconPosition = new Vector3(0, 0, 2);

        WorldManager.District DongNae = new WorldManager.District();
        DongNae.name = "DongNae";
        DongNae.adjacentArea = new List<string>();
        DongNae.adjacentArea.Add("GeumJeong");
        DongNae.adjacentArea.Add("HaeUnDae");
        DongNae.adjacentArea.Add("YeonJe");
        DongNae.adjacentArea.Add("BuSanJin");
        DongNae.adjacentArea.Add("Buk");
        DongNae.sceneName = "BattleField_Busan_DongNae";
        DongNae.IconPosition = new Vector3(3, 0, 2);

        WorldManager.District HaeUnDae = new WorldManager.District();
        HaeUnDae.name = "HaeUnDae";
        HaeUnDae.adjacentArea = new List<string>();
        HaeUnDae.adjacentArea.Add("SuYeong");
        HaeUnDae.adjacentArea.Add("YeonJe");
        HaeUnDae.adjacentArea.Add("DongNae");
        HaeUnDae.adjacentArea.Add("GeumJeong");
        HaeUnDae.adjacentArea.Add("GiJang");
        HaeUnDae.sceneName = "BattleField_Busan_HaeUnDae";
        HaeUnDae.IconPosition = new Vector3(6, 0, 2);

        WorldManager.District SaHa = new WorldManager.District();
        SaHa.name = "SaHa";
        SaHa.adjacentArea = new List<string>();
        SaHa.adjacentArea.Add("GangSeo");
        SaHa.adjacentArea.Add("SaSang");
        SaHa.adjacentArea.Add("Seo");
        SaHa.sceneName = "BattleField_Busan_SaHa";
        SaHa.IconPosition = new Vector3(-6, 0, 0);

        WorldManager.District SaSang = new WorldManager.District();
        SaSang.name = "SaSang";
        SaSang.adjacentArea = new List<string>();
        SaSang.adjacentArea.Add("GangSeo");
        SaSang.adjacentArea.Add("Buk");
        SaSang.adjacentArea.Add("BuSanJin");
        SaSang.adjacentArea.Add("Seo");
        SaSang.adjacentArea.Add("SaHa");
        SaSang.sceneName = "BattleField_Busan_SaSang";
        SaSang.IconPosition = new Vector3(-3, 0, 0);

        WorldManager.District BuSanJin = new WorldManager.District();
        BuSanJin.name = "BuSanJin";
        BuSanJin.adjacentArea = new List<string>();
        BuSanJin.adjacentArea.Add("Buk");
        BuSanJin.adjacentArea.Add("DongNae");
        BuSanJin.adjacentArea.Add("YeonJe");
        BuSanJin.adjacentArea.Add("Nam");
        BuSanJin.adjacentArea.Add("Dong");
        BuSanJin.adjacentArea.Add("Seo");
        BuSanJin.adjacentArea.Add("SaSang");
        BuSanJin.sceneName = "BattleField_Busan_BuSanJin";
        BuSanJin.IconPosition = new Vector3(0, 0, 0);

        WorldManager.District YeonJe = new WorldManager.District();
        YeonJe.name = "YeonJe";
        YeonJe.adjacentArea = new List<string>();
        YeonJe.adjacentArea.Add("DongNae");
        YeonJe.adjacentArea.Add("HaeUnDae");
        YeonJe.adjacentArea.Add("SuYeong");
        YeonJe.adjacentArea.Add("Nam");
        YeonJe.adjacentArea.Add("BuSanJin");
        YeonJe.sceneName = "BattleField_Busan_YeonJe";
        YeonJe.IconPosition = new Vector3(3, 0, 0);

        WorldManager.District SuYeong = new WorldManager.District();
        SuYeong.name = "SuYeong";
        SuYeong.adjacentArea = new List<string>();
        SuYeong.adjacentArea.Add("Nam");
        SuYeong.adjacentArea.Add("YeonJe");
        SuYeong.adjacentArea.Add("HaeUnDae");
        SuYeong.sceneName = "BattleField_Busan_SuYeong";
        SuYeong.IconPosition = new Vector3(6, 0, 0);

        WorldManager.District Seo = new WorldManager.District();
        Seo.name = "Seo";
        Seo.adjacentArea = new List<string>();
        Seo.adjacentArea.Add("SaHa");
        Seo.adjacentArea.Add("SaSang");
        Seo.adjacentArea.Add("BuSanJin");
        Seo.adjacentArea.Add("Dong");
        Seo.adjacentArea.Add("Jung");
        Seo.sceneName = "BattleField_Busan_Seo";
        Seo.IconPosition = new Vector3(-3, 0, -2);

        WorldManager.District Dong = new WorldManager.District();
        Dong.name = "Dong";
        Dong.adjacentArea = new List<string>();
        Dong.adjacentArea.Add("Jung");
        Dong.adjacentArea.Add("Seo");
        Dong.adjacentArea.Add("BuSanJin");
        Dong.adjacentArea.Add("Nam");
        Dong.sceneName = "BattleField_Busan_Dong";
        Dong.IconPosition = new Vector3(0, 0, -2);

        WorldManager.District Nam = new WorldManager.District();
        Nam.name = "Nam";
        Nam.adjacentArea = new List<string>();
        Nam.adjacentArea.Add("Dong");
        Nam.adjacentArea.Add("BuSanJin");
        Nam.adjacentArea.Add("YeonJe");
        Nam.adjacentArea.Add("SuYeong");
        Nam.sceneName = "BattleField_Busan_Nam";
        Nam.IconPosition = new Vector3(3, 0, -2);

        WorldManager.District Jung = new WorldManager.District();
        Jung.name = "Jung";
        Jung.adjacentArea = new List<string>();
        Jung.adjacentArea.Add("Seo");
        Jung.adjacentArea.Add("Dong");
        Jung.adjacentArea.Add("YeongDo");
        Jung.sceneName = "BattleField_Busan_Jung";
        Jung.IconPosition = new Vector3(3, 0, -4);

        WorldManager.District YeongDo = new WorldManager.District();
        YeongDo.name = "YeongDo";
        YeongDo.adjacentArea = new List<string>();
        YeongDo.adjacentArea.Add("Jung");
        YeongDo.sceneName = "BattleField_Busan_YeongDo";
        YeongDo.IconPosition = new Vector3(0, 0, -6);
        #endregion

        WorldManager.World Busan = new WorldManager.World();
        Busan.worldName = "Busan";
        Busan.Area = new Dictionary<string, WorldManager.District>();
        Busan.Area.Add("GeumJeong", GeumJeong);
        Busan.Area.Add("GiJang", GiJang);
        Busan.Area.Add("GangSeo", GangSeo);
        Busan.Area.Add("Buk", Buk);
        Busan.Area.Add("DongNae", DongNae);
        Busan.Area.Add("HaeUnDae", HaeUnDae);
        Busan.Area.Add("SaHa", SaHa);
        Busan.Area.Add("SaSang", SaSang);
        Busan.Area.Add("BuSanJin", BuSanJin);
        Busan.Area.Add("YeonJe", YeonJe);
        Busan.Area.Add("SuYeong", SuYeong);
        Busan.Area.Add("Seo", Seo);
        Busan.Area.Add("Dong", Dong);
        Busan.Area.Add("Nam", Nam);
        Busan.Area.Add("Jung", Jung);
        Busan.Area.Add("YeongDo", YeongDo);

        GameManager.DataSaving(Busan.ToData(), "World", "World_Busan", "Busan");
    }
}
