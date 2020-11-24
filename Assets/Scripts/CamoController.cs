using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoController : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private Material camoMaterial;

    private float threshold = 0.5f;

    private void Start()
    {
        //terrain = Terrain.activeTerrain;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SetCamo();
        }
    }

    private void SetCamo()
    {
        TerrainData tData = terrain.terrainData;

        Vector3 position;
        position.x = ((transform.position.x - terrain.transform.position.x) / tData.size.x) * tData.alphamapWidth;
        position.z = ((transform.position.z - terrain.transform.position.z) / tData.size.z) * tData.alphamapHeight;

        int textureID = 0;

        float[,,] alphamaps = tData.GetAlphamaps((int)position.x, (int)position.z, 1, 1);

        for (int i = 0; i < alphamaps.GetLength(2); ++i)
        {
            if(threshold < alphamaps[0, 0, i])
            {
                textureID = i;
            }
        }

        Texture2D camoTexture = tData.terrainLayers[textureID].diffuseTexture;
        camoMaterial.mainTexture = camoTexture;

        CamoDatabase.Instance.GetCamoColour(camoTexture);
    }
}
