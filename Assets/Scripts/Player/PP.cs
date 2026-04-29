using UnityEngine;
public class PP : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D c) {
        if(!c.gameObject.CompareTag("MovingPlatform")) return;
        foreach(var ct in c.contacts) if(ct.normal.y>0.5f) { transform.SetParent(c.transform); return; }
    }
    void OnCollisionExit2D(Collision2D c) { if(c.gameObject.CompareTag("MovingPlatform")) transform.SetParent(null); }
}
