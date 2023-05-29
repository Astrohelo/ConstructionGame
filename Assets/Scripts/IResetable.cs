using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResetable
{
    public virtual void ResetPosition() { Debug.Log("Need implementation for ResetPosition"); }
}
