using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNpc : MonoBehaviour
{

    public int[] Colors;
    public GameObject Headwears;
    public GameObject Heads;
    public GameObject Hands;
    public GameObject Weapons;
    public GameObject Torsos;
    public GameObject Shoes;

    private int Color;

    // Start is called before the first frame update
    void Start()
    {
        Color = Colors[Random.Range(0, Colors.Length)];
        Debug.Log(Color);
    }
}
