#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ToySetupToolEditor : EditorWindow
{
    private PhysicsMaterial physicsMaterial;
    private ToyProperties defaultToyProperties;
    private bool overwriteExisting = false;
    private bool freezeZ = true;

    [MenuItem("Tools/Setup Selected Toys")]
    public static void ShowWindow()
    {
        GetWindow<ToySetupToolEditor>("Toy Prefab Setup");
    }

    void OnGUI()
    {
        GUILayout.Label("Toy Setup Options", EditorStyles.boldLabel);

        physicsMaterial = (PhysicsMaterial)EditorGUILayout.ObjectField("Physics Material", physicsMaterial, typeof(PhysicsMaterial), false);
        defaultToyProperties = (ToyProperties)EditorGUILayout.ObjectField("Default ToyProperties", defaultToyProperties, typeof(ToyProperties), false);
        overwriteExisting = EditorGUILayout.Toggle("Overwrite Existing Components", overwriteExisting);
        freezeZ = EditorGUILayout.Toggle("Freeze Position Z", freezeZ);

        if (GUILayout.Button("Apply to Selected Prefabs"))
        {
            ApplySetupToSelectedPrefabs();
        }
    }

    private void ApplySetupToSelectedPrefabs()
    {
        var database = GetOrCreateToyDatabase();

        foreach (GameObject obj in Selection.gameObjects)
        {
            if (!PrefabUtility.IsPartOfAnyPrefab(obj))
            {
                Debug.LogWarning($"Skipping {obj.name} � not a prefab.");
                continue;
            }

            string path = AssetDatabase.GetAssetPath(obj);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at {path}");
                continue;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(instance, "Setup Toy Prefab");

            // Rigidbody
            var rb = instance.GetComponent<Rigidbody>();
            if (rb == null || overwriteExisting)
            {
                if (rb == null) rb = instance.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.angularDamping = 0.05f;
                rb.constraints = freezeZ ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.None;
            }

            // Collider
            var collider = instance.GetComponent<Collider>();
            if (collider == null || overwriteExisting)
            {
                if (collider == null) collider = instance.AddComponent<BoxCollider>();
                collider.material = physicsMaterial;
            }

            // Toy tag
            instance.tag = "Toy";

            // ToyObject
            var toyObject = instance.GetComponent<ToyObject>();
            if (toyObject == null || overwriteExisting)
            {
                if (toyObject == null) toyObject = instance.AddComponent<ToyObject>();
                var matched = GetMatchingToyProperties(instance.name);
                toyObject.properties = matched != null ? matched : defaultToyProperties;

            }

            // ToyDraggable
            var draggable = instance.GetComponent<ToyDraggable>();
            if (draggable == null || overwriteExisting)
            {
                if (draggable == null)
                {
                    draggable = instance.AddComponent<ToyDraggable>();
                }
            }

            // Save prefab back
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            DestroyImmediate(instance);

            // ToyDefinition
            var def = CreateToyDefinitionIfNeeded(prefab, prefab.name);
            if (def != null && !database.Toys.Contains(def))
            {
                database.Toys.Add(def);
                EditorUtility.SetDirty(database);
            }


            Debug.Log($"? Finished setup for {prefab.name}");
        }

        AssetDatabase.SaveAssets();
    }

    private ToyProperties GetMatchingToyProperties(string prefabName)
    {
        string[] guids = AssetDatabase.FindAssets($"{prefabName} t:ToyProperties");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<ToyProperties>(path);
        }
        return null;
    }

    private ToyDefinition CreateToyDefinitionIfNeeded(GameObject prefab, string name)
    {
        string path = $"Assets/ScriptableObjects/Definitions/{name}.asset";

        if (!Directory.Exists("Assets/ScriptableObjects/Definitions"))
        {
            Directory.CreateDirectory("Assets/ScriptableObjects/Definitions");
        }

        ToyDefinition existing = AssetDatabase.LoadAssetAtPath<ToyDefinition>(path);
        if (existing != null) return existing;

        ToyDefinition def = ScriptableObject.CreateInstance<ToyDefinition>();
        def.prefab = prefab;

        AssetDatabase.CreateAsset(def, path);
        AssetDatabase.SaveAssets();
        return def;
    }

    private ToyDatabase GetOrCreateToyDatabase()
    {
        string[] guids = AssetDatabase.FindAssets("t:ToyDatabase");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<ToyDatabase>(path);
        }

        string folderPath = "Assets/ScriptableObjects";
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        ToyDatabase db = ScriptableObject.CreateInstance<ToyDatabase>();
        AssetDatabase.CreateAsset(db, $"{folderPath}/ToyDatabase.asset");
        AssetDatabase.SaveAssets();
        return db;
    }
}
#endif
