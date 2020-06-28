using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public List<GameObject> setCard { get; /*=> _setCard;*/ }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetFirstUpgrade()
    {
        return setCard[0];
    }

}
