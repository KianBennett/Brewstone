using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour {

    public Animator modelAnimator;
    public SkinnedMeshRenderer skinnedDoor;
    public MeshRenderer staticDoor;
    public Transform brewingCameraPos;
    public Transform potionContainer;
    public GameObject bellyInsides;

    private Vector3 potionContainerInitPos;

    void Awake() {
        potionContainerInitPos = potionContainer.transform.localPosition;
    }

    void Update() {
        // staticDoor.transform.localRotation = Quaternion.Lerp(staticDoor.transform.localRotation, isBellyOpen ? Quaternion.Euler(90, 0, 130) : Quaternion.Euler(90, 0, 0), Time.deltaTime * 5);

        // if(!isBellyOpen && !hasReplacedStaticDoor && staticDoor.transform.localRotation.eulerAngles.y < 5) {
        //     skinnedDoor.gameObject.SetActive(true);
        //     staticDoor.gameObject.SetActive(false);    
        //     hasReplacedStaticDoor = true;
        // }

        potionContainer.transform.localPosition = new Vector3(potionContainerInitPos.x, potionContainerInitPos.y + 0.0004f * Mathf.Sin(Time.time * 4), potionContainerInitPos.z);
    }

    public void OpenBelly() {
        skinnedDoor.gameObject.SetActive(false);
        staticDoor.gameObject.SetActive(true);
        modelAnimator.SetBool("isBellyDoorOpen", true);
        bellyInsides.gameObject.SetActive(true);
    }

    public void CloseBelly() {
        modelAnimator.SetBool("isBellyDoorOpen", false);
        bellyInsides.gameObject.SetActive(false);
    }

    public void ReplaceBellyDoor() {
        skinnedDoor.gameObject.SetActive(true);
        staticDoor.gameObject.SetActive(false);
    }
}
