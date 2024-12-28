using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputController
{
    protected bool enabled = false;

    public bool Enabled { get { return enabled; } set { SetEnabled(value); } }

    public BaseInputController() {
        Init();
        SetActions();
    }

    private void SetEnabled(bool value)
    {
        enabled = value;

        if (enabled) OnEnable();
        else OnDisable();
    }

    protected virtual void Init() { }
    protected virtual void SetActions() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }


}
