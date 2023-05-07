using UnityEngine;

namespace ProceduralWorlds.HDRPTOD
{
    [System.Serializable]
    public class WeatherShaderData
    {
        public RainShaderData m_rainShaderData = new RainShaderData();
        public HailShaderData m_hailShaderData = new HailShaderData();
        public SnowShaderData m_snowShaderData = new SnowShaderData();
        public SandShaderData m_sandShaderData = new SandShaderData();
        public WindShaderData m_windShaderData = new WindShaderData();
        public MiscShaderData m_miscShaderData = new MiscShaderData();
    }
    [System.Serializable]
    public class RainShaderData
    {
        public bool m_applyShaderSettings = true;
        [Range(0f, 1f)]
        public float m_rainPower = 0f;
        [Range(0f, 1f)]
        public float m_rainPowerOnTerrain = 0.7f;
        public float m_rainMinHeight = 0f;
        public float m_rainMaxHeight = 3000f;
        public float m_rainSpeed = 1f;
        public float m_rainDarkness = 0.7f;
        public float m_rainSmoothness = 0.8f;
        public float m_rainScale = 0.3f;
        public Texture2D m_rainTexture;

        /// <summary>
        /// Used to copy all the settings over into a new data block
        /// Helpful when you don't want to change the scriptable object data but want to edit values
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RainShaderData Copy(RainShaderData data)
        {
            if (data == null)
            {
                return new RainShaderData();
            }
            return new RainShaderData
            {
                m_applyShaderSettings = data.m_applyShaderSettings,
                m_rainDarkness = data.m_rainDarkness,
                m_rainMaxHeight = data.m_rainMaxHeight,
                m_rainMinHeight = data.m_rainMinHeight,
                m_rainPower = data.m_rainPower,
                m_rainSpeed = data.m_rainSpeed,
                m_rainScale = data.m_rainScale,
                m_rainTexture = data.m_rainTexture,
                m_rainPowerOnTerrain = data.m_rainPowerOnTerrain,
                m_rainSmoothness = data.m_rainSmoothness
            };
        }
        /// <summary>
        /// Gets the packed data
        /// Rain Power/
        /// Rain Power On Terrain/
        /// Rain Min Height/
        /// Rain Max Height
        /// </summary>
        /// <returns></returns>
        public Vector4 GetRainDataA()
        {
            return new Vector4(m_rainPower, m_rainPowerOnTerrain, m_rainMinHeight, m_rainMaxHeight);
        }
        /// <summary>
        /// Sets the rain data a
        /// </summary>
        /// <param name="dataA"></param>
        public void SetRainDataA(Vector4 dataA)
        {
            m_rainPower = dataA.x;
            m_rainPowerOnTerrain = dataA.y;
            m_rainMinHeight = dataA.z;
            m_rainMaxHeight = dataA.w;
        }
        /// <summary>
        /// Gets the packed data
        /// Rain Speed/
        /// Rain Darkness/
        /// Rain Smoothness/
        /// Rain Scale
        /// </summary>
        /// <returns></returns>
        public Vector4 GetRainDataB()
        {
            return new Vector4(m_rainSpeed, m_rainDarkness, m_rainSmoothness, m_rainScale);
        }
        /// <summary>
        /// Sets the rain data a
        /// </summary>
        /// <param name="dataA"></param>
        public void SetRainDataB(Vector4 dataA)
        {
            m_rainSpeed = dataA.x;
            m_rainDarkness = dataA.y;
            m_rainSmoothness = dataA.z;
            m_rainScale = dataA.w;
        }
        /// <summary>
        /// Gets the rain texture map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetRainTexture()
        {
            return m_rainTexture;
        }
        /// <summary>
        /// Sets the rain texture
        /// </summary>
        /// <param name="rainMap"></param>
        public void SetRainTexture(Texture2D rainMap)
        {
            m_rainTexture = rainMap;
        }
    }
    [System.Serializable]
    public class HailShaderData
    {
        //TODO : Add hail stuff when it's done
    }
    [System.Serializable]
    public class SnowShaderData
    {
        public bool m_applyShaderSettings = true;
        [Range(0f, 1f)]
        public float m_snowPower = 0f;
        [Range(0f, 1f)]
        public float m_snowPowerOnTerrain = 0.7f;
        public float m_snowMinHeight = 0f;
        public float m_snowAge = 0f;
        public float m_snowContrast = 2f;
        public float m_snowWorldScale = 0.1f;
        public Texture2D m_snowAlbedo;
        public Texture2D m_snowNormal;
        public Texture2D m_snowMask;

