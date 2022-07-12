namespace FreenoveBigHexapod.Client.Unity
{
    using UnityEngine;


    public class HexapodUserInput : MonoBehaviour
    {
        public HexapodClientInterface clientInterface;

        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const string Pitch = "Pitch";
        private const string Roll = "Roll";

        private const string Strafe = "Strafe";

        private Vector2 currentMovement;
        
        public void Update()
        {
            if (!this.clientInterface.IsReady)
                return;

            HandleMovement();
            HandleHead();
        }


        private void HandleHead()
        {
            // Note: Axis has to be inverted, because does things backwards.
            int pitch = (int)-Input.GetAxisRaw(Pitch);
            int roll = (int)-Input.GetAxisRaw(Roll);
            
            if (pitch != 0)
                this.clientInterface.PitchHead(pitch);
            
            
            if (roll != 0)
                this.clientInterface.RollHead(roll);
            
        }
        
        private void HandleMovement()
        {
            int x = (int)Input.GetAxisRaw(Horizontal);
            int y = (int)Input.GetAxisRaw(Vertical);
            bool strafeHeld = Input.GetButton(Strafe);

            Vector2 newMovement = new Vector2(x, y);
    
            if (newMovement != this.currentMovement
                && newMovement != Vector2.zero)
            {
                this.currentMovement = newMovement;
                if (strafeHeld)
                    this.clientInterface.Move(x, y);
                else
                {
                    if (x < 0)
                        this.clientInterface.Rotate(Rotation.CounterClockwise);
                    else if (x > 0)
                        this.clientInterface.Rotate(Rotation.Clockwise);
                    else
                        this.clientInterface.Move(0, y);
                }
                    
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