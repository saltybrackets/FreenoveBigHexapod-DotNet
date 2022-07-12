namespace FreenoveBigHexapod.Client
{
    using System;
    using UnityEngine;


    [Serializable]
    public class HexapodHead
    {
        // CMD_HEAD#0#50 to 180 ---- (down to up)
        // CMD_HEAD#1#180 to 0 (left to right)
        public const string HeadMoveCommand = "CMD_HEAD";

        public float Speed = 0.1f;
        
        private HexapodClient client;
        private float currentPitch = 115;
        private float currentRoll = 90;


        public HexapodHead(HexapodClient client)
        {
            this.client = client;
        }


        public float CurrentPitch
        {
            get { return this.currentPitch; }
        }

        public float CurrentRoll
        {
            get { return this.currentRoll; }
        }


        public void Pitch(int amount)
        {
            float angle = this.CurrentPitch + (amount * this.Speed);
            SetPitch(angle);
        }


        public void Roll(int amount)
        {
            float angle = this.CurrentRoll + (amount * this.Speed);
            SetRoll(angle);
        }
        

        /// <summary>
        /// Move head up and down.
        /// Max down is 50.
        /// Max up is 180.
        /// </summary>
        /// <param name="angle">Angle between 50 and 180.</param>
        public void SetPitch(float angle)
        {
            if (angle is < 50 or > 180)
                return;
            
            this.currentPitch = angle;
            string command = $"{HeadMoveCommand}"
                             + $"#0"
                             + $"#{(int)angle}";
            this.client.WriteToSocket(command);
            
        }


        /// <summary>
        /// Move head left and right.
        /// Max right is 0.
        /// Max left is 180.
        /// </summary>
        /// <param name="angle">Angle between 0 and 180.</param>
        public void SetRoll(float angle)
        {
            if (angle is < 0 or > 180)
                return;
            
            this.currentRoll = angle;
            string command = $"{HeadMoveCommand}"
                             + $"#1"
                             + $"#{(int)angle}";
            this.client.WriteToSocket(command);
        }
    }
}