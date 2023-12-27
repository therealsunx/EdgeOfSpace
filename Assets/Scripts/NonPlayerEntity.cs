using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerEntity: MonoBehaviour {
    
    public bool isAttacker;
    public float speed;
    public float effectOnHealth;
    public float effectOnShield;

    Transform player;

    void Start(){
        if(isAttacker) player = GameObject.Find("Player").transform;
    }

    void Update(){
        if(isAttacker){
            Vector3 dir = (player.position - transform.position).normalized;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * 57.29f);
            transform.Translate(Vector3.right * speed  * Time.deltaTime);
        }
    }

}
