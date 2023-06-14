using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    [SerializeField]
    private float delay = 0f;

    private Tween toggleTween;

    private Vector3 startPosition;


    [SerializeField]
    private Vector3 openVector = Vector3.zero;

    [SerializeField]
    private float translateSpeed = 0;

    [SerializeField]
    private Ease translateEase = Ease.InOutSine;

    private void Start()
    {

        startPosition = transform.position;

        StartCoroutine( OscillateCo() );

    }

    private IEnumerator OscillateCo()
    {
        bool up = true;

        WaitForSeconds tweenDelay = new WaitForSeconds( delay );

        while ( true )
        {
            toggleTween = transform.DOMove( startPosition + ( up ? openVector : Vector3.zero ), translateSpeed ).SetEase( translateEase ).OnComplete( () => toggleTween = null ).SetUpdate( UpdateType.Fixed );

            yield return toggleTween.WaitForCompletion();

            yield return tweenDelay;

            up = !up;
        }
    }
}
