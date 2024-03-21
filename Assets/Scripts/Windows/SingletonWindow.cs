using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingletonWindow<T> : Window where T : SingletonWindow<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = (T)this;
        else
            Destroy(transform.parent == null ? gameObject : transform.parent.gameObject);
    }

    public override void Show()
    {
        base.Show();

        GameController.Instance.WindowsStack.Push((T)this);
    }

    public override void Hide()
    {
        base.Hide();

        try
        {
            if (GameController.Instance.WindowsStack.Contains((T)this))
                GameController.Instance.WindowsStack.Pop();
        }
        catch (NullReferenceException)
        {

        }
    }
}
