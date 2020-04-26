using g3;
using gs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geometry3Test
{
    class test_MeshOps
    {
        internal static void test_cut_contained()
        {
            Console.WriteLine("Testing boolean union.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("box1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("box2.obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var mBool = new MeshMeshCut();
            mBool.Target = b1;
            mBool.CutMesh = b2;
            mBool.Compute();
            mBool.RemoveContained();
            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            var outF = TestUtil.WriteTestOutputMesh(mBool.Target, "MeshOps_CutRemoveContained.obj");
            Console.WriteLine($"Written to: {outF}");
        }

        internal static void test_MeshAutoRepair()
        {
            Console.Write("test_MeshAutoRepair...");
            // b1 will has edges that are overlapping, but disconnected, it's the result of a merge
            // I guess autorepair should weld it back

            DMesh3 b1 = TestUtil.LoadTestInputMesh("NeedHealing.obj");
            
            // b2 is the same mesh, but we weld it 
            var b2 = new DMesh3(b1, true);
            MergeCoincidentEdges merg = new MergeCoincidentEdges(b2);
            merg.Apply();

            MeshAutoRepair autoRepair = new MeshAutoRepair(b1);
            autoRepair.Apply();

            var allOk =
                b1.VertexCount == b2.VertexCount
                && b1.EdgeCount == b2.EdgeCount
                && b1.TriangleCount == b2.TriangleCount;
            if (!allOk)
            {
                var outF = TestUtil.WriteTestOutputMesh(b1, "MeshOps_MeshAutoRepair.obj");
                Console.WriteLine($"Error, shape written to: {outF}");
            }
            else
            {
                Console.WriteLine("Ok");
            }
        }

        internal static void test_cut_external()
        {
            Console.WriteLine("Testing boolean union.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("box1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("box2.obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var mBool = new MeshMeshCut();
            mBool.Target = b1;
            mBool.CutMesh = b2;
            mBool.Compute();
            mBool.RemoveExternal();

            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            var outF = TestUtil.WriteTestOutputMesh(mBool.Target, "MeshOps_CutRemoveExternal.obj");
            Console.WriteLine($"Written to: {outF}");
        }


    }
}
