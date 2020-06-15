using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public PlayerController player;
    public new Rigidbody rigidbody;
    public Animator modelAnimator;
    public Transform cameraPos; // The transform that the camera should follow (around the shoulders)
    public float walkSpeed, runSpeed, turnSpeedWalk, turnSpeedRun;
    public float jumpForce;
    [ReadOnly] public bool isRunning;

    private Vector2 inputDir; // Set from controller input in PlayerController
    private Vector3 lookDir; // The direction for the model to be looking at
    private float animForward; // What to set the animator parameter moveSpeed to, not to be confused with actual movement speed
    private bool isGrounded; // Whether the collider is sitting on the ground

    void Update() {
        // Rotate the input vector by camera rotation
        Vector3 input = CameraController.instance.transform.rotation * new Vector3(inputDir.x, 0, inputDir.y);
        // Normalise input vector otherwise diagonal movements (1, 1) will have a length 1.4 so the velocity will be higher
        if(input.magnitude > 1) input.Normalize();

        // Set animator values
        float transitionSpeed = 10;
        float animForwardTarget = input.magnitude;
        if (!isRunning && animForwardTarget > 0.5f) {
            animForwardTarget = 0.5f;
        }
        animForward = Mathf.Lerp(animForward, animForwardTarget, transitionSpeed * Time.deltaTime);
        if(animForward < 0.01f) animForward = 0;
        modelAnimator.SetFloat("moveSpeed", animForward);
        modelAnimator.SetBool("isJumping", !isGrounded);

        // Scale input by animation speed
        if (animForwardTarget > 0) input *= animForward / animForwardTarget;

        // Get movement vector by multiplying input my current speed (depending on whether the player is running etc)
        Vector3 movement = input * GetMaxSpeed();

        // Set look direction to input vector (i.e. the direction the player is moving)
        if(input != Vector3.zero) {
            lookDir = input;
        }
        // Only rotate model in xz axis
        lookDir.y = 0;

        // Don't update rotation if lookDir is zero as Quaterion.LookRotation throws an error
        if (lookDir != Vector3.zero) {
            // Linearly interpolate to make turning smooth (turn quicker when running)
            float turnSpeed = isRunning ? turnSpeedRun : turnSpeedWalk;
            modelAnimator.transform.rotation = Quaternion.Lerp(modelAnimator.transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * turnSpeed);
            float angle = Quaternion.Angle(modelAnimator.transform.rotation, Quaternion.LookRotation(lookDir));
            movement *= 1 - angle / 180;
        }

        // Set upper limit on y velocity to avoid weird moments with the stomp platforms
        if(rigidbody.velocity.y > 50) {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 50, rigidbody.velocity.z);
        }
        // Keep y velocity otherwise jumping/gravity won't work
        movement.y = rigidbody.velocity.y;
        rigidbody.velocity = movement;

        // Update camera to position of cameraPos transform
        CameraController.instance.SetCameraPosition(cameraPos.position);
    }

    public float GetMaxSpeed() {
        float speed = walkSpeed;
        if(isRunning) speed = runSpeed;
        return speed;
    }

    public void SetInput(Vector2 input) {
        inputDir = input;
    }

    public void LookAt(Vector3 pos) {
        lookDir = pos - transform.position;
    }

    public void LookAtImmediate(Vector3 pos) {
        LookAt(pos);
        modelAnimator.transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void Jump() {
        if(!isGrounded) return;
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    public void SetRunning(bool running) {
        isRunning = running;
    }

     void OnCollisionEnter(Collision other) {
        if(isCollisionWithGround(other)) {
            if(!isGrounded) {
                isGrounded = true;
            }
        }
    }

    // Gets the angle of the collision normal (the vector perpendicular to the surface), if it's below 45 then it counts as a flat surface (i.e. ground)
    private bool isCollisionWithGround(Collision collision) {
        for(int i = 0; i < collision.contactCount; i++) {
            ContactPoint contact = collision.contacts[0];
            float angle = Vector3.Angle(Vector3.up, contact.normal);
            if(angle < 45) {
                return true;
            }
        }
        return false;
    }
}