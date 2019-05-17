using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScaler : MonoBehaviour
{
    public static ScreenScaler Instance;

    public GameObject GameEnvironment;

    private Boundary _boundary;
      
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        var aspect = (float)Screen.width / Screen.height;

        if (aspect >= 0.74) // (12:16) 2048x2732
        {
            GameEnvironment.transform.localScale = new Vector3(1.24f, 1.02f, 1f);

            _boundary = new Boundary(-3.25f, 1.5f);
        }
        else if (aspect >= 0.5625) // (9:16) 1334x750
        {
            Debug.Log("default");
            _boundary = new Boundary(-2.31f, 1.12f);
        }
        else if (aspect >= 0.5) // (9:18) 1080x2160
        {
            Debug.Log("0.5");

            GameEnvironment.transform.localScale = new Vector3(0.89f, 0.99f, 1f);
            _boundary = new Boundary(-2.05f, 0.95f);

        }
        else if (aspect >= 0.48) // (9:19) 1125x2436
        {
            Debug.Log("0.48");

            GameEnvironment.transform.localScale = new Vector3(0.85f, 0.98f, 1f);
            _boundary = new Boundary(-1.95f, 0.90f);
        }
        else if (aspect >= 0.46) // (9:19) 1125x2436
        {
            Debug.Log("0.46");

            GameEnvironment.transform.localScale = new Vector3(0.82f, 0.97f, 1f);
            _boundary = new Boundary(-1.85f, 0.85f); 
        }
    }

    public Boundary Boundary
    {
        get { return _boundary; }
    }

    public float ScaleX
    {
        get { return GameEnvironment.transform.localScale.x; }
    }
}
