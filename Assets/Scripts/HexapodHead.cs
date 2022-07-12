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

        public float Speed = 0.3f;
        
        private HexapodClient client;
        private int currentPitch = 115;
        private int currentRoll = 90;

        private float timer;

        public HexapodHead(HexapodClient client)
        {
            this.client = client;
        }


        public void IncrementTimer(float amount)
        {
            this.timer += amount;
        }
        
        public int CurrentPitch
        {
            get { return this.currentPitch; }
        }

        public int CurrentRoll
        {
            get { return this.currentRoll; }
        }


        public void Pitch(int amount)
        {
            int angle = this.CurrentPitch + amount;
            SetPitch(angle);
        }


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
            if (angle is < 50 or > 180)
                return;
            
            if (this.timer < this.Speed)
                return;
            
            this.timer = 0;
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
            if (angle is < 0 or > 180)
                return;
            
            if (this.timer < this.Speed)
                return;
            
            this.timer = 0;
            string command = $"{HeadMoveCommand}"
                             + $"#1"
                             + $"#{angle}";
            this.client.WriteToSocket(command);
            this.currentRoll = angle;
        }
    }
}