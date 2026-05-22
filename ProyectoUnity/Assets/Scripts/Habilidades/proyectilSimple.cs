using UnityEngine;
using static Herramientas;
public class proyectilSimple : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject prefabExplosion;
    MagoStats jugadorStats;
    void Start()
    {
        jugadorStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>().jugadorStats;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (jugadorStats.velocidadDeAtaqueActual + 13f) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(jugadorStats.criticoMax);
        Instantiate(prefabExplosion, transform.position, Quaternion.identity).GetComponent<explosionGenerica>().Explotar(8, 0.3f, 0, jugadorStats.danoMagicoActual, jugadorStats.criticoActual);
        
        Destroy(gameObject);
    }
}
