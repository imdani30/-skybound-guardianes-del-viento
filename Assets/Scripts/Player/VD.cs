using UnityEngine;
public class VD : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D o) { if(o.CompareTag("Player")) o.GetComponent<PC>()?.Hit(); }
}
