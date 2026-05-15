using UnityEngine;

public class proyectilSimple : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject prefabExplosion;
    float velocidadAtaque;
    void Start()
    {
        velocidadAtaque = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>().jugadorStats.velocidadDeAtaqueActual;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (velocidadAtaque + 13f) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Instantiate(prefabExplosion, transform.position, Quaternion.identity).GetComponent<explosionGenerica>().Explotar(8, 0.3f);
        
        Destroy(gameObject);
    }
}
