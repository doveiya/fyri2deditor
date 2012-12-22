﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Fyri2dEditor
{
    /// <summary>
    /// Class to handle drawing a list of RoundLines.
    /// </summary>
    public class XnaLine2dBatch
    {
        private GraphicsDevice device;
        private Effect effect;
        private EffectParameter viewProjMatrixParameter;
        private EffectParameter instanceDataParameter;
        private EffectParameter timeParameter;
        private EffectParameter lineRadiusParameter;
        private EffectParameter lineColorParameter;
        private EffectParameter blurThresholdParameter;
        private VertexBuffer vb;
        private IndexBuffer ib;
        private int numInstances;
        private int numVertices;
        private int numIndices;
        private int numPrimitivesPerInstance;
        private int numPrimitives;
        float[] translationData;

        public int NumLinesDrawn;
        public float BlurThreshold = 0.97f;

        public void Init(GraphicsDevice device, Effect roundLineEffect)
        {
            this.device = device;
            effect = roundLineEffect;
            viewProjMatrixParameter = effect.Parameters["viewProj"];
            instanceDataParameter = effect.Parameters["instanceData"];
            timeParameter = effect.Parameters["time"];
            lineRadiusParameter = effect.Parameters["lineRadius"];
            lineColorParameter = effect.Parameters["lineColor"];
            blurThresholdParameter = effect.Parameters["blurThreshold"];

            CreateRoundLineMesh();
        }

        public string[] TechniqueNames
        {
            get
            {
                string[] names = new string[effect.Techniques.Count];
                int index = 0;
                foreach (EffectTechnique technique in effect.Techniques)
                    names[index++] = technique.Name;
                return names;
            }
        }

        /// <summary>
        /// Create a mesh for a RoundLine.
        /// </summary>
        /// <remarks>
        /// The RoundLine mesh has 3 sections:
        /// 1.  Two quads, from 0 to 1 (left to right)
        /// 2.  A half-disc, off the left side of the quad
        /// 3.  A half-disc, off the right side of the quad
        ///
        /// The X and Y coordinates of the "normal" encode the rho and theta of each vertex
        /// The "texture" encodes whether to scale and translate the vertex horizontally by length and radius
        /// </remarks>
        private void CreateRoundLineMesh()
        {
            const int primsPerCap = 12; // A higher primsPerCap produces rounder endcaps at the cost of more vertices
            const int verticesPerCap = primsPerCap * 2 + 2;
            const int primsPerCore = 4;
            const int verticesPerCore = 8;

            numInstances = 200;
            numVertices = (verticesPerCore + verticesPerCap + verticesPerCap) * numInstances;
            numPrimitivesPerInstance = primsPerCore + primsPerCap + primsPerCap;
            numPrimitives = numPrimitivesPerInstance * numInstances;
            numIndices = 3 * numPrimitives;
            short[] indices = new short[numIndices];
            XnaLine2dVertex[] tri = new XnaLine2dVertex[numVertices];
            translationData = new float[numInstances * 4]; // Used in Draw()

            int iv = 0;
            int ii = 0;
            int iVertex;
            int iIndex;
            for (int instance = 0; instance < numInstances; instance++)
            {
                // core vertices
                const float pi2 = MathHelper.PiOver2;
                const float threePi2 = 3 * pi2;
                iVertex = iv;
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, -1.0f, 0), new Vector2(1, threePi2), new Vector2(0, 0), instance);
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, -1.0f, 0), new Vector2(1, threePi2), new Vector2(0, 1), instance);
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, threePi2), new Vector2(0, 1), instance);
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, threePi2), new Vector2(0, 0), instance);
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, pi2), new Vector2(0, 1), instance);
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, pi2), new Vector2(0, 0), instance);
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, 1.0f, 0), new Vector2(1, pi2), new Vector2(0, 1), instance);
                tri[iv++] = new XnaLine2dVertex(new Vector3(0.0f, 1.0f, 0), new Vector2(1, pi2), new Vector2(0, 0), instance);

                // core indices
                indices[ii++] = (short)(iVertex + 0);
                indices[ii++] = (short)(iVertex + 1);
                indices[ii++] = (short)(iVertex + 2);
                indices[ii++] = (short)(iVertex + 2);
                indices[ii++] = (short)(iVertex + 3);
                indices[ii++] = (short)(iVertex + 0);

                indices[ii++] = (short)(iVertex + 4);
                indices[ii++] = (short)(iVertex + 6);
                indices[ii++] = (short)(iVertex + 5);
                indices[ii++] = (short)(iVertex + 6);
                indices[ii++] = (short)(iVertex + 7);
                indices[ii++] = (short)(iVertex + 5);

                // left halfdisc
                iVertex = iv;
                iIndex = ii;
                for (int i = 0; i < primsPerCap + 1; i++)
                {
                    float deltaTheta = MathHelper.Pi / primsPerCap;
                    float theta0 = MathHelper.PiOver2 + i * deltaTheta;
                    float theta1 = theta0 + deltaTheta / 2;
                    // even-numbered indices are at the center of the halfdisc
                    tri[iVertex + 0] = new XnaLine2dVertex(new Vector3(0, 0, 0), new Vector2(0, theta1), new Vector2(0, 0), instance);

                    // odd-numbered indices are at the perimeter of the halfdisc
                    float x = (float)Math.Cos(theta0);
                    float y = (float)Math.Sin(theta0);
                    tri[iVertex + 1] = new XnaLine2dVertex(new Vector3(x, y, 0), new Vector2(1, theta0), new Vector2(1, 0), instance);

                    if (i < primsPerCap)
                    {
                        // indices follow this pattern: (0, 1, 3), (2, 3, 5), (4, 5, 7), ...
                        indices[iIndex + 0] = (short)(iVertex + 0);
                        indices[iIndex + 1] = (short)(iVertex + 1);
                        indices[iIndex + 2] = (short)(iVertex + 3);
                        iIndex += 3;
                        ii += 3;
                    }
                    iVertex += 2;
                    iv += 2;
                }

                // right halfdisc
                for (int i = 0; i < primsPerCap + 1; i++)
                {
                    float deltaTheta = MathHelper.Pi / primsPerCap;
                    float theta0 = 3 * MathHelper.PiOver2 + i * deltaTheta;
                    float theta1 = theta0 + deltaTheta / 2;
                    float theta2 = theta0 + deltaTheta;
                    // even-numbered indices are at the center of the halfdisc
                    tri[iVertex + 0] = new XnaLine2dVertex(new Vector3(0, 0, 0), new Vector2(0, theta1), new Vector2(0, 1), instance);

                    // odd-numbered indices are at the perimeter of the halfdisc
                    float x = (float)Math.Cos(theta0);
                    float y = (float)Math.Sin(theta0);
                    tri[iVertex + 1] = new XnaLine2dVertex(new Vector3(x, y, 0), new Vector2(1, theta0), new Vector2(1, 1), instance);

                    if (i < primsPerCap)
                    {
                        // indices follow this pattern: (0, 1, 3), (2, 3, 5), (4, 5, 7), ...
                        indices[iIndex + 0] = (short)(iVertex + 0);
                        indices[iIndex + 1] = (short)(iVertex + 1);
                        indices[iIndex + 2] = (short)(iVertex + 3);
                        iIndex += 3;
                        ii += 3;
                    }
                    iVertex += 2;
                    iv += 2;
                }
            }

            vb = new VertexBuffer(device, typeof(XnaLine2dVertex), numVertices, BufferUsage.None);
            vb.SetData<XnaLine2dVertex>(tri);

            ib = new IndexBuffer(device, IndexElementSize.SixteenBits, numIndices, BufferUsage.None);
            ib.SetData<short>(indices);
        }

        /// <summary>
        /// Compute a reasonable "BlurThreshold" value to use when drawing RoundLines.
        /// See how wide lines of the specified radius will be (in pixels) when drawn
        /// to the back buffer.  Then apply an empirically-determined mapping to get
        /// a good BlurThreshold for such lines.
        /// </summary>
        public float ComputeBlurThreshold(float lineRadius, Matrix viewProjMatrix, float viewportWidth)
        {
            Vector4 lineRadiusTestBase = new Vector4(0, 0, 0, 1);
            Vector4 lineRadiusTest = new Vector4(lineRadius, 0, 0, 1);
            Vector4 delta = lineRadiusTest - lineRadiusTestBase;
            Vector4 output = Vector4.Transform(delta, viewProjMatrix);
            output.X *= viewportWidth;

            double newBlur = 0.125 * Math.Log(output.X) + 0.4;

            return MathHelper.Clamp((float)newBlur, 0.5f, 0.99f);
        }

        /// <summary>
        /// Draw a single RoundLine.  Usually you want to draw a list of RoundLines
        /// at a time instead for better performance.
        /// </summary>
        public void Draw(XnaLine2d roundLine,  Matrix viewProjMatrix,
            float time, string techniqueName)
        {
            device.SetVertexBuffer(vb);
            device.Indices = ib;

            viewProjMatrixParameter.SetValue(viewProjMatrix);
            timeParameter.SetValue(time);
            lineColorParameter.SetValue(roundLine.Color.ToVector4());
            lineRadiusParameter.SetValue(roundLine.Radius);
            blurThresholdParameter.SetValue(BlurThreshold);

            int iData = 0;
            translationData[iData++] = roundLine.StartPoint.X;
            translationData[iData++] = roundLine.StartPoint.Y;
            translationData[iData++] = roundLine.Length;
            translationData[iData++] = roundLine.Angle;
            instanceDataParameter.SetValue(translationData);

            if (techniqueName == null)
                effect.CurrentTechnique = effect.Techniques[0];
            else
                effect.CurrentTechnique = effect.Techniques[techniqueName];
            EffectPass pass = effect.CurrentTechnique.Passes[0];
            pass.Apply();

            int numInstancesThisDraw = 1;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numVertices, 0, numPrimitivesPerInstance * numInstancesThisDraw);
            NumLinesDrawn += numInstancesThisDraw;
        }

        /// <summary>
        /// Draw a list of Lines.
        /// </summary>
        public void Draw(List<XnaLine2d> roundLines, Matrix viewProjMatrix,
            float time, string techniqueName)
        {
            device.SetVertexBuffer(vb);
            device.Indices = ib;

            viewProjMatrixParameter.SetValue(viewProjMatrix);
            timeParameter.SetValue(time);
            
            blurThresholdParameter.SetValue(BlurThreshold);

            if (techniqueName == null)
                effect.CurrentTechnique = effect.Techniques[0];
            else
                effect.CurrentTechnique = effect.Techniques[techniqueName];
            EffectPass pass = effect.CurrentTechnique.Passes[0];
            pass.Apply();

            int iData = 0;
            int numInstancesThisDraw = 0;
            foreach (XnaLine2d roundLine in roundLines)
            {
                lineColorParameter.SetValue(roundLine.Color.ToVector4());
                lineRadiusParameter.SetValue(roundLine.Radius);

                translationData[iData++] = roundLine.StartPoint.X;
                translationData[iData++] = roundLine.StartPoint.Y;
                translationData[iData++] = roundLine.Length;
                translationData[iData++] = roundLine.Angle;
                numInstancesThisDraw++;

                if (numInstancesThisDraw == numInstances)
                {
                    instanceDataParameter.SetValue(translationData);
                    pass.Apply();
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numVertices, 0, numPrimitivesPerInstance * numInstancesThisDraw);
                    NumLinesDrawn += numInstancesThisDraw;
                    numInstancesThisDraw = 0;
                    iData = 0;
                }
            }
            if (numInstancesThisDraw > 0)
            {
                instanceDataParameter.SetValue(translationData);
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numVertices, 0, numPrimitivesPerInstance * numInstancesThisDraw);
                NumLinesDrawn += numInstancesThisDraw;
            }
        }
    }
}
