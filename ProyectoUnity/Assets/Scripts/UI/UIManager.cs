using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Herramientas;
public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Image barraHP;
    Image barraMP;
    TextMeshProUGUI slimeRecolectado;
    TextMeshProUGUI textoHP;
    TextMeshProUGUI textoMP;

    TextMeshProUGUI textoNivel;
    TextMeshProUGUI textoProfundidad;
    TextMeshProUGUI textoConjurando;

    MagoStats jugador;

    HabilidadUI habilidad1;
    HabilidadUI habilidad2;
    HabilidadUI habilidad3;
    HabilidadUI habilidad4;

    public class HabilidadUI
    {
        public Image marco;
        public Image logo;
        public TextMeshProUGUI cooldown;
        public TextMeshProUGUI costoMana;
        public HabilidadUI(GameObject[] hijos)
        {
            for (int i = 0; i < hijos.Length; i++)
            {
                switch (i)
                {
                    case 0: marco = hijos[i].GetComponent<Image>(); break;
                    case 1: logo = hijos[i].GetComponent<Image>(); break;
                    case 2: cooldown = hijos[i].GetComponent<TextMeshProUGUI>(); break;
                    case 3: costoMana = hijos[i].GetComponent<TextMeshProUGUI>(); break;
                }
                
            }
        }
    }

    public void IniciarUI(MagoStats jugador)
    {
        this.jugador = jugador;
        GameObject[] habilidadesHijos = new GameObject[4];

        foreach (Transform hijo in transform.GetChild(0).transform)
        {

            switch (hijo.name)
            {
                case "habilidad1":
                    for(int i = 0; i < hijo.childCount; i++)
                    {
                        habilidadesHijos[i] = hijo.GetChild(i).gameObject;
                    }
                    habilidad1 = new HabilidadUI(habilidadesHijos);
                    break;

                case "habilidad2":
                    for (int i = 0; i < hijo.childCount; i++)
                    {
                        habilidadesHijos[i] = hijo.GetChild(i).gameObject;
                    }
                    habilidad2 = new HabilidadUI(habilidadesHijos);
                    break;

                case "habilidad3":
                    for (int i = 0; i < hijo.childCount; i++)
                    {
                        habilidadesHijos[i] = hijo.GetChild(i).gameObject;
                    }
                    habilidad3 = new HabilidadUI(habilidadesHijos);
                    break;

                case "habilidad4":
                    for (int i = 0; i < hijo.childCount; i++)
                    {
                        habilidadesHijos[i] = hijo.GetChild(i).gameObject;
                    }
                    habilidad4 = new HabilidadUI(habilidadesHijos);
                    break;
            }
            
                
            foreach (Transform hijoDeHijo in hijo.transform)
            {
                
                
                switch (hijoDeHijo.name)
                {
                    case "barraHP": barraHP = hijoDeHijo.GetComponent<Image>(); break;
                    case "barraMP": barraMP = hijoDeHijo.GetComponent<Image>(); break;
                    case "slimeRecolectado": slimeRecolectado = hijoDeHijo.GetComponent<TextMeshProUGUI>(); break;
                    case "cantidadHP": textoHP = hijoDeHijo.GetComponent<TextMeshProUGUI>(); break;
                    case "cantidadMP": textoMP = hijoDeHijo.GetComponent<TextMeshProUGUI>(); break;

                    case "nivelActual": textoNivel = hijoDeHijo.GetComponent<TextMeshProUGUI>(); break;
                    case "profundidadActual": textoProfundidad = hijoDeHijo.GetComponent<TextMeshProUGUI>(); break;
                    case "textoConjurando": textoConjurando = hijoDeHijo.GetComponent<TextMeshProUGUI>(); break;
                }
            }
        }
        textoConjurando.transform.parent.gameObject.SetActive(false);
        ActualizarStats();
        ActualizarMP();
        ActualizarCooldowns(0);
        ActualizarCooldowns(1);
        ActualizarCooldowns(2);
        ActualizarCooldowns(3);
    }

    // Update is called once per frame
    public void ActualizarMP()
    {
        habilidad1.costoMana.text = jugador.habilidades[0].costoMana.ToString();
        habilidad2.costoMana.text = jugador.habilidades[1].costoMana.ToString(); 
        habilidad3.costoMana.text = jugador.habilidades[2].costoMana.ToString();
        habilidad4.costoMana.text = jugador.habilidades[3].costoMana.ToString(); 
    }
    public void ActualizarStats()
    {
        Debug.Log(jugador.hpActual);
        barraHP.fillAmount = jugador.hpActual/jugador.hpMax;
        barraMP.fillAmount = jugador.mpActual/jugador.mpMax;
        textoHP.text = $"HP: {jugador.hpActual} / {jugador.hpMax}";
        textoMP.text = $"MP: {jugador.mpActual} / {jugador.mpMax}";
    }
    public void ActualizarCooldowns(int habilidad)
    {
        switch (habilidad)
        {
            case 0: habilidad1.cooldown.text = jugador.habilidades[0].cooldownActual.ToString() + "s"; break;
            case 1: habilidad2.cooldown.text = jugador.habilidades[1].cooldownActual.ToString() + "s"; break;
            case 2: habilidad3.cooldown.text = jugador.habilidades[2].cooldownActual.ToString() + "s"; break;
            case 3: habilidad4.cooldown.text = jugador.habilidades[3].cooldownActual.ToString() + "s"; break;
        }
    }

    public void desactivarHabilidad(int habilidad)
    {
        switch (habilidad)
        {
            case 0:
                habilidad1.logo.color = new Color32(104, 104, 104, 255);
                habilidad1.marco.color = new Color32(104, 104, 104, 255);
                break;
            case 1:
                habilidad2.logo.color = new Color32(104, 104, 104, 255);
                habilidad2.marco.color = new Color32(104, 104, 104, 255);
                break;
            case 2:
                habilidad3.logo.color = new Color32(104, 104, 104, 255);
                habilidad3.marco.color = new Color32(104, 104, 104, 255);
                break;
            case 3:
                habilidad4.logo.color = new Color32(104, 104, 104, 255);
                habilidad4.marco.color = new Color32(104, 104, 104, 255);
                break;
        }
    }

    public void activarHabilidad(int habilidad)
    {
        switch (habilidad)
        {
            case 0:
                habilidad1.logo.color = new Color32(255, 255, 255, 255);
                habilidad1.marco.color = new Color32(255, 255, 255, 255);
                break;  
            case 1:
                habilidad2.logo.color = new Color32(255, 255, 255, 255);
                habilidad2.marco.color = new Color32(255, 255, 255, 255);
                break;
            case 2:
                habilidad3.logo.color = new Color32(255, 255, 255, 255);
                habilidad3.marco.color = new Color32(255, 255, 255, 255);
                break;
            case 3:
                habilidad4.logo.color = new Color32(255, 255, 255, 255);
                habilidad4.marco.color = new Color32(255, 255, 255, 255);
                break;
        }
    }

    public void ActualizarNivel(int nivel, int profundidad)
    {
        textoNivel.text = $"Nivel: {nivel}";
        textoProfundidad.text = $"Profundidad: {profundidad}";
    }

    public void toggleConjurando(bool activo)
    {
        textoConjurando.transform.parent.gameObject.SetActive(activo);
    }
}
