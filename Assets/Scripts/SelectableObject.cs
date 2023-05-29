using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SelectableObject : NetworkBehaviour
{
    protected Rigidbody rb;
    protected BoxCollider bc;
    [SerializeField]private Transform placePoint;
    [SerializeField]private MeshRenderer selectable;

    public virtual void Interact(Player player) { DestroyObjectServerRpc(); }
    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc() {
        DestroyObjectClientRpc();
    }
    [ClientRpc]
    private void DestroyObjectClientRpc() {
        Destroy(gameObject);
    }

    public void ShowSelected() {
        selectable.enabled = true;
    }
    public void HideSelected() {
        selectable.enabled = false;
    }
    public virtual void PlaceWithNail() { Debug.Log("InteractCarry not implemented"); }

    }
