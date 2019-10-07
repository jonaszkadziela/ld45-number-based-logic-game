﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float initialSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private float speed;

    private Vector3 previousPosition;
    private float distanceMoved = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        previousPosition = transform.position;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        speed = initialSpeed;

        if (PlayerController.Instance.heldObject)
        {
            if (PlayerController.Instance.heldObject.GetComponent<Stone>())
            {
                float stoneMass = PlayerController.Instance.heldObject.GetComponent<Rigidbody2D>().mass / 10f;
                float massFactor = stoneMass / (rb.mass + stoneMass);

                speed *= 1 - massFactor;
            }
        }

        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, previousPosition) > 0.01f)
        {
            distanceMoved += Vector3.Distance(transform.position, previousPosition);
            PlayerController.Instance.DetermineCurrentEnergy(distanceMoved);

            previousPosition = transform.position;
        }
    }
}
