using UnityEngine;

public class HeatMapCube : MonoBehaviour
{
    public void SetColor(Color c)
    {
        Material mat = GetComponent<Renderer>().material;
        c.a = mat.color.a;
        mat.color = c;
    }
}
