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
    #region 정적 클래스
    /// <summary>
    ///     머신의 이름들을 명시한 구간입니다.
    /// </summary>
    /// <remarks>
    ///     만약 새로운 머신을 만들게 되면 여기에 새로운 이름을 추가하시면 됩니다.
    /// </remarks>
    public static class MechanicType
    {
        public const string DEFAULT = "machine";
        


        public static class Machine
        {
            public const string DEFAULT = "machine";

        }
        public static class Hareketli // 움직이는 개체
        {

        }
    }


    #endregion
}
