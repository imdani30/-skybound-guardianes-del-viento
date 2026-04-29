#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// SpriteGenerator — Generates pixel art sprites for Skybound.
/// Menu: SkyBound > Generar Sprites Pixel Art
/// Menu: SkyBound > Aplicar Sprites a la Escena
/// </summary>
public class SpriteGenerator : Editor
{
    [MenuItem("SkyBound/Generar Sprites Pixel Art")]
    public static void GenerateAll()
    {
        EnsureFolders();

        // Aero (20x32)
        Save(AeroIdle(),  "Assets/Sprites/Aero/aero_idle.png",  20, 32);
        Save(AeroRun1(),  "Assets/Sprites/Aero/aero_run1.png",  20, 32);
        Save(AeroRun2(),  "Assets/Sprites/Aero/aero_run2.png",  20, 32);
        Save(AeroJump(),  "Assets/Sprites/Aero/aero_jump.png",  20, 32);
        Save(AeroGlide(), "Assets/Sprites/Aero/aero_glide.png", 20, 32);
        Save(AeroDash(),  "Assets/Sprites/Aero/aero_dash.png",  20, 32);
        Save(AeroHurt(),  "Assets/Sprites/Aero/aero_hurt.png",  20, 32);

        // Neblin (16x16)
        Save(NeblinNormal(), "Assets/Sprites/Enemies/neblin_normal.png", 16, 16);
        Save(NeblinAlert(),  "Assets/Sprites/Enemies/neblin_alert.png",  16, 16);
        Save(NeblinAttack(), "Assets/Sprites/Enemies/neblin_attack.png", 16, 16);

        // Environment
        Save(Crystal(),     "Assets/Sprites/Platforms/crystal.png",     16, 16);
        Save(PortalOff(),   "Assets/Sprites/Platforms/portal_off.png",   24, 32);
        Save(PortalOn(),    "Assets/Sprites/Platforms/portal_on.png",    24, 32);
        Save(Checkpoint(),  "Assets/Sprites/Platforms/checkpoint.png",   12, 24);

        AssetDatabase.Refresh();
        ConfigureSprites();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Sprites Generados",
            "15 sprites creados en Assets/Sprites/\n\nAhora usa:\nSkyBound > Aplicar Sprites a la Escena", "OK");
    }

    [MenuItem("SkyBound/Aplicar Sprites a la Escena")]
    public static void ApplySprites()
    {
        // Aero
        var aero = GameObject.FindGameObjectWithTag("Player");
        if(aero != null)
        {
            ApplySpr(aero, "Assets/Sprites/Aero/aero_idle.png");
            SetActive(aero, "Head",  false);
            SetActive(aero, "Scarf", false);
        }

        // Neblins
        foreach(var nb in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            ApplySpr(nb, "Assets/Sprites/Enemies/neblin_normal.png");
            SetActive(nb, "Eyes",  false);
            SetActive(nb, "Wings", false);
        }

        // Crystals
        foreach(var cr in GameObject.FindGameObjectsWithTag("Crystal"))
        {
            ApplySpr(cr, "Assets/Sprites/Platforms/crystal.png");
            SetActive(cr, "Glow", false);
            var sr = cr.GetComponent<SpriteRenderer>();
            if(sr) sr.color = Color.white;
        }

        // Portal
        var portal = GameObject.Find("PortalFinal");
        if(portal != null) ApplySprWhite(portal, "Assets/Sprites/Platforms/portal_off.png");

        // Checkpoint
        var chk = GameObject.Find("Checkpoint");
        if(chk != null) ApplySprWhite(chk, "Assets/Sprites/Platforms/checkpoint.png");

        Debug.Log("SkyBound: Sprites aplicados!");
        EditorUtility.DisplayDialog("Sprites Aplicados", "Presiona PLAY para ver el resultado.", "OK");
    }

    // ── Helpers ───────────────────────────────────────────────────────────
    static void ApplySpr(GameObject go, string path)
    {
        var sr = go.GetComponent<SpriteRenderer>();
        if(sr == null) return;
        var spr = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if(spr != null) { sr.sprite = spr; sr.color = Color.white; }
    }
    static void ApplySprWhite(GameObject go, string path) => ApplySpr(go, path);
    static void SetActive(GameObject go, string childName, bool active)
    {
        var t = go.transform.Find(childName);
        if(t != null) t.gameObject.SetActive(active);
    }

    // ── Sprite factory ────────────────────────────────────────────────────
    static Color T   = new Color(0,0,0,0);
    static Color BL  = new Color(0.05f,0.05f,0.08f);
    static Color SK  = new Color(0.91f,0.76f,0.58f);
    static Color SKD = new Color(0.75f,0.60f,0.42f);
    static Color HA  = new Color(0.38f,0.22f,0.09f);
    static Color HAL = new Color(0.55f,0.35f,0.14f);
    static Color JA  = new Color(0.18f,0.38f,0.78f);
    static Color JAD = new Color(0.10f,0.25f,0.58f);
    static Color JAL = new Color(0.35f,0.55f,0.92f);
    static Color SC  = new Color(0.95f,0.95f,0.95f);
    static Color SCD = new Color(0.75f,0.75f,0.78f);
    static Color PA  = new Color(0.18f,0.18f,0.32f);
    static Color PAL = new Color(0.28f,0.28f,0.48f);
    static Color BO  = new Color(0.32f,0.20f,0.10f);
    static Color BOL = new Color(0.50f,0.34f,0.18f);
    static Color GO  = new Color(0.45f,0.72f,0.88f);
    static Color GOF = new Color(0.62f,0.45f,0.22f);
    static Color GL  = new Color(0.88f,0.72f,0.30f);
    static Color WI  = new Color(0.65f,0.88f,1.00f);
    static Color NB  = new Color(0.12f,0.08f,0.20f);
    static Color NBM = new Color(0.22f,0.15f,0.35f);
    static Color NBL = new Color(0.35f,0.25f,0.52f);
    static Color EW  = new Color(0.95f,0.95f,1.00f);
    static Color ER  = new Color(1.00f,0.15f,0.10f);
    static Color EG  = new Color(1.00f,0.90f,0.30f);

    // ── AERO IDLE (20x32) ─────────────────────────────────────────────────
    static Color[] AeroIdle()
    {
        int W=20, H=32;
        var p = Blank(W,H);

        // Hair (rows 28-31)
        HLine(p,W,30,7,12,BL); HLine(p,W,31,8,11,HA);
        Px(p,W,31,8,HAL); Px(p,W,31,10,HAL);
        HLine(p,W,29,6,13,HA); Px(p,W,29,7,HAL);
        // Face outline (row 25-28)
        for(int y=24;y<=27;y++) for(int x=4;x<=14;x++) Px(p,W,y,x,SK);
        Px(p,W,27,4,BL); Px(p,W,27,14,BL);
        // Goggles (row 26)
        Px(p,W,26,5,GOF); Px(p,W,26,6,GO); Px(p,W,26,7,GOF);
        Px(p,W,26,9,GOF); Px(p,W,26,10,GO); Px(p,W,26,11,GOF);
        // Eyes (row 24)
        Px(p,W,24,5,BL); Px(p,W,24,6,BL); Px(p,W,24,9,BL); Px(p,W,24,10,BL);
        // Scarf (rows 21-22)
        for(int x=4;x<=13;x++){ Px(p,W,22,x,SC); Px(p,W,21,x,SCD); }
        // Jacket body (rows 14-20)
        for(int y=14;y<=20;y++) for(int x=2;x<=16;x++) Px(p,W,y,x,JA);
        for(int y=14;y<=20;y++){ Px(p,W,y,2,JAD); Px(p,W,y,16,JAD); }
        Px(p,W,18,4,GL); Px(p,W,18,13,GL); // gold accents
        // Arms (rows 12-13)
        for(int x=0;x<=2;x++){ Px(p,W,13,x,JAD); Px(p,W,12,x,SK); }
        for(int x=16;x<=19;x++){ Px(p,W,13,x,JAD); Px(p,W,12,x,SK); }
        // Belt area
        Px(p,W,15,5,SC); Px(p,W,15,10,SC);
        // Pants (rows 7-13)
        for(int y=7;y<=13;y++) for(int x=6;x<=13;x++) Px(p,W,y,x,PA);
        for(int y=7;y<=13;y++){ Px(p,W,y,7,PAL); Px(p,W,y,11,PAL); }
        // Legs split (rows 4-7)
        for(int y=4;y<=7;y++){
            for(int x=4;x<=7;x++) Px(p,W,y,x,PA);
            for(int x=11;x<=14;x++) Px(p,W,y,x,PA);
        }
        // Boots (rows 0-4)
        for(int y=0;y<=3;y++){
            for(int x=3;x<=7;x++) Px(p,W,y,x,BO);
            for(int x=11;x<=15;x++) Px(p,W,y,x,BO);
        }
        Px(p,W,3,4,BOL); Px(p,W,3,12,BOL);
        // Outline pass
        Outline(p,W,H);
        return p;
    }

    static Color[] AeroRun1()
    {
        var p = AeroIdle();
        int W=20;
        // Move left leg forward
        for(int y=0;y<=6;y++) for(int x=3;x<=7;x++) Px(p,W,y,x,T);
        for(int y=1;y<=5;y++) for(int x=2;x<=6;x++) Px(p,W,y,x,BO);
        for(int y=5;y<=9;y++) for(int x=2;x<=6;x++) Px(p,W,y,x,PA);
        return p;
    }

    static Color[] AeroRun2()
    {
        // Mirror of run1
        var src = AeroRun1();
        return FlipH(src, 20, 32);
    }

    static Color[] AeroJump()
    {
        var p = AeroIdle();
        int W=20;
        // Raise legs
        for(int y=0;y<=5;y++) for(int x=3;x<=15;x++) Px(p,W,y,x,T);
        for(int y=6;y<=9;y++){
            for(int x=4;x<=6;x++) Px(p,W,y,x,PA);
            for(int x=12;x<=14;x++) Px(p,W,y,x,PA);
        }
        for(int y=6;y<=8;y++){
            for(int x=4;x<=6;x++) Px(p,W,y,x,BO);
            for(int x=12;x<=14;x++) Px(p,W,y,x,BO);
        }
        return p;
    }

    static Color[] AeroGlide()
    {
        var p = AeroIdle();
        int W=20;
        // Extend arms wide
        for(int y=12;y<=13;y++){
            Px(p,W,y,0,SK); Px(p,W,y,1,SK);
            Px(p,W,y,18,SK); Px(p,W,y,19,SK);
            // Wind trail
            Px(p,W,y,0,WI); Px(p,W,y,19,WI);
        }
        // Tilt body slightly
        Px(p,W,17,8,WI); Px(p,W,17,9,WI);
        return p;
    }

    static Color[] AeroDash()
    {
        var p = AeroIdle();
        int W=20, H=32;
        // Blue wind tint
        for(int i=0;i<p.Length;i++)
            if(p[i].a>0.5f) p[i]=Color.Lerp(p[i],WI,0.3f);
        // Wind trail on left side
        for(int y=10;y<=20;y++) Px(p,W,y,0,new Color(WI.r,WI.g,WI.b,0.6f));
        return p;
    }

    static Color[] AeroHurt()
    {
        var p = AeroIdle();
        Color red = new Color(1f,0.2f,0.2f);
        for(int i=0;i<p.Length;i++)
            if(p[i].a>0.5f) p[i]=Color.Lerp(p[i],red,0.45f);
        return p;
    }

    // ── NEBLIN (16x16) ────────────────────────────────────────────────────
    static Color[] NeblinNormal()
    {
        int W=16,H=16;
        var p=Blank(W,H);
        // Cloud body ellipse
        for(int y=4;y<=12;y++) for(int x=2;x<=13;x++){
            float cx=x-7.5f, cy=y-8f;
            if(cx*cx/25f+cy*cy/20f<1f) Px(p,W,y,x,NBM);
        }
        // Lighter top
        for(int x=5;x<=10;x++) Px(p,W,11,x,NBL);
        for(int x=4;x<=11;x++) Px(p,W,12,x,NBL);
        // Eyes (white)
        Px(p,W,9,5,EW); Px(p,W,9,6,EW);
        Px(p,W,9,9,EW); Px(p,W,9,10,EW);
        Px(p,W,8,5,EW); Px(p,W,8,6,EW);
        Px(p,W,8,9,EW); Px(p,W,8,10,EW);
        // Pupils
        Px(p,W,8,6,NB); Px(p,W,8,10,NB);
        // Wispy tendrils
        Px(p,W,4,5,NBL); Px(p,W,3,4,NBL);
        Px(p,W,4,10,NBL); Px(p,W,3,11,NBL);
        Px(p,W,5,7,NBL); Px(p,W,4,8,NBL);
        Outline(p,W,H);
        return p;
    }

    static Color[] NeblinAlert()
    {
        var p = NeblinNormal();
        int W=16;
        // Eyes turn red and bigger
        for(int y=7;y<=10;y++) for(int x=4;x<=7;x++) Px(p,W,y,x,ER);
        for(int y=7;y<=10;y++) for(int x=8;x<=11;x++) Px(p,W,y,x,ER);
        // Glow pupils
        Px(p,W,9,5,EG); Px(p,W,9,6,EG);
        Px(p,W,9,9,EG); Px(p,W,9,10,EG);
        // Bigger body
        for(int x=3;x<=12;x++) Px(p,W,13,x,NBL);
        return p;
    }

    static Color[] NeblinAttack()
    {
        int W=16,H=16;
        var p=Blank(W,H);
        // Elongated horizontal attack shape
        for(int y=5;y<=10;y++) for(int x=1;x<=14;x++){
            float cx=x-7.5f, cy=y-7.5f;
            if(cx*cx/40f+cy*cy/9f<1f) Px(p,W,y,x,NB);
        }
        for(int y=6;y<=9;y++) for(int x=2;x<=13;x++){
            float cx=x-7.5f, cy=y-7.5f;
            if(cx*cx/35f+cy*cy/6f<1f) Px(p,W,y,x,NBM);
        }
        // Red eyes wide
        Px(p,W,8,10,ER); Px(p,W,8,11,ER);
        Px(p,W,7,10,ER); Px(p,W,7,11,ER);
        Px(p,W,8,11,EG); Px(p,W,7,11,EG);
        Outline(p,W,H);
        return p;
    }

    // ── CRYSTAL (16x16) ───────────────────────────────────────────────────
    static Color[] Crystal()
    {
        Color CB  = new Color(0.05f,0.65f,0.85f);
        Color CBL = new Color(0.40f,0.90f,1.00f);
        Color CBD = new Color(0.02f,0.40f,0.60f);
        Color CGL = new Color(1.00f,1.00f,1.00f);
        int W=16,H=16;
        var p=Blank(W,H);
        // Diamond shape
        int cx=7, cy=7, rx=5, ry=6;
        for(int y=0;y<H;y++) for(int x=0;x<W;x++){
            float dx=Mathf.Abs(x-cx)/(float)rx;
            float dy=Mathf.Abs(y-cy)/(float)ry;
            if(dx+dy<1f){
                Color c = x<cx?CBL:CB;
                if(x==cx&&y>cy) c=CBD;
                Px(p,W,y,x,c);
            }
        }
        Px(p,W,11,5,CGL); Px(p,W,10,5,CGL); // glint
        Outline(p,W,H);
        return p;
    }

    // ── PORTAL OFF (24x32) ────────────────────────────────────────────────
    static Color[] PortalOff()
    {
        Color PF=new Color(0.38f,0.38f,0.45f);
        Color PI=new Color(0.22f,0.22f,0.28f);
        Color PS=new Color(0.52f,0.52f,0.60f);
        int W=24,H=32;
        var p=Blank(W,H);
        // Stone arch
        for(int y=5;y<H;y++) for(int x=0;x<W;x++){
            float cx=x-11.5f, cy=y-12f;
            bool outer=cx*cx/110f+cy*cy/144f<1f;
            bool inner=cx*cx/64f+cy*cy/100f<1f;
            if(outer&&!inner&&y>5) Px(p,W,y,x,(x+y)%3==0?PS:PF);
            else if(inner&&y>5) Px(p,W,y,x,PI);
        }
        // Base pillars
        for(int y=2;y<=6;y++){
            for(int x=1;x<=5;x++) Px(p,W,y,x,PF);
            for(int x=18;x<=22;x++) Px(p,W,y,x,PF);
        }
        Outline(p,W,H);
        return p;
    }

    // ── PORTAL ON (24x32) ─────────────────────────────────────────────────
    static Color[] PortalOn()
    {
        Color PF=new Color(0.45f,0.15f,0.80f);
        Color PI=new Color(0.65f,0.20f,1.00f);
        Color PG=new Color(0.85f,0.60f,1.00f);
        Color PS=new Color(0.55f,0.25f,0.90f);
        int W=24,H=32;
        var p=Blank(W,H);
        for(int y=5;y<H;y++) for(int x=0;x<W;x++){
            float cx=x-11.5f, cy=y-12f;
            bool outer=cx*cx/110f+cy*cy/144f<1f;
            bool inner=cx*cx/64f+cy*cy/100f<1f;
            if(outer&&!inner&&y>5) Px(p,W,y,x,(x+y)%3==0?PS:PF);
            else if(inner&&y>5){
                float t=1f-(cx*cx/64f+cy*cy/100f);
                Px(p,W,y,x,Color.Lerp(PI,PG,t));
            }
        }
        for(int y=2;y<=6;y++){
            for(int x=1;x<=5;x++) Px(p,W,y,x,PF);
            for(int x=18;x<=22;x++) Px(p,W,y,x,PF);
        }
        Outline(p,W,H);
        return p;
    }

    // ── CHECKPOINT (12x24) ────────────────────────────────────────────────
    static Color[] Checkpoint()
    {
        Color FL=new Color(1.00f,0.85f,0.10f);
        Color FLD=new Color(0.80f,0.62f,0.05f);
        Color PO=new Color(0.55f,0.38f,0.22f);
        Color POL=new Color(0.72f,0.52f,0.30f);
        int W=12,H=24;
        var p=Blank(W,H);
        // Pole
        for(int y=0;y<H;y++){ Px(p,W,y,5,PO); Px(p,W,y,6,POL); }
        // Flag
        for(int y=16;y<H;y++) for(int x=6;x<11;x++){
            if(x<11) Px(p,W,y,x,y%2==0?FL:FLD);
        }
        // Base
        for(int x=2;x<10;x++){ Px(p,W,0,x,PO); Px(p,W,1,x,PO); Px(p,W,2,x,POL); }
        Outline(p,W,H);
        return p;
    }

    // ── LOW LEVEL PIXEL HELPERS ───────────────────────────────────────────
    static Color[] Blank(int W, int H)
    {
        var p = new Color[W*H];
        for(int i=0;i<p.Length;i++) p[i]=T;
        return p;
    }
    static void Px(Color[] p, int W, int y, int x, Color c)
    {
        if(x<0||y<0||x>=W||y>=p.Length/W) return;
        p[y*W+x]=c;
    }
    static void HLine(Color[] p, int W, int y, int x0, int x1, Color c)
    { for(int x=x0;x<=x1;x++) Px(p,W,y,x,c); }

    static void Outline(Color[] p, int W, int H)
    {
        var src = (Color[])p.Clone();
        int[] dx={-1,1,0,0}; int[] dy={0,0,-1,1};
        for(int y=0;y<H;y++) for(int x=0;x<W;x++){
            if(src[y*W+x].a>0.5f) continue;
            for(int d=0;d<4;d++){
                int nx=x+dx[d], ny=y+dy[d];
                if(nx<0||ny<0||nx>=W||ny>=H) continue;
                if(src[ny*W+nx].a>0.5f){ p[y*W+x]=BL; break; }
            }
        }
    }

    static Color[] FlipH(Color[] src, int W, int H)
    {
        var dst=new Color[W*H];
        for(int y=0;y<H;y++) for(int x=0;x<W;x++)
            dst[y*W+(W-1-x)]=src[y*W+x];
        return dst;
    }

    static void Save(Color[] pixels, string path, int W, int H)
    {
        var tex=new Texture2D(W,H); tex.filterMode=FilterMode.Point;
        tex.SetPixels(pixels); tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }

    static void ConfigureSprites()
    {
        string[] folders={"Assets/Sprites/Aero","Assets/Sprites/Enemies","Assets/Sprites/Platforms"};
        foreach(string folder in folders){
            string[] guids=AssetDatabase.FindAssets("t:Texture2D",new[]{folder});
            foreach(string guid in guids){
                string ap=AssetDatabase.GUIDToAssetPath(guid);
                var im=AssetImporter.GetAtPath(ap) as TextureImporter; if(im==null)continue;
                im.textureType=TextureImporterType.Sprite;
                im.spritePixelsPerUnit=32f;
                im.filterMode=FilterMode.Point;
                im.textureCompression=TextureImporterCompression.Uncompressed;
                im.alphaIsTransparency=true;
                im.SaveAndReimport();
            }
        }
    }

    static void EnsureFolders()
    {
        string[] dirs={"Assets/Sprites","Assets/Sprites/Aero","Assets/Sprites/Enemies","Assets/Sprites/Platforms"};
        foreach(string d in dirs){
            string[] pts=d.Split('/'); string cur=pts[0];
            for(int i=1;i<pts.Length;i++){
                string nx=cur+"/"+pts[i];
                if(!AssetDatabase.IsValidFolder(nx))AssetDatabase.CreateFolder(cur,pts[i]);
                cur=nx;
            }
        }
    }
}
#endif
