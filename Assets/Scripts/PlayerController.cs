using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    Camera cam;
    Rigidbody2D rb;

    [SerializeField] TrailRenderer trail;
    [SerializeField] LineRenderer line;
    [SerializeField] float dashTime = 0.2f, dashCoolDown = 0.4f, dashMultiplier = 1f;
    public float maxSqrDrag = 36f;
    
    Vector3 pointerPos;
    Vector3 initialPos;
    Vector3 stretch;

    bool dragging = false;
    
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update(){
        trail.emitting = rb.velocity.sqrMagnitude > 100f;
        HandleInput();
    }

    void HandleInput(){
        pointerPos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0) && !dragging){
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            dragging = true;
            line.positionCount = 1;
            line.SetPosition(0, transform.position);
            initialPos = pointerPos;
        }else if(dragging){

            stretch = pointerPos - initialPos;
            if(stretch.sqrMagnitude > maxSqrDrag){
                stretch = stretch.normalized * Mathf.Sqrt(maxSqrDrag);
            }

            if(Input.GetMouseButtonUp(0)){
                dragging = false;
                line.positionCount = 0;
                rb.AddForce(-stretch* dashMultiplier, ForceMode2D.Impulse);
                StartCoroutine(Dash()); 
            }else {
                line.positionCount = 2;
                line.SetPosition(1, transform.position + stretch);
            }
        }
    }

    IEnumerator Dash(){
        yield return new WaitForSeconds(dashTime);
        rb.velocity *= 0.3f;
        yield return new WaitForSeconds(dashCoolDown);
        rb.gravityScale = 4f;
    }
}
