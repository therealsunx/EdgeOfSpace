using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEntity : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] Vector3 speed;
    
    float maxGap;

    void Start(){
        float h_screenH = Camera.main.orthographicSize;
        maxGap = h_screenH * 2f;
    }

    void Update(){
        transform.Translate(speed * Time.deltaTime);
        if(player.position.y - transform.position.y > maxGap){
            Vector3 _p = transform.position;
            _p.y = player.position.y - maxGap;
            transform.position = _p;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag != "Player"){
            Destroy(other.gameObject);
        }else{
            other.GetComponent<PlayerController>().HandleDeath();
        }
    }
}
