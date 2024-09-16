using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenBorderLimit
{
    public static float Right()
    {
        Vector2 screenRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f));

        float x = screenRight.x;

        return x;
    }

    public static float Left()
    {
        Vector2 screenLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0.5f));

        float x = screenLeft.x;

        return x;
    }

    public static float Top()
    {
        Vector2 screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));

        float y = screenTop.y;

        return y;
    }

    public static float Bottom()
    {
        Vector2 screenBottom = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0));

        float y = screenBottom.y;

        return y;
    }
}
