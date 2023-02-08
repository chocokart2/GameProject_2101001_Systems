using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     유닛의 각도에 따라 자신의 모습을 보여주니다.
/// </summary>
public class UnitAppearance : MonoBehaviour
{
    // public field
    // private field
    private bool m_isMatarialFilled;
    // private instance component field
    private UnitBase m_unitBase;
    private MeshRenderer m_meshRenderer;
    [SerializeField] private Material materialPositiveX; // 유닛이 바라보는 방향이 양의 X일때
    [SerializeField] private Material materialPositiveY; // 유닛이 바라보는 방향이 양의 Y일때
    [SerializeField] private Material materialNegativeX; // 유닛이 바라보는 방향이 음의 X일때
    [SerializeField] private Material materialNegativeY; // 유닛이 바라보는 방향이 음의 Y일때
    [SerializeField] private Material materialPositiveXDead; // 유닛이 바라보는 방향이 양의 X일때
    [SerializeField] private Material materialPositiveYDead; // 유닛이 바라보는 방향이 양의 Y일때
    [SerializeField] private Material materialNegativeXDead; // 유닛이 바라보는 방향이 음의 X일때
    [SerializeField] private Material materialNegativeYDead; // 유닛이 바라보는 방향이 음의 Y일때
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

        // 자신의 각도를 변경합니다.
        Vector3 vecDirection = direction.normalized;

        // 자신의 외형을 변경합니다.
        if (m_isMatarialFilled  && (m_meshRenderer != null))
        {
            //
            if (direction.z > Mathf.Cos(45 * Mathf.Deg2Rad)) // 북쪽
            {
                m_meshRenderer.material = materialPositiveY;
            }
            else if (direction.z < Mathf.Cos(135 * Mathf.Deg2Rad)) // 남쪽
            {
                m_meshRenderer.material = materialNegativeY;
            }
            else if (direction.x >= Mathf.Cos(45 * Mathf.Deg2Rad)) // 동쪽
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
