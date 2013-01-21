using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Xna2dEditor
{
    public static class MatrixExtensions
    {
        private static System.Drawing.PointF[] PTS4 = new System.Drawing.PointF[4];

        public static float[] GetElements(Matrix m)
        {
            float[] ret = new float[6];

            ret[0] = m.M11;
            ret[1] = m.M12;
            ret[2] = m.M21;
            ret[3] = m.M22;
            ret[4] = m.M41;
            ret[5] = m.M42;

            return ret;
        }

        public static Matrix SetElements(float el0, float el1, float el2, float el3, float el4, float el5)
        {
            Matrix ret = Matrix.Identity;

            ret.M11 = el0;
            ret.M12 = el1;
            ret.M21 = el2;
            ret.M22 = el3;
            ret.M41 = el4;
            ret.M42 = el5;

            return ret;
        }

        private static Matrix SetElements(System.Drawing.Drawing2D.Matrix drawMatrix)
        {
            Matrix ret = Matrix.Identity;

            ret.M11 = drawMatrix.Elements[0];
            ret.M12 = drawMatrix.Elements[1];
            ret.M21 = drawMatrix.Elements[2];
            ret.M22 = drawMatrix.Elements[3];
            ret.M41 = drawMatrix.Elements[4];
            ret.M42 = drawMatrix.Elements[5];

            return ret;
        }

        internal static Matrix SetScale(Matrix m, float scale)
        {
            float Scale = MatrixExtensions.GetScale(m);
            if (Scale != 0)
            {
                return ScaleBy(m, scale / Scale);
            }
            else
            {
                throw new InvalidOperationException("Can't set Scale when Scale is equal to 0");
            }	
        }

        

        internal static PointFx Transform(Matrix tempMatrix, PointFx point)
        {
            System.Drawing.PointF[] pts = { new System.Drawing.PointF(point.X, point.Y) };
            System.Drawing.PointF[] newPts = TransformPoints(tempMatrix, pts);
            return new PointFx(newPts[0].X, newPts[0].Y);
        }

        internal static SizeFx Transform(Matrix tempMatrix, SizeFx point)
        {
            System.Drawing.PointF[] pts = { new System.Drawing.PointF(point.Width, point.Height) };
            System.Drawing.PointF[] newPts = TransformPoints(tempMatrix, pts);
            return new SizeFx(newPts[0].X, newPts[0].Y);
        }

        internal static RectangleFx Transform(Matrix matrix, RectangleFx rect)
        {
            
            float x = rect.X;
            float y = rect.Y;
            float width = rect.Width;
            float height = rect.Height;

            PTS4[0].X = x;
            PTS4[0].Y = y;
            PTS4[1].X = x + width;
            PTS4[1].Y = y;
            PTS4[2].X = x + width;
            PTS4[2].Y = y + height;
            PTS4[3].X = x;
            PTS4[3].Y = y + height;

            PTS4= MatrixExtensions.TransformPoints(matrix, PTS4);

            float minX = PTS4[0].X;
            float minY = PTS4[0].Y;
            float maxX = PTS4[0].X;
            float maxY = PTS4[0].Y;

            for (int i = 1; i < 4; i++)
            {
                x = PTS4[i].X;
                y = PTS4[i].Y;

                if (x < minX)
                {
                    minX = x;
                }
                if (y < minY)
                {
                    minY = y;
                }
                if (x > maxX)
                {
                    maxX = x;
                }
                if (y > maxY)
                {
                    maxY = y;
                }
            }

            RectangleFx ret = new RectangleFx(minX, minY, maxX - minX, maxY - minY);
            return rect;
        }

        public static System.Drawing.PointF[] TransformPoints(Matrix matrix, System.Drawing.PointF[] inPts) 
        {
            System.Drawing.PointF[] outPts = new System.Drawing.PointF[inPts.Length];
			float[] elements = MatrixExtensions.GetElements(matrix);
			
			float x, y;
			int count = inPts.Length;
			for (int i = 0; i < count; i++) 
            {
                x = elements[0] * inPts[i].X + -elements[1] * inPts[i].Y + elements[4];
                y = -elements[2] * inPts[i].X + elements[3] * inPts[i].Y + elements[5];

				outPts[i].X = x;
				outPts[i].Y = y;
			}

            return outPts;
		}

        public static RectangleFx InverseTransform(Matrix matrix, RectangleFx rect)
        {
            if (matrix.Determinant() != 0.0f)
            {
                Matrix tempMatrix = Matrix.Identity;
                tempMatrix = Matrix.Multiply(tempMatrix, matrix);
                tempMatrix = Matrix.Invert(tempMatrix);
                rect = MatrixExtensions.Transform(tempMatrix, rect);
            }
            return rect;
        }

        internal static PointFx InverseTransform(Matrix matrix, PointFx point)
        {
            if (matrix.Determinant() != 0.0f)
            {
                Matrix tempMatrix = Matrix.Identity;
                tempMatrix = Matrix.Multiply(tempMatrix, matrix);
                tempMatrix = Matrix.Invert(tempMatrix);
                point = MatrixExtensions.Transform(tempMatrix, point);

            }
            return point;
        }

        internal static SizeFx InverseTransform(Matrix matrix, SizeFx point)
        {
            if (matrix.Determinant() != 0.0f)
            {
                Matrix tempMatrix = Matrix.Identity;
                tempMatrix = Matrix.Multiply(tempMatrix, matrix);
                tempMatrix = Matrix.Invert(tempMatrix);
                point = MatrixExtensions.Transform(tempMatrix, point);

            }
            return point;
        }

        internal static float GetScale(Matrix matrix)
        {
            System.Drawing.PointF[] pts = {new System.Drawing.PointF(0, 0), new System.Drawing.PointF(1, 0)};
            System.Drawing.PointF[] newPts = MatrixExtensions.TransformPoints(matrix, pts);
            return DistanceBetweenPoints(newPts[0], newPts[1]);
        }

        internal static float GetRotation(Matrix matrix)
        {
            PointFx p1 = new PointFx(0, 0);
            PointFx p2 = new PointFx(1, 0);
            PointFx tp1 = MatrixExtensions.Transform(matrix, p1);
            PointFx tp2 = MatrixExtensions.Transform(matrix, p2);

            double dy = Math.Abs(tp2.Y - tp1.Y);
            float l = DistanceBetweenPoints(tp1, tp2);
            double rotation = Math.Asin(dy / l);

            // correct for quadrant
            if (tp2.Y - tp1.Y > 0)
            {
                if (tp2.X - tp1.X < 0)
                {
                    rotation = Math.PI - rotation;
                }
            }
            else
            {
                if (tp2.X - tp1.X > 0)
                {
                    rotation = 2 * Math.PI - rotation;
                }
                else
                {
                    rotation = rotation + Math.PI;
                }
            }

            // convert to degrees
            return (float)(rotation * (180 / Math.PI));
        }

        /// <summary>
        /// Returns the geometric distance between the two given points.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>The distance between p1 and p2.</returns>
        public static float DistanceBetweenPoints(PointFx p1, PointFx p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        /// <summary>
        /// Returns the geometric distance between the two given points.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>The distance between p1 and p2.</returns>
        public static float DistanceBetweenPoints(System.Drawing.PointF p1, System.Drawing.PointF p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        internal static Matrix SetRotation(Matrix m, float theta)
        {
            //if (theta == 0.0f)
            //    return m;

            float Rotation = GetRotation(m);
            return RotateBy(m, theta - Rotation);
        }

        /// <summary>
        /// Applies the specified scale vector to this <see cref="PMatrix"/> object by
        /// prepending the scale vector.
        /// </summary>
        /// <param name="scale">
        /// The value by which to scale this <see cref="PMatrix"/> along both axes.
        /// </param>
        /// <remarks>
        /// This value will be applied to the current scale value of the matrix.  This is not
        /// the same as setting the <see cref="PMatrix.Scale"/> directly.
        /// </remarks>
        internal static Matrix ScaleBy(Matrix matrix, float scale)
        {
            return MatrixExtensions.ScaleBy(matrix, scale, scale);
        }

        /// <summary>
        /// Applies the specified scale vector to this <see cref="PMatrix"/> object by
        /// prepending the scale vector.
        /// </summary>
        /// <param name="scaleX">
        /// The value by which to scale this <see cref="PMatrix"/> in the x-axis direction. 
        /// </param>
        /// <param name="scaleY">
        /// The value by which to scale this <see cref="PMatrix"/> in the y-axis direction. 
        /// </param>
        /// <remarks>
        /// This value will be applied to the current scale value of the matrix.  This is not
        /// the same as setting the <see cref="PMatrix.Scale"/> directly.
        /// </remarks>
        internal static Matrix ScaleBy(Matrix matrix, float scaleX, float scaleY)
        {
            return MatrixExtensions.Scale(matrix, scaleX, scaleY);
        }

        internal static Matrix Scale(Matrix matrix, float scaleX, float scaleY)
        {
            Vector3 vector = new Vector3(scaleX, scaleY, 0.0f);
            return Matrix.CreateScale(vector);
        }

        /// <summary>
        /// Scale about the specified point.  Applies the specified scale vector to this
        /// <see cref="PMatrix"/> object by translating to the given point before prepending the
        /// scale vector.
        /// </summary>
        /// <param name="scale">
        /// The value by which to scale this <see cref="PMatrix"/> along both axes, around the point
        /// (x, y).
        /// </param>
        /// <param name="x">
        /// The x-coordinate of the point to scale about.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the point to scale about.
        /// </param>
        /// <remarks>
        /// This value will be applied to the current scale value of the matrix.  This is not
        /// the same as setting the <see cref="PMatrix.Scale"/> directly.
        /// </remarks>
        internal static Matrix ScaleBy(Matrix matrix, float scale, float x, float y)
        {
            Matrix tempMatrix = Matrix.Identity;
            tempMatrix = MatrixExtensions.Translate(matrix, x, y);
            tempMatrix = MatrixExtensions.ScaleBy(matrix, scale);
            tempMatrix = MatrixExtensions.Translate(matrix, -x, -y);
            return tempMatrix;
        }

        internal static Matrix Translate(Matrix matrix, float x, float y)
        {
            Vector3 vector = new Vector3(x, y, 0.0f);
            return Matrix.CreateTranslation(vector);
        }

        /// <summary>
        /// Applies the specified translation vector (dx and dy) to this <see cref="PMatrix"/>
        /// object by prepending the translation vector.
        /// </summary>
        /// <param name="dx">
        /// The x value by which to translate this <see cref="PMatrix"/>.
        /// </param>
        /// <param name="dy">
        /// The y value by which to translate this <see cref="PMatrix"/>.
        /// </param>
        internal static Matrix TranslateBy(Matrix matrix, float dx, float dy)
        {
            return MatrixExtensions.Translate(matrix, dx, dy);
        }

        /// <summary>
        /// Prepend to this <see cref="PMatrix"/> object a clockwise rotation, around the origin
        /// and by the specified angle.
        /// <see cref="PMatrix"/> object.
        /// </summary>
        /// <param name="theta">The angle of the rotation, in degrees.</param>
        /// <remarks>
        /// This value will be applied to the current rotation value of the matrix.  This is not
        /// the same as setting the <see cref="PMatrix.Rotation"/> directly.
        /// </remarks>
        internal static Matrix RotateBy(Matrix matrix, float theta)
        {
            Matrix tempMatrix = Matrix.Identity;
            System.Drawing.Drawing2D.Matrix drawMatrix = new System.Drawing.Drawing2D.Matrix(
                    matrix.M11, matrix.M12,
                    matrix.M21, matrix.M22, 
                    matrix.M41, matrix.M42);
            drawMatrix.Rotate(theta);
            tempMatrix = SetElements(drawMatrix);

            return tempMatrix;
            //MatrixExtensions.Rotate(ref matrix, theta, out outMatrix);
        }

        

        /// <summary>
        /// Applies a clockwise rotation about the specified point to this <see cref="PMatrix"/>
        /// object by prepending the rotation.
        /// </summary>
        /// <param name="theta">The angle of the rotation, in degrees.</param>
        /// <param name="x">The x-coordinate of the point to rotate about.</param>
        /// <param name="y">The y-coordinate of the point to rotate about.</param>
        /// <remarks>
        /// This value will be applied to the current rotation value of the matrix.  This is not
        /// the same as setting the <see cref="PMatrix.Rotation"/> directly.
        /// </remarks>
        internal static Matrix RotateBy(Matrix matrix, float theta, float x, float y)
        {
            Matrix temp = MatrixExtensions.RotateAt(matrix, theta, new PointFx(x, y));
            return temp;
        }

        private static Matrix RotateAt(Matrix matrix, float theta, PointFx pointFx)
        {
            Matrix tempMatrix = Matrix.Identity;
            System.Drawing.Drawing2D.Matrix drawMatrix = new System.Drawing.Drawing2D.Matrix(
                    matrix.M11, matrix.M12,
                    matrix.M21, matrix.M22,
                    matrix.M31, matrix.M32);
            drawMatrix.RotateAt(theta, new System.Drawing.PointF(pointFx.X, pointFx.Y));
            tempMatrix = SetElements(drawMatrix);
            return tempMatrix;
        }

        public static Vector3 Rotate(Vector3 v, Vector3 axis, float angle, Vector3 rotationCentre)
        {
            var result = new Vector3();

            float tr = t(angle);
            float cos = c(angle);
            float sin = s(angle);

            //RotationCentre is the point what should be the center of the rotation. 

            result.X = rotationCentre.X +
                       (a1(angle, axis, tr, cos) * v.X + a2(angle, axis, tr, sin) * v.Y + a3(angle, axis, tr, sin) * v.Z);
            result.Y = rotationCentre.Y +
                       (b1(angle, axis, tr, sin) * v.X + b2(angle, axis, tr, cos) * v.Y + b3(angle, axis, tr, sin) * v.Z);
            result.Z = rotationCentre.Z +
                       (c1(angle, axis, tr, sin) * v.X + c2(angle, axis, tr, sin) * v.Y + c3(angle, axis, tr, cos) * v.Z);

            return result;
        }

        private static float t(float angle)
        {
            return 1 - (float)Math.Cos(angle);
        }

        private static float c(float angle)
        {
            return (float)Math.Cos(angle);
        }

        private static float s(float angle)
        {
            return (float)Math.Sin(angle);
        }

        private static float a1(float angle, Vector3 axis, float tr, float cos)
        {
            return (tr * axis.X * axis.X) + cos;
        }

        private static float a2(float angle, Vector3 axis, float tr, float sin)
        {
            return (tr * axis.X * axis.Y) - (sin * axis.Z);
        }

        private static float a3(float angle, Vector3 axis, float tr, float sin)
        {
            return (tr * axis.X * axis.Z) + (sin * axis.Y);
        }

        private static float b1(float angle, Vector3 axis, float tr, float sin)
        {
            return (tr * axis.X * axis.Y) + (sin * axis.Z);
        }

        private static float b2(float angle, Vector3 axis, float tr, float cos)
        {
            return (tr * axis.Y * axis.Y) + cos;
        }

        private static float b3(float angle, Vector3 axis, float tr, float sin)
        {
            return (tr * axis.Y * axis.Z) - (sin * axis.X);
        }

        private static float c1(float angle, Vector3 axis, float tr, float sin)
        {
            return (tr * axis.X * axis.Z) - (sin * axis.Y);
        }

        private static float c2(float angle, Vector3 axis, float tr, float sin)
        {
            return (tr * axis.Y * axis.Z) + (sin * axis.X);
        }

        private static float c3(float angle, Vector3 axis, float tr, float cos)
        {
            return (tr * axis.Z * axis.Z) + cos;
        }

        
    }
}
