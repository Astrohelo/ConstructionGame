using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAndDie : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision) { 
        if (collision.gameObject.tag == "Player") {
            if (collision.gameObject.TryGetComponent(out Player player)) {
                player.Hurt();
            }
        }
    }
    

}
