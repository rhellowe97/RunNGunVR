using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    [SerializeField]
    protected Transform vrTarget;

    [SerializeField]
    protected Transform rigTarget;

    [SerializeField]
    protected Vector3 trackingPositionOffset;

    [SerializeField]
    protected Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint( trackingPositionOffset );
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler( trackingRotationOffset );
    }
}

public class VRBodyRig : MonoBehaviour
{
    [SerializeField]
    protected Transform headConstraint;

    [SerializeField]
    protected float smoothTurn = 4f;

    [SerializeField]
    protected VRMap head;

    [SerializeField]
    protected VRMap leftHand;

    [SerializeField]
    protected VRMap rightHand;

    private Vector3 headBodyOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp( transform.forward, Vector3.ProjectOnPlane( headConstraint.up, Vector3.up ).normalized, smoothTurn * Time.deltaTime );

        head.Map();

        leftHand.Map();

        rightHand.Map();
    }
}
