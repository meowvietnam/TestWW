using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public int candyPoolIndex;
    public Vector2 pos;
    public GameObject objCandyRender;

    public virtual void SetUp(int i , int j)
    {
        Vector2 posNew = new Vector3(j * 0.64f, -i * 0.64f, 0);
        pos = posNew;
        candyPoolIndex = -1;


    }
    public virtual void SetCandyInBoard()
    {

        objCandyRender.GetComponent<BoxCollider2D>().enabled = false;
        objCandyRender.transform.localPosition = pos;
    }


}
public class BoardWait : Board
{

    public override void SetUp(int i, int j)
    {
        base.SetUp(i,j);
        int candyObjectIndex = Random.Range(0, TilePool.Instance.listPool.Count);
        candyPoolIndex = candyObjectIndex;
        TilePool.Instance.ActivePoolObject(candyPoolIndex);
        objCandyRender = TilePool.Instance.poolObject;
        SetCandyInBoard();
    }
    public override void SetCandyInBoard()
    {
        if (objCandyRender != null)
        {
            objCandyRender.transform.SetParent(BoardManager.Instance.boardWaitObject.transform);
            base.SetCandyInBoard();

        }

    }

}
public class BoardGame : Board
{

    public override void SetUp(int i , int j)
    {
        base.SetUp(i, j);
    }
    public override void SetCandyInBoard()
    {
        if (objCandyRender != null)
        {
            objCandyRender.transform.SetParent(BoardManager.Instance.boardGameObject.transform);
            base.SetCandyInBoard();

        }

    }

}


public class BoardManager : MonoBehaviour
{
    private static BoardManager instance;
    public static BoardManager Instance { get => instance; }


    private BoardWait[,] boardWait;
    private int countCandyInBoardWait;
    private Board[,] boardGame;




    private HashSet<(int, int)> listIndexEqualNeedDestroy = new(); // trong hashSet không có cặp giá trị nào trùng nhau
    private HashSet<(int, int)> listIndexEqualUpOrDown = new();
    private HashSet<(int, int)> listIndexEqualRightOrLeft = new();

    public Square squareColliderBoardGame;
    public Square squareComponentSelect;

    public GameObject cellBG0;
    public GameObject cellBG1;
    public GameObject boardWaitObject;
    public GameObject boardGameObject;
    
    void PrintSquare(int i, int j, Vector2 pos, GameObject parentObj)
    {
         
        if (j % 2 == 0 && i % 2 == 0 || i%2!= 0 && j % 2 != 0)
        {
            GameObject cellBG = Instantiate(cellBG0);
            SetUpCellBGObject(cellBG);
        }
        else
        {
            GameObject cellBG = Instantiate(cellBG1);
            SetUpCellBGObject(cellBG);
        }
      
        void SetUpCellBGObject(GameObject cellBGObj)
        {
            cellBGObj.transform.SetParent(parentObj.transform);
            cellBGObj.transform.localPosition = pos;
            Square cellBGObjComponent = cellBGObj.GetComponent<Square>();
            cellBGObjComponent.indexX = i;
            cellBGObjComponent.indexY = j;

            if (parentObj == boardWaitObject)
            {
                cellBGObj.tag = "SquareBoardWait";
            }  
            else
            {
                cellBGObj.tag = "SquareBoardGame";

            }
        }
        
    }
    
    void FillBoardWait(int rows, int cols) // 
    {


        boardWait = new BoardWait[rows, cols];
        countCandyInBoardWait = rows * cols;
        void PrintBoardWaitToScene(int i, int j)
        {
            boardWait[i, j] = new BoardWait();
            boardWait[i, j].SetUp(i,j);
            PrintSquare(i , j, boardWait[i, j].pos, boardWaitObject);

        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {

              
                    PrintBoardWaitToScene(i, j);
            }
        }
    }
    void FillBoardGame(int rows, int cols) // 
    {

        boardGame = new BoardGame[rows, cols];
        void PrintBoardGameToScene(int i, int j)
        {
            boardGame[i, j] = new BoardGame();
            boardGame[i, j].SetUp(i,j);
            PrintSquare(i, j, boardGame[i, j].pos, boardGameObject);

        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {


                PrintBoardGameToScene(i, j);


            }
        }
    }




