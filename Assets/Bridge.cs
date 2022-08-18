using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{

    Animator animator;
    public bool startRaised;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!startRaised)
            Lower();
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
