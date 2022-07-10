namespace FreenoveBigHexapod.Client.Unity
{
    using UnityEngine;


    public class HexapodUserInput : MonoBehaviour
    {
        public HexapodClientInterface clientInterface;

        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        private Vector2 currentMovement;
        
        public void Update()
        {
            if (!this.clientInterface.IsReady)
                return;

            float x = Input.GetAxisRaw(Horizontal);
            float y = Input.GetAxisRaw(Vertical);

            Vector2 newMovement = new Vector2(x, y);
    
            if (newMovement != this.currentMovement
                && newMovement != Vector2.zero)
            {
                this.clientInterface.Move(x, y);
                this.currentMovement = newMovement;
            }
            else if (this.currentMovement != Vector2.zero
                     && newMovement == Vector2.zero)
            {
                this.clientInterface.Stop();
                this.currentMovement = Vector2.zero;
            }
        }
    }
}