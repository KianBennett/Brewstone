using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour {

    public Transform respawnPoint;

    void OnTriggerEnter(Collider other) {
        if(other.gameObject == PlayerController.instance.gameObject) {
            PlayerController.instance.Respawn(respawnPoint.position);
        }    

        CurlingStone stone = other.GetComponent<CurlingStone>();
        if(stone) {
            stone.Explode();
        }
    }
}
