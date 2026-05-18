using Unity.VisualScripting;
using UnityEngine;

public class puertaCargarNivel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool activado;
    private void Start()
    {
        activado = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !activado)
        {
            GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>().EntrarAlCuarto();
            activado = true;
            Debug.Log("Puerta activada");
            
        }
    }
}
