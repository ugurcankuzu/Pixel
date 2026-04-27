using System;
using System.Collections.Generic;

namespace PixelV0.Modules.Social.Entity
{
    // ─── Interfaces ───────────────────────────────

    public interface IPresetTab
    {
        string TabKey { get; }
        string TabName { get; }
        void Reset();
    }

    public interface IBasicTab : IPresetTab
    {
        int? Exposure { get; set; }      // -100 .. 100
        int? Contrast { get; set; }      // -100 .. 100
        int? Brightness { get; set; }    // -100 .. 100
        int? Saturation { get; set; }    // -100 .. 100
        int? Sharpness { get; set; }     //    0 .. 100
        int? WhiteBalance { get; set; }  // 2000 .. 12000 (Kelvin)
        int? Tint { get; set; }          // -100 .. 100
    }

    public interface IToneTab : IPresetTab
    {
        int? Highlights { get; set; }       // -100 .. 100
        int? Shadows { get; set; }          // -100 .. 100
        int? Whites { get; set; }           // -100 .. 100
        int? Blacks { get; set; }           // -100 .. 100
        int? MidtoneContrast { get; set; }  // -100 .. 100
    }

    public interface IColorTab : IPresetTab
    {
        Dictionary<string, HslChannel?> Channels { get; set; }
    }

    public interface ILensTab : IPresetTab
    {
        int? FocalLength { get; set; }          //   10 .. 300
        int? Aperture { get; set; }             //    1 .. 220  (f × 10)
        int? FocusDistance { get; set; }        //    1 .. 1000 (unit × 10)
        bool? DofEnabled { get; set; }
        int? VignetteIntensity { get; set; }    // -100 .. 100
        int? VignetteSmoothness { get; set; }   //    0 .. 100
        int? ChromaticAberration { get; set; }  //    0 .. 100
        int? LensDistortion { get; set; }       // -100 .. 100
        int? LensFlare { get; set; }            //    0 .. 100
    }

    public interface IEffectsTab : IPresetTab
    {
        int? BloomIntensity { get; set; }   //   0 .. 100
        int? BloomThreshold { get; set; }   //   0 .. 100
        int? GrainIntensity { get; set; }   //   0 .. 100
        int? GrainSize { get; set; }        //   1 .. 50
        bool? GrainColored { get; set; }
        Guid? LutId { get; set; }
        int? LutContribution { get; set; }  //   0 .. 100
    }

    public interface ICamera
    {
        int? FOV { get; set; }              //  10 .. 170
        int? CameraRotation { get; set; }   //   0 .. 359
        bool? HandheldShake { get; set; }
        int? ShakeIntensity { get; set; }   //   0 .. 100
    }

    public interface IFrameTab : IPresetTab
    {
        string? OverlayId { get; set; }
        int? OverlayOpacity { get; set; }  //   0 .. 100
        string? AspectRatio { get; set; }  // "free" | "16:9" | "1:1" | "3:2" | "21:9"
    }

    // ─── Shared Value Type ───────────────────────

    public class HslChannel
    {
        public int? Hue { get; set; }        // -180 .. 180
        public int? Saturation { get; set; } // -100 .. 100
        public int? Luminance { get; set; }  // -100 .. 100
    }

    // ─── Concrete Tab Implementations ────────────
    // DbContext'in görebilmesi için hepsi "public" yapıldı.

    public class BasicTab : IBasicTab
    {
        public string TabKey => "basic";
        public string TabName => "Basic";

        public int? Exposure { get; set; }
        public int? Contrast { get; set; }
        public int? Brightness { get; set; }
        public int? Saturation { get; set; }
        public int? Sharpness { get; set; }
        public int? WhiteBalance { get; set; }
        public int? Tint { get; set; }

        public void Reset() =>
            Exposure = Contrast = Brightness = Saturation = Sharpness = WhiteBalance = Tint = null;
    }

