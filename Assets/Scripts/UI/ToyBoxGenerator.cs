using System.Collections.Generic;
using UnityEngine;
using ToyDefinitions; // Ensure this matches your folder/namespace if used

[CreateAssetMenu(menuName = "Toy Box/Toy Database")]
public class ToyDatabase : ScriptableObject
{
    [SerializeField]
    private List<ToyDefinition> toys = new List<ToyDefinition>();

    // Public getter allows read-only access from other scripts
    public List<ToyDefinition> Toys => toys;
}
