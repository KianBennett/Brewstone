using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public Animator animator;

    private bool isDead;

    void OnTriggerEnter(Collider other) {
        if(other.gameObject == PlayerController.instance.gameObject) {
            PlayerController.instance.Hurt(transform.position);
            animator.SetTrigger("attack");
        }
    }

    public void Kill() {
        if(isDead) return;
        isDead = true;
        animator.SetTrigger("die");
    }
}
