namespace SceneManagement
{
    public class SceneManagement
    {
        private ISceneState currentState;

        public void ChangeState(ISceneState newState)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }

            currentState = newState;
            currentState.OnEnter();
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.UpdateState();
            }
        }
    }
}