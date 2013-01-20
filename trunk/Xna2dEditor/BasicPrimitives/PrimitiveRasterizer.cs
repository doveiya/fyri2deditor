#region File Description

/*
 * File     :   "PrimitiveRasterizer.cs"
 * Author   :   Jeffrey Feck
 * Version  :   XNA 4.0 
 * Purpose  :   Contains PrimitiveRasterizer data members and function implementations.
 * Contributions & Special Thanks : http://forums.xna.com/forums/t/7414.aspx
 */

#endregion // File Description

#region Using Statements

// System libraries.
using System;
using System.Collections.Generic; // List.

// XNA libraries.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; // GraphicsDevice.

// Other libraries.
using Triangulator;

#endregion // Using Statments

namespace BasicPrimitiveRendering
{
    /// <summary>
    /// <para>Derives : None</para>
    /// <para>Purpose : Utility to translate, rotate, and scale 2D shapes.</para>
    /// <para>Version : 2.0b</para>
    /// </summary>
    public class PrimitiveRasterizer
    {
        /*********************************************************************/
        // Members.
        /*********************************************************************/

        #region Enumerations

        /// <summary>Defines line antialiasing usage.</summary>
        public enum Antialiasing
        {
            /// <summary>Creates a line without antialiasing.</summary>
            Disabled,
            /// <summary>Creates a line with antialiasing.</summary>
            Enabled
        }

        /// <summary>Defines fill mode rendering.</summary>
        public enum FillMode
        {
            /// <summary>Creates a fill in shape.</summary>
            Fill,
            /// <summary>Creates an outline of the shape.</summary>
            Outline,
        }

        #endregion // Enumerations

        #region Static Members

        private static class StateObjects
        {
            public static SamplerState ClampTextureAddressMode = new SamplerState()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp
            };
        }

        #endregion // Static Members

        #region 2D Members

        private Vector2 m_vPosition = Vector2.Zero;
        /// <summary>Get/Set the position.</summary>
        public Vector2 Position
        {
            get { return m_vPosition; }
            set
            {
                // Set 2D position.
                m_vPosition = value;

                // The outline shape draws by default [0, 0] from the top left, while the 
                // fill in shape draws by default at the center [0, 0, 0] of the screen 
                // in world space. So we need to calculate the middle of the screen, then 
                // subtract the position, unproject into world space, reverse [X, Y] coordinates,
                // clamp Z to zero, and set it to the world matrix.
                Vector2 vScreenOffset = new Vector2(m_GraphicsDevice.Viewport.Width >> 1, m_GraphicsDevice.Viewport.Height >> 1);
                Vector3 vTranslation = m_GraphicsDevice.Viewport.Unproject(new Vector3(vScreenOffset - value, 0f), m_Projection, m_View, Matrix.Identity);
                vTranslation.X *= -1f;
                vTranslation.Y *= -1f;
                vTranslation.Z = 0f;
                m_World.Translation = vTranslation;
            }
        }

        private Vector2 m_vCentriod = Vector2.Zero;
        /// <summary>Get the center average position.</summary>
        public Vector2 Centroid
        {
            get { return m_vCentriod; }
        }

        /// <summary>Set both the outline and fill in color.</summary>
        public Color Colour
        {
            set { OutlineColour = value; FillInColour = value; }
        }

        private Color m_OutlineColor = Color.White;
        /// <summary>Get/Set the outline color.</summary>
        public Color OutlineColour
        {
            get { return m_OutlineColor; }
            set { m_OutlineColor = value; }
        }

        /// <summary>The fill in color.</summary>
        private Color m_FillInColor = Color.White;
        /// <summary>Get/Set the outline color.</summary>
        public Color FillInColour
        {
            get { return m_FillInColor; }
            set 
            { 
                m_FillInColor = value;
                m_Effect.DiffuseColor = m_FillInColor.ToVector3();
            }
        }

