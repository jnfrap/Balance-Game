using System;
using UnityEngine;

public class ControlPieza : MonoBehaviour
{

    public bool soltado;
    private Rigidbody2D rb2D;
    private float comxBalanza;
    private float comxBalanzaF;
    private float rotacion;
    public float rotacionAnnadida = 0.5f;
    public bool regenerado = false;
    private bool unavez = true;
    private bool unavez2 = true;
    private int fid = 0;
    private float initializationTime;
    public float timeSinceInitialization;
    public bool esIA = false;
    private float velIA;

    //Referencias
    private ControlGeneral cGeneral;
    private ControlTurnos cTurnos;
    private ControlCronometro cCronometro;
    private GameObject balanza;

    void Awake() {
        //Inicializar referencias
        cGeneral = GameObject.Find("Circle").GetComponent<ControlGeneral>();
        cTurnos = GameObject.Find("Circle").GetComponent<ControlTurnos>();
        cCronometro = GameObject.Find("Cronometro").GetComponent<ControlCronometro>();
        balanza = GameObject.Find("Balanza");

        soltado = false;
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
        rotacion = 0;
        initializationTime = Time.timeSinceLevelLoad;

        comxBalanza = balanza.GetComponent<Renderer>().bounds.size.x * (balanza.transform.rotation.z*10);
        comxBalanzaF = UnityEngine.Random.Range(comxBalanza-1, comxBalanza+1);
        if (comxBalanzaF>5.6f){
            comxBalanzaF=5.6f;
        }
        if (comxBalanzaF<-5.6f){
            comxBalanzaF=-5.6f;
        }
    }

    void Update()
    {

        //==========Zona de DEBUG==================//

        //Debug.Log(transform.position.x);

        //==========Fin zona de DEBUG==============//

        //Tiempo de vida desde que aparecio
        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        if(!soltado){ //Mientras la estas arrastrando

            if (!esIA){ //Si la controla un jugador
                //Rotacion
                if (Input.GetMouseButton(1)){
                    girarPieza();
                }

                //Sigue el raton o el dedo
                if (Input.touchCount > 0){ //Dedo
                    //Recoge la pulsacion
                    Touch touch = Input.GetTouch(0);

                    //Comprobar si el dedo ha cambiado
                    if(unavez2){
                        fid = touch.fingerId;
                    }
                    unavez2=false;
                    if (fid!=touch.fingerId){
                        soltado = true;
                    }else{ //Si no ha cambiado
                        //La pieza se mueve hacia la pulsacion
                        Vector3 objectPos = Camera.main.ScreenToWorldPoint(touch.position);
                        transform.position = new Vector2(objectPos.x,objectPos.y+1.5f);
                    }

                }else if (Input.touchCount==0){ //Raton
                    Vector3 objectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    transform.position = new Vector2(objectPos.x,objectPos.y+1.5f);
                    unavez2=true;
                }

                //Cuando levantas el boton se suelta
                if (Input.GetMouseButtonUp(0)){
                    soltado=true;
                }
            }else{ //Si la controla la IA
                float xx = transform.position.x;
                velIA = 2f*Time.deltaTime;
                if (xx+0.2f<comxBalanzaF){
                    transform.position = new Vector2(transform.position.x+velIA,transform.position.y);
                }else if (xx-0.2f>comxBalanzaF){
                    transform.position = new Vector2(transform.position.x-velIA,transform.position.y);
                }else{
                    transform.position = new Vector2(transform.position.x,transform.position.y-velIA);
                }
            }

        }else{ //Cuando la sueltas
            rb2D.gravityScale = 1; //Se devuelve la gravedad

            //Se activa el cronometro
            if (unavez && !regenerado){
                cCronometro.activado = true;
            }
            unavez=false;
        }

        //Si va muy rapido resetear el cronometro
        if ((Math.Abs(rb2D.velocity.x)+Math.Abs(rb2D.velocity.y))>1){
            cCronometro.segundosF = cCronometro.segundos+1;
        }

        //Destruir cuando cae
        if (transform.position.y<-10){
            if (cTurnos.getGanador()==null && !cGeneral.piezaCaida){
                cGeneral.piezaCaida = true;
                cTurnos.getJugadorActual().perdido=true;
                //ControlTurnos.turno--;
                if (cTurnos.listaJugadores.Count-cTurnos.getJugadoresQueHanPerdido()>1){
                    GameObject.Find("CuboFondo").GetComponent<Animator>().SetBool("JugadorPerdido",true);
                }else{
                    cGeneral.estadoPartida=1;
                }
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D() {
        soltado = true;
    }

    public void girarPieza(){
        rotacion+=rotacionAnnadida*(Time.deltaTime*300);
        transform.rotation = Quaternion.Euler(0,0,rotacion);
    }
}
