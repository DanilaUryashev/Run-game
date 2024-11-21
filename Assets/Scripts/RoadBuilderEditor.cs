using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(RoadBuilder))]
public class RoadBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoadBuilder roadBuilder = (RoadBuilder)target;

        // Добавление поля для изменения отступа
        roadBuilder.offset = EditorGUILayout.FloatField("Offset", roadBuilder.offset);

        if (GUILayout.Button("Add Straight Segment"))
        {
            roadBuilder.AddStraightSegment(null);
        }

        if (GUILayout.Button("Add Right Turn Segment"))
        {
            roadBuilder.AddRightTurnSegment();
        }

        if (GUILayout.Button("Add Left Turn Segment"))
        {
            roadBuilder.AddLeftTurnSegment();
        }
        if (GUILayout.Button("Add School Segment"))
        {
            roadBuilder.SchoolAndParty();
        }
        if (GUILayout.Button("Add Finish Segment"))
        {
            roadBuilder.SchoolAndParty();
        }

        if (GUILayout.Button("Remove Last Segment"))
        {
            roadBuilder.RemoveLastSegment();
        }
        if (GUILayout.Button("ResetRoad"))
        {
            roadBuilder.ResetRoad();
        }
        if (GUILayout.Button("Add Bottles And Dollars"))
        {
            roadBuilder.AddBottleAndDollarSegment();
        }

        DrawDefaultInspector(); // Рисуем стандартные элементы инспектора
    }
}
