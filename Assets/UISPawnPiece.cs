using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISPawnPiece : MonoBehaviour
{

    
    public Sprite[] tileGraphicSprites;

  

    public GameObject[] pieces;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    
    public GameObject showUIPiece(PieceType PieceToHold, Vector2 location){
        int theI = 0;
        GameObject newPiece = gameObject;
        switch (PieceToHold)
        {
            case PieceType.I:
                newPiece = Instantiate(pieces[1], location, Quaternion.identity);
                theI = 1;
                break;

            case PieceType.J:
                newPiece = Instantiate(pieces[5], location, Quaternion.identity);
                theI = 5;
                break;

            case PieceType.L:
                newPiece = Instantiate(pieces[4], location, Quaternion.identity);
                theI = 4;
                break;

            case PieceType.O:
               newPiece = Instantiate(pieces[0], location, Quaternion.identity);
               theI = 0;
                break;

            case PieceType.S:
                newPiece = Instantiate(pieces[2], location, Quaternion.identity);
                theI = 2;
                break;

            case PieceType.T:
               newPiece = Instantiate(pieces[6], location, Quaternion.identity);
                theI = 6;
                break;

            case PieceType.Z:
                newPiece = Instantiate(pieces[3], location, Quaternion.identity);
                theI = 3;
                break;

            default:

                break;
        }

        SpriteRenderer[] tileSprites = newPiece.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sprite in tileSprites){
            sprite.sprite = tileGraphicSprites[theI];
        }

        return newPiece;
    }
    

    
}
