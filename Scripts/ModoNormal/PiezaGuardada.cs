using UnityEngine;

public class PiezaGuardada {
    public PiezaGuardada(float x, float y, float z, Quaternion rot) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.rot = rot;
    }

    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public Quaternion rot { get; set; }
}