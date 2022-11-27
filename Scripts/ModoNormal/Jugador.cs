using UnityEngine;

public class Jugador {
    public Jugador(int id, string nombre, bool jugable, bool perdido, int fichasColocadas, int turno, GameObject marco) {
        this.id = id;
        this.nombre = nombre;
        this.jugable = jugable;
        this.perdido = perdido;
        this.fichasColocadas = fichasColocadas;
        this.turno = turno;
        this.marco = marco;
    }

    public int id { get; set; }
    public string nombre { get; set; }
    public bool jugable { get; set; }
    public bool perdido { get; set; }
    public int fichasColocadas { get; set; }
    public int turno { get; set; }
    public GameObject marco { get; set; }
}
