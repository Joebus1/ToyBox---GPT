using UnityEngine;
using UnityEngine.UI;

public class ToyBoxGenerator : MonoBehaviour
{
    public ToyDatabase database;
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    private void Start()
    {
        GenerateToyButtons();
    }

    public void GenerateToyButtons()
    {
        if (database == null || buttonPrefab == null || buttonContainer == null)
        {
            Debug.LogWarning("ToyBoxGenerator is missing references.");
            return;
        }

        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var toy in database.Toys)
        {
            if (toy == null) continue;

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
