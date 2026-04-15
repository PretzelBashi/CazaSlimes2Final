using UnityEngine;

public class Slime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject jugador;
    GameObject modeloSlime;
    Vector3 rotacion;

    bool apuntando;
    bool saltando;
    bool cayendo;
    float cooldownSalto;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        rotacion = Vector3.zero;
        modeloSlime = this.transform.GetChild(0).gameObject;
        apuntando = true;
        cayendo = true;
        saltando = true;
        cooldownSalto = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rotacion.y = Herramientas.ObtenerAngulo2D(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(jugador.transform.position.x, jugador.transform.position.z));

        if(apuntando)
        {
            transform.rotation = Quaternion.Euler(rotacion);
        } else
        {
            if(!(cooldownSalto >= 5))
            {
                cooldownSalto += Time.deltaTime;
            } else if ()
            {

            }
        }
        
    }
}
