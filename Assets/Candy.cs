using System.Collections;
using UnityEngine;

public class Candy : MonoBehaviour
{


    [SerializeField]public CandyType selectedCandyType;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("vacham");
         
        if (collision.gameObject.CompareTag("SquareBoardGame") )
        {
            Debug.Log("vacham với square");
            BoardManager.Instance.squareColliderBoardGame = collision.gameObject.GetComponent<Square>();
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SquareBoardGame"))
        {
            StartCoroutine(SetNull());
        }
    }

    IEnumerator SetNull()
    {
        yield return null;
        BoardManager.Instance.squareColliderBoardGame = null;

    }

    public enum CandyType
    {
        candyRed = 50,
        candyYellow = 60,
        candyPurple = 30,
        candyOrange = 40,
        candyGreen = 70,
        candyBlue = 45,
        
    }

}
