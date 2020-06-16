using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {

    public PlayerAppearance appearance;
    public PlayerMovement movement;
    public new Collider collider;
    public CurlingStone curlingStonePrefab;
    public Transform curlingStonePos;
    public ThrowStrengthIndicator throwStrengthIndicator;
    public int healthMax;
    [ReadOnly] public int healthCurrent;

     public bool canControlPlayer {
        get { return controls.Character.enabled; }
        set { 
            if(value) controls.Character.Enable(); 
                else controls.Character.Disable();
        }
    }

    private PlayerInput controls;
    private bool isBrewing;
    private bool isCurling;
    private float curlStrength, curlPrepareTime;
    private CurlingStone curlingStoneActive;
    private bool isHurt;

    protected override void Awake() {
        base.Awake();

        controls = new PlayerInput();

        controls.Character.Jump.performed += ctx => movement.Jump();
        controls.Character.Move.performed += ctx => movement.SetInput(ctx.ReadValue<Vector2>());
        controls.Character.Move.canceled += ctx => movement.SetInput(Vector2.zero);
        controls.Character.Sprint.performed += ctx => movement.SetRunning(true);
        controls.Character.Sprint.canceled += ctx => movement.SetRunning(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        healthCurrent = healthMax;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
        }

        if(Input.GetMouseButtonDown(0)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
        }

        if(Input.GetKeyDown(KeyCode.B)) {
            if(isBrewing) StopBrewing();
                else StartBrewing();
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            if(!isCurling && !isBrewing && movement.canMove) {
                PrepareCurl();
            }
        }

        if(Input.GetKeyUp(KeyCode.E)) {
            if(isCurling) {
                ThrowCurl();
            }
        }

        if(isCurling) {
            curlPrepareTime += Time.deltaTime;
            curlStrength = 0.5f - 0.5f * Mathf.Cos(curlPrepareTime * 4);
            throwStrengthIndicator.SetStrength(curlStrength);
        }
        if(curlingStoneActive) {
            curlingStoneActive.transform.position = curlingStonePos.position;
            curlingStoneActive.transform.rotation = appearance.modelAnimator.transform.rotation;
        }
    }

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    public void StartBrewing() {
        canControlPlayer = false;
        movement.LookAt(transform.position - Vector3.forward);
        CameraController.instance.SetOverriddenCameraPos(appearance.brewingCameraPos);
        appearance.OpenBelly();
        isBrewing = true;
    }

    public void StopBrewing() {
        canControlPlayer = true;
        CameraController.instance.SetOverriddenCameraPos(null);
        appearance.CloseBelly();
        isBrewing = false;
    }

    public void PrepareCurl() {
        isCurling = true;
        curlStrength = curlPrepareTime = 0;
        throwStrengthIndicator.Show();
        appearance.modelAnimator.SetTrigger("curlPrepare");
        curlingStoneActive = Instantiate(curlingStonePrefab, curlingStonePos.position, appearance.modelAnimator.transform.rotation);
        Physics.IgnoreCollision(collider, curlingStoneActive.collider, true);
        movement.canMove = false;
        CameraController.instance.smoothCamera = true;
    }

    public void ThrowCurl() {
        isCurling = false;
        throwStrengthIndicator.Hide();
        appearance.modelAnimator.SetTrigger("curlThrow");
        if(curlingStoneActive) {
            curlingStoneActive.Throw(curlStrength);
            Physics.IgnoreCollision(collider, curlingStoneActive.collider, false);
            curlingStoneActive = null;
        }
        movement.canLook = false;
        CameraController.instance.smoothCamera = false;
    }

    public void Hurt(Vector3 pos) {
        if(isHurt) return;
        isHurt = true;
        appearance.modelAnimator.SetTrigger("hurt");
        movement.canMove = false;
        movement.canLook = false;
        Vector3 dir = (transform.position - pos).normalized;
        dir.y = 0;
        movement.rigidbody.velocity = Vector3.zero;
        movement.rigidbody.AddForce(dir * 12, ForceMode.Impulse);
        movement.LookAt(pos);
    }

    public void Recover() {
        isHurt = false;
        movement.canLook = true;
        movement.canMove = true;
    }
}
