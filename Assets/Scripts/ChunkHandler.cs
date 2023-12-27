using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHandler : MonoBehaviour {
    
    public float minSpread, maxSpread, h_width, h_height;
    public int min_n_obs, max_n_obs;
    
    EnvironmentHandler env;
    List<GameObject> contents = new List<GameObject>();
    
    Vector3 lastPos;
    float spread;
    int n_objects;
    
    void Awake(){
        env = GetComponentInParent<EnvironmentHandler>();
        lastPos = new Vector3(Random.Range(-h_width, h_width), Random.Range(-h_height, h_height), 0f) + transform.position;
        n_objects = Random.Range(min_n_obs, max_n_obs+1);

        if(transform.position.y < 8f) return;

        Vector3 pos = lastPos;
        
        for(int i=0; i<n_objects; i++){
            spread = Random.Range(minSpread, maxSpread);
            for(int _i=0; _i<20; _i++){
                pos = Random.onUnitSphere;
                pos.z = 0f;
                pos = lastPos + pos.normalized * spread;
                if(IsValid(pos)) break;
            }
            contents.Add(Instantiate(env.objects[Random.Range(0, env.objects.Length)], pos, Quaternion.identity, transform));
            lastPos = pos;
        }
    }

    bool IsValid(Vector2 pos){
        Vector2 del = pos - (Vector2) transform.position;
        del.Set(Mathf.Abs(del.x), Mathf.Abs(del.y));
        return (del.x < h_width && del.y < h_height);
    }

    void OnDestroy(){
        foreach(GameObject i in contents){
            Destroy(i);
        }
        contents.Clear();
    }
}
