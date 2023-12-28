using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHandler : MonoBehaviour {
    EnvironmentHandler env;

    [SerializeField] Vector2 minSpread, maxSpread, size;

    float minProb = 0.8f;

    void Awake(){
        env = GetComponentInParent<EnvironmentHandler>();

        Vector2 spread = new Vector2(Random.Range(minSpread.x, maxSpread.x), Random.Range(minSpread.y, maxSpread.y));
        int xc = Mathf.RoundToInt(size.x/spread.x);
        int yc = Mathf.RoundToInt(size.y/spread.y);
        
        bool spawn = true;
        float minP = minProb / PlayerController.difficulty;

        Vector3 spawnPoint = transform.position - (Vector3) (size - spread)/2f;
        spawnPoint.z = 0f;
        for(int y=0; y < yc; y++){
            for(int x=0; x < xc; x++){
                spawn = Random.value > minP;
                if(spawn){
                    Debug.Log("Spawned at " + spawnPoint);
                    Instantiate(env.objects[Random.Range(0, env.objects.Length)], spawnPoint, Quaternion.identity, transform);
                }
                spawnPoint.x += spread.x;
            }
            spawnPoint.x -= spread.x * xc;
            spawnPoint.y += spread.y;
        }
    }
}
