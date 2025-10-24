using UnityEngine;

public class Documents : MonoBehaviour
{
    [SerializeField] private GameObject _documents1;
    [SerializeField] private GameObject _documents2;

    [SerializeField] private AudioClip _clip1;
    [SerializeField] private AudioClip _clip2;

    private AudioSource _audioSource1;
    private AudioSource _audioSource2;

    private int trigger1;
    private int trigger2;

    private int etatDocuments;
    private string prendreDocuments = "Vous avez pris les documents...";
    private string deposerDocuments = "Vous avez déposé les documents...";

    void Start()
    {
        _audioSource1 = _documents1.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_documents1".
        _audioSource2 = _documents2.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_documents2".

        _documents1.SetActive(true); // Le "_documents1" est actif.
        _documents2.SetActive(false); // Le "_documents2" est innactif.

        etatDocuments = 0; // Les documents sont dans leur premier état.
        trigger1 = 0; // Initialisation du trigger 1.
        trigger2 = 0; // Initialisation du trigger 2.
    }

    void Update()
    {
        PrendreDocuments();
        DeposerDocuments();
    }

    private void PrendreDocuments()
    {
        if (trigger1 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_documents1.transform.position, GameManager.instance.positionJ); // Calcul la distance entre les documents1 et le joueur.

        // Si le joueur est proche de les documents1.
        if (distance < 5)
        {
            // Si les documents sont dans leur 1er état, que les mains sont vides et que le joueur peut intéragir avec les documents.
            if (etatDocuments == 0 && GameManager.instance.mainsVides == true && GameManager.instance.connaitDanger == true)
            {
                trigger1 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

                GameManager.instance.Afficher(prendreDocuments + GameManager.instance.txtMains); // Texte pour informer le joueur.
                Debug.Log(prendreDocuments + GameManager.instance.txtMains);

                GameManager.instance.mainsVides = false; // Les mains sont maintenant pleines.
                _audioSource1.PlayOneShot(_clip1); // Joue l'audio du joueur qui prend les documents.
                _documents1.SetActive(false); // Les documents est déactiver pour qu'il ne soit plus visible au joueur.
                etatDocuments = 1; // Les documents est dans sa deuxième état.
            }

            // Si le joueur peut intéragir avec Les documents, mais les mains sont remplis.
            else if (GameManager.instance.mainsVides == false && GameManager.instance.connaitDanger == true)
            {
                GameManager.instance.Afficher(GameManager.instance.txtMains); // Texte qui informe que les mains sont remplis.
                Debug.Log(GameManager.instance.txtMains);
            }
        }
    }

    private void DeposerDocuments()
    {
        if (trigger2 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

       float distance = Vector3.Distance(_documents2.transform.position, GameManager.instance.positionJ); // Calcul la distance entre les documents2 et le joueur.

        // Si le joueur est proche des documents et qu'ils sont dans leur 2ème état.
        if (distance < 5 && etatDocuments == 1)
        {
            trigger2 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée

            GameManager.instance.Afficher("Vous avez déposé Les documents..."); // Texte pour informer le joueur.
            Debug.Log("Vous avez déposé Les documents...");

            GameManager.instance.mainsVides = true; // Les mains sont vides.
            _audioSource2.PlayOneShot(_clip2); // Joue l'audio du joueur qui dépose Les documents.
            _documents2.SetActive(true); // Les documents est activer pour qu'il soit visible au joueur.
            etatDocuments = 2; // Les documents est dans son état final.
        }
    }
}
