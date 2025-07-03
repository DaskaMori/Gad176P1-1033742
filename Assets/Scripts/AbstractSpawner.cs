using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour
{
    protected virtual void Start()
    {
        Spawn();
    }

    protected abstract void Spawn();
}
