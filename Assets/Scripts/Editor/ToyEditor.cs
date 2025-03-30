using UnityEditor;
using UnityEngine;

public class ToyEditor : EditorWindow
{
    private ToyDatabase database;
    private Vector2 scrollPos;

    [MenuItem("ToyBox/Toy Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToyEditor>("Toy Editor");
    }

    private void OnEnable()
    {
        if (database == null)
        {
            // Auto-load from Resources folder, or drag manually if needed
            database = Resources.Load<ToyDatabase>("ToyDatabase");
        }
    }

    private void OnGUI()
    {
        // Allow manual assignment in case Resources.Load fails
        database = (ToyDatabase)EditorGUILayout.ObjectField("Toy Database", database, typeof(ToyDatabase), false);

        if (database == null)
        {
            EditorGUILayout.HelpBox("Assign a ToyDatabase asset.", MessageType.Warning);
            return;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);  // << SCROLL BEGIN

        foreach (var toy in database.Toys)
        {
            if (toy == null || toy.properties == null)
                continue;

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField(toy.name, EditorStyles.boldLabel);

            toy.properties.mass = EditorGUILayout.FloatField("Mass", toy.properties.mass);
            toy.properties.bounciness = EditorGUILayout.FloatField("Bounciness", toy.properties.bounciness);
            toy.properties.friction = EditorGUILayout.FloatField("Friction", toy.properties.friction);
        }

        EditorGUILayout.EndScrollView();  // << SCROLL END
    }
}
