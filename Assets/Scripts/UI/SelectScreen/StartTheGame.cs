using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTheGame : MonoBehaviour
{

    [SerializeField] private List<ChoseYourChara> list = new List<ChoseYourChara>();

    private void Start()
    {
        ChoseYourChara.OnReady += ChoseYourChara_OnReady;
    }

    private void ChoseYourChara_OnReady()
    {
        //On test si tous les joueurs sont ready
        foreach (ChoseYourChara chara in list)
        {
            if (!chara.isReady) return;
        }

        Debug.Log("ALL READY");
        //Lancer un timer 3 2 1 GO et le jeu 
    }

    public void CreateTheUIPlayer(GameObject eeeeee)
    {
        //GameObject MANANA = Instantiate(eeeeee);
        //MANANA.transform.position = new Vector3(5f, 0f, 0f);
    }

    private void OnDestroy()
    {
        ChoseYourChara.OnReady -= ChoseYourChara_OnReady;
    }
}
