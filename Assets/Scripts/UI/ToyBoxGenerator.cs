using UnityEngine;
using UnityEngine.UI;

public class ToyBoxGenerator : MonoBehaviour
{
    public ToyDatabase database;
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject); // Clear old
        }

        foreach (var toy in database.toys)
        {
            GameObject newBtn = Instantiate(buttonPrefab, buttonContainer);
            newBtn.name = toy.displayName;

            Image img = newBtn.GetComponent<Image>();
            if (img != null && toy.icon != null)
                img.sprite = toy.icon;

            ToyBoxItem tbi = newBtn.GetComponent<ToyBoxItem>();
            if (tbi != null)
                tbi.prefab = toy.prefab;
        }
    }
}
