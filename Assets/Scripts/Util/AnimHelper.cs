using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHelper : MonoBehaviour {

    public GameObject rootObject;

    public void Destroy() {
        Destroy(rootObject);
    }

    public void EndCutscene() {
        LevelManager.instance.EndCutscene();
    }
}