    void Check3TileEqual(HashSet<(int, int)> tempListIndexEqual)
    {
        if (tempListIndexEqual.Count > 2)
        {
            listIndexEqualNeedDestroy.UnionWith(tempListIndexEqual);

        }
    }
    int FindPiece(int row, int col)
    {
        return boardGame[row, col].candyPoolIndex;
    }
    void CheckUp(int row, int col, int objIndexCheck)
    {
        // check up

        for (int i = row; i >= 0; i--)
        {
            if (objIndexCheck == boardGame[i, col].candyPoolIndex)
            {

                listIndexEqualUpOrDown.Add((i, col));
            }
            else
            {
                break;
            }
        }
        Check3TileEqual(listIndexEqualUpOrDown);

    }
    void CheckDown(int row, int col, int objIndexCheck)
    {
        // check down

        for (int i = row; i < boardGame.GetLength(0); i++)
        {
            if (objIndexCheck == boardGame[i, col].candyPoolIndex)
            {

                listIndexEqualUpOrDown.Add((i, col));
            }
            else
            {
                break;
            }
        }
        Check3TileEqual(listIndexEqualUpOrDown);



    }
    void CheckRight(int row, int col, int objIndexCheck)
    {
        // check phải

        for (int i = col; i < boardGame.GetLength(1); i++)
        {
            if (objIndexCheck == boardGame[row, i].candyPoolIndex)
            {
                listIndexEqualRightOrLeft.Add((row, i));

            }
            else
            {
                break;
            }
        }
        Check3TileEqual(listIndexEqualRightOrLeft);

    }
    void CheckLeft(int row, int col, int objIndexCheck)
    {
        // check trái
        for (int i = col; i >= 0; i--)
        {
            if (objIndexCheck == boardGame[row, i].candyPoolIndex)
            {
                listIndexEqualRightOrLeft.Add((row, i));
            }
            else
            {
                break;
            }
        }
        Check3TileEqual(listIndexEqualRightOrLeft);


    }
    void Checking(int row, int col)
    {
        listIndexEqualUpOrDown.Clear();
        listIndexEqualRightOrLeft.Clear();
        listIndexEqualNeedDestroy.Clear();
        int thisPiece = FindPiece(row, col);
        if (CheckInSide(row, col))
        {

            CheckUp(row, col, thisPiece);
            CheckDown(row, col, thisPiece);
            CheckRight(row, col, thisPiece);
            CheckLeft(row, col, thisPiece);




        }
        else // ở edge
        {
            // nếu ở các đầu mút
            if (CheckEdgeRow(col) && CheckEdgeColumn(row))
            {
                if (col == 0)
                {
                    // check phải 
                    CheckRight(row, col, thisPiece);

                }
                else
                {
                    // check trái
                    CheckLeft(row, col, thisPiece);

                }
                if (row == 0)
                {
                    // check down
                    CheckDown(row, col, thisPiece);

                }
                else
                {
                    // check up
                    CheckUp(row, col, thisPiece);

                }
            }
            else
            {
                if (col == 0)
                {
                    // check up 
                    CheckUp(row, col, thisPiece);

                    // check down\
                    CheckDown(row, col, thisPiece);


                    // check phải
                    CheckRight(row, col, thisPiece);


                }
                else
                {

                    // check up 
                    CheckUp(row, col, thisPiece);

                    // check down\
                    CheckDown(row, col, thisPiece);


                    // check trái
                    CheckLeft(row, col, thisPiece);


                }
                if (row == 0)
                {
                    // check phải 
                    CheckRight(row, col, thisPiece);

                    // check trái
                    CheckLeft(row, col, thisPiece);


                    // check down
                    CheckDown(row, col, thisPiece);


                }
                else
                {
                    // check phải 
                    CheckRight(row, col, thisPiece);

                    // check trái 
                    CheckLeft(row, col, thisPiece);


                    // check up
                    CheckUp(row, col, thisPiece);


                }


            }
        }
        EventAfterChecking();
    }


