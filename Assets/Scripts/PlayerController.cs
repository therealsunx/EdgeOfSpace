using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    Camera cam;
    Rigidbody2D rb;
    
    [SerializeField] TrailRenderer trail;
    [SerializeField] LineRenderer line;
    [SerializeField] float dashTime = 0.2f, dashCoolDown = 0.4f, dashMultiplier = 1f;
    public float maxDrag = 6.8f;
    
    public float health = 1f;
    public float shield = 0f;
    public int score = 0;
    
    float invincibleTime = 0.3f;
    bool isInvincible = false;

    Vector3 pointerPos;
    Vector3 initialPos;
    Vector3 stretch;

    bool dragging = false;
    
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update(){
        trail.emitting = rb.velocity.magnitude > 10f;
        HandleScore();
        HandleInput();
    }

    void HandleScore(){
        int _h = Mathf.RoundToInt(transform.position.y / 5f);
        if(_h > score) score = _h;
    }

    public void HandleDeath(){
        Debug.Log("Ded");
    }

    void HandleInput(){
        pointerPos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(!dragging){
            if(Input.GetMouseButtonDown(0)){
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                dragging = true;
                line.positionCount = 1;
                line.SetPosition(0, transform.position);
                initialPos = pointerPos;
            }else if(rb.velocity.magnitude > 3f){
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 57.29f);
            }
        }else{
            stretch = pointerPos - initialPos;
            if(stretch.magnitude> maxDrag){
                stretch = stretch.normalized * maxDrag;
            }

            if(Input.GetMouseButtonUp(0)){
                HandleRelease();
            }else {
                line.positionCount = 2;
                line.SetPosition(1, transform.position + stretch);
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(-stretch.y, -stretch.x) * 57.29f);
            }
        }
    }

    void HandleRelease(){
        dragging = false;
        line.positionCount = 0;
        rb.AddForce(-stretch * dashMultiplier, ForceMode2D.Impulse);
        StartCoroutine(Dash()); 
    }

    IEnumerator Dash(){
        yield return new WaitForSeconds(dashTime);
        rb.velocity *= 0.1f;
        yield return new WaitForSeconds(dashCoolDown);
        rb.gravityScale = 3f;
    }

    IEnumerator Invincible(){
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "npe"){

            NonPlayerEntity npe = other.GetComponent<NonPlayerEntity>();

            if(npe.isAttacker){
                if(rb.velocity.magnitude > 30f) {
                    Destroy(other.gameObject);
                    return;
                }
                if(!isInvincible){
                    health = (1f - shield) * npe.effectOnHealth;
                    if(shield > 0f) shield -= npe.effectOnShield;
                    if(health < 0f) HandleDeath();
                    StartCoroutine(Invincible());
                }
            }else{
                health += npe.effectOnHealth;
                shield += npe.effectOnShield;

                if(health > 1f) health = 1f;
                if(shield > 1f) shield = 1f;
                Destroy(other.gameObject);
            }
        }
    }
}
