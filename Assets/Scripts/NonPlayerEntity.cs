using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerEntity: MonoBehaviour {
    
    public bool isAttacker;
    public float speed;
    public float effectOnHealth;
    public float effectOnArmor;
    public int captureReward;

    Transform player;

    void Start(){
        if(isAttacker) player = GameObject.Find("Player").transform;
    }

    void Update(){
        if(isAttacker){
            Vector3 del = player.position - transform.position;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(del.y, del.x) * 57.29f);
            if(del.magnitude > 0.5f) transform.Translate(Vector3.right * speed * PlayerController.difficulty * Time.deltaTime);
        }
    }

}
