using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurlingStone : MonoBehaviour {

    public new Rigidbody rigidbody;
    public new Collider collider;
    public GameObject model;
    public Light pointLight;
    public ParticleSystem explodeParticlesPrefab, smokeParticlesPrefab;
    public float explosionRadius;
    public PotionType type;

    private float tick;
    private bool hasBeenThrown;
    [HideInInspector] public bool hasExploded;

    void Awake() {
    }

    void Update() {
        CameraController.instance.SetCameraPosition(transform.position);

        if(hasBeenThrown && (rigidbody.velocity.magnitude < 0.05f || hasExploded)) {
            tick += Time.deltaTime;
            if(tick > 0.1f) {
                if(!hasExploded) Explode();
                pointLight.intensity = Mathf.MoveTowards(pointLight.intensity, 0, Time.deltaTime * 10);
            }
            if(tick > 1.1f) {
                Destroy();
            }
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            Explode();
        }
    }

    public void Throw(float strength) {
        hasBeenThrown = true;
        rigidbody.AddForce(transform.forward * (20 + 25 * strength) , ForceMode.Impulse);
        rigidbody.AddTorque(Vector3.up * (10 + 15 * strength), ForceMode.Impulse);
    }

    public void Explode() {
        if(hasExploded) return;
        hasExploded = true;
        rigidbody.velocity = Vector3.zero;
        pointLight.gameObject.SetActive(true);
        Destroy(model);
        Instantiate(explodeParticlesPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
        Instantiate(smokeParticlesPrefab, transform.position, Quaternion.Euler(-90, 0, 0));

        // Not optimal but quick to implement
        foreach(Ghost ghost in GameObject.FindObjectsOfType<Ghost>()) {
            float dist = Vector3.Distance(ghost.transform.position, transform.position);
            if(dist < explosionRadius && ((ghost.hasWeakness && ghost.weakness == type) || !ghost.hasWeakness)) {
                ghost.Kill();
            }
        }
    }

    public void Destroy() {
        Destroy(gameObject);
        PlayerController.instance.movement.canLook = true;
        PlayerController.instance.movement.canMove = true;
    }

    // void OnTriggerEnter(Collider other) {
    //     Ghost ghost = other.GetComponent<Ghost>();

    //     if(ghost) {
    //         ghost.Kill();
    //     }    
    // }
}
