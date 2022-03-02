namespace Config
{
    public class HardcodeGameConfigManager: IConfigManager
    {
        public GameConfig GetConfig()
        {
            return new GameConfig() { GameSpeed = 1.0f};
        }
    }
}