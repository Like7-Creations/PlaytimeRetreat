using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public string buttonType;

    public UnityEvent buttonPressed;

    private void OnMouseDown()
    {
        Debug.Log($"{buttonType} Pressed");
        buttonPressed.Invoke();
    }
}
