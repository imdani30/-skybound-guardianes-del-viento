using UnityEngine;
public class AM : MonoBehaviour {
    public static AM I { get; private set; }
    public AudioClip jump, dash, hurt, crystal, checkpoint, portalOpen, portalEnter, done, dead, click, bgm;
    AudioSource _m, _s;
    void Awake() {
        if(I&&I!=this){Destroy(gameObject);return;} I=this; DontDestroyOnLoad(gameObject);
        _m=gameObject.AddComponent<AudioSource>(); _m.loop=true; _m.volume=0.45f;
        _s=gameObject.AddComponent<AudioSource>(); _s.volume=0.8f;
    }
    void Start() { if(bgm){_m.clip=bgm;_m.Play();} }
    void P(AudioClip c) { if(c) _s.PlayOneShot(c, 0.8f); }
    public void Jump()=>P(jump); public void Dash()=>P(dash); public void Hurt()=>P(hurt);
    public void Crystal()=>P(crystal); public void Checkpoint()=>P(checkpoint);
    public void PortalOpen()=>P(portalOpen); public void PortalEnter()=>P(portalEnter);
    public void Done()=>P(done); public void Dead()=>P(dead); public void Click()=>P(click);
}