        /// <summary>
        /// Used to copy all the settings over into a new data block
        /// Helpful when you don't want to change the scriptable object data but want to edit values
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SnowShaderData Copy(SnowShaderData data)
        {
            if (data == null)
            {
                return new SnowShaderData();
            }
            return new SnowShaderData
            {
                m_applyShaderSettings = data.m_applyShaderSettings, 
                m_snowAge = data.m_snowAge, 
                m_snowAlbedo = data.m_snowAlbedo, 
                m_snowContrast = data.m_snowContrast, 
                m_snowMask = data.m_snowMask, 
                m_snowMinHeight = data.m_snowMinHeight, 
                m_snowNormal = data.m_snowNormal, 
                m_snowPower = data.m_snowPower, 
                m_snowWorldScale = data.m_snowWorldScale, 
                m_snowPowerOnTerrain = data.m_snowPowerOnTerrain
            };
        }
        /// <summary>
        /// Gets the packed data
        /// Snow Power/
        /// Snow Power On Terrain/
        /// Snow Min Height/
        /// Snow Age
        /// </summary>
        /// <returns></returns>
        public Vector4 GetSnowDataA()
        {
            return new Vector4(m_snowPower, m_snowPowerOnTerrain, m_snowMinHeight, m_snowAge);
        }
        /// <summary>
        /// Set the snow data a
        /// </summary>
        /// <param name="data"></param>
        public void SetSnowDataA(Vector4 data)
        {
            m_snowPower = data.x;
            m_snowPower = data.y;
            m_snowPower = data.z;
            m_snowPower = data.w;
        }
        /// <summary>
        /// Gets the packed data
        /// Snow World Scale/
        /// Snow Contrast
        /// </summary>
        /// <returns></returns>
        public Vector4 GetSnowDataB()
        {
            //TODO : Add z + w values when we have more to add
            return new Vector4(m_snowWorldScale, m_snowContrast, 0f, 0f);
        }
        /// <summary>
        /// Set the snow data a
        /// </summary>
        /// <param name="data"></param>
        public void SetSnowDataB(Vector4 data)
        {
            m_snowWorldScale = data.x;
            m_snowContrast = data.y;
            m_snowPower = 0f;
            m_snowPower = 0f;
        }
        /// <summary>
        /// Get the snow albedo map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetSnowAlbedo()
        {
            return m_snowAlbedo;
        }
        /// <summary>
        /// Get the snow normal map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetSnowNormal()
        {
            return m_snowNormal;
        }
        /// <summary>
        /// Get the snow mask map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetSnowMask()
        {
            return m_snowMask;
        }
        /// <summary>
        /// Sets all the snow textures
        /// </summary>
        /// <param name="albedo"></param>
        /// <param name="normal"></param>
        /// <param name="mask"></param>
        public void SetSnowTextures(Texture2D albedo, Texture2D normal, Texture2D mask)
        {
            m_snowAlbedo = albedo;
            m_snowNormal = normal;
            m_snowMask = mask;
        }
    }
    [System.Serializable]
    public class SandShaderData
    {
        public bool m_applyShaderSettings = true;
        [Range(0f, 1f)]
        public float m_sandPower = 0f;
        public float m_sandContrast = 2f;
        public float m_sandWorldScale = 0.1f;
        public float m_sandMaxHeight = 3000f;
        public Texture2D m_sandAlbedo;
        public Texture2D m_sandNormal;
        public Texture2D m_sandMask;

