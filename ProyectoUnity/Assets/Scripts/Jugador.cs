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
    PlayerInput playerInput;
    Vector3 direccionDash;

    float velocidadPersonaje;
    Animator animator;

    int estadoAnimacion;

    GameObject contenedorCamara;
    Camera camaraInterior;

    float cooldownDash;
    float contadorDash;
    bool dasheando;

    float contadorSalto;
    bool salto;

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
        velocidadPersonaje = 5;
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

        if (animator.GetInteger("Estado") != estadoAnimacion)
        {
            animator.SetInteger("Estado", estadoAnimacion);
            animator.SetTrigger("CambioEstado");
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

        characterController.Move(new Vector3(direccionCamara.x, velocidad.y, direccionCamara.z)  * Time.deltaTime);


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
}
