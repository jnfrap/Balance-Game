using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlTurnos : MonoBehaviour
{
    public int turno; //El 0 no existe por favor que si no me voy a terminar confundiendo
    public int numeroJugadores;
    public int numeroJugadoresJugables;
    public bool turnoJugador = true;
    public List<Jugador> listaJugadores = new List<Jugador>();
    public List<GameObject> listaMarcos = new List<GameObject>();
    public GameObject marco;
    public Sprite spriteMarcoIA;

    private bool unavez = false;
    private float distanciaMarcos = 0.95f;
    public bool turnosReordenados = false;

    //Referencias
    private ControlGeneral cGeneral;
    private ControlCronometro cCronometro;

    void Awake() {
        turno = 1;
        numeroJugadores = 4;
        numeroJugadoresJugables = 1;
        //Inicializar referencias
        cGeneral = GameObject.Find("Circle").GetComponent<ControlGeneral>();
        cCronometro = GameObject.Find("Cronometro").GetComponent<ControlCronometro>();

        //Instanciar marcos
        for (int i = 0;i<numeroJugadores;i++){
            GameObject m = Instantiate(marco,transform.position,Quaternion.identity);
            GameObject canvas = m.transform.GetChild(0).gameObject;
            canvas.GetComponent<Canvas>().worldCamera = Camera.main;
            listaMarcos.Add(m);
        }

        //Llenar lista de jugadores
        int jugables = numeroJugadoresJugables;
        bool jugable;
        for (int i = 0;i<numeroJugadores;i++)
        {
            if (jugables>0){
                jugable=true;
                jugables--;
            }else{
                jugable = false;
            }

            listaJugadores.Add(new Jugador(i+1,"Jugador "+(i+1),jugable,false,0,i+1,listaMarcos[i]));

            listaJugadores[i].marco.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(listaJugadores[i].nombre);

            if (!jugable){
                listaJugadores[i].marco.GetComponent<SpriteRenderer>().sprite = spriteMarcoIA;
            }

        }

    }

    void Update() {
        //Posicionar cosas
        float disMar = distanciaMarcos;
        foreach (Jugador jugador in listaJugadores){
            //Marco
            GameObject m = jugador.marco;
            m.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.95f,Screen.height*disMar,10));

            //Nombre
            float separacionMarcos = (1.8f*Screen.width)/1920f;
            GameObject child = m.transform.GetChild(0).GetChild(0).gameObject;
            child.transform.position = new Vector3(m.transform.position.x-separacionMarcos,m.transform.position.y,m.transform.position.z);
            //Colores de los nombres (¿Temporal?)
            TextMeshProUGUI textoNombre = child.GetComponent<TextMeshProUGUI>();
            if (jugador.Equals(getJugadorActual())){
                textoNombre.color = new Color(80f/255f, 222f/255f, 194f/255f, 1.0f);
            }else if (jugador.perdido){
                textoNombre.color = new Color(218f/255f, 0f/255f, 55f/255f, 1.0f);
            }else{
                textoNombre.color = new Color(237f/255f, 237f/255f, 237f/255f, 1.0f);
            }

            //Numero
            GameObject numero = m.transform.GetChild(0).GetChild(1).gameObject;
            numero.transform.position = new Vector3(m.transform.position.x+separacionMarcos,m.transform.position.y,m.transform.position.z);

            disMar-=0.1f;
        }

        //Pasar un turno cuando se desactiva el cronometro
        if (!cCronometro.activado && !cGeneral.piezaCaida){
            if (unavez){
                pasarTurno();
                unavez=false;
            }
        }else{
            unavez=true;
        }

        //Comprobar si el jugador es jugable
        turnoJugador = comprobarSiEsJugador();

        //Mostrar numero de fichas colocadas por jugador
        foreach (Jugador j in listaJugadores){
            GameObject marco = j.marco;
            TextMeshProUGUI numero = marco.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            numero.SetText(""+j.fichasColocadas);
        }

        //Los turnos no pueden ser mayor al numero de jugadores
        if (turno>numeroJugadores-getJugadoresQueHanPerdido()){
            turno=1;
        }
        //Tampoco pueden ser menor que 1
        if (turno<1){
            turno=1;
        }
    }

    //Metodo que comprueba si el jugador ha perdido
    public bool comprobarSiJugadorPerdio(Jugador jugador) {
        bool res = false;

        if (jugador!=null){
            res = jugador.perdido;
        }

        return res;
    }

    //Metodo que comprueba si el jugador es humano
    private bool comprobarSiEsJugador() {
        bool res = false;

        if (getJugadorActual()!=null){
            res = getJugadorActual().perdido;
        }

        return res;
    }

    //Metodo que devuelve el jugador actual
    public Jugador getJugadorActual(){
        Jugador jugador = null;

        foreach (Jugador j in listaJugadores){
            if (j.turno==turno){
                jugador = j;
            }
        }

        return jugador;
    }

    //Metodo que reordena los turnos
    public void reordenarTurnos(){
        int i = 1;
        foreach (Jugador j in listaJugadores){
            if (j.perdido){
                j.turno=0;
            }else{
                j.turno=i;
                i++;
            }
        }

        turnosReordenados = true;
    }

    public int getJugadoresQueHanPerdido(){
        int n = 0;

        foreach (Jugador j in listaJugadores){
            if (j.perdido){
                n++;
            }
        }

        return n;
    }

    public Jugador getGanador(){
        Jugador jug = null;

        if (listaJugadores.Count-getJugadoresQueHanPerdido()==1){
            foreach (Jugador j in listaJugadores){
                if (!j.perdido){
                    jug = j;
                }
            }
        }

        return jug;
    }

    public void pasarTurno(){
        if (!turnosReordenados){
            Jugador j = getJugadorActual();
            if (!j.perdido){
                j.fichasColocadas++;
            }
        }else{
            turnosReordenados = false;
        }
        turno++;
    }
}
