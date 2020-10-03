using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class WireCabinet : MonoBehaviour
{
    public Animator animator;
    public GameObject Bridge;

    private bool isOpen;
    private bool isFixed;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsOpen", false);
        animator.SetBool("IsFixed", false);
        isOpen = false;
        isFixed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeItem(string itemName)
    {
        if (itemName.Equals("Drill") && !isOpen)
        {
            animator.SetBool("IsOpen", true);
            isOpen = true;
        }
        else if (itemName.Equals("Wires") && !isFixed)
        {
            animator.SetBool("IsOpen", false);
            isOpen = false;
            animator.SetBool("IsFixed", true);
            isFixed = true;
            Bridge.GetComponent<Bridge>().Extend();
        }
    }
}
