using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public List<Transform> tiendasSpawneadas;

    List<Vector3> ordenDeCoordenadas;
    public GameObject[] prefabsDrops;
    public GameObject[] prefabsStatDrops;
    public List<Objeto> ObjetosPosibles;
    EfectosDeSonido jugadorSonidos;
    CreadorDeEscenarios creadorDeEscenarios;

    public List<GameObject> slimesExistentes;
    GameObject puertaBufferEntrada;
    GameObject puertaBufferSalida;
    UIManager uiManager;

    bool entroAlCuarto;
    int slimesAMatar;
    int slimesPorCuarto;
    public Vector3 ultimoSpawnPoint;
    int slimesDerrotadosTotal;
    int slimeRecolectadoTotal;
    public bool flawless;

    bool tutorial;
    public bool dialogoSlime;
    public bool dialogoTutoTerminado;
    bool primerDrop;
    void Awake()
    {

        slimesPorCuarto = 10;
        creadorDeEscenarios = GameObject.FindGameObjectWithTag("CreadorDeCuevas").GetComponent<CreadorDeEscenarios>();
        ObjetosPosibles = new List<Objeto>();
        dialogoSlime = false;
        tutorial = false;
        dialogoTutoTerminado = false;
        primerDrop = false;
        flawless = true;
        IniciarObjetos();
        ObjetosPosibles.AddRange(DatosDeGuardado.datosDeGuardado.objetosDesbloqueados);
        partida = new Partida();
        jugadorSonidos = GameObject.FindGameObjectWithTag("SonidosJugador").GetComponent<EfectosDeSonido>();
        ultimoSpawnPoint = new Vector3(0,0.3f,0);
        entroAlCuarto = false;
        spawnersDeSlimes = new List<GameObject>();
        slimesSpawneados = new List<GameObject>();
        ordenDeCoordenadas = new List<Vector3>();
        tiendasSpawneadas = new List<Transform>();

        partida.cueva = 0;
        partida.profundidad = 0;
        slimesAMatar = 0;
        slimesDerrotadosTotal = 0;
        partida.cuartosPorProfundidad = 3; //La cantidad se multiplica por 2 para tomar en cuenta puentes.
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //Falta ejecutar el renderizado
    }

    async public void TerminarPartida()
    {
        await uiManager.AparecerMuerte();

        UltimasStats.cantidadObjetos = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>().jugadorStats.objetos.Count ;
        UltimasStats.cantidadSlimes = slimesDerrotadosTotal;
        UltimasStats.cantidadCuevas = partida.cueva;
        UltimasStats.cantidadSlimeRecolectado = slimeRecolectadoTotal;
        UltimasStats.jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>().jugadorStats;
        UltimasStats.flawless = flawless;

        await Task.Delay(2500);
        SceneManager.LoadScene("PantallaMuerte");
    }

    async public void PartidaTutorial()
    {
        tutorial = true;
        primerDrop = true;
        jugadorSonidos.Musica.loop = true;
        jugadorSonidos.Musica.clip = jugadorSonidos.intermedioGenerico;
        jugadorSonidos.Musica.Play();

        partida.cuartosPorProfundidad = 2;
        await CrearNivel(true,true);
        await CargarSiguienteNivel(true);
        partida.cuartosPorProfundidad = 3;
        do { await Task.Yield(); } while (!dialogoSlime);

        AparecerSlimes(1);

        do
        {


            do { await Task.Yield(); } while (entroAlCuarto == false);

            partida.cueva++;
            entroAlCuarto = false;
            uiManager.ActualizarNivel(partida.cueva + 1, 1);
            if (creadorDeEscenarios.mapa.Count - 1 <= partida.cueva)
            {
                await CrearNivel(false, false);
                await CargarSiguienteNivel(false);
            }
            if (creadorDeEscenarios.mapa[partida.cueva].tipoDeCuarto == 3)
            {
                await DestruirSpawners();
                await DestruirSlimes();

                await CargarSiguienteNivel(true);
                AparecerSlimes(slimesPorCuarto + (partida.cueva));
            }
            else
            {
                await DestruirSpawners();
                await DestruirSlimes();

                await CargarSiguienteNivel(false);


                AbrirPuertas();


            }


        } while (true);
    }
    async public void PartidaNormal()
    {
        await CrearNivel(true, false);
        await CargarSiguienteNivel(true);
        AparecerSlimes(slimesPorCuarto);


        do {


            do { await Task.Yield(); } while (entroAlCuarto == false);

            partida.cueva++;
            entroAlCuarto = false;
            uiManager.ActualizarNivel(partida.cueva + 1, 1);
            if(creadorDeEscenarios.mapa.Count-1 <= partida.cueva)
            {
                await CrearNivel(false, false);
                await CargarSiguienteNivel(false);
            }
            if (creadorDeEscenarios.mapa[partida.cueva].tipoDeCuarto == 3)
            {
                await DestruirSpawners();
                await DestruirSlimes();

                await CargarSiguienteNivel(true);
                AparecerSlimes(slimesPorCuarto + (partida.cueva));
            }
            else
            {
                await DestruirSpawners();
                await DestruirSlimes();

                await CargarSiguienteNivel(false);


                AbrirPuertas();

  
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
                slimesDerrotadosTotal++;
                slimesSpawneados.RemoveAt(i);

                int drop = Random.Range(0, 101);
                if(drop <= 35 || primerDrop)
                {
                    drop = Random.Range(0, 101);
                    if (drop < 75) { drop = 0; }
                    else if (drop < 95) { drop = 1; }
                    else if (drop < 99) { drop = 2; }
                    else if (partida.cueva >= 15 || Herramientas.dificultad >= 1.7) { drop = 3; }
                    else { drop = 2; }

                    if (primerDrop) 
                    { 
                        drop = 0; Instantiate(prefabsDrops[drop], new Vector3(slime.transform.position.x, slime.transform.position.y + 1, slime.transform.position.z), Quaternion.identity); primerDrop = false;
                    }
                    Instantiate(prefabsDrops[drop], new Vector3(slime.transform.position.x, slime.transform.position.y + 1, slime.transform.position.z), Quaternion.identity);
                }
                Instantiate(prefabsStatDrops[0], new Vector3(slime.transform.position.x + 0.3f, slime.transform.position.y + 1, slime.transform.position.z), Quaternion.identity);
                Instantiate(prefabsStatDrops[1], new Vector3(slime.transform.position.x - 0.3f, slime.transform.position.y + 1, slime.transform.position.z), Quaternion.identity);

                slimesAMatar--;
                int slimeGanado = Mathf.FloorToInt(Random.Range(20, 51) * dificultad);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>().jugadorStats.slimeRecolectado += slimeGanado;
                slimeRecolectadoTotal += slimeGanado;
                uiManager.ActualizarSlime();
                break;
            }
        }

        if (slimesAMatar == 0)
        {
            SlimesMuertos();
        }
        return;
    }
    async public void SlimesMuertos()
    {
        if (tutorial) { do { await Task.Yield(); } while (!dialogoTutoTerminado); }
        Herramientas.perspectiva = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camara>().cambioDeCamara = true;

        AbrirPuertas();
        jugadorSonidos.Musica.Stop();
        jugadorSonidos.Musica.PlayOneShot(jugadorSonidos.NivelTerminado);
        Debug.Log(creadorDeEscenarios.mapa[partida.cueva + 1].id);
        if (creadorDeEscenarios.mapa[partida.cueva + 1].id == 0)
        {
            jugadorSonidos.Musica.clip = jugadorSonidos.intermedioGenerico;
            jugadorSonidos.Musica.Play();
        }
        else
        {
            if(DatosDeGuardado.datosDeGuardado.dialogosTiendaEscuchados <= 13)
            {
                DatosDeGuardado.datosDeGuardado.dialogosTiendaEscuchados++;
                GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(DatosDeGuardado.datosDeGuardado.dialogosTiendaEscuchados);
            } else
            {
                GameObject.FindGameObjectWithTag("dialogosManager").GetComponent<dialogosUI>().IniciarDialogo(DatosDeGuardado.datosDeGuardado.dialogosTiendaEscuchados);
            }

                jugadorSonidos.Musica.clip = jugadorSonidos.Tienda;
            jugadorSonidos.Musica.Play();
        }

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
        catch { }
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
        Herramientas.perspectiva = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camara>().cambioDeCamara = true;
        jugadorSonidos.Musica.clip = jugadorSonidos.Pelea;
        jugadorSonidos.Musica.loop = true;
        jugadorSonidos.Musica.Play();

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
            do { await Task.Delay(Random.Range(1000, 2500)); }
            while (slimesSpawneados.Count >= 5 + ((partida.cueva/2) * Herramientas.dificultad));

        } while (slimesAparecidos < cantidadSlimes);

        await (Task.Delay(1000));

    }
    async public Task CrearNivel(bool inicial, bool tutorial)
    {
        if (inicial)
        {
            await creadorDeEscenarios.CargarMapa(partida.cuartosPorProfundidad * 2 - 1, tutorial); //La primera generacion usa 1 cuarto menos
            await creadorDeEscenarios.ReemplazarCoordenadas();
            RenderizarCuartosOrdenados(true);
        } else
        { //Cambiar esto para contar profundidad despues
            await creadorDeEscenarios.CargarMapa(partida.cuartosPorProfundidad * 2, tutorial);
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
                    if (cuevas[i].transform.GetChild(j).tag == "vendedora")
                    {
                        tiendasSpawneadas.Add(cuevas[i].transform.GetChild(j).transform);
                        cuevas[i].transform.GetChild(j).transform.GetComponent<Tienda>().id = tiendasSpawneadas.Count - 1;
                        Debug.Log("Tienda encontrada" + cuevas[i].transform.GetChild(j).transform.GetComponent<Tienda>().id);
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

    public void IniciarObjetos() //0 Comun, 1 raro, 2 legendario, 3 unico ID MAXIMA ACTUAL = 10
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
            0.5f, //Critico
            0//Saltos
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
            0.5f,//Critico
            0//Saltos
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
            0f,//Critico
            0//Saltos
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
            0.8f,//Critico
            0//Saltos
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
            0.5f,//Critico
            0//Saltos
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
            0f,//Critico
            0//Saltos
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
            5f,//Critico
            0//Saltos
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
            5f,//Critico
            0//Saltos
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
            0f,//Critico
            0//Saltos
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
            0f,//Critico
            0//Saltos
            ));

        //Unicos
        ObjetosPosibles.Add(new Objeto(
            "Gema de la entropia", //Nombre
            10, //id
            3, //Rareza
            50f, //hpMax
            50f, //mpMax
            -20f, //DanoFisico
            20f, //DanoMagico
            0f, //DefFisica
            0f, //DefMagica
            0f, //VelocidadDeAtaque
            25f,//Critico
            0//Saltos
            ));

        ObjetosPosibles.Add(new Objeto(
            "Botas ligeras", //Nombre
            11, //id
            2, //Rareza
            30f, //hpMax
            0f, //mpMax
            2f, //DanoFisico
            2f, //DanoMagico
            0.3f, //DefFisica
            0.3f, //DefMagica
            0f, //VelocidadDeAtaque
            0f,//Critico
            1//Saltos
            ));



        //12 al 15 en muerteUI
    }
}

