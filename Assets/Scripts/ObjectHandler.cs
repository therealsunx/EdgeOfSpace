using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler: MonoBehaviour {
    [SerializeField] GameObject[] objects;
    [SerializeField] GameObject[] entities;
    [SerializeField] Transform player;
    [SerializeField] float p_gap;
    [SerializeField] float h_width;
    
    float lastY;
    float prob = 0.3f;
    float spreadY;
    Vector3 spawnPoint;

    void Start(){
        spawnPoint = Vector3.up * (player.position.y + p_gap+1f);
        spreadY = 0f;
        lastY = spawnPoint.y;
    }

    void Update(){
        spawnPoint.y = player.position.y + p_gap;
        if((spawnPoint.y - lastY) > spreadY){
            spawnPoint.x = -h_width;
            spreadY = 0f;
            
            GameObject o;
            for (int i = 0; i < Mathf.RoundToInt(h_width); i++){
                switch (Random.Range(0, 5)){
                    case 0: 
                    case 1:
                    case 3:
                        o = objects[Random.Range(0, objects.Length)];
                         break;
                    case 2:
                    case 4:
                    default:
                        o = entities[Random.Range(0, entities.Length)];
                        break;
                }
                spawnPoint.x += o.transform.localScale.x * 2f;
                if(spawnPoint.x > (h_width-o.transform.localScale.x/2f)) break;
                if(o.transform.localScale.y > spreadY) spreadY = o.transform.localScale.y;
                if(Random.value <= prob) Instantiate(o, spawnPoint, Quaternion.identity, transform);
            }
            lastY = spawnPoint.y;
            spreadY += 3f;
        }
    }
}
