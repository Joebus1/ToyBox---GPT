using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Toy Box/Toy Database")]
public class ToyDatabase : ScriptableObject
{
    [SerializeField]
    private List<ToyDefinition> toyList = new List<ToyDefinition>();

    // Exposed as read-only for other scripts
    public List<ToyDefinition> Toys => toyList;
}
