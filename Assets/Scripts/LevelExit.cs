using UnityEngine;
using Assets.Scripts;

public class LevelExit : MonoBehaviour
{
    private bool isOpen = false;
    private GameManager gameManager;
    private SpriteRenderer doorSpriteRenderer;

    private void Start()
    {
        doorSpriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        doorSpriteRenderer.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.CompareTag(Tags.Player))
        {
            gameManager.LoadNextLevel();
        }
    }

    public void Open()
    {
        isOpen = true;
        doorSpriteRenderer.color = Color.white;
    }
}