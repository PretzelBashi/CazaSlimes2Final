
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Herramientas;
public class UIManager : MonoBehaviour
{
    public List<Sprite> objetos;
    public List<Sprite> marcosRarezas;
    public List<Sprite> marcosRarezasTienda;

    public List<GameObject> objetosEnMenu;
    public List<GameObject> objetosEnTienda;

    public GameObject objetoPrefab;
    public GameObject objetoTiendaPrefab;
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
    TextMeshProUGUI textoInteractuar;

    MagoStats jugador;

    HabilidadUI habilidad1;
    HabilidadUI habilidad2;
    HabilidadUI habilidad3;
    HabilidadUI habilidad4;

    GameObject menuObjetos;
    GameObject menuStats;
    Transform menuTienda;
    infoPartidaActual infoPartida;

    bool menuTABCargado;
    bool menuTiendaCargado;


    public class HabilidadUI
    {
        public Image marco;
        public Image logo;
        public TextMeshProUGUI cooldown;
        public TextMeshProUGUI costoMana;
        public HabilidadUI(GameObject[] hijos)
        {
            for (int i = 0; i < 4; i++)
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
    private void Start()
    {
        menuTABCargado = false;
        menuObjetos = GameObject.FindGameObjectWithTag("menuObjetos");
        menuStats = GameObject.FindGameObjectWithTag("menuStats");
        menuTienda = GameObject.FindGameObjectWithTag("tiendaUI").transform ;
        infoPartida = GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>();
    }
    
    public void ActualizarMenuStats()
    {

        TextMeshProUGUI[] textos = menuStats.transform.GetChild(2).GetComponentsInChildren<TextMeshProUGUI>();
        for(int i = 0; i < textos.Length; i++)
        {
            switch (i)
            {
               
                case 0: textos[i].text = Mathf.FloorToInt(jugador.hpActual).ToString(); ; break;
                case 1: textos[i].text = Mathf.FloorToInt(jugador.mpActual).ToString(); break;
                case 2: textos[i].text = Mathf.FloorToInt(jugador.danoFisicoActual).ToString(); break;
                case 3: textos[i].text = Mathf.FloorToInt(jugador.danoMagicoActual).ToString(); break;
                case 4: textos[i].text = Mathf.FloorToInt(jugador.defensaFisicaActual).ToString(); break;
                case 5: textos[i].text = Mathf.FloorToInt(jugador.defensaMagicaActual).ToString(); break;
                case 6: textos[i].text = Mathf.FloorToInt(jugador.velocidadDeAtaqueActual).ToString(); break;
                case 7: textos[i].text = Mathf.FloorToInt(jugador.criticoActual).ToString(); break;
                case 8: textos[i].text = Mathf.FloorToInt(jugador.saltosMax).ToString(); break;
            }
        }
    }
    public void DescargarTienda()
    {
        if (menuTiendaCargado)
        {
            Debug.Log("Escondido");
            GameObject.FindGameObjectWithTag("oscurecerUI").GetComponent<CanvasGroup>().alpha = 0;
            menuTienda.GetComponent<CanvasGroup>().alpha = 0f;
            menuTienda.GetComponent<CanvasGroup>().interactable = false;
            menuTienda.GetComponent<CanvasGroup>().blocksRaycasts = false;

            foreach (GameObject objeto in objetosEnTienda)
            {
                Destroy(objeto);
            }

            for (int i = 0; i < GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.childCount; i++)
            {
                Destroy(GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.GetChild(i).gameObject);
            }

            objetosEnTienda.Clear();
            menuTiendaCargado = false;
        }
    }

    public void CargarTienda(int idTienda)
    {
        if (!menuTiendaCargado)
        {
            menuTienda.GetComponent<CanvasGroup>().alpha = 1.0f;
            menuTienda.GetComponent<CanvasGroup>().interactable = true;
            menuTienda.GetComponent<CanvasGroup>().blocksRaycasts = true;

            GameObject.FindGameObjectWithTag("oscurecerUI").GetComponent<CanvasGroup>().alpha = 1;

            for(int i =0; i< infoPartida.tiendasSpawneadas[idTienda].GetComponent<Tienda>().objetosTienda.Count; i++)
            {
                Objeto objeto = infoPartida.tiendasSpawneadas[idTienda].GetComponent<Tienda>().objetosTienda[i];
                Transform content = menuTienda.transform.GetChild(1).GetChild(0).GetChild(0);

                GameObject objetoUI = Instantiate(objetoTiendaPrefab, content);

                Image logo = objetoUI.transform.GetChild(1).GetComponent<Image>();



                objetosEnTienda.Add(logo.transform.parent.gameObject);
                logo.sprite = objetos[objeto.id];
                objetoUI.transform.GetChild(0).GetComponent<Image>().sprite = marcosRarezasTienda[objeto.rareza];
                objetoUI.transform.GetComponent<infoItemTienda>().RecibirObjeto(objeto);
            }

            foreach (Objeto objeto in infoPartida.tiendasSpawneadas[idTienda].GetComponent<Tienda>().objetosTienda)
            {

            }

            menuTiendaCargado = true;
        }
        else
        {

            GameObject.FindGameObjectWithTag("oscurecerUI").GetComponent<CanvasGroup>().alpha = 0;
            menuTienda.GetComponent<CanvasGroup>().alpha = 0f;
            menuTienda.GetComponent<CanvasGroup>().interactable = false;
            menuTienda.GetComponent<CanvasGroup>().blocksRaycasts = false;

            foreach (GameObject objeto in objetosEnTienda)
            {
                Destroy(objeto);
            }

            for (int i = 0; i < GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.childCount; i++)
            {
                Destroy(GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.GetChild(i).gameObject);
            }

            objetosEnTienda.Clear();
            menuTiendaCargado = false;
        }

    }

    public void CargarMenuTAB()
    {
        if (!menuTABCargado)
        {
            GameObject.FindGameObjectWithTag("oscurecerUI").GetComponent<CanvasGroup>().alpha = 1;

            menuObjetos.GetComponent<CanvasGroup>().alpha = 1;
            menuObjetos.GetComponent<CanvasGroup>().interactable = true;
            menuObjetos.GetComponent<CanvasGroup>().blocksRaycasts = true;

            menuStats.GetComponent<CanvasGroup>().alpha = 1;
            menuStats.GetComponent<CanvasGroup>().interactable = true;
            menuStats.GetComponent<CanvasGroup>().blocksRaycasts = true;


            foreach (Objeto objeto in jugador.objetos)
            {
                Transform content = menuObjetos.transform.GetChild(1).GetChild(0).GetChild(0);

                GameObject objetoUI = Instantiate(objetoPrefab, content);

                Image logo = objetoUI.transform.GetChild(1).GetComponent<Image>();

                objetosEnMenu.Add(logo.transform.parent.gameObject);
                logo.sprite = objetos[objeto.id];
                objetoUI.transform.GetChild(0).GetComponent<Image>().sprite = marcosRarezas[objeto.rareza];
                objetoUI.GetComponent<infoItem>().RecibirObjeto(objeto);
            }


            ActualizarMenuStats();

            menuTABCargado = true;
        } else
        {

            GameObject.FindGameObjectWithTag("oscurecerUI").GetComponent<CanvasGroup>().alpha = 0;
            menuObjetos.GetComponent<CanvasGroup>().alpha = 0;
            menuObjetos.GetComponent<CanvasGroup>().interactable = false;
            menuObjetos.GetComponent<CanvasGroup>().blocksRaycasts = false;

            menuStats.GetComponent<CanvasGroup>().alpha = 0;
            menuStats.GetComponent<CanvasGroup>().interactable = false;
            menuStats.GetComponent<CanvasGroup>().blocksRaycasts = false;

            foreach (GameObject objeto in objetosEnMenu)
            {
                Destroy(objeto);
            }

            for (int i = 0; i < GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.childCount; i++)
            {
                Destroy(GameObject.FindGameObjectWithTag("statsObjetosOverlay").transform.GetChild(i).gameObject);
            }

            objetosEnMenu.Clear();
            menuTABCargado = false;
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
                    for(int i = 0; i < 4; i++)
                    {
                        habilidadesHijos[i] = hijo.GetChild(i).gameObject;
                    }
                    habilidad1 = new HabilidadUI(habilidadesHijos);
                    break;

                case "habilidad2":
                    for (int i = 0; i < 4; i++)
                    {
                        habilidadesHijos[i] = hijo.GetChild(i).gameObject;
                    }
                    habilidad2 = new HabilidadUI(habilidadesHijos);
                    break;

                case "habilidad3":
                    for (int i = 0; i < 4; i++)
                    {
                        habilidadesHijos[i] = hijo.GetChild(i).gameObject;
                    }
                    habilidad3 = new HabilidadUI(habilidadesHijos);
                    break;

                case "habilidad4":
                    for (int i = 0; i < 4; i++)
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
                    case "textoInteractuar": textoInteractuar = hijoDeHijo.GetComponent<TextMeshProUGUI>(); break;
                }
            }
        }
        textoConjurando.transform.parent.gameObject.SetActive(false);
        textoInteractuar.transform.parent.gameObject.SetActive(false);

        ActualizarStats();
        ActualizarMP();
        ActualizarSlime();
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

    public void ActualizarSlime()
    {
        slimeRecolectado.text = $"Slime: {jugador.slimeRecolectado}";
    }

    public void ActualizarNivel(int nivel, int profundidad)
    {
        textoNivel.text = $"Nivel: {nivel}";
        textoProfundidad.text = $"Profundidad: {profundidad}";
    }

    public void toggleInteractuar(bool activo)
    {
        textoInteractuar.transform.parent.gameObject.SetActive(activo);
    }
    public void toggleConjurando(bool activo)
    {
        textoConjurando.transform.parent.gameObject.SetActive(activo);
    }
}
