using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private Collider standingCollider;
    [SerializeField] private Collider proneCollider;

    private bool isMoving = false;
    private bool isProne = false;

    private Animator animator;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            isProne = !isProne;
            animator.SetBool("IsProne", isProne);

            standingCollider.enabled = !isProne;
            proneCollider.enabled = isProne;
        }

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var move = new Vector3(x, 0.0f, z) * moveSpeed * (isProne ? 0.4f : 1.0f);

        rigidbody.velocity = transform.TransformVector(move);

        isMoving = Mathf.Abs(x) > 0.05f || Mathf.Abs(z) > 0.05f;
        animator.SetBool("IsMoving", isMoving);
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
