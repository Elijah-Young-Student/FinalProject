using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Card card = (Card)target;

        Color bgColor = GetBackgroundColor(card.actionType);

        GUI.backgroundColor = bgColor;

        GUILayout.BeginVertical("box");

        GUI.backgroundColor = Color.white;

        GUILayout.Label(
            string.IsNullOrEmpty(card.cardName)
                ? "Unnamed Card"
                : card.cardName,
            EditorStyles.boldLabel
        );

        serializedObject.Update();

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("cardPrefab")
        );

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("cardName")
        );

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("description")
        );

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("cost")
        );

        SerializedProperty cardDraws =
            serializedObject.FindProperty("cardDraws");

        EditorGUILayout.PropertyField(cardDraws);

        if (cardDraws.boolValue)
        {
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("cardsToDraw")
            );
        }

        SerializedProperty cpChange =
            serializedObject.FindProperty("CPChange");

        EditorGUILayout.PropertyField(cpChange);

        if (cpChange.boolValue)
        {
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("CPToGain")
            );
        }

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("actionType")
        );

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("cardType")
        );

        serializedObject.ApplyModifiedProperties();

        GUILayout.EndVertical();
    }

    private Color GetBackgroundColor(Card.ActionType actionType)
    {
        switch (actionType)
        {
            case Card.ActionType.Instant:
                return new Color(1f, 0.5f, 0.5f);

            case Card.ActionType.MainPhase:
                return new Color(0.5f, 0.7f, 1f);

            case Card.ActionType.RollPhase:
                return new Color(1f, 0.7f, 0.3f);

            case Card.ActionType.DefensivePhase:
                return new Color(0.5f, 1f, 0.5f);

            default:
                return Color.white;
        }
    }
}