        private float m_fThickness = 1f;
        /// <summary>Get/Set the line thickness. Default is 1f. The value is automatically clamped between 1 and float.MaxValue</para></summary>
        public float Thickness
        {
            get { return m_fThickness; }
            set
            {
                // Clamp the thickness value.
                m_fThickness = MathHelper.Clamp(value, 1f, float.MaxValue);

                // Calculate scale offset. We do this since scaling in 2D is based on pixels,
                // while scaling in 3D is based on matrices. Precision is based on the offset
                // speed. The higher the value, the slower the scale and vise versa.
                float fScaleOffset = (m_fThickness - 1.0f) / ((m_fThickness * (m_fScaleOffsetSpeed * m_fThickness) + 20f));

                // Calculate orign and scale from screen space to world space.
                Vector3 vOrigin = m_GraphicsDevice.Viewport.Unproject(new Vector3(m_vCentriod, 0f), m_Projection, m_View, Matrix.Identity);
                Vector3 vScale = fScaleOffset * m_GraphicsDevice.Viewport.Unproject(new Vector3(-(float)Math.Pow(m_fThickness, 2), -(float)Math.Pow(m_fThickness, 2), 0f), m_Projection, m_View, Matrix.Identity);

                // Store scale matrix based on "Scale From Origin" algorithm.
                // Final Result = -Orign[XYZ] * Scale * Orign[XYZ]
                m_matScale = Matrix.CreateTranslation(-vOrigin) * Matrix.CreateScale(new Vector3(1f + -vScale.X, 1f + vScale.Y, 1f)) * Matrix.CreateTranslation(vOrigin);
            }
        }

        /// <summary>Pixel texture handle.</summary>
        private Texture2D m_PixelTexture = null;
        /// <summary>Transparent color data.</summary>
        private Color[] m_PixelClearData;

        private List<Vector2> m_VectorList = new List<Vector2>();
        /// <summary>Add a new vector position to the list.
        /// <para>Function automatically calculates centriod.</para></summary>
        /// <param name="_vPosition">Position to add.</param>
        private void AddVector(Vector2 _vPosition)
        {
            m_VectorList.Add(_vPosition);
            m_vCentriod = CalculateCentroid();
        }
        /// <summary>Add a new vector position to the list.
        /// <para>Function automatically calculates centriod.</para><</summary>
        /// <param name="_vListPos">A list of positions to add.</param>
        private void AddVector(List<Vector2> _vListPos)
        {
            for (int i = _vListPos.Count - 1; i >= 0; --i)
                m_VectorList.Add(_vListPos[i]);
            m_vCentriod = CalculateCentroid();
        }

        #endregion // 2D Members

        #region 3D Members

        private Antialiasing m_Antialiasing = Antialiasing.Disabled;
        private FillMode m_FillMode = FillMode.Outline;

        /// <summary>Graphics device handle.</summary>
        private GraphicsDevice m_GraphicsDevice = null;
        /// <summary>Sprite batch handle.</summary>
        private SpriteBatch m_SpriteBatch = null;
        /// <summary>Used to set states for rendering.</summary>
        private RasterizerState m_RasterizerState = new RasterizerState();
        /// <summary>Effect handle.</summary>
        private BasicEffect m_Effect = null;
        /// <summary>Vertex buffer.</summary>
        private VertexBuffer m_VertexBuffer = null;
        /// <summary>Index buffer.</summary>
        private IndexBuffer m_IndexBuffer = null;
        /// <summary>World matrix used for custom shadeer.</summary>
        private Matrix m_World = Matrix.Identity;
        /// <summary>View matrix used for custom shader.</summary>
        private Matrix m_View = Matrix.Identity;
        /// <summary>Projection matrix used for custom shader.</summary>
        private Matrix m_Projection = Matrix.Identity;
        /// <summary>Scale matrix.</summary>
        private Matrix m_matScale = Matrix.Identity;
        /// <summary>Accumulated rotational value.</summary>
        private float m_fRotation = 0f;
        /// <summary>Used to calculate the scale matrix.</summary>
        private float m_fScaleOffsetSpeed = 0.043f;
        /// <summary>Used to calculate precise rotation from 2D to 3D.</summary>
        private float m_fRotationOffset = 0.015f;
        /// <summary>Number of vertices.</summary>
        private int m_nVertices = 0;
        /// <summary>Number of primitives.</summary>
        private int m_nPrimitives = 0;

        public void SetWorld(Matrix matrix)
        {
            this.m_World = Matrix.Multiply(this.m_World, matrix);
        }

        #endregion // 3D Members

        /*********************************************************************/
        // Functions.
        /*********************************************************************/

        #region Initialization

