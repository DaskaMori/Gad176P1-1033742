using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour
{
    protected virtual void Start()
    {
        Spawn();
    }

    protected abstract void Spawn();
}
