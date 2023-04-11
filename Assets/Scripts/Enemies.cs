using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemies : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float lives = 3;
    [SerializeField] private Slider healthBar;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private bool isBoss;
    [SerializeField] private AnimationClip idlingAnimation;
    private Rigidbody2D enemyRigidBody;
    private Animator animator;
    private AudioSource audioSource;
    private bool shouldMove = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        healthBar.maxValue = lives;
        enemyRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    public void RestoreScaleX()
    {
        if (
            (moveSpeed < 0 
            && transform.localScale.x > 0)
            || (moveSpeed > 0
            && transform.localScale.x < 0)
        )
        {
            Flip();
            ChangeXVelocityDirection();
        }
    }

    private void Move()
    {
        if (shouldMove)
        {
            enemyRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            enemyRigidBody.velocity = new Vector2(0f, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(Tags.Player))
        {
            ChangeXVelocityDirection();
            Flip();
            StartCoroutine(StartIdling());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Orb))
        {
            TakeLife();
        }
        if (lives < 1)
        {
            Die();
        }
    }

    private void TakeLife()
    {
        lives--;
        healthBar.value += 1;
    }

    private void Die()
    {
        if (isBoss)
        {
            FindObjectOfType<GameManager>().OpenTheDoor();
        }
        audioSource.PlayOneShot(deathSFX);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find(Constants.EnemyHealthBar).gameObject.SetActive(false);
        Destroy(gameObject, deathSFX.length);
    }

    private IEnumerator StartIdling()
    {
        StopMoving();
        yield return new WaitForSecondsRealtime(idlingAnimation.length);
        StartMoving();
    }

    public void StopMoving()
    {
        shouldMove = false;
        animator.SetBool(Animations.Moving, false);
    }

    public void StartMoving()
    {
        shouldMove = true;
        animator.SetBool(Animations.Moving, true);
    }

    public void ChangeXVelocityDirection()
    {
        moveSpeed = -moveSpeed;
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}