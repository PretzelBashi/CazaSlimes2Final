using UnityEngine;
using static Herramientas;

public class statDrop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool hpMP;
    public void Start()
    {

        switch (transform.tag)
        {
            case "hpDrop": hpMP = true; break;
            case "mpDrop": hpMP = false; break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Jugador jugador = other.GetComponent<Jugador>();
        if (other.tag == "Player")
        {
            if (hpMP)
            {
                Debug.Log("Curacion");
                jugador.jugadorStats.ActualizarHP((6.5f * jugador.jugadorStats.hpMax) / 100, 2, jugador.jugadorStats.criticoActual, jugador.prefabNumeroDano);
            } else
            {
                jugador.jugadorStats.ActualizarMP((5 * jugador.jugadorStats.mpMax) / 100);
            }
            

            Destroy(gameObject);
        }

    }
}
