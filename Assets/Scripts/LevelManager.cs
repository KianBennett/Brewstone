using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager> {

    private class IngredientSpawnInfo {
        public float timer;
        public Vector3 pos;
        public PotionType type;
    }

    public GroundItem brimstonePrefab, nitrogenPrefab, crystalPrefab, mushroomPrefab, gunpowerPrefab;

    private List<IngredientSpawnInfo> ingredientSpawnList = new List<IngredientSpawnInfo>();
    private const float ingredientSpawnInterval = 20.0f;

    void Update() {
        // Iterate over list backwards to allow for remove items from list
        for(int i = ingredientSpawnList.Count; i > 0; i--) {
            IngredientSpawnInfo spawnInfo = ingredientSpawnList[i - 1];
            spawnInfo.timer -= Time.deltaTime;
            if(spawnInfo.timer < 0) {
                Instantiate(getIngredientPrefab(spawnInfo.type), spawnInfo.pos, Quaternion.identity);
                ingredientSpawnList.Remove(spawnInfo);
            }
        }
    }

    public void AddIngredientToSpawn(Vector3 pos, PotionType type) {
        ingredientSpawnList.Add(new IngredientSpawnInfo() {
            timer = ingredientSpawnInterval,
            pos = pos,
            type = type
        });
    }

    private GroundItem getIngredientPrefab(PotionType type) {
        switch(type) {
            case PotionType.Brimstone:
                return brimstonePrefab;
            case PotionType.Nitrogen:
                return nitrogenPrefab;
            case PotionType.Crystal:
                return crystalPrefab;
            case PotionType.Mushroom:
                return mushroomPrefab;
            case PotionType.Gunpowder:
                return gunpowerPrefab;
        }
        return null;
    }
}
