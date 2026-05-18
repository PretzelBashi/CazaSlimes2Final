using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Herramientas;

public class infoPartidaActual : MonoBehaviour
{
    


    public class Partida
    {
        public int cueva; //nivel actual a cargar
        public int profundidad;
        public int cuartosPorProfundidad;


        public Partida()
        {
            cueva = 0;
            profundidad = 0;
            cuartosPorProfundidad = 0;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Partida partida;

    List<GameObject> spawnersDeSlimes;
    List<GameObject> slimesSpawneados;
    List<Vector3> ordenDeCoordenadas;
    public List<Objeto> ObjetosPosibles;

    CreadorDeEscenarios creadorDeEscenarios;

    public List<GameObject> slimesExistentes;
    GameObject puertaBufferEntrada;
    GameObject puertaBufferSalida;

    bool entroAlCuarto;
    int slimesAMatar;
    int slimesPorCuarto;


    void Awake()
    {
        slimesPorCuarto = 10;
        creadorDeEscenarios = GameObject.FindGameObjectWithTag("CreadorDeCuevas").GetComponent<CreadorDeEscenarios>();
        ObjetosPosibles = new List<Objeto>();
        IniciarObjetos();
        partida = new Partida();
        
        entroAlCuarto = false;
        spawnersDeSlimes = new List<GameObject>();
        slimesSpawneados = new List<GameObject>();
        ordenDeCoordenadas = new List<Vector3>();

        partida.cueva = 0;
        partida.profundidad = 0;
        slimesAMatar = 0;
        partida.cuartosPorProfundidad = 3; //La cantidad se multiplica por 2 para tomar en cuenta puentes.
        
        //Falta ejecutar el renderizado
    }

    async public void PartidaNormal()
    {
        await CrearNivel(true);
        await CargarSiguienteNivel(true);
        AparecerSlimes(slimesPorCuarto);

        do {
            do { await Task.Yield(); } while (entroAlCuarto == false);
            partida.cueva++;
            entroAlCuarto = false;
            if (creadorDeEscenarios.mapa[partida.cueva].tipoDeCuarto == 3)
            {
                await DestruirSpawners();
                await DestruirSlimes();

                await CargarSiguienteNivel(true);
                AparecerSlimes(slimesPorCuarto);
            }
            else
            {
                await DestruirSpawners();
                await DestruirSlimes();

                await CargarSiguienteNivel(false);
                AbrirPuertas();
                continue;
            }


        } while (true);
        
        /* do {
            AparecerSlimes(5 * ((partida.cueva + 1) / 2));
        } while (true);
        */
    }

    async public Task DestruirSpawners()
    {
        foreach(GameObject spawn in spawnersDeSlimes)
        {
            Destroy(spawn.gameObject);
        }
        spawnersDeSlimes.Clear();
        return;
    }

    async public Task DestruirSlimes() //Esto es por si acaso
    {
        foreach (GameObject slime in slimesSpawneados)
        {
            Destroy(slime.gameObject);
        }
        slimesSpawneados.Clear();
        return;
    }

    async public Task EliminarSlime(GameObject slime) //--------------------------------------------------------------------OBJEOTS
    {
        for(int i =0; i< slimesSpawneados.Count; i++)
        {
            if (slimesSpawneados[i] == slime)
            {
                slimesSpawneados.RemoveAt(i);

                int drop = Random.Range(0, 101);
                if(drop <= 100)
                {
                    drop = Random.Range(0, 101);
                    if (drop < 55) { drop = 0; }
                    else if (drop < 85 ) { drop = 1; }
                    else if (drop < 95) { drop = 2; }
                    else { drop = 3; }
                    Instantiate(slime.GetComponent<Slime>().prefabsDrops[drop], new Vector3(slime.transform.position.x, slime.transform.position.y + 1, slime.transform.position.z), Quaternion.identity);
                }
                slimesAMatar--;

                break;
            }
        }

        if (slimesAMatar == 0)
        {
            AbrirPuertas();
            Debug.Log("Todos muertos");
        }
        return;
    }
    public void AbrirPuertas()
    {
        puertaBufferEntrada.GetComponent<BoxCollider>().enabled = false;
        puertaBufferEntrada.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        try
        {
            puertaBufferSalida.GetComponent<BoxCollider>().enabled = false;
            puertaBufferSalida.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
        catch { Debug.Log("No hay puerta de salida"); }
    }
    public void EntrarAlCuarto()
    {
        puertaBufferSalida = puertaBufferEntrada;

        puertaBufferSalida.GetComponent<BoxCollider>().enabled = true;
        puertaBufferSalida.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        entroAlCuarto = true;
    }

    async public void AparecerSlimes(float cantidadSlimes)
    {
        
        int slimesAparecidos = 0;
        if (cantidadSlimes == 0) { cantidadSlimes = 1; }

        cantidadSlimes = Mathf.FloorToInt(cantidadSlimes);
        slimesAMatar = Mathf.FloorToInt(cantidadSlimes);

        do
        {
            int id = Random.Range(0, slimesExistentes.Count);

            GameObject slimeA = Instantiate(slimesExistentes[id], spawnersDeSlimes[Random.Range(0, spawnersDeSlimes.Count)].transform.position, Quaternion.identity);
            slimeA.GetComponent<Slime>().IniciarSlime(id);
            slimesSpawneados.Add(slimeA);
            
            slimesAparecidos++;
            do { await Task.Delay(Random.Range(2000, 5000)); }
            while (slimesSpawneados.Count >= 10 + partida.cueva);

        } while (slimesAparecidos < cantidadSlimes);
    }
    async public Task CrearNivel(bool inicial)
    {
        if (inicial)
        {
            await creadorDeEscenarios.CargarMapa(partida.cuartosPorProfundidad * 2 - 1); //La primera generacion usa 1 cuarto menos
            await creadorDeEscenarios.ReemplazarCoordenadas();
            RenderizarCuartosOrdenados(true);
        } else
        { //Cambiar esto para contar profundidad despues
            await creadorDeEscenarios.CargarMapa(partida.cuartosPorProfundidad * 2);
            await creadorDeEscenarios.ReemplazarCoordenadas();
            RenderizarCuartosOrdenados(false);
        }
    }
    async public void RenderizarCuartosOrdenados(bool inicial)
    {

        Vector3[] coordenadasBuffer;
        if (inicial) { coordenadasBuffer = await creadorDeEscenarios.RenderizarCuartos(partida.cuartosPorProfundidad * 2 - 1, 0); }
        else { coordenadasBuffer = await creadorDeEscenarios.RenderizarCuartos(partida.cuartosPorProfundidad * 2, 0); }


            for (int i = 0; i < coordenadasBuffer.Length; i++)
            {

                ordenDeCoordenadas.Add(coordenadasBuffer[i]);
            }
        //RevisarCoordenadasCargadas();
    }
    public void RevisarCoordenadasCargadas()
    {
        foreach(Vector3 coordenada in ordenDeCoordenadas)
        {
            Debug.Log($"{coordenada.x},{coordenada.y},{coordenada.z}");
        }
    }
    async public Task CargarSiguienteNivel(bool cueva)
    {
        GameObject[] cuevas = GameObject.FindGameObjectsWithTag("Cueva");

        for (int i = 0;i < cuevas.Length; i++)
        {
            
            Vector3 coordenadasCueva = cuevas[i].GetComponent<cuevaInfo>().leerCoordenadas();
            //Debug.Log($"{coordenadasCueva.x},{coordenadasCueva.y},{coordenadasCueva.z}");

            if ((coordenadasCueva.x == ordenDeCoordenadas[partida.cueva].x) &&
                (coordenadasCueva.y == ordenDeCoordenadas[partida.cueva].y) &&
                (coordenadasCueva.z == ordenDeCoordenadas[partida.cueva].z))
            {
                
                for (int j = 0; j < cuevas[i].transform.childCount; j++)
                {
                    
                    GameObject estalagtitaHija;
                    if (cuevas[i].transform.GetChild(j).tag == "puertaSalida")
                    {
                        puertaBufferEntrada = cuevas[i].transform.GetChild(j).gameObject;
                    }
                    if (cuevas[i].transform.GetChild(j).tag == "SpawnPosible" && cueva)
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

    public Objeto RegresarDrop(int rareza)
    {
        List<Objeto> lista = new List<Objeto>();
        foreach (Objeto objeto in ObjetosPosibles)
        {
            if(objeto.rareza == rareza)
            {
                lista.Add(objeto);
            }
        }
        return (lista[Random.Range(0, lista.Count)]);
    }

    public void IniciarObjetos() //0 Comun, 1 raro, 2 legendario, 3 unico
    {
        //Comunes
        ObjetosPosibles.Add(new Objeto(
            "Daga oxidad", //Nombre
            0, //id
            0, //Rareza
            0f, //hpMax
            2f, //mpMax
            2f, //DanoFisico
            0f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0.02f, //VelocidadDeAtaque
            0.5f //Critico
            ));
        ObjetosPosibles.Add(new Objeto(
            "Ramita extrana", //Nombre
            1, //id
            0, //Rareza
            0f, //hpMax
            5f, //mpMax
            0f, //DanoFisico
            2f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0f, //VelocidadDeAtaque
            0.5f //Critico
            ));
        ObjetosPosibles.Add(new Objeto(
            "Pechera desgastada", //Nombre
            2, //id
            0, //Rareza
            4f, //hpMax
            0f, //mpMax
            0f, //DanoFisico
            0f, //DanoMagico
            1f, //DefFisica
            0.5f, //DefMagica
            0f, //VelocidadDeAtaque
            0f //Critico
            ));

        //Raro
        ObjetosPosibles.Add(new Objeto(
            "Espada de hierro", //Nombre
            3, //id
            1, //Rareza
            0f, //hpMax
            3f, //mpMax
            3.5f, //DanoFisico
            0f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0.01f, //VelocidadDeAtaque
            0.8f //Critico
            ));
        ObjetosPosibles.Add(new Objeto(
            "Hechizos sencillos Vol.1", //Nombre
            4, //id
            1, //Rareza
            0f, //hpMax
            5f, //mpMax
            0f, //DanoFisico
            3.5f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0f, //VelocidadDeAtaque
            0.5f //Critico
            ));
        ObjetosPosibles.Add(new Objeto(
            "Grebas de hierro", //Nombre
            5, //id
            1, //Rareza
            7f, //hpMax
            0f, //mpMax
            0f, //DanoFisico
            0f, //DanoMagico
            1.5f, //DefFisica
            0.7f, //DefMagica
            0f, //VelocidadDeAtaque
            0f //Critico
            ));

        //Legendarios
        ObjetosPosibles.Add(new Objeto(
            "Katana de mitrilo", //Nombre
            6, //id
            2, //Rareza
            0f, //hpMax
            10f, //mpMax
            9f, //DanoFisico
            4f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0.2f, //VelocidadDeAtaque
            5f //Critico
            ));
        ObjetosPosibles.Add(new Objeto(
            "Lanza esmaltada", //Nombre
            7, //id
            2, //Rareza
            0f, //hpMax
            20f, //mpMax
            4f, //DanoFisico
            9f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0f, //VelocidadDeAtaque
            5f //Critico
            ));
        ObjetosPosibles.Add(new Objeto(
            "Casco de titanio", //Nombre
            8, //id
            2, //Rareza
            50f, //hpMax
            20f, //mpMax
            0f, //DanoFisico
            0f, //DanoMagico
            2.5f, //DefFisica
            0.5f, //DefMagica
            0f, //VelocidadDeAtaque
            0f //Critico
            ));
        ObjetosPosibles.Add(new Objeto(
            "Casco de mitrilo", //Nombre
            9, //id
            2, //Rareza
            70f, //hpMax
            30f, //mpMax
            0f, //DanoFisico
            0f, //DanoMagico
            0.5f, //DefFisica
            1.8f, //DefMagica
            0f, //VelocidadDeAtaque
            0f //Critico
            ));

        //Unicos
        ObjetosPosibles.Add(new Objeto(
            "Gema de la entropia", //Nombre
            10, //id
            3, //Rareza
            100f, //hpMax
            100f, //mpMax
            -40f, //DanoFisico
            50f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0f, //VelocidadDeAtaque
            50f //Critico
            ));
    }
}

