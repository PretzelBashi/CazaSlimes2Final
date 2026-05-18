using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Camara : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject jugador;
    GameObject camara;
    GameObject camaraInterior;
    Renderer modeloJugador;
    GameObject huesoBaculo;
    GameObject posicionDeDisparo;

    PlayerInput playerInput;

    Vector3 rotacion;

    bool cambioDeCamara;
    float sensibilidadMouse;
    float sensibilidadControl;
    public bool controlador; //false es mouse, true es control
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        camara = GameObject.FindGameObjectWithTag("MainCamera");
        posicionDeDisparo = GameObject.FindGameObjectWithTag("posicionDisparoBaculo");
        modeloJugador = jugador.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        camaraInterior = camara.transform.GetChild(0).gameObject;

        playerInput = jugador.GetComponent<PlayerInput>();

        rotacion = Vector3.zero;
        controlador = true;

        Herramientas.perspectiva = false;
        cambioDeCamara = true;
        sensibilidadMouse = 0.3f;
        sensibilidadControl = 1.5f;

        Renderer[] renders = jugador.GetComponentsInChildren<Renderer>(true);
        huesoBaculo = GameObject.FindGameObjectWithTag("Hueso.ManoR");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (controlador)
        {
            rotacion.y += playerInput.actions["Look"].ReadValue<Vector2>().x * sensibilidadMouse;
            rotacion.x += playerInput.actions["Look"].ReadValue<Vector2>().y * -sensibilidadMouse;
        }
        else
        {
            rotacion.y += playerInput.actions["Look"].ReadValue<Vector2>().x * sensibilidadControl;
            rotacion.x += playerInput.actions["Look"].ReadValue<Vector2>().y * -sensibilidadControl;
        }

        if (Herramientas.perspectiva)
        {

            if (rotacion.x > 50)
            {
                rotacion.x = 50;
            }
            if (rotacion.x < -50)
            {
                rotacion.x = -50;
            }

            if (cambioDeCamara)
            {
                camaraInterior.GetComponent<Camera>().fieldOfView = 60;
                rotacion.y = -5.7f;
                camaraInterior.transform.position = new Vector3(transform.position.x + 1.36f, transform.position.y, transform.position.z + -2.15f);
                cambioDeCamara = false;
            }

            transform.rotation = Quaternion.Euler(rotacion);
            transform.position = new Vector3 (jugador.transform.position.x, jugador.transform.position.y + 1.7f, jugador.transform.position.z);


        } else
        {


            if (rotacion.x > 55)
            {
                rotacion.x = 55;
            }
            else if (rotacion.x < -90)
            {
                rotacion.x = -90;
            }

            
            if (cambioDeCamara)
            {
                camaraInterior.GetComponent<Camera>().fieldOfView = 90;
                camaraInterior.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                cambioDeCamara = false;
                
                Renderer[] renders = jugador.GetComponentsInChildren<Renderer>(true);

                foreach (Renderer rend in renders)
                {
                    if (rend.transform.name != "Mango" && rend.transform.name != "Gema" && rend.transform.name != "Mono")
                    {
                        rend.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                        rend.receiveShadows = false;
                    }
                }
                
            }

            Quaternion animRotation = huesoBaculo.transform.localRotation;

            Quaternion pitch =
                Quaternion.AngleAxis(-rotacion.x+10, Vector3.up);

            // inclinacion fija hacia la derecha
            Quaternion offset =
                Quaternion.AngleAxis(-20f, Vector3.forward);

            huesoBaculo.transform.localRotation =
                animRotation * pitch * offset;

            posicionDeDisparo.transform.rotation = huesoBaculo.transform.rotation; //ajustar rotacion del puto origen de disparo
            posicionDeDisparo.transform.position = huesoBaculo.transform.position;
            transform.rotation = Quaternion.Euler(rotacion);
            transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y + 1.7f, jugador.transform.position.z);
            

        }
    }
}
