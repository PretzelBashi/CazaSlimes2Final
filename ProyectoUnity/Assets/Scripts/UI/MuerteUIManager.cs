using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Herramientas;



public class MuerteUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool datosGuardados;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1;
        datosGuardados = false;
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Slimes derrotados: " + UltimasStats.cantidadSlimes;
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Cantidad de objetos: " + UltimasStats.cantidadObjetos;
        transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Cuevas recorridas: " + UltimasStats.cantidadCuevas;
        transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "Slime recolectado: " + UltimasStats.cantidadSlimeRecolectado;


        DatosDeGuardado.datosDeGuardado.slimesAsesinados += UltimasStats.cantidadSlimes;
        DatosDeGuardado.datosDeGuardado.slimeRecolectado += UltimasStats.cantidadSlimeRecolectado;
        DatosDeGuardado.datosDeGuardado.cuartosRecorridos += UltimasStats.cantidadCuevas;

        if (UltimasStats.cantidadCuevas > DatosDeGuardado.datosDeGuardado.cuartoMasAlto)
        {
            DatosDeGuardado.datosDeGuardado.cuartoMasAlto = UltimasStats.cantidadCuevas;
        }

        for (int i = 0; i< DatosDeGuardado.datosDeGuardado.logros.Length; i++)
        {
            switch (i)
            {
                case 0: if(DatosDeGuardado.datosDeGuardado.slimesAsesinados >= 100) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 1: if (DatosDeGuardado.datosDeGuardado.slimesAsesinados >= 500) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 2: if (DatosDeGuardado.datosDeGuardado.slimesAsesinados >= 700) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 3: if (DatosDeGuardado.datosDeGuardado.slimesAsesinados >= 1000) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 4: if (DatosDeGuardado.datosDeGuardado.slimesAsesinados >= 2000) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;

                case 5: if (UltimasStats.cantidadCuevas >= 25 && Herramientas.dificultad >= 1.7f) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 6: if (UltimasStats.cantidadCuevas >= 50 && Herramientas.dificultad >= 1.3f) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 7: if (UltimasStats.cantidadCuevas >= 100 && Herramientas.dificultad >= 1f) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 8: if (UltimasStats.cantidadCuevas >= 200 && Herramientas.dificultad >= 1f) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;

                case 9: if (UltimasStats.jugador.danoMagicoMax >= 500) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 10: if (UltimasStats.jugador.danoFisicoMax >= 500) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 11: if (UltimasStats.jugador.defensaFisicaMax >= 250 || UltimasStats.jugador.defensaMagicaMax >= 250) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 12: if (UltimasStats.jugador.velocidadDeAtaqueMax >= 50) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;

                case 13: if (UltimasStats.flawless && UltimasStats.cantidadCuevas >= 10) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
                case 14: if (UltimasStats.flawless && UltimasStats.cantidadCuevas >= 20) { DatosDeGuardado.datosDeGuardado.logros[i] = true; } break;
            }
        }

        if (DatosDeGuardado.datosDeGuardado.objetosDesbloqueados == null)
        {
            DatosDeGuardado.datosDeGuardado.objetosDesbloqueados = new List<Objeto>();
        }
        else
        {
            DatosDeGuardado.datosDeGuardado.objetosDesbloqueados.Clear();
        }

        for (int i = 0; i < DatosDeGuardado.datosDeGuardado.logros.Length; i++)
        {
            switch (i)
            {
                case 9:
                    if (DatosDeGuardado.datosDeGuardado.logros[i] == true)
                    {
                        DatosDeGuardado.datosDeGuardado.objetosDesbloqueados.Add(new Objeto(
                            "Baculo Entropico", //Nombre
                            12, //id
                            3, //Rareza
                            0f, //hpMax
                            150f, //mpMax
                            0f, //DanoFisico
                            45f, //DanoMagico
                            0f, //DefFisica
                            3f, //DefMagica
                            0.35f, //VelocidadDeAtaque
                            30f, //Critico
                            0//Saltos
                            ));
                    }
                    break;
                case 10:
                    if (DatosDeGuardado.datosDeGuardado.logros[i] == true)
                    {
                        DatosDeGuardado.datosDeGuardado.objetosDesbloqueados.Add(new Objeto(
                            "Entropia", //Nombre
                            13, //id
                            3, //Rareza
                            150f, //hpMax
                            100f, //mpMax
                            80f, //DanoFisico
                            0f, //DanoMagico
                            45f, //DefFisica
                            5f, //DefMagica
                            0.5f, //VelocidadDeAtaque
                            20f, //Critico
                            2//Saltos
                            ));
                    }
                    break;
                case 11:
                    if (DatosDeGuardado.datosDeGuardado.logros[i] == true)
                    {
                        DatosDeGuardado.datosDeGuardado.objetosDesbloqueados.Add(new Objeto(
                            "Pechera esmaltada de mitrilo", //Nombre
                            14, //id
                            3, //Rareza
                            300f, //hpMax
                            150f, //mpMax
                            0f, //DanoFisico
                            0f, //DanoMagico
                            20f, //DefFisica
                            35f, //DefMagica
                            0f, //VelocidadDeAtaque
                            0f, //Critico
                            0//Saltos
                            ));
                    }
                    break;
                case 12:
                    if (DatosDeGuardado.datosDeGuardado.logros[i] == true)
                    {
                        DatosDeGuardado.datosDeGuardado.objetosDesbloqueados.Add(new Objeto(
                            "Matakrakens", //Nombre
                            15, //id
                            3, //Rareza
                            0f, //hpMax
                            0f, //mpMax
                            40f, //DanoFisico
                            20f, //DanoMagico
                            0f, //DefFisica
                            0f, //DefMagica
                            2f, //VelocidadDeAtaque
                            30f, //Critico
                            0//Saltos
                            ));
                    }
                    break;
            }
        }

        DatosDeGuardado.datosDeGuardado.Guardar();
        datosGuardados = true;
    }

    async public void VolverAInicio()
    {
        this.transform.GetComponent<AudioSource>().Play();
        if (!datosGuardados) return;
        switch (Herramientas.dificultad)
        {
            case 1: await GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(2); break;
            case 1.3f: await GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(3); break;
            case 1.7f: await GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(4); break;
            case 2.3f: 
                if(UltimasStats.cantidadCuevas >= 25)
                {await GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(6);} 
                else { await GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(5); }
                break;
            default: await GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(2); break;
        }
        do { transform.GetChild(9).GetComponent<CanvasGroup>().alpha += Time.deltaTime / 2; await Task.Yield(); Debug.Log("Atorado"); } while (transform.GetChild(9).GetComponent<CanvasGroup>().alpha < 1);
        SceneManager.LoadScene("PantallaInicio");
    }
}
