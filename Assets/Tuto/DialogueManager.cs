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
            StartCoroutine(Uno());
        }
        GetRebouchedgros = false;
        GetRebouchedptit = false;
        SoundManager.instance.Play("Ambiance");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Canva.SetActive(true);
            //GrosTombe();
            // PtitTombe();
            StartSceneByName();
        }
        PtitTombe();
        GrosTombe();

    }

    IEnumerator Uno()
    {
        yield return new WaitForSeconds(2f);
        NextSentence();
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
            //Animalese();
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

    void Animalese()
    {
        if (Index == 0)
        {
            SoundManager.instance.Play("Dialogue 1");
        }
        if (Index == 1)
        {
            SoundManager.instance.Play("Dialogue 2");
        }
        if (Index == 2)
        {
            SoundManager.instance.Play("Dialogue 3");
        }
        if (Index == 3)
        {
            SoundManager.instance.Play("Dialogue 4");
        }
        if (Index == 4)
        {
            SoundManager.instance.Play("Dialogue 5");
        }
        if (Index == 5)
        {
            SoundManager.instance.Play("Dialogue 6");
        }
        if (Index == 6)
        {
            SoundManager.instance.Play("Dialogue 7");
        }
        if (Index == 7)
        {
            SoundManager.instance.Play("Dialogue 8");
        }
        if (Index == 8)
        {
            SoundManager.instance.Play("Dialogue 9");
        }
        if (Index == 9)
        {
            SoundManager.instance.Play("Dialogue 10");
        }
        if (Index == 10)
        {
            SoundManager.instance.Play("Dialogue 11");
        }
        if (Index == 11)
        {
            SoundManager.instance.Play("Dialogue 12");
        }
        if (Index == 12)
        {
            SoundManager.instance.Play("Dialogue 13");
        }
    }

    public void StartSceneByName()
    {
        SceneManager.LoadScene("TestCamera2 1");
    }
}
