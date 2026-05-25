using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using static Herramientas;
using static infoPartidaActual;

public class Slime : MonoBehaviour
{
    // Probar usar una hitbox cilindrica para el jugador en vez del rigidbody

    //Bugs conocidos:
    //Si toca el suelo antes de que termine de saltar, se bugea y no puede reiniciar ciclo de salto
    //El desatorado no funciona

    public class SlimeStats : Stats
    {
        public int tipoSlime;
        public SlimeStats(
            float hpMax,
            float danoFisicoMax,
            float danoMagicoMax,
            float defensaFisicaMax,
            float defensaMagicaMax,
            float velocidadDeAtaqueMax,
            float criticoMax,
            float IFrames)
        {
            this.hpMax = hpMax;
            hpActual = hpMax;

            this.danoFisicoMax = danoFisicoMax;
            danoFisicoActual = danoFisicoMax;

            this.danoMagicoMax = danoMagicoMax;
            danoMagicoActual = danoMagicoMax;

            this.defensaFisicaMax = defensaFisicaMax;
            defensaFisicaActual = defensaFisicaMax;

            this.defensaMagicaMax = defensaMagicaMax;
            defensaMagicaActual = defensaMagicaMax;

            this.velocidadDeAtaqueMax = velocidadDeAtaqueMax;
            velocidadDeAtaqueActual = velocidadDeAtaqueMax;

            this.criticoMax = criticoMax;
            criticoActual = criticoMax;
            this.IFrames = IFrames;
        }
    }

    GameObject jugador;
    Animator modeloSlime;

    Vector3 rotacion;
    Vector3 velocidad;
    Vector3 velocidadFrontal;

    CharacterController characterController;
    bool saltando;
    bool saltoTerminado;
    bool verJugador;
    bool rebotando;
    bool grounded;
    bool preSaltoActivo;
    bool cargado;
    int id;
    float contadorAtorado;
    float resistencia;

    AudioSource efectosSonido;

    Partida partida;
    public SlimeStats slimeStats;
    public GameObject prefabNumeroDano;
    public GameObject prefabProyectil;

