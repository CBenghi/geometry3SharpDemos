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
