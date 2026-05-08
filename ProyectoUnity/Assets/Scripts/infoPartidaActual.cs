using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class infoPartidaActual : MonoBehaviour
{
    public class Stats
    {
        float hpMax = 100;
        float hpActual = 100;

        float mpMax = 100;
        float mpActual = 100;

        float dañoFisicoMax = 5;
        float dañoFisicoActual = 5;

        float dañoMagicoMax = 5;
        float dañoMagicoActual = 5;

        float defensaFisicaMax = 5;
        float defensaFisicaActual = 5;

        float defensaMagicaMax = 5;
        float defensaMagicaActual = 5;

        float critico = 5;

        //Falta aplicar daño critico y todo eso
        public void ActualizarHP(float dañoARecibir, int tipoDeDaño)
        {
            float vidaFinal = 0;

            switch (tipoDeDaño) //0 es fisico, 1 es magico y 2 es curacion
            {
                case 0: vidaFinal = hpActual - (dañoARecibir - defensaFisicaActual); break;
                case 1: vidaFinal = hpActual - (dañoARecibir - defensaFisicaActual); break;
                case 2: vidaFinal = hpActual + dañoARecibir; break;
            }

            if(vidaFinal > hpMax)
            {
                hpActual = hpMax;
            } 
            else if (vidaFinal > 0)
            {
                hpActual = vidaFinal;
            }
            else
            {
                Debug.Log("Muerte");
            }
        }

        public void ActualizarMP(float mpRecibido) //Pueden ser negativos
        {
            float mpFinal = 0;
            mpFinal = mpActual + mpRecibido;

            if (mpFinal > mpMax)
            {
                mpActual = mpActual;
            }
            else if (mpFinal > 0)
            {
                mpActual = mpFinal;
            }
            else
            {
                mpActual = 0;
            }
        }

        public void ActualizarStatsMaximas(float hpMax, float mpMax, float dañoFisicoMax, float dañoMagicoMax, float defensaFisicaMax, float defensaMagicaMax, bool SumarOAbsoluto)
        {
            if (SumarOAbsoluto) //Sumas
            {
                this.hpMax += hpMax;
                this.mpMax += mpMax;
                this.dañoFisicoMax += dañoFisicoMax;
                this.dañoMagicoMax += dañoMagicoMax;
                this.defensaFisicaMax += defensaFisicaMax;
                this.defensaMagicaMax += defensaMagicaMax;
            } else //Valores absolutos (Por si acaso)
            {
                if (hpMax > 0) { this.hpMax = hpMax; }
                if (mpMax > 0) { this.mpMax = mpMax; }
                if (dañoFisicoMax > 0) { this.dañoFisicoMax = dañoFisicoMax; }
                if (dañoMagicoMax > 0) { this.dañoMagicoMax = dañoMagicoMax; }
                if (defensaFisicaMax > 0) { this.defensaFisicaMax = defensaFisicaMax; }
                if (defensaMagicaMax > 0) { this.defensaMagicaMax = defensaMagicaMax; }
            }
        }

        public void ActualizarStatsActuales(string statACambiar, float nuevoValor)
        {
            switch (statACambiar)
            {
                case "dañoFisico": dañoFisicoActual = nuevoValor; break;
                case "dañoMagico": dañoMagicoActual = nuevoValor; break;
                case "defensaFisica": defensaFisicaActual = nuevoValor; break;
                case "defensaMagica": defensaMagicaActual = nuevoValor; break;
            }
        }

        public void ReiniciarStatActual(string statAReiniciar)
        {
            switch(statAReiniciar)
            {
                case "hp": hpActual = hpMax; break;
                case "mp": mpActual = mpMax; break;
                case "dañoFisico": dañoFisicoActual = dañoFisicoMax; break;
                case "dañoMagico": dañoMagicoActual = dañoMagicoMax; break;
                case "defensaFisica": defensaFisicaActual = defensaFisicaMax; break;
                case "defensaMagica": defensaMagicaActual = defensaMagicaMax; break;
            }
        }
    }

    public class Partida
    {
        public int cueva; //nivel actual a cargar
        public int profundidad;
        
        public Partida()
        {
            cueva = 0;
            profundidad = 0;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Stats jugador;
    Partida partida;

    List<GameObject> spawnersDeSlimes;
    List<Vector3> ordenDeCoordenadas;
    ImpresoraDeSlimes impresoraDeSlimes;
    CreadorDeEscenarios creadorDeEscenarios;
    void Start()
    {
        jugador = new Stats();
        partida = new Partida();
        creadorDeEscenarios = GameObject.FindGameObjectWithTag("CreadorDeCuevas").GetComponent<CreadorDeEscenarios>();
        impresoraDeSlimes = GameObject.FindGameObjectWithTag("ImpresoraDeSlimes").GetComponent<ImpresoraDeSlimes>();
        spawnersDeSlimes = new List<GameObject>();

        creadorDeEscenarios.CargarMapa(3);
        partida.cueva = 0;

        creadorDeEscenarios.CargarMapa(3);
        creadorDeEscenarios.ReemplazarCoordenadas();
        //Falta ejecutar el renderizado
    }
    public void RenderizarCuartosOrdenados()
    {

        List<Vector3> coordenadasBuffer = creadorDeEscenarios.RenderizarCuartos(6);
        for (int i = 0; i < coordenadasBuffer.Count; i++)
        {
            ordenDeCoordenadas.Add(coordenadasBuffer[i]);
        }
    }
    public void cargarSiguienteNivel()
    {
        GameObject[] cuevas = GameObject.FindGameObjectsWithTag("Cueva");

        for (int i = 0;i < cuevas.Length; i++)
        {
            Vector3 coordenadasCueva = cuevas[i].GetComponent<cuevaInfo>().leerCoordenadas();
            if ((coordenadasCueva.x == ordenDeCoordenadas[partida.cueva].x) &&
                (coordenadasCueva.y == ordenDeCoordenadas[partida.cueva].y) &&
                (coordenadasCueva.z == ordenDeCoordenadas[partida.cueva].z))
            {
                for (int j = 0; j < cuevas[i].transform.childCount; j++)
                {
                    GameObject estalagtitaHija;
                    if(cuevas[i].transform.GetChild(i).tag == "SlimeSpawn")
                    {
                        estalagtitaHija = cuevas[i].transform.GetChild(i).gameObject;
                        GameObject slimeSpawn = Instantiate(
                            new GameObject(), 
                            new Vector3(estalagtitaHija.transform.position.x, estalagtitaHija.transform.position.y, estalagtitaHija.transform.position.z), 
                            Quaternion.identity);
                        spawnersDeSlimes.Add(slimeSpawn); //Comprobar que los spawns si aparecen
                    }
                    
                } 
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
