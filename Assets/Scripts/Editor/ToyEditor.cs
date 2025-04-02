using UnityEngine;
using UnityEditor;

public class ToyEditor : EditorWindow
{
    private ToyDatabase toyDatabase;
    private Vector2 scrollPos;

    [MenuItem("ToyBox/Toy Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToyEditor>("Toy Editor");
    }

    private void OnEnable()
    {
        string path = EditorPrefs.GetString("ToyDatabasePath", "");
        if (!string.IsNullOrEmpty(path))
        {
            toyDatabase = AssetDatabase.LoadAssetAtPath<ToyDatabase>(path);
        }
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Select a ToyProperties asset", EditorStyles.boldLabel);
        toyDatabase = (ToyDatabase)EditorGUILayout.ObjectField("Toy Database", toyDatabase, typeof(ToyDatabase), false);

        if (toyDatabase == null)
        {
            EditorGUILayout.HelpBox("Assign the ToyDatabase asset to begin editing.", MessageType.Info);
            return;
        }

        // Save database path for next session
        string path = AssetDatabase.GetAssetPath(toyDatabase);
        EditorPrefs.SetString("ToyDatabasePath", path);

        GUILayout.Space(10);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < toyDatabase.Toys.Count; i++)
        {
            ToyDefinition toy = toyDatabase.Toys[i];

            if (toy == null)
            {
                EditorGUILayout.LabelField($"Element {i} is null.");
                continue;
            }

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label(toy.displayName, EditorStyles.boldLabel);
            toy.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", toy.prefab, typeof(GameObject), false);
            toy.icon = (Sprite)EditorGUILayout.ObjectField("Icon", toy.icon, typeof(Sprite), false);
            toy.properties = (ToyProperties)EditorGUILayout.ObjectField("Properties", toy.properties, typeof(ToyProperties), false);

            if (toy.properties != null)
            {
                EditorGUILayout.LabelField("Mass", toy.properties.mass.ToString("F3"));
                toy.properties.mass = EditorGUILayout.Slider(toy.properties.mass, 0.01f, 2f);

                EditorGUILayout.LabelField("Bounciness", toy.properties.bounciness.ToString("F2"));
                toy.properties.bounciness = EditorGUILayout.Slider(toy.properties.bounciness, 0f, 1f);

                EditorGUILayout.LabelField("Friction", toy.properties.friction.ToString("F2"));
                toy.properties.friction = EditorGUILayout.Slider(toy.properties.friction, 0f, 1f);

                // Ensure the properties asset is marked dirty
                EditorUtility.SetDirty(toy.properties);
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Save Changes"))
        {
            EditorUtility.SetDirty(toyDatabase);
            AssetDatabase.SaveAssets();
            Debug.Log("ToyDatabase and Properties saved.");
        }
    }
}
