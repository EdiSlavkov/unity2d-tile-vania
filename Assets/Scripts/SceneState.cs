using UnityEngine;

public class SceneState : MonoBehaviour
{
    private SceneState[] persistScene;

    private void Awake()
    {
        SaveFirstStateOnly();
    }

    private void SaveFirstStateOnly()
    {
        persistScene = FindObjectsOfType<SceneState>();
        if (persistScene.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DestroyCurrentPersistScene()
    {
        Destroy(gameObject);
    }
}