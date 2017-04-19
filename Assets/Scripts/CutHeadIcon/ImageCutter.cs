using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageCutter{

    static public Texture2D CutTexture(Texture2D originTexture, Vector2 startPos, int width, int height)
    {

        float diffX = (startPos.x + width) - originTexture.width;
        float diffY = (startPos.y + height - originTexture.height);

        if (diffX > 0)
        {
            startPos.x -= diffX;
            startPos.x = Mathf.Clamp(startPos.x, 0, startPos.x);
        }

        if (diffY > 0)
        {
            startPos.y -= diffY;
            startPos.y = Mathf.Clamp(startPos.y, 0, startPos.y);
        }
        
        //if (startPos.x + width > originTexture.width)
        //{
        //    width = originTexture.width - (int)startPos.x;
        //}

        //if (startPos.y + height > originTexture.height)
        //{
        //    height = originTexture.width - (int)startPos.y;
        //}
        Texture2D newTexture = new Texture2D(width,height);

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Color color = originTexture.GetPixel((int)startPos.x + x, (int)startPos.y + y);
                newTexture.SetPixel(x, y, color);
            }
        }
        newTexture.Apply();

        Texture2D finalTexture = ScaleTexture(newTexture, 180, 180);
        GameObject.Destroy(newTexture);
        return finalTexture;

        //return newTexture;
    }


    static private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }

	
}
