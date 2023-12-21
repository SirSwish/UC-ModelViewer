/*                         MIT License
                 Copyright (c) 2017 TGASharpLib

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;



namespace UC_ModelViewer.MVVM.Model
{
    class ImageConvert
    {
        #region Enums
        public enum TgaColorMapType : byte
        {
            NoColorMap = 0, ColorMap = 1, Truevision_2, Truevision_3, Truevision_4,
            Truevision_5, Truevision_6, Truevision_7, Truevision_8, Truevision_9,
            Truevision_10, Truevision_11, Truevision_12, Truevision_13, Truevision_14,
            Truevision_15, Truevision_16, Truevision_17, Truevision_18, Truevision_19,
            Truevision_20, Truevision_21, Truevision_22, Truevision_23, Truevision_24,
            Truevision_25, Truevision_26, Truevision_27, Truevision_28, Truevision_29,
            Truevision_30, Truevision_31, Truevision_32, Truevision_33, Truevision_34,
            Truevision_35, Truevision_36, Truevision_37, Truevision_38, Truevision_39,
            Truevision_40, Truevision_41, Truevision_42, Truevision_43, Truevision_44,
            Truevision_45, Truevision_46, Truevision_47, Truevision_48, Truevision_49,
            Truevision_50, Truevision_51, Truevision_52, Truevision_53, Truevision_54,
            Truevision_55, Truevision_56, Truevision_57, Truevision_58, Truevision_59,
            Truevision_60, Truevision_61, Truevision_62, Truevision_63, Truevision_64,
            Truevision_65, Truevision_66, Truevision_67, Truevision_68, Truevision_69,
            Truevision_70, Truevision_71, Truevision_72, Truevision_73, Truevision_74,
            Truevision_75, Truevision_76, Truevision_77, Truevision_78, Truevision_79,
            Truevision_80, Truevision_81, Truevision_82, Truevision_83, Truevision_84,
            Truevision_85, Truevision_86, Truevision_87, Truevision_88, Truevision_89,
            Truevision_90, Truevision_91, Truevision_92, Truevision_93, Truevision_94,
            Truevision_95, Truevision_96, Truevision_97, Truevision_98, Truevision_99,
            Truevision_100, Truevision_101, Truevision_102, Truevision_103, Truevision_104,
            Truevision_105, Truevision_106, Truevision_107, Truevision_108, Truevision_109,
            Truevision_110, Truevision_111, Truevision_112, Truevision_113, Truevision_114,
            Truevision_115, Truevision_116, Truevision_117, Truevision_118, Truevision_119,
            Truevision_120, Truevision_121, Truevision_122, Truevision_123, Truevision_124,
            Truevision_125, Truevision_126, Truevision_127, Other_128, Other_129,
            Other_130, Other_131, Other_132, Other_133, Other_134, Other_135, Other_136,
            Other_137, Other_138, Other_139, Other_140, Other_141, Other_142, Other_143,
            Other_144, Other_145, Other_146, Other_147, Other_148, Other_149, Other_150,
            Other_151, Other_152, Other_153, Other_154, Other_155, Other_156, Other_157,
            Other_158, Other_159, Other_160, Other_161, Other_162, Other_163, Other_164,
            Other_165, Other_166, Other_167, Other_168, Other_169, Other_170, Other_171,
            Other_172, Other_173, Other_174, Other_175, Other_176, Other_177, Other_178,
            Other_179, Other_180, Other_181, Other_182, Other_183, Other_184, Other_185,
            Other_186, Other_187, Other_188, Other_189, Other_190, Other_191, Other_192,
            Other_193, Other_194, Other_195, Other_196, Other_197, Other_198, Other_199,
            Other_200, Other_201, Other_202, Other_203, Other_204, Other_205, Other_206,
            Other_207, Other_208, Other_209, Other_210, Other_211, Other_212, Other_213,
            Other_214, Other_215, Other_216, Other_217, Other_218, Other_219, Other_220,
            Other_221, Other_222, Other_223, Other_224, Other_225, Other_226, Other_227,
            Other_228, Other_229, Other_230, Other_231, Other_232, Other_233, Other_234,
            Other_235, Other_236, Other_237, Other_238, Other_239, Other_240, Other_241,
            Other_242, Other_243, Other_244, Other_245, Other_246, Other_247, Other_248,
            Other_249, Other_250, Other_251, Other_252, Other_253, Other_254, Other_255
        }


        public enum TgaColorMapEntrySize : byte
        {
            Other = 0,
            X1R5G5B5 = 15,
            A1R5G5B5 = 16,
            R8G8B8 = 24,
            A8R8G8B8 = 32
        }

        public enum TgaImageType : byte
        {
            NoImageData = 0, Uncompressed_ColorMapped = 1, Uncompressed_TrueColor, Uncompressed_BlackWhite, _Truevision_4,
            _Truevision_5, _Truevision_6, _Truevision_7, _Truevision_8, RLE_ColorMapped = 9,
            RLE_TrueColor, RLE_BlackWhite, _Truevision_12, _Truevision_13, _Truevision_14,
            _Truevision_15, _Truevision_16, _Truevision_17, _Truevision_18, _Truevision_19,
            _Truevision_20, _Truevision_21, _Truevision_22, _Truevision_23, _Truevision_24,
            _Truevision_25, _Truevision_26, _Truevision_27, _Truevision_28, _Truevision_29,
            _Truevision_30, _Truevision_31, _Truevision_32, _Truevision_33, _Truevision_34,
            _Truevision_35, _Truevision_36, _Truevision_37, _Truevision_38, _Truevision_39,
            _Truevision_40, _Truevision_41, _Truevision_42, _Truevision_43, _Truevision_44,
            _Truevision_45, _Truevision_46, _Truevision_47, _Truevision_48, _Truevision_49,
            _Truevision_50, _Truevision_51, _Truevision_52, _Truevision_53, _Truevision_54,
            _Truevision_55, _Truevision_56, _Truevision_57, _Truevision_58, _Truevision_59,
            _Truevision_60, _Truevision_61, _Truevision_62, _Truevision_63, _Truevision_64,
            _Truevision_65, _Truevision_66, _Truevision_67, _Truevision_68, _Truevision_69,
            _Truevision_70, _Truevision_71, _Truevision_72, _Truevision_73, _Truevision_74,
            _Truevision_75, _Truevision_76, _Truevision_77, _Truevision_78, _Truevision_79,
            _Truevision_80, _Truevision_81, _Truevision_82, _Truevision_83, _Truevision_84,
            _Truevision_85, _Truevision_86, _Truevision_87, _Truevision_88, _Truevision_89,
            _Truevision_90, _Truevision_91, _Truevision_92, _Truevision_93, _Truevision_94,
            _Truevision_95, _Truevision_96, _Truevision_97, _Truevision_98, _Truevision_99,
            _Truevision_100, _Truevision_101, _Truevision_102, _Truevision_103, _Truevision_104,
            _Truevision_105, _Truevision_106, _Truevision_107, _Truevision_108, _Truevision_109,
            _Truevision_110, _Truevision_111, _Truevision_112, _Truevision_113, _Truevision_114,
            _Truevision_115, _Truevision_116, _Truevision_117, _Truevision_118, _Truevision_119,
            _Truevision_120, _Truevision_121, _Truevision_122, _Truevision_123, _Truevision_124,
            _Truevision_125, _Truevision_126, _Truevision_127, _Other_128, _Other_129,
            _Other_130, _Other_131, _Other_132, _Other_133, _Other_134, _Other_135, _Other_136,
            _Other_137, _Other_138, _Other_139, _Other_140, _Other_141, _Other_142, _Other_143,
            _Other_144, _Other_145, _Other_146, _Other_147, _Other_148, _Other_149, _Other_150,
            _Other_151, _Other_152, _Other_153, _Other_154, _Other_155, _Other_156, _Other_157,
            _Other_158, _Other_159, _Other_160, _Other_161, _Other_162, _Other_163, _Other_164,
            _Other_165, _Other_166, _Other_167, _Other_168, _Other_169, _Other_170, _Other_171,
            _Other_172, _Other_173, _Other_174, _Other_175, _Other_176, _Other_177, _Other_178,
            _Other_179, _Other_180, _Other_181, _Other_182, _Other_183, _Other_184, _Other_185,
            _Other_186, _Other_187, _Other_188, _Other_189, _Other_190, _Other_191, _Other_192,
            _Other_193, _Other_194, _Other_195, _Other_196, _Other_197, _Other_198, _Other_199,
            _Other_200, _Other_201, _Other_202, _Other_203, _Other_204, _Other_205, _Other_206,
            _Other_207, _Other_208, _Other_209, _Other_210, _Other_211, _Other_212, _Other_213,
            _Other_214, _Other_215, _Other_216, _Other_217, _Other_218, _Other_219, _Other_220,
            _Other_221, _Other_222, _Other_223, _Other_224, _Other_225, _Other_226, _Other_227,
            _Other_228, _Other_229, _Other_230, _Other_231, _Other_232, _Other_233, _Other_234,
            _Other_235, _Other_236, _Other_237, _Other_238, _Other_239, _Other_240, _Other_241,
            _Other_242, _Other_243, _Other_244, _Other_245, _Other_246, _Other_247, _Other_248,
            _Other_249, _Other_250, _Other_251, _Other_252, _Other_253, _Other_254, _Other_255
        }

        public enum TgaPixelDepth : byte
        {
            Other = 0,
            Bpp8 = 8,
            Bpp16 = 16,
            Bpp24 = 24,
            Bpp32 = 32
        }

        public enum TgaImgOrigin : byte
        {
            BottomLeft = 0,
            BottomRight,
            TopLeft,
            TopRight
        }

        public enum TgaAttrType : byte
        {
            NoAlpha = 0, UndefinedAlphaCanBeIgnored, UndefinedAlphaButShouldBeRetained, UsefulAlpha, PreMultipliedAlpha,
            _Reserved_5, _Reserved_6, _Reserved_7, _Reserved_8, _Reserved_9,
            _Reserved_10, _Reserved_11, _Reserved_12, _Reserved_13, _Reserved_14,
            _Reserved_15, _Reserved_16, _Reserved_17, _Reserved_18, _Reserved_19,
            _Reserved_20, _Reserved_21, _Reserved_22, _Reserved_23, _Reserved_24,
            _Reserved_25, _Reserved_26, _Reserved_27, _Reserved_28, _Reserved_29,
            _Reserved_30, _Reserved_31, _Reserved_32, _Reserved_33, _Reserved_34,
            _Reserved_35, _Reserved_36, _Reserved_37, _Reserved_38, _Reserved_39,
            _Reserved_40, _Reserved_41, _Reserved_42, _Reserved_43, _Reserved_44,
            _Reserved_45, _Reserved_46, _Reserved_47, _Reserved_48, _Reserved_49,
            _Reserved_50, _Reserved_51, _Reserved_52, _Reserved_53, _Reserved_54,
            _Reserved_55, _Reserved_56, _Reserved_57, _Reserved_58, _Reserved_59,
            _Reserved_60, _Reserved_61, _Reserved_62, _Reserved_63, _Reserved_64,
            _Reserved_65, _Reserved_66, _Reserved_67, _Reserved_68, _Reserved_69,
            _Reserved_70, _Reserved_71, _Reserved_72, _Reserved_73, _Reserved_74,
            _Reserved_75, _Reserved_76, _Reserved_77, _Reserved_78, _Reserved_79,
            _Reserved_80, _Reserved_81, _Reserved_82, _Reserved_83, _Reserved_84,
            _Reserved_85, _Reserved_86, _Reserved_87, _Reserved_88, _Reserved_89,
            _Reserved_90, _Reserved_91, _Reserved_92, _Reserved_93, _Reserved_94,
            _Reserved_95, _Reserved_96, _Reserved_97, _Reserved_98, _Reserved_99,
            _Reserved_100, _Reserved_101, _Reserved_102, _Reserved_103, _Reserved_104,
            _Reserved_105, _Reserved_106, _Reserved_107, _Reserved_108, _Reserved_109,
            _Reserved_110, _Reserved_111, _Reserved_112, _Reserved_113, _Reserved_114,
            _Reserved_115, _Reserved_116, _Reserved_117, _Reserved_118, _Reserved_119,
            _Reserved_120, _Reserved_121, _Reserved_122, _Reserved_123, _Reserved_124,
            _Reserved_125, _Reserved_126, _Reserved_127, _UnAssigned_128, _UnAssigned_129,
            _UnAssigned_130, _UnAssigned_131, _UnAssigned_132, _UnAssigned_133, _UnAssigned_134,
            _UnAssigned_135, _UnAssigned_136, _UnAssigned_137, _UnAssigned_138, _UnAssigned_139,
            _UnAssigned_140, _UnAssigned_141, _UnAssigned_142, _UnAssigned_143, _UnAssigned_144,
            _UnAssigned_145, _UnAssigned_146, _UnAssigned_147, _UnAssigned_148, _UnAssigned_149,
            _UnAssigned_150, _UnAssigned_151, _UnAssigned_152, _UnAssigned_153, _UnAssigned_154,
            _UnAssigned_155, _UnAssigned_156, _UnAssigned_157, _UnAssigned_158, _UnAssigned_159,
            _UnAssigned_160, _UnAssigned_161, _UnAssigned_162, _UnAssigned_163, _UnAssigned_164,
            _UnAssigned_165, _UnAssigned_166, _UnAssigned_167, _UnAssigned_168, _UnAssigned_169,
            _UnAssigned_170, _UnAssigned_171, _UnAssigned_172, _UnAssigned_173, _UnAssigned_174,
            _UnAssigned_175, _UnAssigned_176, _UnAssigned_177, _UnAssigned_178, _UnAssigned_179,
            _UnAssigned_180, _UnAssigned_181, _UnAssigned_182, _UnAssigned_183, _UnAssigned_184,
            _UnAssigned_185, _UnAssigned_186, _UnAssigned_187, _UnAssigned_188, _UnAssigned_189,
            _UnAssigned_190, _UnAssigned_191, _UnAssigned_192, _UnAssigned_193, _UnAssigned_194,
            _UnAssigned_195, _UnAssigned_196, _UnAssigned_197, _UnAssigned_198, _UnAssigned_199,
            _UnAssigned_200, _UnAssigned_201, _UnAssigned_202, _UnAssigned_203, _UnAssigned_204,
            _UnAssigned_205, _UnAssigned_206, _UnAssigned_207, _UnAssigned_208, _UnAssigned_209,
            _UnAssigned_210, _UnAssigned_211, _UnAssigned_212, _UnAssigned_213, _UnAssigned_214,
            _UnAssigned_215, _UnAssigned_216, _UnAssigned_217, _UnAssigned_218, _UnAssigned_219,
            _UnAssigned_220, _UnAssigned_221, _UnAssigned_222, _UnAssigned_223, _UnAssigned_224,
            _UnAssigned_225, _UnAssigned_226, _UnAssigned_227, _UnAssigned_228, _UnAssigned_229,
            _UnAssigned_230, _UnAssigned_231, _UnAssigned_232, _UnAssigned_233, _UnAssigned_234,
            _UnAssigned_235, _UnAssigned_236, _UnAssigned_237, _UnAssigned_238, _UnAssigned_239,
            _UnAssigned_240, _UnAssigned_241, _UnAssigned_242, _UnAssigned_243, _UnAssigned_244,
            _UnAssigned_245, _UnAssigned_246, _UnAssigned_247, _UnAssigned_248, _UnAssigned_249,
            _UnAssigned_250, _UnAssigned_251, _UnAssigned_252, _UnAssigned_253, _UnAssigned_254,
            _UnAssigned_255
        }

        #endregion

        #region Classes
        public class TgaColorKey : ICloneable
        {
            byte a = 0;
            byte r = 0;
            byte g = 0;
            byte b = 0;

            public TgaColorKey() { }

            public TgaColorKey(byte A, byte R, byte G, byte B)
            {
                a = A;
                r = R;
                g = G;
                b = B;
            }

            public TgaColorKey(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                Color color = Color.FromArgb(BitConverter.ToInt32(Bytes, 0));
                a = color.A;
                r = color.R;
                g = color.G;
                b = color.B;
            }

            public TgaColorKey(int ARGB)
            {
                Color ColorARGB = Color.FromArgb(ARGB);
                a = ColorARGB.A;
                r = ColorARGB.R;
                g = ColorARGB.G;
                b = ColorARGB.B;
            }

            public TgaColorKey(System.Drawing.Color color)
            {
                a = color.A;
                r = color.R;
                g = color.G;
                b = color.B;
            }

            public byte A
            {
                get
                {
                    return a;
                }
                set
                {
                    a = value;
                }
            }

            public byte R
            {
                get
                {
                    return r;
                }
                set
                {
                    r = value;
                }
            }

            public byte G
            {
                get
                {
                    return g;
                }
                set
                {
                    g = value;
                }
            }

            public byte B
            {
                get
                {
                    return b;
                }
                set
                {
                    b = value;
                }
            }

            public
            const int Size = 4;

            public TgaColorKey Clone()
            {
                return new TgaColorKey(a, r, g, b);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaColorKey) ? Equals((TgaColorKey)obj) : false);
            }

            public bool Equals(TgaColorKey item)
            {
                return (a == item.a && r == item.r && g == item.g && b == item.b);
            }

            public static bool operator ==(TgaColorKey item1, TgaColorKey item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaColorKey item1, TgaColorKey item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                return ToInt().GetHashCode();
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {2}={3}, {4}={5}, {6}={7}",
                  nameof(A), a, nameof(R), r, nameof(G), g, nameof(B), b);
            }

            public byte[] ToBytes()
            {
                return BitConverter.GetBytes(ToInt());
            }

            public System.Drawing.Color ToColor()
            {
                return System.Drawing.Color.FromArgb(a, r, g, b);
            }

            public int ToInt()
            {
                return ToColor().ToArgb();
            }
        }

        public class TgaColorMapSpec : ICloneable
        {
            ushort firstEntryIndex = 0;
            ushort colorMapLength = 0;
            TgaColorMapEntrySize colorMapEntrySize = TgaColorMapEntrySize.Other;

            public TgaColorMapSpec() { }

            public TgaColorMapSpec(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                firstEntryIndex = BitConverter.ToUInt16(Bytes, 0);
                colorMapLength = BitConverter.ToUInt16(Bytes, 2);
                colorMapEntrySize = (TgaColorMapEntrySize)Bytes[4];
            }

            public ushort FirstEntryIndex
            {
                get
                {
                    return firstEntryIndex;
                }
                set
                {
                    firstEntryIndex = value;
                }
            }

            public ushort ColorMapLength
            {
                get
                {
                    return colorMapLength;
                }
                set
                {
                    colorMapLength = value;
                }
            }

            public TgaColorMapEntrySize ColorMapEntrySize
            {
                get
                {
                    return colorMapEntrySize;
                }
                set
                {
                    colorMapEntrySize = value;
                }
            }

            public
            const int Size = 5;

            public TgaColorMapSpec Clone()
            {
                return new TgaColorMapSpec(ToBytes());
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaColorMapSpec) ? Equals((TgaColorMapSpec)obj) : false);
            }

            public bool Equals(TgaColorMapSpec item)
            {
                return (firstEntryIndex == item.firstEntryIndex &&
                  colorMapLength == item.colorMapLength &&
                  colorMapEntrySize == item.colorMapEntrySize);
            }

            public static bool operator ==(TgaColorMapSpec item1, TgaColorMapSpec item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaColorMapSpec item1, TgaColorMapSpec item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (firstEntryIndex << 16 | colorMapLength).GetHashCode() ^ colorMapEntrySize.GetHashCode();
                }
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {2}={3}, {4}={5}", nameof(FirstEntryIndex), FirstEntryIndex,
                  nameof(ColorMapLength), ColorMapLength, nameof(ColorMapEntrySize), ColorMapEntrySize);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(firstEntryIndex, colorMapLength, (byte)colorMapEntrySize);
            }
        }

        public class TgaComment : ICloneable
        {
            const int StrNLen = 80;
            string origString = String.Empty;
            char blankSpaceChar = TgaString.DefaultBlankSpaceChar;

            public TgaComment() { }

            public TgaComment(string Str, char BlankSpaceChar = '\0')
            {
                if (Str == null)
                    throw new ArgumentNullException(nameof(Str) + " = null!");

                origString = Str;
                blankSpaceChar = BlankSpaceChar;
            }

            public TgaComment(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                string s = Encoding.ASCII.GetString(Bytes, 0, StrNLen);
                s += Encoding.ASCII.GetString(Bytes, 81, StrNLen);
                s += Encoding.ASCII.GetString(Bytes, 162, StrNLen);
                s += Encoding.ASCII.GetString(Bytes, 243, StrNLen);

                switch (s[s.Length - 1])
                {
                    case '\0':
                    case ' ':
                        blankSpaceChar = s[s.Length - 1];
                        origString = s.TrimEnd(new char[] {
                            s[s.Length - 1]
                         });
                        break;
                    default:
                        origString = s;
                        break;
                }
            }

            public
            const int Size = 81 * 4;

            public string OriginalString
            {
                get
                {
                    return origString;
                }
                set
                {
                    origString = value;
                }
            }

            public char BlankSpaceChar
            {
                get
                {
                    return blankSpaceChar;
                }
                set
                {
                    blankSpaceChar = value;
                }
            }

            public TgaComment Clone()
            {
                return new TgaComment(origString, blankSpaceChar);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaComment) ? Equals((TgaComment)obj) : false);
            }

            public bool Equals(TgaComment item)
            {
                return (origString == item.origString && blankSpaceChar == item.blankSpaceChar);
            }

            public static bool operator ==(TgaComment item1, TgaComment item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaComment item1, TgaComment item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                return origString.GetHashCode() ^ blankSpaceChar.GetHashCode();
            }

            public override string ToString()
            {
                return Encoding.ASCII.GetString(ToBytes()).Replace("\0", @"\0");
            }

            public string GetString()
            {
                String Str = Encoding.ASCII.GetString(ToBytes());
                for (int i = 1; i < 4; i++)
                    Str = Str.Insert((StrNLen + 1) * i + i - 1, "\n");
                return Str.Replace("\0", String.Empty).TrimEnd(new char[] {
          '\n'
        });
            }

            public byte[] ToBytes()
            {
                return ToBytes(origString, blankSpaceChar);
            }

            public static byte[] ToBytes(string Str, char BlankSpaceChar = '\0')
            {
                char[] C = new char[81 * 4];

                for (int i = 0; i < C.Length; i++)
                {
                    if ((i + 82) % 81 == 0)
                        C[i] = TgaString.DefaultEndingChar;
                    else
                    {
                        int Index = i - i / 81;
                        C[i] = (Index < Str.Length ? Str[Index] : BlankSpaceChar);
                    }
                }
                return Encoding.ASCII.GetBytes(C);
            }
        }

        public class TgaDateTime : ICloneable
        {
            ushort month = 0;
            ushort day = 0;
            ushort year = 0;
            ushort hour = 0;
            ushort minute = 0;
            ushort second = 0;

            public TgaDateTime() { }

            public TgaDateTime(DateTime DateAndTime)
            {
                month = (ushort)DateAndTime.Month;
                day = (ushort)DateAndTime.Day;
                year = (ushort)DateAndTime.Year;
                hour = (ushort)DateAndTime.Hour;
                minute = (ushort)DateAndTime.Minute;
                second = (ushort)DateAndTime.Second;
            }

            public TgaDateTime(ushort Month, ushort Day, ushort Year, ushort Hour, ushort Minute, ushort Second)
            {
                month = Month;
                day = Day;
                year = Year;
                hour = Hour;
                minute = Minute;
                second = Second;
            }

            public TgaDateTime(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                else if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes) + " must be equal " + Size + "!");

                month = BitConverter.ToUInt16(Bytes, 0);
                day = BitConverter.ToUInt16(Bytes, 2);
                year = BitConverter.ToUInt16(Bytes, 4);
                hour = BitConverter.ToUInt16(Bytes, 6);
                minute = BitConverter.ToUInt16(Bytes, 8);
                second = BitConverter.ToUInt16(Bytes, 10);
            }

            public ushort Month
            {
                get
                {
                    return month;
                }
                set
                {
                    month = value;
                }
            }

            public ushort Day
            {
                get
                {
                    return day;
                }
                set
                {
                    day = value;
                }
            }

            public ushort Year
            {
                get
                {
                    return year;
                }
                set
                {
                    year = value;
                }
            }

            public ushort Hour
            {
                get
                {
                    return hour;
                }
                set
                {
                    hour = value;
                }
            }

            public ushort Minute
            {
                get
                {
                    return minute;
                }
                set
                {
                    minute = value;
                }
            }

            public ushort Second
            {
                get
                {
                    return second;
                }
                set
                {
                    second = value;
                }
            }

            public
            const int Size = 12;

            public TgaDateTime Clone()
            {
                return new TgaDateTime(month, day, year, hour, minute, second);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaDateTime) ? Equals((TgaDateTime)obj) : false);
            }

            public bool Equals(TgaDateTime item)
            {
                return (
                  month == item.month &&
                  day == item.day &&
                  year == item.year &&
                  hour == item.hour &&
                  minute == item.minute &&
                  second == item.second);
            }

            public static bool operator ==(TgaDateTime item1, TgaDateTime item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaDateTime item1, TgaDateTime item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + (month << 16 | hour).GetHashCode();
                    hash = hash * 23 + (day << 16 | minute).GetHashCode();
                    hash = hash * 23 + (year << 16 | second).GetHashCode();
                    return hash;
                }
            }

            public override string ToString()
            {
                return String.Format("{0:D4}.{1:D2}.{2:D2} {3}:{4:D2}:{5:D2}", year, month, day, hour, minute, second);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(month, day, year, hour, minute, second);
            }

            public DateTime ToDateTime()
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
        }

        public class TgaDevEntry : ICloneable
        {
            ushort fieldTag = 0;
            uint fieldFileOffset = 0;
            byte[] data = null;

            public TgaDevEntry() { }

            public TgaDevEntry(TgaDevEntry Entry)
            {
                if (Entry == null)
                    throw new ArgumentNullException();

                fieldTag = Entry.fieldTag;
                fieldFileOffset = Entry.fieldFileOffset;
                data = BitConverterExt.ToBytes(Entry.data);
            }

            public TgaDevEntry(ushort Tag, uint Offset, byte[] Data = null)
            {
                fieldTag = Tag;
                fieldFileOffset = Offset;
                data = Data;
            }

            public TgaDevEntry(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                else if (Bytes.Length < 6)
                    throw new ArgumentOutOfRangeException(nameof(Bytes) + " must be >= 6!");

                fieldTag = BitConverter.ToUInt16(Bytes, 0);
                fieldFileOffset = BitConverter.ToUInt32(Bytes, 2);

                if (Bytes.Length > 6)
                    data = BitConverterExt.GetElements(Bytes, 6, Bytes.Length - 6);
            }

            public ushort Tag
            {
                get
                {
                    return fieldTag;
                }
                set
                {
                    fieldTag = value;
                }
            }

            public uint Offset
            {
                get
                {
                    return fieldFileOffset;
                }
                set
                {
                    fieldFileOffset = value;
                }
            }

            public byte[] Data
            {
                get
                {
                    return data;
                }
                set
                {
                    data = value;
                }
            }

            public int FieldSize
            {
                get
                {
                    if (Data == null)
                        return -1;

                    return Data.Length;
                }
            }

            public
            const int Size = 10;

            public TgaDevEntry Clone()
            {
                return new TgaDevEntry(this);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaDevEntry) ? Equals((TgaDevEntry)obj) : false);
            }

            public bool Equals(TgaDevEntry item)
            {
                return (fieldTag == item.fieldTag &&
                  fieldFileOffset == item.fieldFileOffset &&
                  BitConverterExt.IsArraysEqual(data, item.data));
            }

            public static bool operator ==(TgaDevEntry item1, TgaDevEntry item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaDevEntry item1, TgaDevEntry item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + fieldTag.GetHashCode();
                    hash = hash * 23 + fieldFileOffset.GetHashCode();

                    if (data != null)
                        for (int i = 0; i < data.Length; i++)
                            hash = hash * 23 + data[i].GetHashCode();

                    return hash;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {1}={2}, {3}={4}", nameof(Tag), fieldTag,
                  nameof(Offset), fieldFileOffset, nameof(FieldSize), FieldSize);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(fieldTag, fieldFileOffset, (data == null ? 0 : data.Length));
            }
        }
        public class TgaFraction : ICloneable
        {
            ushort numerator = 0;
            ushort denominator = 0;

            public TgaFraction(ushort Numerator = 0, ushort Denominator = 0)
            {
                numerator = Numerator;
                denominator = Denominator;
            }

            public TgaFraction(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                numerator = BitConverter.ToUInt16(Bytes, 0);
                denominator = BitConverter.ToUInt16(Bytes, 2);
            }

            public ushort Numerator
            {
                get
                {
                    return numerator;
                }
                set
                {
                    numerator = value;
                }
            }

            public ushort Denominator
            {
                get
                {
                    return denominator;
                }
                set
                {
                    denominator = value;
                }
            }

            public float AspectRatio
            {
                get
                {
                    if (numerator == denominator)
                        return 1f;

                    return numerator / (float)denominator;
                }
            }

            public static readonly TgaFraction Empty = new TgaFraction();

            public static readonly TgaFraction One = new TgaFraction(1, 1);

            public
            const int Size = 4;

            public TgaFraction Clone()
            {
                return new TgaFraction(numerator, denominator);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaFraction) ? Equals((TgaFraction)obj) : false);
            }

            public bool Equals(TgaFraction item)
            {
                return (numerator == item.numerator && denominator == item.denominator);
            }

            public static bool operator ==(TgaFraction item1, TgaFraction item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaFraction item1, TgaFraction item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                return (numerator << 16 | denominator).GetHashCode();
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {2}={3}", nameof(Numerator), numerator,
                  nameof(Denominator), denominator);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(numerator, denominator);
            }
        }

        public class TgaImageDescriptor : ICloneable
        {
            TgaImgOrigin imageOrigin = 0;
            byte alphaChannelBits = 0;
            public TgaImageDescriptor() { }

            public TgaImageDescriptor(byte b)
            {
                imageOrigin = (TgaImgOrigin)((b & 0x30) >> 4);
                alphaChannelBits = (byte)(b & 0x0F);
            }

            public TgaImgOrigin ImageOrigin
            {
                get
                {
                    return imageOrigin;
                }
                set
                {
                    imageOrigin = value;
                }
            }

            public byte AlphaChannelBits
            {
                get
                {
                    return alphaChannelBits;
                }
                set
                {
                    alphaChannelBits = value;
                }
            }

            public
            const int Size = 1;

            public TgaImageDescriptor Clone()
            {
                return new TgaImageDescriptor(ToByte());
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaImageDescriptor) ? Equals((TgaImageDescriptor)obj) : false);
            }

            public bool Equals(TgaImageDescriptor item)
            {
                return (imageOrigin == item.imageOrigin && alphaChannelBits == item.alphaChannelBits);
            }

            public static bool operator ==(TgaImageDescriptor item1, TgaImageDescriptor item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaImageDescriptor item1, TgaImageDescriptor item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((int)ImageOrigin << 4 | alphaChannelBits).GetHashCode();
                }
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {2}={3}, ImageDescriptor_AsByte={4}", nameof(ImageOrigin),
                  imageOrigin, nameof(AlphaChannelBits), alphaChannelBits, ToByte());
            }

            public byte ToByte()
            {
                return (byte)(((int)imageOrigin << 4) | alphaChannelBits);
            }
        }

        public class TgaImageSpec : ICloneable
        {
            ushort x_Origin = 0;
            ushort y_Origin = 0;
            ushort imageWidth = 0;
            ushort imageHeight = 0;
            TgaPixelDepth pixelDepth = TgaPixelDepth.Other;
            TgaImageDescriptor imageDescriptor = new TgaImageDescriptor();

            public TgaImageSpec() { }

            public TgaImageSpec(ushort X_Origin, ushort Y_Origin, ushort ImageWidth, ushort ImageHeight,
              TgaPixelDepth PixelDepth, TgaImageDescriptor ImageDescriptor)
            {
                x_Origin = X_Origin;
                y_Origin = Y_Origin;
                imageWidth = ImageWidth;
                imageHeight = ImageHeight;
                pixelDepth = PixelDepth;
                imageDescriptor = ImageDescriptor;
            }

            public TgaImageSpec(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                x_Origin = BitConverter.ToUInt16(Bytes, 0);
                y_Origin = BitConverter.ToUInt16(Bytes, 2);
                imageWidth = BitConverter.ToUInt16(Bytes, 4);
                imageHeight = BitConverter.ToUInt16(Bytes, 6);
                pixelDepth = (TgaPixelDepth)Bytes[8];
                imageDescriptor = new TgaImageDescriptor(Bytes[9]);
            }

            public ushort X_Origin
            {
                get
                {
                    return x_Origin;
                }
                set
                {
                    x_Origin = value;
                }
            }

            public ushort Y_Origin
            {
                get
                {
                    return y_Origin;
                }
                set
                {
                    y_Origin = value;
                }
            }

            public ushort ImageWidth
            {
                get
                {
                    return imageWidth;
                }
                set
                {
                    imageWidth = value;
                }
            }

            public ushort ImageHeight
            {
                get
                {
                    return imageHeight;
                }
                set
                {
                    imageHeight = value;
                }
            }

            public TgaPixelDepth PixelDepth
            {
                get
                {
                    return pixelDepth;
                }
                set
                {
                    pixelDepth = value;
                }
            }

            public TgaImageDescriptor ImageDescriptor
            {
                get
                {
                    return imageDescriptor;
                }
                set
                {
                    imageDescriptor = value;
                }
            }

            public
            const int Size = 10;

            public TgaImageSpec Clone()
            {
                return new TgaImageSpec(ToBytes());
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaImageSpec) ? Equals((TgaImageSpec)obj) : false);
            }

            public bool Equals(TgaImageSpec item)
            {
                return (
                  x_Origin == item.x_Origin &&
                  y_Origin == item.y_Origin &&
                  imageWidth == item.imageWidth &&
                  imageHeight == item.imageHeight &&
                  pixelDepth == item.pixelDepth &&
                  imageDescriptor == item.imageDescriptor);
            }

            public static bool operator ==(TgaImageSpec item1, TgaImageSpec item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaImageSpec item1, TgaImageSpec item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + x_Origin.GetHashCode();
                    hash = hash * 23 + y_Origin.GetHashCode();
                    hash = hash * 23 + imageWidth.GetHashCode();
                    hash = hash * 23 + imageHeight.GetHashCode();
                    hash = hash * 23 + pixelDepth.GetHashCode();

                    if (imageDescriptor != null)
                        hash = hash * 23 + imageDescriptor.GetHashCode();

                    return hash;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {2}={3}, {4}={5}, {6}={7}, {8}={9}, {10}={11}",
                  nameof(X_Origin), x_Origin,
                  nameof(Y_Origin), y_Origin,
                  nameof(ImageWidth), imageWidth,
                  nameof(ImageHeight), imageHeight,
                  nameof(PixelDepth), pixelDepth,
                  nameof(ImageDescriptor), imageDescriptor);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(x_Origin, y_Origin, imageWidth, imageHeight,
                  (byte)pixelDepth, (imageDescriptor == null ? byte.MinValue : imageDescriptor.ToByte()));
            }
        }

        public class TgaPostageStampImage : ICloneable
        {
            byte width = 0;
            byte height = 0;
            byte[] data = null;

            public TgaPostageStampImage() { }

            public TgaPostageStampImage(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length < 2)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be >= " + 2 + "!");

                width = Bytes[0];
                height = Bytes[1];

                if (Bytes.Length > 2)
                    data = BitConverterExt.GetElements(Bytes, 2, Bytes.Length - 2);
            }

            public TgaPostageStampImage(byte Width, byte Height, byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");

                width = Width;
                height = Height;
                data = Bytes;
            }

            public byte[] Data
            {
                get
                {
                    return data;
                }
                set
                {
                    data = value;
                }
            }

            public byte Width
            {
                get
                {
                    return width;
                }
                set
                {
                    width = value;
                }
            }

            public byte Height
            {
                get
                {
                    return height;
                }
                set
                {
                    height = value;
                }
            }

            public TgaPostageStampImage Clone()
            {
                return new TgaPostageStampImage(width, height, BitConverterExt.ToBytes(data));
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaPostageStampImage) ? Equals((TgaPostageStampImage)obj) : false);
            }

            public bool Equals(TgaPostageStampImage item)
            {
                return width == item.width && height == item.height && BitConverterExt.IsArraysEqual(data, item.data);
            }

            public static bool operator ==(TgaPostageStampImage item1, TgaPostageStampImage item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaPostageStampImage item1, TgaPostageStampImage item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 27;
                    hash = (13 * hash) + width.GetHashCode();
                    hash = (13 * hash) + height.GetHashCode();
                    if (data != null)
                        for (int i = 0; i < data.Length; i++)
                            hash = (13 * hash) + data[i].GetHashCode();
                    return hash;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {2}={3}, DataLength={4}",
                  nameof(Width), width, nameof(Height), height, (data == null ? -1 : data.Length));
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(width, height, data);
            }
        }

        public class TgaSoftVersion : ICloneable
        {
            ushort versionNumber = 0;
            char versionLetter = ' ';

            public TgaSoftVersion() { }

            public TgaSoftVersion(string Str)
            {
                if (Str == null)
                    throw new ArgumentNullException();
                if (Str.Length < 3 || Str.Length > 4)
                    throw new ArgumentOutOfRangeException(nameof(Str.Length) + " must be equal 3 or 4!");

                bool Res = ushort.TryParse(Str.Substring(0, 3), out versionNumber);
                if (Res && Str.Length == 4)
                    versionLetter = Str[3];
            }

            public TgaSoftVersion(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                versionNumber = BitConverter.ToUInt16(Bytes, 0);
                versionLetter = Encoding.ASCII.GetString(Bytes, 2, 1)[0];
            }

            public TgaSoftVersion(ushort VersionNumber, char VersionLetter = ' ')
            {
                versionNumber = VersionNumber;
                versionLetter = VersionLetter;
            }

            public ushort VersionNumber
            {
                get
                {
                    return versionNumber;
                }
                set
                {
                    versionNumber = value;
                }
            }

            public char VersionLetter
            {
                get
                {
                    return versionLetter;
                }
                set
                {
                    versionLetter = value;
                }
            }

            public
            const int Size = 3;

            public TgaSoftVersion Clone()
            {
                return new TgaSoftVersion(versionNumber, versionLetter);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaSoftVersion) ? Equals((TgaSoftVersion)obj) : false);
            }

            public bool Equals(TgaSoftVersion item)
            {
                return (versionNumber == item.versionNumber && versionLetter == item.versionLetter);
            }

            public static bool operator ==(TgaSoftVersion item1, TgaSoftVersion item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaSoftVersion item1, TgaSoftVersion item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                return versionNumber.GetHashCode() ^ versionLetter.GetHashCode();
            }

            public override string ToString()
            {
                return (versionNumber.ToString("000") + versionLetter).TrimEnd(new char[] {
          ' ',
          '\0'
        });
            }

            public byte[] ToBytes()
            {
                return ToBytes(versionNumber, versionLetter);
            }

            public static byte[] ToBytes(ushort VersionNumber, char VersionLetter = ' ')
            {
                return BitConverterExt.ToBytes(VersionNumber, Encoding.ASCII.GetBytes(VersionLetter.ToString()));
            }
        }

        public class TgaString : ICloneable
        {
            public
            const string XFileSignatuteConst = "TRUEVISION-XFILE";
            public
            const string DotSymbolConst = ".";

            string origString = String.Empty;
            int length = 0;
            char blankSpaceChar = DefaultBlankSpaceChar;
            bool useEnding = false;

            public TgaString(bool UseEnding = false)
            {
                useEnding = UseEnding;
            }

            public TgaString(byte[] Bytes, bool UseEnding = false)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");

                length = Bytes.Length;
                useEnding = UseEnding;
                string s = Encoding.ASCII.GetString(Bytes, 0, Bytes.Length - (useEnding ? 1 : 0));

                if (s.Length > 0)
                    switch (s[s.Length - 1])
                    {
                        case '\0':
                        case ' ':
                            blankSpaceChar = s[s.Length - 1];
                            origString = s.TrimEnd(new char[] {
              s[s.Length - 1]
            });
                            break;
                        default:
                            origString = s;
                            break;
                    }
            }

            public TgaString(int Length, bool UseEnding = false)
            {
                length = Length;
                useEnding = UseEnding;
            }

            public TgaString(string Str, int Length, bool UseEnding = false, char BlankSpaceChar = '\0')
            {
                if (Str == null)
                    throw new ArgumentNullException(nameof(Str) + " = null!");

                origString = Str;
                length = Length;
                blankSpaceChar = BlankSpaceChar;
                useEnding = UseEnding;
            }

            public string OriginalString
            {
                get
                {
                    return origString;
                }
                set
                {
                    origString = value;
                }
            }

            public int Length
            {
                get
                {
                    return length;
                }
                set
                {
                    length = value;
                }
            }

            public char BlankSpaceChar
            {
                get
                {
                    return blankSpaceChar;
                }
                set
                {
                    blankSpaceChar = value;
                }
            }

            public bool UseEndingChar
            {
                get
                {
                    return useEnding;
                }
                set
                {
                    useEnding = value;
                }
            }

            public static readonly char DefaultEndingChar = '\0';

            public static readonly char DefaultBlankSpaceChar = '\0';

            public static readonly TgaString Empty = new TgaString();

            public static readonly TgaString ZeroTerminator = new TgaString(true);

            public static readonly TgaString DotSymbol = new TgaString(DotSymbolConst, DotSymbolConst.Length);

            public static readonly TgaString XFileSignatute = new TgaString(XFileSignatuteConst, XFileSignatuteConst.Length);

            public TgaString Clone()
            {
                return new TgaString(origString, length, useEnding, blankSpaceChar);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaString) ? Equals((TgaString)obj) : false);
            }

            public bool Equals(TgaString item)
            {
                return (
                  origString == item.origString &&
                  length == item.length &&
                  blankSpaceChar == item.blankSpaceChar &&
                  useEnding == item.useEnding);
            }

            public static bool operator ==(TgaString item1, TgaString item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaString item1, TgaString item2)
            {
                return !(item1 == item2);
            }

            public static TgaString operator +(TgaString item1, TgaString item2)
            {
                if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
                    throw new ArgumentNullException();

                return new TgaString(BitConverterExt.ToBytes(item1.ToBytes(), item2.ToBytes()));
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + origString.GetHashCode();
                    hash = hash * 23 + length.GetHashCode();
                    hash = hash * 23 + blankSpaceChar.GetHashCode();
                    hash = hash * 23 + useEnding.GetHashCode();
                    return hash;
                }
            }

            public override string ToString()
            {
                return Encoding.ASCII.GetString(ToBytes()).Replace("\0", @"\0");
            }

            public string GetString()
            {
                String Str = Encoding.ASCII.GetString(ToBytes());
                int EndIndex = Str.IndexOf('\0');
                if (EndIndex != -1)
                    Str = Str.Substring(0, EndIndex);
                return Str;
            }

            public byte[] ToBytes()
            {
                return ToBytes(origString, length, useEnding, blankSpaceChar);
            }

            public static byte[] ToBytes(string str, int Length, bool UseEnding = true, char BlankSpaceChar = '\0')
            {
                char[] C = new char[Math.Max(Length, (UseEnding ? 1 : 0))];

                for (int i = 0; i < C.Length; i++)
                    C[i] = (i < str.Length ? str[i] : BlankSpaceChar);

                if (UseEnding)
                    C[C.Length - 1] = DefaultEndingChar;

                return Encoding.ASCII.GetBytes(C);
            }
        }

        public class TgaTime : ICloneable
        {
            ushort hours = 0;
            ushort minutes = 0;
            ushort seconds = 0;

            public TgaTime() { }

            public TgaTime(TimeSpan Time)
            {
                hours = (ushort)Time.TotalHours;
                minutes = (ushort)Time.Minutes;
                seconds = (ushort)Time.Seconds;
            }

            public TgaTime(ushort Hours, ushort Minutes, ushort Seconds)
            {
                hours = Hours;
                minutes = Minutes;
                seconds = Seconds;
            }

            public TgaTime(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                else if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes) + " must be equal " + Size + "!");

                hours = BitConverter.ToUInt16(Bytes, 0);
                minutes = BitConverter.ToUInt16(Bytes, 2);
                seconds = BitConverter.ToUInt16(Bytes, 4);
            }

            public ushort Hours
            {
                get
                {
                    return hours;
                }
                set
                {
                    hours = value;
                }
            }

            public ushort Minutes
            {
                get
                {
                    return minutes;
                }
                set
                {
                    minutes = value;
                }
            }

            public ushort Seconds
            {
                get
                {
                    return seconds;
                }
                set
                {
                    seconds = value;
                }
            }

            public
            const int Size = 6;

            public TgaTime Clone()
            {
                return new TgaTime(hours, minutes, seconds);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaTime) ? Equals((TgaTime)obj) : false);
            }

            public bool Equals(TgaTime item)
            {
                return (hours == item.hours && minutes == item.minutes && seconds == item.seconds);
            }

            public static bool operator ==(TgaTime item1, TgaTime item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaTime item1, TgaTime item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + hours.GetHashCode();
                    hash = hash * 23 + (minutes << 16 | seconds).GetHashCode();
                    return hash;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}:{1}:{2}", hours, minutes, seconds);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(hours, minutes, seconds);
            }

            public TimeSpan ToTimeSpan()
            {
                return new TimeSpan(hours, minutes, seconds);
            }
        }

        public class TgaHeader : ICloneable
        {
            byte idLength = 0;
            TgaColorMapType colorMapType = TgaColorMapType.NoColorMap;
            TgaImageType imageType = TgaImageType.NoImageData;
            TgaColorMapSpec colorMapSpec = new TgaColorMapSpec();
            TgaImageSpec imageSpec = new TgaImageSpec();

            public TgaHeader() { }

            public TgaHeader(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                idLength = Bytes[0];
                colorMapType = (TgaColorMapType)Bytes[1];
                imageType = (TgaImageType)Bytes[2];
                colorMapSpec = new TgaColorMapSpec(BitConverterExt.GetElements(Bytes, 3, TgaColorMapSpec.Size));
                imageSpec = new TgaImageSpec(BitConverterExt.GetElements(Bytes, 8, TgaImageSpec.Size));
            }

            public byte IDLength
            {
                get
                {
                    return idLength;
                }
                set
                {
                    idLength = value;
                }
            }

            public TgaColorMapType ColorMapType
            {
                get
                {
                    return colorMapType;
                }
                set
                {
                    colorMapType = value;
                }
            }

            public TgaImageType ImageType
            {
                get
                {
                    return imageType;
                }
                set
                {
                    imageType = value;
                }
            }

            public TgaColorMapSpec ColorMapSpec
            {
                get
                {
                    return colorMapSpec;
                }
                set
                {
                    colorMapSpec = value;
                }
            }

            public TgaImageSpec ImageSpec
            {
                get
                {
                    return imageSpec;
                }
                set
                {
                    imageSpec = value;
                }
            }

            public
            const int Size = 18;

            public TgaHeader Clone()
            {
                return new TgaHeader(ToBytes());
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaHeader) ? Equals((TgaHeader)obj) : false);
            }

            public bool Equals(TgaHeader item)
            {
                return (idLength == item.idLength &&
                  colorMapType == item.colorMapType &&
                  imageType == item.imageType &&
                  colorMapSpec == item.colorMapSpec &&
                  imageSpec == item.imageSpec);
            }

            public static bool operator ==(TgaHeader item1, TgaHeader item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaHeader item1, TgaHeader item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + (idLength << 24 | (byte)colorMapType << 8 | (byte)imageType).GetHashCode();

                    if (colorMapSpec != null)
                        hash = hash * 23 + colorMapSpec.GetHashCode();

                    if (imageSpec != null)
                        hash = hash * 23 + imageSpec.GetHashCode();

                    return hash;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}={1}, {2}={3}, {4}={5}, {6}={7}, {8}={9}",
                  nameof(IDLength), idLength,
                  nameof(ColorMapType), colorMapType,
                  nameof(ImageType), imageType,
                  nameof(ColorMapSpec), colorMapSpec,
                  nameof(ImageSpec), imageSpec);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(idLength, (byte)colorMapType, (byte)imageType,
                  (colorMapSpec == null ? new byte[TgaColorMapSpec.Size] : colorMapSpec.ToBytes()),
                  (imageSpec == null ? new byte[TgaImageSpec.Size] : imageSpec.ToBytes()));
            }
        }

        public class TgaImgOrColMap : ICloneable
        {
            TgaString imageID = null;
            byte[] colorMapData = null;
            byte[] imageData = null;

            public TgaImgOrColMap() { }

            public TgaImgOrColMap(TgaString ImageID, byte[] ColorMapData, byte[] ImageData)
            {
                imageID = ImageID;
                colorMapData = ColorMapData;
                imageData = ImageData;
            }

            public TgaString ImageID
            {
                get
                {
                    return imageID;
                }
                set
                {
                    imageID = value;
                }
            }

            public byte[] ColorMapData
            {
                get
                {
                    return colorMapData;
                }
                set
                {
                    colorMapData = value;
                }
            }

            public byte[] ImageData
            {
                get
                {
                    return imageData;
                }
                set
                {
                    imageData = value;
                }
            }

            public TgaImgOrColMap Clone()
            {
                return new TgaImgOrColMap(
                  (imageID == null ? null : imageID.Clone()),
                  (colorMapData == null ? null : (byte[])colorMapData.Clone()),
                  (imageData == null ? null : (byte[])imageData.Clone()));
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaImgOrColMap) ? Equals((TgaImgOrColMap)obj) : false);
            }

            public bool Equals(TgaImgOrColMap item)
            {
                return imageID == item.imageID &&
                  BitConverterExt.IsArraysEqual(colorMapData, item.colorMapData) &&
                  BitConverterExt.IsArraysEqual(imageData, item.imageData);
            }

            public static bool operator ==(TgaImgOrColMap item1, TgaImgOrColMap item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaImgOrColMap item1, TgaImgOrColMap item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 27;

                    if (imageID != null)
                        hash = (13 * hash) + imageID.GetHashCode();
                    if (colorMapData != null)
                        for (int i = 0; i < colorMapData.Length; i++)
                            hash = (13 * hash) + colorMapData[i].GetHashCode();
                    if (imageData != null)
                        for (int i = 0; i < imageData.Length; i++)
                            hash = (13 * hash) + imageData[i].GetHashCode();

                    return hash;
                }
            }
        }
        public class TgaDevArea : ICloneable
        {
            List<TgaDevEntry> entries = new List<TgaDevEntry>();

            public TgaDevArea() { }

            public TgaDevArea(List<TgaDevEntry> Entries)
            {
                if (Entries == null)
                    throw new ArgumentNullException(nameof(Entries) + " = null!");

                entries = Entries;
            }

            public List<TgaDevEntry> Entries
            {
                get
                {
                    return entries;
                }
                set
                {
                    entries = value;
                }
            }

            public int Count
            {
                get
                {
                    return entries.Count;
                }
            }

            public TgaDevEntry this[int index]
            {
                get
                {
                    return entries[index];
                }
                set
                {
                    entries[index] = value;
                }
            }

            public TgaDevArea Clone()
            {
                if (entries == null)
                    return new TgaDevArea(null);

                List<TgaDevEntry> L = new List<TgaDevEntry>();
                for (int i = 0; i < entries.Count; i++)
                    L.Add(entries[i].Clone());

                return new TgaDevArea(L);
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaDevArea) ? Equals((TgaDevArea)obj) : false);
            }

            public bool Equals(TgaDevArea item)
            {
                return BitConverterExt.IsListsEqual(entries, item.entries);
            }

            public static bool operator ==(TgaDevArea item1, TgaDevArea item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaDevArea item1, TgaDevArea item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 27;
                    if (entries != null)
                        for (int i = 0; i < entries.Count; i++)
                            hash = (13 * hash) + entries[i].GetHashCode();
                    return hash;
                }
            }

            public byte[] ToBytes()
            {
                if (entries == null)
                    throw new Exception(nameof(Entries) + " = null!");

                ushort NumberOfEntries = (ushort)Math.Min(ushort.MaxValue, entries.Count);
                List<byte> DevDir = new List<byte>(BitConverter.GetBytes(NumberOfEntries));

                for (int i = 0; i < entries.Count; i++)
                {
                    DevDir.AddRange(BitConverter.GetBytes(entries[i].Tag));
                    DevDir.AddRange(BitConverter.GetBytes(entries[i].Offset));
                    DevDir.AddRange(BitConverter.GetBytes(entries[i].FieldSize));
                }

                return DevDir.ToArray();
            }
        }
        public class TgaExtArea : ICloneable
        {
            public
            const int MinSize = 495;
            ushort extensionSize = MinSize;
            TgaString authorName = new TgaString(41, true);
            TgaComment authorComments = new TgaComment();
            TgaDateTime dateTimeStamp = new TgaDateTime();
            TgaString jobNameOrID = new TgaString(41, true);
            TgaTime jobTime = new TgaTime();
            TgaString softwareID = new TgaString(41, true);
            TgaSoftVersion softVersion = new TgaSoftVersion();
            TgaColorKey keyColor = new TgaColorKey();
            TgaFraction pixelAspectRatio = TgaFraction.Empty;
            TgaFraction gammaValue = TgaFraction.Empty;
            uint colorCorrectionOffset = 0;
            uint postageStampOffset = 0;
            uint scanLineOffset = 0;
            TgaAttrType attributesType = TgaAttrType.NoAlpha;
            uint[] scanLineTable = null;
            TgaPostageStampImage postageStampImage = null;
            ushort[] colorCorrectionTable = null;
            byte[] otherDataInExtensionArea = null;

            public TgaExtArea() { }

            public TgaExtArea(byte[] Bytes, uint[] SLT = null, TgaPostageStampImage PostImg = null, ushort[] CCT = null)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length < MinSize)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be >= " + MinSize + "!");

                extensionSize = BitConverter.ToUInt16(Bytes, 0);
                authorName = new TgaString(BitConverterExt.GetElements(Bytes, 2, 41), true);
                authorComments = new TgaComment(BitConverterExt.GetElements(Bytes, 43, TgaComment.Size));
                dateTimeStamp = new TgaDateTime(BitConverterExt.GetElements(Bytes, 367, TgaDateTime.Size));
                jobNameOrID = new TgaString(BitConverterExt.GetElements(Bytes, 379, 41), true);
                jobTime = new TgaTime(BitConverterExt.GetElements(Bytes, 420, TgaTime.Size));
                softwareID = new TgaString(BitConverterExt.GetElements(Bytes, 426, 41), true);
                softVersion = new TgaSoftVersion(BitConverterExt.GetElements(Bytes, 467, TgaSoftVersion.Size));
                keyColor = new TgaColorKey(BitConverterExt.GetElements(Bytes, 470, TgaColorKey.Size));
                pixelAspectRatio = new TgaFraction(BitConverterExt.GetElements(Bytes, 474, TgaFraction.Size));
                gammaValue = new TgaFraction(BitConverterExt.GetElements(Bytes, 478, TgaFraction.Size));
                colorCorrectionOffset = BitConverter.ToUInt32(Bytes, 482);
                postageStampOffset = BitConverter.ToUInt32(Bytes, 486);
                scanLineOffset = BitConverter.ToUInt32(Bytes, 490);
                attributesType = (TgaAttrType)Bytes[494];

                if (extensionSize > MinSize)
                    otherDataInExtensionArea = BitConverterExt.GetElements(Bytes, 495, Bytes.Length - MinSize);

                scanLineTable = SLT;
                postageStampImage = PostImg;
                colorCorrectionTable = CCT;
            }

            #region Properties
            public ushort ExtensionSize
            {
                get
                {
                    return extensionSize;
                }
                set
                {
                    extensionSize = value;
                }
            }

            public TgaString AuthorName
            {
                get
                {
                    return authorName;
                }
                set
                {
                    authorName = value;
                }
            }

            public TgaComment AuthorComments
            {
                get
                {
                    return authorComments;
                }
                set
                {
                    authorComments = value;
                }
            }

            public TgaDateTime DateTimeStamp
            {
                get
                {
                    return dateTimeStamp;
                }
                set
                {
                    dateTimeStamp = value;
                }
            }

            public TgaString JobNameOrID
            {
                get
                {
                    return jobNameOrID;
                }
                set
                {
                    jobNameOrID = value;
                }
            }

            public TgaTime JobTime
            {
                get
                {
                    return jobTime;
                }
                set
                {
                    jobTime = value;
                }
            }

            public TgaString SoftwareID
            {
                get
                {
                    return softwareID;
                }
                set
                {
                    softwareID = value;
                }
            }

            public TgaSoftVersion SoftVersion
            {
                get
                {
                    return softVersion;
                }
                set
                {
                    softVersion = value;
                }
            }

            public TgaColorKey KeyColor
            {
                get
                {
                    return keyColor;
                }
                set
                {
                    keyColor = value;
                }
            }

            public TgaFraction PixelAspectRatio
            {
                get
                {
                    return pixelAspectRatio;
                }
                set
                {
                    pixelAspectRatio = value;
                }
            }

            public TgaFraction GammaValue
            {
                get
                {
                    return gammaValue;
                }
                set
                {
                    gammaValue = value;
                }
            }

            public uint ColorCorrectionTableOffset
            {
                get
                {
                    return colorCorrectionOffset;
                }
                set
                {
                    colorCorrectionOffset = value;
                }
            }

            public uint PostageStampOffset
            {
                get
                {
                    return postageStampOffset;
                }
                set
                {
                    postageStampOffset = value;
                }
            }

            public uint ScanLineOffset
            {
                get
                {
                    return scanLineOffset;
                }
                set
                {
                    scanLineOffset = value;
                }
            }

            public TgaAttrType AttributesType
            {
                get
                {
                    return attributesType;
                }
                set
                {
                    attributesType = value;
                }
            }

            public uint[] ScanLineTable
            {
                get
                {
                    return scanLineTable;
                }
                set
                {
                    scanLineTable = value;
                }
            }

            public TgaPostageStampImage PostageStampImage
            {
                get
                {
                    return postageStampImage;
                }
                set
                {
                    postageStampImage = value;
                }
            }

            public ushort[] ColorCorrectionTable
            {
                get
                {
                    return colorCorrectionTable;
                }
                set
                {
                    colorCorrectionTable = value;
                }
            }

            public byte[] OtherDataInExtensionArea
            {
                get
                {
                    return otherDataInExtensionArea;
                }
                set
                {
                    otherDataInExtensionArea = value;
                }
            }
            #endregion

            public TgaExtArea Clone()
            {
                TgaExtArea NewExtArea = new TgaExtArea();
                NewExtArea.extensionSize = extensionSize;
                NewExtArea.authorName = authorName.Clone();
                NewExtArea.authorComments = authorComments.Clone();
                NewExtArea.dateTimeStamp = dateTimeStamp.Clone();
                NewExtArea.jobNameOrID = jobNameOrID.Clone();
                NewExtArea.jobTime = jobTime.Clone();
                NewExtArea.softwareID = softwareID.Clone();
                NewExtArea.softVersion = softVersion.Clone();
                NewExtArea.keyColor = keyColor.Clone();
                NewExtArea.pixelAspectRatio = pixelAspectRatio.Clone();
                NewExtArea.gammaValue = gammaValue.Clone();
                NewExtArea.colorCorrectionOffset = colorCorrectionOffset;
                NewExtArea.postageStampOffset = postageStampOffset;
                NewExtArea.scanLineOffset = scanLineOffset;
                NewExtArea.attributesType = attributesType;

                if (scanLineTable != null)
                    NewExtArea.scanLineTable = (uint[])scanLineTable.Clone();
                if (postageStampImage != null)
                    NewExtArea.postageStampImage = new TgaPostageStampImage(postageStampImage.ToBytes());
                if (colorCorrectionTable != null)
                    NewExtArea.colorCorrectionTable = (ushort[])colorCorrectionTable.Clone();

                if (otherDataInExtensionArea != null)
                    NewExtArea.otherDataInExtensionArea = (byte[])otherDataInExtensionArea.Clone();

                return NewExtArea;
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public override bool Equals(object obj)
            {
                return ((obj is TgaExtArea) ? Equals((TgaExtArea)obj) : false);
            }

            public bool Equals(TgaExtArea item)
            {
                return (extensionSize == item.extensionSize &&
                  authorName == item.authorName &&
                  authorComments == item.authorComments &&
                  dateTimeStamp == item.dateTimeStamp &&
                  jobNameOrID == item.jobNameOrID &&
                  jobTime == item.jobTime &&
                  softwareID == item.softwareID &&
                  softVersion == item.softVersion &&
                  keyColor == item.keyColor &&
                  pixelAspectRatio == item.pixelAspectRatio &&
                  gammaValue == item.gammaValue &&
                  colorCorrectionOffset == item.colorCorrectionOffset &&
                  postageStampOffset == item.postageStampOffset &&
                  scanLineOffset == item.scanLineOffset &&
                  attributesType == item.attributesType &&

                  BitConverterExt.IsArraysEqual(scanLineTable, item.scanLineTable) &&
                  postageStampImage == item.postageStampImage &&
                  BitConverterExt.IsArraysEqual(colorCorrectionTable, item.colorCorrectionTable) &&

                  BitConverterExt.IsArraysEqual(otherDataInExtensionArea, item.otherDataInExtensionArea));
            }

            public static bool operator ==(TgaExtArea item1, TgaExtArea item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaExtArea item1, TgaExtArea item2)
            {
                return !(item1 == item2);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 27;
                    hash = (13 * hash) + extensionSize.GetHashCode();
                    hash = (13 * hash) + authorName.GetHashCode();
                    hash = (13 * hash) + authorComments.GetHashCode();
                    hash = (13 * hash) + dateTimeStamp.GetHashCode();
                    hash = (13 * hash) + jobNameOrID.GetHashCode();
                    hash = (13 * hash) + jobTime.GetHashCode();
                    hash = (13 * hash) + softwareID.GetHashCode();
                    hash = (13 * hash) + softVersion.GetHashCode();
                    hash = (13 * hash) + keyColor.GetHashCode();
                    hash = (13 * hash) + pixelAspectRatio.GetHashCode();
                    hash = (13 * hash) + gammaValue.GetHashCode();
                    hash = (13 * hash) + colorCorrectionOffset.GetHashCode();
                    hash = (13 * hash) + postageStampOffset.GetHashCode();
                    hash = (13 * hash) + scanLineOffset.GetHashCode();
                    hash = (13 * hash) + attributesType.GetHashCode();

                    if (scanLineTable != null)
                        for (int i = 0; i < scanLineTable.Length; i++)
                            hash = (13 * hash) + scanLineTable[i].GetHashCode();

                    if (postageStampImage != null)
                        hash = (13 * hash) + postageStampImage.GetHashCode();

                    if (colorCorrectionTable != null)
                        for (int i = 0; i < colorCorrectionTable.Length; i++)
                            hash = (13 * hash) + colorCorrectionTable[i].GetHashCode();

                    if (otherDataInExtensionArea != null)
                        for (int i = 0; i < otherDataInExtensionArea.Length; i++)
                            hash = (13 * hash) + otherDataInExtensionArea[i].GetHashCode();

                    return hash;
                }
            }

            public byte[] ToBytes()
            {
                #region Exceptions check
                if (authorName == null)
                    authorName = new TgaString(41, true);

                if (authorComments == null)
                    authorComments = new TgaComment();

                if (dateTimeStamp == null)
                    dateTimeStamp = new TgaDateTime(DateTime.UtcNow);

                if (jobNameOrID == null)
                    jobNameOrID = new TgaString(41, true);

                if (jobTime == null)
                    jobTime = new TgaTime();

                if (softwareID == null)
                    softwareID = new TgaString(41, true);

                if (softVersion == null)
                    softVersion = new TgaSoftVersion();

                if (keyColor == null)
                    keyColor = new TgaColorKey();

                if (pixelAspectRatio == null)
                    pixelAspectRatio = new TgaFraction();

                if (gammaValue == null)
                    gammaValue = new TgaFraction();
                #endregion

                return BitConverterExt.ToBytes(
                  extensionSize,
                  authorName.ToBytes(),
                  authorComments.ToBytes(),
                  dateTimeStamp.ToBytes(),
                  jobNameOrID.ToBytes(),
                  jobTime.ToBytes(),
                  softwareID.ToBytes(),
                  softVersion.ToBytes(),
                  keyColor.ToBytes(),
                  pixelAspectRatio.ToBytes(),
                  gammaValue.ToBytes(),
                  colorCorrectionOffset,
                  postageStampOffset,
                  scanLineOffset,
                  (byte)attributesType,
                  otherDataInExtensionArea);
            }
        }
        public class TgaFooter : ICloneable
        {
            uint extAreaOffset = 0;
            uint devDirOffset = 0;
            TgaString signature = TgaString.XFileSignatute;
            TgaString reservedChar = TgaString.DotSymbol;
            TgaString zeroStrTerminator = TgaString.ZeroTerminator;

            public TgaFooter() { }

            public TgaFooter(uint ExtOff, uint DevDirOff, TgaString Sign, TgaString ReservChr, TgaString Termin)
            {
                extAreaOffset = ExtOff;
                devDirOffset = DevDirOff;
                signature = Sign;
                reservedChar = ReservChr;
                zeroStrTerminator = Termin;
            }

            public TgaFooter(byte[] Bytes)
            {
                if (Bytes == null)
                    throw new ArgumentNullException(nameof(Bytes) + " = null!");
                if (Bytes.Length != Size)
                    throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

                extAreaOffset = BitConverter.ToUInt32(Bytes, 0);
                devDirOffset = BitConverter.ToUInt32(Bytes, 4);
                signature = new TgaString(BitConverterExt.GetElements(Bytes, 8, TgaString.XFileSignatuteConst.Length));
                reservedChar = new TgaString(new byte[] {
          Bytes[24]
        });
                zeroStrTerminator = new TgaString(new byte[] {
          Bytes[25]
        });
            }

            public uint ExtensionAreaOffset
            {
                get
                {
                    return extAreaOffset;
                }
                set
                {
                    extAreaOffset = value;
                }
            }

            public uint DeveloperDirectoryOffset
            {
                get
                {
                    return devDirOffset;
                }
                set
                {
                    devDirOffset = value;
                }
            }

            public TgaString Signature
            {
                get
                {
                    return signature;
                }
                set
                {
                    signature = value;
                }
            }

            public TgaString ReservedCharacter
            {
                get
                {
                    return reservedChar;
                }
                set
                {
                    reservedChar = value;
                }
            }

            public TgaString BinaryZeroStringTerminator
            {
                get
                {
                    return zeroStrTerminator;
                }
                set
                {
                    zeroStrTerminator = value;
                }
            }

            public TgaFooter Clone()
            {
                return new TgaFooter(extAreaOffset, devDirOffset, signature.Clone(),
                  reservedChar.Clone(), zeroStrTerminator.Clone());
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public
            const int Size = 26;

            public override bool Equals(object obj)
            {
                return ((obj is TgaFooter) ? Equals((TgaFooter)obj) : false);
            }

            public bool Equals(TgaFooter item)
            {
                return (extAreaOffset == item.extAreaOffset &&
                  devDirOffset == item.devDirOffset &&
                  signature == item.signature &&
                  reservedChar == item.reservedChar &&
                  zeroStrTerminator == item.zeroStrTerminator);
            }

            public static bool operator ==(TgaFooter item1, TgaFooter item2)
            {
                if (ReferenceEquals(item1, null))
                    return ReferenceEquals(item2, null);

                if (ReferenceEquals(item2, null))
                    return ReferenceEquals(item1, null);

                return item1.Equals(item2);
            }

            public static bool operator !=(TgaFooter item1, TgaFooter item2)
            {
                return !(item1 == item2);
            }

            public byte[] ToBytes()
            {
                return BitConverterExt.ToBytes(extAreaOffset, devDirOffset,
                  signature.ToBytes(), reservedChar.ToBytes(), zeroStrTerminator.ToBytes());
            }

            public bool IsFooterCorrect
            {
                get
                {
                    return signature == TgaString.XFileSignatute;
                }
            }
        }

        public static class BitConverterExt
        {
            public static byte[] ToBytes(params object[] obj)
            {
                if (obj == null)
                    return null;

                List<byte> BytesList = new List<byte>();

                for (int i = 0; i < obj.Length; i++)
                {
                    if (obj[i] == null)
                        continue;
                    else if (obj[i] is byte)
                        BytesList.Add((byte)obj[i]);
                    else if (obj[i] is byte[])
                        BytesList.AddRange((byte[])obj[i]);
                    else if (obj[i] is short)
                        BytesList.AddRange(BitConverter.GetBytes((short)obj[i]));
                    else if (obj[i] is ushort)
                        BytesList.AddRange(BitConverter.GetBytes((ushort)obj[i]));
                    else if (obj[i] is int)
                        BytesList.AddRange(BitConverter.GetBytes((int)obj[i]));
                    else if (obj[i] is uint)
                        BytesList.AddRange(BitConverter.GetBytes((uint)obj[i]));
                    else if (obj[i] is long)
                        BytesList.AddRange(BitConverter.GetBytes((long)obj[i]));
                    else if (obj[i] is ulong)
                        BytesList.AddRange(BitConverter.GetBytes((ulong)obj[i]));
                }
                return BytesList.ToArray();
            }

            public static T[] GetElements<T>(T[] SrcArray, int Offset, int Count)
            {
                if (SrcArray == null)
                    throw new ArgumentNullException(nameof(SrcArray) + " is null!");

                if (Offset >= SrcArray.Length || Offset < 0)
                    throw new ArgumentOutOfRangeException(nameof(Offset) + " has wrong value!");

                if (Count <= 0 || Offset + Count > SrcArray.Length)
                    throw new ArgumentOutOfRangeException(nameof(Count) + " has wrong value!");

                T[] Buff = new T[Count];
                Array.Copy(SrcArray, Offset, Buff, 0, Buff.Length);
                return Buff;
            }

            public static bool IsArraysEqual<T>(T[] item1, T[] item2)
            {
                if (ReferenceEquals(item1, item2))
                    return true;

                if (item1 == null || item2 == null)
                    return false;

                if (item1.Length != item2.Length)
                    return false;

                EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                for (int i = 0; i < item1.Length; i++)
                    if (!comparer.Equals(item1[i], item2[i]))
                        return false;
                return true;
            }

            public static bool IsListsEqual<T>(List<T> item1, List<T> item2)
            {
                if (ReferenceEquals(item1, item2))
                    return true;

                if (item1 == null || item2 == null)
                    return false;

                if (item1.Count != item2.Count)
                    return false;

                for (int i = 0; i < item1.Count; i++)
                    if (!item1[i].Equals(item2[i]))
                        return false;
                return true;
            }

            public static bool IsElementsEqual<T>(T[] Arr, int Offset1, int Offset2, int Count)
            {
                if (Arr == null)
                    throw new ArgumentNullException(nameof(Arr) + " is null!");

                if (Offset1 >= Arr.Length || Offset1 < 0)
                    throw new ArgumentOutOfRangeException(nameof(Offset1) + " has wrong value!");

                if (Offset2 >= Arr.Length || Offset2 < 0)
                    throw new ArgumentOutOfRangeException(nameof(Offset2) + " has wrong value!");

                if (Count <= 0 || Offset1 + Count > Arr.Length || Offset2 + Count > Arr.Length)
                    throw new ArgumentOutOfRangeException(nameof(Count) + " has wrong value!");

                if (Offset1 == Offset2)
                    return true;

                for (int i = 0; i < Count; i++)
                    if (!Arr[Offset1 + i].Equals(Arr[Offset2 + i]))
                        return false;

                return true;
            }
        }
        #endregion

        public class TGA
        {
            public TgaHeader Header = new TgaHeader();
            public TgaImgOrColMap ImageOrColorMapArea = new TgaImgOrColMap();
            public TgaDevArea DevArea = null;
            public TgaExtArea ExtArea = null;
            public TgaFooter Footer = null;

            #region TGA Creation, Loading, Saving(all are public, have reference to private metods).

            public TGA(TGA tga)
            {
                Header = tga.Header.Clone();
                ImageOrColorMapArea = tga.ImageOrColorMapArea.Clone();
                DevArea = tga.DevArea.Clone();
                ExtArea = tga.ExtArea.Clone();
                Footer = tga.Footer.Clone();
            }

            public TGA(string filename)
            {
                LoadFunc(filename);
            }
            public TGA(Stream stream)
            {
                LoadFunc(stream);
            }

            public TGA(Bitmap bmp, bool UseRLE = false, bool NewFormat = false, bool ColorMap2BytesEntry = false)
            {
                LoadFunc(bmp, UseRLE, NewFormat, ColorMap2BytesEntry);
            }

            public static TGA FromFile(string filename)
            {
                return new TGA(filename);
            }

            public static TGA FromStream(Stream stream)
            {
                return new TGA(stream);
            }

            public static TGA FromBitmap(Bitmap bmp, bool UseRLE = false,
              bool NewFormat = true, bool ColorMap2BytesEntry = false)
            {
                return new TGA(bmp, UseRLE, NewFormat, ColorMap2BytesEntry);
            }

            public bool Save(string filename)
            {
                try
                {
                    bool Result = false;
                    using (FileStream Fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (MemoryStream Ms = new MemoryStream())
                        {
                            Result = SaveFunc(Ms);
                            Ms.WriteTo(Fs);
                            Fs.Flush();
                        }
                    }
                    return Result;
                }
                catch
                {
                    return false;
                }
            }

            public bool Save(Stream stream)
            {
                return SaveFunc(stream);
            }
            #endregion

            public ushort Width
            {
                get
                {
                    return Header.ImageSpec.ImageWidth;
                }
                set
                {
                    Header.ImageSpec.ImageWidth = value;
                }
            }

            public ushort Height
            {
                get
                {
                    return Header.ImageSpec.ImageHeight;
                }
                set
                {
                    Header.ImageSpec.ImageHeight = value;
                }
            }

            public TGA Clone()
            {
                return new TGA(this);
            }

            public bool CheckAndUpdateOffsets(out string ErrorStr)
            {
                ErrorStr = String.Empty;

                if (Header == null)
                {
                    ErrorStr = "Header = null";
                    return false;
                }

                if (ImageOrColorMapArea == null)
                {
                    ErrorStr = "ImageOrColorMapArea = null";
                    return false;
                }

                uint Offset = TgaHeader.Size;
                #region Header
                if (ImageOrColorMapArea.ImageID != null)
                {
                    int StrMaxLen = 255;
                    if (ImageOrColorMapArea.ImageID.UseEndingChar)
                        StrMaxLen--;

                    Header.IDLength = (byte)Math.Min(ImageOrColorMapArea.ImageID.OriginalString.Length, StrMaxLen);
                    ImageOrColorMapArea.ImageID.Length = Header.IDLength;
                    Offset += Header.IDLength;
                }
                else
                    Header.IDLength = 0;
                #endregion

                #region ColorMap
                if (Header.ColorMapType != TgaColorMapType.NoColorMap)
                {
                    if (Header.ColorMapSpec == null)
                    {
                        ErrorStr = "Header.ColorMapSpec = null";
                        return false;
                    }

                    if (Header.ColorMapSpec.ColorMapLength == 0)
                    {
                        ErrorStr = "Header.ColorMapSpec.ColorMapLength = 0";
                        return false;
                    }

                    if (ImageOrColorMapArea.ColorMapData == null)
                    {
                        ErrorStr = "ImageOrColorMapArea.ColorMapData = null";
                        return false;
                    }

                    int CmBytesPerPixel = (int)Math.Ceiling((double)Header.ColorMapSpec.ColorMapEntrySize / 8.0);
                    int LenBytes = Header.ColorMapSpec.ColorMapLength * CmBytesPerPixel;

                    if (LenBytes != ImageOrColorMapArea.ColorMapData.Length)
                    {
                        ErrorStr = "ImageOrColorMapArea.ColorMapData.Length has wrong size!";
                        return false;
                    }

                    Offset += (uint)ImageOrColorMapArea.ColorMapData.Length;
                }
                #endregion

                #region Image Data
                int BytesPerPixel = 0;
                if (Header.ImageType != TgaImageType.NoImageData)
                {
                    if (Header.ImageSpec == null)
                    {
                        ErrorStr = "Header.ImageSpec = null";
                        return false;
                    }

                    if (Header.ImageSpec.ImageWidth == 0 || Header.ImageSpec.ImageHeight == 0)
                    {
                        ErrorStr = "Header.ImageSpec.ImageWidth = 0 or Header.ImageSpec.ImageHeight = 0";
                        return false;
                    }

                    if (ImageOrColorMapArea.ImageData == null)
                    {
                        ErrorStr = "ImageOrColorMapArea.ImageData = null";
                        return false;
                    }

                    BytesPerPixel = (int)Math.Ceiling((double)Header.ImageSpec.PixelDepth / 8.0);
                    if (Width * Height * BytesPerPixel != ImageOrColorMapArea.ImageData.Length)
                    {
                        ErrorStr = "ImageOrColorMapArea.ImageData.Length has wrong size!";
                        return false;
                    }

                    if (Header.ImageType >= TgaImageType.RLE_ColorMapped &&
                      Header.ImageType <= TgaImageType.RLE_BlackWhite)
                    {
                        byte[] RLE = RLE_Encode(ImageOrColorMapArea.ImageData, Width, Height);
                        if (RLE == null)
                        {
                            ErrorStr = "RLE Compressing error! Check Image Data size.";
                            return false;
                        }

                        Offset += (uint)RLE.Length;
                        RLE = null;
                    }
                    else
                        Offset += (uint)ImageOrColorMapArea.ImageData.Length;
                }
                #endregion

                #region Footer, DevArea, ExtArea
                if (Footer != null)
                {
                    #region DevArea
                    if (DevArea != null)
                    {
                        int DevAreaCount = DevArea.Count;
                        for (int i = 0; i < DevAreaCount; i++)
                            if (DevArea[i] == null || DevArea[i].FieldSize <= 0)
                            {
                                DevArea.Entries.RemoveAt(i);
                                DevAreaCount--;
                                i--;
                            }

                        if (DevArea.Count <= 0)
                            Footer.DeveloperDirectoryOffset = 0;

                        if (DevArea.Count > 2)
                        {
                            DevArea.Entries.Sort((a, b) => {
                                return a.Tag.CompareTo(b.Tag);
                            });
                            for (int i = 0; i < DevArea.Count - 1; i++)
                                if (DevArea[i].Tag == DevArea[i + 1].Tag)
                                {
                                    ErrorStr = "DevArea Enties has same Tags!";
                                    return false;
                                }
                        }

                        for (int i = 0; i < DevArea.Count; i++)
                        {
                            DevArea[i].Offset = Offset;
                            Offset += (uint)DevArea[i].FieldSize;
                        }

                        Footer.DeveloperDirectoryOffset = Offset;
                        Offset += (uint)(DevArea.Count * 10 + 2);
                    }
                    else
                        Footer.DeveloperDirectoryOffset = 0;
                    #endregion

                    #region ExtArea
                    if (ExtArea != null)
                    {
                        ExtArea.ExtensionSize = TgaExtArea.MinSize;
                        if (ExtArea.OtherDataInExtensionArea != null)
                            ExtArea.ExtensionSize += (ushort)ExtArea.OtherDataInExtensionArea.Length;

                        ExtArea.DateTimeStamp = new TgaDateTime(DateTime.UtcNow);

                        Footer.ExtensionAreaOffset = Offset;
                        Offset += ExtArea.ExtensionSize;

                        #region ScanLineTable
                        if (ExtArea.ScanLineTable == null)
                            ExtArea.ScanLineOffset = 0;
                        else
                        {
                            if (ExtArea.ScanLineTable.Length != Height)
                            {
                                ErrorStr = "ExtArea.ScanLineTable.Length != Height";
                                return false;
                            }

                            ExtArea.ScanLineOffset = Offset;
                            Offset += (uint)(ExtArea.ScanLineTable.Length * 4);
                        }
                        #endregion

                        #region PostageStampImage
                        if (ExtArea.PostageStampImage == null)
                            ExtArea.PostageStampOffset = 0;
                        else
                        {
                            if (ExtArea.PostageStampImage.Width == 0 || ExtArea.PostageStampImage.Height == 0)
                            {
                                ErrorStr = "ExtArea.PostageStampImage Width or Height is equal 0!";
                                return false;
                            }

                            if (ExtArea.PostageStampImage.Data == null)
                            {
                                ErrorStr = "ExtArea.PostageStampImage.Data == null";
                                return false;
                            }

                            int PImgSB = ExtArea.PostageStampImage.Width * ExtArea.PostageStampImage.Height * BytesPerPixel;
                            if (Header.ImageType != TgaImageType.NoImageData &&
                              ExtArea.PostageStampImage.Data.Length != PImgSB)
                            {
                                ErrorStr = "ExtArea.PostageStampImage.Data.Length is wrong!";
                                return false;
                            }

                            ExtArea.PostageStampOffset = Offset;
                            Offset += (uint)(ExtArea.PostageStampImage.Data.Length);
                        }
                        #endregion

                        #region ColorCorrectionTable
                        if (ExtArea.ColorCorrectionTable == null)
                            ExtArea.ColorCorrectionTableOffset = 0;
                        else
                        {
                            if (ExtArea.ColorCorrectionTable.Length != 1024)
                            {
                                ErrorStr = "ExtArea.ColorCorrectionTable.Length != 256 * 4";
                                return false;
                            }

                            ExtArea.ColorCorrectionTableOffset = Offset;
                            Offset += (uint)(ExtArea.ColorCorrectionTable.Length * 2);
                        }
                        #endregion
                    }
                    else
                        Footer.ExtensionAreaOffset = 0;
                    #endregion

                    #region Footer
                    if (Footer.ToBytes().Length != TgaFooter.Size)
                    {
                        ErrorStr = "Footer.Length is wrong!";
                        return false;
                    }

                    Offset += TgaFooter.Size;
                    #endregion
                }
                #endregion

                return true;
            }

            #region Convert
            public Bitmap ToBitmap(bool ForceUseAlpha = false)
            {
                return ToBitmapFunc(ForceUseAlpha, false);
            }
            #endregion

            #region Private functions
            bool LoadFunc(string filename)
            {
                if (!File.Exists(filename))
                    throw new FileNotFoundException("File: \"" + filename + "\" not found!");

                try
                {
                    using (FileStream FS = new FileStream(filename, FileMode.Open))
                        return LoadFunc(FS);
                }
                catch
                {
                    return false;
                }
            }

            bool LoadFunc(byte[] bytes)
            {
                if (bytes == null)
                    throw new ArgumentNullException();

                try
                {
                    using (MemoryStream FS = new MemoryStream(bytes, false))
                        return LoadFunc(FS);
                }
                catch
                {
                    return false;
                }
            }

            bool LoadFunc(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException();
                if (!(stream.CanRead && stream.CanSeek))
                    throw new FileLoadException("Stream reading or seeking is not avaiable!");

                try
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    BinaryReader Br = new BinaryReader(stream);

                    Header = new TgaHeader(Br.ReadBytes(TgaHeader.Size));

                    if (Header.IDLength > 0)
                        ImageOrColorMapArea.ImageID = new TgaString(Br.ReadBytes(Header.IDLength));

                    if (Header.ColorMapSpec.ColorMapLength > 0)
                    {
                        int CmBytesPerPixel = (int)Math.Ceiling((double)Header.ColorMapSpec.ColorMapEntrySize / 8.0);
                        int LenBytes = Header.ColorMapSpec.ColorMapLength * CmBytesPerPixel;
                        ImageOrColorMapArea.ColorMapData = Br.ReadBytes(LenBytes);
                    }

                    #region Read Image Data
                    int BytesPerPixel = (int)Math.Ceiling((double)Header.ImageSpec.PixelDepth / 8.0);
                    if (Header.ImageType != TgaImageType.NoImageData)
                    {
                        int ImageDataSize = Width * Height * BytesPerPixel;
                        switch (Header.ImageType)
                        {
                            case TgaImageType.RLE_ColorMapped:
                            case TgaImageType.RLE_TrueColor:
                            case TgaImageType.RLE_BlackWhite:

                                int DataOffset = 0;
                                byte PacketInfo;
                                int PacketCount;
                                byte[] RLE_Bytes, RLE_Part;
                                ImageOrColorMapArea.ImageData = new byte[ImageDataSize];

                                do
                                {
                                    PacketInfo = Br.ReadByte();
                                    PacketCount = (PacketInfo & 127) + 1;

                                    if (PacketInfo >= 128)
                                    {
                                        RLE_Bytes = new byte[PacketCount * BytesPerPixel];
                                        RLE_Part = Br.ReadBytes(BytesPerPixel);
                                        for (int i = 0; i < RLE_Bytes.Length; i++)
                                            RLE_Bytes[i] = RLE_Part[i % BytesPerPixel];
                                    }
                                    else RLE_Bytes = Br.ReadBytes(PacketCount * BytesPerPixel);

                                    Buffer.BlockCopy(RLE_Bytes, 0, ImageOrColorMapArea.ImageData, DataOffset, RLE_Bytes.Length);
                                    DataOffset += RLE_Bytes.Length;
                                }
                                while (DataOffset < ImageDataSize);
                                RLE_Bytes = null;
                                break;

                            case TgaImageType.Uncompressed_ColorMapped:
                            case TgaImageType.Uncompressed_TrueColor:
                            case TgaImageType.Uncompressed_BlackWhite:
                                ImageOrColorMapArea.ImageData = Br.ReadBytes(ImageDataSize);
                                break;
                        }
                    }
                    #endregion

                    #region Try parse Footer
                    stream.Seek(-TgaFooter.Size, SeekOrigin.End);
                    uint FooterOffset = (uint)stream.Position;
                    TgaFooter MbFooter = new TgaFooter(Br.ReadBytes(TgaFooter.Size));
                    if (MbFooter.IsFooterCorrect)
                    {
                        Footer = MbFooter;
                        uint DevDirOffset = Footer.DeveloperDirectoryOffset;
                        uint ExtAreaOffset = Footer.ExtensionAreaOffset;

                        #region If Dev Area exist, read it.
                        if (DevDirOffset != 0)
                        {
                            stream.Seek(DevDirOffset, SeekOrigin.Begin);
                            DevArea = new TgaDevArea();
                            uint NumberOfTags = Br.ReadUInt16();

                            ushort[] Tags = new ushort[NumberOfTags];
                            uint[] TagOffsets = new uint[NumberOfTags];
                            uint[] TagSizes = new uint[NumberOfTags];

                            for (int i = 0; i < NumberOfTags; i++)
                            {
                                Tags[i] = Br.ReadUInt16();
                                TagOffsets[i] = Br.ReadUInt32();
                                TagSizes[i] = Br.ReadUInt32();
                            }

                            for (int i = 0; i < NumberOfTags; i++)
                            {
                                stream.Seek(TagOffsets[i], SeekOrigin.Begin);
                                var Ent = new TgaDevEntry(Tags[i], TagOffsets[i], Br.ReadBytes((int)TagSizes[i]));
                                DevArea.Entries.Add(Ent);
                            }

                            Tags = null;
                            TagOffsets = null;
                            TagSizes = null;
                        }
                        #endregion

                        #region If Ext Area exist, read it.
                        if (ExtAreaOffset != 0)
                        {
                            stream.Seek(ExtAreaOffset, SeekOrigin.Begin);
                            ushort ExtAreaSize = Math.Max((ushort)TgaExtArea.MinSize, Br.ReadUInt16());
                            stream.Seek(ExtAreaOffset, SeekOrigin.Begin);
                            ExtArea = new TgaExtArea(Br.ReadBytes(ExtAreaSize));

                            if (ExtArea.ScanLineOffset > 0)
                            {
                                stream.Seek(ExtArea.ScanLineOffset, SeekOrigin.Begin);
                                ExtArea.ScanLineTable = new uint[Height];
                                for (int i = 0; i < ExtArea.ScanLineTable.Length; i++)
                                    ExtArea.ScanLineTable[i] = Br.ReadUInt32();
                            }

                            if (ExtArea.PostageStampOffset > 0)
                            {
                                stream.Seek(ExtArea.PostageStampOffset, SeekOrigin.Begin);
                                byte W = Br.ReadByte();
                                byte H = Br.ReadByte();
                                int ImgDataSize = W * H * BytesPerPixel;
                                if (ImgDataSize > 0)
                                    ExtArea.PostageStampImage = new TgaPostageStampImage(W, H, Br.ReadBytes(ImgDataSize));
                            }

                            if (ExtArea.ColorCorrectionTableOffset > 0)
                            {
                                stream.Seek(ExtArea.ColorCorrectionTableOffset, SeekOrigin.Begin);
                                ExtArea.ColorCorrectionTable = new ushort[256 * 4];
                                for (int i = 0; i < ExtArea.ColorCorrectionTable.Length; i++)
                                    ExtArea.ColorCorrectionTable[i] = Br.ReadUInt16();
                            }
                        }
                        #endregion
                    }
                    #endregion

                    Br.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            bool LoadFunc(Bitmap bmp, bool UseRLE = false, bool NewFormat = true, bool ColorMap2BytesEntry = false)
            {
                if (bmp == null)
                    throw new ArgumentNullException();

                try
                {
                    Header.ImageSpec.ImageWidth = (ushort)bmp.Width;
                    Header.ImageSpec.ImageHeight = (ushort)bmp.Height;
                    Header.ImageSpec.ImageDescriptor.ImageOrigin = TgaImgOrigin.TopLeft;

                    switch (bmp.PixelFormat)
                    {
                        case PixelFormat.Indexed:
                        case PixelFormat.Gdi:
                        case PixelFormat.Alpha:
                        case PixelFormat.Undefined:
                        case PixelFormat.PAlpha:
                        case PixelFormat.Extended:
                        case PixelFormat.Max:
                        case PixelFormat.Canonical:
                        case PixelFormat.Format16bppRgb565:
                        default:
                            throw new FormatException(nameof(PixelFormat) + " is not supported!");

                        case PixelFormat.Format1bppIndexed:
                        case PixelFormat.Format4bppIndexed:
                        case PixelFormat.Format8bppIndexed:
                        case PixelFormat.Format16bppGrayScale:
                        case PixelFormat.Format16bppRgb555:
                        case PixelFormat.Format16bppArgb1555:
                        case PixelFormat.Format24bppRgb:
                        case PixelFormat.Format32bppRgb:
                        case PixelFormat.Format32bppArgb:
                        case PixelFormat.Format32bppPArgb:
                        case PixelFormat.Format48bppRgb:
                        case PixelFormat.Format64bppArgb:
                        case PixelFormat.Format64bppPArgb:

                            int bpp = Math.Max(8, Image.GetPixelFormatSize(bmp.PixelFormat));
                            int BytesPP = bpp / 8;

                            if (bmp.PixelFormat == PixelFormat.Format16bppRgb555)
                                bpp = 15;

                            bool IsAlpha = Image.IsAlphaPixelFormat(bmp.PixelFormat);
                            bool IsPreAlpha = IsAlpha && bmp.PixelFormat.ToString().EndsWith("PArgb");
                            bool IsColorMapped = bmp.PixelFormat.ToString().EndsWith("Indexed");

                            Header.ImageSpec.PixelDepth = (TgaPixelDepth)(BytesPP * 8);

                            if (IsAlpha)
                            {
                                Header.ImageSpec.ImageDescriptor.AlphaChannelBits = (byte)(BytesPP * 2);

                                if (bmp.PixelFormat == PixelFormat.Format16bppArgb1555)
                                    Header.ImageSpec.ImageDescriptor.AlphaChannelBits = 1;
                            }

                            #region ColorMap
                            bool IsGrayImage = (bmp.PixelFormat == PixelFormat.Format16bppGrayScale | IsColorMapped);

                            if (IsColorMapped && bmp.Palette != null)
                            {
                                Color[] Colors = bmp.Palette.Entries;

                                #region Analyze ColorMapType
                                int AlphaSum = 0;
                                bool ColorMapUseAlpha = false;

                                for (int i = 0; i < Colors.Length; i++)
                                {
                                    IsGrayImage &= (Colors[i].R == Colors[i].G && Colors[i].G == Colors[i].B);
                                    ColorMapUseAlpha |= (Colors[i].A < 248);
                                    AlphaSum |= Colors[i].A;
                                }
                                ColorMapUseAlpha &= (AlphaSum > 0);

                                int CMapBpp = (ColorMap2BytesEntry ? 15 : 24) + (ColorMapUseAlpha ? (ColorMap2BytesEntry ? 1 : 8) : 0);
                                int CMBytesPP = (int)Math.Ceiling(CMapBpp / 8.0);
                                #endregion

                                Header.ColorMapSpec.ColorMapLength = Math.Min((ushort)Colors.Length, ushort.MaxValue);
                                Header.ColorMapSpec.ColorMapEntrySize = (TgaColorMapEntrySize)CMapBpp;
                                ImageOrColorMapArea.ColorMapData = new byte[Header.ColorMapSpec.ColorMapLength * CMBytesPP];

                                byte[] CMapEntry = new byte[CMBytesPP];

                                const float To5Bit = 32f / 256f;
                                for (int i = 0; i < Colors.Length; i++)
                                {
                                    switch (Header.ColorMapSpec.ColorMapEntrySize)
                                    {
                                        case TgaColorMapEntrySize.A1R5G5B5:
                                        case TgaColorMapEntrySize.X1R5G5B5:
                                            int R = (int)(Colors[i].R * To5Bit);
                                            int G = (int)(Colors[i].G * To5Bit) << 5;
                                            int B = (int)(Colors[i].B * To5Bit) << 10;
                                            int A = 0;

                                            if (Header.ColorMapSpec.ColorMapEntrySize == TgaColorMapEntrySize.A1R5G5B5)
                                                A = ((Colors[i].A & 0x80) << 15);

                                            CMapEntry = BitConverter.GetBytes(A | R | G | B);
                                            break;

                                        case TgaColorMapEntrySize.R8G8B8:
                                            CMapEntry[0] = Colors[i].B;
                                            CMapEntry[1] = Colors[i].G;
                                            CMapEntry[2] = Colors[i].R;
                                            break;

                                        case TgaColorMapEntrySize.A8R8G8B8:
                                            CMapEntry[0] = Colors[i].B;
                                            CMapEntry[1] = Colors[i].G;
                                            CMapEntry[2] = Colors[i].R;
                                            CMapEntry[3] = Colors[i].A;
                                            break;

                                        case TgaColorMapEntrySize.Other:
                                        default:
                                            break;
                                    }

                                    Buffer.BlockCopy(CMapEntry, 0, ImageOrColorMapArea.ColorMapData, i * CMBytesPP, CMBytesPP);
                                }
                            }
                            #endregion

                            #region ImageType
                            if (UseRLE)
                            {
                                if (IsGrayImage)
                                    Header.ImageType = TgaImageType.RLE_BlackWhite;
                                else if (IsColorMapped)
                                    Header.ImageType = TgaImageType.RLE_ColorMapped;
                                else
                                    Header.ImageType = TgaImageType.RLE_TrueColor;
                            }
                            else
                            {
                                if (IsGrayImage)
                                    Header.ImageType = TgaImageType.Uncompressed_BlackWhite;
                                else if (IsColorMapped)
                                    Header.ImageType = TgaImageType.Uncompressed_ColorMapped;
                                else
                                    Header.ImageType = TgaImageType.Uncompressed_TrueColor;
                            }

                            Header.ColorMapType = (IsColorMapped ? TgaColorMapType.ColorMap : TgaColorMapType.NoColorMap);
                            #endregion

                            #region NewFormat
                            if (NewFormat)
                            {
                                Footer = new TgaFooter();
                                ExtArea = new TgaExtArea();
                                ExtArea.DateTimeStamp = new TgaDateTime(DateTime.UtcNow);

                                if (IsAlpha)
                                {
                                    ExtArea.AttributesType = TgaAttrType.UsefulAlpha;

                                    if (IsPreAlpha)
                                        ExtArea.AttributesType = TgaAttrType.PreMultipliedAlpha;
                                }
                                else
                                {
                                    ExtArea.AttributesType = TgaAttrType.NoAlpha;

                                    if (Header.ImageSpec.ImageDescriptor.AlphaChannelBits > 0)
                                        ExtArea.AttributesType = TgaAttrType.UndefinedAlphaButShouldBeRetained;
                                }
                            }
                            #endregion

                            #region Bitmap width is aligned by 32 bits = 4 bytes!Delete it.
                            int StrideBytes = bmp.Width * BytesPP;
                            int PaddingBytes = (int)Math.Ceiling(StrideBytes / 4.0) * 4 - StrideBytes;

                            byte[] ImageData = new byte[(StrideBytes + PaddingBytes) * bmp.Height];

                            Rectangle Re = new Rectangle(0, 0, bmp.Width, bmp.Height);
                            BitmapData BmpData = bmp.LockBits(Re, ImageLockMode.ReadOnly, bmp.PixelFormat);
                            Marshal.Copy(BmpData.Scan0, ImageData, 0, ImageData.Length);
                            bmp.UnlockBits(BmpData);
                            BmpData = null;

                            if (PaddingBytes > 0)
                            {
                                ImageOrColorMapArea.ImageData = new byte[StrideBytes * bmp.Height];
                                for (int i = 0; i < bmp.Height; i++)
                                    Buffer.BlockCopy(ImageData, i * (StrideBytes + PaddingBytes),
                                      ImageOrColorMapArea.ImageData, i * StrideBytes, StrideBytes);
                            }
                            else
                                ImageOrColorMapArea.ImageData = ImageData;

                            ImageData = null;

                            if (bmp.PixelFormat == PixelFormat.Format16bppGrayScale)
                            {
                                for (long i = 0; i < ImageOrColorMapArea.ImageData.Length; i++)
                                    ImageOrColorMapArea.ImageData[i] ^= byte.MaxValue;
                            }
                            #endregion

                            break;
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            bool SaveFunc(Stream stream)
            {
                try
                {
                    if (stream == null)
                        throw new ArgumentNullException();
                    if (!(stream.CanWrite && stream.CanSeek))
                        throw new FileLoadException("Stream writing or seeking is not avaiable!");

                    string CheckResult;
                    if (!CheckAndUpdateOffsets(out CheckResult))
                        return false;

                    BinaryWriter Bw = new BinaryWriter(stream);
                    Bw.Write(Header.ToBytes());

                    if (ImageOrColorMapArea.ImageID != null)
                        Bw.Write(ImageOrColorMapArea.ImageID.ToBytes());

                    if (Header.ColorMapType != TgaColorMapType.NoColorMap)
                        Bw.Write(ImageOrColorMapArea.ColorMapData);

                    #region ImageData
                    if (Header.ImageType != TgaImageType.NoImageData)
                    {
                        if (Header.ImageType >= TgaImageType.RLE_ColorMapped &&
                          Header.ImageType <= TgaImageType.RLE_BlackWhite)
                            Bw.Write(RLE_Encode(ImageOrColorMapArea.ImageData, Width, Height));
                        else
                            Bw.Write(ImageOrColorMapArea.ImageData);
                    }
                    #endregion

                    #region Footer
                    if (Footer != null)
                    {
                        #region DevArea
                        if (DevArea != null)
                        {
                            for (int i = 0; i < DevArea.Count; i++)
                                Bw.Write(DevArea[i].Data);

                            Bw.Write((ushort)DevArea.Count);

                            for (int i = 0; i < DevArea.Count; i++)
                            {
                                Bw.Write(DevArea[i].Tag);
                                Bw.Write(DevArea[i].Offset);
                                Bw.Write(DevArea[i].FieldSize);
                            }
                        }
                        #endregion

                        #region ExtArea
                        if (ExtArea != null)
                        {
                            Bw.Write(ExtArea.ToBytes());

                            if (ExtArea.ScanLineTable != null)
                                for (int i = 0; i < ExtArea.ScanLineTable.Length; i++)
                                    Bw.Write(ExtArea.ScanLineTable[i]);

                            if (ExtArea.PostageStampImage != null)
                                Bw.Write(ExtArea.PostageStampImage.ToBytes());

                            if (ExtArea.ColorCorrectionTable != null)
                                for (int i = 0; i < ExtArea.ColorCorrectionTable.Length; i++)
                                    Bw.Write(ExtArea.ColorCorrectionTable[i]);
                        }
                        #endregion

                        Bw.Write(Footer.ToBytes());
                    }
                    #endregion

                    Bw.Flush();
                    stream.Flush();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            byte[] RLE_Encode(byte[] ImageData, int Width, int Height)
            {
                if (ImageData == null)
                    throw new ArgumentNullException(nameof(ImageData) + "in null!");

                if (Width <= 0 || Height <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Width) + " and " + nameof(Height) + " must be > 0!");

                int Bpp = ImageData.Length / Width / Height;
                int ScanLineSize = Width * Bpp;

                if (ScanLineSize * Height != ImageData.Length)
                    throw new ArgumentOutOfRangeException("ImageData has wrong Length!");

                try
                {
                    int Count = 0;
                    int Pos = 0;
                    bool IsRLE = false;
                    List<byte> Encoded = new List<byte>();
                    byte[] RowData = new byte[ScanLineSize];

                    for (int y = 0; y < Height; y++)
                    {
                        Pos = 0;
                        Buffer.BlockCopy(ImageData, y * ScanLineSize, RowData, 0, ScanLineSize);

                        while (Pos < ScanLineSize)
                        {
                            if (Pos >= ScanLineSize - Bpp)
                            {
                                Encoded.Add(0);
                                Encoded.AddRange(BitConverterExt.GetElements(RowData, Pos, Bpp));
                                Pos += Bpp;
                                break;
                            }

                            Count = 0;
                            IsRLE = BitConverterExt.IsElementsEqual(RowData, Pos, Pos + Bpp, Bpp);

                            for (int i = Pos + Bpp; i < Math.Min(Pos + 128 * Bpp, ScanLineSize) - Bpp; i += Bpp)
                            {
                                if (IsRLE ^ BitConverterExt.IsElementsEqual(RowData, (IsRLE ? Pos : i), i + Bpp, Bpp))
                                {
                                    break;
                                }
                                else
                                    Count++;
                            }

                            int CountBpp = (Count + 1) * Bpp;
                            Encoded.Add((byte)(IsRLE ? Count | 128 : Count));
                            Encoded.AddRange(BitConverterExt.GetElements(RowData, Pos, (IsRLE ? Bpp : CountBpp)));
                            Pos += CountBpp;
                        }
                    }

                    return Encoded.ToArray();
                }
                catch
                {
                    return null;
                }
            }

            Bitmap ToBitmapFunc(bool ForceUseAlpha = false, bool PostageStampImage = false)
            {
                try
                {
                    #region UseAlpha ?
                    bool UseAlpha = true;
                    if (ExtArea != null)
                    {
                        switch (ExtArea.AttributesType)
                        {
                            case TgaAttrType.NoAlpha:
                            case TgaAttrType.UndefinedAlphaCanBeIgnored:
                            case TgaAttrType.UndefinedAlphaButShouldBeRetained:
                                UseAlpha = false;
                                break;
                            case TgaAttrType.UsefulAlpha:
                            case TgaAttrType.PreMultipliedAlpha:
                            default:
                                break;
                        }
                    }
                    UseAlpha = (Header.ImageSpec.ImageDescriptor.AlphaChannelBits > 0 && UseAlpha) | ForceUseAlpha;
                    #endregion

                    #region IsGrayImage
                    bool IsGrayImage = Header.ImageType == TgaImageType.RLE_BlackWhite ||
                      Header.ImageType == TgaImageType.Uncompressed_BlackWhite;
                    #endregion

                    #region Get PixelFormat
                    PixelFormat PixFormat = PixelFormat.Format24bppRgb;

                    switch (Header.ImageSpec.PixelDepth)
                    {
                        case TgaPixelDepth.Bpp8:
                            PixFormat = PixelFormat.Format8bppIndexed;
                            break;

                        case TgaPixelDepth.Bpp16:
                            if (IsGrayImage)
                                PixFormat = PixelFormat.Format16bppGrayScale;
                            else
                                PixFormat = (UseAlpha ? PixelFormat.Format16bppArgb1555 : PixelFormat.Format16bppRgb555);
                            break;

                        case TgaPixelDepth.Bpp24:
                            PixFormat = PixelFormat.Format24bppRgb;
                            break;

                        case TgaPixelDepth.Bpp32:
                            if (UseAlpha)
                            {
                                var f = Footer;
                                if (ExtArea?.AttributesType == TgaAttrType.PreMultipliedAlpha)
                                    PixFormat = PixelFormat.Format32bppPArgb;
                                else
                                    PixFormat = PixelFormat.Format32bppArgb;
                            }
                            else
                                PixFormat = PixelFormat.Format32bppRgb;
                            break;

                        default:
                            PixFormat = PixelFormat.Undefined;
                            break;
                    }
                    #endregion

                    ushort BMP_Width = (PostageStampImage ? ExtArea.PostageStampImage.Width : Width);
                    ushort BMP_Height = (PostageStampImage ? ExtArea.PostageStampImage.Height : Height);
                    Bitmap BMP = new Bitmap(BMP_Width, BMP_Height, PixFormat);

                    #region ColorMap and GrayPalette
                    if (Header.ColorMapType == TgaColorMapType.ColorMap &&
                      (Header.ImageType == TgaImageType.RLE_ColorMapped ||
                        Header.ImageType == TgaImageType.Uncompressed_ColorMapped))
                    {

                        ColorPalette ColorMap = BMP.Palette;
                        Color[] CMapColors = ColorMap.Entries;

                        switch (Header.ColorMapSpec.ColorMapEntrySize)
                        {
                            case TgaColorMapEntrySize.X1R5G5B5:
                            case TgaColorMapEntrySize.A1R5G5B5:
                                const float To8Bit = 255f / 31f;
                                for (int i = 0; i < Math.Min(CMapColors.Length, Header.ColorMapSpec.ColorMapLength); i++)
                                {
                                    ushort A1R5G5B5 = BitConverter.ToUInt16(ImageOrColorMapArea.ColorMapData, i * 2);
                                    int A = (UseAlpha ? (A1R5G5B5 & 0x8000) >> 15 : 1) * 255;
                                    int R = (int)(((A1R5G5B5 & 0x7C00) >> 10) * To8Bit);
                                    int G = (int)(((A1R5G5B5 & 0x3E0) >> 5) * To8Bit);
                                    int B = (int)((A1R5G5B5 & 0x1F) * To8Bit);
                                    CMapColors[i] = Color.FromArgb(A, R, G, B);
                                }
                                break;

                            case TgaColorMapEntrySize.R8G8B8:
                                for (int i = 0; i < Math.Min(CMapColors.Length, Header.ColorMapSpec.ColorMapLength); i++)
                                {
                                    int Index = i * 3;
                                    int R = ImageOrColorMapArea.ColorMapData[Index + 2];
                                    int G = ImageOrColorMapArea.ColorMapData[Index + 1];
                                    int B = ImageOrColorMapArea.ColorMapData[Index];
                                    CMapColors[i] = Color.FromArgb(R, G, B);
                                }
                                break;

                            case TgaColorMapEntrySize.A8R8G8B8:
                                for (int i = 0; i < Math.Min(CMapColors.Length, Header.ColorMapSpec.ColorMapLength); i++)
                                {
                                    int ARGB = BitConverter.ToInt32(ImageOrColorMapArea.ColorMapData, i * 4);
                                    CMapColors[i] = Color.FromArgb(UseAlpha ? ARGB | (0xFF << 24) : ARGB);
                                }
                                break;

                            default:
                                ColorMap = null;
                                break;
                        }

                        if (ColorMap != null)
                            BMP.Palette = ColorMap;
                    }

                    if (PixFormat == PixelFormat.Format8bppIndexed && IsGrayImage)
                    {
                        ColorPalette GrayPalette = BMP.Palette;
                        Color[] GrayColors = GrayPalette.Entries;
                        for (int i = 0; i < GrayColors.Length; i++)
                            GrayColors[i] = Color.FromArgb(i, i, i);
                        BMP.Palette = GrayPalette;
                    }
                    #endregion

                    #region Bitmap width must by aligned(align value = 32 bits = 4 bytes) !
                    byte[] ImageData;
                    int BytesPerPixel = (int)Math.Ceiling((double)Header.ImageSpec.PixelDepth / 8.0);
                    int StrideBytes = BMP.Width * BytesPerPixel;
                    int PaddingBytes = (int)Math.Ceiling(StrideBytes / 4.0) * 4 - StrideBytes;

                    if (PaddingBytes > 0)
                    {
                        ImageData = new byte[(StrideBytes + PaddingBytes) * BMP.Height];
                        for (int i = 0; i < BMP.Height; i++)
                            Buffer.BlockCopy(PostageStampImage ? ExtArea.PostageStampImage.Data :
                              ImageOrColorMapArea.ImageData, i * StrideBytes, ImageData,
                              i * (StrideBytes + PaddingBytes), StrideBytes);
                    }
                    else
                        ImageData = BitConverterExt.ToBytes(PostageStampImage ? ExtArea.PostageStampImage.Data :
                          ImageOrColorMapArea.ImageData);

                    if (PixFormat == PixelFormat.Format16bppGrayScale)
                    {
                        for (long i = 0; i < ImageData.Length; i++)
                            ImageData[i] ^= byte.MaxValue;
                    }
                    #endregion

                    Rectangle Re = new Rectangle(0, 0, BMP.Width, BMP.Height);
                    BitmapData BmpData = BMP.LockBits(Re, ImageLockMode.WriteOnly, BMP.PixelFormat);
                    Marshal.Copy(ImageData, 0, BmpData.Scan0, ImageData.Length);
                    BMP.UnlockBits(BmpData);
                    ImageData = null;
                    BmpData = null;

                    if (ExtArea != null && ExtArea.KeyColor.ToInt() != 0)
                        BMP.MakeTransparent(ExtArea.KeyColor.ToColor());

                    #region Flip Image
                    switch (Header.ImageSpec.ImageDescriptor.ImageOrigin)
                    {
                        case TgaImgOrigin.BottomLeft:
                            BMP.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            break;
                        case TgaImgOrigin.BottomRight:
                            BMP.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                            break;
                        case TgaImgOrigin.TopLeft:
                        default:
                            break;
                        case TgaImgOrigin.TopRight:
                            BMP.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                    }
                    #endregion

                    return BMP;
                }
                catch
                {
                    return null;
                }
            }
            #endregion

            #region Explicit
            public static explicit operator Bitmap(TGA tga)
            {
                return tga.ToBitmap();
            }

            public static explicit operator TGA(Bitmap bmp)
            {
                return FromBitmap(bmp);
            }
            #endregion
        }
    }
}