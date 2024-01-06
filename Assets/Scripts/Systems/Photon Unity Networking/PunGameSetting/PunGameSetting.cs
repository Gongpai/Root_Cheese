using GDD.Util;

namespace GDD.PUN
{
    public class PunGameSetting
    {
        //Keys
        public const string GAMESTATE = "GameState";
        public const string PRE_RANDOMTARGETPOSITION = "PreRandomTargetPosition";
        public const string RANDOMPOSITIONTARGETCOUNT = "RandomPositionTargetCount";

        //Values
        public static float2D[] Pre_RandomTargetPosition;
        public static int RandomPositionTargetCount;
    }
}