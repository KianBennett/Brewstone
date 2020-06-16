using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

    public Transform cameraContainer;
    public new Camera camera;
    public bool smoothCamera;

    private Vector3 cameraTargetPos;
    private Transform overridenCameraContainerPos;
    private Vector3 cameraContainerPosInit;
    private Quaternion cameraContainerRotInit;

    protected override void Awake() {
        base.Awake();

        cameraContainerPosInit = cameraContainer.localPosition;
        cameraContainerRotInit = cameraContainer.localRotation;
    }

    void LateUpdate() {
        if(overridenCameraContainerPos) {
            cameraContainer.transform.position = Vector3.Lerp(cameraContainer.transform.position, overridenCameraContainerPos.position, Time.deltaTime * 7);
            cameraContainer.transform.rotation = Quaternion.Lerp(cameraContainer.transform.rotation, overridenCameraContainerPos.rotation, Time.deltaTime * 7);
        } else {
            cameraContainer.transform.localPosition = Vector3.Lerp(cameraContainer.transform.localPosition, cameraContainerPosInit, Time.deltaTime * 5);
            cameraContainer.transform.localRotation = Quaternion.Lerp(cameraContainer.transform.localRotation, cameraContainerRotInit, Time.deltaTime * 5);
        }
        // cameraContainer.transform.position = Vector3.MoveTowards(cameraContainer.transform.position, 
        //     overridenCameraContainerPos ? overridenCameraContainerPos.position : cameraContainerPosInit, Time.deltaTime * 10);
        // cameraContainer.transform.rotation = Quaternion.RotateTowards(cameraContainer.transform.rotation, 
        //     overridenCameraContainerPos ? overridenCameraContainerPos.rotation : cameraContainerRotInit, Time.deltaTime * 10);

        if(smoothCamera) {
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, cameraTargetPos, ref velocity, Time.deltaTime * 5, 50);
        } else {
            transform.position = cameraTargetPos;
        }
    }

    // Updates the camera target position
    public void SetCameraPosition(Vector3 position) {
        cameraTargetPos = position;
    }

    public void SetOverriddenCameraPos(Transform transform) {
        overridenCameraContainerPos = transform;
    }
}