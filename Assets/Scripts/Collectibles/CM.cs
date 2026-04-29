using UnityEngine;
public class CM : MonoBehaviour {
    public static CM I { get; private set; }
    int total;
    void Awake() { if(I&&I!=this){Destroy(gameObject);return;} I=this; }
    public void Reg() { total++; GM.I?.SetTotal(total); }
    void OnDestroy() { if(I==this) I=null; }
}
