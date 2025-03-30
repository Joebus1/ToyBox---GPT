using UnityEditor;
using UnityEngine;

public class ToyEditor : EditorWindow
{
    private ToyDatabase toyDatabase;

    [MenuItem("ToyBox/Toy Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToyEditor>("Toy Editor");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        toyDatabase = (ToyDatabase)EditorGUILayout.ObjectField("Toy Database", toyDatabase, typeof(ToyDatabase), false);

        if (toyDatabase == null)
        {
            EditorGUILayout.HelpBox("Please assign a ToyDatabase asset.", MessageType.Info);
            return;
        }

        GUILayout.Space(10);

        foreach (var toy in toyDatabase.Toys)
        {
            if (toy == null || toy.properties == null)
                continue;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(toy.displayName, EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            toy.properties.mass = EditorGUILayout.FloatField("Mass", toy.properties.mass);
            toy.properties.bounciness = EditorGUILayout.Slider("Bounciness", toy.properties.bounciness, 0f, 1f);
            toy.properties.friction = EditorGUILayout.Slider("Friction", toy.properties.friction, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(toy.properties);
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }
    }
}
