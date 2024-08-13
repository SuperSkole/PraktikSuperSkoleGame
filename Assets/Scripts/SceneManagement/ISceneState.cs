namespace SceneManagement
{
    public interface ISceneState
    {
        void OnEnter();
        void OnExit();
        void UpdateState();
    }
}