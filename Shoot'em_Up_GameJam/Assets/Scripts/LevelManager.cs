using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField]
    List<GameObject> setCardList;
    [SerializeField]
    GameObject upgradePlace1, upgradePlace2, upgradePlace3;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void GainLevel()
    {
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
        List<GameObject> listCardChoosen = new List<GameObject>();
        foreach(int setCardIndex in listIndexSetCardSelected)
        {
            listCardChoosen.Add(setCardList[setCardIndex]);
        }

        //Ici changer le texte, la miniature et les attributs

        ChooseUpgrade(listCardChoosen[0]);


    }

    public void ChooseUpgrade(GameObject setCard)
    {
        setCard.GetComponent<LevelScript>().setCard[0].GetComponent<IUpgrade>().UpgradePlayer();
        setCard.GetComponent<LevelScript>().setCard.RemoveAt(0);
        if(setCard.GetComponent<LevelScript>().setCard.Count == 0)
        {
            setCardList.Remove(setCard);

        }
    }
}
