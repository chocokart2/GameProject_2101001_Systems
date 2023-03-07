using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HumanUnitBase))]
public class HumanPartBaseInspector : Editor
{
    #region Member
    #region Field
    HumanUnitBase component;
    bool isShowMember = false;

    
    #endregion
    #region Function

    void OnEnable()
    {
        component = (HumanUnitBase)target;
    }

    public override void OnInspectorGUI()
    {
        //bool[] isShowOrganPartIndex = new bool[component.individual.organParts.Length];

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("설명 : \"HumanPart의 기본적인 정보를 담고 있습니다.\"");
        EditorGUILayout.Space();

        isShowMember = EditorGUILayout.Foldout(isShowMember, "정보 뜯어보기");
        if (isShowMember)
        {
            EditorGUILayout.LabelField("내부 정보");
            for (int i = 0; i < component.individual.organParts.Length; i++)
            {
                EditorGUILayout.Space(3);
                EditorGUILayout.LabelField($"organParts[{i}] {component.individual.organParts[i].Name}");


                // Tagged Chemicals <write / read>
                // Demand Chemicals <write / read>
                // Others Chemicals <write / read>
                // Chemicalwholeness.wholeness <read>
#warning 추가 요소
                // chemicalWholeness.AngleWholeness직선Graph <Read>
            }
            

        }



        base.OnInspectorGUI();
        EditorGUILayout.EndVertical();
    }
    #endregion
    #endregion






}
