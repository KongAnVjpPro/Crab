using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMonobehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        this.LoadComponents();
    }
    protected virtual void LoadComponents()
    {

    }
    protected virtual void Reset()
    {
        this.LoadComponents();
        this.ResetValue();
    }
    protected virtual void ResetValue()
    {

    }
}
