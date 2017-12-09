using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    private GameObject mPegSelected;//Is a peg currently selected for a move?
    public int mPegSlotLayer;//Layer the peg slots are in.  Used for raycast layer mask
    public int mNumPegs;

    //Audio clips
    public AudioClip select;
    public AudioClip movePeg;
    public AudioClip wrongMove;
    public AudioClip win;
    private AudioSource mAudioSource;

    //Images
    public Image instructionsImage;
    public Image youWinImage;

    // Use this for initialization
    void Start()
    {
        mPegSelected = null;
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))//Check for mouse click and handle
        {
            handleMouseClick();
        }
        else if (Input.GetKeyDown(KeyCode.Z))//Restart game on Z press
        {
            SceneManager.LoadScene("PegTriangle");
        }
        else if (Input.GetKeyDown(KeyCode.H))//Display Instructions menu
        {
            instructionsImage.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.H))//Hide Instructions menu
        {
            instructionsImage.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))//End game on ESC press
        {
            Application.Quit();
        }
    }

    private void handleMouseClick()
    {
        GameObject clickedPeg = getPegFromMousePosition();
        if (mPegSelected!=null)//Currently have a first peg selected
        {
            if (clickedPeg == null)//Deselect selected peg if new click isn't on a peg
            {
                mPegSelected.GetComponent<PegSlot>().deselectPeg();
                mPegSelected = null;
            }
            else//Otherwise attempt to move selected peg to the new clicked peg
            {
                if (mPegSelected.GetComponent<PegSlot>().attemptMove(clickedPeg))
                {
                    mNumPegs--;
                    mAudioSource.clip = movePeg;
                    mAudioSource.Play();
                    if (mNumPegs==1)
                    {
                        //You win
                        youWinImage.enabled = true;
                        mAudioSource.clip = win;
                        mAudioSource.Play();
                    }
                }
                else//Move was not a success
                {
                    mPegSelected.GetComponent<PegSlot>().deselectPeg();
                    mAudioSource.clip = wrongMove;
                    mAudioSource.Play();
                }
                mPegSelected = null;
            }
        }
        else//No first peg selected
        {
            if (clickedPeg!=null && clickedPeg.GetComponent<PegSlot>().mSlotFilled)
            {
                mPegSelected = clickedPeg;
                clickedPeg.GetComponent<PegSlot>().selectPeg();
                mAudioSource.clip = select;
                mAudioSource.Play();
            }
        }
    }

    //Returns a peg slot given the current mouse position. Returns null if mouse not over any pegs
    private GameObject getPegFromMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int raycastMask = 1 <<mPegSlotLayer;//Only look for colliders in the layer where the peg slot colliders are
        RaycastHit2D hit=Physics2D.Raycast(ray.origin,ray.direction,Mathf.Infinity,raycastMask);

        if (hit.collider!=null)
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}
