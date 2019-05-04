
namespace GameLogic
{
    class GameLogicData
    {
#if SERVER
        public const uint GAME_TICKER_GROUP_LIST_CAPCITY = 20;
#else
        public const uint GAME_TICKER_GROUP_LIST_CAPCITY = 10;
#endif

    }
}
