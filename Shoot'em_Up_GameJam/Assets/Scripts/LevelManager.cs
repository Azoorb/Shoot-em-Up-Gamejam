using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    List<GameObject> setCardList;
    [SerializeField]
    List<GameObject> cardPlaceList;
    [SerializeField]
    GameObject panelUpgrade;
    public Animator transition;
    List<GameObject> listCardChoosen;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        setCardList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            setCardList.Add(transform.GetChild(i).gameObject);
        }
    }

    public void GainLevel()
    {
        transition.SetBool("End", false);
        transition.SetBool("Start", true);
        Time.timeScale = 0;
        List<int> listIndexSetCardSelected = new List<int>();
        for(int counter = 0;counter < 3 && counter< setCardList.Count;counter++)
        {
            int randomSetCard = Random.Range(0, setCardList.Count);
            while(listIndexSetCardSelected.Contains(randomSetCard))
            {
                randomSetCard = Random.Range(0, setCardList.Count);
            }
            listIndexSetCardSelected.Add(randomSetCard);
        }
        listCardChoosen = new List<GameObject>();
        foreach(int setCardIndex in listIndexSetCardSelected)
        {
            listCardChoosen.Add(setCardList[setCardIndex]);
        }

        for(int i = 2; i >= listCardChoosen.Count;i-- )
        {
            cardPlaceList[i].SetActive(false);
        }
        for(int i =0;i<listCardChoosen.Count;i++)
        {
            Debug.Log("Yo : " + i);
            cardPlaceList[i].transform.GetChild(0).GetComponent<Image>().sprite = listCardChoosen[i].GetComponent<LevelScript>().setCard[0].GetComponent<UpgradeBase>().miniatureSprite;
            cardPlaceList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = listCardChoosen[i].GetComponent<LevelScript>().setCard[0].GetComponent<UpgradeBase>().description;
            cardPlaceList[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = listCardChoosen[i].GetComponent<LevelScript>().setCard[0].GetComponent<UpgradeBase>().attributs;
            GameObject card = listCardChoosen[i];
            cardPlaceList[i].transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
            cardPlaceList[i].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { ChooseUpgrade(card); } ) ;
        }


    }

    

    public void ChooseUpgrade(GameObject setCard)
    {
        setCard.GetComponent<LevelScript>().setCard[0].GetComponent<IUpgrade>().UpgradePlayer();
        setCard.GetComponent<LevelScript>().setCard.RemoveAt(0);
        if (setCard.GetComponent<LevelScript>().setCard.Count == 0)
        {
            setCardList.Remove(setCard);

        }
        transition.SetBool("Start", false);
        transition.SetBool("End", true);
        Time.timeScale = 1;
    }
}
