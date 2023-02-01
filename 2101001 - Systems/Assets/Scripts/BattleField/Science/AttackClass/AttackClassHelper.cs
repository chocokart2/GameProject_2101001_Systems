using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ���ݿ� ���� ������ ��� �ִ� AttackInfo�� ���Ͽ� ������ ������Ʈ Ŭ�����Դϴ�.
/// </summary>
public class AttackClassHelper : MonoBehaviour
{
    /// <summary>
    /// ���ݿ� ���� �� ����ü, ���� ��� Į, �Ѿ� ���� ������ �ִ� ���ݿ� ���� �����Դϴ�.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [System.Serializable]
    public class AttackInfo
    {
        public ChemicalHelper.Chemicals chemicals;
        public EnergyHelper.Energies energies;
    }
}

// �߰����� �̾߱�
// + � �������� ������ ���� ���� / �ӵ��� �� �������� EnergyHelper.Energies�� � �������� �����ϰ� �ǹǷ� ǥ������ �ʾҽ��ϴ�.