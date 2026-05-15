using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class infoPartidaActual : MonoBehaviour
{
    


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
    Partida partida;

    List<GameObject> spawnersDeSlimes;
    List<GameObject> slimesSpawneados;
    List<Vector3> ordenDeCoordenadas;

    CreadorDeEscenarios creadorDeEscenarios;

    public List<GameObject> slimesExistentes;


    int cuartosPorProfundidad;

    void Start()
    {
        partida = new Partida();
        creadorDeEscenarios = GameObject.FindGameObjectWithTag("CreadorDeCuevas").GetComponent<CreadorDeEscenarios>();

        spawnersDeSlimes = new List<GameObject>();
        slimesSpawneados = new List<GameObject>();
        ordenDeCoordenadas = new List<Vector3>();
        partida.cueva = 0;
        cuartosPorProfundidad = 3; //La cantidad se multiplica por 2 para tomar en cuenta puentes.
        
        //Falta ejecutar el renderizado
    }

    async public void PartidaNormal()
    {
        await CrearNivel(true);
        await CargarSiguienteNivel();
        AparecerSlimes(20);
        /* do {
            AparecerSlimes(5 * ((partida.cueva + 1) / 2));
        } while (true);
        */
    }

    async public void AparecerSlimes(float cantidadSlimes)
    {
        
        int slimesAparecidos = 0;
        cantidadSlimes = Mathf.FloorToInt(cantidadSlimes);
        if (cantidadSlimes == 0) { cantidadSlimes = 1; }

        do
        {
            slimesSpawneados.Add(Instantiate(slimesExistentes[Random.Range(0, slimesExistentes.Count)], spawnersDeSlimes[Random.Range(0, spawnersDeSlimes.Count)].transform.position, Quaternion.identity));
            slimesAparecidos++;
            do { await Task.Delay(Random.Range(1500, 3000)); }
            while (slimesSpawneados.Count >= 10 + partida.cueva);

        } while (slimesAparecidos < cantidadSlimes);
    }
    async public Task CrearNivel(bool inicial)
    {
        if (inicial)
        {
            await creadorDeEscenarios.CargarMapa(cuartosPorProfundidad * 2 - 1); //La primera generacion usa 1 cuarto menos
            await creadorDeEscenarios.ReemplazarCoordenadas();
            RenderizarCuartosOrdenados();
        } else
        {
            await creadorDeEscenarios.CargarMapa(cuartosPorProfundidad * 2);
            await creadorDeEscenarios.ReemplazarCoordenadas();
            RenderizarCuartosOrdenados();
        }
    }
    async public void RenderizarCuartosOrdenados()
    {

        Vector3[] coordenadasBuffer = await creadorDeEscenarios.RenderizarCuartos(cuartosPorProfundidad, 0);
        
        for (int i = 0; i < coordenadasBuffer.Length; i++)
        {

            ordenDeCoordenadas.Add(coordenadasBuffer[i]);
        }
    }
    async public Task CargarSiguienteNivel()
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
                    if(cuevas[i].transform.GetChild(j).tag == "SpawnPosible")
                    {
                        estalagtitaHija = cuevas[i].transform.GetChild(j).gameObject;
                        GameObject slimeSpawn = new GameObject($"Spawner{j}");
                        slimeSpawn.transform.position = estalagtitaHija.transform.position;
                        slimeSpawn.transform.SetParent(estalagtitaHija.transform);

                        spawnersDeSlimes.Add(slimeSpawn); //Comprobar que los spawns si aparecen
                    }
                }
                return;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
