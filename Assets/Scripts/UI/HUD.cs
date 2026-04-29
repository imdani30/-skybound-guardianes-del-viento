using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour {
    public Image[] hearts; public Text crystalTxt; public Slider dashBar;
    public GameObject pausePanel, completePanel, deadPanel;
    PC player;
    void OnEnable() {
        if(GM.I==null) return;
        GM.I.OnLives   +=UpdateLives;
        GM.I.OnCrystals+=UpdateCrystals;
        GM.I.OnPause   +=()=>{if(pausePanel)    pausePanel.SetActive(true);};
        GM.I.OnResume  +=()=>{if(pausePanel)    pausePanel.SetActive(false);};
        GM.I.OnComplete+=()=>{if(completePanel) completePanel.SetActive(true);};
        GM.I.OnDead    +=()=>{if(deadPanel)     deadPanel.SetActive(true);};
    }
    void OnDisable() {
        if(GM.I==null) return;
        GM.I.OnLives   -=UpdateLives;
        GM.I.OnCrystals-=UpdateCrystals;
        if(player) player.OnDash-=UpdateDash;
    }
    void Start() {
        player=FindAnyObjectByType<PC>(); if(player) player.OnDash+=UpdateDash;
        if(GM.I!=null){UpdateLives(GM.I.Lives);UpdateCrystals(GM.I.Crystals,GM.I.Total);}
    }
    void UpdateLives(int n){for(int i=0;i<hearts.Length;i++)if(hearts[i])hearts[i].color=i<n?new Color(0.95f,0.15f,0.2f):new Color(0.3f,0.3f,0.3f,0.5f);}
    void UpdateCrystals(int c,int t){if(crystalTxt)crystalTxt.text="CRISTALES: "+c+" / "+t;}
    void UpdateDash(float v){if(dashBar)dashBar.value=v;}
    public void ShowComplete(){if(completePanel)completePanel.SetActive(true);}
}
