using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoDatabase
{
    private Dictionary<Texture2D, Color> camoColours =
        new Dictionary<Texture2D, Color>();

    private static CamoDatabase instance = null;
    public static CamoDatabase Instance {
        get
        {
            if(instance == null)
            {
                return new CamoDatabase();
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public Color GetCamoColour(Texture2D texture)
    {
        if(camoColours.ContainsKey(texture))
        {
            return camoColours[texture];
        }
        else
        {
            return AddCamoColour(texture);
        }
    }

    public Color AddCamoColour(Texture2D texture)
    {
        var pixels = texture.GetPixels();
        Vector3 color = new Vector3();

        for(int i = 0; i < pixels.Length; ++i)
        {
            color.x += pixels[i].r;
            color.y += pixels[i].g;
            color.z += pixels[i].b;
        }

        color /= pixels.Length;

        var returnColor = new Color(color.x, color.y, color.z);
        camoColours.Add(texture, returnColor);
        return returnColor;
    }
}
