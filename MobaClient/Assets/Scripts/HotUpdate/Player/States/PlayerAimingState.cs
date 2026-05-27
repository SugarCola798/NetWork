using UnityEngine;

namespace HotUpdate.Player.States
{
    public class PlayerAimingState : PlayerState
    {
        private int aimingHashX;
        private int aimingHashY;
        private float aimingX;
        private float aimingY;

        private float transitionSpeed = 5;
        
        public PlayerAimingState(PlayerController controller) : base(controller)
        {
               
        }
        
        public override void Enter()
        {
            Debug.Log("Entered PlayerAimingState");
            aimingHashX = Animator.StringToHash("AimingX");
            aimingHashY = Animator.StringToHash("AimingY");
            Controller.PlayAnimation("Aiming");
            Controller.EnterAim();
        }

        public override void Tick()
        {
            base.Tick();
            aimingX = Mathf.Lerp(aimingX, Controller.MoveInput.x, transitionSpeed * Time.deltaTime);
            aimingY = Mathf.Lerp(aimingY, Controller.MoveInput.y, transitionSpeed * Time.deltaTime);
            Controller.SetBlendStateHash(aimingHashX, aimingX);
            Controller.SetBlendStateHash(aimingHashY, aimingY);
            
           // Controller.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        }

        public override void Exit()
        {
            base.Exit();
            Controller.ExitAim();
        }
    }
}