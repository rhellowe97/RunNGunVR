using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlugSocketInteractor : SlottableSocketInteractor
{
    public delegate void ReloadDelegate();
    public event ReloadDelegate OnReload;

    protected override void Start()
    {
        base.Start();

        selectEntered.AddListener( OnLoad );
    }

    private void OnLoad( SelectEnterEventArgs args )
    {
        Magazine slug = args.interactable.GetComponent<Magazine>();

        ObjectPool.Instance.ReturnToPool( slug );

        OnReload?.Invoke();
    }
}
