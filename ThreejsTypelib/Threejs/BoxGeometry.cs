using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public class BoxGeometry : Geometry
    {

        /// <summary>
        /// width — Width of the sides on the X axis.
        /// height — Height of the sides on the Y axis.
        /// depth — Depth of the sides on the Z axis.
        /// widthSegments — Optional. Number of segmented faces along the width of the sides. Default is 1.
        // /heightSegments — Optional. Number of segmented faces along the height of the sides. Default is 1.
        /// depthSegments — Optional. Number of segmented faces along the depth of the sides. Default is 1.
        /// </summary>
        public BoxGeometry(double width, double height, double depth, int widthSegments = 1, int heightSegments = 1, int depthSegments = 1)
        {

        }
    }
}
