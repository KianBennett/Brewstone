using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PotionType {
    Brimstone, Crystal, Nitrogen, Mushroom, Gunpowder
}

public class PlayerController : Singleton<PlayerController> {

    public PlayerAppearance appearance;
    public PlayerMovement movement;
    public new Collider collider;
    public CurlingStone curlingStoneBrimstonePrefab, curlingStoneNitrogenPrefab, curlingStoneCrystalPrefab, curlingStoneMushroomPrefab;
    public Transform curlingStonePos;
    public ThrowStrengthIndicator throwStrengthIndicator;
    public int healthMax;
    [ReadOnly] public int healthCurrent;
    [ReadOnly] public List<GroundItem> itemsNearby = new List<GroundItem>();
    [ReadOnly] public PotionType potionTypeSelected;

     public bool canControlPlayer {
        get { return controls.Character.enabled; }
        set { 
            if(value) controls.Character.Enable(); 
                else controls.Character.Disable();
        }
    }

    private PlayerInput controls;
    private bool isBrewing;
    public bool isCurling;
    private float curlStrength, curlPrepareTime;
    private CurlingStone curlingStoneActive;
    public bool isHurt;
    private GroundItem itemGrabbing;

    protected override void Awake() {
        base.Awake();

        controls = new PlayerInput();

        controls.Character.Jump.performed += ctx => movement.Jump();
        controls.Character.Move.performed += ctx => movement.SetInput(ctx.ReadValue<Vector2>());
        controls.Character.Move.canceled += ctx => movement.SetInput(Vector2.zero);
        controls.Character.Sprint.performed += ctx => movement.SetRunning(true);
        controls.Character.Sprint.canceled += ctx => movement.SetRunning(false);
        controls.Character.Grab.performed += ctx => GrabNearestItem();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        healthCurrent = healthMax;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
        }

        //if(Input.GetMouseButtonDown(0)) {
        //    Cursor.visible = false;
        //    Cursor.lockState = CursorLockMode.Locked; 
        //}

        if(Input.GetKeyDown(KeyCode.B)) {
            if(isBrewing) StopBrewing();
                else StartBrewing();
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            if(!isCurling && !isBrewing && movement.canMove && hasPotionSelected()) {
                PrepareCurl();
            }
        }

        if(Input.GetKeyUp(KeyCode.E)) {
            if(isCurling) {
                ThrowCurl();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            potionTypeSelected = PotionType.Brimstone;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            potionTypeSelected = PotionType.Crystal;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            potionTypeSelected = PotionType.Nitrogen;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            potionTypeSelected = PotionType.Mushroom;
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StopBrewing() {
        canControlPlayer = true;
        CameraController.instance.SetOverriddenCameraPos(null);
        appearance.CloseBelly();
        isBrewing = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PrepareCurl() {
        isCurling = true;
        curlStrength = curlPrepareTime = 0;
        throwStrengthIndicator.Show();
        appearance.modelAnimator.SetTrigger("curlPrepare");
        curlingStoneActive = Instantiate(getCurlingStonePrefab(potionTypeSelected), curlingStonePos.position, appearance.modelAnimator.transform.rotation);
        Physics.IgnoreCollision(collider, curlingStoneActive.collider, true);
        movement.canMove = false;
        movement.rigidbody.velocity = Vector3.zero;
        CameraController.instance.smoothCamera = true;

        switch(potionTypeSelected) {
            case PotionType.Brimstone:
                InventoryManager.instance.brimstonePotionHeld--;
                break;
            case PotionType.Crystal:
                InventoryManager.instance.crystalPotionHeld--;
                break;
            case PotionType.Nitrogen:
                InventoryManager.instance.nitrogenPotionHeld--;
                break;
            case PotionType.Mushroom:
                InventoryManager.instance.mushroomPotionHeld--;
                break;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    public void GrabNearestItem() {
        if(itemsNearby.Count == 0) return;

        Grab(itemsNearby.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).ToArray()[0]);
    }

    public void Grab(GroundItem item) {
        appearance.modelAnimator.SetTrigger("grab");
        itemGrabbing = item;
        // movement.canLook = false;
        movement.canMove = false;
        movement.LookAt(item.transform.position);
        movement.rigidbody.velocity = Vector3.zero;
    }

    public void PickUpItem() {
        if(itemGrabbing != null) {
            switch(itemGrabbing.type) {
                case PotionType.Brimstone:
                    InventoryManager.instance.brimstoneHeld++;
                    break;
                case PotionType.Crystal:
                    InventoryManager.instance.crystalHeld++;
                    break;
                case PotionType.Nitrogen:
                    InventoryManager.instance.nitrogenHeld++;
                    break;
                case PotionType.Mushroom:
                    InventoryManager.instance.mushroomHeld++;
                    break;
                case PotionType.Gunpowder:
                    InventoryManager.instance.gunpowderHeld += 5;
                    break;
            }
            LevelManager.instance.AddIngredientToSpawn(itemGrabbing.transform.position, itemGrabbing.type);
            Destroy(itemGrabbing.gameObject);
        }
    }

    private bool hasPotionSelected() {
        switch(potionTypeSelected) {
            case PotionType.Brimstone:
                return InventoryManager.instance.brimstonePotionHeld > 0;
            case PotionType.Crystal:
                return InventoryManager.instance.crystalPotionHeld > 0;
            case PotionType.Nitrogen:
                return InventoryManager.instance.nitrogenPotionHeld > 0;
            case PotionType.Mushroom:
                return InventoryManager.instance.mushroomPotionHeld > 0;
        }
        return false;
    }

    private CurlingStone getCurlingStonePrefab(PotionType type) {
        switch(type) {
            case PotionType.Brimstone:
                return curlingStoneBrimstonePrefab;
            case PotionType.Nitrogen:
                return curlingStoneNitrogenPrefab;
            case PotionType.Crystal:
                return curlingStoneCrystalPrefab;
            case PotionType.Mushroom:
                return curlingStoneMushroomPrefab;
        }
        return null;
    }
}