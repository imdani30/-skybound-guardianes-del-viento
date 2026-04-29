using UnityEngine;
public class CP : MonoBehaviour {
    bool done;
    void OnTriggerEnter2D(Collider2D o) {
        if(done||!o.CompareTag("Player")) return;
        done=true;
        o.GetComponent<PC>()?.SetCP(transform.position);
        var sr=GetComponent<SpriteRenderer>(); if(sr) sr.color=new Color(0.3f,1f,0.3f);
        AM.I?.Checkpoint();
    }
}
