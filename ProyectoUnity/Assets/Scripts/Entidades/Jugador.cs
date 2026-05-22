using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Herramientas;

public class Jugador : MonoBehaviour
{
    public MagoStats jugadorStats;

    CharacterController characterController;
    PlayerInput playerInput;
    Vector3 velocidad;
    Vector3 velocidadRebotando;
    Vector3 direccionDash;

    float velocidadPersonaje;


    int estadoAnimacionCuerpo;
    int estadoAnimacionManos;

    GameObject contenedorCamara;
    Camera camaraInterior;
    Animator animator;

    UIManager uiManager;

    Renderer baculoGema;
    Renderer baculoMango;

    float contadorSalto;
    float cooldownDash;
    float contadorDash;
    float contadorRebotando;
    float resistencia;
    float contadorIFrame;
    public Transform vendedora;

    bool dasheando;
    bool salto;
    bool rebotando;

    bool casteando;
    bool casteoTerminado;
    bool[] habilidadesCargadas;
    bool enRangoTienda;

    public GameObject proyectilDisparo;
    public GameObject proyectilMelee;
    public GameObject proyectilHabilidad2;
    public GameObject circuloInvocacion;
    public GameObject rayoHabilidad4;
    public GameObject prefabNumeroDano;
    void Start()
    {
        jugadorStats = new MagoStats();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        contenedorCamara = GameObject.FindGameObjectWithTag("MainCamera");
        camaraInterior = contenedorCamara.transform.GetChild(0).gameObject.GetComponent<Camera>();

        playerInput = GetComponent<PlayerInput>();

        baculoGema = GameObject.FindGameObjectWithTag("BaculoGema").GetComponent<Renderer>();
        baculoMango = GameObject.FindGameObjectWithTag("BaculoMango").GetComponent<Renderer>();

        velocidad = Vector3.zero;

        salto = false;
        rebotando = false;
        casteando = false;
        casteoTerminado = false;
        enRangoTienda = false;

        habilidadesCargadas = new[] { true, true, true, true };

        velocidadPersonaje = 5;
        contadorRebotando = 0;
        resistencia = 5;
        cooldownDash = 0;
        jugadorStats.padre = this.gameObject;
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uiManager.IniciarUI(jugadorStats);
    }


