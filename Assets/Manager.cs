using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] Mastermind mastermind; //Mastermind script
    [SerializeField] GameObject[] gameSlot = new GameObject[10];//Game slot
    [SerializeField] GameObject[] gameVerif = new GameObject[4];//Small game slot for verification
    private int currentSlot = 0, currentCol = 1; //position on slot
    private Sprite emptySprite; //Sprite empty hole
    [SerializeField] string[] code = new string[4]; //Store the color code     

    void Start()
    {
        mastermind.GetNewSecretCode();//Generate new secret code
        emptySprite = gameSlot[currentSlot].transform.Find("c1").GetComponent<Image>().sprite;//Assign empty sprite
    }

    /// <summary>
    /// Method for selection of color in keyboard
    /// </summary>
    /// <param name="sp">Sprite</param>
    public void ColorSelect(Sprite sp)
    {
        if (!mastermind.HiddenSlot.activeInHierarchy) return;

        gameSlot[currentSlot].transform.Find("c" + currentCol).GetComponent<Image>().sprite = sp;
        code.SetValue(sp.name, currentCol - 1);
        currentCol++;
        if (currentCol == 5) currentCol = 1;
    }

    /// <summary>
    /// Cancel the slot selection
    /// </summary>
    public void Cancel()
    {
        for (int i = 1; i < 5; i++)
        {
            gameSlot[currentSlot].transform.Find("c" + i).GetComponent<Image>().sprite = emptySprite;
        }
        currentCol = 1;
    }

    /// <summary>
    /// Reload the game scene
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Check the color 
    /// </summary>
    public void Check()
    {
        if (!mastermind.HiddenSlot.activeInHierarchy) return; //Return if mastermind slot is active

        //Assign de gameobects gameVerif for current slot
        int x = 0;
        foreach (Transform child in gameSlot[currentSlot].transform)
        {
            if(child.tag=="Verif")
            {
                gameVerif[x] = child.gameObject;
                x++;                
            }
        }

        //return if a slot is empty
        for (int i = 1; i < 5; i++)
        {
            if (gameSlot[currentSlot].transform.Find("c" + i).GetComponent<Image>().sprite == emptySprite) return;
        }

        //Show the good position
        int nbGoodPosition = mastermind.GetGoodPosition(code);
        for (int i = 0; i < nbGoodPosition; i++)
        {
            gameVerif[i].GetComponent<Image>().sprite = mastermind.Black;
        }

        //Show the wrong position
        int nbWrongPosition = mastermind.GetWrongPosition();
        for (int i = nbGoodPosition; i < nbWrongPosition + nbGoodPosition; i++)
        {
            gameVerif[i].GetComponent<Image>().sprite = mastermind.White;
        }

        //Win the mastermind
        if(nbGoodPosition==4)
        {
            Debug.Log("You Win");
            Win();
            return;
        }

        //Loose the mastermind
        if(currentSlot==9)
        {
            Loose();
            Debug.Log("You loose");
            return;
        }

        currentSlot++; //Next slot

        //Change alpha of the current slot ans the previous slot
        Color originColor = gameSlot[currentSlot].GetComponent<Image>().color;
        Color selectionColor = originColor;
        selectionColor.a = 0.25f;
        gameSlot[currentSlot].GetComponent<Image>().color = selectionColor;
        gameSlot[currentSlot - 1].GetComponent<Image>().color = originColor;
    }
    
    void Win()
    {
        mastermind.HiddenSlot.SetActive(false);
    }

    void Loose()
    {
        mastermind.HiddenSlot.SetActive(false);
    }
}

