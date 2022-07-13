namespace FreenoveBigHexapod.Client
{
    using System;


    [Serializable]
    public class HexapodPosing
    {   // X, Y, Z
        // X, Y -> -40 to 40
        // Z -> -20 to 20
        // CMD_POSITION#0#0#-1
        public const string PositionCommand = "CMD_POSITION";

        
        // Left/Right, Forward/Back, Twist
        public const string AttitudeCommand = "CMD_ATTITUDE";

        private HexapodClient client;


        public HexapodPosing(HexapodClient client)
        {
            this.client = client;
        }


        public void SetPosition(
            int x, 
            int y, 
            int z)
        {
            string command = $"{PositionCommand}"
                             + $"#{x}"
                             + $"#{y}"
                             + $"#{z}";
            this.client.WriteToSocket(command);
        }


        public void SetAttitude(int roll, int pitch, int yaw)
        {
            
        }
    }
}