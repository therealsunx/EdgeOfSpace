using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerProps {
    public bool isAlive;
    public bool isDashing;
    public bool isInvincible;
    public float health;
    public float armor;
    public int score;
};

public class PlayerController : MonoBehaviour {

    public static float difficulty = 1f;
    PlayerProps props = new PlayerProps{
        isAlive = true, isDashing = false, isInvincible = false, health = 1f, armor = 1f, score=0
    };

    Camera cam;
    Rigidbody2D rb;
    
    [SerializeField] TrailRenderer trail;
    [SerializeField] LineRenderer line;
    [SerializeField] float dashTime = 0.2f, dashCoolDown = 0.4f, dashMultiplier = 1f;
    [SerializeField] Color nrm_color, hrt_color, att_color;
    public float maxDrag = 6.8f;
    
    float invincibleTime = 0.2f;
    
    Vector3 pointerPos;
    Vector3 initialPos;
    Vector3 stretch;

    bool dragging = false;
    float _lastH = 0f;

    SpriteRenderer sp;
    Color trg_color;
    
    void Start(){
        trg_color = nrm_color;
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update(){
        if(!props.isAlive) return;
        trail.emitting = rb.velocity.magnitude > 10f;
        HandleScore();
        HandleInput();
        UpdateColor();
    }

    void HandleScore(){
        if(props.health <= 0f) HandleDeath();

        if((transform.position.y - _lastH)>=10f) {
           props.score += 1;
           _lastH = transform.position.y;
           difficulty = 1f + props.score / 300f;
        }
    }

    void UpdateColor(){
        sp.color = Color.Lerp(sp.color, trg_color, 0.2f);
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
                Vector3 _p = transform.position;
                _p.z=0.1f;
                line.SetPosition(0, _p);
                initialPos = pointerPos;
            }else if(rb.velocity.magnitude > 3f){
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 57.29f);
            }
        }else{
            stretch = pointerPos - initialPos;
            if(stretch.magnitude > maxDrag * difficulty){
                stretch = stretch.normalized * maxDrag * difficulty;
            }

            if(Input.GetMouseButtonUp(0)){
                HandleRelease();
            }else {
                line.positionCount = 2;
                Vector3 _p = transform.position;
                _p.z=0.1f;
                line.SetPosition(1, _p + stretch);
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
        trg_color = att_color;
        props.isDashing = true;
        props.isInvincible =true;
        yield return new WaitForSeconds(dashTime);
        rb.velocity *= 0.1f;
        yield return new WaitForSeconds(dashCoolDown);
        props.isDashing = false;
        props.isInvincible = false;
        rb.gravityScale = 3f;
        trg_color = nrm_color;
    }

    IEnumerator Invincible(){
        trg_color = hrt_color;
        props.isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        props.isInvincible = false;
        trg_color = nrm_color;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "npe"){
            if(other.GetComponent<NonPlayerEntity>().HandlePlayerCollision(ref props)) StartCoroutine(Invincible());
        }
    }
}
