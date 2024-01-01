using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerEntity: MonoBehaviour {
    
    public bool isAttacker;
    public float speed;
    public float effectOnHealth;
    public float effectOnArmor;
    public int captureReward;
    
    float maxSpeed;
    Transform player;

    void Start(){
        if(isAttacker){
            player = GameObject.Find("Player").transform;
            maxSpeed = speed * 2f;
        }
    }

    void Update(){
        if(isAttacker){
            Vector3 del = player.position - transform.position;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(del.y, del.x) * 57.29f);
            float _s = speed * PlayerController.difficulty;
            if(_s>maxSpeed) _s=maxSpeed;
            if(del.magnitude > 0.5f) transform.Translate(Vector3.right *_s* Time.deltaTime);
        }
    }

    public bool HandlePlayerCollision(ref PlayerProps props){
        if(isAttacker){
            if(props.isDashing) {
                props.score += captureReward;
                Destroy(gameObject);
                CameraShake.instance.ShakeCamera(4.2f);
                ParticleManager.instance.StartExplosion(transform.position);
            }else if(!props.isInvincible){
                props.health -= (1f - props.armor) * effectOnHealth;
                props.armor -= effectOnArmor;
                CameraShake.instance.ShakeCamera(3.2f);
                return true;
            }
        }else{
            props.health += effectOnHealth;
            props.armor += effectOnArmor;
            Destroy(gameObject);
        }
        return false;
    }
}
