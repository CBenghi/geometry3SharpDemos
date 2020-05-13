using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geometry3Test
{
    public class test_MeshCutPrecursors
    {

        public static void all()
        {
            // transform();
            // test_meshGen1();
            // test_meshGen2();
            test_meshGen3();
        }



        private static void test_meshGen2()
        {
            Console.WriteLine("test_meshGen2");
            TriangulatedPolygonGenerator tg = new TriangulatedPolygonGenerator();
            tg.Polygon = new GeneralPolygon2d(new Polygon2d(
                new List<Vector2d>()
                {
                    // clockwise
                    new Vector2d(0,0),
                    new Vector2d(0,9),
                    new Vector2d(9,9),
                    new Vector2d(7,7),
                    new Vector2d(5,7),
                    new Vector2d(2,7),
                    new Vector2d(2,4),
                    new Vector2d(2,2)
                }
                ));
            tg.Generate();
            var mesh = tg.MakeDMesh();

            Console.WriteLine(TestUtil.WriteTestOutputMesh(mesh));
        }
        private static void test_meshGen3()
        {
            Console.WriteLine("test_meshGen3");
            TriangulatedPolygonGenerator tg = new TriangulatedPolygonGenerator();
            var poly = new Polygon2d(
                new List<Vector2d>()
                {
                    // clockwise
                    new Vector2d(0,0),
                    new Vector2d(0,9),
                    new Vector2d(9,9),
                    new Vector2d(7,7),
                    new Vector2d(6.99999,7),
                    new Vector2d(2,7),
                    new Vector2d(2,2.00001),
                    new Vector2d(2,2)
                });

            tg.Polygon = new GeneralPolygon2d(poly);
            tg.Generate();
            var mesh = tg.MakeDMesh();

            Console.WriteLine(TestUtil.WriteTestOutputMesh(mesh));
        }

        private static void test_meshGen1()
        {
            Console.WriteLine("test_meshGen1");
            TriangulatedPolygonGenerator tg = new TriangulatedPolygonGenerator();
            tg.Polygon = new GeneralPolygon2d(new Polygon2d(
                new List<Vector2d>()
                {
                    // clockwise
                    new Vector2d(0,0),
                    new Vector2d(0,4),
                    new Vector2d(5,4),
                    new Vector2d(5,0)
                }
                ));
            tg.Polygon.AddHole(new Polygon2d(
                new List<Vector2d>()
                {
                    // counter-clockwise
                    new Vector2d(1,1),
                    new Vector2d(2,1),
                    new Vector2d(1,2)
                }
                ));
            tg.Generate();
            var m = tg.MakeDMesh();
            Console.WriteLine(TestUtil.WriteTestOutputMesh(m));

        }

        public static void transform()
        {
            // test math to rotate normal to normal
            var from = new Vector3d(0, -1, 0);
            var to = new Vector3d(0, 0, 1);
            var q = new Quaterniond(from, to);  // direct transform
            var test = new Vector3d(1, 0, 1); // test vector
            var transformed = q * test;
            var q1 = q.Inverse();
            var reTransformed = q1 * transformed;
        }

        
    }
}
