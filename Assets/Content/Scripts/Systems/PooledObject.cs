using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    [SerializeField]
    private PooledType pooledType;
    public PooledType PooledType => pooledType;

    public abstract void Init();

    public abstract void ResetObject();

    public virtual void Returned()
    {
        gameObject.SetActive( false );
    }
}