        /// <summary>Creates a new primitive ojbect.</summary>
        /// <param name="_graphicsDevice">Graphics device handle.</param>
        /// <param name="_spriteBatch">Sprite batch handle.</param>
        public PrimitiveRasterizer(GraphicsDevice _graphicsDevice, SpriteBatch _spriteBatch)
        {
            // Store members.
            m_GraphicsDevice = _graphicsDevice;
            m_SpriteBatch = _spriteBatch;

            // Local variables.
            int nViewportWidth = _graphicsDevice.Viewport.Width;
            int nViewportHeight = _graphicsDevice.Viewport.Height;

            // Create pixel texture.
            m_PixelTexture = new Texture2D(_graphicsDevice,     // Graphics device handle.
                                           nViewportWidth,      // Texture width.
                                           nViewportHeight,     // Texture height.
                                           false,               // Generate a full mipmap chain.
                                           SurfaceFormat.Color);// Texture data format.

            // Create transparent color data.
            m_PixelClearData = new Color[nViewportWidth * nViewportHeight];
            for (int i = (nViewportWidth * nViewportHeight) - 1; i >= 0; --i)
                m_PixelClearData[i] = new Color(0f, 0f, 0f, 0f); // r, g, b, a.
            Clear();

            /// Setup view and projection matrix.
            m_View = Matrix.CreateLookAt(Vector3.UnitZ, 
                                        -Vector3.UnitZ, 
                                         Vector3.Up);
            m_Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                                                               m_GraphicsDevice.Viewport.AspectRatio, 
                                                               0.1f, 
                                                               1.0f);
            // Setup basic effect.
            m_Effect = new BasicEffect(_graphicsDevice);
            Vector3 vPoint = m_GraphicsDevice.Viewport.Unproject(new Vector3(new Vector2(m_GraphicsDevice.Viewport.Width / 2, 
                                                                                         m_GraphicsDevice.Viewport.Height / 2), 0f), 
                                                                 m_Projection, 
                                                                 m_View, 
                                                                 Matrix.Identity);
            vPoint.Z = 0f;
            m_World.Translation = vPoint;
            m_Effect.View = m_View;
            m_Effect.Projection = m_Projection;

