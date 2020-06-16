using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHelper : MonoBehaviour {

    public PlayerController player;

    public void ReplaceBellyDoor() {
        player.appearance.ReplaceBellyDoor();
    }

    public void GetUp() {
        player.Recover();
    }
}
