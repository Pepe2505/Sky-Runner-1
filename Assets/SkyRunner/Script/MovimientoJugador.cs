using UnityEngine;
using UnityEngine.EventSystems;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadLateral = 10f;    // Movimiento horizontal (X)
    public float velocidadVertical = 10f;   // Movimiento vertical (Y)
    public float minX = -5f;               // Limite izquierdo
    public float maxX = 5f;                // Limite derecho

    [Header("Marcador de destino")]
    public GameObject Prefabdedo;          // Prefab para mostrar el toque

    Camera camara;
    GameObject puntoActual;

    void Awake()
    {
        camara = Camera.main;
    }

    void Update()
    {
        Vector3 movimiento = Vector3.zero;

        // --- M�vil ---
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);
            if (!IsPointerOverUI(toque.fingerId))
            {
                Vector2 posicion = toque.position;

                // Dividir pantalla en 4 cuadrantes:
                // Izquierda/Derecha para X, Arriba/Abajo para Y
                if (posicion.x < Screen.width / 2f)
                    movimiento.x = -velocidadLateral;
                else
                    movimiento.x = velocidadLateral;

                if (posicion.y > Screen.height / 2f)
                    movimiento.y = velocidadVertical;
                else
                    movimiento.y = -velocidadVertical;

                if (toque.phase == TouchPhase.Began)
                    TryShowMarker(posicion);
            }
        }

        // --- PC para pruebas ---
        float inputX = Input.GetAxis("Horizontal"); // A/D o flechas
        float inputY = Input.GetAxis("Vertical");   // W/S o flechas arriba/abajo
        movimiento.x += inputX * velocidadLateral;
        movimiento.y += inputY * velocidadVertical;

        // Aplicar movimiento
        Vector3 pos = transform.position + movimiento * Time.deltaTime;

        // Z fijo
        pos.z = 0f;

        transform.position = pos;

        // Click para probar marcador en PC
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI(-1))
        {
            TryShowMarker(Input.mousePosition);
        }
    }

    bool IsPointerOverUI(int fingerId)
    {
        if (EventSystem.current == null) return false;
        return EventSystem.current.IsPointerOverGameObject(fingerId);
    }

    void TryShowMarker(Vector2 posicion)
    {
        Ray ray = camara.ScreenPointToRay(posicion);
        if (Physics.Raycast(ray, out RaycastHit golpes, Mathf.Infinity))
        {
            ShowMarker(golpes.point);
        }
    }

    void ShowMarker(Vector3 worldPos)
    {
        if (Prefabdedo == null) return;
        if (puntoActual == null) puntoActual = Instantiate(Prefabdedo);
        puntoActual.transform.position = worldPos;
    }
}