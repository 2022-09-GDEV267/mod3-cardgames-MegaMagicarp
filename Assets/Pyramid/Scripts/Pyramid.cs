using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Pyramid;

namespace Pyramid
{
    public class Pyramid : MonoBehaviour
    {
        static public Pyramid S;

        [Header("Set in Inspector")]
        public TextAsset deckXML;
        public TextAsset layoutXML;
        public float xOffset = 3;
        public float yOffset = -2.5f;
        public Vector3 layoutCenter;
        public Vector2 fsPosMid = new Vector2(0.5f, 0.90f);
        public Vector2 fsPosRun = new Vector2(0.5f, 0.75f);
        public Vector2 fsPosMid2 = new Vector2(0.4f, 1.0f);
        public Vector2 fsPosEnd = new Vector2(0.5f, 0.95f);
        public float reloadDelay = 2f;// 2 sec delay between rounds
        public Text gameOverText, roundResultText, highScoreText;

        [Header("Set Dynamically")]
        public List<CardPyramid> possibleMatch;
        public Deck deck;
        public Pyramid_Layout layout;
        public List<CardPyramid> drawPile;
        public Transform layoutAnchor;
        public List<CardPyramid> targets;
        public List<CardPyramid> tableau;
        public List<CardPyramid> discardPile;

        void Awake()
        {
            S = this;
        }

        void Start()
        {
            deck = GetComponent<Deck>();
            deck.InitDeck(deckXML.text);
            Deck.Shuffle(ref deck.cards); // This shuffles the deck by reference
            layout = GetComponent<Pyramid_Layout>();  // Get the Layout component

            layout.ReadLayout(layoutXML.text); // Pass LayoutXML to it
            drawPile = ConvertListCardsToListCardPyramid(deck.cards);
            targets = new List<CardPyramid>();
            possibleMatch = new List<CardPyramid>();
            LayoutGame();
        }
        List<CardPyramid> ConvertListCardsToListCardPyramid(List<Card> lCD)
        {
            List<CardPyramid> lCP = new List<CardPyramid>();
            CardPyramid tCP;
            foreach (Card tCD in lCD)
            {
                tCP = tCD as CardPyramid;
                lCP.Add(tCP);
            }
            return (lCP);
        }

        CardPyramid Draw()
        {
            CardPyramid cd = drawPile[0]; // Pull the 0th CardPyramid
            drawPile.RemoveAt(0);            // Then remove it from List<> drawPile
            return (cd);                      // And return it
        }

        // LayoutGame() positions the initial tableau of cards, a.k.a. the "mine"
        void LayoutGame()
        {
            // Create an empty GameObject to serve as an anchor for the tableau
            if (layoutAnchor == null)
            {
                GameObject tGO = new GameObject("_LayoutAnchor");
                // ^ Create an empty GameObject named _LayoutAnchor in the Hierarchy

                layoutAnchor = tGO.transform;              // Grab its Transform
                layoutAnchor.transform.position = layoutCenter;   // Position it
            }

            CardPyramid cp;
            // Follow the layout
            foreach (SlotDef tSD in layout.slotDefs)
            {
                // ^ Iterate through all the SlotDefs in the layout.slotDefs as tSD
                cp = Draw(); // Pull a card from the top (beginning) of the draw Pile
                cp.faceUp = tSD.faceUp;  // Set its faceUp to the value in SlotDef
                cp.transform.parent = layoutAnchor; // Make its parent layoutAnchor
                                                    // This replaces the previous parent: deck.deckAnchor, which
                                                    //  appears as _Deck in the Hierarchy when the scene is playing.
                cp.transform.localPosition = new Vector3(layout.multiplier.x * tSD.x,
                    layout.multiplier.y * tSD.y, -tSD.layerID);
                // ^ Set the localPosition of the card based on slotDef
                cp.layoutID = tSD.id;
                cp.slotDef = tSD;
                cp.SetSortingLayerName(tSD.layerName); // Set the sorting layers
                                                       // CardPyramids in the tableau have the state CardState.tableau
                cp.state = ePyramidCardState.tableau;
                tableau.Add(cp); // Add this CardPyramid to the List<> tableau
            }

            // Set which cards are hiding others
            foreach (CardPyramid tCP in tableau)
            {
                foreach (int hid in tCP.slotDef.hiddenBy)
                {
                    cp = FindCardByLayoutID(hid);
                    tCP.hiddenBy.Add(cp);
                }
            }
            // Set up the Draw pile
            UpdateDrawPile();
        }

        // Convert from the layoutID int to the CardPyramid with that ID
        CardPyramid FindCardByLayoutID(int layoutID)
        {
            foreach (CardPyramid tCP in tableau)
            {
                // Search through all cards in the tableau List<>
                if (tCP.layoutID == layoutID)
                {
                    // If the card has the same ID, return it
                    return (tCP);
                }
            }
            // If it's not found, return null
            return (null);
        }

        // This turns cards in the Mine face-up or face-down
        void SetTableauFaces()
        {
            foreach (CardPyramid cd in tableau)
            {
                bool faceUp = true; // Assume the card will be face-up
                foreach (CardPyramid cover in cd.hiddenBy)
                {
                    // If either of the covering cards are in the tableau
                    if (cover.state == ePyramidCardState.tableau)
                    {
                        faceUp = false; // then this card is face-down
                    }
                }
                cd.faceUp = faceUp; // Set the value on the card
            }
        }

