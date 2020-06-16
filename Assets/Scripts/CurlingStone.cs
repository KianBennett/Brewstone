using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurlingStone : MonoBehaviour {

    public new Rigidbody rigidbody;
    public new Collider collider;

    void Awake() {
    }

    public void Throw(float strength) {
        rigidbody.AddForce(transform.forward * (15 + 15 * strength) , ForceMode.Impulse);
        rigidbody.AddTorque(Vector3.up * 20, ForceMode.Impulse);
        Destroy(gameObject, 3);
    }

    void Update() {
        // Update camera to position of cameraPos transform
        CameraController.instance.SetCameraPosition(transform.position);
    }
}
