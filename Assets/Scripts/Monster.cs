using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Monster : MonoBehaviour
{
    [SerializeField] private GameObject orb;
    [SerializeField] private Transform weapon;
    [SerializeField] private float attackRange = 5;
    [SerializeField] private AudioClip attackSFX;
    private PlayerMovements player;
    private AudioSource audioSource;
    private Enemies enemiesScript;
    private float distance;
    private bool shouldAttack = true;
    private Animator animator;

    private void OnLevelWasLoaded()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovements>();
        }
    }

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovements>();
        audioSource = GetComponent<AudioSource>();
        enemiesScript = GetComponent<Enemies>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        SetDistanceWithPlayer();
        if (distance < attackRange)
        {
            enemiesScript.StopMoving();
            TurnToPlayerSide();
            ThrowFireBall();
        }
        else
        {
            enemiesScript.StartMoving();
            enemiesScript.RestoreScaleX();
        }
    }

    private void TurnToPlayerSide()
    {
        if (distance < attackRange)
        {
            if (
                (player.transform.position.x < transform.position.x
                && transform.localScale.x > 0)
                || (player.transform.position.x > transform.position.x
                && transform.localScale.x < 0)
            )
            {
                enemiesScript.Flip();

            }
        }
    }

    private void ThrowFireBall()
    {
        if (shouldAttack)
        {
            animator.SetTrigger(Animations.Attack);
            audioSource.PlayOneShot(attackSFX);
            GameObject fireBallInstance = Instantiate(orb, weapon.position, transform.rotation);
            fireBallInstance.GetComponent<Orb>().ChangeXScale(transform.localScale.x);
            shouldAttack = false;
            StartCoroutine(WaitTillNextAttack());
        }
    }

    private IEnumerator WaitTillNextAttack()
    {
        int attackRateMin = 2;
        int attackRateMax = 5;
        int randomTime = Random.Range(attackRateMin, attackRateMax);
        yield return new WaitForSecondsRealtime(randomTime);
        shouldAttack = true;
    }

    private void SetDistanceWithPlayer()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
    }
}