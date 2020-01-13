namespace SpaceInvaders.Game_Managers
{
    public interface IScoreManager
    {
        string HighScore();
        void SaveHighScore();
        void UpdateScore();
        void ResetScore();
        string CurrentScore();
    }
}