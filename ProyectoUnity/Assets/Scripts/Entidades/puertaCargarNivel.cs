using Unity.VisualScripting;
using UnityEngine;

public class puertaCargarNivel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool activado;
    infoPartidaActual infoPartidaActual;
    private void Start()
    {
        activado = false;
        infoPartidaActual = GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !activado)
        {
            infoPartidaActual.EntrarAlCuarto();
            infoPartidaActual.ultimoSpawnPoint = transform.parent.GetChild(1).position;
            activado = true;
            Debug.Log("Puerta activada");
            
        }
    }
}
