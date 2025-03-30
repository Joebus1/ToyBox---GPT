using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Toy Box/Toy Database")]
public class ToyDatabase : ScriptableObject
{
    [SerializeField]
    private List<ToyDefinition> toyList = new List<ToyDefinition>();

    public List<ToyDefinition> Toys => toyList;
}
