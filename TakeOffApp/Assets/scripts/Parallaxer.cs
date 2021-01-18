using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parallaxer : MonoBehaviour {

    public enum Direction {
        Left,
        Top,
        Right,
        Bottom
    }

    class PoolObject {
        public Transform transform;
        public bool inUse;
        public Direction direction;
        public GameObject prefab;
        public float initialXVelocity;
        public float initialYVelocity;
        public bool toDestroy = false;

        public PoolObject(GameObject p) {
            prefab = Instantiate(p) as GameObject;
            prefab.transform.parent = GameObject.Find("ObstacleSpawner").GetComponent<Transform>().transform;
            transform = prefab.transform;
            inUse = true;
        }
    }

    [System.Serializable]
    public struct MasterTemplate {
        public GameObject prefab;
        public int stage;
        public Direction comingFrom;
        public float initialXVelocity;
        public float initialYVelocity;
    }

    public MasterTemplate[] MasterObjects;
    public int poolSize;
    public float initialShiftSpeedAdder;
    public float maxShiftSpeed;
    public float initialSpawnRate;

    private float shiftSpeedAdder;
    private float spawnRate;

    public int maxStage;

    public Vector2 topLeftCoords;
    public Vector2 bottomRightCoords;

    public Vector2 targetAspectRatio;

    float spawnTimer;
    float targetAspect;
    int currentTime;
    int stageLength;
    int stage;
    int nextStage;

    List<PoolObject> poolObjects;

    private bool gameOver = false;

    private void Awake() {
        shiftSpeedAdder = 0f;
        stageLength = GameObject.Find("Background").GetComponent<SpriteGradient>().GetStageLength();
        nextStage = 2;
        Configure();
    }

    public void resetShift() {
        spawnRate = initialSpawnRate;
        shiftSpeedAdder = 0.0f;
    }

    public void increaseShift() {
        if (shiftSpeedAdder < maxShiftSpeed) {
            shiftSpeedAdder += initialShiftSpeedAdder;
        }
    }

    public void OnDisable() {
        for(int i = 0; i < poolObjects.Count; i++) {
            DestroyImmediate(poolObjects[i].prefab);
            poolObjects.Remove(poolObjects[i]);
        }
        for (int i = 0; i < poolObjects.Count; i++) {
            DestroyImmediate(poolObjects[i].prefab);
            poolObjects.Remove(poolObjects[i]);
        }
    }
    
    private void Update() {
        gameOver = GameObject.Find("Main Camera").GetComponent<GameManager>().GameOver();
        currentTime = GameObject.Find("Main Camera").GetComponent<GameManager>().GetScore();
        stage = (currentTime / stageLength) + 1;

        if (!gameOver) {
            Shift();
            spawnTimer += Time.deltaTime;
            if(spawnTimer > spawnRate && poolObjects.Count < poolSize) {
                Spawn();
                spawnTimer = 0f;
                increaseShift();
            }
        }

        if(stage >= nextStage && stage < maxStage) {
            resetShift();
            nextStage = stage + 1;
        }


    }

    void Configure() {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        spawnRate = initialSpawnRate;
        poolObjects = new List<PoolObject>();

    }

    private void Spawn() {
        int pointer = UnityEngine.Random.Range(0, MasterObjects.Length);
        while ((stage < maxStage && MasterObjects[pointer].stage != stage) || MasterObjects[pointer].stage < maxStage && stage >= maxStage) {
            pointer = UnityEngine.Random.Range(0, MasterObjects.Length);
        }

        poolObjects.Add(new PoolObject(MasterObjects[pointer].prefab));

        poolObjects[poolObjects.Count - 1].direction = MasterObjects[pointer].comingFrom;
        poolObjects[poolObjects.Count - 1].transform.position = changePos(poolObjects[poolObjects.Count - 1].direction);


        poolObjects[poolObjects.Count - 1].initialXVelocity = MasterObjects[pointer].initialXVelocity;
        poolObjects[poolObjects.Count - 1].initialYVelocity = MasterObjects[pointer].initialYVelocity;

    }

    void Shift() {
        for (int i = 0; i < poolObjects.Count; i++) {
            if (!poolObjects[i].toDestroy) {
                Vector3 temp = Vector3.zero;
                if(poolObjects[i].direction == Direction.Left) {
                    temp = new Vector3(poolObjects[i].initialXVelocity + shiftSpeedAdder, poolObjects[i].initialYVelocity - shiftSpeedAdder, 0);
                }
                else if (poolObjects[i].direction == Direction.Top) {
                    temp = new Vector3(poolObjects[i].initialXVelocity, poolObjects[i].initialYVelocity - shiftSpeedAdder, 0);
                }
                else if (poolObjects[i].direction == Direction.Right) {
                    temp = new Vector3(poolObjects[i].initialXVelocity - shiftSpeedAdder, poolObjects[i].initialYVelocity - shiftSpeedAdder, 0);
                }
                else if (poolObjects[i].direction == Direction.Bottom) {
                    temp = new Vector3(poolObjects[i].initialXVelocity, poolObjects[i].initialYVelocity + shiftSpeedAdder, 0);
                }
                poolObjects[i].transform.position += temp;
                CheckDisposeObject(poolObjects[i]);
            }
        }
    }

    void CheckDisposeObject(PoolObject poolObject) {

        if(poolObject.direction == Direction.Left && poolObject.transform.position.x > bottomRightCoords.x) {
            poolObject.toDestroy = true;
        }
        else if (poolObject.direction == Direction.Top && poolObject.transform.position.y < bottomRightCoords.y) {
            poolObject.toDestroy = true;
        }
        else if (poolObject.direction == Direction.Right && poolObject.transform.position.x < topLeftCoords.x) {
            poolObject.toDestroy = true;
        }
        else if (poolObject.direction == Direction.Bottom) {

        }

        if (poolObject.toDestroy) {
            DestroyImmediate(poolObject.prefab);
            poolObjects.Remove(poolObject);
        }
    }

    public Vector3 changePos(Direction d) {
        Vector3 pos = Vector3.zero;
        if (d == Direction.Left) {
            pos.x = topLeftCoords.x + UnityEngine.Random.Range(-7, 0) * Camera.main.aspect / targetAspect; ;
            pos.y = UnityEngine.Random.Range(topLeftCoords.y, bottomRightCoords.y);
        }

        else if (d == Direction.Top) {
            pos.x = UnityEngine.Random.Range(topLeftCoords.x, bottomRightCoords.x);
            pos.y = topLeftCoords.y + UnityEngine.Random.Range(0, 7) * Camera.main.aspect / targetAspect;
        }

        else if (d == Direction.Right) {
            pos.x = bottomRightCoords.x + UnityEngine.Random.Range(0, 7) * Camera.main.aspect / targetAspect; ;
            pos.y = UnityEngine.Random.Range(topLeftCoords.y, bottomRightCoords.y);
        }

        else if (d == Direction.Bottom) {

        }

        return pos;
    }

    public bool OnScreen(Transform t) {
        if(t.position.y < bottomRightCoords.y || t.position.y > topLeftCoords.y || t.position.x < topLeftCoords.x || t.position.x > bottomRightCoords.x) {
            return false;
        }

        return true;
    }
}
