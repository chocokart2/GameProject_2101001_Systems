using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicPartBase : UnitPartBase
{
    //nested class
    public class MechanicalPart: UnitPart
    {
        public override float wholeness
        {
            get => throw new System.NotImplementedException(); 
            set => throw new System.NotImplementedException();
        }
    }
    #region ���� Ŭ����
    /// <summary>
    ///     �ӽ��� �̸����� ����� �����Դϴ�.
    /// </summary>
    /// <remarks>
    ///     ���� ���ο� �ӽ��� ����� �Ǹ� ���⿡ ���ο� �̸��� �߰��Ͻø� �˴ϴ�.
    /// </remarks>
    public static class MechanicType
    {
        public const string DEFAULT = "machine";
        


        public static class Machine
        {
            public const string DEFAULT = "machine";

        }
        public static class Hareketli // �����̴� ��ü
        {

        }
    }


    #endregion
}