        /// <summary>
        /// Used to copy all the settings over into a new data block
        /// Helpful when you don't want to change the scriptable object data but want to edit values
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SandShaderData Copy(SandShaderData data)
        {
            if (data == null)
            {
                return new SandShaderData();
            }
            return new SandShaderData
            {
                m_applyShaderSettings = data.m_applyShaderSettings,
                m_sandAlbedo = data.m_sandAlbedo,
                m_sandContrast = data.m_sandContrast,
                m_sandMask = data.m_sandMask,
                m_sandMaxHeight = data.m_sandMaxHeight,
                m_sandNormal = data.m_sandNormal, m_sandPower = data.m_sandPower,
                m_sandWorldScale = data.m_sandWorldScale
            };
        }
        /// <summary>
        /// Gets the packed data
        /// Sand Power/
        /// Sand Max Height/
        /// Sand Contrast/
        /// Sand World Scale
        /// </summary>
        /// <returns></returns>
        public Vector4 GetSandDataA()
        {
            return new Vector4(m_sandPower, m_sandMaxHeight, m_sandContrast, m_sandWorldScale);
        }
        /// <summary>
        /// Sets the sand data a
        /// </summary>
        /// <param name="data"></param>
        public void SetSandDataA(Vector4 data)
        {
            m_sandPower = data.x;
            m_sandMaxHeight = data.y;
            m_sandContrast = data.z;
            m_sandWorldScale = data.w;
        }
        /// <summary>
        /// Gets the sand albedo map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetSandAlbedo()
        {
            return m_sandAlbedo;
        }
        /// <summary>
        /// Gets the sand normal map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetSandNormal()
        {
            return m_sandNormal;
        }
        /// <summary>
        /// Gets the sand mask map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetSandMask()
        {
            return m_sandMask;
        }
        /// <summary>
        /// Sets the sand textures
        /// </summary>
        /// <param name="albedo"></param>
        /// <param name="normal"></param>
        /// <param name="mask"></param>
        public void SetSandTextures(Texture2D albedo, Texture2D normal, Texture2D mask)
        {
            m_sandAlbedo = albedo;
            m_sandNormal = normal;
            m_sandMask = mask;
        }
    }
    [System.Serializable]
    public class WindShaderData
    {
        //TODO : Add wind when it's ready
    }
    [System.Serializable]
    public class MiscShaderData
    {
        public bool m_applyShaderSettings = true;
        public float m_seaLevel;
        public float m_temperature;
        public float m_humidity;
        public Texture2D m_weatherMask;

        /// <summary>
        /// Gets the packed data
        /// Sea Level/
        /// Temperature/
        /// Humidity
        /// </summary>
        /// <returns></returns>
        public Vector4 GetMiscDataA()
        {
            //TODO : Add w value when we have one to use
            return new Vector4(m_seaLevel, m_temperature, m_humidity, 0f);
        }
        /// <summary>
        /// Sets the misc data a
        /// </summary>
        /// <param name="data"></param>
        public void SetMiscDataA(Vector4 data)
        {
            m_seaLevel = data.x;
            m_temperature = data.y;
            m_humidity = data.z;
        }
        /// <summary>
        /// Gets the weather mask map
        /// </summary>
        /// <returns></returns>
        public Texture2D GetWeatherMask()
        {
            return m_weatherMask;
        }
        /// <summary>
        /// Sets the weather mask
        /// </summary>
        /// <param name="weatherMask"></param>
        public void SetWeatherMask(Texture2D weatherMask)
        {
            m_weatherMask = weatherMask;
        }
    }

    /// <summary>
    /// Shader ID class for the shader properties for the weather system
    /// </summary>
    public static class WeatherShaderID
    {
        //Rain
        public static readonly int m_TOD_RainDataA;
        public static readonly int m_TOD_RainDataB;
        public static readonly int m_TOD_RainMap;
        //Snow
        public static readonly int m_TOD_SnowDataA;
        public static readonly int m_TOD_SnowDataB;
        public static readonly int m_TOD_SnowAlbedo;
        public static readonly int m_TOD_SnowNormal;
        public static readonly int m_TOD_SnowMask;
        //Sand
        public static readonly int m_TOD_SandDataA;
        public static readonly int m_TOD_SandAlbedo;
        public static readonly int m_TOD_SandNormal;
        public static readonly int m_TOD_SandMask;
        //Misc
        public static readonly int m_TOD_WeatherDataA;
        public static readonly int m_TOD_WeatherMask;
        //Legacy
        public static readonly int m_TOD_Legacy_Snow_Power;

        static WeatherShaderID()
        {
            //Rain
            m_TOD_RainDataA = Shader.PropertyToID("_TOD_RainDataA");
            m_TOD_RainDataB = Shader.PropertyToID("_TOD_RainDataB");
            m_TOD_RainMap = Shader.PropertyToID("_TOD_RainMap");
            //Snow
            m_TOD_SnowDataA = Shader.PropertyToID("_TOD_SnowDataA");
            m_TOD_SnowDataB = Shader.PropertyToID("_TOD_SnowDataB");
            m_TOD_SnowAlbedo = Shader.PropertyToID("_TOD_SnowAlbedo");
            m_TOD_SnowNormal = Shader.PropertyToID("_TOD_SnowNormal");
            m_TOD_SnowMask = Shader.PropertyToID("_TOD_SnowMask");
            //Sand
            m_TOD_SandDataA = Shader.PropertyToID("_TOD_SandDataA");
            m_TOD_SandAlbedo = Shader.PropertyToID("_TOD_SandAlbedo");
            m_TOD_SandNormal = Shader.PropertyToID("_TOD_SandNormal");
            m_TOD_SandMask = Shader.PropertyToID("_TOD_SandMask");
            //Misc
            m_TOD_WeatherDataA = Shader.PropertyToID("_TOD_WeatherDataA");
            m_TOD_WeatherMask = Shader.PropertyToID("_TOD_WeatherMask");
            //Legacy
            m_TOD_Legacy_Snow_Power = Shader.PropertyToID("_PW_Global_CoverLayer1Progress");
        }
    }

