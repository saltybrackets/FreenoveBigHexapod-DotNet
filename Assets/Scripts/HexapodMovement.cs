namespace FreenoveBigHexapod.Client
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;


    [Serializable]
    public class HexapodMovement
    {
        public const string MoveCommand = "CMD_MOVE";

        private const int MoveFactor = 35;

        [Range(1, 10)]
        public int Speed = 10;

        [Range(1, 2)]
        public int GaitMode = 1;

        public bool ClampSpeed = true;

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
        

        public void Move(
            float x,
            float y)
        {
            Debug.Log($"Moving: x{x} y{y}");
            
            if (!this.client.SocketReady)
                return;

            int speed = 1;

            if (y != 0)
            {
                speed = this.ClampSpeed 
                            ? (int)Mathf.Abs((Mathf.Sign(y) * this.Speed)) 
                            : Mathf.Abs(Mathf.RoundToInt(y * this.Speed));
                if (y > 0)
                    y = MoveFactor;
                else if (y < 0)
                    y = -MoveFactor;
            }
            
            // Forward/back movement always takes priority.
            else if (x != 0)
            {
                speed = this.ClampSpeed  
                            ? (int)Mathf.Abs((Mathf.Sign(x) * this.Speed))
                            : Mathf.Abs(Mathf.RoundToInt(x * this.Speed));
                if (x > 0)
                    x = MoveFactor;
                else if (x < 0)
                    x = -MoveFactor;
            }

            // TODO
            int angle = 0;
            int gaitFlag = 1;
            
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