        // Moves the current target to the discardPile
        void MoveToDiscard(CardPyramid cd)
        {
            // Set the state of the card to discard
            cd.state = ePyramidCardState.discard;
            discardPile.Add(cd); // Add it to the discardPile List<>
            cd.transform.parent = layoutAnchor; // Update its transform parent

            // Position this card on the discardPile
            cd.transform.localPosition = new Vector3(
                layout.multiplier.x * layout.discardPile.x,
                layout.multiplier.y * layout.discardPile.y,
                -layout.discardPile.layerID + 0.5f);
            cd.faceUp = false;

            // Place it on top of the pile for depth sorting
            cd.SetSortingLayerName(layout.discardPile.layerName);
            cd.SetSortOrder(-100 + discardPile.Count);
        }

        // Make cd the new target card
        void MoveToTarget(CardPyramid cd)
        {
            // If there is currently a target card, move it to discardPile
            //if (target != null) MoveToDiscard(target);
            targets.Insert(0,cd); // cd is the new target
            cd.state = ePyramidCardState.target;
            cd.transform.parent = layoutAnchor;

            // Move to the target position
            cd.transform.localPosition = new Vector3(
                layout.multiplier.x * layout.targetPile.x,
                layout.multiplier.y * layout.targetPile.y,
                -layout.targetPile.layerID);

            cd.faceUp = true; // Make it face-up

            for (int i = 0; i < targets.Count; i++)
            {
                cd = targets[i];
                cd.transform.parent = layoutAnchor;
                
                cd.transform.localPosition = new Vector3(
                layout.multiplier.x * (layout.targetPile.x),
                layout.multiplier.y * (layout.targetPile.y),
                -layout.targetPile.layerID + 0.1f * i);
                cd.state = ePyramidCardState.target;

                // Set depth sorting
                cd.SetSortingLayerName(layout.drawPile.layerName);
                cd.SetSortOrder(-10 * i);
            }
        }

        // Arranges all the cards of the drawPile to show how many are left
        void UpdateDrawPile()
        {
            CardPyramid cd;

            // Go through all the cards of the drawPile
            for (int i = 0; i < drawPile.Count; i++)
            {
                cd = drawPile[i];
                cd.transform.parent = layoutAnchor;

                // Position it correctly with the layout.drawPile.stagger
                Vector2 dpStagger = layout.drawPile.stagger;
                cd.transform.localPosition = new Vector3(
                layout.multiplier.x * (layout.drawPile.x + i * dpStagger.x),
                layout.multiplier.y * (layout.drawPile.y + i * dpStagger.y),
                -layout.drawPile.layerID + 0.1f * i);

                cd.faceUp = false; // Make them all face-down
                cd.state = ePyramidCardState.drawpile;

                // Set depth sorting
                cd.SetSortingLayerName(layout.drawPile.layerName);
                cd.SetSortOrder(-10 * i);
            }
        }

        // CardClicked is called any time a card in the game is clicked
        public void CardClicked(CardPyramid cd)
        {
            // The reaction is determined by the state of the clicked card
            switch (cd.state)
            {
                case ePyramidCardState.target:
                    if(cd.rank == 13)
                    {
                        MoveToDiscard(cd);
                        possibleMatch.Clear();
                    }
                    else
                    {
                        possibleMatch.Add(cd);
                    }
                    break;

                case ePyramidCardState.drawpile:
                    // Clicking any card in the drawPile will draw the next card
                    MoveToTarget(Draw());  // Moves the next drawn card to the target
                    UpdateDrawPile();     // Restacks the drawPile
                    possibleMatch.Clear();
                    break;

                case ePyramidCardState.tableau:
                    // Clicking a card in the tableau will check if it's a valid play
                    if (!cd.faceUp)
                    {
                        possibleMatch.Clear();
                        return;
                    }
                    else
                    {
                        possibleMatch.Add(cd);
                    }
                    
                    if(possibleMatch.Count == 2)
                    {
                        if (Add13(possibleMatch[0], possibleMatch[1]))
                        {
                            Debug.Log("Found a pair!");
                        }
                        else
                        {
                            Debug.Log("Not a match!");
                        }
                        possibleMatch.Clear();
                    }
                    //if (!Add13(cd, targets[0]))
                    //{
                    //    // If it's not an adjacent rank, it's not valid
                    //    validMatch = false;
                    //}

                    //tableau.Remove(cd); // Remove it from the tableau List
                    //MoveToTarget(cd);  // Make it the target card
                    SetTableauFaces();  // Update tableau card face-ups
                    break;
            }
            // Check to see whether the game is over or not
            CheckForGameOver();
        }

        // Test whether the game is over
        void CheckForGameOver()
        {
            // If the tableau is empty, the game is over
            if (tableau.Count == 0)
            {
                // Call GameOver() with a win
                GameOver(true);
                return;
            }

            // If there are still cards in the draw pile, the game's not over
            if (drawPile.Count > 0)
            {
                return;
            }

            // Check for remaining valid plays
            foreach (CardPyramid cd in tableau)
            {
                if (Add13(cd, targets[0]))
                {
                    // If there is a valid play, the game's not over
                    return;
                }
            }

            // Since there are no valid plays, the game is over
            // Call GameOver with a loss
            GameOver(false);
        }

        // Called when the game is over. Simple for now, but expandable
        void GameOver(bool won)
        {
            Invoke("ReloadLevel", reloadDelay);
        }

        void ReloadLevel()
        {
            // Reload the scene, resetting the game
            SceneManager.LoadScene("Pyramid");
        }

        // Return true if the two cards are adjacent in rank (A & K wrap around)
        public bool Add13(CardPyramid c0, CardPyramid c1)
        {
            // If either card is face-down, it's not adjacent.
            if (!c0.faceUp || !c1.faceUp) return (false);

            // If they are 1 apart, they are adjacent
            if (Mathf.Abs(c0.rank + c1.rank) == 13)
            {
                return (true);
            }

            // If one is Ace and the other King, they are adjacent
            if (c0.rank == 13) return (true);

            // Otherwise, return false
            return (false);
        }
    }
}