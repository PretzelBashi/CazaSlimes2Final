using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Jugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CharacterController characterController;
    Vector3 velocidad;
    Vector3 rotacion;
    Vector3 velocidadRebotando;
    PlayerInput playerInput;
    Vector3 direccionDash;

    float velocidadPersonaje;
    Animator animator;

    int estadoAnimacion;

    GameObject contenedorCamara;
    Camera camaraInterior;

    float contadorSalto;
    float cooldownDash;
    float contadorDash;
    float contadorRebotando;
    float resistencia;

    bool dasheando;
    bool salto;
    bool rebotando;




    GameObject auxiliarRotacionCamara;
    void Start()
    {
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        contenedorCamara = GameObject.FindGameObjectWithTag("MainCamera");
        camaraInterior = contenedorCamara.transform.GetChild(0).gameObject.GetComponent<Camera>();
        playerInput = GetComponent<PlayerInput>();

        velocidad = Vector3.zero;
        rotacion = Vector3.zero;
        
        salto = false;
        rebotando = false;
        velocidadPersonaje = 5;
        contadorRebotando = 0;
        resistencia = 5;
        cooldownDash = 0;
    }


    void Update()
    {


        //Controlador de animaciones de movimiento
        if (!dasheando)
        {
            if (velocidad.x == 0 && velocidad.z == 0 && characterController.isGrounded)
            {
                estadoAnimacion = 1;
            }
            else if (!characterController.isGrounded)
            {
                if (velocidad.y > -1) estadoAnimacion = 11;
                else estadoAnimacion = 5;
            }
            else if ((velocidad.x != 0 || velocidad.z != 0) && (characterController.isGrounded))
            {

                if (velocidad.z > 0) estadoAnimacion = 3;
                else if (velocidad.z < 0) estadoAnimacion = 10;

                if (velocidad.x > 0) estadoAnimacion = 2;
                else if (velocidad.x < 0) estadoAnimacion = 4;

            }
        }
        else if ((velocidad.x != 0 || velocidad.z != 0) && (characterController.isGrounded || dasheando))
        {
            if (velocidad.z > 0) estadoAnimacion = 8;
            else if (velocidad.z < 0) estadoAnimacion = 6;

            if (velocidad.x > 0) estadoAnimacion = 7;
            else if (velocidad.x < 0) estadoAnimacion = 9;

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
            estadoAnimacion = 5;

            if (contadorRebotando < 1)
            {
                contadorRebotando += Time.deltaTime;
            } else
            {
                rebotando = false;
                contadorRebotando = 0;
                velocidadRebotando = Vector3.zero;
                Debug.Log("Rebotando Reset");
            }
        }

        if (animator.GetInteger("Estado") != estadoAnimacion)
        {
            animator.SetInteger("Estado", estadoAnimacion);
            animator.SetTrigger("CambioEstado");
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
            direccionDash = new Vector3(velocidad.x, 0 , velocidad.z);
        }
    }

    public void ReboteConSlime(Vector3 velocidadRebote)
    {
        velocidadRebotando = new Vector3(velocidadRebote.x, velocidadRebote.y, velocidadRebote.z);
        rebotando = true;
        Debug.Log(velocidadRebotando);
    }
}
