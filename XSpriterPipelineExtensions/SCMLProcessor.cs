using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Drawing;

namespace FuncWorks.XNA.XSpriter
{
    [ContentProcessor(DisplayName="XSpriter - Spriter SCML")]
    public class SCMLProcessor : ContentProcessor<XDocument, CharacterContent>
    {
        [DisplayName("Texture - Format")]
        [DefaultValue(TextureProcessorOutputFormat.Color)]
        [Description("Texture processor output format if loading textures")]
        public TextureProcessorOutputFormat TextureFormat { get; set; }

        [DisplayName("Texture - Premultiply Alpha")]
        [DefaultValue(true)]
        [Description("If true, texture is converted to premultiplied alpha format")]
        public Boolean PremultiplyAlpha { get; set; }

        [DisplayName("Animation FPS")]
        [DefaultValue(60)]
        [Description("Sets the number of frames per second that animations are processed into.")]
        public Int32 AnimationFPS { get; set; }

        public SCMLProcessor()
        {
            TextureFormat = TextureProcessorOutputFormat.Color;
            PremultiplyAlpha = true;
            AnimationFPS = 60;
        }

        public override CharacterContent Process(XDocument input, ContentProcessorContext context)
        {
            // Prevent parsing issues based on culture (i.e., decimals vs. commas in numbers)
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            CharacterContent character = new CharacterContent();

            // Internal
            character.FramesPerSecond = AnimationFPS;

            // Textures
            // <folder> and <file> should be numbered in sequential order, so we shouldn't need
            // to get the id values here; <file> name attributes should also contain folder names
            //TODO: Future versions will support atlas images and sounds
            List<List<string>> textures = (from fld in input.Root.Elements("folder")
                                           select
                                               (
                                                   from file in fld.Elements("file")
                                                   select file.Attribute("name").Value
                                               ).ToList()
                                          ).ToList();
            character.Textures = (from fld in input.Root.Elements("folder")
                                  select
                                      (
                                          from file in fld.Elements("file")
                                          select XmlHelpers.FromImageXml(file, Path.GetDirectoryName(context.OutputFilename.Remove(0, context.OutputDirectory.Length)))
                                      ).ToArray()).ToArray();

            // Entities
            // Currently one entity per file, with no ID or name
            //TODO: Future versions may include more than one entity, with names

            // Animations
            List<AnimationContent> animations = new List<AnimationContent>();
            foreach (XElement xanim in input.Root.Element("entity").Elements("animation"))
            {
                // Make a dictionary of external timelines which get pulled into the mainline
                Dictionary<int, Dictionary<int, List<FrameImageContent>>> objCache = new Dictionary<int, Dictionary<int, List<FrameImageContent>>>();
                Dictionary<int, Dictionary<int, List<Bone>>> boneCache = new Dictionary<int, Dictionary<int, List<Bone>>>();
                foreach (XElement tl in xanim.Elements("timeline"))
                {
                    int tlId = int.Parse(tl.Attribute("id").Value);
                    if (!objCache.ContainsKey(tlId))
                    {
                        objCache[tlId] = new Dictionary<int, List<FrameImageContent>>();
                    }
                    if (!boneCache.ContainsKey(tlId))
                    {
                        boneCache[tlId] = new Dictionary<int, List<Bone>>();
                    }

                    foreach (XElement key in tl.Elements("key"))
                    {
                        int objId = int.Parse(key.Attribute("id").Value);
                        objCache[tlId][objId] =
                            (from obj in key.Elements("object") select XmlHelpers.FromObjectXml(obj, character.Textures, true)).ToList();
                        boneCache[tlId][objId] =
                            (from bone in key.Elements("bone") select XmlHelpers.FromBoneXml(bone)).ToList();
                    }
                }

                // Build the animation itself
                AnimationContent anim = new AnimationContent()
                                     {
                                         Name = xanim.Attribute("name").Value,
                                         Length = long.Parse(xanim.Attribute("length").Value),
                                         Looping = XmlHelpers.GetBoolAttribute(xanim, "looping", true),
                                     };

                // Retrieve a list of keyframes from the mainline
                List<Keyframe> keyframes = new List<Keyframe>();
                foreach (XElement xkey in xanim.Element("mainline").Elements("key"))
                {
                    Keyframe key = new Keyframe();
                    key.Time = XmlHelpers.GetInt64Attribute(xkey, "time", 0);

                    List<FrameImageContent> objs = new List<FrameImageContent>();
                    List<Bone> bones = new List<Bone>();
                    foreach (XElement xobj in xkey.DescendantNodes())
                    {
                        if (xobj.Name == "object")
                        {
                            objs.Add(XmlHelpers.FromObjectXml(xobj, character.Textures, false));
                        }
                        else if (xobj.Name == "bone")
                        {
                            bones.Add(XmlHelpers.FromBoneXml(xobj));
                        }
                        else if (xobj.Name == "object_ref")
                        {
                            int tlId = int.Parse(xobj.Attribute("timeline").Value);
                            int keyId = int.Parse(xobj.Attribute("key").Value);
                            int zindex = int.Parse(xobj.Attribute("z_index").Value);
                            int parentId = XmlHelpers.GetInt32Attribute(xobj, "parent", -1);

                            foreach (FrameImageContent fimg in objCache[tlId][keyId])
                            {
                                objs.Add(fimg.CloneFrameImage(zindex, tlId, parentId));
                            }
                        }
                        else if (xobj.Name == "bone_ref")
                        {
                            int tlId = int.Parse(xobj.Attribute("timeline").Value);
                            int keyId = int.Parse(xobj.Attribute("key").Value);
                            int parentId = XmlHelpers.GetInt32Attribute(xobj, "parent", -1);

                            foreach (Bone bone in boneCache[tlId][keyId])
                            {
                                bones.Add(bone.CloneBone(tlId, parentId));
                            }
                        }
                    }

                    // Save to object
                    key.Objects = objs.ToArray();
                    key.Bones = bones.OrderBy(x => x.Id).ToArray();

                    keyframes.Add(key);
                }
                if (keyframes[keyframes.Count - 1].Time < anim.Length)
                {
                    keyframes.Add(keyframes[0].Clone(anim.Length));
                }

                // Process into frames
                Int32 frameTimeStep = 1000/AnimationFPS;
                List<FrameContent> frames = new List<FrameContent>();
                for (int frameTime = 0; frameTime <= anim.Length; frameTime += frameTimeStep)
                {
                    Keyframe currentFrame =
                        (from kf in keyframes where kf.Time <= frameTime orderby kf.Time descending select kf).
                            FirstOrDefault();
                    Keyframe nextFrame =
                        (from kf in keyframes where kf.Time > frameTime orderby kf.Time ascending select kf).
                            FirstOrDefault();

                    float timePct = (nextFrame != null) ?
                        ((frameTime - currentFrame.Time) / (float)(nextFrame.Time - currentFrame.Time)) :
                        0;

                    List<FrameImageContent> frameImages = new List<FrameImageContent>();
                    foreach (FrameImageContent currentFrameImage in currentFrame.Objects)
                    {
                        FrameImageContent nextFrameImage = (currentFrameImage.Tweened && nextFrame != null && nextFrame.Objects.Where(x => x.TimelineId == currentFrameImage.TimelineId).Any())
                                                        ? nextFrame.Objects.Where(x => x.TimelineId == currentFrameImage.TimelineId).Select(x => x).First()
                                                        : null;

                        // Tweening
                        //TODO: Future versions will use tweening methods other than linear
                        if (nextFrameImage != null)
                        {
                            frameImages.Add(currentFrameImage.Tween(nextFrameImage, timePct));
                        }
                        // Select last frame
                        else
                        {
                            frameImages.Add(currentFrameImage.CloneFrameImage());
                        }
                    }

                    List<Bone> bones = new List<Bone>();
                    foreach (Bone currentBone in currentFrame.Bones)
                    {
                        Bone nextBone = (nextFrame != null &&
                                         nextFrame.Bones.Where(x => x.TimelineId == currentBone.TimelineId).Any())
                                            ? nextFrame.Bones.Where(x => x.TimelineId == currentBone.TimelineId)
                                                       .Select(x => x)
                                                       .First()
                                            : null;

                        // Tweening
                        if (nextBone != null)
                        {
                            bones.Add(currentBone.Tween(nextBone, timePct));
                        }
                        else
                        {
                            bones.Add(currentBone.CloneBone());
                        }
                    }

                    FrameContent frame = new FrameContent() {Objects = frameImages.OrderBy(x => x.ZIndex).ToArray(), Bones = bones.OrderBy(x => x.Id).ToArray()};
                    frames.Add(frame);
                }
                anim.Frames = frames.ToArray();

                // Build timeline/texture lookup
                anim.TextureTimelines = new Dictionary<string, int>();
                foreach (List<string> folder in textures)
                {
                    foreach (string file in folder)
                    {
                        var textimeline = frames.SelectMany(x => x.Objects)
                                              .Where(x => x.TextureName != null && x.TextureName.Equals(file))
                                              .Select(x => x.TimelineId);
                        if (textimeline.Any())
                        {
                            anim.TextureTimelines[file] = textimeline.First();
                        }
                    }
                }

                // Build timeline/bone lookup
                anim.BoneTimelines = new Dictionary<string, int>();
                foreach (Bone bone in frames.SelectMany(x => x.Bones))
                {
                    if (!String.IsNullOrEmpty(bone.Name) && !anim.BoneTimelines.ContainsKey(bone.Name))
                    {
                        anim.BoneTimelines.Add(bone.Name, bone.TimelineId);
                    }
                }

                animations.Add(anim);
            }

            character.Animations = new List<AnimationContent>();
            character.Animations.AddRange(animations);

            // Build external textures
            foreach (List<string> folder in textures)
            {
                foreach (string txfile in folder)
                {
                    String assetName = Path.Combine(
                        Path.GetDirectoryName(context.OutputFilename.Remove(0, context.OutputDirectory.Length)),
                        Path.GetFileNameWithoutExtension(context.OutputFilename),
                        textures.IndexOf(folder).ToString("00"),
                        folder.IndexOf(txfile).ToString("00"));

                    String sourceName = Path.Combine(
                        Path.GetDirectoryName(context.OutputFilename.Remove(0, context.OutputDirectory.Length)),
                        txfile
                        );

                    OpaqueDataDictionary data = new OpaqueDataDictionary();
                    data.Add("GenerateMipmaps", false);
                    data.Add("ResizeToPowerOfTwo", false);
                    data.Add("PremultiplyAlpha", PremultiplyAlpha);
                    data.Add("TextureFormat", TextureFormat);
                    context.BuildAsset<TextureContent, TextureContent>(
                        new ExternalReference<TextureContent>(sourceName),
                        "TextureProcessor", data, "TextureImporter", assetName);                       
                }
            }

            Thread.CurrentThread.CurrentCulture = culture;
            return character;
        }
    }
}
