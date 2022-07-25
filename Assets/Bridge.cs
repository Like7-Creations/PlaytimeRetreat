using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Raise()
    {
        animator.SetTrigger("Raise");
    }

    public void Lower()
    {
        animator.SetTrigger("Lower");
    }
}