    void Update()
    {


        //Controlador de animaciones de movimiento
        if (!dasheando)
        {
            if (velocidad.x == 0 && velocidad.z == 0 && characterController.isGrounded)
            {
                estadoAnimacionCuerpo = 1;
            }
            else if (!characterController.isGrounded)
            {
                if (velocidad.y > -1) estadoAnimacionCuerpo = 11;
                else estadoAnimacionCuerpo = 5;
            }
            else if ((velocidad.x != 0 || velocidad.z != 0) && (characterController.isGrounded))
            {

                if (velocidad.z > 0) estadoAnimacionCuerpo = 3;
                else if (velocidad.z < 0) estadoAnimacionCuerpo = 10;

                if (velocidad.x > 0) estadoAnimacionCuerpo = 2;
                else if (velocidad.x < 0) estadoAnimacionCuerpo = 4;

            }
        }
        else if ((velocidad.x != 0 || velocidad.z != 0) && (characterController.isGrounded || dasheando))
        {
            if (velocidad.z > 0) estadoAnimacionCuerpo = 8;
            else if (velocidad.z < 0) estadoAnimacionCuerpo = 6;

            if (velocidad.x > 0) estadoAnimacionCuerpo = 7;
            else if (velocidad.x < 0) estadoAnimacionCuerpo = 9;

        }


        if (characterController.isGrounded)
        {
            velocidad.y = -1;
        }





        if (dasheando)
        {
            velocidad = direccionDash * 5f;
            contadorDash += Time.deltaTime;
        }
        else
        {
            velocidad.x = playerInput.actions["Move"].ReadValue<Vector2>().x * velocidadPersonaje;
            velocidad.z = playerInput.actions["Move"].ReadValue<Vector2>().y * velocidadPersonaje;
            velocidad.y -= 25 * Time.deltaTime;

            if (cooldownDash < 1)
            {
                cooldownDash += Time.deltaTime;
            }
        }

        if (contadorDash > 0.2f && dasheando)
        {
            dasheando = false;
            cooldownDash = 0;
        }

        Vector3 direccionCamara = contenedorCamara.transform.forward * velocidad.z + contenedorCamara.transform.right * velocidad.x;
        Vector3 direccionCamaraY = contenedorCamara.transform.forward;
        direccionCamaraY.y = 0f;

        if (direccionCamaraY != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direccionCamaraY), 300f * Time.deltaTime);
        }

        if (!rebotando)
        {
            characterController.Move(new Vector3(direccionCamara.x, velocidad.y, direccionCamara.z) * Time.deltaTime);
        } else
        {


            velocidadRebotando.y -= 25 * Time.deltaTime;

            if (!(velocidadRebotando.x < -1f || velocidadRebotando.x > 1f))
            {
                velocidadRebotando.x = 0;
            }
            else if (velocidadRebotando.x < 0)
            {
                velocidadRebotando.x += resistencia * Time.deltaTime;
            }
            else if (velocidadRebotando.x > 0)
            {
                velocidadRebotando.x -= resistencia * Time.deltaTime;
            }

            if (!(velocidadRebotando.z < -2f || velocidadRebotando.z > 2f))
            {
                velocidadRebotando.z = 0;
            }
            else if (velocidadRebotando.z < 0)
            {
                velocidadRebotando.z += resistencia * Time.deltaTime;
            }
            else if (velocidadRebotando.z > 0)
            {
                velocidadRebotando.z -= resistencia * Time.deltaTime;
            }

            characterController.Move(velocidadRebotando * Time.deltaTime);
            estadoAnimacionCuerpo = 5;

            if (contadorRebotando < 1)
            {
                contadorRebotando += Time.deltaTime;
            } else
            {
                rebotando = false;
                contadorRebotando = 0;
                velocidadRebotando = Vector3.zero;

            }
        }

        if (animator.GetInteger("Estado") != estadoAnimacionCuerpo)
        {
            animator.SetInteger("Estado", estadoAnimacionCuerpo);
            animator.SetTrigger("CambioEstado");
        }


        if (jugadorStats.habilidades[0].cooldownActual < jugadorStats.habilidades[0].cooldownMax)
        {
            jugadorStats.habilidades[0].cooldownActual += Time.deltaTime;
        } else if (!habilidadesCargadas[0])
        {
            uiManager.activarHabilidad(0);
            habilidadesCargadas[0] = true;
        }

        if (jugadorStats.habilidades[1].cooldownActual < jugadorStats.habilidades[1].cooldownMax)
        {
            jugadorStats.habilidades[1].cooldownActual += Time.deltaTime;
        }
        else if (!habilidadesCargadas[1])
        {
            uiManager.activarHabilidad(1);
            habilidadesCargadas[1] = true;
        }

        if (jugadorStats.habilidades[2].cooldownActual < jugadorStats.habilidades[2].cooldownMax)
        {
            jugadorStats.habilidades[2].cooldownActual += Time.deltaTime;
        }
        else if (!habilidadesCargadas[2])
        {
            uiManager.activarHabilidad(2);
            jugadorStats.ReiniciarStatActual("criticoActual");
            habilidadesCargadas[2] = true;
        }

        if (jugadorStats.habilidades[3].cooldownActual < jugadorStats.habilidades[3].cooldownMax)
        {
            jugadorStats.habilidades[3].cooldownActual += Time.deltaTime;
        }
        else if (!habilidadesCargadas[3])
        {
            uiManager.activarHabilidad(3);
            habilidadesCargadas[3] = true;
        }
    }

    public void Saltar(InputAction.CallbackContext callbackContext)
    {
        //Las acciones tienen 3 estados (Started, performed, canceled)
        if (callbackContext.performed)
        {
            characterController.Move(Vector3.up * 0.1f);
            velocidad.y = 10;
        }

    }

    public void Dash(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed) return;
        if (dasheando == false && (cooldownDash >= 0.7f))
        {
            dasheando = true;
            contadorDash = 0;
            direccionDash = new Vector3(velocidad.x, 0, velocidad.z);
        }
    }

    public void ReboteConSlime(Vector3 velocidadRebote)
    {
        velocidadRebotando = new Vector3(velocidadRebote.x, velocidadRebote.y, velocidadRebote.z);
        rebotando = true;

    }

    //Habilidades

    async public void Disparo()
    {
        if (casteando) return;
        
        baculoGema.material.EnableKeyword("_EMISSION");
        baculoGema.material.SetColor("_EmissionColor", Color.magenta * 5f);

        uiManager.toggleConjurando(true);
        casteando = true;
        casteoTerminado = false;

        animator.SetInteger("EstadoBaculo", 3);
        animator.SetTrigger("CambioBaculo");

        // tiempo de casteo
        await Task.Delay(
            Mathf.RoundToInt((1f / jugadorStats.velocidadDeAtaqueActual) * 1000f));

        uiManager.toggleConjurando(false);
        animator.SetInteger("EstadoBaculo", 5);
        animator.SetTrigger("CambioBaculo");

        //aqui sespera la animacion
        while (!casteoTerminado)
        {
            await Task.Yield();
        }

        baculoGema.material.SetColor("_EmissionColor", Color.black);
        baculoGema.material.DisableKeyword("_EMISSION");

        // AQUI ESTA LA HABILIDAD
        RaycastHit hit;

        Vector3 origen = new Vector3(
            transform.position.x,
            transform.position.y + 1.7f,
            transform.position.z
        );

        Vector3 direccion = camaraInterior.transform.forward;

        Vector3 puntoFinal;

        Debug.DrawRay(
            origen,
            direccion * 20f,
            Color.red,
            10f
        );


        if (Physics.Raycast(origen, direccion, out hit, 20f))
        {
            puntoFinal = hit.point;
        }
        else
        {
            puntoFinal = origen + direccion * 20f;
        }

        Vector3 direccionProyectil = puntoFinal - origen;

        Instantiate(proyectilDisparo, GameObject.FindGameObjectWithTag("posicionDisparoBaculo").transform.GetChild(0).transform.position, Quaternion.LookRotation(direccionProyectil));
        jugadorStats.ActualizarMP((8 * jugadorStats.mpMax) / 100);
        //AQUI TERMINA LA HABILIDAD

        await Task.Delay(Mathf.RoundToInt(((1f / jugadorStats.velocidadDeAtaqueActual) / 2f) * 1000f));

        animator.SetInteger("EstadoBaculo", 2);
        animator.SetTrigger("CambioBaculo");

        casteando = false;
        casteoTerminado = false;
    }

    async public void Habilidad1() //Golpe melee
    {
        if (casteando || (jugadorStats.habilidades[0].cooldownActual < jugadorStats.habilidades[0].cooldownMax) || jugadorStats.mpActual < jugadorStats.habilidades[0].costoMana) return;
        jugadorStats.mpActual -= jugadorStats.habilidades[0].costoMana;
        uiManager.ActualizarStats();
        uiManager.desactivarHabilidad(0);
        habilidadesCargadas[0] = false;
        jugadorStats.habilidades[0].cooldownActual = 0;

        baculoMango.material.EnableKeyword("_EMISSION");
        baculoMango.material.SetColor("_EmissionColor", Color.red * 5f);

        uiManager.toggleConjurando(true);
        casteando = true;
        casteoTerminado = false;

        animator.SetInteger("EstadoBaculo", 3);
        animator.SetTrigger("CambioBaculo");

        // tiempo de casteo
        await Task.Delay(
            Mathf.RoundToInt((1f / jugadorStats.velocidadDeAtaqueActual) * 2000f));

        uiManager.toggleConjurando(false);
        animator.SetInteger("EstadoBaculo", 4);
        animator.SetTrigger("CambioBaculo");

        //aqui sespera la animacion
        while (!casteoTerminado)
        {
            await Task.Yield();
        }

        baculoMango.material.SetColor("_EmissionColor", Color.black);
        baculoMango.material.DisableKeyword("_EMISSION");

        // AQUI ESTA LA HABILIDAD
        RaycastHit hit;

        Vector3 origen = new Vector3(
            transform.position.x,
            transform.position.y + 1.7f,
            transform.position.z
        );

        Vector3 direccion = camaraInterior.transform.forward;

        Vector3 puntoFinal;

        Debug.DrawRay(
            origen,
            direccion * 20f,
            Color.red,
            10f
        );


        if (Physics.Raycast(origen, direccion, out hit, 20f))
        {
            puntoFinal = hit.point;
        }
        else
        {
            puntoFinal = origen + direccion * 20f;
        }

        Vector3 direccionProyectil = puntoFinal - origen;

        Instantiate(proyectilMelee, GameObject.FindGameObjectWithTag("posicionDisparoBaculo").transform.GetChild(0).transform.position, Quaternion.LookRotation(direccionProyectil));
        //AQUI TERMINA LA HABILIDAD

        await Task.Delay(Mathf.RoundToInt(((1f / jugadorStats.velocidadDeAtaqueActual) / 2f) * 500f));

        animator.SetInteger("EstadoBaculo", 2);
        animator.SetTrigger("CambioBaculo");

        casteando = false;
        casteoTerminado = false;
    }

    async public void Habilidad2()
    {
        if (casteando || (jugadorStats.habilidades[1].cooldownActual < jugadorStats.habilidades[1].cooldownMax) || jugadorStats.mpActual < jugadorStats.habilidades[1].costoMana) return;
        jugadorStats.mpActual -= jugadorStats.habilidades[1].costoMana;
        uiManager.ActualizarStats();
        uiManager.desactivarHabilidad(1);
        habilidadesCargadas[1] = false;
        jugadorStats.habilidades[1].cooldownActual = 0;

        baculoGema.material.EnableKeyword("_EMISSION");
        baculoGema.material.SetColor("_EmissionColor", Color.magenta * 5f);

        uiManager.toggleConjurando(true);
        casteando = true;
        casteoTerminado = false;

        animator.SetInteger("EstadoBaculo", 3);
        animator.SetTrigger("CambioBaculo");

        // tiempo de casteo
        await Task.Delay(
            Mathf.RoundToInt((1f / jugadorStats.velocidadDeAtaqueActual) * 3000f));

        uiManager.toggleConjurando(false);
        animator.SetInteger("EstadoBaculo", 5);
        animator.SetTrigger("CambioBaculo");

        //aqui sespera la animacion
        while (!casteoTerminado)
        {
            await Task.Yield();
        }

        baculoGema.material.SetColor("_EmissionColor", Color.black);
        baculoGema.material.DisableKeyword("_EMISSION");

        // AQUI ESTA LA HABILIDAD
        RaycastHit hit;

        Vector3 origen = new Vector3(
            transform.position.x,
            transform.position.y + 1.7f,
            transform.position.z
        );

        Vector3 direccion = camaraInterior.transform.forward;

        Vector3 puntoFinal;

        Debug.DrawRay(
            origen,
            direccion * 20f,
            Color.red,
            10f
        );


        if (Physics.Raycast(origen, direccion, out hit, 20f))
        {
            puntoFinal = hit.point;
        }
        else
        {
            puntoFinal = origen + direccion * 20f;
        }

        Vector3 direccionProyectil = puntoFinal - origen;

        Vector3 posicion =
        GameObject.FindGameObjectWithTag("posicionDisparoBaculo").transform.GetChild(0).position;

        // Centro
        Instantiate(proyectilHabilidad2,posicion,Quaternion.LookRotation(direccionProyectil));

        // Izquierda
        Instantiate(proyectilHabilidad2, posicion,Quaternion.LookRotation(Quaternion.Euler(0, -30, 0) * direccionProyectil
            )
        );
        // Derecha
        Instantiate(proyectilHabilidad2, posicion,Quaternion.LookRotation(Quaternion.Euler(0, 30, 0) * direccionProyectil
            )
        );
        //AQUI TERMINA LA HABILIDAD

        await Task.Delay(Mathf.RoundToInt(((1f / jugadorStats.velocidadDeAtaqueActual) / 2f) * 1000f));

        animator.SetInteger("EstadoBaculo", 2);
        animator.SetTrigger("CambioBaculo");

        casteando = false;
        casteoTerminado = false;
    }

    async public void Habilidad3()
    {
        if (casteando || (jugadorStats.habilidades[2].cooldownActual < jugadorStats.habilidades[2].cooldownMax) || jugadorStats.mpActual < jugadorStats.habilidades[2].costoMana) return;
        jugadorStats.mpActual -= jugadorStats.habilidades[2].costoMana;
        uiManager.ActualizarStats();
        uiManager.desactivarHabilidad(2);
        habilidadesCargadas[2] = false;
        jugadorStats.habilidades[2].cooldownActual = 0;

        baculoGema.material.EnableKeyword("_EMISSION");
        baculoGema.material.SetColor("_EmissionColor", Color.magenta * 5f);

        uiManager.toggleConjurando(true);
        casteando = true;
        casteoTerminado = false;

        animator.SetInteger("EstadoBaculo", 3);
        animator.SetTrigger("CambioBaculo");

        // tiempo de casteo
        await Task.Delay(
            Mathf.RoundToInt((1f / jugadorStats.velocidadDeAtaqueActual) * 5000f));

        uiManager.toggleConjurando(false);

        animator.SetInteger("EstadoBaculo", 4);
        animator.SetTrigger("CambioBaculo");

        //aqui sespera la animacion
        while (!casteoTerminado)
        {
            await Task.Yield();
        }

        baculoGema.material.SetColor("_EmissionColor", Color.black);
        baculoGema.material.DisableKeyword("_EMISSION");

        // AQUI ESTA LA HABILIDAD
        

        Instantiate(circuloInvocacion, GameObject.FindGameObjectWithTag("posicionDisparoBaculo").transform.GetChild(0).transform.position, Quaternion.identity);
        jugadorStats.ActualizarStatsActuales("criticoActual", jugadorStats.criticoActual + 50);
        //AQUI TERMINA LA HABILIDAD

        await Task.Delay(Mathf.RoundToInt(((1f / jugadorStats.velocidadDeAtaqueActual) / 2f) * 1000f));

        animator.SetInteger("EstadoBaculo", 2);
        animator.SetTrigger("CambioBaculo");

        casteando = false;
        casteoTerminado = false;
    }

    async public void Habilidad4()
    {
        if (casteando || (jugadorStats.habilidades[3].cooldownActual < jugadorStats.habilidades[3].cooldownMax) || jugadorStats.mpActual < jugadorStats.habilidades[3].costoMana) return;
        jugadorStats.mpActual -= jugadorStats.habilidades[3].costoMana;
        uiManager.ActualizarStats();
        uiManager.desactivarHabilidad(3);
        habilidadesCargadas[3] = false;
        jugadorStats.habilidades[3].cooldownActual = 0;

        baculoGema.material.EnableKeyword("_EMISSION");
        baculoGema.material.SetColor("_EmissionColor", Color.magenta * 5f);

        uiManager.toggleConjurando(true);
        casteando = true;
        casteoTerminado = false;

        animator.SetInteger("EstadoBaculo", 3);
        animator.SetTrigger("CambioBaculo");

        // tiempo de casteo
        await Task.Delay(
            Mathf.RoundToInt((1f / jugadorStats.velocidadDeAtaqueActual) * 500f));

        uiManager.toggleConjurando(false);
        animator.SetInteger("EstadoBaculo", 5);
        animator.SetTrigger("CambioBaculo");

        //aqui sespera la animacion
        while (!casteoTerminado)
        {
            await Task.Yield();
        }

        baculoGema.material.SetColor("_EmissionColor", Color.black);
        baculoGema.material.DisableKeyword("_EMISSION");

        // AQUI ESTA LA HABILIDAD

        Vector3 posicion =
        GameObject.FindGameObjectWithTag("posicionDisparoBaculo").transform.GetChild(0).position;

        await Instantiate(rayoHabilidad4, posicion, Quaternion.identity).GetComponent<rayoHabilidad4Posicio>().IniciarRayo();

        await Task.Delay(Mathf.RoundToInt(((1f / jugadorStats.velocidadDeAtaqueActual) / 2f) * 1000f));

        animator.SetInteger("EstadoBaculo", 2);
        animator.SetTrigger("CambioBaculo");

        casteando = false;
        casteoTerminado = false;
    }
    public void AnimacionDisparoTerminada()
    {
        casteoTerminado = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "vendedora")
        {
            enRangoTienda = true;
            vendedora = other.transform;
            uiManager.toggleInteractuar(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "vendedora" && enRangoTienda)
        {
            enRangoTienda = false;
            uiManager.toggleInteractuar(false);
            uiManager.DescargarTienda();
        }
    }

    public void InteractuarVendedora()
    {
        if (enRangoTienda)
        {
            uiManager.CargarTienda(vendedora.GetComponent<Tienda>().id);
        }
    }
}