    void EventAfterChecking()
    {
       

    
        
        if (IsChecking())
        {

            // swap obj ở render
             EventIfCheckingTrue();
        
            Debug.Log("Đúng");
        }
       
    }
 
    
    void EventIfCheckingTrue()
    {
        foreach ((int i, int j) in listIndexEqualNeedDestroy)
        {
            if (boardGame[i, j].objCandyRender != null)
            {
                Debug.Log(boardGame[i, j].objCandyRender.name);
                TilePool.Instance.DestroyAfterEvent(boardGame[i, j].objCandyRender, boardGame[i, j].candyPoolIndex);
                VfxPool.Instance.SetVfxCoin(boardGame[i, j].pos);
                GameManager.Instance.SetScoreText((int)boardGame[i, j].objCandyRender.GetComponent<Candy>().selectedCandyType);
                boardGame[i, j].objCandyRender = null;
                boardGame[i, j].candyPoolIndex = -1;

            }


            //boardArray[i, j] = null;
        }

        //Filling(FilterListObjectForFilling());

        // yield return StartCoroutine(CheckFullBoard());


    }
    
  

  
    bool IsChecking()
    {
        Debug.Log(listIndexEqualNeedDestroy.Count);
        if (listIndexEqualNeedDestroy.Count <= 0) // nếu check k có cái nào có thể bị destroy thì
        {

            return false;
        }
        return true;


      

    }    
    bool CheckInSide(int row, int col)
    {   
        if(CheckEdgeRow(col) == true || CheckEdgeColumn(row) == true)
        {
            return false;
        }    
        return true;
       
    }    
    bool CheckEdgeRow(int col)
    {
        // kiểm tra hàng
           
        if( col == 0 || col == boardGame.GetLength(1) - 1)
        {
            return true; 
        }
        return false; 
    }
    bool CheckEdgeColumn(int row)
    {
        // kiểm tra hàng

        if (row == 0 || row == boardGame.GetLength(0) - 1)
        {
            return true; 
        }
        return false; 
    }

    
    public void FillCandyToBoardGame()
    {
        if(boardGame[squareColliderBoardGame.indexX, squareColliderBoardGame.indexY].objCandyRender == null)
        {
            if(CheckIsSquareSelectInBoardGame())
            {
                boardGame[squareColliderBoardGame.indexX, squareColliderBoardGame.indexY].candyPoolIndex = boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].candyPoolIndex;
                boardGame[squareColliderBoardGame.indexX, squareColliderBoardGame.indexY].objCandyRender = boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender;
                boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender = null;
                boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].candyPoolIndex = -1;


            }
            else
            {
                boardGame[squareColliderBoardGame.indexX, squareColliderBoardGame.indexY].candyPoolIndex = boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].candyPoolIndex;
                boardGame[squareColliderBoardGame.indexX, squareColliderBoardGame.indexY].objCandyRender = boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender;
                boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender = null;
                countCandyInBoardWait--;
                FillCandyToBoardWait();
            }
            boardGame[squareColliderBoardGame.indexX, squareColliderBoardGame.indexY].SetCandyInBoard();


        }

        //InputManager.Instance.candySelect.transform.position = squareCollider.transform.position;
    }
    public void FillCandyToBoardWait()
    {
        if(countCandyInBoardWait<=0)
        {
            FillBoardWait(3,7);
        }    
    }
    public void EventCandyAfterMouseDown()
    {
        if(CheckIsSquareSelectInBoardGame())
        {
            if (boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender != null)
            {
                boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender.GetComponent<BoxCollider2D>().enabled = true;

            }
        }   
        else
        {
            if (boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender != null)
            {
                boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender.GetComponent<BoxCollider2D>().enabled = true;

            }
        }
    
        

    }
    public void EventCandyAfterMouseUp()
    {
        if (squareColliderBoardGame != null)
        {
           if (boardGame[squareColliderBoardGame.indexX, squareColliderBoardGame.indexY].objCandyRender != null)
           {
                ResetPosCandy();

           }
           else
           {
                FillCandyToBoardGame();
                //checking
                Checking(squareColliderBoardGame.indexX, squareColliderBoardGame.indexY);

            }
        }
        else 
        {
            ResetPosCandy();
        }
    }
    void ResetPosCandy()
    {
        if (squareComponentSelect != null)
        {
            if (CheckIsSquareSelectInBoardGame())
            {
                boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].SetCandyInBoard();

            }
            else
            {
                boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].SetCandyInBoard();

            }
        }
    }    
    bool CheckIsSquareSelectInBoardGame()
    {  
        if (squareComponentSelect.gameObject.tag == "SquareBoardGame")
        {
            return true;
        }
        return false;
    }    
    public void SetIndexSquare(GameObject squareObj, int i , int j)
    {
        Square squareComponent = squareObj.GetComponent<Square>();
        squareComponent.indexX = i;
        squareComponent.indexY = j;
    }
    void CandyMoveWithMouse()
    {
        if (InputManager.Instance.isDrag == true)
        {
            if(CheckIsSquareSelectInBoardGame() && boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender != null)
            {
                boardGame[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender.transform.position = InputManager.Instance.mouseWorldPosition;

            }
            else if(!CheckIsSquareSelectInBoardGame() && boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender != null)
            {
                boardWait[squareComponentSelect.indexX, squareComponentSelect.indexY].objCandyRender.transform.position = InputManager.Instance.mouseWorldPosition;

            }
        }
    }

    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        
        FillBoardWait(3, 7);
        FillBoardGame(6, 6);

    }
    private void Update()
    {
        CandyMoveWithMouse();
    }
}
