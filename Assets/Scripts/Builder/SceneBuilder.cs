#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// SceneBuilder — Construye la escena completa de Skybound: Guardianes del Viento
/// Menu: SkyBound > CONSTRUIR ESCENA
/// </summary>
public class SceneBuilder : Editor
{
    [MenuItem("SkyBound/CONSTRUIR ESCENA")]
    public static void Build()
    {
        // Ensure tags exist
        EnsureTag("Player"); EnsureTag("Enemy");
        EnsureTag("Crystal"); EnsureTag("MovingPlatform");

        // New empty scene
        var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
            UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,
            UnityEditor.SceneManagement.NewSceneMode.Single);

        int gLayer = LayerMask.NameToLayer("Ground");  if(gLayer<0) gLayer=0;
        int pLayer = LayerMask.NameToLayer("Player");  if(pLayer<0) pLayer=0;
        int eLayer = LayerMask.NameToLayer("Enemy");   if(eLayer<0) eLayer=0;

        // Camera + sky
        var camGO = new GameObject("Main Camera"); camGO.tag="MainCamera";
        camGO.AddComponent<AudioListener>();
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic=true; cam.orthographicSize=6f;
        cam.clearFlags=CameraClearFlags.SolidColor;
        cam.backgroundColor=new Color(0.53f,0.81f,0.98f);
        camGO.transform.position=new Vector3(4,5,-10);

        // Clouds background
        var cloudFolder=new GameObject("=== NUBES ===");
        float[,] clouds={{-8,14,5,1.2f},{0,16,4,1},{10,15,6,1.4f},{20,17,4,1},{30,14,5,1.2f},{15,13,3.5f,0.9f}};
        for(int i=0;i<clouds.GetLength(0);i++){
            var c=new GameObject("Cloud_"+i); c.transform.SetParent(cloudFolder.transform);
            c.transform.position=new Vector3(clouds[i,0],clouds[i,1],0);
            c.AddComponent<SpriteRenderer>().sprite=Spr(new Color(1,1,1,0.4f),clouds[i,2],clouds[i,3]);
            c.GetComponent<SpriteRenderer>().sortingOrder=-10;
        }

        // Islands
        var platFolder=new GameObject("=== ISLAS FLOTANTES ===");
        float[,] plats={{0,0,12,1.2f},{8,3,5,.9f},{15,2,4,.9f},{-3,4,3,.8f},{11,5,5,.9f},{5,7,3,.8f},{18,6,4,.9f},{11,9,5,.9f},{21,8,3,.8f},{29,11,8,1.2f},{-5,3,2,.7f},{25,6,2,.7f}};
        Color[] ic={new Color(0.29f,0.68f,0.31f),new Color(0.25f,0.58f,0.27f),new Color(0.32f,0.72f,0.34f)};
        for(int i=0;i<plats.GetLength(0);i++){
            float x=plats[i,0],y=plats[i,1],w=plats[i,2],h=plats[i,3];
            var go=new GameObject("Isla_"+(i+1)); go.transform.SetParent(platFolder.transform);
            go.transform.position=new Vector3(x,y,0); go.layer=gLayer;
            go.AddComponent<BoxCollider2D>().size=new Vector2(w,h);
            go.AddComponent<SpriteRenderer>().sprite=Spr(ic[i%3],w,h);
            // Grass top
            var gr=new GameObject("Grass"); gr.transform.SetParent(go.transform);
            gr.transform.localPosition=new Vector3(0,h*0.5f+0.09f,0);
            var gsr=gr.AddComponent<SpriteRenderer>(); gsr.sprite=Spr(new Color(0.38f,0.82f,0.22f),w,0.2f); gsr.sortingOrder=1;
        }

