using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private ScreenBase[] screens;

    void Awake()
    {
        App.screenManager = this;
        screens = GetComponentsInChildren<ScreenBase>(true);
    }

    public bool IsScreenDisplayed<T>()
    {
        foreach (ScreenBase screen in screens)
        {
            if (screen.GetType() == typeof(T))
            {
                return screen.gameObject.activeInHierarchy;
            }
        }
        return false;
    }

    public void Show<T>()
    {
        foreach (ScreenBase screen in screens)
        {
            if (screen.GetType() == typeof(T))
            {
                screen.Show();
            }
        }
    }

    public void Hide<T>()
    {
        foreach (ScreenBase screen in screens)
        {
            if (screen.GetType() == typeof(T))
            {
                screen.Hide();
            }
        }
    }
}
