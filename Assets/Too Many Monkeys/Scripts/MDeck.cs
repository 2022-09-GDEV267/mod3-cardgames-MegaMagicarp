using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey
{
    public class MDeck : MonoBehaviour
    {
        [Header("Set in Inspector")]
        public Sprite[] faceSprites;
        public Sprite cardBack;
        public Sprite cardFront;

        // Prefabs
        public GameObject prefabSprite;
        public GameObject prefabCard;

        [Header("Set Dynamically")]
        public PT_XMLReader xmlr;
        public List<string> cardNames;
        public List<Card> cards;
        public Transform deckAnchor;
        public List<MCardDefinition> cardDefs;

        public void InitDeck(string MonkeyDeckXMLText)
        {
            if (GameObject.Find("_Deck") == null)
            {
                GameObject anchorGO = new GameObject("_Deck");
                deckAnchor = anchorGO.transform;
            }

            ReadDeck(MonkeyDeckXMLText);
            MakeCards();
        }

        private void ReadDeck(string MonkeyDeckXMLText)
        {
            xmlr = new PT_XMLReader();
            xmlr.Parse(MonkeyDeckXMLText);
            cardDefs = new List<MCardDefinition>();
            PT_XMLHashList xCardDefs = xmlr.xml["xml"][0]["card"];

            for (int i = 0; i < xCardDefs.Count; i++)
            {
                // for each carddef in the XML, copy attributes and set up in cDef
                MCardDefinition cDef = new MCardDefinition();
                cDef.rank = int.Parse(xCardDefs[i].att("rank"));
                cDef.count = int.Parse(xCardDefs[i].att("count"));

                // if it's a face card, map the proper sprite
                if (xCardDefs[i].HasAtt("face"))
                {
                    cDef.face = xCardDefs[i].att("face");
                }
                cardDefs.Add(cDef);
            }
        }

        private void MakeCards()
        {
            // list of all Cards
            cards = new List<Card>();

            // temp variables
            Sprite tS = null;
            GameObject tGO = null;
            SpriteRenderer tSR = null;

            for (int i = 0; i < cardDefs[0].count; i++)
            {
                GameObject cgo = Instantiate(prefabCard) as GameObject;
                cgo.transform.parent = deckAnchor;
                Card card = cgo.GetComponent<Card>();
                card.name = cardDefs[0].face + "_" + i;
                card.rank = cardDefs[0].rank;
                cgo.transform.localPosition = new Vector3(i % 13 * 3, i / 13 * 3, 0);

                //if (card.def.face != "")
                {
                    tGO = Instantiate(prefabSprite) as GameObject;
                    tSR = tGO.GetComponent<SpriteRenderer>();

                    //tS = GetFace(card.def.face + card.suit);
                    tSR.sprite = faceSprites[0];
                    tSR.sortingOrder = 1;
                    tGO.transform.parent = cgo.transform;
                    tGO.transform.localPosition = Vector3.zero;
                    tGO.name = "face";
                }

                cards.Add(card);
            }

            for (int i = 0; i < cardDefs[1].count; i++)
            {
                GameObject cgo = Instantiate(prefabCard) as GameObject;
                cgo.transform.parent = deckAnchor;
                Card card = cgo.GetComponent<Card>();
                card.name = cardDefs[1].face + "_" + i;
                card.rank = cardDefs[1].rank;
                cgo.transform.localPosition = new Vector3(i % 13 * 3, i / 13 * 4, 1);

                //if (card.def.face != "")
                {
                    tGO = Instantiate(prefabSprite) as GameObject;
                    tSR = tGO.GetComponent<SpriteRenderer>();

                    //tS = GetFace(card.def.face + card.suit);
                    tSR.sprite = faceSprites[1];
                    tSR.sortingOrder = 1;
                    tGO.transform.parent = cgo.transform;
                    tGO.transform.localPosition = Vector3.zero;
                    tGO.name = "face";
                }

                cards.Add(card);
            }

            for (int i = 0; i < cardDefs[2].count; i++)
            {
                GameObject cgo = Instantiate(prefabCard) as GameObject;
                cgo.transform.parent = deckAnchor;
                Card card = cgo.GetComponent<Card>();
                card.name = cardDefs[2].face + "_" + i;
                card.rank = cardDefs[2].rank;
                cgo.transform.localPosition = new Vector3(i % 13 * 3, i / 13 * 5, 2);

                //if (card.def.face != "")
                {
                    tGO = Instantiate(prefabSprite) as GameObject;
                    tSR = tGO.GetComponent<SpriteRenderer>();

                    //tS = GetFace(card.def.face + card.suit);
                    tSR.sprite = faceSprites[2];
                    tSR.sortingOrder = 1;
                    tGO.transform.parent = cgo.transform;
                    tGO.transform.localPosition = Vector3.zero;
                    tGO.name = "face";
                }

                cards.Add(card);
            }

            for (int i = 0; i < cardDefs[3].count; i++)
            {
                GameObject cgo = Instantiate(prefabCard) as GameObject;
                cgo.transform.parent = deckAnchor;
                Card card = cgo.GetComponent<Card>();
                card.name = cardDefs[3].face + "_" + i;
                card.rank = cardDefs[3].rank;
                cgo.transform.localPosition = new Vector3(i % 13 * 3, i / 13 * 6, 3);

                //if (card.def.face != "")
                {
                    tGO = Instantiate(prefabSprite) as GameObject;
                    tSR = tGO.GetComponent<SpriteRenderer>();

                    //tS = GetFace(card.def.face + card.suit);
                    tSR.sprite = faceSprites[3];
                    tSR.sortingOrder = 1;
                    tGO.transform.parent = cgo.transform;
                    tGO.transform.localPosition = Vector3.zero;
                    tGO.name = "face";
                }

                cards.Add(card);
            }

            for (int i = 0; i < cardDefs[4].count; i++)
            {
                GameObject cgo = Instantiate(prefabCard) as GameObject;
                cgo.transform.parent = deckAnchor;
                Card card = cgo.GetComponent<Card>();
                card.name = cardDefs[4].face + "_" + i;
                card.rank = cardDefs[4].rank;
                cgo.transform.localPosition = new Vector3(i % 13 * 3, i / 13 * 6, 4);

                //if (card.def.face != "")
                {
                    tGO = Instantiate(prefabSprite) as GameObject;
                    tSR = tGO.GetComponent<SpriteRenderer>();

                    //tS = GetFace(card.def.face + card.suit);
                    tSR.sprite = faceSprites[4];
                    tSR.sortingOrder = 1;
                    tGO.transform.parent = cgo.transform;
                    tGO.transform.localPosition = Vector3.zero;
                    tGO.name = "face";
                }

                cards.Add(card);
            }

            for (int i = 0; i < cardDefs[5].count; i++)
            {
                GameObject cgo = Instantiate(prefabCard) as GameObject;
                cgo.transform.parent = deckAnchor;
                Card card = cgo.GetComponent<Card>();
                card.name = cardDefs[5].face + "_" + i;
                card.rank = cardDefs[5].rank;
                cgo.transform.localPosition = new Vector3(i % 13 * 3, i / 13 * 6, 5);

                //if (card.def.face != "")
                {
                    tGO = Instantiate(prefabSprite) as GameObject;
                    tSR = tGO.GetComponent<SpriteRenderer>();

                    //tS = GetFace(card.def.face + card.suit);
                    tSR.sprite = faceSprites[5];
                    tSR.sortingOrder = 1;
                    tGO.transform.parent = cgo.transform;
                    tGO.transform.localPosition = Vector3.zero;
                    tGO.name = "face";
                }

                cards.Add(card);
            }


            //    card.def = GetCardDefinitionByRank(card.rank);

            //    //Handle face cards
            //    if (card.def.face != "")
            //    {
            //        tGO = Instantiate(prefabSprite) as GameObject;
            //        tSR = tGO.GetComponent<SpriteRenderer>();

            //        tS = GetFace(card.def.face + card.suit);
            //        tSR.sprite = tS;
            //        tSR.sortingOrder = 1;
            //        tGO.transform.parent = card.transform;
            //        tGO.transform.localPosition = Vector3.zero; 
            //        tGO.name = "face";
            //    }

            //    tGO = Instantiate(prefabSprite) as GameObject;
            //    tSR = tGO.GetComponent<SpriteRenderer>();
            //    tSR.sprite = cardBack;
            //    tGO.transform.SetParent(card.transform);
            //    tGO.transform.localPosition = Vector3.zero;
            //    tSR.sortingOrder = 2;
            //    tGO.name = "back";
            //    card.back = tGO;
            //    card.faceUp = false;
        }
    

        internal static void Shuffle(ref List<Card> cards)
        {
            //List<Card> tCards = new List<Card>();

            //int ndx;   // which card to move

            //tCards = new List<Card>();

            //while (oCards.Count > 0)
            //{
            //    // find a random card, add it to shuffled list and remove from original deck
            //    ndx = Random.Range(0, oCards.Count);
            //    tCards.Add(oCards[ndx]);
            //    oCards.RemoveAt(ndx);
            //}

            //oCards = tCards;
            ////because oCards is a ref parameter, the changes made are propogated back
            ////for ref paramters changes made in the function persist.
        }
    }
}