        // Moving platforms
        var movFolder=new GameObject("=== PLATAFORMAS MOVILES ===");
        (Vector3 a,Vector3 b,float s)[] movs={(new Vector3(3,2,0),new Vector3(7,2,0),1.8f),(new Vector3(14,4,0),new Vector3(18,4,0),2f),(new Vector3(23,7,0),new Vector3(27,7,0),2.2f)};
        foreach(var(a,b,s) in movs){
            var go=new GameObject("PlataformaMovil"); go.transform.SetParent(movFolder.transform);
            go.transform.position=a; go.tag="MovingPlatform"; go.layer=gLayer;
            go.AddComponent<BoxCollider2D>().size=new Vector2(3.2f,.7f);
            go.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.45f,0.85f,0.95f),3.2f,.7f);
            var gr=new GameObject("Grass"); gr.transform.SetParent(go.transform); gr.transform.localPosition=new Vector3(0,.43f,0);
            gr.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.38f,0.82f,0.22f),3.2f,.18f);
            var mp=go.AddComponent<MP>(); mp.ptA=a; mp.ptB=b; mp.speed=s;
        }

        // Wind zones
        var windFolder=new GameObject("=== CORRIENTES DE VIENTO ===");
        (Vector3 pos,Vector2 sz,float lift)[] winds={(new Vector3(-2,3,0),new Vector2(1.8f,5),13f),(new Vector3(23,7,0),new Vector2(1.8f,6),15f)};
        foreach(var(pos,sz,lift) in winds){
            var go=new GameObject("CorrienteViento"); go.transform.SetParent(windFolder.transform);
            go.transform.position=pos;
            var col=go.AddComponent<BoxCollider2D>(); col.isTrigger=true; col.size=sz;
            var sr=go.AddComponent<SpriteRenderer>(); sr.sprite=Spr(new Color(0.65f,0.92f,1f,0.25f),sz.x,sz.y); sr.sortingOrder=-2;
            go.AddComponent<WZ>().lift=lift;
        }

        // Crystals
        var crystalFolder=new GameObject("=== CRISTALES DE ENERGIA ===");
        new GameObject("CrystalMgr").AddComponent<CM>();
        Vector2[] cPos={new Vector2(1,1.8f),new Vector2(9,4.8f),new Vector2(16,3.8f),new Vector2(-3,5.8f),new Vector2(12,6.8f),new Vector2(6,8.8f),new Vector2(12,10.8f),new Vector2(22,9.8f)};
        foreach(var p in cPos){
            var go=new GameObject("Cristal"); go.transform.SetParent(crystalFolder.transform);
            go.transform.position=new Vector3(p.x,p.y,0); go.tag="Crystal";
            var col=go.AddComponent<CircleCollider2D>(); col.isTrigger=true; col.radius=.32f;
            var sr=go.AddComponent<SpriteRenderer>(); sr.sprite=Spr(new Color(0.1f,0.9f,0.95f),.52f,.52f); sr.sortingOrder=5;
            var glow=new GameObject("Glow"); glow.transform.SetParent(go.transform);
            glow.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.6f,1f,1f,0.5f),.3f,.3f);
            go.AddComponent<CR>();
        }

        // Neblins
        var enemyFolder=new GameObject("=== NEBLINS ===");
        (Vector2 s,Vector2 a,Vector2 b)[] enms={(new Vector2(9,4.5f),new Vector2(7,4.5f),new Vector2(12,4.5f)),(new Vector2(11,6.5f),new Vector2(9,6.5f),new Vector2(14,6.5f)),(new Vector2(21,9.5f),new Vector2(19,9.5f),new Vector2(24,9.5f))};
        int idx=1;
        foreach(var(sp,pa,pb) in enms){
            var go=new GameObject("Neblin_"+idx++); go.transform.SetParent(enemyFolder.transform);
            go.transform.position=new Vector3(sp.x,sp.y,0); go.tag="Enemy"; go.layer=eLayer;
            var rb=go.AddComponent<Rigidbody2D>(); rb.gravityScale=0; rb.constraints=RigidbodyConstraints2D.FreezeRotation;
            var col=go.AddComponent<CircleCollider2D>(); col.isTrigger=true; col.radius=.45f;
            var sr=go.AddComponent<SpriteRenderer>(); sr.sprite=Spr(new Color(0.35f,0.12f,0.55f),.78f,.78f); sr.sortingOrder=6;
            var eyes=new GameObject("Eyes"); eyes.transform.SetParent(go.transform); eyes.transform.localPosition=new Vector3(0.08f,0.15f,0);
            eyes.AddComponent<SpriteRenderer>().sprite=Spr(new Color(1f,0.2f,0.2f),.32f,.14f);
            var wings=new GameObject("Wings"); wings.transform.SetParent(go.transform); wings.transform.localPosition=new Vector3(0,0.2f,0);
            wings.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.55f,0.25f,0.75f,0.7f),.9f,.35f);
            var wpa=new GameObject("A"); wpa.transform.SetParent(go.transform); wpa.transform.position=new Vector3(pa.x,pa.y,0);
            var wpb=new GameObject("B"); wpb.transform.SetParent(go.transform); wpb.transform.position=new Vector3(pb.x,pb.y,0);
            var ai=go.AddComponent<NB>(); ai.ptA=wpa.transform; ai.ptB=wpb.transform;
            ai.playerMask=LayerMask.GetMask("Player");
        }

        // Checkpoint
        var chk=new GameObject("Checkpoint"); chk.transform.position=new Vector3(13,1.8f,0);
        var chkCol=chk.AddComponent<CircleCollider2D>(); chkCol.isTrigger=true; chkCol.radius=.7f;
        chk.AddComponent<SpriteRenderer>().sprite=Spr(new Color(1f,0.88f,0.1f),.55f,1.3f);
        chk.AddComponent<CP>();

        // Portal
        var portal=new GameObject("PortalFinal"); portal.transform.position=new Vector3(31,13,0);
        var pCol=portal.AddComponent<CircleCollider2D>(); pCol.isTrigger=true; pCol.radius=1f; pCol.enabled=false;
        portal.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.3f,0.3f,0.35f),2f,2.5f);
        portal.AddComponent<PT>();

        // Cloud sea / void
        var vd=new GameObject("MarDeNubes"); vd.transform.position=new Vector3(14,-4,0);
        vd.AddComponent<BoxCollider2D>().isTrigger=true;
        vd.GetComponent<BoxCollider2D>().size=new Vector2(120,2);
        vd.AddComponent<VD>();
        var sea=new GameObject("MarDeNubes_Visual"); sea.transform.position=new Vector3(14,-5,0);
        sea.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.85f,0.92f,1f,0.6f),120,3);

        // Aero
        var aero=BuildAero(pLayer,gLayer);

        // Camera follow
        var cf=camGO.AddComponent<CF>(); cf.target=aero.transform; cf.offY=1.5f;

        // Systems
        var sys=new GameObject("=== SISTEMAS ===");
        sys.AddComponent<GM>(); sys.AddComponent<AM>();

        // HUD
        BuildHUD();

        // Save
        if(!AssetDatabase.IsValidFolder("Assets/Scenes")) AssetDatabase.CreateFolder("Assets","Scenes");
        string path="Assets/Scenes/IslaDelAmanecer.unity";
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene,path);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("SKYBOUND: Guardianes del Viento",
            "Escena construida!\n\nAbre: Assets/Scenes/IslaDelAmanecer.unity\n\nPresiona PLAY\n\nControles:\nA / D = Mover a Aero\nEspacio = Saltar\nEspacio x2 = Doble salto\nShift = Impulso de viento\nEsc = Pausa","JUGAR!");
        Debug.Log("SkyBound: Escena guardada en "+path);
    }

    static GameObject BuildAero(int pLayer, int gLayer)
    {
        var go=new GameObject("Aero"); go.tag="Player"; go.layer=pLayer;
        go.transform.position=new Vector3(0,2,0);
        var sr=go.AddComponent<SpriteRenderer>(); sr.sprite=Spr(new Color(0.18f,0.42f,0.82f),.72f,1.1f); sr.sortingOrder=10;
        var head=new GameObject("Head"); head.transform.SetParent(go.transform); head.transform.localPosition=new Vector3(0,.68f,0);
        head.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.9f,0.76f,0.58f),.52f,.48f);
        var scarf=new GameObject("Scarf"); scarf.transform.SetParent(go.transform); scarf.transform.localPosition=new Vector3(0.22f,0.38f,0);
        scarf.AddComponent<SpriteRenderer>().sprite=Spr(new Color(0.95f,0.22f,0.18f),.55f,.14f);
        var rb=go.AddComponent<Rigidbody2D>(); rb.collisionDetectionMode=CollisionDetectionMode2D.Continuous; rb.constraints=RigidbodyConstraints2D.FreezeRotation; rb.gravityScale=3f;
        go.AddComponent<BoxCollider2D>().size=new Vector2(.62f,1f);
        var tc=go.AddComponent<CircleCollider2D>(); tc.isTrigger=true; tc.radius=.34f;
        var pc=go.AddComponent<PC>(); pc.ground=LayerMask.GetMask("Ground");
        go.AddComponent<PP>();
        return go;
    }

    static void BuildHUD()
    {
        var cgo=new GameObject("Canvas_HUD");
        var canvas=cgo.AddComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay;
        var cs=cgo.AddComponent<CanvasScaler>(); cs.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize; cs.referenceResolution=new Vector2(1920,1080);
        cgo.AddComponent<GraphicRaycaster>();
        var root=new GameObject("HUD"); root.transform.SetParent(cgo.transform,false);
        var hud=root.AddComponent<HUD>();

        // Hearts
        var hp=RT(root.transform,"Hearts",new Vector2(-820,500),new Vector2(220,50));
        var hearts=new Image[3];
        for(int i=0;i<3;i++){var h=RT(hp.transform,"H"+(i+1),new Vector2(-55+i*52,0),new Vector2(40,40));hearts[i]=h.AddComponent<Image>();hearts[i].color=new Color(0.95f,0.15f,0.2f);}
        hud.hearts=hearts;

        // Crystal counter
        var cp=RT(root.transform,"Cristales",new Vector2(0,500),new Vector2(320,50));
        hud.crystalTxt=Txt(cp,"CRISTALES: 0 / 8",22,Color.white);

        // Dash bar
        var dp=RT(root.transform,"DashPanel",new Vector2(-820,455),new Vector2(220,20));
        Txt(RT(dp.transform,"L",new Vector2(-60,0),new Vector2(100,20)),"IMPULSO",14,new Color(0.5f,0.85f,1f));
        var db=RT(dp.transform,"Bar",new Vector2(60,0),new Vector2(140,14));
        var sl=db.AddComponent<Slider>(); sl.value=1f;
        var bg=RT(db.transform,"BG",Vector2.zero,Vector2.zero); Stretch(bg); bg.AddComponent<Image>().color=new Color(0.1f,0.1f,0.2f,0.8f);
        var fill=RT(db.transform,"Fill",Vector2.zero,Vector2.zero); Stretch(fill); fill.AddComponent<Image>().color=new Color(0.2f,0.65f,1f);
        sl.fillRect=fill.GetComponent<RectTransform>(); hud.dashBar=sl;

        // Panels
        hud.pausePanel=MkPanel(root.transform,"Pausa",new Color(0,0,0,.82f),"PAUSA",40,Color.white);
        MkBtn(hud.pausePanel.transform,"Resume",new Vector2(0,20),"REANUDAR",()=>GM.I?.Toggle());
        MkBtn(hud.pausePanel.transform,"Restart",new Vector2(0,-50),"REINICIAR",Reload);

        hud.completePanel=MkPanel(root.transform,"Completo",new Color(0,.2f,0,.88f),"NIVEL COMPLETADO!",36,new Color(0.3f,1f,0.4f));
        Txt(RT(hud.completePanel.transform,"Sub",new Vector2(0,10),new Vector2(600,45)),"Todos los cristales recuperados",20,new Color(0.7f,1f,0.7f));
        MkBtn(hud.completePanel.transform,"Retry",new Vector2(0,-50),"JUGAR DE NUEVO",Reload);

        hud.deadPanel=MkPanel(root.transform,"GameOver",new Color(0.25f,0,0,.9f),"GAME OVER",44,new Color(1f,0.25f,0.25f));
        Txt(RT(hud.deadPanel.transform,"Sub",new Vector2(0,10),new Vector2(500,40)),"Las islas estan en peligro...",20,new Color(1f,0.6f,0.6f));
        MkBtn(hud.deadPanel.transform,"Retry",new Vector2(0,-55),"REINTENTAR",Reload);
    }

    static void Reload(){Time.timeScale=1f;UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);}
    static GameObject MkPanel(Transform p,string n,Color bg,string title,int fs,Color tc){
        var go=new GameObject(n);go.transform.SetParent(p,false);
        var rt=go.AddComponent<RectTransform>();rt.anchorMin=Vector2.zero;rt.anchorMax=Vector2.one;rt.offsetMin=rt.offsetMax=Vector2.zero;
        go.AddComponent<Image>().color=bg;
        Txt(RT(go.transform,"T",new Vector2(0,80),new Vector2(700,90)),title,fs,tc);
        go.SetActive(false);return go;
    }
    static void MkBtn(Transform p,string n,Vector2 pos,string label,UnityEngine.Events.UnityAction act){
        var go=RT(p,n,pos,new Vector2(260,52));go.AddComponent<Image>().color=new Color(1,1,1,.18f);
        go.AddComponent<Button>().onClick.AddListener(act);
        var tgo=RT(go.transform,"L",Vector2.zero,Vector2.zero);Stretch(tgo);Txt(tgo,label,22,Color.white);
    }
    static Text Txt(GameObject go,string text,int size,Color color){
        var t=go.AddComponent<Text>();t.text=text;t.fontSize=size;t.color=color;t.alignment=TextAnchor.MiddleCenter;t.fontStyle=FontStyle.Bold;return t;
    }
    static GameObject RT(Transform parent,string name,Vector2 pos,Vector2 size){
        var go=new GameObject(name);go.transform.SetParent(parent,false);
        var rt=go.AddComponent<RectTransform>();rt.anchorMin=rt.anchorMax=new Vector2(.5f,.5f);rt.anchoredPosition=pos;rt.sizeDelta=size;return go;
    }
    static void Stretch(GameObject go){var rt=go.GetComponent<RectTransform>();rt.anchorMin=Vector2.zero;rt.anchorMax=Vector2.one;rt.offsetMin=rt.offsetMax=Vector2.zero;}
    static Sprite Spr(Color c,float w,float h){
        int pw=Mathf.Max(2,Mathf.RoundToInt(w*32)),ph=Mathf.Max(2,Mathf.RoundToInt(h*32));
        var tex=new Texture2D(pw,ph);var px=new Color[pw*ph];for(int i=0;i<px.Length;i++)px[i]=c;
        tex.SetPixels(px);tex.Apply();tex.filterMode=FilterMode.Point;
        return Sprite.Create(tex,new Rect(0,0,pw,ph),new Vector2(.5f,.5f),32f);
    }
    static void EnsureTag(string tag){
        var tm=new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var tp=tm.FindProperty("tags");
        for(int i=0;i<tp.arraySize;i++)if(tp.GetArrayElementAtIndex(i).stringValue==tag)return;
        tp.InsertArrayElementAtIndex(tp.arraySize);tp.GetArrayElementAtIndex(tp.arraySize-1).stringValue=tag;
        tm.ApplyModifiedPropertiesWithoutUndo();
    }
}
#endif
