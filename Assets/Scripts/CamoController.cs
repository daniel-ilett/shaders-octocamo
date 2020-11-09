using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoController : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private Material camoMaterial;

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
        float alpha = 0.0f;

        float[,,] alphamaps = tData.GetAlphamaps(0, 0, tData.alphamapWidth, tData.alphamapHeight);

        for (int i = 0; i < tData.alphamapTextureCount; ++i)
        {
            if(alpha <= alphamaps[(int)position.x, (int)position.z, i])
            {
                textureID = i;
                //alpha = alphamaps[(int)position.x, (int)position.z, i];
            }
        }

        Texture2D camoTexture = tData.terrainLayers[textureID].diffuseTexture;
        camoMaterial.mainTexture = camoTexture;
    }
}
