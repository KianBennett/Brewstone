using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

    public Transform cameraContainer;
    public new Camera camera;

    private Vector3 cameraTargetPos;

    protected override void Awake() {
        base.Awake();
    }

    void LateUpdate() {
        transform.position = cameraTargetPos;
    }

    // Updates the camera target position
    public void SetCameraPosition(Vector3 position) {
        cameraTargetPos = position;
    }
}