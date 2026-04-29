// NeblinAI (NB) - Enemy artificial intelligence for Neblin creatures
// Two-state FSM: Patrol (waypoint pendular movement) and Chase (player pursuit)
// Detection uses Physics2D.OverlapCircle with configurable radius
// Author: Daniel Andres Castro Silva - UNAD 213027A - 2026
using UnityEngine;
public class NB : MonoBehaviour {
    public Transform ptA, ptB;
    public float pSpeed=2.5f, cSpeed=4.5f, detect=4f, giveUp=7f;
    public LayerMask playerMask;
    Rigidbody2D rb; SpriteRenderer sr; Transform cur, tgt; bool chasing;
    void Awake() { rb=GetComponent<Rigidbody2D>(); sr=GetComponent<SpriteRenderer>(); }
    void Start() { cur=ptA; }
    void Update() { if(chasing) Chase(); else Patrol(); }
    void Patrol() {
        if(!ptA||!ptB) return;
        Vector2 d=((Vector2)cur.position-(Vector2)transform.position).normalized;
        rb.linearVelocity=d*pSpeed; if(sr) sr.flipX=d.x<0;
        if(Vector2.Distance(transform.position,cur.position)<0.3f) cur=cur==ptA?ptB:ptA;
        var h=Physics2D.OverlapCircle(transform.position,detect,playerMask);
        if(h) { tgt=h.transform; chasing=true; }
    }
    void Chase() {
        if(!tgt||Vector2.Distance(transform.position,tgt.position)>giveUp) { chasing=false; tgt=null; return; }
        Vector2 d=((Vector2)tgt.position-(Vector2)transform.position).normalized;
        rb.linearVelocity=d*cSpeed; if(sr) sr.flipX=d.x<0;
    }
    void OnTriggerEnter2D(Collider2D o) { if(o.CompareTag("Player")) o.GetComponent<PC>()?.Hit(); }
}
