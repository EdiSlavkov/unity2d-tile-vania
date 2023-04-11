using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private float speed = 200f;
    [SerializeField] private float maxRange = 5f;
    private Rigidbody2D myRigidBody;
    private float xDirection;
    private float xPosition;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        xPosition = transform.position.x;
        transform.localScale = new Vector3(xDirection, transform.localScale.y, 0f);
    }

    private void Update()
    {
        myRigidBody.velocity = new Vector2(xDirection * speed, 0f);
        DestroyAfterMaxRange();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public void ChangeXScale(float value)
    {
        xDirection = value;
    }

    private void DestroyAfterMaxRange()
    {
        if (xPosition + maxRange < transform.position.x
            || xPosition - maxRange > transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
