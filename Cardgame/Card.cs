using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame
{
    internal struct Card
    {
        //Properties
        public byte LeftAttack { get; private set; } //LeftAttack Value of the card

        public byte UpAttack { get; private set; } //UpAttack Value of the card
        public byte RightAttack { get; private set; } //RightAttack Value of the card
        public byte DownAttack { get; private set; } //DownAttack Value of the card
        public byte ValueOfCard { get; private set; } //Value of the card (Which is visually is in the upper right corner)

        //Index of the card in the window
        public sbyte index { get; set; }

        //Which Side if the card on Default is red
        public bool Side { get; set; }

        //Constructor
        public Card(byte LeftAttack, byte UpAttack, byte RightAttack, byte DownAttack, byte ValueOfCard)
        {
            this.LeftAttack = LeftAttack;
            this.UpAttack = UpAttack;
            this.RightAttack = RightAttack;
            this.DownAttack = DownAttack;
            this.ValueOfCard = ValueOfCard;
            this.index = -1;
            this.Side = false;
        }
    }
}