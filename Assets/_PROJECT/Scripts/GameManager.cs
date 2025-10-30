using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;
// using Unity.Android.Gradle;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool connaitDanger;
    [SerializeField] public bool mainsVides;
    [SerializeField] private GameObject _joueur;
    [SerializeField] private Text _txt;
    public Vector3 positionJ;

    public string txtMains = "Vos mains sont pleines...";

    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }

    void Awake()
    {
        // S'il n'y a pas déjà d'instance, crée une instance.
        if (_instance == null)
        {
            _instance = this; // Initialisation du Singleton!
        }
        else
        {
            Debug.LogError("GameManager vient d'empêcher une instanciation additionnelle de lui-même (Singleton!)");
        }
    }

    void Start()
    {
        connaitDanger = false;
        mainsVides = true;
        _txt.text = "";
    }

    void Update()
    {
        positionJ = _joueur.transform.position;
    }

    public void Afficher(string message)
    {
        _txt.text = message;
    }

    public void Deafficher()
    {
        _txt.text = "";
    }
}
