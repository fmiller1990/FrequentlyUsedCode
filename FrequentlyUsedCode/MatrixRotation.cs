using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequentlyUsedCode
{
    class MatrixRotation
    {
        /// <summary>
        /// Rotate a vector
        /// </summary>
        /// <param name="displacement"></param>
        /// <returns>new displaced vector</returns>
        public Vector Rotate(double angleInRadians, AxesOfRotation axis) {
            var R = GetRotationMatrix(angleInRadians, axis);
            return new Vector(
                R[0, 0] * X + R[0, 1] * Y + R[0, 2] * Z,
                R[1, 0] * X + R[1, 1] * Y + R[1, 2] * Z,
                R[2, 0] * X + R[2, 1] * Y + R[2, 2] * Z);
        }

        #region Private Methods
        /// <summary>
        /// Obtain the rotation matrix
        /// </summary>
        private double[,] GetRotationMatrix(double angleInRadians, AxesOfRotation axis) {
            double cos = Math.Cos(angleInRadians);
            double sin = Math.Sin(angleInRadians);
            switch (axis) {
                case AxesOfRotation.X:
                    return new double[,] {
                    { 1.0, 0.0, 0.0 },
                    { 0.0, cos,-sin },
                    { 0.0, sin, cos }};
                case AxesOfRotation.Y:
                    return new double[,] {
                    { cos, 0.0, sin },
                    { 0.0, 1.0, 0.0 },
                    {-sin, 0.0, cos }};
                case AxesOfRotation.Z:
                    return new double[,] {
                    { cos,-sin, 0.0 },
                    { sin, cos, 0.0 },
                    { 0.0, 0.0, 1.0 }};
                default:
                    throw new ArgumentException(String.Format("Axis '{0}' is not supported!", axis.ToString()));
            };
        }
        #endregion



    }
}
