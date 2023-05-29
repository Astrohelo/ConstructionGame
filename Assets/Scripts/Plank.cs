using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Plank : SelectableObject, IResetable
{
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private NetworkObject networkObject;
    public void Start() {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        networkObject = GetComponent<NetworkObject>();
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    public override void PlaceWithNail() {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Debug.Log(rb.constraints);
        //transform.parent = null;
        ///networkObject.TryRemoveParent();
        //transform.position = placeToBePlaced.position;
    }
    public void ResetPosition() {
        //Collided with the "lava"
        transform.position = startingPosition;
        transform.rotation = startingRotation;
    }

}
