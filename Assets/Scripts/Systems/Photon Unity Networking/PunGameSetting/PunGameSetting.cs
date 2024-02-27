using GDD.Util;

namespace GDD.PUN
{
    public class PunGameSetting
    {
        //Keys
        public const string GAMESTATE = "GameState";
        public const string PRE_RANDOMTARGETPOSITION = "PreRandomTargetPosition";
        public const string RANDOMPOSITIONTARGETCOUNT = "RandomPositionTargetCount";
        public const string PLAYERREADYNEXTLEVEL = "PlayerReadyNextLevel";
        public const string ROOMNAME = "RoomName";
        public const string NUMBERCHAPTER = "NumberChapter";

        //Values
        public static float2D[] Pre_RandomTargetPosition;
        public static int RandomPositionTargetCount;
        public static string roomName = "";
    }
}