using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToyDatabase", menuName = "ToyBox/Toy Database")]
public class ToyDatabase : ScriptableObject
{
    [SerializeField] private List<ToyDefinition> toys = new();

    // Public property to access/edit the list in scripts or custom windows
    public List<ToyDefinition> Toys
    {
        get => toys;
        set => toys = value;
    }
}
