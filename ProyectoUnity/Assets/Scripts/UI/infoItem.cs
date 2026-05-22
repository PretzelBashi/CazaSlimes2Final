using TMPro;

using UnityEngine;


using UnityEngine.EventSystems;
using static Herramientas;

public class infoItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TextMeshProUGUI statsObjeto;
    TextMeshProUGUI nombreObjeto;
    CanvasGroup infoObjeto;
    Objeto objeto;
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        infoObjeto.alpha = 1;
        infoObjeto.interactable = true;

        if (setValores)
        {
            nombreObjeto.text = objeto.nombre;
            statsObjeto.text =
                $"Vida = {objeto.hpMax}\n" +
                $"Mana = {objeto.mpMax}\n" +
                $"Dano Fisico = {objeto.danoFisicoMax}\n" +
                $"Dano Magico = {objeto.danoMagicoMax}\n" +
                $"Defensa Fisica = {objeto.defensaFisicaMax}\n" +
                $"Defensa Magica = {objeto.defensaMagicaMax}\n" +
                $"Velocidad de ataque = {objeto.velocidadDeAtaqueMax}\n" +
                $"Critico = {objeto.critico}\n" +
                $"Saltos = {objeto.saltosMax}";
            setValores = false;
        }
        infoObjeto.transform.SetParent(GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        infoObjeto.alpha = 0;
        infoObjeto.interactable = false;
        infoObjeto.transform.SetParent(this.transform);
    }
}
