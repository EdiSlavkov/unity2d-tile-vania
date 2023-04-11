using UnityEngine;
using Assets.Scripts;

public class Collectables : MonoBehaviour
{
    [SerializeField] private float collectableValue;
    private PlayerStats playerStats;
    private AudioSource audioSource;
    private bool isCollected = false;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player) && !isCollected)
        {
            playerStats.AddMoney(collectableValue);
            isCollected = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
