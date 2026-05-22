using TMPro;
using UnityEngine;
using static Herramientas;
using UnityEngine.EventSystems;

public class infoItemTienda : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TextMeshProUGUI statsObjeto;
    TextMeshProUGUI nombreObjeto;
    CanvasGroup infoObjeto;
    Jugador jugador;
    Objeto objeto;
    int precio;
    bool setValores;

    void Start()
    {
        infoObjeto = transform.GetChild(3).GetComponent<CanvasGroup>();
        nombreObjeto = statsObjeto = transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        statsObjeto = transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>();
        setValores = true;

        infoObjeto.alpha = 0;
        infoObjeto.interactable = false;
        infoObjeto.blocksRaycasts = false;


    }

    public void RecibirObjeto(Objeto objeto)
    {
        this.objeto = objeto;

        switch (objeto.rareza)
        {
            case 0: precio = 200; break;
            case 1: precio = 350; break;
            case 2: precio = 550; break;
            case 3: precio = 1000; break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover");
        infoObjeto.alpha = 1;
        infoObjeto.interactable = true;

        if (setValores)
        {
            nombreObjeto.text = objeto.nombre;
            statsObjeto.text =
                $"Precio = {precio}\n slime" +
                $"Vida = {objeto.hpMax}\n" +
                $"Mana = {objeto.mpMax}\n" +
                $"Dano Fisico = {objeto.danoFisicoMax}\n" +
                $"Dano Magico = {objeto.danoMagicoMax}\n" +
                $"Defensa Fisica = {objeto.defensaFisicaMax}\n" +
                $"Defensa Magica = {objeto.defensaMagicaMax}\n" +
                $"Velocidad de ataque = {objeto.velocidadDeAtaqueMax}\n" +
                $"Critico = {objeto.critico}";
            setValores = false;
        }
        infoObjeto.transform.SetParent(GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Deshover");
        infoObjeto.alpha = 0;
        infoObjeto.interactable = false;
        infoObjeto.transform.SetParent(this.transform);
    }

    public void IntentarComprar()
    {
        if(jugador.jugadorStats.slimeRecolectado >= precio)
        {
            jugador.jugadorStats.slimeRecolectado -= precio;
            jugador.jugadorStats.AgregarObjeto(objeto);
            jugador.vendedora.GetComponent<Tienda>().RemoverObjeto(objeto);
            for (int i = 0; i < GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.childCount; i++)
            {
                Destroy(GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.GetChild(i).gameObject);
            }
            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarSlime();
            Destroy(gameObject);
        }

    }
}
