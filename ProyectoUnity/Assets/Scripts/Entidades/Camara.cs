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
    float sensibilidad;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        camara = GameObject.FindGameObjectWithTag("MainCamera");
        posicionDeDisparo = GameObject.FindGameObjectWithTag("posicionDisparoBaculo");
        modeloJugador = jugador.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        camaraInterior = camara.transform.GetChild(0).gameObject;

        playerInput = jugador.GetComponent<PlayerInput>();

        rotacion = Vector3.zero;

        Herramientas.perspectiva = false;
        cambioDeCamara = true;
        sensibilidad = 0.3f;

        Renderer[] renders = jugador.GetComponentsInChildren<Renderer>(true);
        huesoBaculo = GameObject.FindGameObjectWithTag("Hueso.ManoR");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Herramientas.perspectiva)
        {
            rotacion.y += playerInput.actions["Look"].ReadValue<Vector2>().x * sensibilidad;
            rotacion.x += playerInput.actions["Look"].ReadValue<Vector2>().y * -sensibilidad;

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
            rotacion.y += playerInput.actions["Look"].ReadValue<Vector2>().x * sensibilidad;
            rotacion.x += playerInput.actions["Look"].ReadValue<Vector2>().y * -sensibilidad;

            if (rotacion.x > 50)
            {
                rotacion.x = 50;
            }
            else if (rotacion.x < -50)
            {
                rotacion.x = -50;
            }

            
            if (cambioDeCamara)
            {
                camaraInterior.GetComponent<Camera>().fieldOfView = 60;
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
            Quaternion rotX = Quaternion.AngleAxis(-rotacion.x, Vector3.up);
            Debug.Log(rotX.eulerAngles);
            Quaternion rotY = Quaternion.AngleAxis(rotacion.y -90, Vector3.up);

            Vector3 rotacionNueva = (rotX).eulerAngles + huesoBaculo.transform.rotation.eulerAngles; //Arreglar que gire bien (ni idea porque no funciona)

            huesoBaculo.transform.rotation = Quaternion.Euler(new Vector3(huesoBaculo.transform.rotation.x, huesoBaculo.transform.rotation.y + rotX.eulerAngles.y, huesoBaculo.transform.rotation.z)); //Hacer que el baculo siga la camara

            posicionDeDisparo.transform.rotation = Quaternion.AngleAxis(rotacion.y, Vector3.up) * Quaternion.AngleAxis(-rotacion.x, Vector3.left);
            posicionDeDisparo.transform.position = huesoBaculo.transform.position;
            transform.rotation = Quaternion.Euler(rotacion);
            transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y + 1.7f, jugador.transform.position.z);
            

        }
    }
}
