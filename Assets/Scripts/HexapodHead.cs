namespace FreenoveBigHexapod.Client
{
    using System;
    using UnityEngine;


    [Serializable]
    public class HexapodHead
    {
        public const string HeadMoveCommand = "CMD_HEAD";

        private const int MaxPitch = 180;
        private const int MinPitch = 50;
        private const int MaxRoll = 180;
        private const int MinRoll = 0;
        
        private HexapodClient client;
        private int currentPitch = 115;
        private int currentRoll = 90;

        
        public HexapodHead(HexapodClient client)
        {
            this.client = client;
        }


        public int CurrentPitch
        {
            get { return this.currentPitch; }
        }

        public int CurrentRoll
        {
            get { return this.currentRoll; }
        }


        /// <summary>
        /// Pitch head forward or back by given amount.
        /// </summary>
        /// <param name="amount">Amount to pitch.</param>
        public void Pitch(int amount)
        {
            int angle = this.CurrentPitch + amount;
            SetPitch(angle);
        }

        
        /// <summary>
        /// Roll head left or right by a given amount.
        /// </summary>
        /// <param name="amount">Amount to roll.</param>
        public void Roll(int amount)
        {
            int angle = this.CurrentRoll + amount;
            SetRoll(angle);
        }


        /// <summary>
        /// Move head up and down.
        /// Max down is 50.
        /// Max up is 180.
        /// </summary>
        /// <param name="angle">Angle between 50 and 180.</param>
        public void SetPitch(int angle)
        {
            if (angle is < MinPitch or > MaxPitch)
                return;
            
            string command = $"{HeadMoveCommand}"
                             + $"#0"
                             + $"#{angle}";
            this.client.WriteToSocket(command);
            this.currentPitch = angle;
        }


        /// <summary>
        /// Move head left and right.
        /// Max right is 0.
        /// Max left is 180.
        /// </summary>
        /// <param name="angle">Angle between 0 and 180.</param>
        public void SetRoll(int angle)
        {
            if (angle is < MinRoll or > MaxRoll)
                return;
           
            string command = $"{HeadMoveCommand}"
                             + $"#1"
                             + $"#{angle}";
            this.client.WriteToSocket(command);
            this.currentRoll = angle;
        }
    }
}