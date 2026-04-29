using UnityEngine;
public class MP : MonoBehaviour {
    public Vector3 ptA, ptB; public float speed=2f, pause=0.5f;
    float prog; bool toB=true, pausing; float pt;
    void Update() {
        if(pausing){pt-=Time.deltaTime;if(pt<=0){pausing=false;toB=!toB;}return;}
        float d=speed/Mathf.Max(0.01f,Vector3.Distance(ptA,ptB))*Time.deltaTime;
        prog=Mathf.Clamp01(prog+(toB?d:-d));
        if(prog>=1||prog<=0){pausing=true;pt=pause;}
        transform.position=Vector3.Lerp(ptA,ptB,Mathf.SmoothStep(0,1,prog));
    }
}