    public AudioClip slimeSalto;
    public AudioClip slimeHit;
    public AudioClip slimeDisparo;
    public AudioClip slimeCargando;
    public void IniciarSlime(int id)
    {
        this.id = id;
        jugador = GameObject.FindGameObjectWithTag("Player");
        characterController = transform.GetComponent<CharacterController>();
        rotacion = Vector3.zero;
        modeloSlime = this.transform.GetChild(0).gameObject.GetComponent<Animator>();
        partida = GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>().partida;

        efectosSonido = transform.GetComponent<AudioSource>();
        saltando = true;
        saltoTerminado = false;
        verJugador = true;
        rebotando = false;
        grounded = false;
        preSaltoActivo = false;

        resistencia = 5;
        contadorAtorado = 0;


        switch (id)
        {
            case 0:
                slimeStats = new SlimeStats(7 + ((1.9f + dificultad) * (partida.cueva + 1)), 25 + ((partida.cueva*dificultad) + 1), 0, ((partida.cueva * dificultad) + 1) * 0.8f, ((partida.cueva * dificultad) + 1), (((partida.cueva * dificultad) + 1) * 0.15f) + 0.7f, 3 + (partida.cueva * dificultad), 0.2f);
                slimeStats.padre = this.gameObject;
                slimeStats.tipoSlime = 0;
                break;
            case 1:
                slimeStats = new SlimeStats(7 + ((1.3f + dificultad) * (partida.cueva + 1)), 0, 23 + ((partida.cueva * dificultad) + 1), ((partida.cueva * dificultad) + 1), ((partida.cueva * dificultad) + 1) * 0.8f, (((partida.cueva * dificultad) + 1) * 0.17f) + 0.9f, 5 + (partida.cueva * dificultad), 0.2f);
                slimeStats.padre = this.gameObject;
                slimeStats.tipoSlime = 1;
                break;
            case 2:
                slimeStats = new SlimeStats(10 + ((2.5f + dificultad) * (partida.cueva + 1)), 30 + ((partida.cueva * (dificultad + 0.3f)) + 1), 0, ((partida.cueva * dificultad) + 1) * 1.2f, ((partida.cueva * dificultad) + 1) * 1.2f, (((partida.cueva * dificultad) + 1) * 0.10f) + 0.80f, 2 + (partida.cueva * dificultad), 0.2f);
                slimeStats.padre = this.gameObject;
                slimeStats.tipoSlime = 0;
                break;
        }
        cargado = true;
        Movimiento();
    }
    async public void Movimiento()
    {
        try
        {
            await Task.Delay(3000);
            switch (slimeStats.tipoSlime)
            {
                case 0: try { do { await Salto(); } while (true); } catch { } break;
                case 1:
                    do
                    {
                        RaycastHit hit;
                        Vector3 direccion = jugador.transform.position - this.transform.position;

                        Debug.DrawRay(
                            this.transform.GetChild(3).position,
                            direccion * 20f,
                            Color.red,
                            10f
                        );


                        if (Physics.Raycast(this.transform.GetChild(3).position, direccion, out hit, 20f))
                        {
                            if (hit.transform.tag == "Player") { try { await Disparar(); } catch { } } else { try { await Salto(); } catch { } }
                        }
                        else
                        {
                            try { await Salto(); } catch { }
                        }


                        await Task.Yield();
                    } while (true);
            }
        }
        catch { }
        
    }
    async public Task Disparar()
    {

        modeloSlime.SetInteger("Estado", 1);
        modeloSlime.SetTrigger("CambioEstado");

        await Task.Delay(Mathf.RoundToInt((1f / slimeStats.velocidadDeAtaqueActual * 6) * 1000));



        modeloSlime.speed = slimeStats.velocidadDeAtaqueActual; //Posible optimizacion
        modeloSlime.SetInteger("Estado", 4);
        modeloSlime.SetTrigger("CambioEstado");

        do { await Task.Yield(); } while (!saltoTerminado);
        efectosSonido.PlayOneShot(slimeDisparo);
        Vector3 posicionNueva = new Vector3(jugador.transform.position.x, jugador.transform.position.y+2f, jugador.transform.position.z);
        Instantiate(prefabProyectil, this.transform.GetChild(3).position, Quaternion.LookRotation(posicionNueva - this.transform.position)).GetComponent<proyectilSlimeSimple>().CambiarDano(slimeStats.danoMagicoActual,slimeStats.criticoActual);

        await Task.Delay(200);

        modeloSlime.SetInteger("Estado", 1);
        modeloSlime.SetTrigger("CambioEstado");

        await Task.Delay(200);

        return;
    }
    async public Task Salto()
    {

        modeloSlime.SetInteger("Estado", 1);
        modeloSlime.SetTrigger("CambioEstado");

        await Task.Delay(Mathf.RoundToInt((1f / slimeStats.velocidadDeAtaqueActual * 6) * 1000));

        modeloSlime.speed = slimeStats.velocidadDeAtaqueActual;
        modeloSlime.SetInteger("Estado", 2);
        modeloSlime.SetTrigger("CambioEstado");

        do { await Task.Yield(); } while (!saltoTerminado);
        efectosSonido.PlayOneShot(slimeSalto);
        saltando = true;
        preSaltoActivo = true;
        grounded = false;
        modeloSlime.SetInteger("Estado", 3);
        modeloSlime.SetTrigger("CambioEstado");
        verJugador = false;
        resistencia = 5;

        velocidad.z = 9 + (slimeStats.velocidadDeAtaqueActual * 2);
        if (jugador.transform.position.y < this.transform.position.y) { velocidad.y = 7; }
        else { velocidad.y = (jugador.transform.position.y - this.transform.position.y) + 7; }

        await Task.Delay(200);
        preSaltoActivo = false;

        do { await Task.Yield(); contadorAtorado += Time.deltaTime; } while (!rebotando && !grounded && contadorAtorado < 6);

        if (rebotando)
        {
            do { await Task.Yield(); } while (!grounded);
        }

        rebotando = false;
        saltando = false;
        grounded = true;
        contadorAtorado = 0;

        saltoTerminado = false;
        verJugador = true;
        modeloSlime.SetInteger("Estado", 1);
        modeloSlime.SetTrigger("CambioEstado");
        await Task.Delay(500);
        return;
    }
    void Update()
    {
        if (!cargado) return;
        if (grounded)
        {
            velocidad.y = 0;
            resistencia = 25;
        }
        else
        {
            //gravedad
            if (this.transform.position.y > jugador.transform.position.y - 3)
            {
                velocidad.y -= 17 * Time.deltaTime;
            }
        }

        //resistencia
        if (!(velocidad.x < -1f || velocidad.x > 1f))
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
        /*

        if (apuntando)
        {
            modeloSlime.SetInteger("Estado", 1);
            modeloSlime.SetTrigger("CambioEstado");
            if (cooldownSalto < Mathf.RoundToInt((1f / slimeStats.velocidadDeAtaqueActual)*6))
            {
                cooldownSalto += Time.deltaTime;
            } else
            {
                apuntando = false;
                cooldownSalto = 0;
                modeloSlime.SetInteger("Estado", 2);
                modeloSlime.SetTrigger("CambioEstado");
            }
            rebotando = false;
            modeloSlime.speed = slimeStats.velocidadDeAtaqueActual;
        } else if (saltoTerminado)
        {
            modeloSlime.SetInteger("Estado", 3);
            modeloSlime.SetTrigger("CambioEstado");
            saltoTerminado = false;
            verJugador = false;
            velocidad.z = 9 + (slimeStats.velocidadDeAtaqueActual * 2);
            resistencia = 5;
            if(jugador.transform.position.y < this.transform.position.y) { velocidad.y = 7; } 
            else { velocidad.y = (jugador.transform.position.y - this.transform.position.y) + 7; }
                
            endlag = true;
            preSaltoActivo = true;
        } else if ((endlag && grounded && velocidad.y == 0))
        {

            apuntando = true;
            endlag = false;
            verJugador = true;
            rebotando = false;
            atorado = false;
        }
        //:B
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
        */

        if (verJugador)
        {
            rotacion.y = Herramientas.ObtenerAngulo2D(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(jugador.transform.position.x, jugador.transform.position.z)) + 90;
        }



        velocidadFrontal = this.transform.forward * this.velocidad.z + this.transform.right * this.velocidad.x;

        characterController.Move(new Vector3(velocidadFrontal.x, velocidad.y, velocidadFrontal.z) * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotacion), 400 * Time.deltaTime);

        
    }
    



    //Arreglar que no se suba a tu cabeza
    //Arreglar que en los bordes de plataformas pueda estar grounded
    public void Particulas()
    {
        this.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
    }
    async public void DestruirSlime()
    {
        await GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>().EliminarSlime(this.gameObject);

        Particulas();
        do
        {
            this.transform.localScale -= Vector3.one * Time.deltaTime;
            await Task.Yield();
        } while (this.transform.localScale.x > 0);


        Destroy(gameObject);
    }

    public void SaltoTerminado()
    {
        saltoTerminado = true;
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag == "Player" && saltando && !rebotando)
        {
            GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>().flawless = false;
            Jugador jugador = hit.transform.GetComponent<Jugador>();
            jugador.ReboteConSlime(-(this.transform.position - jugador.transform.position).normalized * 8.5f);
            if(id == 1)
            {
                jugador.jugadorStats.ActualizarHP(slimeStats.danoMagicoActual, 1, slimeStats.criticoActual, prefabNumeroDano);
            } else
            {
                jugador.jugadorStats.ActualizarHP(slimeStats.danoFisicoActual, 0, slimeStats.criticoActual, prefabNumeroDano);
            }

                Vector3 diferencia = (this.transform.position - jugador.transform.position).normalized * 9;
            velocidad = new Vector3(diferencia.x, velocidad.y, diferencia.z);


            velocidad.y = 3;
            rebotando = true;
            /*
            saltoTerminado = false;
            verJugador = false;
            endlag = true;
            */
        };
    }

    private void OnTriggerStay(Collider colision)
    {
        if (colision.gameObject.tag == ("Suelo") && !preSaltoActivo)
        {
            grounded = true;
        }

        if (colision.gameObject.tag == ("Player") || colision.gameObject.tag == ("Slime"))
        {
            resistencia = 0;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        grounded = false;

    }
}
