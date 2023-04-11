using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private LevelExit door;
    [SerializeField] private float loadTime = 2;
    public bool IsPaused;

    public IEnumerator DelayingRestart()
    {
        yield return new WaitForSecondsRealtime(loadTime);
        RestartTheGame();
    }

    public void LoadNextLevel()
    {
        FindObjectOfType<PersistInteractableObjects>().DestroyCurrentInteractables();
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
    }

    private void Pause()
    {
        IsPaused = true;
        menu.SetActive(true);
        Time.timeScale = 0f;
    } 

    private void Resume()
    {
        IsPaused = false;
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public IEnumerator ReloadLevel()
    {
        yield return new WaitForSecondsRealtime(loadTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartTheGame()
    {
        FindObjectOfType<PersistInteractableObjects>().DestroyCurrentInteractables();
        FindObjectOfType<SceneState>().DestroyCurrentPersistScene();
        SceneManager.LoadScene(0);
    }

    public void EndScreenGameRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenTheDoor()
    {
        door.Open();
    }
}