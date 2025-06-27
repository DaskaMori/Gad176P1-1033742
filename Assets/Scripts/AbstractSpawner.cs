using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    protected abstract void Spawn();
}
