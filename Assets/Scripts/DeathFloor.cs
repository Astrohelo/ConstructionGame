using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, speed, 0);
    }

    private void OnTriggerEnter(Collider other) {
        other.TryGetComponent<IResetable>(out IResetable resetable);
        if(resetable != null) {
            resetable.ResetPosition();
        }
    }
}
