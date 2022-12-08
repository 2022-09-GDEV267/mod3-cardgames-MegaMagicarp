using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey
{
    public class MonkeySM : MonoBehaviour
    {
        public MDeck deck;
        public MLayout layout;
        public TextAsset MonkeyDeckXML;
        public TextAsset MonkeyLayoutXML;

        // Start is called before the first frame update
        void Start()
        {
            deck = GetComponent<MDeck>();
            deck.InitDeck(MonkeyDeckXML.text);
            MDeck.Shuffle(ref deck.cards);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}