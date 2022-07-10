namespace FreenoveBigHexapod.Client.Unity
{
    using UnityEngine;


    public class HexapodUserInput : MonoBehaviour
    {
        public HexapodClientInterface clientInterface;

        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        public bool IsMoving { get; set; }
        
        public void Update()
        {
            if (!this.clientInterface.IsReady)
                return;

            float horizontal = Input.GetAxis(Horizontal);
            float vertical = Input.GetAxis(Vertical);

            bool hasMoveInput = horizontal != 0 
                                || vertical != 0;
                
            if (!this.IsMoving 
                && hasMoveInput)
            {
                this.clientInterface.Move(horizontal, vertical);
                this.IsMoving = true;
            }
            else if (this.IsMoving
                     && !hasMoveInput)
            {
                this.clientInterface.Stop();
                this.IsMoving = false;
            }
        }
    }
}