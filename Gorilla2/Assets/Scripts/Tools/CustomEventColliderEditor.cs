using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(EventCollider))]
public class CustomEventColliderEditor : Editor
{
    private SerializedProperty eventEntriesProp;

    private void OnEnable()
    {
        eventEntriesProp = serializedObject.FindProperty("eventEntries");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EventCollider collider = (EventCollider)target;

        GUILayout.Space(10);

        for (int i = 0; i < eventEntriesProp.arraySize; i++)
        {
            SerializedProperty entry = eventEntriesProp.GetArrayElementAtIndex(i);
            SerializedProperty eventTypeProp = entry.FindPropertyRelative("eventType");
            SerializedProperty callbackProp = entry.FindPropertyRelative("callback");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(eventTypeProp.enumDisplayNames[eventTypeProp.enumValueIndex], EditorStyles.boldLabel);

            if (GUILayout.Button("❌", GUILayout.Width(30)))
            {
                eventEntriesProp.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(callbackProp);

            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add Event"))
        {
            GenericMenu menu = new GenericMenu();

            foreach (EventCollider.EventType eventType in System.Enum.GetValues(typeof(EventCollider.EventType)))
            {
                if (!EventAlreadyExists(collider, eventType))
                {
                    menu.AddItem(new GUIContent(eventType.ToString()), false, () => AddEvent(collider, eventType));
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent(eventType.ToString()));
                }
            }

            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private static bool EventAlreadyExists(EventCollider collider, EventCollider.EventType type)
    {
        return collider.eventEntries.Any(entry => entry.eventType == type);
    }

    private static void AddEvent(EventCollider collider, EventCollider.EventType type)
    {
        Undo.RecordObject(collider, "Add Event Entry");

        EventCollider.EventEntry newEntry = new EventCollider.EventEntry
        {
            eventType = type
        };

        collider.eventEntries.Add(newEntry);

        EditorUtility.SetDirty(collider);
    }
}
#endif