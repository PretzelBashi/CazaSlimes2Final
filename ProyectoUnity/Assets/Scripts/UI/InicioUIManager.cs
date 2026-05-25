using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Herramientas;


public class InicioUIManager : MonoBehaviour
{
    public AudioClip seleccion;
    Transform logrosPadre;
    CanvasGroup menuJugar;
    CanvasGroup menuLogros;
    DatosGuardadosLocal datos;

    void Start()
    {
        

        logrosPadre = GameObject.FindGameObjectWithTag("contentLogros").transform;
        datos = new DatosGuardadosLocal();

        menuJugar = transform.GetChild(1).GetComponent<CanvasGroup>();
        menuLogros = transform.GetChild(2).GetComponent<CanvasGroup>();

        logrosPadre.GetComponent<logrosUIInicio>().CargarLogros();
        /*
        for (int i=0;i< logrosPadre.childCount; i++)
        {
            if (datos.logros[i])
            {
                logrosPadre.GetChild(i).GetComponent<TextMeshProUGUI>().color = Color.green;
            }
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void VolverAInicio()
    {

        menuLogros.alpha = 0;
        menuLogros.interactable = false;
        menuLogros.blocksRaycasts = false;

        menuJugar.alpha = 0;
        menuJugar.interactable = false;
        menuJugar.blocksRaycasts = false;
    }
    public void AbrirLogros()
    {
        menuLogros.alpha = 1;
        menuLogros.interactable = true;
        menuLogros.blocksRaycasts = true;
    }
    public void AbrirJugar()
    {
        menuJugar.alpha = 1;
        menuJugar.interactable = true;
        menuJugar.blocksRaycasts = true;
    }
    public void CerrarJuego()
    {
        Application.Quit();
    }
    public void IniciarFacil()
    {
        Herramientas.dificultad = 1f;
        SceneManager.LoadScene("Partida");
    }

    public void IniciarIntermedio()
    {
        Herramientas.dificultad = 1.3f;
        SceneManager.LoadScene("Partida");
    }

    public void IniciarEsquizofrenico()
    {
        Herramientas.dificultad = 1.7f;
        SceneManager.LoadScene("Partida");
    }

    public void IniciarEntropia()
    {
        Herramientas.dificultad = 2.3f;
        SceneManager.LoadScene("Partida");
    }

    public void IniciarTutorial()
    {
        Herramientas.dificultad = 0.7f;
        SceneManager.LoadScene("Partida");
    }
}
