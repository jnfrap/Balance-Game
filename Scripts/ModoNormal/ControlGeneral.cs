using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlGeneral : MonoBehaviour
{

    public GameObject pieza;
    public int contadorPiezas = 0;
    public int estadoPartida = 0;
    public bool piezaCaida = false;

    private GameObject cuboFondo;
    private GameObject balanza;
    private GameObject cronometro;
    private TextMeshProUGUI textoContador;
    private TextMeshProUGUI textoCrono;
    private TextMeshProUGUI textoFinal;

    private float contador = 0;
    private float contadorIA = 0;

    private float velocidadAlpha = 0.015f;
    private Quaternion rotacionBalanza;
    private List<PiezaGuardada> piezasGuardadas = new List<PiezaGuardada>();
    private bool unavez = true;
    private bool unavez2 = true;
    private int contadorPiezasMaximo = 0;

    //Referencias
    private ControlTurnos cTurnos;
    private ControlCronometro cCronometro;
    private ControlFlecha cFlecha;

    void Start() {
        //Inicializar referencias
        cTurnos = GameObject.Find("Circle").GetComponent<ControlTurnos>();
        cCronometro = GameObject.Find("Cronometro").GetComponent<ControlCronometro>();
        cFlecha = GameObject.Find("Flecha").GetComponent<ControlFlecha>();

        textoContador = GameObject.Find("TextoContador").GetComponent<TextMeshProUGUI>();
        textoCrono = GameObject.Find("TextoCrono").GetComponent<TextMeshProUGUI>();
        textoFinal = GameObject.Find("TextoFinal").GetComponent<TextMeshProUGUI>();
        cuboFondo = GameObject.Find("CuboFondo");
        balanza = GameObject.Find("Balanza");
        cronometro = GameObject.Find("Cronometro");
    }

    void Update()
    {
        //==========Zona de DEBUG==================//
        //Debug.Log(balanza.transform.rotation.z);

        //==========Fin zona de DEBUG==============//
        //Contador
        GameObject[] listaPiezas = GameObject.FindGameObjectsWithTag("Pieza");
        int contadorPiezasActual = listaPiezas.Length;
        if (contadorPiezasActual>contadorPiezasMaximo){
            contadorPiezasMaximo = contadorPiezasActual;
        }
        textoContador.SetText(""+contadorPiezasMaximo);

        //Control de victoria o derrota
        // 0 -> Jugando
        // 1 -> Acabada
        if (estadoPartida == 0){//Jugando


            if (cTurnos.getJugadorActual()!=null){
                //Comprobar si el turno actual es jugador
                if (cTurnos.getJugadorActual().jugable){
                    //Aparecer pieza
                    if (Input.GetMouseButtonDown(0) && !cCronometro.activado){
                        Vector3 mousePos = Input.mousePosition;
                        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
                        Instantiate(pieza, new Vector3(objectPos.x,objectPos.y+1.5f,0), Quaternion.identity);
                        if (objectPos.x<0){
                            cFlecha.posicion = 0;
                        }else{
                            cFlecha.posicion = 1;
                        }
                    }
                }else{ //Si es la IA
                    contadorIA+=Time.deltaTime;
                    if (unavez2 && !cCronometro.activado && contadorIA>0.5f){
                        GameObject piezaIA = Instantiate(pieza, new Vector3(0,3,0), Quaternion.identity);
                        piezaIA.GetComponent<ControlPieza>().esIA = true;
                        unavez2 = false;
                    }
                }
            }

            //Desaparecer flechas
            if (Input.GetMouseButtonUp(0)){
                cFlecha.posicion = -1;
            }

            //Parar fisicas si el cronometro esta desactivado
            if (cCronometro.activado){
                pararFisicas(true);
                contadorIA = 0;
                if (unavez){
                    guardarPiezas();
                    unavez2 = true;
                }
                unavez = false;
            }else{
                pararFisicas(false);
                unavez = true;
            }

            //Cosas que pasan cuando pierde un jugador
            /*
            if (piezaCaida && ControlTurnos.getGanador()==null){
                recuperarPiezas();
                piezaCaida = false;
            }*/

            if (cTurnos.getGanador()!=null){
                estadoPartida = 1;
            }

        }else if (estadoPartida == 1){//Acabada

            contador+=Time.deltaTime;

            if (contador>6 || GameObject.FindGameObjectsWithTag("Pieza").Length==0){
                //Activar animacion del cubo
                cuboFondo.GetComponent<Animator>().SetBool("PartidaPerdida",true);
                //Diluir el resto de cosas
                //Balanza
                Color colorBal = balanza.GetComponent<Renderer>().material.color;
                colorBal.a -= velocidadAlpha;
                balanza.GetComponent<Renderer>().material.color = colorBal;
                //Cronometro
                Color colorCro = cronometro.GetComponent<Renderer>().material.color;
                colorCro.a -= velocidadAlpha;
                cronometro.GetComponent<Renderer>().material.color = colorCro;
                //Punto
                Color colorPun = GetComponent<Renderer>().material.color;
                colorPun.a -= velocidadAlpha;
                GetComponent<Renderer>().material.color = colorPun;
                //Texto cronometro
                Color colorCont = (Color) textoCrono.faceColor;
                colorCont.a -= velocidadAlpha;
                textoCrono.faceColor = colorCont;

                //Piezas
                GameObject[] objects = GameObject.FindGameObjectsWithTag("Pieza");
                foreach (GameObject obj in objects) {
                    Color color = obj.GetComponent<Renderer>().material.color;
                    color.a -= velocidadAlpha;
                    obj.GetComponent<Renderer>().material.color = color;
                    obj.GetComponent<TrailRenderer>().enabled = false;
                }

                //Aparecer texto fin de partida
                Color colorFin = textoFinal.color;
                colorFin.a += 0.002f;
                textoFinal.color = colorFin;
                textoFinal.text = cTurnos.getGanador().nombre+" control his balance";

                //Reiniciar el juego =========> CAMBIAR ESTO <===========
                if(Input.GetMouseButtonDown(0)){
                    SceneManager.LoadScene("ModoNormal");
                }
            }
        }
    }

    //Controlador de fisicas
    private void pararFisicas(bool paradas){
        GameObject[] piezas = GameObject.FindGameObjectsWithTag("Pieza");
        for (int i=0;piezas.Length>i;i++){
            Rigidbody2D rb = piezas[i].GetComponent<Rigidbody2D>();

            if (paradas){
                rb.constraints = RigidbodyConstraints2D.None;
            }else{
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        GameObject balanza = GameObject.Find("Balanza");
        Rigidbody2D rbb = balanza.GetComponent<Rigidbody2D>();
        if (paradas){
            rbb.constraints = RigidbodyConstraints2D.None;
            rbb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }else{
            rbb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rbb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //Hacer el "Save State"
    private void guardarPiezas(){
        piezasGuardadas.Clear();
        GameObject[] piezas = GameObject.FindGameObjectsWithTag("Pieza");
        GameObject noGuardar = piezas[0];
        for (int i=0;piezas.Length>i;i++){
            if (piezas[i].GetComponent<ControlPieza>().timeSinceInitialization<noGuardar.GetComponent<ControlPieza>().timeSinceInitialization){
                noGuardar = piezas[i];
            }
        }
        for (int i=0;piezas.Length>i;i++){
            if (!noGuardar.Equals(piezas[i])){
                PiezaGuardada piezaGuardada = new PiezaGuardada(
                        piezas[i].transform.position.x,
                        piezas[i].transform.position.y,
                        piezas[i].transform.position.z,
                        piezas[i].transform.rotation
                );
                piezasGuardadas.Add(piezaGuardada);
            }
        }

        rotacionBalanza = balanza.transform.rotation;
    }

    //Restaurar el "Save State"
    public void recuperarPiezas(){
        GameObject[] piezas = GameObject.FindGameObjectsWithTag("Pieza");
        foreach (GameObject p in piezas){
            Destroy(p);
        }

        foreach (PiezaGuardada obj in piezasGuardadas) {
            GameObject piezaInstanciada = Instantiate(pieza,new Vector3(obj.x,obj.y,obj.z),obj.rot);
            piezaInstanciada.GetComponent<ControlPieza>().regenerado = true;
            piezaInstanciada.GetComponent<ControlPieza>().soltado = true;
        }

        cCronometro.activado = false;

        balanza.transform.rotation = rotacionBalanza;
    }
}
