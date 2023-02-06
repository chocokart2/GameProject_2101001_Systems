using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     �׽�Ʈ�� ���� �����ϴ� Ŭ�����Դϴ�. �ϵ��ڵ��� ��ü�� �������ִ� ���� �޼ҵ带 ������ �ֽ��ϴ�.
/// </summary>
/// <remarks>
///     UnitItemPack���� �׽�Ʈ������ �����ߴ� �����ڴ� �����! UnitItemPack�� �κ��丮�� ��� ���ҿ��� �����ϴ�.
/// </remarks>
public class DemoItem : ItemHelper
{
    #region item - weapon - knife
    static public Knife GetDemoKnife()
    {
        Knife result = new Knife()
        {
            Name = "[Demo] melee",
            subItems = new SubItemArray(
                new SubItem()
                {
                    name = "blade",
                    amount = 1,
                    attackInfo = DemoAttackInfo.GetDemoAttackInfoData()
                }
                )
        };
        return result;
    }
    #endregion
    #region item - weapon - pistol
    static public Pistol GetDemoPistol()
    {
        Pistol result = new Pistol(7)
        {
            Name = "[Demo] pistol",
            subItems = new SubItemArray(
                new SubItem()
                {
                    Name = "bullet",
                    amount = 7,
                    attackInfo = DemoAttackInfo.GetDemoAttackInfoData()
                }
                )
        };
        return result;
    }
    #endregion
    #region item - weapon - rifle

    #endregion
    #region item - build
    #region item - build - buildtool
    static public BuildTool GetDemoBuildTool()
    {
        BuildTool result = new BuildTool()
        {
            Name = "[Demo] hammer",
            subItems = new SubItemArray(
                new SubItem()
                {
                    name = "MachineUnit101",
                    amount = 3
                }, // 101
                new SubItem()
                {
                    name = "MachineUnit102",
                    amount = 3
                }, // 102
                new SubItem()
                {
                    name = "MachineUnit103",
                    amount = 3
                }, // 103
                new SubItem()
                {
                    name = "MachineUnit201",
                    amount = 3
                }, // 201
                new SubItem()
                {
                    name = "MachineUnit301",
                    amount = 3
                }, // 301
                new SubItem()
                {
                    name = "MachineUnit502",
                    amount = 3
                }, // 502
                new SubItem()
                {
                    name = "MachineUnit702",
                    amount = 3
                }, // 702
                new SubItem()
                {
                    name = "MachineUnit803",
                    amount = 3
                }  // 803
                )
        };
        return result;
    }
    #endregion
    #endregion
    #region mach - weapon - cannnon
    static public Turret GetDemoTurret()
    {
        Turret result = new Turret()
        {
            Name = "[Demo] turret",
            subItems = new SubItemArray(
                new SubItem()
                {
                    Name = "ammo",
                    amount = 2000,
                    attackInfo = DemoAttackInfo.GetDemoAttackInfoData()
                }
                )
        };
        return result;
    }
    #endregion

    #region inventory - default

    #endregion
    #region inventory - watcher
    public Inventory GetDemoInventoryWatcher()
    {
        Inventory result = new Inventory()
        {
            self = new Item[3]
            {
                new Radio(),
                new Knife(),
                new Blank()
            }
        };
        return result;
    }
    #endregion
    #region inventory - warrior
    static public Inventory GetDemoInventoryWarrior()
    {
        Inventory result = new Inventory()
        {
            self = new Item[3]
            {
                new Radio(),
                new Knife(),
                new Pistol()
            }
        };
        return result;
    }
    #endregion
    #region inventory - builder
    static public Inventory GetDemoInventoryBuilder()
    {
        Inventory result = new Inventory()
        {
            self = new Item[3]
            {
                new Radio(),
                new Knife(),
                new BuildTool()
            }
        };
        return result;
    }
    #endregion
    #region inventory - supplyer
    static public Inventory GetDemoInventorySupplyer()
    {
        Inventory result = new Inventory()
        {
            self = new Item[3]
            {
                new Radio(),
                new Knife(),
                new Blank()
            }
        };
        return result;
    }
    #endregion
    #region inventory - custom
    /// <summary>
    ///     ��Ʈ���� �Ű������� �޾� �κ��丮�� �������� �����մϴ�.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static Inventory GetDemoInventoryCustom(string[] items)
    {
        Inventory result = new Inventory();
        result.self = new Item[items.Length];

        for (int inventoryIndex = 0; inventoryIndex < items.Length; ++inventoryIndex)
        {
            switch (items[inventoryIndex])
            {
                case "Radios":
                    result[inventoryIndex] = new Radio();
                    break;
                case "Knife":
                    result[inventoryIndex] = new Knife();
                    break;
                case "Pistol":
                    result[inventoryIndex] = new Pistol();
                    break;
                case "BuildTool":
                    result[inventoryIndex] = new BuildTool();
                    break;
                case "Turret":
                    result[inventoryIndex] = new Turret();
                    break;
                case "Blank":
                default:
                    result[inventoryIndex] = new Blank();
                    break;
            }
        }
        return result;
    }
    #endregion

}
