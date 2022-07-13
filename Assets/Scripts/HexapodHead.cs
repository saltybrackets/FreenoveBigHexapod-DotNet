namespace FreenoveBigHexapod.Client
{
    using System;
    using UnityEngine;


    [Serializable]
    public class HexapodHead
    {
        public const string HeadMoveCommand = "CMD_HEAD";
        public const int MaxPitch = 180;
        public const int MinPitch = 50;
        public const int MaxRoll = 180;
        public const int MinRoll = 0;

        private HexapodClient client;


        public HexapodHead(HexapodClient client)
        {
            this.client = client;
        }


        public int CurrentPitch { get; private set; } = 115;
        public int CurrentRoll { get; private set; } = 90;


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
            this.CurrentPitch = angle;
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
            this.CurrentRoll = angle;
        }
    }
}