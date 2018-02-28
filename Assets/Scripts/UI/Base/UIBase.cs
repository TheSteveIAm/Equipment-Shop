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
        EnsureComponents();
        canvas.enabled = false;
    }

    public void EnsureComponents()
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }
        Canvas.ForceUpdateCanvases();
    }

    public virtual void EnableUI(bool enabled)
    {
        EnsureComponents();

        canvas.enabled = enabled;
        interactable = enabled;
    }
}
