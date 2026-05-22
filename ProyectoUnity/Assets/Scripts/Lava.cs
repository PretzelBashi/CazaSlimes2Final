using UnityEngine;
using static Herramientas;

public class Lava : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Lava activada");
            Jugador jugador = other.GetComponent<Jugador>();

            infoPartidaActual infoPartidaActual = GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>();

            other.transform.GetComponent<CharacterController>().enabled = false;
            other.transform.position = infoPartidaActual.ultimoSpawnPoint;
            other.transform.GetComponent<CharacterController>().enabled = true;


            float dano = (20 * jugador.jugadorStats.hpMax)/100;
            jugador.jugadorStats.ActualizarHP(dano,1,0,jugador.prefabNumeroDano);
        }
    }
}
