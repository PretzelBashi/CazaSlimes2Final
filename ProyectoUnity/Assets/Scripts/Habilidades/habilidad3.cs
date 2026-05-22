using UnityEngine;

public class habilidad3 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform jugador;
    float duracionBuff;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        duracionBuff = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = jugador.position;
        if(duracionBuff < 5)
        {
            duracionBuff += Time.deltaTime;
        } else
        {
            jugador.GetComponent<Jugador>().jugadorStats.ReiniciarStatActual("criticoActual");
            Destroy(gameObject);
        }
    }
}
