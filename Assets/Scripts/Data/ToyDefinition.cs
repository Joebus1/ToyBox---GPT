using UnityEngine;

[CreateAssetMenu(menuName = "Toy Box/Toy Definition")]
public class ToyDefinition : ScriptableObject
{
    public string displayName;
    public GameObject prefab;
    public Sprite icon;
    public ToyProperties properties;
}
