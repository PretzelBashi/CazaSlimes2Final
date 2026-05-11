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

        float danoFisicoMax = 5;
        float danoFisicoActual = 5;

        float danoMagicoMax = 5;
        float danoMagicoActual = 5;

        float defensaFisicaMax = 5;
        float defensaFisicaActual = 5;

        float defensaMagicaMax = 5;
        float defensaMagicaActual = 5;

        float critico = 5;

        //Falta aplicar daño critico y todo eso
        public void ActualizarHP(float danoARecibir, int tipoDeDano)
        {
            float vidaFinal = 0;

            switch (tipoDeDano) //0 es fisico, 1 es magico y 2 es curacion
            {
                case 0: vidaFinal = hpActual - (danoARecibir - defensaFisicaActual); break;
                case 1: vidaFinal = hpActual - (danoARecibir - defensaFisicaActual); break;
                case 2: vidaFinal = hpActual + danoARecibir; break;
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

        public void ActualizarStatsMaximas(float hpMax, float mpMax, float danoFisicoMax, float danoMagicoMax, float defensaFisicaMax, float defensaMagicaMax, bool SumarOAbsoluto)
        {
            if (SumarOAbsoluto) //Sumas
            {
                this.hpMax += hpMax;
                this.mpMax += mpMax;
                this.danoFisicoMax += danoFisicoMax;
                this.danoMagicoMax += danoMagicoMax;
                this.defensaFisicaMax += defensaFisicaMax;
                this.defensaMagicaMax += defensaMagicaMax;
            } else //Valores absolutos (Por si acaso)
            {
                if (hpMax > 0) { this.hpMax = hpMax; }
                if (mpMax > 0) { this.mpMax = mpMax; }
                if (danoFisicoMax > 0) { this.danoFisicoMax = danoFisicoMax; }
                if (danoMagicoMax > 0) { this.danoMagicoMax = danoMagicoMax; }
                if (defensaFisicaMax > 0) { this.defensaFisicaMax = defensaFisicaMax; }
                if (defensaMagicaMax > 0) { this.defensaMagicaMax = defensaMagicaMax; }
            }
        }

        public void ActualizarStatsActuales(string statACambiar, float nuevoValor)
        {
            switch (statACambiar)
            {
                case "dañoFisico": danoFisicoActual = nuevoValor; break;
                case "dañoMagico": danoMagicoActual = nuevoValor; break;
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
                case "dañoFisico": danoFisicoActual = danoFisicoMax; break;
                case "dañoMagico": danoMagicoActual = danoMagicoMax; break;
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
        ordenDeCoordenadas = new List<Vector3>();
        partida.cueva = 0;

        
        
        //Falta ejecutar el renderizado
    }
    async public void CrearNivel()
    {
        await creadorDeEscenarios.CargarMapa(3);
        await creadorDeEscenarios.ReemplazarCoordenadas();
        RenderizarCuartosOrdenados();

        await creadorDeEscenarios.CargarMapa(3);
        await creadorDeEscenarios.ReemplazarCoordenadas();
        RenderizarCuartosOrdenados();
    }
    async public void RenderizarCuartosOrdenados()
    {

        Vector3[] coordenadasBuffer = await creadorDeEscenarios.RenderizarCuartos(3,0);
        
        for (int i = 0; i < coordenadasBuffer.Length; i++)
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
