using System.Windows;
using System.Windows.Controls;
using FFXIV.Framework.Common;

namespace FFXIV.Framework.Extensions
{
    public static class FontExtensions
    {
        public static FontInfo GetFontInfo(
            this Control control)
        {
            return new FontInfo(
                control.FontFamily,
                control.FontSize,
                control.FontStyle,
                control.FontWeight,
                control.FontStretch);
        }

        public static void SetFontInfo(
            this Control control,
            FontInfo fontInfo)
        {
            if (control.GetFontInfo().ToString() != fontInfo.ToString())
            {
                control.FontFamily = fontInfo.Family;
                control.FontSize = fontInfo.Size;
                control.FontStyle = fontInfo.Style;
                control.FontWeight = fontInfo.Weight;
                control.FontStretch = fontInfo.Stretch;
            }
        }

        public static FontInfo GetFontInfo(
            this TextBlock control)
        {
            return new FontInfo(
                control.FontFamily,
                control.FontSize,
                control.FontStyle,
                control.FontWeight,
                control.FontStretch);
        }

        public static void SetFontInfo(
            this TextBlock control,
            FontInfo fontInfo)
        {
            if (control.GetFontInfo().ToString() != fontInfo.ToString())
            {
                control.FontFamily = fontInfo.Family;
                control.FontSize = fontInfo.Size;
                control.FontStyle = fontInfo.Style;
                control.FontWeight = fontInfo.Weight;
                control.FontStretch = fontInfo.Stretch;
            }
        }

        /// <summary>
        /// WPF向けFontFamilyに変換する
        /// </summary>
        /// <param name="font">Font</param>
        /// <returns>FontFamily</returns>
        public static System.Windows.Media.FontFamily ToFontFamilyWPF(
            this System.Drawing.Font font)
        {
            return new System.Windows.Media.FontFamily(font.Name);
        }

        /// <summary>
        /// FontInfoに変換する
        /// </summary>
        /// <param name="font">Font</param>
        /// <returns>FontInfo</returns>
        public static FontInfo ToFontInfo(
            this System.Drawing.Font font)
        {
            if (font == null)
            {
                return null;
            }

            var fi = new FontInfo()
            {
                Family = font.ToFontFamilyWPF(),
                Size = font.Size,
                Style = font.ToFontStyleWPF(),
                Weight = font.ToFontWeightWPF(),
                Stretch = System.Windows.FontStretches.Normal
            };

            return fi;
        }

        /// <summary>
        /// WPF向けFontStyleに変換する
        /// </summary>
        /// <param name="font">Font</param>
        /// <returns>FontStyle</returns>
        public static System.Windows.FontStyle ToFontStyleWPF(
            this System.Drawing.Font font)
        {
            return (font.Style & System.Drawing.FontStyle.Italic) != 0 ? FontStyles.Italic : FontStyles.Normal;
        }

        /// <summary>
        /// WPF向けFontWeightに変換する
        /// </summary>
        /// <param name="font">Font</param>
        /// <returns>FontWeight</returns>
        public static System.Windows.FontWeight ToFontWeightWPF(
            this System.Drawing.Font font)
        {
            return (font.Style & System.Drawing.FontStyle.Bold) != 0 ? FontWeights.Bold : FontWeights.Normal;
        }
    }
}