using UnityEngine;

public class Slime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject jugador;
    GameObject modeloSlime;
    Vector3 rotacion;
    Vector3 velocidad;

    CharacterController characterController;
    bool apuntando;
    bool saltando;
    bool saltoTerminado;
    bool cayendo;
    bool endlag;
    bool verJugador;

    float duracionApuntado;
    float duracioncayendo;

    float cooldownSalto;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        characterController = transform.GetComponent<CharacterController>();
        rotacion = Vector3.zero;
        modeloSlime = this.transform.GetChild(0).gameObject;
        apuntando = true;
        cayendo = true;
        saltando = true;
        saltoTerminado = false;
        verJugador = true;

        duracionApuntado = 3;
        duracioncayendo = 0;

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
            velocidad.x += 5 * Time.deltaTime;
        } else if (velocidad.x > 0)
        {
            velocidad.x -= 5 * Time.deltaTime;
        }

        if (!(velocidad.z < -2f || velocidad.z > 2f))
        {
            velocidad.z = 0;
        }
        else if (velocidad.z < 0)
        {
            velocidad.z += 5 * Time.deltaTime;
        }
        else if (velocidad.z > 0)
        {
            velocidad.z -= 5 * Time.deltaTime;
        }
        // :P

        if (characterController.isGrounded)
        {
            velocidad.y = -1;
            velocidad.x = 0;
            velocidad.z = 0;
        }

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
        }

        if (saltando)
        {
            modeloSlime.GetComponent<Animator>().SetInteger("Estado", 2);
            modeloSlime.GetComponent<Animator>().SetTrigger("CambioEstado");
            saltando = false;
        }
        //:B

        if (saltoTerminado)
        {
            modeloSlime.GetComponent<Animator>().SetInteger("Estado", 3);
            modeloSlime.GetComponent<Animator>().SetTrigger("CambioEstado");
            saltoTerminado = false;
            verJugador = false;
            velocidad.z = 10;
            velocidad.y = 10;
            endlag = true;
        }


        if(endlag && characterController.isGrounded && velocidad.y == -1)
        {
            apuntando = true;
            endlag = false;
            verJugador = true;
        }

        if(verJugador)
        {
            rotacion.y = Herramientas.ObtenerAngulo2D(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(jugador.transform.position.x, jugador.transform.position.z)) + 90;
        }

        Vector3 direccionFrontal = this.transform.forward * this.velocidad.z + this.transform.right * this.velocidad.x;
        transform.rotation =Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotacion), 200 * Time.deltaTime);
        characterController.Move(new Vector3(direccionFrontal.x, velocidad.y, direccionFrontal.z) * Time.deltaTime);
        
    }

    public void SaltoTerminado()
    {
        saltoTerminado = true;
    }
}
