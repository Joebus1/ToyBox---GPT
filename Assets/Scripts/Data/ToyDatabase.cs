using UnityEngine;

[CreateAssetMenu(fileName = "Toy Database", menuName = "ToyBox/Toy Database")]
public class ToyDatabase : ScriptableObject
{
    public ToyDefinition[] toys;
}
