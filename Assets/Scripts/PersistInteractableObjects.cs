using UnityEngine;

public class PersistInteractableObjects : MonoBehaviour
{
    private PersistInteractableObjects [] persistInteractableObjects;

    private void Awake()
    {
        SaveFirstInteractableObjects();
    }

    private void SaveFirstInteractableObjects()
    {
        persistInteractableObjects = FindObjectsOfType<PersistInteractableObjects>();
        if (persistInteractableObjects.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DestroyCurrentInteractables()
    {
        Destroy(gameObject);
    }
}