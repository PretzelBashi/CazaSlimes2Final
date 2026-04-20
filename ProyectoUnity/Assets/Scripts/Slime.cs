using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    // Probar usar una hitbox cilindrica para el jugador en vez del rigidbody

    GameObject jugador;
    GameObject modeloSlime;

    Vector3 rotacion;
    Vector3 velocidad;
    Vector3 velocidadFrontal;

    CharacterController characterController;
    BoxCollider detectorInferior;
    bool apuntando;
    bool saltando;
    bool saltoTerminado;
    bool cayendo;
    bool endlag;
    bool verJugador;
    bool rebotando;
    bool grounded;
    bool preSaltoActivo;

    float duracionApuntado;
    float duracioncayendo;
    float resistencia;
    float preSalto; //Contador para el tiempo que el collider es desactivado al saltar


    float cooldownSalto;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        characterController = transform.GetComponent<CharacterController>();
        detectorInferior = transform.GetComponent<BoxCollider>();
        rotacion = Vector3.zero;
        modeloSlime = this.transform.GetChild(0).gameObject;

        apuntando = true;
        cayendo = true;
        saltando = true;
        saltoTerminado = false;
        verJugador = true;
        rebotando = false;
        grounded = false;
        preSaltoActivo = false;

        duracionApuntado = 3;
        duracioncayendo = 0;
        preSalto = 0;

        resistencia = 5;
        cooldownSalto = 0;
    }

    // Update is called once per frame
    void Update()
    {




        //gravedad

            velocidad.y -= 20 * Time.deltaTime;

            





        //resistencia
        if(!(velocidad.x < -1f || velocidad.x > 1f))
        {
            velocidad.x = 0;
        } else if(velocidad.x < 0)
        {
            velocidad.x += resistencia * Time.deltaTime;
        } else if (velocidad.x > 0)
        {
            velocidad.x -= resistencia * Time.deltaTime;
        }

        if (!(velocidad.z < -2f || velocidad.z > 2f))
        {
            velocidad.z = 0;
        }
        else if (velocidad.z < 0)
        {
            velocidad.z += resistencia * Time.deltaTime;
        }
        else if (velocidad.z > 0)
        {
            velocidad.z -= resistencia * Time.deltaTime;
        }
        // :P



        //Saltos


        if (apuntando)
        {
            modeloSlime.GetComponent<Animator>().SetInteger("Estado", 1);
            modeloSlime.GetComponent<Animator>().SetTrigger("CambioEstado");
            if (cooldownSalto < 3)
            {
                cooldownSalto += 3;
            } else
            {
                saltando = true;
                apuntando = false;
                cooldownSalto = 0;
            }
            rebotando = false;
        }

        if (saltando)
        {
            modeloSlime.GetComponent<Animator>().SetInteger("Estado", 2);
            modeloSlime.GetComponent<Animator>().SetTrigger("CambioEstado");
            saltando = false;
        }
        //:B

        if (grounded)
        {
            velocidad.y = 0;
            resistencia = 25;
        }

        if (saltoTerminado)
        {
            modeloSlime.GetComponent<Animator>().SetInteger("Estado", 3);
            modeloSlime.GetComponent<Animator>().SetTrigger("CambioEstado");
            saltoTerminado = false;
            verJugador = false;
            velocidad.z = 10;
            resistencia = 5;
            velocidad.y = jugador.transform.position.y + 7;
            endlag = true;
            preSaltoActivo = true;
        }

   


        if (preSaltoActivo)
        {
            grounded = false;
            detectorInferior.enabled = false;
            preSalto += Time.deltaTime;

            if (preSalto > 0.2)
            {
                preSalto = 0;
                detectorInferior.enabled = true;
                preSaltoActivo = false;
            }
        }

        if (endlag && grounded && velocidad.y == 0)
        {

            apuntando = true;
            endlag = false;
            verJugador = true;
            rebotando = false;

        }

        if(verJugador)
        {
            rotacion.y = Herramientas.ObtenerAngulo2D(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(jugador.transform.position.x, jugador.transform.position.z)) + 90;
        }



        velocidadFrontal = this.transform.forward * this.velocidad.z + this.transform.right * this.velocidad.x;

        characterController.Move(new Vector3(velocidadFrontal.x, velocidad.y, velocidadFrontal.z) * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotacion), 200 * Time.deltaTime);

        
    }


    //Arreglar que no se suba a tu cabeza
    //Arreglar que en los bordes de plataformas pueda estar grounded
    public void SaltoTerminado()
    {
        saltoTerminado = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.tag);
        if (hit.gameObject.tag == "Player" && endlag && !rebotando)
        {
            Vector3 diferencia = (this.transform.position - jugador.transform.position).normalized * 7;
            velocidad = new Vector3(diferencia.x, velocidad.y, diferencia.z);
            Debug.Log(diferencia);


            velocidad.y = 3;
            rebotando = true;
            saltoTerminado = false;
            verJugador = false;
            endlag = true;
        };
    }

    private void OnTriggerStay(Collider colision)
    {
        if (colision.gameObject.tag == ("Suelo"))
        {
            grounded = true;
        }

        if (colision.gameObject.tag == ("Player"))
        {
            resistencia = 0;
        }

    }
    
    private void OnTriggerExit(Collider other)
    {
        grounded = false;

    }
}
