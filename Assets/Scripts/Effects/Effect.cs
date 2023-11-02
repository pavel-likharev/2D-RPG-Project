using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour 
{
    public virtual void ExecuteEffect(Transform target)
    {
        Debug.Log("effect executed");
    }
}