            // Setup rasterizer states.
            m_RasterizerState.CullMode = CullMode.None;
            m_RasterizerState.ScissorTestEnable = true;
        }

        #endregion // Initialization

        #region Pixel Manipulation

        /// <summary>Clear the texture data to be transparent.</summary>
        public void Clear()
        {
            m_VectorList.Clear();
            m_PixelTexture.SetData<Color>(m_PixelClearData);
        }

        /// <summary>Sets the color to a position (in pixels) on the texture.</summary>
        /// <param name="_nX">X-coordinate in pixels.</param>
        /// <param name="_nY">Y-coordinate in pixels.</param>
        /// <param name="_colour">The color to set.</param>
        private void SetPixel(int _nX, int _nY, Color _colour)
        {
            m_PixelTexture.SetData<Color>(0,                                // Mipmap level.
                                          new Rectangle(_nX, _nY, 1, 1),    // Defines the position and location (in pixels) of the data.
                                          new Color[] { _colour },          // Array of data.
                                          0,                                // Index of the first element to set.
                                          1);                               // Number of elements to set.
        }

        /// <summary>Sets the color to a position (in pixels) on the texture.</summary>
        /// <param name="_dX">X-coordinate in pixels.</param>
        /// <param name="_dY">Y-coordinate in pixels.</param>
        /// <param name="_dColourValue">Color offset. Used for antialiasing.</param>
        private void SetPixel(double _dX, double _dY, double _dColourValue)
        {
            SetPixel((int)_dX,
                     (int)_dY,
                     new Color(m_OutlineColor.ToVector4() * new Vector4(MathHelper.Clamp((float)_dColourValue, 0f, 1f))));
        }

        #endregion // Pixel Manipulation

        #region Mathmatical Functions

        /// <summary>Switch the values using the swap algorithm.
        /// <para>This is a modular function that takes any type, but makes a shallow copy.</para></summary>
        /// <param name="_nRefA">First value.</param>
        /// <param name="_nRefB">Second value.</param>
        public void SwapValues<Type>(ref Type _refA, ref Type _refB)
        {
            Type nTemp = _refA;
            _refA = _refB;
            _refB = nTemp;
        }

        /// <summary>Get the integer truncation.</summary>
        /// <param name="_dX">Value to use.</param>
        private double GetInteger(double _dX)
        {
            return Math.Truncate(_dX); // return (int)_dX; <-- Reference for multi-platform coding.
        }

        /// <summary>Get the value rounded up to the nearest integer truncation.</summary>
        /// <param name="_dX">Value to use.</param>
        private double GetRound(double _dX)
        {
            return GetInteger(_dX + 0.5); // return (int)(_dX + 0.5); <-- Reference for multi-platform coding.
        }

        /// <summary>Get the fractional truncation.</summary>
        /// <param name="_dX">Value to use.</param>
        private double GetFractional(double _dX)
        {
            return _dX - Math.Truncate(_dX); // return _dX - GetInteger(_dX); <-- Reference for multi-platform coding.
        }

        /// <summary>Get the reverse fractional truncation.</summary>
        /// <param name="_dX">Value to use.</param>
        private double GetReverseFractional(double _dX)
        {
            return 1 - GetFractional(_dX); // return 1.0 - GetFractional(_dX); <-- Reference for multi-platform coding.
        }

        /// <summary>Calculate center position.</summary>
        private Vector2 CalculateCentroid()
        {
            Vector2 vCenter = Vector2.Zero;

            // Single point.
            if (m_VectorList.Count == 1)
            {
                vCenter = m_VectorList[0];
            }
            // Line segment.
            else if (m_VectorList.Count == 2)
            {
                // Calculate midpoint.
                vCenter.X = (m_VectorList[0].X + m_VectorList[1].X) / 2f;
                vCenter.Y = (m_VectorList[0].Y + m_VectorList[1].Y) / 2f;
            }
            // Polygon.
            else if (m_VectorList.Count > 2)
            {
                // Local variables.
                float fArea = 0.0f, fDistance = 0.0f;
                int nIndex = 0, nLastPointIndex = m_VectorList.Count - 1;

                // Run through the list of positions.
                for (int i = 0; i <= nLastPointIndex; ++i)
                {
                    // Cacluate index.
                    nIndex = (i + 1) % (nLastPointIndex + 1);

                    // Calculate distance.
                    fDistance = m_VectorList[i].X * m_VectorList[nIndex].Y - m_VectorList[nIndex].X * m_VectorList[i].Y;

                    // Acculmate area.
                    fArea += fDistance;

                    // Move center positions based on positions and distance.
                    vCenter.X += (m_VectorList[i].X + m_VectorList[nIndex].X) * fDistance;
                    vCenter.Y += (m_VectorList[i].Y + m_VectorList[nIndex].Y) * fDistance;
                }

                // Calculate the final center position.
                fArea *= 0.5f;
                vCenter.X *= 1.0f / (6.0f * fArea);
                vCenter.Y *= 1.0f / (6.0f * fArea);
            }

            return vCenter;
        }

        #endregion // Mathmatical Functions

        #region Creation Methods

        /// <summary>Create a line from start to end points, in pixels.
        /// <para>Using Bresenham's Line Algorithm.</para></summary>
        /// <param name="_nStartPoint">Starting point of the line, in pixels.</param>
        /// <param name="_nEndPoint">Ending point of the line, in pixels.</param>
        /// <param name="_ableAA">Determines to enable/disable antialiasing.</param>
        /// <param name="_fillMode">Determines the fill in mode.</param>
        public void CreateLine(Vector2 _nStartPoint, Vector2 _nEndPoint, Antialiasing _ableAA, FillMode _fillMode)
        {
            AddVector(new List<Vector2> { _nStartPoint, _nEndPoint });
            CreateLine((int)_nStartPoint.X, (int)_nStartPoint.Y, (int)_nEndPoint.X, (int)_nEndPoint.Y, _ableAA, _fillMode);
        }

        /// <summary>Create a line from start to end points, in pixels.
        /// <para>Using Bresenham's Line Algorithm.</para></summary>
        /// <param name="_nStartX">Starting x-coordinate, in pixels.</param>
        /// <param name="_nStartY">Starting y-coordinate, in pixels.</param>
        /// <param name="_nEndX">Ending x-coordinate, in pixels.</param>
        /// <param name="_nEndY">Ending y-coordinate, in pixels.</param>
        /// <param name="_ableAA">Determines to enable/disable antialiasing.</param>
        /// <param name="_fillMode">Determines the fill in mode.</param>
        private void CreateLine(int _nStartX, int _nStartY, int _nEndX, int _nEndY, Antialiasing _ableAA, FillMode _fillMode)
        {
            CalculateShape(_nStartX, _nStartY, _nEndX, _nEndY, _ableAA, _fillMode);
        }

        /// <summary>Create a triangle primitive.</summary>
        /// <param name="_vPoint1">Fist point, in pixels.</param>
        /// <param name="_vPoint2">Second point, in pixels.</param>
        /// <param name="_vPoint3">Third point, in pixels.</param>
        /// <param name="_ableAA">Determines to enable/disable antialiasing.</param>
        /// <param name="_fillMode">Determines the fill in mode.</param>
        public void CreateTriangle(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3, Antialiasing _ableAA, FillMode _fillMode)
        {
            AddVector(new List<Vector2> { _vPoint1, _vPoint2, _vPoint3 });
            CreateTriangle((int)_vPoint1.X, (int)_vPoint1.Y, (int)_vPoint2.X, (int)_vPoint2.Y, _ableAA, _fillMode);
            CreateTriangle((int)_vPoint3.X, (int)_vPoint3.Y, (int)_vPoint2.X, (int)_vPoint2.Y, _ableAA, _fillMode);
            CreateTriangle((int)_vPoint3.X, (int)_vPoint3.Y, (int)_vPoint1.X, (int)_vPoint1.Y, _ableAA, _fillMode);
        }

        /// <summary>Create a triangle from start to end points, in pixels.
        /// <para>Using Bresenham's Line Algorithm.</para></summary>
        /// <param name="_nStartX">Starting x-coordinate, in pixels.</param>
        /// <param name="_nStartY">Starting y-coordinate, in pixels.</param>
        /// <param name="_nEndX">Ending x-coordinate, in pixels.</param>
        /// <param name="_nEndY">Ending y-coordinate, in pixels.</param>
        /// <param name="_ableAA">Determines to enable/disable antialiasing.</param>
        /// <param name="_fillMode">Determines the fill in mode.</param>
        private void CreateTriangle(int _nStartX, int _nStartY, int _nEndX, int _nEndY, Antialiasing _ableAA, FillMode _fillMode)
        {
            CalculateShape(_nStartX, _nStartY, _nEndX, _nEndY, _ableAA, _fillMode); 
        }

        /// <summary>Calculate shape.</summary>
        /// <param name="_nStartX">Starting x-coordinate, in pixels.</param>
        /// <param name="_nStartY">Starting y-coordinate, in pixels.</param>
        /// <param name="_nEndX">Ending x-coordinate, in pixels.</param>
        /// <param name="_nEndY">Ending y-coordinate, in pixels.</param>
        /// <param name="_ableAA">Determines to enable/disable antialiasing.</param>
        /// <param name="_fillMode">Determines the fill in mode.</param>
        private void CalculateShape(int _nStartX, int _nStartY, int _nEndX, int _nEndY, Antialiasing _ableAA, FillMode _fillMode)
        {
            m_Antialiasing = _ableAA;

            switch (_ableAA)
            {
                case Antialiasing.Enabled:
                    XiaolinWuLine(_nStartX, _nStartY, _nEndX, _nEndY); break;
                case Antialiasing.Disabled:
                    BresenhamLine(_nStartX, _nStartY, _nEndX, _nEndY); break;
                default: break;
            }

            switch (_fillMode)
            {
                case FillMode.Outline:
                    {
                        //m_Effect.EnableDefaultLighting();
                        m_Effect.TextureEnabled = true;
                        m_Effect.Texture = m_PixelTexture;

                        CalculateOutlineShape(WindingOrder.CounterClockwise);
                    }
                    break;
                case FillMode.Fill:
                    {
                        m_Effect.TextureEnabled = false;
                        m_Effect.Texture = null;

                        CalculateFillInShape(WindingOrder.CounterClockwise);
                    }
                    break;
                default: break;
            }

            m_FillMode = _fillMode;
        }

        /// <summary>Bresenham's line algorithm.</summary>
        /// <param name="_nStartX">Starting x-coordinate, in pixels.</param>
        /// <param name="_nStartY">Starting y-coordinate, in pixels.</param>
        /// <param name="_nEndX">Ending x-coordinate, in pixels.</param>
        /// <param name="_nEndY">Ending y-coordinate, in pixels.</param>
        private void BresenhamLine(int _nStartX, int _nStartY, int _nEndX, int _nEndY)
        {
            // Calculate distances.
            double dXDistance = Math.Abs(_nEndX - _nStartX);
            double dYDistance = Math.Abs(_nEndY - _nStartY);

            // Calculate direcitonal increments.
            int nXDirection = (_nStartX < _nEndX) ? 1 : -1;
            int nYDirection = (_nStartY < _nEndY) ? 1 : -1;

            // Error calculation in both directions, simultaneously.
            double dError = dXDistance - dYDistance;

            // Run through at least once.
            do
            {
                SetPixel(_nStartX, _nStartY, m_OutlineColor);

                // Accumulate error calculation.
                double dAccumError = 2 * dError;

                // Check y distance and increment in the x direction.
                if (dAccumError > -dYDistance)
                {
                    dError -= dYDistance;
                    _nStartX += nXDirection;
                }
                // Check x distance and increment in the y direction.
                if (dAccumError < dXDistance)
                {
                    dError += dXDistance;
                    _nStartY += nYDirection;
                }
            }
            while (_nStartX != _nEndX || _nStartY != _nEndY); // Continue until the end is near.
        }

        /// <summary>Xiaolin Wu's line algorithm.</summary>
        /// <param name="_nStartX">Starting x-coordinate, in pixels.</param>
        /// <param name="_nStartY">Starting y-coordinate, in pixels.</param>
        /// <param name="_nEndX">Ending x-coordinate, in pixels.</param>
        /// <param name="_nEndY">Ending y-coordinate, in pixels.</param>
        private void XiaolinWuLine(int _nStartX, int _nStartY, int _nEndX, int _nEndY)
        {
            // Calculate distances.
            double dXDistance = (double)_nEndX - (double)_nStartX;
            double dYDistance = (double)_nEndY - (double)_nStartY;
            
            if (Math.Abs(dXDistance) > Math.Abs(dYDistance))
            {
                if (_nStartX > _nEndX)
                {
                    SwapValues(ref _nStartX, ref  _nEndX);
                    SwapValues(ref _nStartY, ref  _nEndY);
                }

                // Handle first endpoint.
                double dGradient = dYDistance / dXDistance;
                double dXEnd = GetRound(_nStartX);
                double dYEnd = _nStartY + dGradient * (dXEnd - _nStartX);
                double dXGap = GetReverseFractional(_nStartX + 0.5);
                int nXPixel1 = (int)dXEnd;
                int nYPixel1 = (int)GetInteger(dYEnd);
                SetPixel(nXPixel1, nYPixel1, GetReverseFractional(dYEnd) * dXGap);
                SetPixel(nXPixel1, nYPixel1 + 1, GetFractional(dYEnd) * dXGap);
                double dYIntersection = dYEnd + dGradient;

                // Handle second endpont.
                dXEnd = GetRound(_nEndX);
                dYEnd = _nEndY + dGradient * (dXEnd - _nEndX);
                dXGap = GetFractional(_nEndX + 0.5);
                int nXPixel2 = (int)dXEnd;
                int nYPixel2 = (int)GetInteger(dYEnd);
                SetPixel(nXPixel2, nYPixel2, GetReverseFractional(dYEnd) * dXGap);
                SetPixel(nXPixel2, nYPixel2 + 1, GetFractional(dYEnd) * dXGap);

                // Run through and set pixels.
                for (int x = nXPixel1 + 1; x <= (nXPixel2 - 1); ++x)
                {
                    SetPixel(x, GetInteger(dYIntersection), GetReverseFractional(dYIntersection));
                    SetPixel(x, GetInteger(dYIntersection) + 1, GetFractional(dYIntersection));
                    dYIntersection += dGradient;
                }
            }
            else
            {
                if (_nStartY > _nEndY)
                {
                    SwapValues(ref _nStartX, ref  _nEndX);
                    SwapValues(ref _nStartY, ref  _nEndY);
                }

                // Handle first endpoint.
                double dGradient = dXDistance / dYDistance;
                double dYEnd = GetRound(_nStartY);
                double dXEnd = _nStartX + dGradient * (dYEnd - _nStartY);
                double dYGap = GetReverseFractional(_nStartY + 0.5);
                int nYPixel1 = (int)dYEnd;
                int nXPixel1 = (int)GetInteger(dXEnd);
                SetPixel(nXPixel1, nYPixel1, GetReverseFractional(dXEnd) * dYGap);
                SetPixel(nXPixel1, nYPixel1 + 1, GetFractional(dXEnd) * dYGap);
                double dXIntersection = dXEnd + dGradient;

                // Handle second endpont.
                dYEnd = GetRound(_nEndY);
                dXEnd = _nEndX + dGradient * (dYEnd - _nEndY);
                dYGap = GetFractional(_nEndY + 0.5);
                int nYPixel2 = (int)dYEnd;
                int nXPixel2 = (int)GetInteger(dXEnd);
                SetPixel(nXPixel2, nYPixel2, GetReverseFractional(dXEnd) * dYGap);
                SetPixel(nXPixel2, nYPixel2 + 1, GetFractional(dXEnd) * dYGap);

                // Run through and set pixels.
                for (int y = nXPixel1 + 1; y <= (nYPixel2 - 1); ++y)
                {
                    SetPixel(GetInteger(dXIntersection), y, GetReverseFractional(dXIntersection));
                    SetPixel(GetInteger(dXIntersection) + 1, y, GetFractional(dXIntersection));
                    dXIntersection += dGradient;
                }
            }
        }

        /// <summary>Calculate outline shape by using textured quad.</summary>
        /// <param name="_WindingOrder">Winding order of the shape used for triangulating.</param>
        private void CalculateOutlineShape(Triangulator.WindingOrder _WindingOrder)
        {
            // Validate.
            if (m_VectorList.Count < 2)
                return;

            // Sharpness determines the line quality by offsetting pixels.
            Vector2 vOffsetSharpness = Vector2.Zero;
            switch (m_Antialiasing)
            {
                case Antialiasing.Disabled: vOffsetSharpness = new Vector2(3f, 3f); break;
                case Antialiasing.Enabled: vOffsetSharpness = new Vector2(1f, 1f); break;

                default: break;
            }

            // Create a list of vertices.
            Vector2[] vSourceVertices = new Vector2[4];
            vSourceVertices[0] = Vector2.Zero;
            vSourceVertices[1] = new Vector2(0f, m_PixelTexture.Height);
            vSourceVertices[2] = new Vector2(m_PixelTexture.Width, m_PixelTexture.Height);
            vSourceVertices[3] = new Vector2(m_PixelTexture.Width, 0f);

            // Create a list of indices.
            int[] nSourceIndices;

            // Triangulate vertices and indices.
            Triangulator.Triangulator.Triangulate(vSourceVertices,
                                                  _WindingOrder,
                                                  out vSourceVertices,
                                                  out nSourceIndices);

            // Store number of vertices and primitives for rendering.
            m_nVertices = vSourceVertices.Length;
            m_nPrimitives = nSourceIndices.Length / 3;

            // Create and setup the list of vertex data.
            VertexPositionColorTexture[] verts = new VertexPositionColorTexture[vSourceVertices.Length];
            for (int i = vSourceVertices.Length - 1; i >= 0; --i)
                verts[i] = new VertexPositionColorTexture(m_GraphicsDevice.Viewport.Unproject(new Vector3(vSourceVertices[i], 0f),
                                                                                                          m_Projection,
                                                                                                          m_View,
                                                                                                          Matrix.Identity),
                                                                                                          m_FillInColor,
                                                                                                          new Vector2(vSourceVertices[i].X / (m_PixelTexture.Width - vOffsetSharpness.X),
                                                                                                                      vSourceVertices[i].Y / (m_PixelTexture.Height - vOffsetSharpness.Y)));

            // Clean up old data.
            if (m_VertexBuffer != null)
            {
                m_VertexBuffer.Dispose();
                m_VertexBuffer = null;
            }
            if (m_IndexBuffer != null)
            {
                m_IndexBuffer.Dispose();
                m_IndexBuffer = null;
            }

            // Create vertex buffer.
            m_VertexBuffer = new VertexBuffer(m_GraphicsDevice,
                                              VertexPositionColorTexture.VertexDeclaration,
                                              verts.Length * VertexPositionColorTexture.VertexDeclaration.VertexStride,
                                              BufferUsage.WriteOnly);
            m_VertexBuffer.SetData(verts);

            // Branch here to convert our indices to shorts if possible for wider GPU support.
            if (verts.Length < UInt16.MaxValue /*65535*/)
            {
                // Create a list of indices.
                short[] sIndices = new short[nSourceIndices.Length];
                for (int i = nSourceIndices.Length - 1; i >= 0; --i)
                    sIndices[i] = (short)nSourceIndices[i];

                // Create index buffer.
                m_IndexBuffer = new IndexBuffer(m_GraphicsDevice,
                                                IndexElementSize.SixteenBits,
                                                sIndices.Length * sizeof(short),
                                                BufferUsage.WriteOnly);
                m_IndexBuffer.SetData(sIndices);
            }
            else
            {
                // Create index buffer.
                m_IndexBuffer = new IndexBuffer(m_GraphicsDevice,
                                                IndexElementSize.ThirtyTwoBits,
                                                nSourceIndices.Length * sizeof(int),
                                                BufferUsage.WriteOnly);
                m_IndexBuffer.SetData(nSourceIndices);
            }
        }

        /// <summary>Calculate fill in shape by using triangulation.</summary>
        /// <param name="_WindingOrder">Winding order of the shape used for triangulating.</param>
        private void CalculateFillInShape(Triangulator.WindingOrder _WindingOrder)
        {
            // Validate.
            if (m_VectorList.Count <= 2)
                return;

            // Create a list of vertices.
            Vector2[] vSourceVertices = new Vector2[m_VectorList.Count];
            for (int i = m_VectorList.Count - 1; i >= 0; --i)
                vSourceVertices[i] = m_VectorList[i];

            // Create a list of indices.
            int[] nSourceIndices;

            // Triangulate vertices and indices.
            Triangulator.Triangulator.Triangulate(vSourceVertices,
                                                  _WindingOrder,
                                                  out vSourceVertices,
                                                  out nSourceIndices);

            // Store number of vertices and primitives for rendering.
            m_nVertices = vSourceVertices.Length;
            m_nPrimitives = nSourceIndices.Length / 3;

            // Create and setup the list of vertex data.
            VertexPositionColor[] verts = new VertexPositionColor[vSourceVertices.Length];
            for (int i = vSourceVertices.Length - 1; i >= 0; --i)
                verts[i] = new VertexPositionColor(m_GraphicsDevice.Viewport.Unproject(new Vector3(vSourceVertices[i], 0f),
                                                                                                   m_Projection,
                                                                                                   m_View,
                                                                                                   Matrix.Identity), m_FillInColor);

            // Clean up old data.
            if (m_VertexBuffer != null)
            {
                m_VertexBuffer.Dispose();
                m_VertexBuffer = null;
            }
            if (m_IndexBuffer != null)
            {
                m_IndexBuffer.Dispose();
                m_IndexBuffer = null;
            }

            // Create vertex buffer.
            m_VertexBuffer = new VertexBuffer(m_GraphicsDevice,
                                              VertexPositionColor.VertexDeclaration,
                                              verts.Length * VertexPositionColor.VertexDeclaration.VertexStride,
                                              BufferUsage.WriteOnly);
            m_VertexBuffer.SetData(verts);

            // Branch here to convert our indices to shorts if possible for wider GPU support.
            if (verts.Length < UInt16.MaxValue /*65535*/)
            {
                // Create a list of indices.
                short[] sIndices = new short[nSourceIndices.Length];
                for (int i = nSourceIndices.Length - 1; i >= 0; --i)
                    sIndices[i] = (short)nSourceIndices[i];

                // Create index buffer.
                m_IndexBuffer = new IndexBuffer(m_GraphicsDevice,
                                                IndexElementSize.SixteenBits,
                                                sIndices.Length * sizeof(short),
                                                BufferUsage.WriteOnly);
                m_IndexBuffer.SetData(sIndices);
            }
            else
            {
                // Create index buffer.
                m_IndexBuffer = new IndexBuffer(m_GraphicsDevice,
                                                IndexElementSize.ThirtyTwoBits,
                                                nSourceIndices.Length * sizeof(int),
                                                BufferUsage.WriteOnly);
                m_IndexBuffer.SetData(nSourceIndices);
            }
        }

        #endregion // Creation Methods

        #region Render Methods

        /// <summary>Render shape.</summary>
        public void Render()
        {
            // Validate.
            if (m_nVertices <= 0 || m_nPrimitives <= 0)
                return;

            // Set rasterizer states.
            m_GraphicsDevice.RasterizerState = m_RasterizerState;

            

            // Set graphic device members.
            m_GraphicsDevice.Indices = m_IndexBuffer;
            m_GraphicsDevice.SetVertexBuffer(m_VertexBuffer);

            // Apply world matrix (scale, rotation, translation).
            m_Effect.World = m_matScale * m_World;

            foreach (EffectPass pass in m_Effect.CurrentTechnique.Passes)
            {
                // Begins the pass.
                pass.Apply();                

                if (m_GraphicsDevice.GraphicsProfile == GraphicsProfile.Reach)
                    m_GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                // Render the list of triangles.
                m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_nVertices, 0, m_nPrimitives);
            }
        }

        #endregion // Render Methods
    }
}
