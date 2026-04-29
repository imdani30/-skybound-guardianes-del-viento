using UnityEngine;
using System.Collections;
public class CR : MonoBehaviour {
    public float amp=0.22f, freq=1.2f, rot=90f;
    Vector3 sp; bool done;
    void Start() { sp=transform.position; CM.I?.Reg(); }
    void Update() {
        if(done) return;
        transform.position=new Vector3(sp.x, sp.y+Mathf.Sin(Time.time*freq*Mathf.PI*2)*amp, sp.z);
        transform.Rotate(0,0,rot*Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D o) {
        if(done||!o.CompareTag("Player")) return;
        done=true; GM.I?.Collect(); AM.I?.Crystal();
        var sr=GetComponent<SpriteRenderer>(); if(sr) sr.enabled=false;
        var col=GetComponent<Collider2D>(); if(col) col.enabled=false;
        Destroy(gameObject, 0.3f);
    }
}
