using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour {

    public Animator animator;
    public new Rigidbody rigidbody;
    public MeshRenderer[] eyeRenderers;
    public Material eyeMat, eyeMatChase;
    public float moveSpeed;
    public bool hasWeakness;
    public PotionType weakness;
    public Transform[] patrolPoints;

    private int patrolPointCurrent;
    private bool isDead, isChasingPlayer;
    private const float chaseRange = 10.0f;
    private Vector3 lookDir;

    void Update() {
        if(!isDead) {
            float playerDist = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
            bool isChasingPlayer = playerDist < chaseRange && !PlayerController.instance.isHurt;

            if(patrolPoints.Length > 0) {
                float patrolPointDist = Vector3.Distance(transform.position, patrolPoints[patrolPointCurrent].position);

                if(!isChasingPlayer && patrolPointDist < 0.1f) {
                    goToNextPatrolPoint();
                }
            }
            
            Vector3 target = transform.position;

            if(isChasingPlayer) {
                target = PlayerController.instance.transform.position;
            } else if(patrolPoints.Length > 0) {
                target = patrolPoints[patrolPointCurrent].position;
            }

            Vector3 dir = (target - transform.position).normalized * moveSpeed;
            lookDir = dir;
            
            dir.y = rigidbody.velocity.y;
            rigidbody.velocity = dir;

            
            lookDir.y = 0;
            if(lookDir != Vector3.zero) {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 4);
            }

            foreach(MeshRenderer eyeRenderer in eyeRenderers) {
                eyeRenderer.material = isChasingPlayer ? eyeMatChase : eyeMat;
            }
        }
    }

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

    private void goToNextPatrolPoint() {
        patrolPointCurrent++;
        if(patrolPointCurrent > patrolPoints.Length - 1) {
            patrolPointCurrent = 0;
        }
    }
}
