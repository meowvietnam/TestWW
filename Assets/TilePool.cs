


public class TilePool : PoolManager
{
    private static TilePool instance;
    public static TilePool Instance { get => instance; }


    protected override  void Awake()
    {
        base.Awake();
        instance = this;

    }
    // Start is called before the first frame update
    
   
}
