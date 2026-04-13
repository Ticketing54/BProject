using UnityEngine;

public static class DataBundle
{
    public static float ENDBOX_POSITION = -12;
    #region Color

    public enum BallColor
    {
        ORANGE,
        BLUE
    }

    public static readonly Color COLOR_BLUE = new Color(0f, 0f, 1f, 1f);
    public static readonly Color COLOR_ORANGE = new Color(1f, 0.48f, 0f, 1f);
    public static readonly Color COLOR_ALPHA_BLUE = new Color(0f, 0f, 1f, 0.49f);
    public static readonly Color COLOR_ALPHA_ORANGE = new Color(1f, 0.48f, 0f, 0.49f);

    public static Color GetColor(BallColor type, bool isAlpha = false)
    {
        if (isAlpha)
        {
            return type == BallColor.BLUE ? COLOR_ALPHA_BLUE : COLOR_ALPHA_ORANGE;
        }
        else
        {
            return type == BallColor.BLUE ? COLOR_BLUE : COLOR_ORANGE;
        }
    }

    #endregion

    #region Duplicate

    public static readonly WaitForSeconds DELAY_REPLICATETIME = new WaitForSeconds(0.1f);

    public const float DUPLICATE_SPRAY_RANGE = 0.1f;
    public const float DUPLICATE_MAX_LENGHT = 9f;
    public static Vector3 DUPLICATE_CANVAS_SCALE = new Vector3(0.003f, 0.003f, 0.003f);

    #endregion

    #region Obstacle
    public enum ObstacleType
    {
        NONE = 0,
        SLANTEDWALL,
        SINGLE_DUPLICATION_BOX,
        DOUBLE_DUPLICATION_BOX,
        WALL,
        SLIDE_BOX

    }
    #endregion
}
