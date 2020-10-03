using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class Bridge : MonoBehaviour
{
    public Animator animator;

    private BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsExtended", false);
        collider = gameObject.GetComponent<BoxCollider2D>();
        collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Extend()
    {
        animator.SetBool("IsExtended", true);
        collider.enabled = false;
    }

    public void Retract()
    {
        animator.SetBool("IsExtended", false);
        collider.enabled = true;
    }
}
