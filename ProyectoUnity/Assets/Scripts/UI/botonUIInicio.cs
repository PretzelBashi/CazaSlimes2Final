using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class botonUIInicio : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    Image imagen;

    Color colorOriginal;

    void Start()
    {
        imagen = GetComponent<Image>();
        Debug.Log("MATENMET");
        colorOriginal = imagen.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("MATENMET");
        imagen.color =
            new Color32(189,  198, 255,255 );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imagen.color = colorOriginal;
    }
}