    public class ToneTab : IToneTab
    {
        public string TabKey => "tone";
        public string TabName => "Tone";

        public int? Highlights { get; set; }
        public int? Shadows { get; set; }
        public int? Whites { get; set; }
        public int? Blacks { get; set; }
        public int? MidtoneContrast { get; set; }

        public void Reset() =>
            Highlights = Shadows = Whites = Blacks = MidtoneContrast = null;
    }

    public class ColorTab : IColorTab
    {
        public string TabKey => "color";
        public string TabName => "Color";

        public Dictionary<string, HslChannel?> Channels { get; set; } = new()
        {
            ["Red"] = null,
            ["Orange"] = null,
            ["Yellow"] = null,
            ["Green"] = null,
            ["Aqua"] = null,
            ["Blue"] = null,
            ["Purple"] = null,
            ["Magenta"] = null,
        };

        public void Reset()
        {
            foreach (var key in Channels.Keys)
                Channels[key] = null;
        }
    }

    public class LensTab : ILensTab
    {
        public string TabKey => "lens";
        public string TabName => "Lens";

        public int? FocalLength { get; set; }
        public int? Aperture { get; set; }
        public int? FocusDistance { get; set; }
        public bool? DofEnabled { get; set; }
        public int? VignetteIntensity { get; set; }
        public int? VignetteSmoothness { get; set; }
        public int? ChromaticAberration { get; set; }
        public int? LensDistortion { get; set; }
        public int? LensFlare { get; set; }

        public void Reset() =>
            FocalLength = Aperture = FocusDistance = VignetteIntensity =
            VignetteSmoothness = ChromaticAberration = LensDistortion = LensFlare = null;
    }

    public class EffectsTab : IEffectsTab
    {
        public string TabKey => "effects";
        public string TabName => "Effects";

        public int? BloomIntensity { get; set; }
        public int? BloomThreshold { get; set; }
        public int? GrainIntensity { get; set; }
        public int? GrainSize { get; set; }
        public bool? GrainColored { get; set; }
        public Guid? LutId { get; set; }
        public int? LutContribution { get; set; }

        public void Reset() =>
            BloomIntensity = BloomThreshold = GrainIntensity = GrainSize = LutContribution = null;
    }

    public class CameraSettings : ICamera
    {
        public int? FOV { get; set; }
        public int? CameraRotation { get; set; }
        public bool? HandheldShake { get; set; }
        public int? ShakeIntensity { get; set; }
    }

    public class FrameTab : IFrameTab
    {
        public string TabKey => "frame";
        public string TabName => "Frame";

        public string? OverlayId { get; set; }
        public int? OverlayOpacity { get; set; }
        public string? AspectRatio { get; set; }

        public void Reset() =>
            OverlayId = AspectRatio = null;
    }

    // ─── Value Object (JSON kolonuna yazılacak kısım) ───

    public class PresetValues
    {
        public BasicTab Basic { get; set; } = new();
        public ToneTab Tone { get; set; } = new();
        public ColorTab Color { get; set; } = new();
        public LensTab Lens { get; set; } = new();
        public EffectsTab Effects { get; set; } = new();
        public FrameTab Frame { get; set; } = new();
        public CameraSettings Camera { get; set; } = new();

        public IReadOnlyList<IPresetTab> GetTabs() =>
            [Basic, Tone, Color, Lens, Effects, Frame];

        public void ResetAll()
        {
            foreach (var tab in GetTabs())
                tab.Reset();

            Camera = new CameraSettings();
        }
    }

    // ─── Main Database Entity ─────────────────────────

    public class Preset
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? User_ID { get; set; }
        public Guid? Game_ID { get; set; }
        public string Name { get; set; } = "New Preset";
        public int Version { get; set; } = 1;

        // Tüm detaylı ayarları tutan ve DB'ye JSON olarak gidecek nesne
        public PresetValues Values { get; set; } = new();

        public void ResetAll()
        {
            Values.ResetAll();
            Version++;
        }
    }
}