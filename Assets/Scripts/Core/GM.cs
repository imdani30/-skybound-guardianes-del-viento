using UnityEngine;
using System.Collections;
public class GM : MonoBehaviour {
    public static GM I { get; private set; }
    public int lives = 3;
    int _l, _c, _t; bool _p;
    public event System.Action<int> OnLives;
    public event System.Action<int,int> OnCrystals;
    public event System.Action OnPause, OnResume, OnComplete, OnDead;
    void Awake() { if(I&&I!=this){Destroy(gameObject);return;} I=this; DontDestroyOnLoad(gameObject); }
    void Start() { _l=lives; OnLives?.Invoke(_l); }
    void Update() {
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if(kb != null && kb.escapeKey.wasPressedThisFrame) Toggle();
    }
    public void SetTotal(int t) { _t=t; _c=0; OnCrystals?.Invoke(0,t); }
    public void Collect() { _c++; OnCrystals?.Invoke(_c,_t); if(_t>0&&_c>=_t) StartCoroutine(Win()); }
    public void Hit() { _l--; OnLives?.Invoke(_l); if(_l<=0) OnDead?.Invoke(); else FindAnyObjectByType<PC>()?.Respawn(); }
    public void Toggle() { _p=!_p; Time.timeScale=_p?0f:1f; if(_p) OnPause?.Invoke(); else OnResume?.Invoke(); }
    public void TriggerComplete() { StartCoroutine(Win()); }
    IEnumerator Win() { OnComplete?.Invoke(); yield return null; }
    public bool Paused=>_p; public int Lives=>_l; public int Crystals=>_c; public int Total=>_t;
}
