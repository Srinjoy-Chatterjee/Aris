#if HDPipeline && UNITY_2021_2_OR_NEWER
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ProceduralWorlds.HDRPTOD
{
    public enum GlobalCloudType { None, Volumetric, Procedural, Both }
    public enum CloudLayerType { Single, Double }
    public enum CloudResolution { Resolution256, Resolution512, Resolution1024, Resolution2048, Resolution4096, Resolution8192 }
    public enum TimeOfDaySkyMode { PhysicallyBased, Gradient }
    public enum GeneralQuality { Low, Medium, High }
    public enum GeneralRenderMode { Performance, Quality }
    public enum UnderwaterOverrideSystemType { Custom, Gaia }
    public enum CloudLayerChannelMode { R,G,B,A }
    public enum SSGIRenderMode { Interior, Exterior, Both }

    [System.Serializable]
    public class TimeOfDayLensFlareProfile
    {
        public bool m_useLensFlare = true;
        public LensFlareDataSRP m_lensFlareData;
        public AnimationCurve m_intensity;
        public AnimationCurve m_scale;
        public bool m_enableOcclusion = true;
        public float m_occlusionRadius = 0.1f;
        public int m_sampleCount = 32;
        public float m_occlusionOffset = 0.05f;
        public bool m_allowOffScreen = false;

        /// <summary>
        /// Copies all the settings into another lens flare profile
        /// </summary>
        /// <param name="profile"></param>
        public void CopySettings(ref TimeOfDayLensFlareProfile profile)
        {
            if (profile == null)
            {
                profile = new TimeOfDayLensFlareProfile();
            }

            if (profile != null)
            {
                profile.m_useLensFlare = m_useLensFlare;
                profile.m_lensFlareData = m_lensFlareData;
                profile.m_intensity = TimeOfDayProfileData.CopyCurve(m_intensity);
                profile.m_scale = TimeOfDayProfileData.CopyCurve(m_scale);
                profile.m_enableOcclusion = m_enableOcclusion;
                profile.m_occlusionRadius = m_occlusionRadius;
                profile.m_sampleCount = m_sampleCount;
                profile.m_occlusionOffset = m_occlusionOffset;
                profile.m_allowOffScreen = m_allowOffScreen;
            }
        }
    }
    [System.Serializable]
    public class TimeOfDayDebugSettings
    {
        public bool m_roundUp = true;
        public float m_simulationSpeed = 1f;
        public bool m_simulate = false;
    }
    [System.Serializable]
    public class TimeOfDayProfileData
    {
        private float m_lastTimeOfDayValue = -1f;
        private TimeOfDayProfileData m_startingData;
        private bool m_hasBeenSetup = false;

        [Header("Duration")]
        public bool m_durationSettings = false;
        public float m_dayDuration = 300f;
        public float m_nightDuration = 120f;
        [Header("Sun")]
        public bool m_sunSettings = false;
        public bool m_enableSunShadows = true;
        public bool m_enableMoonShadows = true;
        public AnimationCurve m_sunIntensity;
        public AnimationCurve m_moonIntensity;
        public AnimationCurve m_sunIntensityMultiplier;
        public AnimationCurve m_sunTemperature;
        public Gradient m_sunColorFilter;
        public AnimationCurve m_moonTemperature;
        public Gradient m_moonColorFilter;
        public AnimationCurve m_sunVolumetrics;
        public AnimationCurve m_sunVolumetricShadowDimmer;
        public float m_globalLightMultiplier = 1f;
        [Header("Sky")]
        public bool m_skySettings = false;
        public float m_horizonOffset = 0.1f;
        public TimeOfDaySkyMode m_skyMode = TimeOfDaySkyMode.Gradient;
        public float m_skyboxExposure = 2.2f;
        public Color m_skyboxGroundColor = new Color(0.9609969f, 0.9024923f, 0.8708023f, 1f);
        public float m_starsDayIntensity = 300f;
        public float m_starsNightIntensity = 4500f;
        //Gradient
        public Gradient m_skyTopColor;
        public Gradient m_skyMiddleColor;
        public Gradient m_skyBottomColor;
        public AnimationCurve m_skyGradientDiffusion;
        public AnimationCurve m_skyExposureGradient;
        [Header("Shadows")]
        public AnimationCurve m_shadowDistance;
        public AnimationCurve m_shadowTransmissionMultiplier;
        public int m_shadowCascadeCount = 4;
        public float m_shadowDistanceMultiplier = 1f;
        [Header("Fog")]
        public bool m_fogSettings = false;
        public GeneralQuality m_fogQuality = GeneralQuality.Medium;
        public bool m_useDenoising = true;
        public GeneralQuality m_denoisingQuality = GeneralQuality.Medium;
        public Gradient m_fogColor;
        public AnimationCurve m_fogDistance;
        public AnimationCurve m_fogDensity;
        public AnimationCurve m_fogHeight;
        public AnimationCurve m_volumetricFogDistance;
        public AnimationCurve m_volumetricFogAnisotropy;
        public AnimationCurve m_volumetricFogSliceDistributionUniformity;
        public AnimationCurve m_localFogMultiplier;
        public float m_globalFogMultiplier = 1f;
        [Header("Clouds")]
        public bool m_cloudSettings = false;
        public GlobalCloudType m_globalCloudType = GlobalCloudType.Volumetric;
        //Volumetric
        public bool m_useLocalClouds = false;
        public VolumetricClouds.CloudPresets m_cloudPresets = VolumetricClouds.CloudPresets.Custom;
        public AnimationCurve m_volumetricDensityMultiplier;
        public AnimationCurve m_volumetricDensityCurve;
        public AnimationCurve m_volumetricShapeFactor;
        public AnimationCurve m_volumetricShapeScale;
        public AnimationCurve m_volumetricErosionFactor;
        public AnimationCurve m_volumetricErosionScale;
        public VolumetricClouds.CloudErosionNoise m_erosionNoiseType;
        public AnimationCurve m_volumetricErosionCurve;
        public AnimationCurve m_volumetricAmbientOcclusionCurve;
        public AnimationCurve m_volumetricLowestCloudAltitude;
        public AnimationCurve m_volumetricCloudThickness;
        public AnimationCurve m_volumetricAmbientLightProbeDimmer;
        public AnimationCurve m_volumetricSunLightDimmer;
        public AnimationCurve m_volumetricErosionOcclusion;
        public Gradient m_volumetricScatteringTint;
        public AnimationCurve m_volumetricPowderEffectIntensity;
        public AnimationCurve m_volumetricMultiScattering;
        public bool m_volumetricCloudShadows = true;
        public VolumetricClouds.CloudShadowResolution m_volumetricCloudShadowResolution = VolumetricClouds.CloudShadowResolution.Medium256;
        public AnimationCurve m_volumetricCloudShadowOpacity;
        //Procedural
        public CloudLayerType m_cloudLayers = CloudLayerType.Single;
        public CloudResolution m_cloudResolution = CloudResolution.Resolution1024;
        public bool m_useCloudShadows = false;
        public AnimationCurve m_cloudOpacity;
        public Gradient m_cloudTintColor;
        public AnimationCurve m_cloudExposure;
        public AnimationCurve m_cloudWindDirection;
        public AnimationCurve m_cloudWindSpeed;
        public AnimationCurve m_cloudShadowOpacity;
        public bool m_cloudLighting = true;
        public Gradient m_cloudShadowColor;
        public CloudLayerChannelMode m_cloudLayerAChannel = CloudLayerChannelMode.R;
        public AnimationCurve m_cloudLayerAOpacityR;
        public AnimationCurve m_cloudLayerAOpacityG;
        public AnimationCurve m_cloudLayerAOpacityB;
        public AnimationCurve m_cloudLayerAOpacityA;
        public CloudLayerChannelMode m_cloudLayerBChannel = CloudLayerChannelMode.B;
        public AnimationCurve m_cloudLayerBOpacityR;
        public AnimationCurve m_cloudLayerBOpacityG;
        public AnimationCurve m_cloudLayerBOpacityB;
        public AnimationCurve m_cloudLayerBOpacityA;
        [Header("Advanced Lighting")]
        public bool m_advancedLightingSettings = false;
        public bool m_useSSGI = false;
        public GeneralQuality m_ssgiQuality = GeneralQuality.Low;
        public SSGIRenderMode m_ssgiRenderMode = SSGIRenderMode.Exterior;
        public bool m_useSSR = false;
        public GeneralQuality m_ssrQuality = GeneralQuality.Medium;
        public bool m_useContactShadows = true;
        public AnimationCurve m_contactShadowsDistance;
        public bool m_useMicroShadows = false;
        public AnimationCurve m_generalExposure;
        public AnimationCurve m_ambientIntensity;
        public AnimationCurve m_ambientReflectionIntensity;
        public AnimationCurve m_planarReflectionIntensity;
        public TimeOfDayLensFlareProfile m_sunLensFlareProfile;
        public TimeOfDayLensFlareProfile m_moonLensFlareProfile;

        /// <summary>
        /// Applies the weather and lerps the values from current to this profile settings
        /// </summary>
        /// <param name="lightData"></param>
        /// <param name="physicallyBasedSky"></param>
        /// <param name="gradientSky"></param>
        /// <param name="exposure"></param>
        /// <param name="fog"></param>
        /// <param name="clouds"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool ReturnFromWeather(HDRPTimeOfDayComponents components, bool isDay, bool overrideLightSource, float time, float duration)
        {
            if (!m_hasBeenSetup)
            {
                Debug.LogError(" Weather was not setup with the starting values please call SetupStartingSettings() before calling Apply weather");
                return false;
            }

            if (duration >= 1f)
            {
                return true;
            }

            HDAdditionalLightData lightData = components.m_sunLightData;
            if (!isDay)
            {
                lightData = components.m_moonLightData;
            }

            //Sun
            if (ValidateSun())
            {
                if (!overrideLightSource)
                {
                    if (isDay)
                    {
                        lightData.SetIntensity(LerpFloat(m_startingData.m_sunIntensity.Evaluate(m_lastTimeOfDayValue), m_sunIntensity.Evaluate(time) * m_globalLightMultiplier, duration));
                        lightData.SetColor(LerpColor(m_startingData.m_sunColorFilter.Evaluate(m_lastTimeOfDayValue), m_sunColorFilter.Evaluate(time), duration), LerpFloat(m_startingData.m_sunTemperature.Evaluate(m_lastTimeOfDayValue), m_sunTemperature.Evaluate(time), duration));
                    }
                    else
                    {
                        lightData.SetIntensity(LerpFloat(m_startingData.m_moonIntensity.Evaluate(m_lastTimeOfDayValue), m_moonIntensity.Evaluate(time) * m_globalLightMultiplier, duration));
                        lightData.SetColor(LerpColor(m_startingData.m_moonColorFilter.Evaluate(m_lastTimeOfDayValue), m_moonColorFilter.Evaluate(time), duration), LerpFloat(m_startingData.m_moonTemperature.Evaluate(m_lastTimeOfDayValue), m_moonTemperature.Evaluate(time), duration));
                    }
                }

                lightData.volumetricDimmer = LerpFloat(m_startingData.m_sunVolumetrics.Evaluate(m_lastTimeOfDayValue), m_sunVolumetrics.Evaluate(time), duration);
                lightData.volumetricShadowDimmer = LerpFloat(m_startingData.m_sunVolumetricShadowDimmer.Evaluate(m_lastTimeOfDayValue), m_sunVolumetricShadowDimmer.Evaluate(time), duration);
                lightData.lightDimmer = LerpFloat(m_startingData.m_sunIntensityMultiplier.Evaluate(m_lastTimeOfDayValue), m_sunIntensityMultiplier.Evaluate(time), duration);
            }
            //Sky
            if (ValidateSky())
            {
                //Physically Based
                components.m_physicallyBasedSky.updateMode.value = EnvironmentUpdateMode.OnChanged;
                //Gradient
                components.m_gradientSky.top.value = LerpColor(m_startingData.m_skyTopColor.Evaluate(m_lastTimeOfDayValue), m_skyTopColor.Evaluate(time), duration);
                components.m_gradientSky.middle.value = LerpColor(m_startingData.m_skyMiddleColor.Evaluate(m_lastTimeOfDayValue), m_skyMiddleColor.Evaluate(time), duration);
                components.m_gradientSky.bottom.value = LerpColor(m_startingData.m_skyBottomColor.Evaluate(m_lastTimeOfDayValue), m_skyBottomColor.Evaluate(time), duration);
                components.m_gradientSky.gradientDiffusion.value = LerpFloat(m_startingData.m_skyGradientDiffusion.Evaluate(m_lastTimeOfDayValue), m_skyGradientDiffusion.Evaluate(time), duration);
                components.m_gradientSky.exposure.value = LerpFloat(m_startingData.m_skyExposureGradient.Evaluate(m_lastTimeOfDayValue), m_skyExposureGradient.Evaluate(time), duration);
            }
            //Advanced Lighting
            if (ValidateAdvancedLighting())
            {
                //Exposure
                components.m_exposure.fixedExposure.value = LerpFloat(m_startingData.m_generalExposure.Evaluate(m_lastTimeOfDayValue), m_generalExposure.Evaluate(time), duration);
                components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value = LerpFloat(m_startingData.m_ambientIntensity.Evaluate(m_lastTimeOfDayValue), m_ambientIntensity.Evaluate(time), duration);
                components.m_indirectLightingController.reflectionLightingMultiplier.value = LerpFloat(m_startingData.m_ambientReflectionIntensity.Evaluate(m_lastTimeOfDayValue), m_ambientReflectionIntensity.Evaluate(time), duration);
                components.m_indirectLightingController.reflectionProbeIntensityMultiplier.value = LerpFloat(m_startingData.m_planarReflectionIntensity.Evaluate(m_lastTimeOfDayValue), m_planarReflectionIntensity.Evaluate(time), duration);
            }
            //Fog
            if (ValidateFog())
            {
                //Local Fog
                components.m_localVolumetricFog.parameters.meanFreePath = LerpFloat(m_startingData.m_fogDensity.Evaluate(m_lastTimeOfDayValue), m_fogDensity.Evaluate(time) / m_globalFogMultiplier, duration);
                components.m_localVolumetricFog.parameters.albedo = LerpColor(m_startingData.m_fogColor.Evaluate(m_lastTimeOfDayValue), m_fogColor.Evaluate(time) * m_localFogMultiplier.Evaluate(time), duration);
                //Global
                components.m_fog.albedo.value = LerpColor(m_startingData.m_fogColor.Evaluate(m_lastTimeOfDayValue), m_fogColor.Evaluate(time), duration);
                components.m_fog.meanFreePath.value = LerpFloat(m_startingData.m_fogDistance.Evaluate(m_lastTimeOfDayValue),m_fogDistance.Evaluate(time), duration);
                float fogHeight = m_fogHeight.Evaluate(time);
                components.m_fog.baseHeight.value = LerpFloat(m_startingData.m_fogHeight.Evaluate(m_lastTimeOfDayValue), fogHeight, duration);
                components.m_fog.maximumHeight.value = LerpFloat(m_startingData.m_fogHeight.Evaluate(m_lastTimeOfDayValue) * 2f, fogHeight * 2f, duration);
                components.m_fog.depthExtent.value = LerpFloat(m_startingData.m_volumetricFogDistance.Evaluate(m_lastTimeOfDayValue), m_volumetricFogDistance.Evaluate(time), duration);
                components.m_fog.anisotropy.value = LerpFloat(m_startingData.m_volumetricFogAnisotropy.Evaluate(m_lastTimeOfDayValue), m_volumetricFogAnisotropy.Evaluate(time), duration);
                components.m_fog.sliceDistributionUniformity.value = LerpFloat(m_startingData.m_volumetricFogSliceDistributionUniformity.Evaluate(m_lastTimeOfDayValue), m_volumetricFogSliceDistributionUniformity.Evaluate(time), duration);
            }
            //Shadows
            if (ValidateShadows())
            {
                components.m_shadows.maxShadowDistance.value = LerpFloat(m_startingData.m_shadowDistance.Evaluate(m_lastTimeOfDayValue) * m_startingData.m_shadowDistanceMultiplier, m_shadowDistance.Evaluate(time) * m_shadowDistanceMultiplier, duration);
                components.m_shadows.directionalTransmissionMultiplier.value = LerpFloat(m_startingData.m_shadowTransmissionMultiplier.Evaluate(m_lastTimeOfDayValue), m_shadowTransmissionMultiplier.Evaluate(time), duration);
            }
            //Clouds
            if (ValidateClouds())
            {
                //Volumetric
                components.m_volumetricClouds.cloudPreset.value = m_cloudPresets;
                components.m_volumetricClouds.densityMultiplier.value = LerpFloat(m_startingData.m_volumetricDensityMultiplier.Evaluate(m_lastTimeOfDayValue), m_volumetricDensityMultiplier.Evaluate(time), duration);
                components.m_volumetricClouds.customDensityCurve.value = m_volumetricDensityCurve;
                components.m_volumetricClouds.shapeFactor.value = LerpFloat(m_startingData.m_volumetricShapeFactor.Evaluate(m_lastTimeOfDayValue), m_volumetricShapeFactor.Evaluate(time), duration);
                components.m_volumetricClouds.shapeScale.value = LerpFloat(m_startingData.m_volumetricShapeScale.Evaluate(m_lastTimeOfDayValue), m_volumetricShapeScale.Evaluate(time), duration);
                components.m_volumetricClouds.erosionFactor.value = LerpFloat(m_startingData.m_volumetricErosionFactor.Evaluate(m_lastTimeOfDayValue), m_volumetricErosionFactor.Evaluate(time), duration);
                components.m_volumetricClouds.erosionScale.value = LerpFloat(m_startingData.m_volumetricErosionScale.Evaluate(m_lastTimeOfDayValue), m_volumetricErosionScale.Evaluate(time), duration);
                components.m_volumetricClouds.lowestCloudAltitude.value = LerpFloat(m_startingData.m_volumetricLowestCloudAltitude.Evaluate(m_lastTimeOfDayValue), m_volumetricLowestCloudAltitude.Evaluate(time), duration);
                components.m_volumetricClouds.cloudThickness.value = LerpFloat(m_startingData.m_volumetricCloudThickness.Evaluate(m_lastTimeOfDayValue), m_volumetricCloudThickness.Evaluate(time), duration);
                components.m_volumetricClouds.ambientLightProbeDimmer.value = LerpFloat(m_startingData.m_volumetricAmbientLightProbeDimmer.Evaluate(m_lastTimeOfDayValue), m_volumetricAmbientLightProbeDimmer.Evaluate(time), duration);
                components.m_volumetricClouds.sunLightDimmer.value = LerpFloat(m_startingData.m_volumetricSunLightDimmer.Evaluate(m_lastTimeOfDayValue), m_volumetricSunLightDimmer.Evaluate(time), duration);
                components.m_volumetricClouds.erosionOcclusion.value = LerpFloat(m_startingData.m_volumetricErosionOcclusion.Evaluate(m_lastTimeOfDayValue), m_volumetricErosionOcclusion.Evaluate(time), duration);
                components.m_volumetricClouds.scatteringTint.value = LerpColor(m_startingData.m_volumetricScatteringTint.Evaluate(m_lastTimeOfDayValue), m_volumetricScatteringTint.Evaluate(time), duration);
                components.m_volumetricClouds.powderEffectIntensity.value = LerpFloat(m_startingData.m_volumetricPowderEffectIntensity.Evaluate(m_lastTimeOfDayValue), m_volumetricPowderEffectIntensity.Evaluate(time), duration);
                components.m_volumetricClouds.multiScattering.value = LerpFloat(m_startingData.m_volumetricMultiScattering.Evaluate(m_lastTimeOfDayValue), m_volumetricMultiScattering.Evaluate(time), duration);
                components.m_volumetricClouds.shadowOpacity.value = LerpFloat(m_startingData.m_volumetricCloudShadowOpacity.Evaluate(m_lastTimeOfDayValue), m_volumetricCloudShadowOpacity.Evaluate(time), duration);
                //Procedural
                components.m_cloudLayer.opacity.value = LerpFloat(m_startingData.m_cloudOpacity.Evaluate(m_lastTimeOfDayValue), m_cloudOpacity.Evaluate(time), duration);
                components.m_cloudLayer.layerA.tint.value = LerpColor(m_startingData.m_cloudTintColor.Evaluate(m_lastTimeOfDayValue), m_cloudTintColor.Evaluate(time), duration);
                components.m_cloudLayer.layerB.tint.value = LerpColor(m_startingData.m_cloudTintColor.Evaluate(m_lastTimeOfDayValue), m_cloudTintColor.Evaluate(time), duration);
                components.m_cloudLayer.layerA.exposure.value = LerpFloat(m_startingData.m_cloudExposure.Evaluate(m_lastTimeOfDayValue), m_cloudExposure.Evaluate(time), duration);
                components.m_cloudLayer.layerB.exposure.value = LerpFloat(m_startingData.m_cloudExposure.Evaluate(m_lastTimeOfDayValue), m_cloudExposure.Evaluate(time), duration);
                components.m_cloudLayer.shadowMultiplier.value = LerpFloat(m_startingData.m_cloudShadowOpacity.Evaluate(m_lastTimeOfDayValue), m_cloudShadowOpacity.Evaluate(time), duration);
                components.m_cloudLayer.shadowTint.value = LerpColor(m_startingData.m_cloudShadowColor.Evaluate(m_lastTimeOfDayValue), m_cloudShadowColor.Evaluate(time), duration);
                components.m_cloudLayer.layerA.opacityR.value = LerpFloat(m_startingData.m_cloudLayerAOpacityR.Evaluate(m_lastTimeOfDayValue), m_cloudLayerAOpacityR.Evaluate(time), duration);
                components.m_cloudLayer.layerA.opacityG.value = LerpFloat(m_startingData.m_cloudLayerAOpacityG.Evaluate(m_lastTimeOfDayValue), m_cloudLayerAOpacityG.Evaluate(time), duration);
                components.m_cloudLayer.layerA.opacityB.value = LerpFloat(m_startingData.m_cloudLayerAOpacityB.Evaluate(m_lastTimeOfDayValue), m_cloudLayerAOpacityB.Evaluate(time), duration);
                components.m_cloudLayer.layerA.opacityA.value = LerpFloat(m_startingData.m_cloudLayerAOpacityA.Evaluate(m_lastTimeOfDayValue), m_cloudLayerAOpacityA.Evaluate(time), duration);
                components.m_cloudLayer.layerB.opacityR.value = LerpFloat(m_startingData.m_cloudLayerBOpacityR.Evaluate(m_lastTimeOfDayValue), m_cloudLayerBOpacityR.Evaluate(time), duration);
                components.m_cloudLayer.layerB.opacityG.value = LerpFloat(m_startingData.m_cloudLayerBOpacityG.Evaluate(m_lastTimeOfDayValue), m_cloudLayerBOpacityG.Evaluate(time), duration);
                components.m_cloudLayer.layerB.opacityB.value = LerpFloat(m_startingData.m_cloudLayerBOpacityB.Evaluate(m_lastTimeOfDayValue), m_cloudLayerBOpacityB.Evaluate(time), duration);
                components.m_cloudLayer.layerB.opacityA.value = LerpFloat(m_startingData.m_cloudLayerBOpacityA.Evaluate(m_lastTimeOfDayValue), m_cloudLayerBOpacityA.Evaluate(time), duration);
            }
            //Sun Flare
            if (ValidateSunLensFlare())
            {
                components.m_sunLensFlare.intensity = LerpFloat(m_startingData.m_sunLensFlareProfile.m_intensity.Evaluate(m_lastTimeOfDayValue), m_sunLensFlareProfile.m_intensity.Evaluate(time), duration);
                components.m_sunLensFlare.scale = LerpFloat(m_startingData.m_sunLensFlareProfile.m_scale.Evaluate(m_lastTimeOfDayValue), m_sunLensFlareProfile.m_scale.Evaluate(time), duration);
            }
            //Moon Flare
            if (ValidateMoonLensFlare())
            {
                components.m_moonLensFlare.intensity = LerpFloat(m_startingData.m_moonLensFlareProfile.m_intensity.Evaluate(m_lastTimeOfDayValue), m_moonLensFlareProfile.m_intensity.Evaluate(time), duration);
                components.m_moonLensFlare.scale = LerpFloat(m_startingData.m_moonLensFlareProfile.m_scale.Evaluate(m_lastTimeOfDayValue), m_moonLensFlareProfile.m_scale.Evaluate(time), duration);
            }

            return false;
        }
        /// <summary>
        /// Sets up the time of day to allow transitions
        /// </summary>
        /// <param name="startingData"></param>
        /// <param name="currentTimeOfDayValue"></param>
        public void SetupStartingSettings(TimeOfDayProfileData startingData, float currentTimeOfDayValue)
        {
            m_startingData = startingData;
            m_lastTimeOfDayValue = currentTimeOfDayValue;
            m_hasBeenSetup = startingData != null && m_lastTimeOfDayValue != -1;
        }
        /// <summary>
        /// Resets the setup for this profile
        /// </summary>
        public void Reset()
        {
            m_startingData = null;
            m_hasBeenSetup = false;
            m_lastTimeOfDayValue = -1f;
        }
        /// <summary>
        /// Copies all the settings to another profile
        /// </summary>
        /// <param name="copyTo"></param>
        public static void CopySettings(TimeOfDayProfileData copyTo, TimeOfDayProfileData copyFrom)
        {
            if (copyTo != null && copyFrom != null)
            {
                //Sun
                copyTo.m_sunIntensity = CopyCurve(copyFrom.m_sunIntensity);
                copyTo.m_moonIntensity = CopyCurve(copyFrom.m_moonIntensity);
                copyTo.m_sunIntensityMultiplier = CopyCurve(copyFrom.m_sunIntensityMultiplier);
                copyTo.m_sunTemperature = CopyCurve(copyFrom.m_sunTemperature);
                copyTo.m_sunColorFilter = CopyGradient(copyFrom.m_sunColorFilter);
                copyTo.m_moonTemperature = CopyCurve(copyFrom.m_moonTemperature);
                copyTo.m_moonColorFilter = CopyGradient(copyFrom.m_moonColorFilter);
                copyTo.m_sunVolumetrics = CopyCurve(copyFrom.m_sunVolumetrics);
                copyTo.m_sunVolumetricShadowDimmer = CopyCurve(copyFrom.m_sunVolumetricShadowDimmer);
                copyTo.m_enableSunShadows = copyFrom.m_enableSunShadows;
                copyTo.m_enableMoonShadows = copyFrom.m_enableMoonShadows;
                copyTo.m_globalLightMultiplier = copyFrom.m_globalLightMultiplier;
                //Sky
                copyTo.m_horizonOffset = copyFrom.m_horizonOffset;
                copyTo.m_skyMode = copyFrom.m_skyMode;
                copyTo.m_skyboxExposure = copyFrom.m_skyboxExposure;
                copyTo.m_skyboxGroundColor = copyFrom.m_skyboxGroundColor;
                copyTo.m_skyTopColor = CopyGradient(copyFrom.m_skyTopColor);
                copyTo.m_skyMiddleColor = CopyGradient(copyFrom.m_skyMiddleColor);
                copyTo.m_skyBottomColor = CopyGradient(copyFrom.m_skyBottomColor);
                copyTo.m_skyGradientDiffusion = CopyCurve(copyFrom.m_skyGradientDiffusion);
                copyTo.m_skyExposureGradient = CopyCurve(copyFrom.m_skyExposureGradient);
                copyTo.m_starsDayIntensity = copyFrom.m_starsDayIntensity;
                copyTo.m_starsNightIntensity = copyFrom.m_starsNightIntensity;
                //Shadows
                copyTo.m_shadowDistance = CopyCurve(copyFrom.m_shadowDistance);
                copyTo.m_shadowTransmissionMultiplier = CopyCurve(copyFrom.m_shadowTransmissionMultiplier);
                copyTo.m_shadowCascadeCount = copyFrom.m_shadowCascadeCount;
                copyTo.m_shadowDistanceMultiplier = copyFrom.m_shadowDistanceMultiplier;
                //Fog
                copyTo.m_fogColor = CopyGradient(copyFrom.m_fogColor);
                copyTo.m_fogDistance = CopyCurve(copyFrom.m_fogDistance);
                copyTo.m_fogDensity = CopyCurve(copyFrom.m_fogDensity);
                copyTo.m_globalFogMultiplier = copyFrom.m_globalFogMultiplier;
                copyTo.m_fogHeight = CopyCurve(copyFrom.m_fogHeight);
                copyTo.m_volumetricFogDistance = CopyCurve(copyFrom.m_volumetricFogDistance);
                copyTo.m_volumetricFogAnisotropy = CopyCurve(copyFrom.m_volumetricFogAnisotropy);
                copyTo.m_volumetricFogSliceDistributionUniformity = CopyCurve(copyFrom.m_volumetricFogSliceDistributionUniformity);
                copyTo.m_localFogMultiplier = CopyCurve(copyFrom.m_localFogMultiplier);
                //Clouds Volumetric
                copyTo.m_cloudPresets = copyFrom.m_cloudPresets;
                copyTo.m_volumetricDensityMultiplier = CopyCurve(copyFrom.m_volumetricDensityMultiplier);
                copyTo.m_volumetricDensityCurve = CopyCurve(copyFrom.m_volumetricDensityCurve);
                copyTo.m_volumetricShapeFactor = CopyCurve(copyFrom.m_volumetricShapeFactor);
                copyTo.m_volumetricShapeScale = CopyCurve(copyFrom.m_volumetricShapeScale);
                copyTo.m_volumetricErosionFactor = CopyCurve(copyFrom.m_volumetricErosionFactor);
                copyTo.m_volumetricErosionScale = CopyCurve(copyFrom.m_volumetricErosionScale);
                copyTo.m_erosionNoiseType = copyFrom.m_erosionNoiseType;
                copyTo.m_volumetricErosionCurve = CopyCurve(copyFrom.m_volumetricErosionCurve);
                copyTo.m_volumetricAmbientOcclusionCurve = CopyCurve(copyFrom.m_volumetricAmbientOcclusionCurve);
                copyTo.m_volumetricLowestCloudAltitude = CopyCurve(copyFrom.m_volumetricLowestCloudAltitude);
                copyTo.m_volumetricCloudThickness = CopyCurve(copyFrom.m_volumetricCloudThickness);
                copyTo.m_volumetricAmbientLightProbeDimmer = CopyCurve(copyFrom.m_volumetricAmbientLightProbeDimmer);
                copyTo.m_volumetricSunLightDimmer = CopyCurve(copyFrom.m_volumetricSunLightDimmer);
                copyTo.m_volumetricErosionOcclusion = CopyCurve(copyFrom.m_volumetricErosionOcclusion);
                copyTo.m_volumetricScatteringTint = CopyGradient(copyFrom.m_volumetricScatteringTint);
                copyTo.m_volumetricPowderEffectIntensity = CopyCurve(copyFrom.m_volumetricPowderEffectIntensity);
                copyTo.m_volumetricMultiScattering = CopyCurve(copyFrom.m_volumetricMultiScattering);
                copyTo.m_volumetricCloudShadowResolution = copyFrom.m_volumetricCloudShadowResolution;
                copyTo.m_volumetricCloudShadowOpacity = CopyCurve(copyFrom.m_volumetricCloudShadowOpacity);
                copyTo.m_volumetricScatteringTint = CopyGradient(copyFrom.m_volumetricScatteringTint);
                //Cloud Procedural
                copyTo.m_cloudLayers = copyFrom.m_cloudLayers;
                copyTo.m_cloudResolution = copyFrom.m_cloudResolution;
                copyTo.m_useCloudShadows = copyFrom.m_useCloudShadows;
                copyTo.m_cloudOpacity = CopyCurve(copyFrom.m_cloudOpacity);
                copyTo.m_cloudTintColor = CopyGradient(copyFrom.m_cloudTintColor);
                copyTo.m_cloudExposure = CopyCurve(copyFrom.m_cloudExposure);
                copyTo.m_cloudWindDirection = CopyCurve(copyFrom.m_cloudWindDirection);
                copyTo.m_cloudWindSpeed = CopyCurve(copyFrom.m_cloudWindSpeed);
                copyTo.m_cloudShadowOpacity = CopyCurve(copyFrom.m_cloudShadowOpacity);
                copyTo.m_cloudLighting = copyFrom.m_cloudLighting;
                copyTo.m_cloudShadowColor = CopyGradient(copyFrom.m_cloudShadowColor);
                copyTo.m_cloudLayerAOpacityR = CopyCurve(copyFrom.m_cloudLayerAOpacityR);
                copyTo.m_cloudLayerAOpacityG = CopyCurve(copyFrom.m_cloudLayerAOpacityG);
                copyTo.m_cloudLayerAOpacityB = CopyCurve(copyFrom.m_cloudLayerAOpacityB);
                copyTo.m_cloudLayerAOpacityA = CopyCurve(copyFrom.m_cloudLayerAOpacityA);
                copyTo.m_cloudLayerBOpacityR = CopyCurve(copyFrom.m_cloudLayerBOpacityR);
                copyTo.m_cloudLayerBOpacityG = CopyCurve(copyFrom.m_cloudLayerBOpacityG);
                copyTo.m_cloudLayerBOpacityB = CopyCurve(copyFrom.m_cloudLayerBOpacityB);
                copyTo.m_cloudLayerBOpacityA = CopyCurve(copyFrom.m_cloudLayerBOpacityA);
                copyTo.m_cloudLayerAChannel = copyFrom.m_cloudLayerAChannel;
                copyTo.m_cloudLayerBChannel = copyFrom.m_cloudLayerBChannel;
                //Advanced Lighting
                copyTo.m_useSSGI = copyFrom.m_useSSGI;
                copyTo.m_ssgiQuality = copyFrom.m_ssgiQuality;
                copyTo.m_useSSR = copyFrom.m_useSSR;
                copyTo.m_ssrQuality = copyFrom.m_ssrQuality;
                copyTo.m_useContactShadows = copyFrom.m_useContactShadows;
                copyTo.m_contactShadowsDistance = CopyCurve(copyFrom.m_contactShadowsDistance);
                copyTo.m_useMicroShadows = copyFrom.m_useMicroShadows;
                copyTo.m_generalExposure = CopyCurve(copyFrom.m_generalExposure);
                copyTo.m_ambientIntensity = CopyCurve(copyFrom.m_ambientIntensity);
                copyTo.m_ambientReflectionIntensity = CopyCurve(copyFrom.m_ambientReflectionIntensity);
                copyTo.m_planarReflectionIntensity = CopyCurve(copyFrom.m_planarReflectionIntensity);
                copyFrom.m_sunLensFlareProfile.CopySettings(ref copyTo.m_sunLensFlareProfile);
                copyFrom.m_moonLensFlareProfile.CopySettings(ref copyTo.m_moonLensFlareProfile);
            }
        }
        /// <summary>
        /// Copies animation curve into a new animation curve field
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static AnimationCurve CopyCurve(AnimationCurve curve)
        {
            if (curve != null)
            {
                Keyframe[] keyFrames = curve.keys;
                AnimationCurve newAnimationCurve = new AnimationCurve();
                foreach (Keyframe keyframe in keyFrames)
                {
                    newAnimationCurve.AddKey(keyframe);
                }

                return newAnimationCurve;
            }
            return new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
        }
        /// <summary>
        /// Copies gradient into a new gradient field
        /// </summary>
        /// <param name="gradient"></param>
        /// <returns></returns>
        public static Gradient CopyGradient(Gradient gradient)
        {
            if (gradient != null)
            {
                GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
                GradientColorKey[] colorKeys = gradient.colorKeys;
                Gradient newGradient = new Gradient();
                GradientAlphaKey[] newAlphaKeys = new GradientAlphaKey[gradient.alphaKeys.Length];
                for (int i = 0; i < gradient.alphaKeys.Length; i++)
                {
                    newAlphaKeys[i].time = alphaKeys[i].time;
                    newAlphaKeys[i].alpha = alphaKeys[i].alpha;
                }

                GradientColorKey[] newColorKeys = new GradientColorKey[gradient.colorKeys.Length];
                for (int i = 0; i < gradient.colorKeys.Length; i++)
                {
                    newColorKeys[i].time = colorKeys[i].time;
                    newColorKeys[i].color = colorKeys[i].color;
                }
                newGradient.SetKeys(newColorKeys, newAlphaKeys);
                return newGradient;
            }

            return new Gradient();
        }

        public bool ValidateSun()
        {
            if (m_sunIntensity == null)
            {
                return false;
            }
            if (m_moonIntensity == null)
            {
                return false;
            }
            if (m_sunIntensityMultiplier == null)
            {
                return false;
            }
            if (m_sunTemperature == null)
            {
                return false;
            }
            if (m_sunColorFilter == null)
            {
                return false;
            }
            if (m_moonTemperature == null)
            {
                return false;
            }
            if (m_moonColorFilter == null)
            {
                return false;
            }
            if (m_sunVolumetrics == null)
            {
                return false;
            }
            if (m_sunVolumetricShadowDimmer == null)
            {
                return false;
            }

            return true;
        }
        public void ApplySunSettings(HDAdditionalLightData lightData, float time, bool isDay, bool overrideSource, OverrideDataInfo overrideData)
        {
            lightData.EnableColorTemperature(true);
            if (!overrideSource)
            {
                if (isDay)
                {
                    lightData.SetIntensity(m_sunIntensity.Evaluate(time) * m_globalLightMultiplier);
                    lightData.SetColor(m_sunColorFilter.Evaluate(time), m_sunTemperature.Evaluate(time));
                    lightData.EnableShadows(m_enableSunShadows);
                }
                else
                {
                    lightData.SetIntensity(m_moonIntensity.Evaluate(time) * m_globalLightMultiplier);
                    lightData.SetColor(m_moonColorFilter.Evaluate(time), m_moonTemperature.Evaluate(time));
                    lightData.EnableShadows(m_enableMoonShadows);
                }
            }

            lightData.affectsVolumetric = true;
            lightData.lightDimmer = m_sunIntensityMultiplier.Evaluate(time);
            bool transitionComplete = overrideData.m_transitionTime < 1f;
            if (!overrideData.m_isInVolue)
            {
                if (transitionComplete)
                {
                    lightData.volumetricDimmer = Mathf.Lerp(lightData.volumetricDimmer, m_sunVolumetrics.Evaluate(time), overrideData.m_transitionTime);
                    lightData.volumetricShadowDimmer = Mathf.Lerp(lightData.volumetricShadowDimmer, m_sunVolumetricShadowDimmer.Evaluate(time), overrideData.m_transitionTime);
                }
                else
                {
                    lightData.volumetricDimmer = m_sunVolumetrics.Evaluate(time);
                    lightData.volumetricShadowDimmer = m_sunVolumetricShadowDimmer.Evaluate(time);
                }
            }
            else
            {
                if (overrideData.m_settings != null)
                {
                    if (overrideData.m_settings.m_sunVolumetric.overrideState)
                    {
                        if (transitionComplete)
                        {
                            lightData.volumetricDimmer = Mathf.Lerp(lightData.volumetricDimmer, overrideData.m_settings.m_sunVolumetric.value, overrideData.m_transitionTime);
                        }
                        else
                        {
                            lightData.volumetricDimmer = overrideData.m_settings.m_sunVolumetric.value;
                        }
                    }
                    else
                    {
                        if (transitionComplete)
                        {
                            lightData.volumetricDimmer = Mathf.Lerp(lightData.volumetricDimmer, m_sunVolumetrics.Evaluate(time), overrideData.m_transitionTime);
                        }
                        else
                        {
                            lightData.volumetricDimmer = m_sunVolumetrics.Evaluate(time);
                        }
                    }

                    if (overrideData.m_settings.m_sunVolumetricDimmer.overrideState)
                    {
                        if (transitionComplete)
                        {
                            lightData.volumetricShadowDimmer = Mathf.Lerp(lightData.volumetricShadowDimmer, overrideData.m_settings.m_sunVolumetricDimmer.value, overrideData.m_transitionTime);
                        }
                        else
                        {
                            lightData.volumetricShadowDimmer = overrideData.m_settings.m_sunVolumetricDimmer.value;
                        }
                    }
                    else
                    {
                        if (transitionComplete)
                        {
                            lightData.volumetricShadowDimmer = Mathf.Lerp(lightData.volumetricShadowDimmer, m_sunVolumetricShadowDimmer.Evaluate(time), overrideData.m_transitionTime);
                        }
                        else
                        {
                            lightData.volumetricShadowDimmer = m_sunVolumetricShadowDimmer.Evaluate(time);
                        }
                    }
                }
            }
        }

        public bool ValidateSky()
        {
            if (m_skyTopColor == null)
            {
                return false;
            }
            if (m_skyMiddleColor == null)
            {
                return false;
            }
            if (m_skyBottomColor == null)
            {
                return false;
            }
            if (m_skyGradientDiffusion == null)
            {
                return false;
            }
            return true;
        }
        public void ApplySkySettings(HDRPTimeOfDayComponents components, float time)
        {
            //Physically based
            components.m_physicallyBasedSky.updateMode.value = EnvironmentUpdateMode.OnChanged;
            //Gradient
            components.m_gradientSky.top.value = m_skyTopColor.Evaluate(time);
            components.m_gradientSky.middle.value = m_skyMiddleColor.Evaluate(time);
            components.m_gradientSky.bottom.value = m_skyBottomColor.Evaluate(time);
            components.m_gradientSky.gradientDiffusion.value = m_skyGradientDiffusion.Evaluate(time);
            components.m_gradientSky.exposure.value = m_skyExposureGradient.Evaluate(time);
        }

        public bool ValidateAdvancedLighting()
        {
            if (m_generalExposure == null)
            {
                return false;
            }
            if (m_ambientIntensity == null)
            {
                m_ambientIntensity = AnimationCurve.Constant(0f, 1f, 1f);
                return false;
            }
            if (m_ambientReflectionIntensity == null)
            {
                m_ambientReflectionIntensity = AnimationCurve.Constant(0f, 1f, 1f);
                return false;
            }
            if (m_planarReflectionIntensity == null)
            {
                m_planarReflectionIntensity = AnimationCurve.Constant(0f, 1f, 1f);
                return false;
            }

            if (m_contactShadowsDistance == null)
            {
                m_contactShadowsDistance = AnimationCurve.Constant(0f, 1f, 10000f);
                return false;
            }

            return true;
        }
        public void ApplyAdvancedLighting(HDRPTimeOfDayComponents components, float time, OverrideDataInfo overrideData)
        {
            if (components == null)
            {
                return;
            }

            bool transitionComplete = overrideData.m_transitionTime < 1f;
            //SSGI
            components.m_globalIllumination.active = true;
            components.m_globalIllumination.enable.value = m_useSSGI;
            components.m_globalIllumination.quality.value = (int)m_ssgiQuality;
            switch (m_ssgiRenderMode)
            {
                case SSGIRenderMode.Interior:
                {
                    components.m_globalIllumination.rayMiss.value = RayMarchingFallbackHierarchy.ReflectionProbes;
                    break;
                }
                case SSGIRenderMode.Exterior:
                {
                    components.m_globalIllumination.rayMiss.value = RayMarchingFallbackHierarchy.Sky;
                    break;
                }
                case SSGIRenderMode.Both:
                {
                    components.m_globalIllumination.rayMiss.value = RayMarchingFallbackHierarchy.ReflectionProbesAndSky;
                    break;
                }
            }
            //SSR
            components.m_screenSpaceReflection.active = true;
            components.m_screenSpaceReflection.enabled.value = m_useSSR;
            components.m_screenSpaceReflection.active = m_useSSR;
            components.m_screenSpaceReflection.quality.value = (int)m_ssrQuality;
            //Exposure
            if (!overrideData.m_isInVolue)
            {
                if (transitionComplete)
                {
                    components.m_exposure.fixedExposure.value = Mathf.Lerp(components.m_exposure.fixedExposure.value, m_generalExposure.Evaluate(time), overrideData.m_transitionTime);
                }
                else
                {
                    components.m_exposure.fixedExposure.value = m_generalExposure.Evaluate(time);
                }
            }
            else
            {
                if (overrideData.m_settings != null)
                {
                    if (overrideData.m_settings.m_exposure.overrideState)
                    {
                        if (transitionComplete)
                        {
                            components.m_exposure.fixedExposure.value = Mathf.Lerp(components.m_exposure.fixedExposure.value, overrideData.m_settings.m_exposure.value, overrideData.m_transitionTime);
                        }
                        else
                        {
                            components.m_exposure.fixedExposure.value = overrideData.m_settings.m_exposure.value;
                        }
                    }
                    else
                    {
                        if (transitionComplete)
                        {
                            components.m_exposure.fixedExposure.value = Mathf.Lerp(components.m_exposure.fixedExposure.value, m_generalExposure.Evaluate(time), overrideData.m_transitionTime);
                        }
                        else
                        {
                            components.m_exposure.fixedExposure.value = m_generalExposure.Evaluate(time);
                        }
                    }
                }
            }
            //Shadows
            components.m_contactShadows.active = true;
            components.m_contactShadows.enable.value = m_useContactShadows;
            components.m_contactShadows.maxDistance.value = Mathf.Clamp(m_contactShadowsDistance.Evaluate(time), 0.01f, float.MaxValue);
            components.m_microShadowing.active = true;
            components.m_microShadowing.enable.value = m_useMicroShadows;
            //Ambient
            if (!overrideData.m_isInVolue)
            {
                if (transitionComplete)
                {
                    components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value = Mathf.Lerp(components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value, m_ambientIntensity.Evaluate(time), overrideData.m_transitionTime);
                    components.m_indirectLightingController.reflectionLightingMultiplier.value = Mathf.Lerp(components.m_indirectLightingController.reflectionLightingMultiplier.value, m_ambientReflectionIntensity.Evaluate(time), overrideData.m_transitionTime);
                }
                else
                {
                    components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value = m_ambientIntensity.Evaluate(time);
                    components.m_indirectLightingController.reflectionLightingMultiplier.value = m_ambientReflectionIntensity.Evaluate(time);
                    components.m_indirectLightingController.reflectionProbeIntensityMultiplier.value = m_planarReflectionIntensity.Evaluate(time);
                }
            }
            else
            {
                if (overrideData.m_settings.m_ambientIntensity.overrideState)
                {
                    if (transitionComplete)
                    {
                        components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value = Mathf.Lerp(components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value, overrideData.m_settings.m_ambientIntensity.value, overrideData.m_transitionTime);
                    }
                    else
                    {
                        components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value = overrideData.m_settings.m_ambientIntensity.value;
                    }
                }
                else
                {
                    if (transitionComplete)
                    {
                        components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value = Mathf.Lerp(components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value, m_ambientIntensity.Evaluate(time), overrideData.m_transitionTime);
                    }
                    else
                    {
                        components.m_indirectLightingController.indirectDiffuseLightingMultiplier.value = m_ambientIntensity.Evaluate(time);
                    }
                }
                
                if (overrideData.m_settings.m_ambientReflectionIntensity.overrideState)
                {
                    if (transitionComplete)
                    {
                        components.m_indirectLightingController.reflectionLightingMultiplier.value = Mathf.Lerp(components.m_indirectLightingController.reflectionLightingMultiplier.value, overrideData.m_settings.m_ambientReflectionIntensity.value, overrideData.m_transitionTime);
                    }
                    else
                    {
                        components.m_indirectLightingController.reflectionLightingMultiplier.value = overrideData.m_settings.m_ambientReflectionIntensity.value;
                    }
                }
                else
                {
                    if (transitionComplete)
                    {
                        components.m_indirectLightingController.reflectionLightingMultiplier.value = Mathf.Lerp(components.m_indirectLightingController.reflectionLightingMultiplier.value, m_ambientReflectionIntensity.Evaluate(time), overrideData.m_transitionTime);
                    }
                    else
                    {
                        components.m_indirectLightingController.reflectionLightingMultiplier.value = m_ambientReflectionIntensity.Evaluate(time);
                    }
                }
            }
        }

        public bool ValidateFog()
        {
            if (m_fogColor == null)
            {
                return false;
            }
            if (m_fogDistance == null)
            {
                return false;
            }
            if (m_fogHeight == null)
            {
                return false;
            }
            if (m_volumetricFogDistance == null)
            {
                return false;
            }
            if (m_volumetricFogAnisotropy == null)
            {
                return false;
            }
            if (m_volumetricFogSliceDistributionUniformity == null)
            {
                return false;
            }
            if (m_localFogMultiplier == null)
            {
                return false;
            }
            if (m_fogDensity == null)
            {
                return false;
            }

            return true;
        }
        public void ApplyFogSettings(HDRPTimeOfDayComponents components, float time, out Color currrentFogColor, out float currentFogDistance)
        {
            currrentFogColor = Color.white;
            currentFogDistance = 150f;
            components.m_localVolumetricFog.parameters.meanFreePath = m_fogDensity.Evaluate(time) / m_globalFogMultiplier;
            currentFogDistance = components.m_localVolumetricFog.parameters.meanFreePath;
            components.m_localVolumetricFog.parameters.albedo = m_fogColor.Evaluate(time) * m_localFogMultiplier.Evaluate(time);
            currrentFogColor = components.m_localVolumetricFog.parameters.albedo;

            components.m_fog.albedo.value = m_fogColor.Evaluate(time);
            components.m_fog.meanFreePath.value = m_fogDistance.Evaluate(time);
            float fogHeight = m_fogHeight.Evaluate(time);
            components.m_fog.baseHeight.value = fogHeight;
            components.m_fog.maximumHeight.value = fogHeight * 2f;
            components.m_fog.depthExtent.value = m_volumetricFogDistance.Evaluate(time);
            components.m_fog.anisotropy.value = m_volumetricFogAnisotropy.Evaluate(time);
            components.m_fog.sliceDistributionUniformity.value = m_volumetricFogSliceDistributionUniformity.Evaluate(time);
            components.m_fog.quality.value = (int)m_fogQuality;
            if (m_useDenoising)
            {
                switch (m_denoisingQuality)
                {
                    case GeneralQuality.Low:
                    {
                        components.m_fog.denoisingMode.value = FogDenoisingMode.Reprojection;
                        break;
                    }
                    case GeneralQuality.Medium:
                    {
                        components.m_fog.denoisingMode.value = FogDenoisingMode.Gaussian;
                        break;
                    }
                    case GeneralQuality.High:
                    {
                        components.m_fog.denoisingMode.value = FogDenoisingMode.Both;
                        break;
                    }
                }
            }
            else
            {
                components.m_fog.denoisingMode.value = FogDenoisingMode.None;
            }
        }

        public bool ValidateShadows()
        {
            if (m_shadowDistance == null)
            {
                return false;
            }
            if (m_shadowTransmissionMultiplier == null)
            {
                return false;
            }

            return true;
        }
        public void ApplyShadowSettings(HDRPTimeOfDayComponents components, float time, OverrideDataInfo overrideData)
        {
            components.m_shadows.directionalTransmissionMultiplier.value = m_shadowTransmissionMultiplier.Evaluate(time);
            components.m_shadows.cascadeShadowSplitCount.value = Mathf.Clamp(m_shadowCascadeCount, 1, 4);

            bool transitionComplete = overrideData.m_transitionTime < 1f;
            if (!overrideData.m_isInVolue)
            {
                if (transitionComplete)
                {
                    components.m_shadows.maxShadowDistance.value = Mathf.Lerp(components.m_shadows.maxShadowDistance.value, (m_shadowDistance.Evaluate(time) * m_shadowDistanceMultiplier), overrideData.m_transitionTime);
                }
                else
                {
                    components.m_shadows.maxShadowDistance.value = (m_shadowDistance.Evaluate(time) * m_shadowDistanceMultiplier);
                }
            }
            else
            {
                if (overrideData.m_settings.m_shadowDistanceMultiplier.overrideState)
                {
                    if (transitionComplete)
                    {
                        components.m_shadows.maxShadowDistance.value = Mathf.Lerp(components.m_shadows.maxShadowDistance.value,  (m_shadowDistance.Evaluate(time) * overrideData.m_settings.m_shadowDistanceMultiplier.value), overrideData.m_transitionTime);
                    }
                    else
                    {
                        components.m_shadows.maxShadowDistance.value = (m_shadowDistance.Evaluate(time) * overrideData.m_settings.m_shadowDistanceMultiplier.value);
                    }
                }
                else
                {
                    if (transitionComplete)
                    {
                        components.m_shadows.maxShadowDistance.value = Mathf.Lerp(components.m_shadows.maxShadowDistance.value, (m_shadowDistance.Evaluate(time) * m_shadowDistanceMultiplier), overrideData.m_transitionTime);
                    }
                    else
                    {
                        components.m_shadows.maxShadowDistance.value = (m_shadowDistance.Evaluate(time) * m_shadowDistanceMultiplier);
                    }
                }
            }
        }

        public bool ValidateClouds()
        {
            //Procedural
            if (m_cloudOpacity == null)
            {
                return false;
            }
            if (m_cloudTintColor == null)
            {
                return false;
            }
            if (m_cloudExposure == null)
            {
                return false;
            }
            if (m_cloudWindDirection == null)
            {
                return false;
            }
            if (m_cloudWindSpeed == null)
            {
                return false;
            }
            if (m_cloudShadowOpacity == null)
            {
                return false;
            }
            if (m_cloudShadowColor == null)
            {
                return false;
            }
            if (m_cloudLayerAOpacityR == null)
            {
                return false;
            }
            if (m_cloudLayerAOpacityG == null)
            {
                return false;
            }
            if (m_cloudLayerAOpacityB == null)
            {
                return false;
            }
            if (m_cloudLayerAOpacityA == null)
            {
                return false;
            }
            if (m_cloudLayerBOpacityR == null)
            {
                return false;
            }
            if (m_cloudLayerBOpacityG == null)
            {
                return false;
            }
            if (m_cloudLayerBOpacityB == null)
            {
                return false;
            }
            if (m_cloudLayerBOpacityA == null)
            {
                return false;
            }
            //Volumetic
            if (m_volumetricDensityMultiplier == null)
            {
                return false;
            }
            if (m_volumetricDensityCurve == null)
            {
                return false;
            }
            if (m_volumetricShapeFactor == null)
            {
                return false;
            }
            if (m_volumetricShapeScale == null)
            {
                return false;
            }
            if (m_volumetricErosionFactor == null)
            {
                return false;
            }
            if (m_volumetricErosionScale == null)
            {
                return false;
            }
            if (m_volumetricErosionCurve == null)
            {
                return false;
            }
            if (m_volumetricAmbientOcclusionCurve == null)
            {
                return false;
            }
            if (m_volumetricAmbientLightProbeDimmer == null)
            {
                return false;
            }
            if (m_volumetricSunLightDimmer == null)
            {
                return false;
            }
            if (m_volumetricErosionOcclusion == null)
            {
                return false;
            }
            if (m_volumetricScatteringTint == null)
            {
                return false;
            }
            if (m_volumetricPowderEffectIntensity == null)
            {
                return false;
            }
            if (m_volumetricMultiScattering == null)
            {
                return false;
            }
            if (m_volumetricCloudShadowOpacity == null)
            {
                return false;
            }

            return true;
        }
        public float ApplyCloudSettings(HDRPTimeOfDayComponents components, float time)
        {
            #region Volumetric

            components.m_volumetricClouds.cloudPreset.value = m_cloudPresets;
            components.m_volumetricClouds.localClouds.value = m_useLocalClouds;
            components.m_volumetricClouds.densityMultiplier.value = m_volumetricDensityMultiplier.Evaluate(time);
            components.m_volumetricClouds.customDensityCurve.value = m_volumetricDensityCurve;
            components.m_volumetricClouds.shapeFactor.value = m_volumetricShapeFactor.Evaluate(time);
            components.m_volumetricClouds.shapeScale.value = m_volumetricShapeScale.Evaluate(time);
            components.m_volumetricClouds.erosionFactor.value = m_volumetricErosionFactor.Evaluate(time);
            components.m_volumetricClouds.erosionScale.value = m_volumetricErosionScale.Evaluate(time);
            components.m_volumetricClouds.erosionNoiseType.value = m_erosionNoiseType;
            components.m_volumetricClouds.customErosionCurve.value = m_volumetricErosionCurve;
            components.m_volumetricClouds.customAmbientOcclusionCurve.value = m_volumetricAmbientOcclusionCurve;
            components.m_volumetricClouds.lowestCloudAltitude.value = m_volumetricLowestCloudAltitude.Evaluate(time);
            components.m_volumetricClouds.cloudThickness.value = m_volumetricCloudThickness.Evaluate(time);
            //Lighting
            components.m_volumetricClouds.ambientLightProbeDimmer.value = m_volumetricAmbientLightProbeDimmer.Evaluate(time);
            components.m_volumetricClouds.sunLightDimmer.value = m_volumetricSunLightDimmer.Evaluate(time);
            components.m_volumetricClouds.erosionOcclusion.value = m_volumetricErosionOcclusion.Evaluate(time);
            components.m_volumetricClouds.scatteringTint.value = m_volumetricScatteringTint.Evaluate(time);
            components.m_volumetricClouds.powderEffectIntensity.value = m_volumetricPowderEffectIntensity.Evaluate(time);
            components.m_volumetricClouds.multiScattering.value = m_volumetricMultiScattering.Evaluate(time);
            //Shadow
            components.m_volumetricClouds.shadows.value = m_volumetricCloudShadows;
            if (m_volumetricCloudShadows)
            {
                components.m_volumetricClouds.shadowResolution.value = m_volumetricCloudShadowResolution;
                components.m_volumetricClouds.shadowOpacity.value = m_volumetricCloudShadowOpacity.Evaluate(time);
            }

            #endregion
            #region Procedural

            components.m_cloudLayer.layers.value = (CloudMapMode)m_cloudLayers;
            switch (m_cloudResolution)
            {
                case CloudResolution.Resolution256:
                {
                    components.m_cloudLayer.resolution.value = UnityEngine.Rendering.HighDefinition.CloudResolution.CloudResolution256;
                    break;
                }
                case CloudResolution.Resolution512:
                {
                    components.m_cloudLayer.resolution.value = UnityEngine.Rendering.HighDefinition.CloudResolution.CloudResolution512;
                    break;
                }
                case CloudResolution.Resolution1024:
                {
                    components.m_cloudLayer.resolution.value = UnityEngine.Rendering.HighDefinition.CloudResolution.CloudResolution1024;
                    break;
                }
                case CloudResolution.Resolution2048:
                {
                    components.m_cloudLayer.resolution.value = UnityEngine.Rendering.HighDefinition.CloudResolution.CloudResolution2048;
                    break;
                }
                case CloudResolution.Resolution4096:
                {
                    components.m_cloudLayer.resolution.value = UnityEngine.Rendering.HighDefinition.CloudResolution.CloudResolution4096;
                    break;
                }
                case CloudResolution.Resolution8192:
                {
                    components.m_cloudLayer.resolution.value = UnityEngine.Rendering.HighDefinition.CloudResolution.CloudResolution8192;
                    break;
                }
            }
            components.m_cloudLayer.opacity.value = m_cloudOpacity.Evaluate(time);
            components.m_cloudLayer.layerA.castShadows.value = m_useCloudShadows;
            components.m_cloudLayer.layerA.lighting.value = m_cloudLighting;
            components.m_cloudLayer.layerB.castShadows.value = m_useCloudShadows;
            components.m_cloudLayer.layerB.lighting.value = m_cloudLighting;
            components.m_cloudLayer.layerA.tint.value = m_cloudTintColor.Evaluate(time);
            components.m_cloudLayer.layerB.tint.value = m_cloudTintColor.Evaluate(time);
            components.m_cloudLayer.layerA.exposure.value = m_cloudExposure.Evaluate(time);
            components.m_cloudLayer.layerB.exposure.value = m_cloudExposure.Evaluate(time);
            components.m_cloudLayer.shadowMultiplier.value = m_cloudShadowOpacity.Evaluate(time);
            components.m_cloudLayer.shadowTint.value = m_cloudShadowColor.Evaluate(time);
            switch (m_cloudLayerAChannel)
            {
                case CloudLayerChannelMode.R:
                {
                    components.m_cloudLayer.layerA.opacityR.value = m_cloudLayerAOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerA.opacityG.value = 0f;
                    components.m_cloudLayer.layerA.opacityB.value = 0f;
                    components.m_cloudLayer.layerA.opacityA.value = 0f;
                    break;
                }
                case CloudLayerChannelMode.G:
                {
                    components.m_cloudLayer.layerA.opacityG.value = m_cloudLayerAOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerA.opacityR.value = 0f;
                    components.m_cloudLayer.layerA.opacityB.value = 0f;
                    components.m_cloudLayer.layerA.opacityA.value = 0f;
                    break;
                }
                case CloudLayerChannelMode.B:
                {
                    components.m_cloudLayer.layerA.opacityB.value = m_cloudLayerAOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerA.opacityR.value = 0f;
                    components.m_cloudLayer.layerA.opacityG.value = 0f;
                    components.m_cloudLayer.layerA.opacityA.value = 0f;
                    break;
                }
                case CloudLayerChannelMode.A:
                {
                    components.m_cloudLayer.layerA.opacityA.value = m_cloudLayerAOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerA.opacityR.value = 0f;
                    components.m_cloudLayer.layerA.opacityG.value = 0f;
                    components.m_cloudLayer.layerA.opacityB.value = 0f;
                    break;
                }
            }
            switch (m_cloudLayerBChannel)
            {
                case CloudLayerChannelMode.R:
                {
                    components.m_cloudLayer.layerB.opacityR.value = m_cloudLayerBOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerB.opacityG.value = 0f;
                    components.m_cloudLayer.layerB.opacityB.value = 0f;
                    components.m_cloudLayer.layerB.opacityA.value = 0f;
                    break;
                }
                case CloudLayerChannelMode.G:
                {
                    components.m_cloudLayer.layerB.opacityG.value = m_cloudLayerBOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerB.opacityR.value = 0f;
                    components.m_cloudLayer.layerB.opacityB.value = 0f;
                    components.m_cloudLayer.layerB.opacityA.value = 0f;
                    break;
                }
                case CloudLayerChannelMode.B:
                {
                    components.m_cloudLayer.layerB.opacityB.value = m_cloudLayerBOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerB.opacityR.value = 0f;
                    components.m_cloudLayer.layerB.opacityG.value = 0f;
                    components.m_cloudLayer.layerB.opacityA.value = 0f;
                    break;
                }
                case CloudLayerChannelMode.A:
                {
                    components.m_cloudLayer.layerB.opacityA.value = m_cloudLayerBOpacityR.Evaluate(time);
                    components.m_cloudLayer.layerB.opacityR.value = 0f;
                    components.m_cloudLayer.layerB.opacityG.value = 0f;
                    components.m_cloudLayer.layerB.opacityB.value = 0f;
                    break;
                }
            }
            components.m_visualEnvironment.windOrientation.value = Mathf.Clamp01(m_cloudWindDirection.Evaluate(time)) * 360f;
            components.m_visualEnvironment.windSpeed.value = m_cloudWindSpeed.Evaluate(time);

            return components.m_cloudLayer.opacity.value;

            #endregion
        }

        public bool ValidateSunLensFlare()
        {
            if (m_sunLensFlareProfile.m_lensFlareData == null)
            {
                return false;
            }
            if (m_sunLensFlareProfile.m_intensity == null)
            {
                return false;
            }
            if (m_sunLensFlareProfile.m_scale == null)
            {
                return false;
            }

            return true;
        }
        public void ApplySunLensFlare(LensFlareComponentSRP lensFlare, float time, bool isDay)
        {
            if (isDay)
            {
                lensFlare.enabled = m_sunLensFlareProfile.m_useLensFlare;
                if (m_sunLensFlareProfile.m_useLensFlare)
                {
                    lensFlare.lensFlareData = m_sunLensFlareProfile.m_lensFlareData;
                    lensFlare.intensity = m_sunLensFlareProfile.m_intensity.Evaluate(time);
                    lensFlare.scale = m_sunLensFlareProfile.m_scale.Evaluate(time);
                    lensFlare.useOcclusion = m_sunLensFlareProfile.m_enableOcclusion;
                    lensFlare.occlusionRadius = m_sunLensFlareProfile.m_occlusionRadius;
                    lensFlare.sampleCount = (uint) m_sunLensFlareProfile.m_sampleCount;
                    lensFlare.occlusionOffset = m_sunLensFlareProfile.m_occlusionOffset;
                    lensFlare.allowOffScreen = m_sunLensFlareProfile.m_allowOffScreen;
                }
            }
            else
            {
                lensFlare.enabled = false;
            }
        }

        public bool ValidateMoonLensFlare()
        {
            if (m_moonLensFlareProfile.m_lensFlareData == null)
            {
                return false;
            }
            if (m_moonLensFlareProfile.m_intensity == null)
            {
                return false;
            }
            if (m_moonLensFlareProfile.m_scale == null)
            {
                return false;
            }

            return true;
        }
        public void ApplyMoonLensFlare(LensFlareComponentSRP lensFlare, float time, bool isDay)
        {
            if (!isDay)
            {
                lensFlare.enabled = m_moonLensFlareProfile.m_useLensFlare;
                if (m_moonLensFlareProfile.m_useLensFlare)
                {
                    lensFlare.lensFlareData = m_moonLensFlareProfile.m_lensFlareData;
                    lensFlare.intensity = m_moonLensFlareProfile.m_intensity.Evaluate(time);
                    lensFlare.scale = m_moonLensFlareProfile.m_scale.Evaluate(time);
                    lensFlare.useOcclusion = m_moonLensFlareProfile.m_enableOcclusion;
                    lensFlare.occlusionRadius = m_moonLensFlareProfile.m_occlusionRadius;
                    lensFlare.sampleCount = (uint)m_moonLensFlareProfile.m_sampleCount;
                    lensFlare.occlusionOffset = m_moonLensFlareProfile.m_occlusionOffset;
                    lensFlare.allowOffScreen = m_moonLensFlareProfile.m_allowOffScreen;
                }
            }
            else
            {
                lensFlare.enabled = false;
            }
        }

        private float LerpFloat(float startingValue, float endValue, float time)
        {
            return Mathf.Lerp(startingValue, endValue, time);
        }
        private Color LerpColor(Color startingValue, Color endValue, float time)
        {
            return Color.Lerp(startingValue, endValue, time);
        }
    }
    [System.Serializable]
    public class RayTraceSettings
    {
        public bool m_rayTraceSettings = false;
        public bool m_rayTraceSSGI = false;
        public GeneralRenderMode m_ssgiRenderMode = GeneralRenderMode.Performance;
        public GeneralQuality m_ssgiQuality = GeneralQuality.High;
        public bool m_rayTraceSSR = true;
        public GeneralRenderMode m_ssrRenderMode = GeneralRenderMode.Performance;
        public GeneralQuality m_ssrQuality = GeneralQuality.High;
        public bool m_rayTraceAmbientOcclusion = false;
        public GeneralQuality m_aoQuality = GeneralQuality.High;
        public bool m_recursiveRendering = true;
        public bool m_rayTraceSubSurfaceScattering = false;
        public int m_subSurfaceScatteringSampleCount = 2;
    }
    [System.Serializable]
    public class UnderwaterOverrideData
    {
        public UnderwaterOverrideSystemType m_systemType = UnderwaterOverrideSystemType.Gaia;
        public bool m_useOverrides = true;
        public Gradient m_underwaterFogColor;
        public AnimationCurve m_underwaterFogColorMultiplier;
        public Color m_multiplyColor = Color.white;
        public bool m_useUnderwaterReverb = true;
        public UnderwaterReverbFilterSettings m_reverbFilterSettings = new UnderwaterReverbFilterSettings();

        /// <summary>
        /// Applies the settings
        /// Time of day should be from 0-1
        /// </summary>
        /// <param name="timeOfDay"></param>
        public void ApplySettings(float timeOfDay)
        {
            if (m_systemType == UnderwaterOverrideSystemType.Gaia)
            {
#if GAIA_PRO_PRESENT
                Gaia.GaiaUnderwaterEffects underwater = Gaia.GaiaUnderwaterEffects.Instance;
                if (underwater != null)
                {
                    underwater.m_timeOfDayValue = timeOfDay;
                    underwater.m_overrideFogColor = m_useOverrides;
                    underwater.m_overrideFogColorGradient = m_underwaterFogColor;
                    underwater.m_overrideFogColorMultiplier = m_underwaterFogColorMultiplier;
                    underwater.m_overrideFogMultiplier = m_multiplyColor;
                    underwater.m_useOverrideReverb = m_useUnderwaterReverb;
                    underwater.m_overrideReverbSettings = m_reverbFilterSettings;
                }
#endif
            }
            else
            {
                //Add your custom code here
            }
        }
    }
    [System.Serializable]
    public class UnderwaterReverbFilterSettings
    {
        public float m_dryLevel = -2500f;
        public float m_room = -1000f;
        public float m_roomHF = -4000f;
        public float m_roomLF = 0f;
        public float m_decayTime = 1.49f;
        public float m_decayHFRatio = 0.1f;
        public float m_reflectionsLevel = -449f;
        public float m_reflectionsDelay = 0f;
        public float m_reverbLevel = 1700f;
        public float m_reverbDelay = 0.011f;
        public float m_hFReference = 5000f;
        public float m_lFReference = 250f;
        public float m_diffusion = 100f;
        public float m_density = 100f;

        /// <summary>
        /// Applies the reverb settings to the filter
        /// Will add one if the filter is null
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="camera"></param>
        /// <param name="destroy"></param>
        /// <returns></returns>
        public AudioReverbFilter ApplyReverb(AudioReverbFilter filter, Camera camera, bool underwater)
        {
            if (filter == null && camera != null)
            {
                filter = camera.gameObject.GetComponent<AudioReverbFilter>();
                if (underwater)
                {
                    if (filter == null)
                    {
                        filter = camera.gameObject.AddComponent<AudioReverbFilter>();
                    }
                }
                else
                {
                    if (filter != null)
                    {
                        GameObject.DestroyImmediate(filter);
                        return null;
                    }
                }
            }

            if (filter != null)
            {
                filter.reverbPreset = AudioReverbPreset.User;
                //Settings
                filter.dryLevel = m_dryLevel;
                filter.room = m_room;
                filter.roomHF = m_roomHF;
                filter.roomLF = m_roomLF;
                filter.decayTime = m_decayTime;
                filter.decayHFRatio = m_decayHFRatio;
                filter.reflectionsLevel = m_reflectionsLevel;
                filter.reflectionsDelay = m_reflectionsDelay;
                filter.reverbLevel = m_reverbLevel;
                filter.reverbDelay = m_reverbDelay;
                filter.hfReference = m_hFReference;
                filter.lfReference = m_lFReference;
                filter.diffusion = m_diffusion;
                filter.density = m_density;
            }

            return filter;
        }
        /// <summary>
        /// Applies the reverb settings to the filter
        /// Will add one if the filter is null
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="camera"></param>
        /// <param name="destroy"></param>
        /// <returns></returns>
        public AudioReverbFilter ApplyReverb(AudioReverbFilter filter, Transform camera, bool underwater)
        {
            if (filter == null && camera != null)
            {
                filter = camera.gameObject.GetComponent<AudioReverbFilter>();
                if (underwater)
                {
                    if (filter == null)
                    {
                        filter = camera.gameObject.AddComponent<AudioReverbFilter>();
                    }
                }
                else
                {
                    if (filter != null)
                    {
                        GameObject.DestroyImmediate(filter);
                        return null;
                    }
                }
            }

            if (filter != null)
            {
                filter.reverbPreset = AudioReverbPreset.User;
                //Settings
                filter.dryLevel = m_dryLevel;
                filter.room = m_room;
                filter.roomHF = m_roomHF;
                filter.roomLF = m_roomLF;
                filter.decayTime = m_decayTime;
                filter.decayHFRatio = m_decayHFRatio;
                filter.reflectionsLevel = m_reflectionsLevel;
                filter.reflectionsDelay = m_reflectionsDelay;
                filter.reverbLevel = m_reverbLevel;
                filter.reverbDelay = m_reverbDelay;
                filter.hfReference = m_hFReference;
                filter.lfReference = m_lFReference;
                filter.diffusion = m_diffusion;
                filter.density = m_density;
            }

            return filter;
        }
    }

    public class HDRPTimeOfDayProfile : ScriptableObject
    {
        public TimeOfDayProfileData TimeOfDayData
        {
            get { return m_timeOfDayData; }
            set
            {
                if (m_timeOfDayData != value)
                {
                    m_timeOfDayData = value;
                }
            }
        }
        [SerializeField] private TimeOfDayProfileData m_timeOfDayData = new TimeOfDayProfileData();

        public UnderwaterOverrideData UnderwaterOverrideData
        {
            get { return m_underwaterOverrideData; }
            set
            {
                m_underwaterOverrideData = value;
            }
        }
        [SerializeField] private UnderwaterOverrideData m_underwaterOverrideData = new UnderwaterOverrideData();

        public RayTraceSettings RayTracingSettings
        {
            get { return m_rayTracingSettings; }
            set
            {
                if (m_rayTracingSettings != value)
                {
                    m_rayTracingSettings = value;
                }
            }
        }
        [SerializeField] private RayTraceSettings m_rayTracingSettings = new RayTraceSettings();
    }
}
#endif