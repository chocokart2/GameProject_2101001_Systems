using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ������ ������ ���� �ڽ��� ����� �����ִϴ�.
/// </summary>
public class UnitAppearance : MonoBehaviour
{
    // public field
    // private field
    private bool m_isMatarialFilled;
    // private instance component field
    private UnitBase m_unitBase;
    private MeshRenderer m_meshRenderer;
    [SerializeField] private Material materialPositiveX; // ������ �ٶ󺸴� ������ ���� X�϶�
    [SerializeField] private Material materialPositiveY; // ������ �ٶ󺸴� ������ ���� Y�϶�
    [SerializeField] private Material materialNegativeX; // ������ �ٶ󺸴� ������ ���� X�϶�
    [SerializeField] private Material materialNegativeY; // ������ �ٶ󺸴� ������ ���� Y�϶�
    [SerializeField] private Material materialPositiveXDead; // ������ �ٶ󺸴� ������ ���� X�϶�
    [SerializeField] private Material materialPositiveYDead; // ������ �ٶ󺸴� ������ ���� Y�϶�
    [SerializeField] private Material materialNegativeXDead; // ������ �ٶ󺸴� ������ ���� X�϶�
    [SerializeField] private Material materialNegativeYDead; // ������ �ٶ󺸴� ������ ���� Y�϶�
    // private static field
    private const string humanDefaultFolder = "Materials/BattleField/Unit/Biological/Human/Base";
    
    // Start is called before the first frame update
    void Start()
    {
        m_isMatarialFilled =
            (materialPositiveX != null) &&
            (materialPositiveY != null) &&
            (materialNegativeX != null) &&
            (materialNegativeY != null);
        m_unitBase = GetComponent<UnitBase>();
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_unitBase != null)
            SetUnitDirection(m_unitBase.Direction);
    }

    public void SetUnitDirection(Vector3 direction)
    {
        //Debug.Log("DEBUG_UnitBase.Setdirection: It Worked!");

        // �ڽ��� ������ �����մϴ�.
        Vector3 vecDirection = direction.normalized;

        // �ڽ��� ������ �����մϴ�.
        if (m_isMatarialFilled  && (m_meshRenderer != null))
        {
            //
            if (direction.z > Mathf.Cos(45 * Mathf.Deg2Rad)) // ����
            {
                m_meshRenderer.material = materialPositiveY;
            }
            else if (direction.z < Mathf.Cos(135 * Mathf.Deg2Rad)) // ����
            {
                m_meshRenderer.material = materialNegativeY;
            }
            else if (direction.x >= Mathf.Cos(45 * Mathf.Deg2Rad)) // ����
            {
                m_meshRenderer.material = materialPositiveX;
            }
            else
            {
                m_meshRenderer.material = materialNegativeX;
            }
        }
    }

    public void Kill()
    {

    }
}
