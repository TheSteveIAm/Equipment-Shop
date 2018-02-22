using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    protected Canvas canvas;
    protected bool interactable;

    protected virtual void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public virtual void EnableUI(bool enabled)
    {
        canvas.enabled = enabled;
        interactable = enabled;
    }
}
