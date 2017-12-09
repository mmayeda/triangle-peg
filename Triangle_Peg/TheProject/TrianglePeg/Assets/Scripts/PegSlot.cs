using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PegSlot : MonoBehaviour
{
    public bool mSlotFilled;//Is there a peg at this slot
    public List<GameObject> mDestinationSlots;//Slots that can be jumped to from this one
    public List<GameObject> mJumpedSlots;//Parallel list of slots we would jump over

    //Sprites
    public Sprite emptySprite;
    public Sprite filledSprite;
    public Sprite selectedSprite;

	// Use this for initialization
	void Start ()
    {
        if (mSlotFilled)
        {
            GetComponent<SpriteRenderer>().sprite = filledSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptySprite;
        }
	}

    public void removePeg()
    {
        mSlotFilled = false;
        GetComponent<SpriteRenderer>().sprite = emptySprite;
    }

    public void addPeg()
    {
        mSlotFilled = true;
        GetComponent<SpriteRenderer>().sprite = filledSprite;
    }

    public void selectPeg()
    {
        GetComponent<SpriteRenderer>().sprite = selectedSprite;
    }

    public void deselectPeg()
    {
        GetComponent<SpriteRenderer>().sprite = filledSprite;
    }

    //Moves this peg slot to the given destination slot and returns true if such a move is legal.  Otherwise returns false
    public bool attemptMove(GameObject destinationPeg)
    {
        int destIndex = getDestinationIndex(destinationPeg);
        //If the move isn't in our list of valid jump destinations, the jump destination is filled, or 
        //the slot being jumped over isn't filled then return false
        if (destIndex==-1 || mDestinationSlots[destIndex].GetComponent<PegSlot>().mSlotFilled
            || !mJumpedSlots[destIndex].GetComponent<PegSlot>().mSlotFilled)
        {
            return false;
        }
        //Otherwise make the move
        removePeg();
        mDestinationSlots[destIndex].GetComponent<PegSlot>().addPeg();
        mJumpedSlots[destIndex].GetComponent<PegSlot>().removePeg();
        return true;
    }

    //Returns the index of the given game object in the mDestinationSlots list.  Returns -1 if the object is not in our list of destinations
    private int getDestinationIndex(GameObject destinationPeg)
    {
        for (int i=0;i<mDestinationSlots.Count;i++)
        {
            if (mDestinationSlots[i]==destinationPeg)
            {
                return i;
            }
        }
        return -1;
    }
}
