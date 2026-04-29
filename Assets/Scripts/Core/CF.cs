using UnityEngine;
public class CF : MonoBehaviour {
    public Transform target; public float offY=1.5f, smX=0.15f, smY=0.2f;
    Vector3 v;
    void LateUpdate() {
        if(!target) return;
        float x=Mathf.SmoothDamp(transform.position.x, target.position.x, ref v.x, smX);
        float y=Mathf.SmoothDamp(transform.position.y, target.position.y+offY, ref v.y, smY);
        transform.position=new Vector3(x,y,transform.position.z);
    }
}
