using UnityEngine;

public class ClearAllToys : MonoBehaviour
{
    public void ClearAll()
    {
        ToyObject[] allToys = Object.FindObjectsByType<ToyObject>(FindObjectsSortMode.None);
        foreach (ToyObject toy in allToys)
        {
            Destroy(toy.gameObject);
        }

        Debug.Log("All toys cleared!");

    }
}
