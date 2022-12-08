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

        public void InitDeck(string deckXMLText)
        {
            if (GameObject.Find("_Deck") == null)
            {
                GameObject anchorGO = new GameObject("_Deck");
                deckAnchor = anchorGO.transform;
            }

            ReadDeck(deckXMLText);
            MakeCards();
        }

        private void MakeCards()
        {
            throw new NotImplementedException();
        }

        private void ReadDeck(string deckXMLText)
        {
            throw new NotImplementedException();
        }
    }
}