namespace FreenoveBigHexapod.Client.Unity
{
    using UnityEngine;


    public class HexapodUserInput : MonoBehaviour
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const string Pitch = "Pitch";
        private const string Roll = "Roll";

        private const string Strafe = "Strafe";
        private const string Sneak = "Sneak";
        
        public HexapodClientInterface clientInterface;

        private Vector2 currentMovement = Vector2.zero;
        private bool isSneaking = true;

        public void Update()
        {
            if (!this.clientInterface.IsReady)
                return;

            if (Input.GetButton(Strafe))
                HandleStrafing();
            else
                HandleMovement();
            
            HandleHead();
            HandlePoseZ();
        }


        // Rotate head up, down, left, and right.
        private void HandleHead()
        {
            // Axis has to be inverted to get expected behavior.
            // Server does things backwards.
            int pitch = (int)-Input.GetAxisRaw(Pitch);
            int roll = (int)-Input.GetAxisRaw(Roll);
            
            if (pitch != 0)
                this.clientInterface.PitchHead(pitch);
            
            if (roll != 0)
                this.clientInterface.RollHead(roll);
        }


        private void HandlePoseZ()
        {
            if (Input.GetButtonDown(Sneak))
                this.clientInterface.SetPosition(0, 0, -20);
            else if (Input.GetButtonUp(Sneak))
                this.clientInterface.SetPosition(0, 0, 20);
        }
        
        // Movement where left and right cause bot to turn.
        private void HandleMovement()
        {
            int x = (int)Input.GetAxisRaw(Horizontal);
            int y = (int)Input.GetAxisRaw(Vertical);
            Vector2 newMovement = new Vector2(x, y);

            // Start moving
            if (newMovement != this.currentMovement
                && newMovement != Vector2.zero)
            {
                this.currentMovement = newMovement;
                
                if (x < 0)
                    this.clientInterface.Rotate(Rotation.CounterClockwise);
                else if (x > 0)
                    this.clientInterface.Rotate(Rotation.Clockwise);
                else
                    this.clientInterface.Move(0, y);
            }
            
            // Stop moving
            else if (this.currentMovement != Vector2.zero
                     && newMovement == Vector2.zero)
            {
                this.currentMovement = Vector2.zero;
                this.clientInterface.Stop();
            }
        }


        // Movement where left and right linearly move left and right.
        private void HandleStrafing()
        {
            int x = (int)Input.GetAxisRaw(Horizontal);
            int y = (int)Input.GetAxisRaw(Vertical);
            

            Vector2 newMovement = new Vector2(x, y);
    
            if (newMovement != this.currentMovement
                && newMovement != Vector2.zero)
            {
                this.currentMovement = newMovement;
                this.clientInterface.Move(x, y);
            }
            else if (this.currentMovement != Vector2.zero
                     && newMovement == Vector2.zero)
            {
                this.currentMovement = Vector2.zero;
                this.clientInterface.Stop();
            }
        }
    }
}