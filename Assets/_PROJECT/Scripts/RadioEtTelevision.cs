using UnityEngine;

public class RadioEtTelevision : MonoBehaviour
{
    [Header("Radio")]

    [SerializeField] private GameObject _radio;
    [SerializeField] private AudioClip _clipRadio;
    private AudioSource _audioSourceR;
    private int triggerR;
    private int etatRadio;


    [Header("Television")]

    [SerializeField] private GameObject _television;
    [SerializeField] private AudioClip _clipTelevision;
    private AudioSource _audioSourceT;
    private int triggerT;
    private int etatTelevision;


    void Start()
    {
        _audioSourceR = _radio.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_radio".
        _audioSourceT = _television.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_television".

        etatRadio = 0; // La radio est dans son 1er état.
        etatTelevision = 0; // La télévision est dans son 1er état.

        triggerR = 0; // Initialisation du trigger radio.
        triggerT = 0; // Initialisation du trigger télévision.
    }

    void Update()
    {
        JouerRadio();
        JouerTelevision();
    }

    private void JouerRadio()
    {
        if (triggerR != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_radio.transform.position, GameManager.instance.positionJ); // Calcul la distance entre la radio et le joueur.

        // Si le joueur est proche de la radio et qu'elle est dans son 1er état.
        if (distance < 5 && etatRadio == 0)
        {
            triggerR = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

            Debug.Log("La radio joue..."); // Informe la console que la radio joue.

            GameManager.instance.connaitDanger = true; // Le joueur peut maintenant intéragir avec plus d'événements.
            _audioSourceR.PlayOneShot(_clipRadio); // Joue l'audio de la radio.
            /*_radio. -Changer le visuel de la radio avec une anim.*/
            etatRadio = 1;
        }
    }
    
    private void JouerTelevision()
    {
        if (triggerT != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_television.transform.position, GameManager.instance.positionJ); // Calcul la distance entre la télévision et le joueur.

        // Si le joueur est proche de la télévision et qu'elle est dans son 1er état.
        if (distance < 5 && etatTelevision == 0)
        {
            triggerT = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

            Debug.Log("La télévision joue..."); // Informe la console que la télévision joue.

            GameManager.instance.connaitDanger = true; // Le joueur peut maintenant intéragir avec plus d'événements.
            _audioSourceT.PlayOneShot(_clipTelevision); //  Joue l'audio de la télévision.
            //_television. -Changer le visuel de la télévision.
            etatTelevision = 1;
        }
    }
}
