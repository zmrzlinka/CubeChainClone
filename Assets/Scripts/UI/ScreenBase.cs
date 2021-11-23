using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    // TODO (maybe): add Show method with parameter(s)

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
