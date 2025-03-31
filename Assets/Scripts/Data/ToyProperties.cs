using UnityEngine;

[CreateAssetMenu(fileName = "ToyProperties", menuName = "ToyBox/Toy Properties")]
public class ToyProperties : ScriptableObject
{
    public float mass = 1f;
    public float bounciness = 0.5f;
    public float friction = 0.1f;
    // Add additional properties as needed.
}
