using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    List<GameObject> setCardList;
    [SerializeField]
    List<GameObject> cardPlaceList;
    [SerializeField]
    GameObject panelUpgrade;
    public Animator cardTransition;
    List<GameObject> listCardChoosen;
    [SerializeField]
    GameObject textRetry;
    [SerializeField] private GameObject textAbandon;
    [SerializeField] private Animator circleTransition;
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
        cardTransition.SetBool("End", false);
        cardTransition.SetBool("Start", true);
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
        cardTransition.SetBool("Start", false);
        cardTransition.SetBool("End", true);
        Time.timeScale = 1;
    }

    public void ShowRetry()
    {
        //Time.timeScale = 0;
        textRetry.SetActive(true);
        textAbandon.SetActive(true);
    }

    public void Retry()
    {

        StartCoroutine(Load(SceneManager.GetActiveScene().name));

    }

    public void Play()
    {
        SceneManager.LoadScene(2);
        //StartCoroutine(Load("Léo"));
    }

    public void Menu()
    {

        StartCoroutine(Load("Menu 1"));
    }

    private IEnumerator Load(string scene)
    {
        circleTransition.SetTrigger("Load");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Waited");
        SceneManager.LoadScene(scene);
    }
}
