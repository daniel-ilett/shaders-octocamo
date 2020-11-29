using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamoController : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private Material camoMaterial;
    [SerializeField] private Text camoIndexText;
    [SerializeField] private Image camoIndexImage;

    private float camoIndex = 0.0f;
    private Texture2D currentCamo;
    private float camoIndexUpdateTime = 0.0f;
    private Coroutine setCamoRoutine = null;

    private const float camoSwitchTime = 0.75f;
    private const float camoThreshold = 0.5f;
    private const float camoIndexUpdateThreshold = 1.0f;

    private PlayerController playerController;

    private void Start()
    {
        CamoDatabase.Instance.GetCamoColour((Texture2D)camoMaterial.mainTexture);
        //terrain = Terrain.activeTerrain;
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && setCamoRoutine == null)
        {
            setCamoRoutine = StartCoroutine(SetCamo());
            UpdateCamoIndex();
        }
        else if(camoIndexUpdateTime > camoIndexUpdateThreshold && playerController.IsMoving())
        {
            UpdateCamoIndex();
        }

        camoIndexUpdateTime += Time.deltaTime;
    }

    private IEnumerator SetCamo()
    {
        var camoTexture = GetTerrainTexture();
        if(camoTexture == camoMaterial.GetTexture("_CamoTexture"))
        {
            yield break;
        }

        for (float t = 0; t < camoSwitchTime; t += Time.deltaTime)
        {
            camoMaterial.SetFloat("_CamoAmount", 1.0f - (t / camoSwitchTime));
            yield return null;
        }
        camoMaterial.SetFloat("_CamoAmount", 0.0f);

        //camoMaterial.mainTexture = camoTexture;
        camoMaterial.SetTexture("_CamoTexture", camoTexture);
        CamoDatabase.Instance.GetCamoColour(camoTexture);

        for (float t = 0; t < camoSwitchTime; t += Time.deltaTime)
        {
            camoMaterial.SetFloat("_CamoAmount", (t / camoSwitchTime));
            yield return null;
        }
        camoMaterial.SetFloat("_CamoAmount", 1.0f);

        setCamoRoutine = null;
    }

    private Texture2D GetTerrainTexture()
    {
        TerrainData tData = terrain.terrainData;

        Vector3 position;
        position.x = ((transform.position.x - terrain.transform.position.x) / tData.size.x) * tData.alphamapWidth;
        position.z = ((transform.position.z - terrain.transform.position.z) / tData.size.z) * tData.alphamapHeight;

        int textureID = 0;

        float[,,] alphamaps = tData.GetAlphamaps((int)position.x, (int)position.z, 1, 1);

        for (int i = 0; i < alphamaps.GetLength(2); ++i)
        {
            if (camoThreshold < alphamaps[0, 0, i])
            {
                textureID = i;
            }
        }

        return tData.terrainLayers[textureID].diffuseTexture;
    }

    private void UpdateCamoIndex()
    {
        var terrainTexture = GetTerrainTexture();

        var terrainColor = CamoDatabase.Instance.GetCamoColour(terrainTexture);
        var camoColor = CamoDatabase.Instance.GetCamoColour((Texture2D)camoMaterial.mainTexture);

        camoIndex = 1.0f - Mathf.Sqrt(Mathf.Pow(terrainColor.r - camoColor.r, 2) + 
            Mathf.Pow(terrainColor.g - camoColor.g, 2) + Mathf.Pow(terrainColor.b - camoColor.b, 2));

        camoIndexText.text = Mathf.RoundToInt(camoIndex * 100.0f) + "%";
        camoIndexImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - camoIndex);

        camoIndexUpdateTime = 0.0f;
    }
}