    [ExecuteAlways]
    public class WeatherShaderManager : MonoBehaviour
    {
        public static WeatherShaderManager Instance
        {
            get { return m_instance; }
        }
        [SerializeField] private static WeatherShaderManager m_instance;

        public WeatherShaderData m_shaderData = new WeatherShaderData();

        private void OnEnable()
        {
            m_instance = this;
        }

        /// <summary>
        /// Applies all the global weather shader values based on the data provided
        /// </summary>
        /// <param name="rainShaderData"></param>
        /// <param name="snowShaderData"></param>
        /// <param name="sandShaderData"></param>
        /// <param name="miscShaderData"></param>
        public static void ApplyAllShaderValues(WeatherShaderData shaderData)
        {
            if (shaderData == null)
            {
                return;
            }

            ApplyRainShaderValues(shaderData.m_rainShaderData);
            ApplySnowShaderValues(shaderData.m_snowShaderData);
            ApplySandShaderValues(shaderData.m_sandShaderData);
            ApplyMiscShaderValues(shaderData.m_miscShaderData);
        }
        /// <summary>
        /// Gets all the shader values
        /// </summary>
        /// <returns></returns>
        public static WeatherShaderData GetAllShaderValues()
        {
            WeatherShaderData shaderData = new WeatherShaderData
            {
                m_rainShaderData = GetRainShaderValues(),
                m_snowShaderData = GetSnowShaderValues(),
                m_sandShaderData = GetSandShaderValues(),
                m_miscShaderData = GetMiscShaderValues()
            };
            return shaderData;
        }
        /// <summary>
        /// Gets the rain shader values
        /// </summary>
        /// <returns></returns>
        public static RainShaderData GetRainShaderValues()
        {
            RainShaderData rainShaderData = new RainShaderData();
            rainShaderData.SetRainDataA(Shader.GetGlobalVector(WeatherShaderID.m_TOD_RainDataA));
            rainShaderData.SetRainDataB(Shader.GetGlobalVector(WeatherShaderID.m_TOD_RainDataB));
            rainShaderData.SetRainTexture((Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_RainMap));
            return rainShaderData;
        }
        /// <summary>
        /// Gets the snow shader values
        /// </summary>
        /// <returns></returns>
        public static SnowShaderData GetSnowShaderValues()
        {
            SnowShaderData snowShaderData = new SnowShaderData();
            snowShaderData.SetSnowDataA(Shader.GetGlobalVector(WeatherShaderID.m_TOD_SnowDataA));
            snowShaderData.SetSnowDataB(Shader.GetGlobalVector(WeatherShaderID.m_TOD_SnowDataB));
            snowShaderData.SetSnowTextures((Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_SnowAlbedo), (Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_SnowNormal), (Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_SnowMask));
            return snowShaderData;
        }
        /// <summary>
        /// Gets the sand shader values
        /// </summary>
        /// <returns></returns>
        public static SandShaderData GetSandShaderValues()
        {
            SandShaderData sandShaderData = new SandShaderData();
            sandShaderData.SetSandDataA(Shader.GetGlobalVector(WeatherShaderID.m_TOD_SandDataA));
            sandShaderData.SetSandTextures((Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_SnowAlbedo), (Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_SandNormal), (Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_SandMask));
            return sandShaderData;
        }
        /// <summary>
        /// Gets the misc shader values
        /// </summary>
        /// <returns></returns>
        public static MiscShaderData GetMiscShaderValues()
        {
            MiscShaderData miscShaderData = new MiscShaderData();
            miscShaderData.SetMiscDataA(Shader.GetGlobalVector(WeatherShaderID.m_TOD_WeatherDataA));
            miscShaderData.SetWeatherMask((Texture2D)Shader.GetGlobalTexture(WeatherShaderID.m_TOD_WeatherMask));
            return miscShaderData;
        }
        /// <summary>
        /// Resets all the weather power values
        /// This is good if you want to reset and remove all the snow, rain, sand from your scene
        /// </summary>
        /// <param name="shaderData"></param>
        public static void ResetAllWeatherPowerValues(WeatherShaderData shaderData)
        {
            if (shaderData == null)
            {
                return;
            }

            ResetRainPower(false, RainShaderData.Copy(shaderData.m_rainShaderData));
            ResetSnowPower(false, SnowShaderData.Copy(shaderData.m_snowShaderData));
            ResetSandPower(false, SandShaderData.Copy(shaderData.m_sandShaderData));
        }
        /// <summary>
        /// Resets the rain power true = on/ false = off
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rainShaderData"></param>
        public static void ResetRainPower(bool value, RainShaderData rainShaderData)
        {
            if (rainShaderData == null)
            {
                return;
            }

            if (value)
            {
                rainShaderData.m_rainPower = 1f;
            }
            else
            {
                rainShaderData.m_rainPower = 0f;
            }

            rainShaderData.m_applyShaderSettings = true;
            ApplyRainShaderValues(rainShaderData);
        }
        /// <summary>
        /// Resets the rain power true = on/ false = off
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rainShaderData"></param>
        public static void ResetSnowPower(bool value, SnowShaderData snowShaderData)
        {
            if (snowShaderData == null)
            {
                return;
            }

            if (value)
            {
                snowShaderData.m_snowPower = 1f;
            }
            else
            {
                snowShaderData.m_snowPower = 0f;
            }

            snowShaderData.m_applyShaderSettings = true;
            ApplySnowShaderValues(snowShaderData);
        }
        /// <summary>
        /// Resets the rain power true = on/ false = off
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rainShaderData"></param>
        public static void ResetSandPower(bool value, SandShaderData sandShaderData)
        {
            if (sandShaderData == null)
            {
                return;
            }

            if (value)
            {
                sandShaderData.m_sandPower = 1f;
            }
            else
            {
                sandShaderData.m_sandPower = 0f;
            }

            sandShaderData.m_applyShaderSettings = true;
            ApplySandShaderValues(sandShaderData);
        }
        /// <summary>
        /// Applies the rain global shader values based on the data provided
        /// </summary>
        /// <param name="rainShaderData"></param>
        public static void ApplyRainShaderValues(RainShaderData rainShaderData)
        {
            if (rainShaderData != null && rainShaderData.m_applyShaderSettings)
            {
                Shader.SetGlobalVector(WeatherShaderID.m_TOD_RainDataA, rainShaderData.GetRainDataA());
                Shader.SetGlobalVector(WeatherShaderID.m_TOD_RainDataB, rainShaderData.GetRainDataB());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_RainMap, rainShaderData.GetRainTexture());
            }
        }
        /// <summary>
        /// Applies the snow global shader values based on the data provided
        /// </summary>
        /// <param name="snowShaderData"></param>
        public static void ApplySnowShaderValues(SnowShaderData snowShaderData)
        {
            if (snowShaderData != null && snowShaderData.m_applyShaderSettings)
            {
                Shader.SetGlobalVector(WeatherShaderID.m_TOD_SnowDataA, snowShaderData.GetSnowDataA());
                Shader.SetGlobalVector(WeatherShaderID.m_TOD_SnowDataB, snowShaderData.GetSnowDataB());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_SnowAlbedo, snowShaderData.GetSnowAlbedo());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_SnowNormal, snowShaderData.GetSnowNormal());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_SnowMask, snowShaderData.GetSnowMask());
                Shader.SetGlobalFloat(WeatherShaderID.m_TOD_Legacy_Snow_Power, snowShaderData.GetSnowDataA().x);
            }
        }
        //Applies the sand global shader values based on the data provided
        public static void ApplySandShaderValues(SandShaderData sandShaderData)
        {
            if (sandShaderData != null && sandShaderData.m_applyShaderSettings)
            {
                Shader.SetGlobalVector(WeatherShaderID.m_TOD_SandDataA, sandShaderData.GetSandDataA());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_SandAlbedo, sandShaderData.GetSandAlbedo());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_SandNormal, sandShaderData.GetSandNormal());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_SandMask, sandShaderData.GetSandMask());
            }
        }
        /// <summary>
        /// Applies the misc global shader values based on the data provided
        /// </summary>
        /// <param name="miscShaderData"></param>
        public static void ApplyMiscShaderValues(MiscShaderData miscShaderData)
        {
            if (miscShaderData != null && miscShaderData.m_applyShaderSettings)
            {
                Shader.SetGlobalVector(WeatherShaderID.m_TOD_WeatherDataA, miscShaderData.GetMiscDataA());
                Shader.SetGlobalTexture(WeatherShaderID.m_TOD_WeatherMask, miscShaderData.GetWeatherMask());
            }
        }
    }
}