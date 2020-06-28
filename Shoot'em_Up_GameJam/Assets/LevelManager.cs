using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField]
    List<GameObject> setCardList;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void GainLevel()
    {
        List<int> listSetCardSelected = new List<int>();
        for(int counter = 0;counter < 3 && counter< setCardList.Count;counter++)
        {
            int randomSetCard = Random.Range(0, setCardList.Count);
            while(listSetCardSelected.Contains(randomSetCard))
            {
                randomSetCard = Random.Range(0, setCardList.Count);
            }
            listSetCardSelected.Add(randomSetCard);
        }
        List<GameObject> listCardChoosen = new List<GameObject>();
        foreach(int setCard in listSetCardSelected)
        {
            listCardChoosen.Add(setCardList[setCard].GetComponent<LevelScript>().GetFirstUpgrade());
        }
        


    }

    public void ChooseUpgrade(int setCard)
    {
        if(setCardList[setCard].GetComponent<LevelScript>().setCard.Count == 0)
        {
            setCardList.RemoveAt(setCard);
        }
    }
}
