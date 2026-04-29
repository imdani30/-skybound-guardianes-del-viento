// PlayerController (PC) - Controls all of Aero's movement and abilities
// Features: horizontal movement, single jump, double jump, glide, wind dash
// Uses Unity 6 new Input System (Keyboard.current)
// Author: Daniel Andres Castro Silva - UNAD 213027A - 2026
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PC : MonoBehaviour {
    public float speed=7, jump=14, fallG=2.5f, lowJ=1.5f, glide=1;
    public float dashF=18, dashCost=0.35f, dashCD=0.4f, dashRech=3f, invTime=1.5f;
    public LayerMask ground;
    Rigidbody2D rb; SpriteRenderer sr;
    float hIn, dE=1, dT; bool grounded, inv;
    int jumpsLeft; const int MAX_JUMPS=2;
    Vector3 cp;
    public event System.Action<float> OnDash;
    void Awake() { rb=GetComponent<Rigidbody2D>(); sr=GetComponent<SpriteRenderer>(); }
    void Start() { cp=transform.position; }
    void Update() {
        if(GM.I!=null && GM.I.Paused) return;
        var kb=Keyboard.current; if(kb==null) return;
        hIn=0f;
        if(kb.aKey.isPressed||kb.leftArrowKey.isPressed)  hIn=-1f;
        if(kb.dKey.isPressed||kb.rightArrowKey.isPressed) hIn= 1f;
        if(kb.spaceKey.wasPressedThisFrame && jumpsLeft>0) {
            rb.linearVelocity=new Vector2(rb.linearVelocity.x, jump);
            jumpsLeft--;
            AM.I?.Jump();
            if(!grounded && sr) StartCoroutine(Flash(new Color(0.5f,0.85f,1f)));
        }
        if((kb.leftShiftKey.wasPressedThisFrame||kb.rightShiftKey.wasPressedThisFrame) && dT<=0 && dE>=dashCost) DoDash();
        dT-=Time.deltaTime;
        bool gl=!grounded && kb.spaceKey.isPressed && rb.linearVelocity.y<0;
        if(gl) rb.linearVelocity=new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y,-glide));
        if(grounded && dE<1) { dE=Mathf.Clamp01(dE+Time.deltaTime/dashRech); OnDash?.Invoke(dE); }
        if(hIn!=0 && sr) sr.flipX=hIn<0;
    }
    void FixedUpdate() {
        if(GM.I!=null && GM.I.Paused) return;
        grounded=Physics2D.OverlapBox((Vector2)transform.position+new Vector2(0,-0.55f), new Vector2(0.45f,0.05f), 0, ground);
        if(grounded) jumpsLeft=MAX_JUMPS;
        rb.linearVelocity=new Vector2(hIn*speed, rb.linearVelocity.y);
        var kb=Keyboard.current;
        if(rb.linearVelocity.y<0) rb.linearVelocity+=Vector2.up*Physics2D.gravity.y*(fallG-1)*Time.fixedDeltaTime;
        else if(rb.linearVelocity.y>0 && (kb==null||!kb.spaceKey.isPressed)) rb.linearVelocity+=Vector2.up*Physics2D.gravity.y*(lowJ-1)*Time.fixedDeltaTime;
    }
    void DoDash() {
        float d=hIn!=0?Mathf.Sign(hIn):(sr&&sr.flipX?-1f:1f);
        rb.linearVelocity=new Vector2(d*dashF, rb.linearVelocity.y);
        dE-=dashCost; dT=dashCD; OnDash?.Invoke(dE); AM.I?.Dash();
    }
    IEnumerator Flash(Color c) { if(!sr) yield break; Color o=sr.color; sr.color=c; yield return new WaitForSeconds(0.08f); sr.color=o; }
    public void Hit() { if(inv) return; AM.I?.Hurt(); GM.I?.Hit(); StartCoroutine(Inv()); }
    IEnumerator Inv() { inv=true; float t=invTime/12f; for(int i=0;i<12;i++){if(sr)sr.enabled=!sr.enabled;yield return new WaitForSeconds(t);} if(sr)sr.enabled=true; inv=false; }
    public void Respawn() { transform.position=cp; rb.linearVelocity=Vector2.zero; jumpsLeft=MAX_JUMPS; }
    public void SetCP(Vector3 p) { cp=p; }
}
