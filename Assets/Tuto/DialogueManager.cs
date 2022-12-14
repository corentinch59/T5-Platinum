using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public int GetRebouchedptit = 3;
    public int GetRebouchedgros = 0;
    private bool CanvasA = true;

    public GameObject UIPtite;
    public GameObject UIGrosse;

    public GameObject Ptitcorp;
    public GameObject Groscorp;


    void Start()
    {
        HoleCount = 0;
        if (TxtCount == 0)
        {
            NextSentence();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Canva.SetActive(true);
            GetRebouchedptit = 3;
            GrosTombe();
            PtitTombe();
        }

    }

    //Le temps entre les textes
    IEnumerator Next()
    {
        
        yield return new WaitForSeconds(6f);
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
        }
        if (TxtCount == 7)
        {
            Canva.SetActive(false);
            CanvasA = false;
            //TxtCount++;
            UIGrosse.SetActive(true);
            Groscorp.SetActive(true);
        }
        if (TxtCount == 12)
        {
            Canva.SetActive(false);
            CanvasA = false;
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
        GetRebouchedptit++;
        if (GetRebouchedptit == 4)
        {
            GetRebouchedptit = 5;
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
        GetRebouchedgros++;
        if (GetRebouchedgros == 2)
        {
            GetRebouchedgros = 3;
            UIGrosse.SetActive(false);
            Groscorp.SetActive(false);
            Canva.SetActive(true);
            CanvasA = true;
            Index = 8;
            NextSentence();
        }
    }

}
