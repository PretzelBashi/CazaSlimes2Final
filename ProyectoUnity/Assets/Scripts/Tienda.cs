
using System.Collections.Generic;
using UnityEngine;
using static Herramientas;

public class Tienda : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    infoPartidaActual infoPartida;
    public List<Objeto> objetosTienda;
    public int id = 0;
    void Start()
    {
        objetosTienda = new List<Objeto>();
        infoPartida = GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>();

        for (int i = 0; i < (infoPartida.partida.cueva + 1) / 2; ++i)
        {
            int drop = Random.Range(0, 101) + ((infoPartida.partida.cueva + 1) / 3);
            if (drop <= 100)
            {
                Debug.Log("Item generado");
                drop = Random.Range(0, 101);
                if (drop < 55) { drop = 0; }
                else if (drop < 85) { drop = 1; }
                else { drop = 2; }
                Debug.Log($"Rareza: {drop}");
                objetosTienda.Add(infoPartida.RegresarDrop(drop));
            }
        }
    }

    public void RemoverObjeto(Objeto objetoRemover)
    {
        objetosTienda.Remove(objetoRemover);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
