using TMPro;
using UnityEngine;
using static Herramientas;
using UnityEngine.EventSystems;

public class infoItemTienda : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TextMeshProUGUI statsObjeto;
    TextMeshProUGUI nombreObjeto;
    CanvasGroup infoObjeto;
    Objeto objeto;
    int precio;
    bool setValores;
    void Start()
    {
        infoObjeto = transform.GetChild(3).GetComponent<CanvasGroup>();
        nombreObjeto = statsObjeto = transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        statsObjeto = transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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

    }
}
