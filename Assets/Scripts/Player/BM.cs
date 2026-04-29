using UnityEngine;

/// <summary>
/// BM (BoundaryManager) — Delimitador invisible del nivel.
/// Si Aero sale del perimetro definido, pierde una vida y regresa al spawn.
/// 
/// COMO USARLO:
/// 1. Selecciona el objeto === SISTEMAS === en la Hierarchy
/// 2. Inspector > Add Component > BM
/// 3. Ajusta los limites (leftLimit, rightLimit, etc.) segun el tamano del nivel
/// 4. El gizmo rojo en la Scene view muestra el perimetro
/// </summary>
public class BM : MonoBehaviour
{
    [Header("Limites del nivel (unidades de mundo)")]
    public float leftLimit   = -12f;
    public float rightLimit  =  40f;
    public float bottomLimit =  -5f;
    public float topLimit    =  20f;

    [Header("Respawn")]
    [Tooltip("Posicion donde reaparece Aero. Se guarda automaticamente al inicio.")]
    public Vector3 spawnPoint = new Vector3(0f, 2f, 0f);

    PC player;
    bool respawning = false;

    void Start()
    {
        player = FindAnyObjectByType<PC>();
        // Guarda la posicion inicial de Aero como punto de spawn
        if(player != null)
            spawnPoint = player.transform.position;
    }

    void Update()
    {
        if(player == null || respawning) return;

        Vector3 pos = player.transform.position;

        bool outOfBounds =
            pos.x < leftLimit  ||
            pos.x > rightLimit ||
            pos.y < bottomLimit||
            pos.y > topLimit;

        if(outOfBounds)
            StartCoroutine(Respawn());
    }

    System.Collections.IEnumerator Respawn()
    {
        respawning = true;

        // Flash rapido antes de reaparecer
        var sr = player.GetComponent<SpriteRenderer>();
        for(int i = 0; i < 6; i++)
        {
            if(sr) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.07f);
        }
        if(sr) sr.enabled = true;

        // Mover al spawn y quitar velocidad
        player.transform.position = spawnPoint;
        var rb = player.GetComponent<Rigidbody2D>();
        if(rb) rb.linearVelocity = Vector2.zero;

        // Perder una vida
        GM.I?.Hit();

        yield return new WaitForSeconds(0.3f);
        respawning = false;
    }

    // Dibuja el perimetro en la vista Scene para que puedas ajustarlo visualmente
    void OnDrawGizmos()
    {
        // Borde rojo = limite del nivel
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.5f);
        Vector3 center = new Vector3(
            (leftLimit + rightLimit)  * 0.5f,
            (bottomLimit + topLimit) * 0.5f,
            0f);
        Vector3 size = new Vector3(
            rightLimit  - leftLimit,
            topLimit - bottomLimit,
            0.1f);
        Gizmos.DrawWireCube(center, size);

        // Punto amarillo = spawn
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spawnPoint, 0.4f);
    }
}
