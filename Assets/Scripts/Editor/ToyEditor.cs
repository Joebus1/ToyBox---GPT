using UnityEditor;
using UnityEngine;

public class ToyEditor : EditorWindow
{
    private ToyDatabase database;
    private Vector2 scrollPos;

    [MenuItem("ToyBox/Toy Properties Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToyEditor>("Toy Editor");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        database = (ToyDatabase)EditorGUILayout.ObjectField("Toy Database", database, typeof(ToyDatabase), false);

        if (database == null)
        {
            EditorGUILayout.HelpBox("Assign a ToyDatabase asset to begin.", MessageType.Info);
            return;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var toy in database.Toys)
        {
            if (toy == null || toy.properties == null)
                continue;

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField(toy.displayName, EditorStyles.boldLabel);

            toy.properties.mass = EditorGUILayout.FloatField("Mass", toy.properties.mass);
            toy.properties.bounciness = EditorGUILayout.Slider("Bounciness", toy.properties.bounciness, 0f, 1f);
            toy.properties.friction = EditorGUILayout.Slider("Friction", toy.properties.friction, 0f, 1f);
        }

        EditorGUILayout.EndScrollView();
    }
}
