using UnityEngine;

public class Goomba2 : MonoBehaviour
{
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


       this.transform.LookAt(new Vector3(jugador.transform.position.x, posicion.y, jugador.transform.position.z));
        

        posicion = Vector3.MoveTowards(posicion, new Vector3(jugador.transform.position.x, posicion.y, jugador.transform.position.z), 1*Time.deltaTime); //Obviamente sigue en todos los ejes

        Debug.Log(Vector3.Distance(this.transform.position, jugador.transform.position)); //Calcula tomando en cuenta todos los ejes
        this.transform.position = posicion;
    }
}
