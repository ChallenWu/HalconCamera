using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;


namespace HalconCamera
{
    public class H_Function
    {
        //--- Common
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem, HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_MaxAscent = null;
            HTuple hv_MaxDescent = null, hv_MaxWidth = null, hv_MaxHeight = null;
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRow = new HTuple();
            HTuple hv_FactorColumn = new HTuple(), hv_UseShadow = null;
            HTuple hv_ShadowColor = null, hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_oldColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();
            try
            {
                HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);

                HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part, out hv_Column2Part);
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                    out hv_WidthWin, out hv_HeightWin);
               // HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
                //default settings
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
                {
                    hv_Color_COPY_INP_TMP = "";
                }
                //
                hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
                //
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                    out hv_MaxWidth, out hv_MaxHeight);
                if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
                {
                    hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                    hv_C1 = hv_Column_COPY_INP_TMP.Clone();
                }
                else
                {
                    //Transform image to window coordinates
                    hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                    hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                    hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                    hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
                }
                //
                //Display text box depending on text size
                hv_UseShadow = 1;
                hv_ShadowColor = "gray";
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
                {
                    if (hv_Box_COPY_INP_TMP == null)
                        hv_Box_COPY_INP_TMP = new HTuple();
                    hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                    hv_ShadowColor = "#f28d26";
                }
                if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                    1))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                    {
                        //Use default ShadowColor set above
                    }
                    else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                        "false"))) != 0)
                    {
                        hv_UseShadow = 0;
                    }
                    else
                    {
                        hv_ShadowColor = hv_Box_COPY_INP_TMP[1];
                        //Valid color?
                        try
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                                1));
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            hv_Exception = "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)";
                            throw new HalconException(hv_Exception);
                        }
                    }
                }
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
                {
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)";
                        throw new HalconException(hv_Exception);
                    }
                    //Calculate box extents
                    hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                    hv_Width = new HTuple();
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                            hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                        hv_Width = hv_Width.TupleConcat(hv_W);
                    }
                    hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        ));
                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                    hv_R2 = hv_R1 + hv_FrameHeight;
                    hv_C2 = hv_C1 + hv_FrameWidth;
                    //Display rectangles
                    HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                    HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                    //Set shadow color
                    HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                    if ((int)(hv_UseShadow) != 0)
                    {
                        HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1, hv_C2 + 1);
                    }
                    //Set box color
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                    HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                }
                //Write text.
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_oldColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                        )));
                    if ((int)((new HTuple(hv_oldColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_oldColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_oldColor);
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                    HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                    HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index));
                }
                //Reset changed window settings
                HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                // HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,hv_Column2Part);



                //HTuple hv_Widthh, hv_Heightt;
                //HOperatorSet.GetImageSize(HObject_Image, out hv_Widthh, out hv_Heightt);
                //HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Heightt, hv_Widthh);
                return;
            }
            catch
            {
                return;
            }

        }
        public void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold, HTuple hv_Slant)
        {

            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            HOperatorSet.GetSystem("operating_system", out hv_OS);

            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }

            string OsType = hv_OS.ToString().Substring(1, 3);
            if (OsType == "Win")
            {
                //Set font on Windows systems
                try
                {
                    //Check, if font scaling is switched on
                    HOperatorSet.OpenWindow(0, 0, 256, 256, 0, "buffer", "", out hv_BufferWindowHandle);
                    HOperatorSet.SetFont(hv_BufferWindowHandle, "-Consolas-16-*-0-*-*-1-");
                    HOperatorSet.GetStringExtents(hv_BufferWindowHandle, "test_string", out hv_Ascent,
                        out hv_Descent, out hv_Width, out hv_Height);
                    //Expected width is 110
                    hv_Scale = 110.0 / hv_Width;
                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
                    HOperatorSet.CloseWindow(hv_BufferWindowHandle);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Consolas";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else if (OsType == "Dar")
            {

                hv_SubFamily = 0;
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(1);
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(2);
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Menlo-Regular";
                    hv_Fonts[1] = "Menlo-Italic";
                    hv_Fonts[2] = "Menlo-Bold";
                    hv_Fonts[3] = "Menlo-BoldItalic";
                }
                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "CourierNewPSMT";
                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
                    hv_Fonts[2] = "CourierNewPS-BoldMT";
                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "ArialMT";
                    hv_Fonts[1] = "Arial-ItalicMT";
                    hv_Fonts[2] = "Arial-BoldMT";
                    hv_Fonts[3] = "Arial-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "TimesNewRomanPSMT";
                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
                }
                else
                {
                    //Attempt to figure out which of the fonts installed on the system
                    //the user could have meant.
                    HOperatorSet.QueryFont(hv_WindowHandle, out hv_SystemFonts);
                    hv_Fonts = new HTuple();
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                }
                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else
            {
                //Set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(((hv_OS.TupleSub(4))).TupleEqual("Linux"))).TupleAnd(
                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                    {
                        HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
                            hv_FontSelRegexp);
                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
                            0))) != 0)
                        {
                            hv_Exception = "Wrong font name";
                            //throw (Exception)
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetFont(hv_WindowHandle, (((hv_FontsCourier.TupleSelect(
                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException2)
                            {
                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                //throw (Exception)
                            }
                        }
                    }
                    //throw (Exception)
                }
            }
            // dev_set_preferences(...); only in hdevelop

            return;
        }
        public void SaveImage(string eleName, HObject Ho_image)
        {
            string ImagePath = @".\Image\";
            if (!Directory.Exists(ImagePath))
                Directory.CreateDirectory(ImagePath);
            ImagePath += eleName + ".bmp";
            HOperatorSet.WriteImage(Ho_image, "bmp", 0, ImagePath);
        }
        public HObject Load_Display_Image(string path, HTuple windows)
        {
            HObject ho_Image;
            HTuple hv_Width, hv_Height;
            HOperatorSet.GenEmptyObj(out ho_Image);
            try
            {
                HOperatorSet.ReadImage(out ho_Image, path);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(windows, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(ho_Image, windows);
            }
            catch
            {
                return null;
            }
            return ho_Image;
        }
        public HObject Load_HObject_Image(string path)
        {

            try
            {
                HObject ho_Image = new HObject();
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.ReadImage(out ho_Image, path);
                return ho_Image;
            }
            catch
            {
                return null;
            }
        }
        public HImage Load_HImage(string path)
        {
            try
            {
                HImage ho_Image = new HImage();
                ho_Image.ReadImage(path);
                return ho_Image;
            }
            catch
            {
                return null;
            }
        }
        public bool Matching_DisplayResult(HObject ImgInput, HObject ImgTemplete, Parameter.Roi Roi, HWindowControl hWindowControl)
        {
            try
            {
                hWindowControl.HalconWindow.SetLineWidth(1);// width of ROI
                // Local iconic variables 
                HObject ho_Image, ho_TemplateImage;
                HObject ho_ModelContours, ho_TransContours;
                // Local control variables 
                HTuple hv_ModelID = new HTuple(), hv_ModelRegionArea = new HTuple();
                HTuple hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
                HTuple hv_HomMat2D = new HTuple(), hv_Row = new HTuple();
                HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
                HTuple hv_Score = new HTuple(), hv_I = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_TemplateImage);
                HOperatorSet.GenEmptyObj(out ho_ModelContours);
                HOperatorSet.GenEmptyObj(out ho_TransContours);
                //Matching 01: ************************************************
                ho_Image.Dispose();
                /// reduce image in ROI
                HObject ho_Roi;
                HObject ho_RoiStand;
                HOperatorSet.GenEmptyObj(out ho_Roi);
                HOperatorSet.GenEmptyObj(out ho_RoiStand);
                HOperatorSet.GenRectangle1(out ho_Roi, Roi.row1, Roi.col1, Roi.row2, Roi.col2);
                HOperatorSet.GenRectangle1(out ho_RoiStand, Roi.row1stand, Roi.col1stand, Roi.row2stand, Roi.col2stand);
                HOperatorSet.ReduceDomain(ImgInput, ho_Roi, out ho_Image);
                //Matching 01: Reduce the model template
                ho_TemplateImage.Dispose();
                ho_TemplateImage = ImgTemplete;
                //Matching 01: Create the shape model
                hv_ModelID.Dispose();
                HOperatorSet.CreateShapeModel(ho_TemplateImage, 1, (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), (new HTuple(1.3068)).TupleRad(), (new HTuple("none")).TupleConcat(
                    "no_pregeneration"), "use_polarity", ((new HTuple(28)).TupleConcat(37)).TupleConcat(
                    9), 4, out hv_ModelID);
                //
                //Matching 01: Get the model contour for transforming it later into the image
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                //
                //Matching 01: Get the reference position
                hv_ModelRegionArea.Dispose(); hv_RefRow.Dispose(); hv_RefColumn.Dispose();
                HOperatorSet.AreaCenter(ho_RoiStand, out hv_ModelRegionArea, out hv_RefRow,
                    out hv_RefColumn);
                hv_HomMat2D.Dispose();
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_RefRow, hv_RefColumn, 0, out hv_HomMat2D);
                ho_TransContours.Dispose();
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                //
                //Matching 01: Find the model
                hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_Score.Dispose();
                HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, (new HTuple(0)).TupleRad(),
                    (new HTuple(360)).TupleRad(), 0.8, 1, 0, "least_squares", (new HTuple(5)).TupleConcat(
                    1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                //
                //Matching 01: Transform the model contours into the detected positions
                // HOperatorSet.DispObj(ho_Image, hWindowControl.HalconWindow);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_HomMat2D.Dispose();
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0,
                            out ExpTmpOutVar_0);
                        hv_HomMat2D.Dispose();
                        hv_HomMat2D = ExpTmpOutVar_0;
                    }
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I), hv_Column.TupleSelect(
                            hv_I), out ExpTmpOutVar_0);
                        hv_HomMat2D.Dispose();
                        hv_HomMat2D = ExpTmpOutVar_0;
                    }
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours,
                        hv_HomMat2D);

                    HOperatorSet.SetColor(hWindowControl.HalconWindow, "green");
                    HOperatorSet.DispObj(ho_TransContours, hWindowControl.HalconWindow);

                }

                disp_message(hWindowControl.HalconWindow, Roi.component, "window", hv_Row, hv_Column, "green", "false");
                // display Roi&RoiStand old
                HOperatorSet.SetColor(hWindowControl.HalconWindow, "red");
                HObject XLD_Roi;
                HObject XLD_RoiStand;
                HOperatorSet.GenEmptyObj(out XLD_Roi);
                HOperatorSet.GenEmptyObj(out XLD_RoiStand);
                XLD_Roi = Create_XLDObject_FromRoi(Roi);
                XLD_RoiStand = Create_XLDObject_FromRoiStand(Roi);
                HOperatorSet.DispXld(XLD_Roi, hWindowControl.HalconWindow);
                HOperatorSet.DispXld(XLD_RoiStand, hWindowControl.HalconWindow);
                XLD_Roi.Dispose();
                XLD_RoiStand.Dispose();
                //
                //Matching 01: *******************************************
                //Matching 01: END of generated code for model application
                //Matching 01: *******************************************
                //
                ho_Image.Dispose();
                ho_Roi.Dispose();
                ho_RoiStand.Dispose();
                ho_TemplateImage.Dispose();
                ho_ModelContours.Dispose();
                ho_TransContours.Dispose();

                hv_ModelID.Dispose();
                hv_ModelRegionArea.Dispose();
                hv_RefRow.Dispose();
                hv_RefColumn.Dispose();
                hv_HomMat2D.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_Score.Dispose();
                hv_I.Dispose();
                if (hv_Score.TupleLength() > 0) return true;
                else return false;
            }
            catch 
            {
                return false;
            }
        }
        public HObject GenRectangle1_FromRoi(Parameter.Roi Roi)
        {
            try
            {
                HObject ho_RectangeROI;
                HOperatorSet.GenEmptyObj(out ho_RectangeROI);
                HOperatorSet.GenRectangle1(out ho_RectangeROI, Roi.row1, Roi.col1, Roi.row2, Roi.col2);
                return ho_RectangeROI;
            }
            catch
            {
                return null;
            }
        }
        public HObject GenRectangle1_FromhDrawRoi(HDrawingObject Draw)
        {
            try
            {
                HObject ho_RectangeROI;
                HOperatorSet.GenEmptyObj(out ho_RectangeROI);
                HTuple paramName = new HTuple();
                string[] paramlist = { "row1", "column1", "row2", "column2" };
                paramName = paramlist;
                HTuple para = Draw.GetDrawingObjectParams(paramName);
                HOperatorSet.GenRectangle1(out ho_RectangeROI, (double)para.LArr[0], (double)para.LArr[1], (double)para.LArr[2], (double)para.LArr[3]);
                return ho_RectangeROI;
            }
            catch
            {
                return null;
            }
        }
        public HObject GenCycle_FromhDrawRoi(HDrawingObject Draw)
        {
            try
            {
                HObject ho_RectangeROI;
                HOperatorSet.GenEmptyObj(out ho_RectangeROI);
                HTuple paramName = new HTuple();
                string[] paramlist = { "row", "column", "radius" };
                paramName = paramlist;
                HTuple para = Draw.GetDrawingObjectParams(paramName);
                HOperatorSet.GenCircle(out ho_RectangeROI, (double)para.DArr[0], (double)para.DArr[1], (double)para.DArr[2]);
                return ho_RectangeROI;
            }
            catch
            {
                return null;
            }
        }

        #region Lấy tọa độ ROI
        public Parameter.Roi GenROI_FromhDrawRoi(HDrawingObject Draw)
        {
            try
            {
                if (Draw != null)
                {
                    HObject ho_RectangeROI;
                    HOperatorSet.GenEmptyObj(out ho_RectangeROI);
                    HTuple paramName = new HTuple();
                    //Lấy các giá trị tương ứng từ đối tượng vẽ.
                    string[] paramlist = { "row1", "column1", "row2", "column2" };
                    paramName = paramlist;
                    HTuple para = Draw.GetDrawingObjectParams(paramName);
                    var roi = new Parameter.Roi();
                    roi.row1 = (double)para.LArr[0];    
                    roi.col1 = (double)para.LArr[1];
                    roi.row2 = (double)para.LArr[2];
                    roi.col2 = (double)para.LArr[3];
                    return roi;
                }
                else
                    return null;

            }
            catch
            {
                return null;
            }
        }
        #endregion

        

        #region Vẽ Object
        public HObject Create_XLDObject_FromRoi(Parameter.Roi Roi)
        {
            try
            {
                HObject ho_ROI_0;
                HOperatorSet.GenEmptyObj(out ho_ROI_0);
                HOperatorSet.GenContourPolygonXld(out ho_ROI_0,
                                                 new HTuple(Roi.row1).TupleConcat( Roi.row1).TupleConcat(Roi.row2).TupleConcat(Roi.row2).TupleConcat(Roi.row1),
                                                 new HTuple(Roi.col1).TupleConcat(Roi.col2).TupleConcat(Roi.col2).TupleConcat( Roi.col1).TupleConcat(Roi.col1)
                                                 );
                return ho_ROI_0;
            }
            catch
            {
                return null;
            }
        }
        public HObject Create_XLDObject_FromRoiStand(Parameter.Roi Roi)
        {
            try
            {
                HObject ho_ROI_0;
                HOperatorSet.GenEmptyObj(out ho_ROI_0);
                HOperatorSet.GenContourPolygonXld(out ho_ROI_0, ((((new HTuple(Roi.row1stand)).TupleConcat(
                     Roi.row1stand)).TupleConcat(Roi.row2stand)).TupleConcat(Roi.row2stand)).TupleConcat(Roi.row1stand),
                     ((((new HTuple(Roi.col1stand)).TupleConcat(Roi.col2stand)).TupleConcat(Roi.col2stand)).TupleConcat(
                     Roi.col1stand)).TupleConcat(Roi.col1stand));
                return ho_ROI_0;
            }
            catch
            {
                return null;
            }
        }

        public HObject Draw_Polygon(double Px1, double Py1, double Px2, double Py2, double Px3, double Py3, double Px4, double Py4)
        {
            try
            {
                HObject ho_ROI_0;
                HOperatorSet.GenEmptyObj(out ho_ROI_0);
                HOperatorSet.GenContourPolygonXld(out ho_ROI_0,
                     new HTuple(Py1).TupleConcat(Py2).TupleConcat(Py3).TupleConcat(Py4).TupleConcat(Py1),
                    new HTuple(Px1).TupleConcat(Px2).TupleConcat(Px3).TupleConcat(Px4).TupleConcat(Px1));
                return ho_ROI_0;
            }
            catch
            {
                return null;
            }
        }
        public HObject Create_XLDObject_FromRoiStandPhi(Parameter.Roi Roi)
        {
            try
            {
                HObject ho_ROI_0;
                HOperatorSet.GenEmptyObj(out ho_ROI_0);
                HOperatorSet.GenContourPolygonXld(out ho_ROI_0, ((((new HTuple(Roi.row1stand)).TupleConcat(
                     Roi.row1stand)).TupleConcat(Roi.row2stand)).TupleConcat(Roi.row2stand)).TupleConcat(Roi.row1stand),
                     ((((new HTuple(Roi.col1stand)).TupleConcat(Roi.col2stand)).TupleConcat(Roi.col2stand)).TupleConcat(
                     Roi.col1stand)).TupleConcat(Roi.col1stand));

                //HOperatorSet.GenRectangle2ContourXld(out HObject rectangle, new HTuple(Roi.row1stand), new HTuple(Roi.col1stand), new HTuple(Roi.a), HTuple length1, HTuple length2)

                return ho_ROI_0;
            }
            catch
            {
                return null;
            }
        }
        public HObject Create_XLDCicle_FromRoi(double row,double col,double r)
        {
            try
            {
                HOperatorSet.GenCircleContourXld(out HObject circleXLD, row, col, r, 0, 6.28318, "positive", 1.0);

                return circleXLD;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        public HObject Threshold_Image(HObject img,int value)
        {
            try
            {
                HObject ho_Image = img.CopyObj(1, -1);
                HObject ho_Region;
                HOperatorSet.GenEmptyObj(out ho_Region);
                ho_Region.Dispose();

                HOperatorSet.Threshold(ho_Image, out ho_Region, 0, value);
                ho_Image.Dispose();
                return ho_Region;
            }
            catch 
            {
                return null;
            }
        }
        
        public HObject ReduceImageRect(HObject image,Parameter.Roi Roi)
        {
            try
            {
                HOperatorSet.GenRectangle1(out HObject ho_Roi, Roi.row1, Roi.col1, Roi.row2, Roi.col2);
                HOperatorSet.ReduceDomain(image.CopyObj(1,-1), ho_Roi, out HObject image_reduce);
                return image_reduce;
            }
            catch { return null; }
        }
        public HObject ReduceImageCycle(HObject image, Parameter.Roi Roi)
        {
            try
            {
                HOperatorSet.GenCircle(out HObject ho_Roi, Roi.row1, Roi.col1, Roi.radius);                
                HOperatorSet.ReduceDomain(image.CopyObj(1, -1), ho_Roi, out HObject image_reduce);
                return image_reduce;
            }
            catch { return null; }
        }

        #region Scan barcode
        //--- Scan Barcode
        public string GetBacode1D(HTuple Window, HObject Image, Parameter.Roi RoiScan)
        {
            try
            {
                HObject ho_Image0 = null;
                HOperatorSet.GenEmptyObj(out ho_Image0);
                ho_Image0.Dispose();
                HObject ho_Roi;
                HOperatorSet.GenEmptyObj(out ho_Roi);
                HOperatorSet.GenRectangle1(out ho_Roi, RoiScan.row1, RoiScan.col1, RoiScan.row2, RoiScan.col2);
                HOperatorSet.ReduceDomain(Image, ho_Roi, out ho_Image0);

                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                HOperatorSet.GetImageSize(ho_Image0, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(Image, Window);

                HTuple hv_BarCodeHandle = new HTuple(), hv_DecodedDataStrings = new HTuple();
                // Initialize local and output iconic variables 
                hv_BarCodeHandle.Dispose();
                HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out hv_BarCodeHandle);
                HObject ho_SymbolRegions; hv_DecodedDataStrings.Dispose();
                HOperatorSet.FindBarCode(ho_Image0, out ho_SymbolRegions, hv_BarCodeHandle, "auto", out hv_DecodedDataStrings);
                ho_Image0.Dispose();
                hv_BarCodeHandle.Dispose();
                if (hv_DecodedDataStrings.ToString().Length > 5)
                {
                    HOperatorSet.SetLineWidth(Window, 5);
                    HOperatorSet.SetDraw(Window, "margin");
                    HOperatorSet.SetColor(Window, "green");
                    HOperatorSet.DispObj(ho_SymbolRegions, Window);
                    disp_message(Window, "Data Code:\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" + hv_DecodedDataStrings.SArr[0].ToString(), "window", 40, 12, "green", "false");
                    ho_SymbolRegions.Dispose();
                    hv_Width.Dispose();
                    hv_Height.Dispose();
                    ho_Image0.Dispose();
                    ho_Roi.Dispose();
                    return hv_DecodedDataStrings.SArr[0].ToString();
                }
                else
                {
                    HOperatorSet.SetLineWidth(Window, 5);
                    HOperatorSet.SetColor(Window, "red");
                    disp_message(Window, "No data code found", "window", 40, 12, "red", "false");
                    ho_SymbolRegions.Dispose();
                    hv_Width.Dispose();
                    hv_Height.Dispose();
                    ho_Image0.Dispose();
                    ho_Roi.Dispose();
                    return "";
                }
            }
            catch
            {
                return "";
            }

        }
        public string GetBacode2D(HTuple Window, HObject Image, Parameter.Roi RoiScan, CodeType type)
        {
            try
            {
                HObject ho_Image0 = null;
                HOperatorSet.GenEmptyObj(out ho_Image0);
                ho_Image0.Dispose();
                HObject ho_Roi;
                HOperatorSet.GenEmptyObj(out ho_Roi);
                HOperatorSet.GenRectangle1(out ho_Roi, RoiScan.row1, RoiScan.col1, RoiScan.row2, RoiScan.col2);
                HOperatorSet.ReduceDomain(Image, ho_Roi, out ho_Image0);

                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                HOperatorSet.GetImageSize(ho_Image0, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(Image, Window);
                HTuple hv_DecodedDataStrings = new HTuple();
                HObject ho_SymbolXLDs;
                HTuple hv_ResultHandles = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
                ho_SymbolXLDs.Dispose();
                hv_ResultHandles.Dispose();
                hv_DecodedDataStrings.Dispose();
                //string[] type = new string[] { "QR Code", "GS1 QR Code", "Micro QR Code", "Aztec Code", "Data Matrix ECC 200", "GS1 Aztec Code", "GS1 DataMatrix", "PDF417" };
                HTuple hv_DataCodeHandle = new HTuple();
                HOperatorSet.CreateDataCode2dModel(SelectCodeType(type), new HTuple(), new HTuple(), out hv_DataCodeHandle);
                HOperatorSet.FindDataCode2d(ho_Image0, out ho_SymbolXLDs, hv_DataCodeHandle, new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
                //Display the results
                if (hv_DecodedDataStrings.ToString().Length > 5)
                {
                    HOperatorSet.SetLineWidth(Window, 5);
                    HOperatorSet.SetColor(Window, "green");
                    HOperatorSet.SetDraw(Window, "margin");
                    HOperatorSet.DispObj(ho_SymbolXLDs, Window);
                    string data_code = "Data Code:" + hv_DecodedDataStrings + System.Environment.NewLine;
                    // data_code += "Type Code:\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" + ty;
                    disp_message(Window, data_code, "window", 40, 12, "green", "false");
                    ho_SymbolXLDs.Dispose();
                    hv_DataCodeHandle.Dispose();
                    hv_ResultHandles.Dispose();
                    hv_Width.Dispose();
                    hv_Height.Dispose();
                    ho_Image0.Dispose();
                    ho_Roi.Dispose();
                    return hv_DecodedDataStrings.SArr[0].ToString();
                }
                else
                {
                    HOperatorSet.SetLineWidth(Window, 5);
                    HOperatorSet.SetColor(Window, "red");
                    disp_message(Window, "No data code found", "window", 40, 12, "red", "false");
                    ho_SymbolXLDs.Dispose();
                    hv_DataCodeHandle.Dispose();
                    hv_ResultHandles.Dispose();
                    hv_DecodedDataStrings.Dispose();
                    hv_Width.Dispose();
                    hv_Height.Dispose();
                    ho_Image0.Dispose();
                    ho_Roi.Dispose();
                }
                return "";
            }
            catch
            {
                return "";
            }



        }
        public string SelectCodeType(CodeType type)
        {
            string typeString = "auto";
            switch (type)
            {
                case CodeType.Auto:
                    typeString = "auto";
                    break;
                case CodeType.Code39:
                    typeString = "Code 39";
                    break;
                case CodeType.Code93:
                    typeString = "Code 93";
                    break;
                case CodeType.Code128:
                    typeString = "Code 128";
                    break;
                case CodeType.EAN_13:
                    typeString = "EAN-13";
                    break;
                case CodeType.Codabar:
                    typeString = "Codabar";
                    break;
                case CodeType.DataMatrixECC200:
                    typeString = "Data Matrix ECC 200";
                    break;
                case CodeType.QRCode:
                    typeString = "QR Code";
                    break;
                case CodeType.MicroQRCode:
                    typeString = "Micro QR Code";
                    break;
                case CodeType.AztecCode:
                    typeString = "Aztec Code";
                    break;
                default:
                    typeString = "auto";
                    break;
            }
            return typeString;
        }
        public enum CodeType
        {
            Auto = 0,
            Code39 = 1,
            Code93 = 2,
            Code128 = 3,
            EAN_13 = 4,
            Codabar = 5,
            DataMatrixECC200 = 20,
            QRCode = 21,
            MicroQRCode = 22,
            AztecCode = 23,
        }
        #endregion

        #region Matching NCC
        //--- Matching NCC
        public void CreateModelNcc(HObject templete,string path)
        {
            HOperatorSet.Rgb1ToGray(templete, out HObject img);
            HOperatorSet.CreateNccModel( img, 5, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), (new HTuple(1)).TupleRad(), "use_polarity", out HTuple hv_ModelID);
            HOperatorSet.WriteNccModel(hv_ModelID, path);
        }
        public Parameter.ResultMatching Matching_NCC(HObject ImgInput, HTuple ModelID, Parameter.Roi Roi, HWindowControl hWindowControl)
        {
            Parameter.ResultMatching result = new Parameter.ResultMatching();
            HObject ho_Image = ImgInput.CopyObj(1, -1);
            HObject ho_ImageReduced = ReduceImageRect(ho_Image, Roi);
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_Score.Dispose();
            HOperatorSet.FindNccModel(ho_ImageReduced, ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), 0.75, 1, 0.9, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            if (hv_Score.D > 0)
            {
                dev_display_ncc_matching_results(hWindowControl,ModelID, "green", hv_Row, hv_Column, hv_Angle.D, 0);
                result.marky = hv_Row.D;
                result.markx = hv_Column.D;
                result.angle = RadianToDegrees(hv_Angle.D);
                result.score = hv_Score.D;
                string dis = Roi.component + "\r\n\r\n\r\n";
                dis += "Row: " + result.marky + "\r\n\r\n\r\n";
                dis += "Col: " + result.markx + "\r\n\r\n\r\n";
                dis += "Angle: " + result.angle + "\r\n\r\n\r\n";
                disp_message(hWindowControl.HalconWindow, dis, "window", 100, 100, "green", "false");
            }
            else
            {
                dev_display_ncc_matching_results(hWindowControl,ModelID, "red", hv_Row, hv_Column, hv_Angle, 0);
            }
            ho_Image.Dispose();
            ho_ImageReduced.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Angle.Dispose();
            hv_Score.Dispose();
            hv_Row.Dispose();
            return result;
        }
       
        public void dev_display_ncc_matching_results(HWindowControl hWindow, HTuple hv_ModelID, HTuple hv_Color,HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_Model)
        {
            HObject ho_ModelRegion = null, ho_ModelContours = null;
            HObject ho_ContoursAffinTrans = null, ho_Cross = null;

            // Local control variables 

            HTuple hv_NumMatches = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Match = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DRotate = new HTuple(), hv_HomMat2DTranslate = new HTuple();
            HTuple hv_RowTrans = new HTuple(), hv_ColTrans = new HTuple();
            HTuple hv_Model_COPY_INP_TMP = new HTuple(hv_Model);

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelRegion);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                //This procedure displays the results of Correlation-Based Matching.
                //
                hv_NumMatches.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_NumMatches = new HTuple(hv_Row.TupleLength()
                        );
                }
                if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        hv_Model_COPY_INP_TMP.Dispose();
                        HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                    }
                    else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength()
                        )).TupleEqual(1))) != 0)
                    {
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out ExpTmpOutVar_0);
                            hv_Model_COPY_INP_TMP.Dispose();
                            hv_Model_COPY_INP_TMP = ExpTmpOutVar_0;
                        }
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        ho_ModelRegion.Dispose();
                        HOperatorSet.GetNccModelRegion(out ho_ModelRegion, hv_ModelID.TupleSelect(
                            hv_Index));
                        ho_ModelContours.Dispose();
                        HOperatorSet.GenContourRegionXld(ho_ModelRegion, out ho_ModelContours,
                            "border_holes");
                        HOperatorSet.SetColor(hWindow.HalconWindow, hv_Color.TupleSelect(
                                hv_Index % (new HTuple(hv_Color.TupleLength()))));
                        HTuple end_val13 = hv_NumMatches - 1;
                        HTuple step_val13 = 1;
                        for (hv_Match = 0; hv_Match.Continue(end_val13, step_val13); hv_Match = hv_Match.TupleAdd(step_val13))
                        {
                            if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(
                                hv_Match)))) != 0)
                            {
                                hv_HomMat2DIdentity.Dispose();
                                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                                hv_HomMat2DRotate.Dispose();
                                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Angle.TupleSelect(
                                    hv_Match), 0, 0, out hv_HomMat2DRotate);
                                hv_HomMat2DTranslate.Dispose();
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row.TupleSelect(
                                    hv_Match), hv_Column.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                                ho_ContoursAffinTrans.Dispose();
                                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffinTrans,
                                    hv_HomMat2DTranslate);
                                HOperatorSet.DispObj(ho_ContoursAffinTrans, hWindow.HalconWindow);
                                hv_RowTrans.Dispose(); hv_ColTrans.Dispose();
                                HOperatorSet.AffineTransPixel(hv_HomMat2DTranslate, 0, 0, out hv_RowTrans,
                                    out hv_ColTrans);
                                ho_Cross.Dispose();
                                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RowTrans, hv_ColTrans,
                                    6, hv_Angle.TupleSelect(hv_Match));
                                HOperatorSet.DispObj(ho_Cross, hWindow.HalconWindow);
                            }
                        }
                    }
                }
                ho_ModelRegion.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();
                ho_Cross.Dispose();

                hv_Model_COPY_INP_TMP.Dispose();
                hv_NumMatches.Dispose();
                hv_Index.Dispose();
                hv_Match.Dispose();
                hv_HomMat2DIdentity.Dispose();
                hv_HomMat2DRotate.Dispose();
                hv_HomMat2DTranslate.Dispose();
                hv_RowTrans.Dispose();
                hv_ColTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelRegion.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();
                ho_Cross.Dispose();

                hv_Model_COPY_INP_TMP.Dispose();
                hv_NumMatches.Dispose();
                hv_Index.Dispose();
                hv_Match.Dispose();
                hv_HomMat2DIdentity.Dispose();
                hv_HomMat2DRotate.Dispose();
                hv_HomMat2DTranslate.Dispose();
                hv_RowTrans.Dispose();
                hv_ColTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public double RadianToDegrees(double radian)
        {
            return radian * (180 / Math.PI);
        }

        #endregion

        #region Matching DUT

        /// <summary>
        /// Tạo model DUT
        /// </summary>
        /// <param name="template"> Ảnh đầu vào</param>
        /// <param name="path">Dường dẫn lưu model</param>
        /// <returns></returns>
        public bool CreateModelDUT(HObject template,string path)
        {
            try
            {
                var numLevels = 1;
                var startAngle = new HTuple(0).TupleRad();
                var extentAngle = new HTuple(360).TupleRad();
                var stepAngle = "auto";
                var optimization = (new HTuple("none")).TupleConcat("no_pregeneration");
                var metric = "use_polarity";
                var contrast = "auto";
                var minContrast = "auto";
                //Tạo ra model train tìm pattern
                HOperatorSet.CreateShapeModel(template,
                                              numLevels,
                                              startAngle,
                                              extentAngle,
                                              stepAngle,
                                              optimization,
                                              metric,
                                              contrast,
                                              minContrast, 
                                              out HTuple hv_ModelID);
                HOperatorSet.WriteShapeModel(hv_ModelID, path);
                template.Dispose();
                hv_ModelID.Dispose();
                return true;
            }
            catch 
            {
                return false;
            }
        }
        /// <summary>
        /// Tìm kiếm DUT
        /// </summary>
        /// <param name="ImgInput"> Ảnh đầu vào</param>
        /// <param name="hv_ModelID"> Model cần tìm kiếm</param>
        /// <param name="Roi">Vùng tìm đối tượng </param>
        /// <param name="hWindowControl">Cửa sổ</param>
        /// <returns></returns>
        public Parameter.ResultMatching FindPatternDUT(HObject ImgInput, HTuple hv_ModelID, Parameter.Roi Roi, HSmartWindowControl hWindowControl)
        {
            //Tạo ra 2 luồng kết quả
            Parameter.ResultMatching resultDUT = new Parameter.ResultMatching();
            Parameter.ResultMatching resultLabel = new Parameter.ResultMatching();
            try
            {
                HOperatorSet.DispObj(ImgInput, hWindowControl.HalconWindow);
                hWindowControl.HalconWindow.SetLineWidth(3);// width of ROI
                // Local iconic variables 
                HObject ho_Image;
                HObject ho_ModelContours;
                HObject ho_TransContours;
                // Local control variables 
                HTuple hv_ModelRegionArea = new HTuple();
                HTuple hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
                HTuple hv_HomMat2D = new HTuple(), hv_Row = new HTuple();
                HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
                HTuple hv_Score = new HTuple(), hv_I = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_ModelContours);
                HOperatorSet.GenEmptyObj(out ho_TransContours);
                //Matching 01: ************************************************
                ho_Image.Dispose();
                /// reduce image in ROI
                HObject ho_Roi;
                HObject ho_RoiStand;
                HOperatorSet.GenEmptyObj(out ho_Roi);
                HOperatorSet.GenEmptyObj(out ho_RoiStand);
                //Tạo vùng cắt ROI
                HOperatorSet.GenRectangle1(out ho_Roi, Roi.row1, Roi.col1, Roi.row2, Roi.col2);
                //Tạo vùng cắt ROI stand
                HOperatorSet.GenRectangle1(out ho_RoiStand, Roi.row1stand, Roi.col1stand, Roi.row2stand, Roi.col2stand);
                //Matching 01: Reduce the model template Tạo ra vùng tìm kiếm
                HOperatorSet.ReduceDomain(ImgInput, ho_Roi, out ho_Image);
                
                //Matching 01: Create the shape model
                //
                //Matching 01: Get the model contour for transforming it later into the image
                ho_ModelContours.Dispose(); //Model DUT
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);                
                //Matching 01: Get the reference position
                hv_ModelRegionArea.Dispose(); 
                hv_RefRow.Dispose(); 
                hv_RefColumn.Dispose();
                //Tìm đường tâm 
                HOperatorSet.AreaCenter(ho_RoiStand, out hv_ModelRegionArea, out hv_RefRow,out hv_RefColumn);
                hv_HomMat2D.Dispose();
                //Tạo ma trận 2D
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_RefRow, hv_RefColumn, 0, out hv_HomMat2D);
                ho_TransContours.Dispose();
                //Thực hiện phép biến đổi Affine
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                //
                //Matching 01: Find the model
                hv_Row.Dispose(); 
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_Score.Dispose();
                ///Model DUT với ảnh đã đc crop
                HOperatorSet.FindShapeModel(ImgInput, hv_ModelID,
                                            new HTuple(0).TupleRad(),           // Góc bắt đầu
                                            new HTuple(360).TupleRad(),         // Góc kết thúc
                                            0.7,                                // Ngưỡng điểm số (score threshold)
                                            0,                                  // Số lần xuất hiện tối thiểu
                                            0,                                  // Số lần xuất hiện tối đa (0 = không giới hạn)
                                            "least_squares",                    // Phương pháp so khớp (least squares)
                                            0,                                  // Phiên bản so khớp chính xác (0 = không dùng)
                                            1,                                  // Số kết quả trả về (1 = chỉ trả về kết quả tốt nhất)
                                            out hv_Row,                         // Hàng (Row) của tâm đối tượng được tìm thấy
                                            out hv_Column,                      // Cột (Column) của tâm đối tượng được tìm thấy
                                            out hv_Angle,                       // Góc xoay của đối tượng tìm thấy
                                            out hv_Score);                      // Điểm số khớp của đối tượng (Score)

                //Nếu có kết quả match DUT
                if (hv_Score.Length> 0)
                {                                      
                    //HOperatorSet.DispObj(Create_XLDCicle_FromRoi(hv_Row.D,hv_Column.D,Roi.radius), hWindowControl.HalconWindow);
                    //Vẽ khung vùng tìm kiếm
                    HOperatorSet.DispObj(Create_XLDObject_FromRoi(Roi), hWindowControl.HalconWindow);

                    //Kết quả tìm được cho DUT
                    resultDUT.marky = hv_Row.D;
                    resultDUT.markx = hv_Column.D;
                    resultDUT.angle = RadianToDegrees(hv_Angle.D);
                    resultDUT.score = hv_Score.D;

                    //Hiển thị message DUT  < Tọa độ tâm >
                    string dis = "Component: "+ Roi.component + "\r\n";
                    dis += "Row: " + resultDUT.marky.ToString("f2") + " ";
                    dis += "Col: " + resultDUT.markx.ToString("f2") + " ";
                    dis += "Angle: " + resultDUT.angle.ToString("f2") + "\r\n";
                   
                    //MessageBox.Show(dis);
                    //MessageBox.Show($"{resultLabel.markx} {resultLabel.marky}");
                   
                    //Tìm vị trí Label mới dựa theo tọa độ DUT
                    resultLabel = FindLabelPositionOnDUT(Roi, resultDUT);

                    //Hiển thị thông số Label mới tìm thấy < Tọa độ tâm >
                    dis += "Component: Label \r\n";
                    dis += "Row: " + resultLabel.marky.ToString("f2") + " ";
                    dis += "Col: " + resultLabel.markx.ToString("f2") + " ";
                    dis += "Angle: " + resultLabel.angle.ToString("f2") + " ";
                    dis += "Score: " + hv_Score.D.ToString("f2");

                    //MessageBox.Show($"{resultLabel.markx} {resultLabel.marky}");

                    //Hiển thị kết quả lên trên bức ảnh
                    disp_message(hWindowControl.HalconWindow, dis, "window", 100, 100, "red", "false");
                    // display new ROI
                    HOperatorSet.SetColor(hWindowControl.HalconWindow, "green");

                    HObject XLD_RoiStand_new;
                    HOperatorSet.GenEmptyObj(out XLD_RoiStand_new);
                    //Vùng ROI mới được tạo ra

                    //Col = X, Row = Y
                    var distance_Y = (Roi.row2stand - Roi.row1stand) / 2;
                    var distance_X = (Roi.col2stand - Roi.col1stand) / 2;
                    var new_roi = new Parameter.Roi
                    {
                        row1stand = resultDUT.marky - distance_Y,
                        col1stand = resultDUT.markx - distance_X,
                        row2stand = resultDUT.marky + distance_Y,
                        col2stand = resultDUT.markx + distance_X,
                    };
                                        

                    var(Py1,Px1) = RotatePoint(resultDUT.marky - distance_Y, resultDUT.markx - distance_X, resultDUT.marky, resultDUT.markx, resultDUT.angle);

                    var(Py2,Px2) = RotatePoint(resultDUT.marky - distance_Y, resultDUT.markx + distance_X, resultDUT.marky, resultDUT.markx, resultDUT.angle);

                    var(Py3,Px3) = RotatePoint(resultDUT.marky + distance_Y, resultDUT.markx + distance_X, resultDUT.marky, resultDUT.markx, resultDUT.angle);

                    var(Py4,Px4) = RotatePoint(resultDUT.marky + distance_Y, resultDUT.markx - distance_X, resultDUT.marky, resultDUT.markx, resultDUT.angle);

                    var test_ROI = new Parameter.Roi
                    {
                        row1stand = Py2,
                        row2stand = Py1,
                        col1stand = Px2,
                        col2stand = Px1,
                    };
                    //Hiển thị vùng ROI mới
                    XLD_RoiStand_new = Create_XLDObject_FromRoiStand(new_roi);
                   // HOperatorSet.DispXld(XLD_RoiStand_new, hWindowControl.HalconWindow);


                    HObject XLD_RoiStand_new1;
                    HOperatorSet.GenEmptyObj(out XLD_RoiStand_new1);
                    XLD_RoiStand_new1 = Draw_Polygon(Px1,Py1,Px2,Py2,Px3,Py3,Px4,Py4);
                    HOperatorSet.DispXld(XLD_RoiStand_new1, hWindowControl.HalconWindow);


                    HObject Sample_Circle;
                    HOperatorSet.GenEmptyObj(out Sample_Circle);
                    HOperatorSet.DispObj(Create_XLDCicle_FromRoi(resultLabel.marky, resultLabel.markx, Roi.radius), hWindowControl.HalconWindow);

                    XLD_RoiStand_new.Dispose();
                }
                else
                {
                    string dis = "Component: "+Roi.component + "";
                    dis += "Row: Null" +  "";
                    dis += "Col: Null" + "";
                    dis += "Angle: Null" + "";
                    disp_message(hWindowControl.HalconWindow, dis, "window", 100, 100, "red", "false");
                    // display RoiStand old
                    HOperatorSet.SetColor(hWindowControl.HalconWindow, "red");
                    HObject XLD_RoiStand_old;
                    HOperatorSet.GenEmptyObj(out XLD_RoiStand_old);
                    XLD_RoiStand_old = Create_XLDObject_FromRoiStand(Roi);
                    HOperatorSet.DispXld(XLD_RoiStand_old, hWindowControl.HalconWindow);
                    XLD_RoiStand_old.Dispose();
                }
                
                
                //
                //Matching 01: *******************************************
                //Matching 01: END of generated code for model application
                //Matching 01: *******************************************
                //Giải phóng các dữ liệu dữ lý
                ho_Image.Dispose();
                ho_Roi.Dispose();
                ho_RoiStand.Dispose();
                ho_ModelContours.Dispose();
                ho_TransContours.Dispose();

                hv_ModelRegionArea.Dispose();
                hv_RefRow.Dispose();
                hv_RefColumn.Dispose();
                hv_HomMat2D.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_Score.Dispose();
                hv_I.Dispose();
            }
            catch
            {
            }
            return resultLabel;
        }
        public Parameter.ResultMatching CaliAfter(HObject ImgInput, HTuple hv_ModelID, Parameter.Roi Roi, HWindowControl hWindowControl)
        {
            Parameter.ResultMatching resultLabel = new Parameter.ResultMatching();
            try
            {
                HOperatorSet.DispObj(ImgInput, hWindowControl.HalconWindow);
                hWindowControl.HalconWindow.SetLineWidth(3);// width of ROI
                // Local iconic variables 
                HObject ho_Image;
                HObject ho_ModelContours, ho_TransContours;
                // Local control variables 
                HTuple hv_ModelRegionArea = new HTuple();
                HTuple hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
                HTuple hv_HomMat2D = new HTuple(), hv_Row = new HTuple();
                HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
                HTuple hv_Score = new HTuple(), hv_I = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_ModelContours);
                HOperatorSet.GenEmptyObj(out ho_TransContours);
                //Matching 01: ************************************************
                ho_Image.Dispose();
                /// reduce image in ROI
                HObject ho_Roi;
                HObject ho_RoiStand;
                HOperatorSet.GenEmptyObj(out ho_Roi);
                HOperatorSet.GenEmptyObj(out ho_RoiStand);
                HOperatorSet.GenRectangle1(out ho_Roi, Roi.row1, Roi.col1, Roi.row2, Roi.col2);
                HOperatorSet.GenRectangle1(out ho_RoiStand, Roi.row1stand, Roi.col1stand, Roi.row2stand, Roi.col2stand);
                HOperatorSet.ReduceDomain(ImgInput, ho_Roi, out ho_Image);
                //Matching 01: Reduce the model template
                //Matching 01: Create the shape model
                //
                //Matching 01: Get the model contour for transforming it later into the image
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                //
                //Matching 01: Get the reference position
                hv_ModelRegionArea.Dispose(); hv_RefRow.Dispose(); hv_RefColumn.Dispose();
                HOperatorSet.AreaCenter(ho_RoiStand, out hv_ModelRegionArea, out hv_RefRow,
                    out hv_RefColumn);
                hv_HomMat2D.Dispose();
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_RefRow, hv_RefColumn, 0, out hv_HomMat2D);
                ho_TransContours.Dispose();
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                //
                //Matching 01: Find the model
                hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_Score.Dispose();
                HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, (new HTuple(0)).TupleRad(),
                    (new HTuple(360)).TupleRad(), 0.7, 1, 0, "least_squares", 0, 1, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                //HOperatorSet.FindShapeModel(grayimg, hv_ModelID, 0, 360, 0.7, 0, 0, "least_squares",0, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);


                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_HomMat2D.Dispose();
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0,
                            out ExpTmpOutVar_0);
                        hv_HomMat2D.Dispose();
                        hv_HomMat2D = ExpTmpOutVar_0;
                    }
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I), hv_Column.TupleSelect(
                            hv_I), out ExpTmpOutVar_0);
                        hv_HomMat2D.Dispose();
                        hv_HomMat2D = ExpTmpOutVar_0;
                    }
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours,
                        hv_HomMat2D);

                    HOperatorSet.SetColor(hWindowControl.HalconWindow, "green");
                    HOperatorSet.DispObj(ho_TransContours, hWindowControl.HalconWindow);

                }
                if (hv_Score.Length > 0)
                {

                    resultLabel.marky = hv_Row.D;
                    resultLabel.markx = hv_Column.D;
                    resultLabel.angle = RadianToDegrees(hv_Angle.D);
                    resultLabel.score = hv_Score.D;
                    //string dis = "Component: Check Label\r\n\r\n\r\n";
                    //resultLabel = FindLabelPositionOnDUT(Roi, resultDUT);
                    //dis += "Component: Label \r\n\r\n\r\n";
                    //dis += "Row: " + resultLabel.marky + "\r\n\r\n\r\n";
                    //dis += "Col: " + resultLabel.markx + "\r\n\r\n\r\n";
                    //dis += "Angle: " + resultLabel.angle + "\r\n\r\n\r\n";
                    //dis += "Score: " + hv_Score.D + "\r\n\r\n\r\n";


                    //disp_message(hWindowControl.HalconWindow, dis, "window", 100, 100, "green", "false");
                    // display Roi new
                    //HOperatorSet.SetColor(hWindowControl.HalconWindow, "green");
                    //HObject XLD_RoiStand_new;
                    //var new_roi = new Parameter.Roi
                    //{
                    //    row1 = resultLabel.marky - Roi.row1stand / 2,
                    //    col1 = resultLabel.markx - Roi.col1stand / 2,
                    //    row2 = resultLabel.marky + Roi.row1stand / 2,
                    //    col2 = resultLabel.markx + Roi.row1stand / 2,
                    //};
                    //HOperatorSet.GenEmptyObj(out XLD_RoiStand_new);
                    //XLD_RoiStand_new = Create_XLDObject_FromRoiStand(new_roi);
                    //HOperatorSet.DispXld(XLD_RoiStand_new, hWindowControl.HalconWindow);
                    //XLD_RoiStand_new.Dispose();

                    // display RoiStand old
                    //HOperatorSet.SetColor(hWindowControl.HalconWindow, "blue");
                    //HObject XLD_RoiStand_old;
                    //HOperatorSet.GenEmptyObj(out XLD_RoiStand_old);
                    //XLD_RoiStand_old = Create_XLDObject_FromRoiStand(Roi);
                    //HOperatorSet.DispXld(XLD_RoiStand_old, hWindowControl.HalconWindow);
                    //XLD_RoiStand_old.Dispose();
                }
                else
                {
                    string dis = "Component: " + Roi.component + "\r\n\r\n\r\n";
                    dis += "Row: Null" + "\r\n\r\n\r\n";
                    dis += "Col: Null" + "\r\n\r\n\r\n";
                    dis += "Angle: Null" + "\r\n\r\n\r\n";
                    disp_message(hWindowControl.HalconWindow, dis, "window", 100, 100, "red", "false");
                    // display RoiStand old
                    HOperatorSet.SetColor(hWindowControl.HalconWindow, "red");
                    HObject XLD_RoiStand_old;
                    HOperatorSet.GenEmptyObj(out XLD_RoiStand_old);
                    XLD_RoiStand_old = Create_XLDObject_FromRoiStand(Roi);
                    HOperatorSet.DispXld(XLD_RoiStand_old, hWindowControl.HalconWindow);
                    XLD_RoiStand_old.Dispose();
                }


                //
                //Matching 01: *******************************************
                //Matching 01: END of generated code for model application
                //Matching 01: *******************************************
                //
                ho_Image.Dispose();
                ho_Roi.Dispose();
                ho_RoiStand.Dispose();
                ho_ModelContours.Dispose();
                ho_TransContours.Dispose();

                hv_ModelRegionArea.Dispose();
                hv_RefRow.Dispose();
                hv_RefColumn.Dispose();
                hv_HomMat2D.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_Score.Dispose();
                hv_I.Dispose();
            }
            catch
            {
            }
            return resultLabel;
        }
        #endregion

        #region Matching Label
        //--Matching label
        public bool CreateModelLabel(HObject templete, string path)
        {
            try
            {
                HOperatorSet.Rgb1ToGray(templete, out HObject img);
                HOperatorSet.CreateNccModel(img, 5, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), (new HTuple(1)).TupleRad(), "use_polarity", out HTuple hv_ModelID);
                HOperatorSet.WriteNccModel(hv_ModelID, path);
                templete.Dispose();
                img.Dispose();
                hv_ModelID.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Parameter.ResultMatching CaliLabel(HObject ho_Image_, HTuple ModelID, Parameter.Roi Roi, HWindowControl hWindowControl)
        {
            Parameter.ResultMatching result = new Parameter.ResultMatching();
            try
            {
                HOperatorSet.DispObj(ho_Image_, hWindowControl.HalconWindow);
                HOperatorSet.Rgb1ToGray(ho_Image_, out HObject ho_Image);
                HObject ho_ImageReduced = ReduceImageRect(ho_Image, Roi);
                HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
                HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
                hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_Score.Dispose();
                HOperatorSet.FindNccModel(ho_ImageReduced, ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), 0.7, 1, 0.9, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                HOperatorSet.SetLineWidth(hWindowControl.HalconWindow, 3);
                if (hv_Score.D > 0)
                {
                    dev_display_ncc_matching_results(hWindowControl, ModelID, "green", hv_Row, hv_Column, hv_Angle.D, 0);
                    result.marky = hv_Row.D;
                    result.markx = hv_Column.D;
                    result.angle = RadianToDegrees(hv_Angle.D);
                    result.score = hv_Score.D;
                    string dis = "Component: " + Roi.component + "\r\n\r\n\r\n";
                    dis += "Row: " + result.marky + "\r\n\r\n\r\n";
                    dis += "Col: " + result.markx + "\r\n\r\n\r\n";
                    dis += "Angle: " + result.angle + "\r\n\r\n\r\n";
                    dis += "Score: " + result.score*100 + "\r\n\r\n\r\n";
                    disp_message(hWindowControl.HalconWindow, dis, "window", 100, 100, "green", "false");
                }
                else
                {
                    dev_display_ncc_matching_results(hWindowControl, ModelID, "red", hv_Row, hv_Column, hv_Angle, 0);
                }
                ho_Image.Dispose();
                ho_ImageReduced.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_Score.Dispose();
                hv_Row.Dispose();
            }
            catch { }
            
            return result;
        }
        public Parameter.ResultMatching FindLabelPositionOnDUT(Parameter.Roi old_, Parameter.ResultMatching new_)
        {
            try
            {
                // Góc xoay (đơn vị: độ)
                double alpha_degrees = new_.angle;

                // Chuyển đổi góc từ độ sang radian
                double alpha_radians = -alpha_degrees * Math.PI / 180.0;

                // Bước 1: Khoảng cách giữa 2 điểm tâm label và tâm DUT
                double distance_X = old_.markx - old_.centerx;
                double distance_Y = old_.marky - old_.centery;

                // Bước 2: Xoay tọa độ của điểm trên hình tròn xung quanh gốc tọa độ (0, 0) bằng góc xoay alpha
                double x1_rotated = distance_X * Math.Cos(alpha_radians) - distance_Y * Math.Sin(alpha_radians);
                double y1_rotated = distance_X * Math.Sin(alpha_radians) + distance_Y * Math.Cos(alpha_radians);

                // Bước 3: Dịch chuyển tọa độ đã xoay về tâm mới sau khi xoay và lệch
                double x_result = x1_rotated + new_.markx;
                double y_result = y1_rotated + new_.marky;

                return new Parameter.ResultMatching
                {
                    markx = x_result,
                    marky = y_result,
                    angle = alpha_degrees,//<90? alpha_degrees:-(360- alpha_degrees),
                    score = new_.score
                };
            }
            catch
            {
                return new Parameter.ResultMatching
                {
                    score = 0
                };
            }

        }
        #endregion

        public static (double PxNew, double PyNew) RotatePoint(double Px, double Py, double Cx, double Cy, double alphaDegrees)
        {
            // Chuyển góc từ độ sang radian
            double alphaRadians = DegreesToRadians(alphaDegrees);

            // Tính sự chênh lệch tọa độ
            double dx = Px - Cx;
            double dy = Py - Cy;

            // Tính tọa độ mới sau khi xoay
            double PxNew = Cx + (dx * Math.Cos(alphaRadians) - dy * Math.Sin(alphaRadians));
            double PyNew = Cy + (dx * Math.Sin(alphaRadians) + dy * Math.Cos(alphaRadians));

            return (PxNew, PyNew);
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }

}
