using System;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
     * sen = ca / h
     * 
     * Se despeja para sacar el cateto
     * 
     * sen * h = ca
     * 
     * Con esto podemos calcular el cateto y el adyacente utilizando el angulo y la hipotenusa (la hipotenusa seria la distancia que queremos recorrer por segundo mas timeDeltaTime
     */
    Vector3 posicion;
    GameObject jugador;
    Vector3 rotacion;
    void Start()
    {
        rotacion = Vector3.zero;
        jugador = GameObject.FindGameObjectWithTag("Player");
        posicion = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        rotacion.y = Herramientas.ObtenerAngulo2D(new Vector2(this.transform.position.x,this.transform.position.z), new Vector2(jugador.transform.position.x, jugador.transform.position.z));
        
        this.transform.rotation = Quaternion.Euler(rotacion);

        posicion.x += Mathf.Cos(rotacion.y * Mathf.Deg2Rad) * 1 * Time.deltaTime;
        posicion.z -= Mathf.Sin(rotacion.y * Mathf.Deg2Rad) * 1 * Time.deltaTime;

        Debug.Log(Vector3.Distance(this.transform.position, jugador.transform.position)); //Calcula tomando en cuenta todos los ejes
        this.transform.position = posicion;
    }
}


