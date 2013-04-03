using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XSpriter
{
    static class XmlHelpers
    {
        public static ImageContent FromImageXml(XElement file, string basePath)
        {
            ImageContent img = new ImageContent();

            img.TextureName = file.Attribute("name").Value;

            // Get dimensions
            string sourceName = Path.Combine(basePath, file.Attribute("name").Value);
            using (Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(sourceName))
            {
                img.Dimensions.X = bitmap.Width;
                img.Dimensions.Y = bitmap.Height;
            }

            // Adjust pivot from UV to pixel coordinates and XNA screen space
            img.Pivot.X = img.Dimensions.X*GetFloatAttribute(file, "pivot_x", 0);
            img.Pivot.Y = img.Dimensions.Y*(1-GetFloatAttribute(file, "pivot_y", 1));

            return img;
        }

        public static Bone FromBoneXml(XElement xml)
        {
            Vector2 position = new Vector2(
                GetFloatAttribute(xml, "x", 0),
                -GetFloatAttribute(xml, "y", 0)
                );

            Vector2 scale = new Vector2(
                GetFloatAttribute(xml, "scale_x", 1),
                GetFloatAttribute(xml, "scale_y", 1)
                );


            Bone bone = new Bone()
                            {
                                Id = GetInt32Attribute(xml, "bone", 0),
                                Parent = GetInt32Attribute(xml, "parent", -1),
                                Angle = -MathHelper.ToRadians(GetFloatAttribute(xml, "angle", 0)),
                                Scale = scale,
                                Clockwise =
                                    xml.Parent.Attribute("spin") != null &&
                                    xml.Parent.Attribute("spin").Value == "-1",
                                Position = position,
                                Name =
                                    (xml.Parent.Parent.Name == "timeline" &&
                                        xml.Parent.Parent.Attribute("name") != null)
                                        ? xml.Parent.Parent.Attribute("name").Value
                                        : null
                            };
            return bone;
        }

        public static FrameImageContent FromObjectXml(XElement xml, ImageContent[][] textures, bool tweened)
        {
            int folderId = int.Parse(xml.Attribute("folder").Value);
            int fileId = int.Parse(xml.Attribute("file").Value);

            Vector2 pivot = Vector2.Zero;
            if (xml.Attribute("pivot_x") != null || xml.Attribute("pivot_y") != null)
            {
                pivot = new Vector2(
                    GetFloatAttribute(xml, "pivot_x", 0) * textures[folderId][fileId].Dimensions.X,
                    (1 - GetFloatAttribute(xml, "pivot_y", 1)) * textures[folderId][fileId].Dimensions.Y
                    );
            }
            else
            {
                try
                {
                    pivot =
                        textures[int.Parse(xml.Attribute("folder").Value)][int.Parse(xml.Attribute("file").Value)].Pivot;
                }
                catch
                {
                    
                }
            }

            Vector2 position = new Vector2(
                GetFloatAttribute(xml, "x", 0),
                -GetFloatAttribute(xml, "y", 0)
                );

            Vector2 scale = new Vector2(
                GetFloatAttribute(xml, "scale_x", 1),
                GetFloatAttribute(xml, "scale_y", 1)
                );

            FrameImageContent frameImage = new FrameImageContent()
                {
                    Angle = -MathHelper.ToRadians(GetFloatAttribute(xml, "angle", 0)),
                    Pivot = pivot,
                    Position = position,
                    TextureFolder = folderId,
                    TextureFile = fileId,
                    TextureName = textures[folderId][fileId].TextureName,
                    Clockwise = xml.Parent.Attribute("spin") != null && xml.Parent.Attribute("spin").Value == "-1",
                    Tweened = tweened,
                    ZIndex = GetInt32Attribute(xml, "z_index", 0),
                    Scale = scale
                };

            return frameImage;
        }

        public static Int32 GetInt32Attribute(XElement xml, string attributeName, Int32 defaultValue)
        {
            try
            {
                return xml.Attribute(attributeName) != null
                           ? Int32.Parse(xml.Attribute(attributeName).Value)
                           : defaultValue;
            }
            catch (Exception ex)
            {
                throw new FormatException(String.Format("Could not convert attribute {0} value '{1}' to Int32: {2}: {3}",
                    attributeName,
                    xml.Attribute(attributeName) != null ? xml.Attribute(attributeName).Value : "NULL",
                    ex.GetType().FullName, ex.Message));
            }
        }

        public static Int32? GetNullableInt32Attribute(XElement xml, string attributeName)
        {
            try
            {
                return xml.Attribute(attributeName) != null
                           ? (Int32?)Int32.Parse(xml.Attribute(attributeName).Value)
                           : null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static float GetFloatAttribute(XElement xml, string attributeName, int defaultValue)
        {
            try
            {
                return xml.Attribute(attributeName) != null
                           ? float.Parse(xml.Attribute(attributeName).Value)
                           : defaultValue;
            }
            catch (Exception ex)
            {
                throw new FormatException(String.Format("Could not convert attribute {0} value '{1}' to float: {2}: {3}",
                    attributeName,
                    xml.Attribute(attributeName) != null ? xml.Attribute(attributeName).Value : "NULL",
                    ex.GetType().FullName, ex.Message));
            }
        }

        public static Int64 GetInt64Attribute(XElement xml, string attributeName, Int64 defaultValue)
        {
            try
            {
                return xml.Attribute(attributeName) != null
                           ? Int64.Parse(xml.Attribute(attributeName).Value)
                           : defaultValue;
            }
            catch (Exception ex)
            {
                throw new FormatException(String.Format("Could not convert attribute {0} value '{1}' to Int64: {2}: {3}",
                    attributeName,
                    xml.Attribute(attributeName) != null ? xml.Attribute(attributeName).Value : "NULL",
                    ex.GetType().FullName, ex.Message));
            }
        }

        public static bool GetBoolAttribute(XElement xml, string attributeName, bool defaultValue)
        {
            try
            {
                return xml.Attribute(attributeName) != null
                           ? bool.Parse(xml.Attribute(attributeName).Value)
                           : defaultValue;
            }
            catch (Exception ex)
            {
                throw new FormatException(String.Format("Could not convert attribute {0} value '{1}' to bool: {2}: {3}",
                    attributeName,
                    xml.Attribute(attributeName) != null ? xml.Attribute(attributeName).Value : "NULL",
                    ex.GetType().FullName, ex.Message));
            }
        }
    }
}
