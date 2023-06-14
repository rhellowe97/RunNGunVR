namespace RunNGunVR.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class WeaponHologram : MonoBehaviour
    {
        [SerializeField]
        protected Transform weapon;

        private Vector3 startingPosition;

        private Vector3 rollingPosition;

        [SerializeField]
        protected float maxRotationSpeed = 540f;

        [SerializeField]
        protected float rotationDecay = 5f;

        [SerializeField]
        protected float handInfluence = 1f;

        [SerializeField]
        protected float bobSpeed = 1f;

        [SerializeField]
        protected float bobAmplitude = 0.1f;

        protected float currentRotationSpeed = 0f;

        protected HoloHand hand;

        protected Vector3 lastHandPos;

        private void Awake()
        {
            rollingPosition = startingPosition = weapon.position;
        }

        private void Update()
        {
            if ( weapon )
            {
                rollingPosition.y = bobAmplitude * Mathf.Sin( Time.time * bobSpeed ) + startingPosition.y;

                weapon.position = rollingPosition;

                if ( hand )
                {
                    currentRotationSpeed += transform.InverseTransformVector( hand.transform.position - lastHandPos ).x * handInfluence;

                    Debug.Log( currentRotationSpeed );

                    lastHandPos = hand.transform.position;
                }

                currentRotationSpeed = Mathf.Lerp( currentRotationSpeed, 0, rotationDecay * Time.deltaTime );

                weapon.Rotate( Vector3.up * currentRotationSpeed * Time.deltaTime );
            }
        }

        private void OnTriggerEnter( Collider other )
        {
            HoloHand possibleHand = other.GetComponent<HoloHand>();

            if ( possibleHand )
            {
                hand = possibleHand;

                lastHandPos = hand.transform.position;
            }
        }

        private void OnTriggerExit( Collider other )
        {
            HoloHand possibleHand = other.GetComponent<HoloHand>();

            if ( possibleHand == hand )
            {
                hand = null;
            }
        }

    }
}
