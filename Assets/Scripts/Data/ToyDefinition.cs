using UnityEngine;

[CreateAssetMenu(fileName = "NewToyDefinition", menuName = "ToyBox/Toy Definition")]
public class ToyDefinition : ScriptableObject
{
    public string displayName;
    public GameObject prefab;
    public Sprite icon;
    public ToyProperties properties;
}
