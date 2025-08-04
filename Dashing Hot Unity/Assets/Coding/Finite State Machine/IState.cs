public interface IState
{
    bool Enter();
    void Update();
    void FixedUpdate();
    bool Exit();
}
