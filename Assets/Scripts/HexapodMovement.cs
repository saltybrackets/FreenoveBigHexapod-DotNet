namespace FreenoveBigHexapod.Client
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;


    [Serializable]
    public class HexapodMovement
    {
        public const string MoveCommand = "CMD_MOVE";

        private const int MoveFactor = 35; // How long to hold movement if left uninterrupted.

        [Range(1,180)]
        public int RotationIncrementAngle = 10;
        
        [Range(1, 11)]
        public int Speed = 10;

        public GaitType GaitMode = GaitType.Normal;

        private HexapodClient client;


        public HexapodMovement(HexapodClient client)
        {
            this.client = client;
        }


        public void Stop()
        {
            string command = $"{MoveCommand}"
                             + $"#0"
                             + $"#0"
                             + $"#0"
                             + $"#0"
                             + $"#0";
            this.client.WriteToSocket(command);
        }


        public void Rotate(Rotation rotationDirection)
        {
            int sign = rotationDirection == Rotation.Clockwise
                           ? 1
                           : -1;

            int angle = this.RotationIncrementAngle * sign;
            int speed = this.Speed;
            int x = MoveFactor * sign;
            int y = 0;
            int gaitFlag = (int)this.GaitMode;
            
            string command = $"{MoveCommand}"
                             + $"#{gaitFlag}"
                             + $"#{x}"
                             + $"#{y}"
                             + $"#{speed}"
                             + $"#{angle}";
            this.client.WriteToSocket(command);
        }
        

        public void Move(
            int x,
            int y)
        {
            if (!this.client.SocketReady)
                return;

            if (y != 0)
            {
                if (y > 0)
                    y = MoveFactor;
                else if (y < 0)
                    y = -MoveFactor;
            }
            
            // Forward/back movement always takes priority.
            if (x != 0)
            {
                if (x > 0)
                    x = MoveFactor;
                else if (x < 0)
                    x = -MoveFactor;
            }

            int speed = this.Speed;
            int angle = 0;
            int gaitFlag = (int)this.GaitMode;
            
            string command = $"{MoveCommand}"
                             + $"#{gaitFlag}"
                             + $"#{x}"
                             + $"#{y}"
                             + $"#{speed}"
                             + $"#{angle}";
            this.client.WriteToSocket(command);
        }
    }
}