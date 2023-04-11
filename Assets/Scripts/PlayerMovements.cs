using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float initialJumpSpeed = 10f;
    [SerializeField] private float initialMoveSpeed = 10f;
    [SerializeField] private float moveSpeedInWater = 3f;
    [SerializeField] private float jumpSpeedInWater = 15f;
    [SerializeField] private float deathJump = 20f;
    [SerializeField] private float playerGravity = 6f;
    [SerializeField] private float maxNegativeVelocityY = -30f;
    [SerializeField] private GameObject orb;
    [SerializeField] private Transform weapon;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip runningSFX;
    [SerializeField] private AudioClip jumpSFX;
    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerBoxCollider;
    private Vector2 moveInput;
    private Animator animator;
    private PlayerStats playerStats;
    private GameManager gameManager;
    private AudioSource audioSource;
    private float moveSpeed;
    private float jumpSpeed;
    private bool isAlive = true;
    private bool isPlayerFallFromAHeight;
    private bool shouldPlayRunningSFX = true;
    private bool canClimb;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jumpSpeed = initialJumpSpeed;
        moveSpeed = initialMoveSpeed;
        playerStats = FindObjectOfType<PlayerStats>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.gravityScale = playerGravity;
    }

    private void Update()
    {
        CheckPlayerForLongFall();
    }

    private void OnMove(InputValue value)
    {
        if (!isAlive || gameManager.IsPaused)
        {
            return;
        }
        moveInput = value.Get<Vector2>();
        if (canClimb)
        {
            playerRigidBody.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
            animator.SetBool(Animations.Jumping, false);
        }
        FlipThePlayer();
        Run();
    }

    public void Run()
    {
        if (!isAlive || gameManager.IsPaused)
        {
            return;
        }
        bool isMoving = moveInput.x != 0f;
        if (isMoving)
        {
            playerRigidBody.velocity = new Vector2(moveInput.x * moveSpeed, playerRigidBody.velocity.y);
            animator.SetBool(Animations.Running, true);
            if (shouldPlayRunningSFX)
            {
                StartCoroutine(PlayRunningSFX());
            }
        }
        else
        {
            playerRigidBody.velocity = new Vector2(moveInput.x, playerRigidBody.velocity.y);
            animator.SetBool(Animations.Running, false);
        }
    }

    private IEnumerator PlayRunningSFX()
    {
        audioSource.PlayOneShot(runningSFX);
        shouldPlayRunningSFX = false;
        yield return new WaitForSecondsRealtime(runningSFX.length);
        shouldPlayRunningSFX = true;
    }

    public void FlipThePlayer()
    {
        if (
            (moveInput.x > 0
            && transform.localScale.x < 0)
            || (moveInput.x < 0
            && transform.localScale.x > 0)
        )
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0f);
        }
    }

    private void OnJump()
    {
        if (!isAlive || gameManager.IsPaused)
        {
            return;
        }
        Jump();
    }

    private void Jump()
    {
        if (playerBoxCollider.IsTouchingLayers(LayerMask.GetMask(Layers.Ground))
            && !playerBoxCollider.IsTouchingLayers(LayerMask.GetMask(Layers.Lader)))
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
            animator.SetBool(Animations.Jumping, true);
            audioSource.PlayOneShot(jumpSFX);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerBoxCollider.IsTouchingLayers(LayerMask.GetMask(Layers.Lader)))
        {
            canClimb = false;
            playerRigidBody.gravityScale = playerGravity;
        }
        if (!playerRigidBody.IsTouchingLayers(LayerMask.GetMask(Layers.Water)))
        {
            moveSpeed = initialMoveSpeed;
            jumpSpeed = initialJumpSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerBoxCollider.IsTouchingLayers(LayerMask.GetMask(Layers.Lader)))
        {
            canClimb = true;
            playerRigidBody.gravityScale = 0;
        }
        if (playerRigidBody.IsTouchingLayers(LayerMask.GetMask(Layers.Water)))
        {
            moveSpeed = moveSpeedInWater;
            jumpSpeed = jumpSpeedInWater;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Ground))
        {
            animator.SetBool(Animations.Jumping, false);
        }
        if (
            collision.gameObject.CompareTag(Tags.Kill)
            || (collision.gameObject.CompareTag(Tags.Ground)
            && isPlayerFallFromAHeight)
        )
        {
            HandlePlayerDeath();
        }
        if (collision.gameObject.CompareTag(Tags.Bounce))
        {
            animator.SetBool(Animations.Jumping, true);
            audioSource.PlayOneShot(jumpSFX);
        }
    }

    private void HandlePlayerDeath()
    {
        if (!isAlive)
        {
            return;
        }
        audioSource.PlayOneShot(deathSFX);
        isAlive = false;
        animator.SetBool(Animations.Alive, false);
        if (playerStats.GetPlayerLifes() <= 1)
        {
            StartCoroutine(gameManager.DelayingRestart());
        }
        else
        {
            playerStats.TakeLife();
            playerRigidBody.velocity = new Vector2(0f, deathJump);
            StartCoroutine(gameManager.ReloadLevel());
        }
    }

    private void OnFire()
    {
        if (!isAlive || gameManager.IsPaused)
        {
            return;
        }
        ThrowFireBall();
    }

    private void ThrowFireBall()
    {
        audioSource.PlayOneShot(attackSFX, attackSFX.length);
        animator.SetTrigger(Animations.Attack);
        GameObject orbInstance = Instantiate(orb, weapon.position, transform.rotation);
        orbInstance.GetComponent<Orb>().ChangeXScale(transform.localScale.x);
    }

    private void CheckPlayerForLongFall()
    {
        if (playerRigidBody.velocity.y < maxNegativeVelocityY)
        {
            isPlayerFallFromAHeight = true;
        }
    }
}