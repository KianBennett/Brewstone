using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {

    public PlayerAppearance appearance;
    public PlayerMovement movement;

     public bool canControlPlayer {
        get { return controls.Character.enabled; }
        set { 
            if(value) controls.Character.Enable(); 
                else controls.Character.Disable();
        }
    }

    private PlayerInput controls;

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
    }

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }
}
