// RoundLine.cs
// By Michael D. Anderson
// Version 4.00, Feb 8 2011
//
// A class to efficiently draw thick lines with rounded ends.

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion


namespace Fyri2dEditor
{
    /// <summary>
    /// Represents a single line segment.  Drawing is handled by the RoundLineManager class.
    /// </summary>
    public partial class XnaLine2d
    {
        private Vector2 startPoint; // Begin point of the line
        private Vector2 endPoint; // End point of the line
        private float length; // Length of the line
        private float angle; // Angle of the line

        private float radius;
        private Color color;

        public Vector2 StartPoint 
        { 
            get 
            { 
                return startPoint; 
            }
            set
            {
                startPoint = value;
                RecalcRhoTheta();
            }
        }

        public Vector2 EndPoint 
        {
            get 
            { 
                return endPoint; 
            }
            set
            {
                endPoint = value;
                RecalcRhoTheta();
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public float Length { get { return length; } }
        public float Angle { get { return angle; } }

        public XnaLine2d(Vector2 p0, Vector2 p1, float radius, Color color)
        {
            this.startPoint = p0;
            this.endPoint = p1;
            this.radius = radius;
            this.color = color;
            RecalcRhoTheta();
        }

        public XnaLine2d(float x0, float y0, float x1, float y1, float radius, Color color)
        {
            this.startPoint = new Vector2(x0, y0);
            this.endPoint = new Vector2(x1, y1);
            this.radius = radius;
            this.color = color;
            RecalcRhoTheta();
        }

        protected void RecalcRhoTheta()
        {
            Vector2 delta = EndPoint - StartPoint;
            length = delta.Length();
            angle = (float)Math.Atan2(delta.Y, delta.X);
        }
    };


    // A "degenerate" RoundLine where both endpoints are equal
    public class XnaDisc2d : XnaLine2d
    {
        public XnaDisc2d(Vector2 p, float radius, Color color) : base(p, p, radius, color) { }
        public XnaDisc2d(float x, float y, float radius, Color color) : base(x, y, x, y, radius, color) { }
        public Vector2 Pos 
        {
            get 
            {
                return StartPoint; 
            }
            set
            {
                StartPoint = value;
                EndPoint = value;
            }
        }
    };


    // A vertex type for drawing RoundLines, including an instance index
    struct XnaLine2dVertex : IVertexType
    {
        public Vector3 pos;
        public Vector2 rhoTheta;
        public Vector2 scaleTrans;
        public float index;

        public XnaLine2dVertex(Vector3 pos, Vector2 norm, Vector2 tex, float index)
        {
            this.pos = pos;
            this.rhoTheta = norm;
            this.scaleTrans = tex;
            this.index = index;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.Normal, 0),
                new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(28, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
            );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }


    
}
