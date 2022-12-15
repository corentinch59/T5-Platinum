using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI DialogueText;
    public string[] Sentences;
    private int Index = 0;
    public float DialogueSpeed;
    public int TxtCount;
    public GameObject Canva;
    public GameObject DetecteursCreux;
    public int HoleCount;
    public bool GetRebouchedptit;
    public bool GetRebouchedgros;
    private bool CanvasA = true;

    public GameObject UIPtite;
    public GameObject UIGrosse;

    public GameObject Ptitcorp;
    public GameObject Groscorp;

    public GameObject Corp1;
    public GameObject Corp2;
    public GameObject Corp3;
    public GameObject Corp4;

    public GameObject Corp5;
    public GameObject Corp6;

    void Start()
    {
        HoleCount = 0;
        if (TxtCount == 0)
        {
            NextSentence();

        }
        GetRebouchedgros = false;
        GetRebouchedptit = false;
    }


    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            //Canva.SetActive(true);
            GrosTombe();
            PtitTombe();
        }*/
        PtitTombe();
        GrosTombe();

    }

    //Le temps entre les textes
    IEnumerator Next()
    {

        yield return new WaitForSeconds(7f);
        if (TxtCount == 3)
        {
            Canva.SetActive(false);
            CanvasA = false;
            DialogueText.text = "";
            DetecteursCreux.SetActive(true);
        }

        if (TxtCount == 5)
        {
            Canva.SetActive(false);
            CanvasA = false;
            UIPtite.SetActive(true);
            Ptitcorp.SetActive(true);
            GetRebouchedptit = true;
        }
        if (TxtCount == 7)
        {
            Canva.SetActive(false);
            CanvasA = false;
            //TxtCount++;
            UIGrosse.SetActive(true);
            Groscorp.SetActive(true);
            GetRebouchedgros = true;
        }
        if (TxtCount == 12)
        {
            Canva.SetActive(false);
            CanvasA = false;
            StartSceneByName();
        }
        else
        {
            TxtCount++;
            NextSentence();
        }

    }

    //L'anim de texte
    void NextSentence()
    {
        if (Index <= Sentences.Length - 1 && CanvasA == true)
        {
            DialogueText.text = "";
            StartCoroutine(WriteSentence());
            StartCoroutine(Next());
        }

    }

    IEnumerator WriteSentence()
    {
        foreach (char Character in Sentences[Index].ToCharArray())
        {
            DialogueText.text += Character;
            yield return new WaitForSeconds(DialogueSpeed);
        }
        Index++;
    }

    //Tkt ça compte les trous pour la première épreuve
    void Ptittrou()
    {
        HoleCount++;
        if (HoleCount == 8)
        {
            HoleCount = 9;
            DetecteursCreux.SetActive(false);
            Canva.SetActive(true);
            CanvasA = true;
            Index = 4;
            NextSentence();
        }
    }

    void PtitTombe()
    {
        //GetRebouchedptit++;
        //if(GetRebouchedptit == 4)
        if (Corp1.activeInHierarchy == false && Corp2.activeInHierarchy == false && Corp3.activeInHierarchy == false && Corp4.activeInHierarchy == false && GetRebouchedptit == true)
        {
            GetRebouchedptit = false;
            UIPtite.SetActive(false);
            Ptitcorp.SetActive(false);
            Canva.SetActive(true);
            CanvasA = true;
            Index = 6;
            NextSentence();
        }
    }

    void GrosTombe()
    {
        //GetRebouchedgros++;
        if (GetRebouchedgros == true && Corp5.activeInHierarchy == false && Corp6.activeInHierarchy == false)
        {
            GetRebouchedgros = false;
            UIGrosse.SetActive(false);
            Groscorp.SetActive(false);
            Canva.SetActive(true);
            CanvasA = true;
            Index = 8;
            NextSentence();
        }
    }


    public void StartSceneByName()
    {
        SceneManager.LoadScene("TestCamera2");
    }

}
