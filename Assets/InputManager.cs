using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; }
    [SerializeField] public bool isDrag;
    public Vector2 mouseWorldPosition;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    bool HitIsSquareInBoardWait(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            if (isDrag == false && hit.collider.CompareTag("SquareBoardWait"))
            {
                return true;
            }
        }
        return false;
    }
    bool HitIsSquareInBoardGame(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            if (isDrag == false && hit.collider.CompareTag("SquareBoardGame"))
            {
                return true;
            }
        }
        return false;
    }
    void SettingEventMouseDown(RaycastHit2D hit)
    {
        if(HitIsSquareInBoardWait(hit))
        {
            BoardManager.Instance.squareComponentSelect = hit.collider.gameObject.GetComponent<Square>();
            BoardManager.Instance.EventCandyAfterMouseDown();

        }
        else if (HitIsSquareInBoardGame(hit))
        {
            BoardManager.Instance.squareComponentSelect = hit.collider.gameObject.GetComponent<Square>();
            BoardManager.Instance.EventCandyAfterMouseDown();

        }
        isDrag = true;

    }
    void SettingEventMouseUp()
    {
      
       BoardManager.Instance.EventCandyAfterMouseUp();
       isDrag = false;

    }
    // Update is called once per frame
    void Update()
    {
        SetMousePos();
    }
    void SetMousePos()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            SettingEventMouseDown(hit);


        }
        if (Input.GetMouseButtonUp(0))
        {
            SettingEventMouseUp();
        }
    }    
   
   
 
}
