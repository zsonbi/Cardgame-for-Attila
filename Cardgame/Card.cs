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

        public byte LeftAttack { get; set; } //LeftAttack Value of the card
        public byte UpAttack { get; set; } //UpAttack Value of the card
        public byte RightAttack { get; set; } //RightAttack Value of the card
        public byte DownAttack { get; set; } //DownAttack Value of the card
        public byte Value { get; set; } //Value of the card (Which is visually is in the upper right corner)
        public Uri PictureUri { get; set; } //The Uri to the Picture
        public sbyte index { get; set; } //Index of the card in the window
        public bool Side { get; set; } //Which Side if the card on Default is red

        //---------------------------------------------------------------------------------------------
        //Constructor
        public Card(byte LeftAttack, byte UpAttack, byte RightAttack, byte DownAttack, byte Value, string PictureUri)
        {
            this.LeftAttack = LeftAttack;
            this.UpAttack = UpAttack;
            this.RightAttack = RightAttack;
            this.DownAttack = DownAttack;
            this.Value = Value;
            this.index = -1;
            this.Side = false;
            this.PictureUri = new Uri(PictureUri);
        }
    }
}