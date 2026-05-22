using System.Threading.Tasks;
using UnityEngine;
using static Herramientas;

public class rayoHabilidad4Posicio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform posicionDisparo;
    public GameObject prefabExplosion;
    Camera camaraInterior;
    MagoStats jugadorStats;
    float contadorExplosion;
    float contadorHabilidad;
    bool rayoIniciado = false;
    bool rayoTerminado;

    async public Task IniciarRayo()
    {
        rayoTerminado = false;
        camaraInterior = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).gameObject.GetComponent<Camera>();

        jugadorStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>().jugadorStats;
        Debug.Log(jugadorStats);
        contadorHabilidad = 0;

        posicionDisparo = GameObject.FindGameObjectWithTag("posicionDisparoBaculo").transform.GetChild(0).transform;
        contadorExplosion = 0;
        rayoIniciado = true;

        do { await Task.Yield(); } while (!rayoTerminado);

        Destroy(gameObject);
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rayoIniciado) return;
        Debug.Log("NATANE");
        RaycastHit hit;

        Vector3 origen = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z
        );

        Vector3 direccion = camaraInterior.transform.forward;

        Vector3 puntoFinal;

        int mascara = ~LayerMask.GetMask("Habiliades","Jugador");
        if (Physics.Raycast(origen, direccion, out hit, 50f, mascara))
        {
            puntoFinal = hit.point;
        }
        else
        {
            puntoFinal = origen + direccion * 50f;
        }

        float distancia = hit.distance;

        transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(30, 30, distancia*(4* 1.352061F));
        transform.GetChild(0).GetChild(1).transform.localScale = new Vector3(30, 30, distancia * (4 * 1.352061F));

        Vector3 direccionProyectil = puntoFinal - origen;
        this.transform.rotation = Quaternion.LookRotation(direccionProyectil);

        if(contadorExplosion < 0.2f)
        {
            contadorExplosion += Time.deltaTime;
        }
        else
        {
            contadorExplosion = 0;
            
            Instantiate(prefabExplosion, puntoFinal, Quaternion.identity).GetComponent<explosionGenerica>().Explotar(14, 0.3f, 0, jugadorStats.danoMagicoActual*0.6f, jugadorStats.criticoActual);
        }

            this.transform.position = posicionDisparo.position;

        if(contadorHabilidad < 4)
        {

            contadorHabilidad += Time.deltaTime;
        } else
        {
            Debug.Log("RAYOTERMINADO");
            rayoTerminado = true;
        }
    }
    // Update is called once per frame
}
