using UnityEngine;
using static Herramientas;

public class dropFisico : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    infoPartidaActual info;
    Objeto objeto;
    void Start()
    {
        info = GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>();
        switch (transform.tag)
        {
            case "comun": objeto = info.RegresarDrop(0); break;
            case "raro": objeto = info.RegresarDrop(1); break;
            case "legendario": objeto = info.RegresarDrop(2); break;
            case "unico": objeto = info.RegresarDrop(3); break;
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Jugador>().jugadorStats.ImprimirStats();
            other.GetComponent<Jugador>().jugadorStats.AgregarObjeto(objeto);
            other.GetComponent<Jugador>().jugadorStats.ImprimirStats();
        }
        Destroy(gameObject);
    }


}
