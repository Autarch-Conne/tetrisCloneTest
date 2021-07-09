using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PiecesController : MonoBehaviour {

    public static PiecesController instance;

    public GameObject piecePrefab;
    
    public Vector2Int spawnPos;
    public float dropTime;
    public int turnsToSac;
    public Coroutine dropCurPiece;
    public Vector2Int[,] JLSTZ_OFFSET_DATA { get; private set; }
    public Vector2Int[,] I_OFFSET_DATA { get; private set; }
    public Vector2Int[,] O_OFFSET_DATA { get; private set; }
    public List<GameObject> piecesInGame;
    public GameObject pieceToDestroy = null;
    public GameObject gameOverText;

    GameObject curPiece = null;
    PieceController curPieceController = null;
    List<GameObject> availablePieces;
    


    int turnCounter;

    public RandomCoordinator pieceSelector;

    public UISPawnPiece uISPawnPiece;

    public PieceType heldPiece;

    public GameObject UIPiece;

    public bool isInput = false;

    public float delayInputTime = 0.1f;
    private bool isActualInput = false;

    public float dropSpeed = 5f;

    private float TimeStamp;




    private float holdTime = 0.20f;


    private bool heldLeft = false;

    private bool heldRight = false;

    private float temp =0;

    public float timeBetweenMoves = 0.1f;

    private float InputTimeStamp;

    private bool isSet;



    /// <summary>
    /// Called as soon as the instance is enabled. Sets the singleton and offset data arrays.
    /// </summary>
    private void Awake()
    {
        instance = this;
        

        JLSTZ_OFFSET_DATA = new Vector2Int[5, 4];
        JLSTZ_OFFSET_DATA[0, 0] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[0, 1] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[0, 2] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[0, 3] = Vector2Int.zero;

        JLSTZ_OFFSET_DATA[1, 0] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[1, 1] = new Vector2Int(1,0);
        JLSTZ_OFFSET_DATA[1, 2] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[1, 3] = new Vector2Int(-1, 0);

        JLSTZ_OFFSET_DATA[2, 0] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[2, 1] = new Vector2Int(1, -1);
        JLSTZ_OFFSET_DATA[2, 2] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[2, 3] = new Vector2Int(-1, -1);

        JLSTZ_OFFSET_DATA[3, 0] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[3, 1] = new Vector2Int(0, 2);
        JLSTZ_OFFSET_DATA[3, 2] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[3, 3] = new Vector2Int(0, 2);

        JLSTZ_OFFSET_DATA[4, 0] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[4, 1] = new Vector2Int(1, 2);
        JLSTZ_OFFSET_DATA[4, 2] = Vector2Int.zero;
        JLSTZ_OFFSET_DATA[4, 3] = new Vector2Int(-1, 2);

        I_OFFSET_DATA = new Vector2Int[5, 4];
        I_OFFSET_DATA[0, 0] = Vector2Int.zero;
        I_OFFSET_DATA[0, 1] = new Vector2Int(-1, 0);
        I_OFFSET_DATA[0, 2] = new Vector2Int(-1, 1);
        I_OFFSET_DATA[0, 3] = new Vector2Int(0, 1);

        I_OFFSET_DATA[1, 0] = new Vector2Int(-1, 0);
        I_OFFSET_DATA[1, 1] = Vector2Int.zero;
        I_OFFSET_DATA[1, 2] = new Vector2Int(1, 1);
        I_OFFSET_DATA[1, 3] = new Vector2Int(0, 1);

        I_OFFSET_DATA[2, 0] = new Vector2Int(2, 0);
        I_OFFSET_DATA[2, 1] = Vector2Int.zero;
        I_OFFSET_DATA[2, 2] = new Vector2Int(-2, 1);
        I_OFFSET_DATA[2, 3] = new Vector2Int(0, 1);

        I_OFFSET_DATA[3, 0] = new Vector2Int(-1, 0);
        I_OFFSET_DATA[3, 1] = new Vector2Int(0, 1);
        I_OFFSET_DATA[3, 2] = new Vector2Int(1, 0);
        I_OFFSET_DATA[3, 3] = new Vector2Int(0, -1);

        I_OFFSET_DATA[4, 0] = new Vector2Int(2, 0);
        I_OFFSET_DATA[4, 1] = new Vector2Int(0, -2);
        I_OFFSET_DATA[4, 2] = new Vector2Int(-2, 0);
        I_OFFSET_DATA[4, 3] = new Vector2Int(0, 2);

        O_OFFSET_DATA = new Vector2Int[1, 4];
        O_OFFSET_DATA[0, 0] = Vector2Int.zero;
        O_OFFSET_DATA[0, 1] = Vector2Int.down;
        O_OFFSET_DATA[0, 2] = new Vector2Int(-1, -1);
        O_OFFSET_DATA[0, 3] = Vector2Int.left;

        pieceSelector = GameObject.FindGameObjectWithTag("PieceGenerator").GetComponent<RandomCoordinator>();

        uISPawnPiece = GameObject.FindGameObjectWithTag("BoardController").GetComponent<UISPawnPiece>();
    }

    /// <summary>
    /// Called at the first frame instance is enabled. Sets some variables.
    /// </summary>
    private void Start()
    {
        piecesInGame = new List<GameObject>();
        availablePieces = new List<GameObject>();
        
        gameOverText.SetActive(false);
    }

    /// <summary>
    /// Called once every frame. Checks for player input.
    /// </summary>
    private void Update()
    {
        

        isActualInput = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            curPieceController.SendPieceToFloor();
        }
        if (Input.GetKey(KeyCode.LeftAlt) && TimeStamp <= Time.time)
        {
            MoveCurPiece(Vector2Int.down);
            TimeStamp = Time.time + (dropTime / dropSpeed);
        }

        //LEFT
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            temp = 0;
            heldLeft = false;
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            temp += Time.deltaTime;
            Debug.Log(temp);
            if(temp > holdTime){
                if(InputTimeStamp <= Time.time){
                    MoveCurPiece(Vector2Int.left);
                    
                    isInput = true;
                    isActualInput = true;
                    Invoke("resetInput", delayInputTime);
                    InputTimeStamp = Time.time + timeBetweenMoves;
                }
            }
            else if(!heldLeft){
                MoveCurPiece(Vector2Int.left);
                
                isInput = true;
                isActualInput = true;
                Invoke("resetInput", delayInputTime);
                heldLeft = true;
            }
        }

          //RIGHT
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            temp = 0;
            heldRight = false;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            temp += Time.deltaTime;
            Debug.Log(temp);
            if(temp > holdTime){
                if(InputTimeStamp <= Time.time){
                    MoveCurPiece(Vector2Int.right);
                    
                    isInput = true;
                    isActualInput = true;
                    Invoke("resetInput", delayInputTime);
                    InputTimeStamp = Time.time + timeBetweenMoves;
                }
            }
            else if(!heldRight){
                MoveCurPiece(Vector2Int.right);
                
                isInput = true;
                isActualInput = true;
                Invoke("resetInput", delayInputTime);
                heldRight = true;
            }
        }


        if(Input.GetKeyDown(KeyCode.LeftShift) && !isInput && !isSet){
            holdPiece();
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(curPieceController != null)
            {
                return;
            }
            turnCounter = 0;
            SpawnPiece();
        }
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            curPieceController.RotatePiece(true, true);
           
            isInput = true;
            isActualInput = true;
            Invoke("resetInput", delayInputTime);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            curPieceController.RotatePiece(false, true);
             
            isInput = true;
            isActualInput = true;
            Invoke("resetInput", delayInputTime);
        }

       

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SpawnDebug(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnDebug(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnDebug(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnDebug(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnDebug(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpawnDebug(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpawnDebug(6);
        }
        
        if(isActualInput){
            CancelInvoke();
             Invoke("resetInput", delayInputTime);
        }
        
        
    }

    void resetInput(){
        isInput = false;
    }

    /// <summary>
    /// Drops the piece the current piece the player is controlling by one unit.
    /// </summary>
    /// <returns>Function is called on a loop based on the 'dropTime' variable.</returns>
    IEnumerator DropCurPiece()
    {     
        while (!BoardController.instance.isSacrificing) {
            MoveCurPiece(Vector2Int.down);
            yield return new WaitForSeconds(dropTime);
        }
    }

    public void holdPiece(){

        PieceType temporaryPiece;
        if(UIPiece != null){
            Destroy(UIPiece);
        }
        temporaryPiece = curPieceController.curType;
        
        
        DestroyPiece(curPiece);
        SpawnDebug((int)heldPiece);

        heldPiece = temporaryPiece;
        UIPiece = uISPawnPiece.showUIPiece(heldPiece, new Vector2(-11.9f, 18f));

        


    }

    /// <summary>
    /// Once the piece is set in it's final location, the coroutine called to repeatedly drop the piece is stopped.
    /// </summary>
    public void PieceSet()
    {
        //if(dropCurPiece == null) { return; }

        
        StopCoroutine(dropCurPiece);
        isSet = true;
    }

    /// <summary>
    /// Makes any necessary changes once the game has ended.
    /// </summary>
    public void GameOver()
    {
        PieceSet();
        gameOverText.SetActive(true);
    }

    /// <summary>
    /// Removes the specified piece from the list of current pieces in the game.
    /// </summary>
    /// <param name="pieceToRem">Game Object of the Tetris piece to be removed.</param>
    public void RemovePiece(GameObject pieceToRem)
    {
        piecesInGame.Remove(pieceToRem);
    }

    /// <summary>
    /// Makes any necessary changes when destroying a piece.
    /// </summary>
    void DestroyPiece(GameObject pieceToRem)
    {
        PieceController curPC = pieceToRem.GetComponent<PieceController>();
        Vector2Int[] tileCoords = curPC.GetTileCoords();
        RemovePiece(pieceToRem);
        Destroy(pieceToRem);
        BoardController.instance.PieceRemoved(tileCoords);

       
    }

    /// <summary>
    /// Spawns a new Tetris piece.
    /// </summary>
    public void SpawnPiece()
    {     
        turnCounter++;

        

        GameObject localGO = GameObject.Instantiate(piecePrefab, transform);
        curPiece = localGO;
        PieceType randPiece = pieceSelector.PieceSelector();   
        curPieceController = curPiece.GetComponent<PieceController>();
        curPieceController.SpawnPiece(randPiece);
        
        
        
        piecesInGame.Add(localGO);

        dropCurPiece = StartCoroutine(DropCurPiece());
        isSet = false;
    }

    public void SpawnDebug(int id)
    {
        GameObject localGO = GameObject.Instantiate(piecePrefab, transform);
        curPiece = localGO;
        PieceType randPiece = (PieceType)id;
        curPieceController = curPiece.GetComponent<PieceController>();
        curPieceController.SpawnPiece(randPiece);

        piecesInGame.Add(localGO);
        isSet = false;
    }

    /// <summary>
    /// Checks to see if the sacrificing operation can be made.
    /// </summary>
    /// <returns>True if operation can be made. False if it can't</returns>
    

    /// <summary>
    /// Moves the current piece controlled by the player.
    /// </summary>
    /// <param name="movement">X,Y amount the piece should be moved by</param>
    public void MoveCurPiece(Vector2Int movement)
    {
        if(curPiece == null)
        {
            return;
        }
        curPieceController.MovePiece(movement);
    }

    
}
