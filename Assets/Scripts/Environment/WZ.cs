using UnityEngine;
public class WZ : MonoBehaviour {
    public float lift=12f, maxSpeed=10f;
    Rigidbody2D prb;
    void OnTriggerEnter2D(Collider2D o) { if(o.CompareTag("Player")) prb=o.GetComponent<Rigidbody2D>(); }
    void OnTriggerExit2D(Collider2D o)  { if(o.CompareTag("Player")) prb=null; }
    void FixedUpdate() {
        if(prb && prb.linearVelocity.y<maxSpeed)
            prb.AddForce(Vector2.up*lift*Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
