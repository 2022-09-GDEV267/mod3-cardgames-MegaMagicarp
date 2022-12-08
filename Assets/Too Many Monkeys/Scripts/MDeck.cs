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
        private List<CardDefinition> cardDefs;

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
            cardDefs = new List<CardDefinition>();
            PT_XMLHashList xCardDefs = xmlr.xml["xml"][0]["card"];

            for (int i = 0; i < xCardDefs.Count; i++)
            {
                // for each carddef in the XML, copy attributes and set up in cDef
                CardDefinition cDef = new CardDefinition();
                cDef.rank = int.Parse(xCardDefs[i].att("rank"));

                // if it's a face card, map the proper sprite
                // foramt is ##A, where ## in 11, 12, 13 and A is letter indicating suit
                if (xCardDefs[i].HasAtt("face"))
                {
                    cDef.face = xCardDefs[i].att("face");
                }
                cardDefs.Add(cDef);
            }
        }

        private void MakeCards()
        {
            throw new NotImplementedException();
        }

        internal static void Shuffle(ref List<Card> cards)
        {
            throw new NotImplementedException();
        }
    }
}