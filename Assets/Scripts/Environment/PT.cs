using UnityEngine;
public class PT : MonoBehaviour {
    bool active; SpriteRenderer sr; Collider2D col;
    void Awake() { sr=GetComponent<SpriteRenderer>(); col=GetComponent<Collider2D>(); if(col) col.enabled=false; }
    void Start() { if(GM.I!=null) GM.I.OnCrystals+=Check; }
    void OnDestroy() { if(GM.I!=null) GM.I.OnCrystals-=Check; }
    void Check(int c, int t) {
        if(active||t==0||c<t) return;
        active=true;
        if(sr) sr.color=new Color(0.55f,0.12f,1f);
        if(col) col.enabled=true;
        AM.I?.PortalOpen();
    }
    void OnTriggerEnter2D(Collider2D o) {
        if(!active||!o.CompareTag("Player")) return;
        AM.I?.PortalEnter();
        var hud=FindAnyObjectByType<HUD>(); if(hud) hud.ShowComplete();
        GM.I?.TriggerComplete();
    }
}
