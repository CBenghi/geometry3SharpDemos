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
        internal static void test_cut()
        {
            Console.WriteLine("Testing boolean union.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("box1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("box2.obj");
            var pOut = Path.ChangeExtension(Path.GetTempFileName(), ".cut.obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var mBool = new MeshMeshCut();
            mBool.Target = b1;
            mBool.CutMesh = b2;
            mBool.Compute();
            mBool.RemoveContained();
            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(mBool.Target) }, WriteOptions.Defaults);
            Console.WriteLine($"{result.message}, {result.code} file: {pOut}");
        }

        internal static void test_cut_external()
        {
            Console.WriteLine("Testing boolean union.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("box1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("box2.obj");
            var pOut = Path.Combine(Path.GetTempPath(), "MeshOps.cut_external.obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var mBool = new MeshMeshCut();
            mBool.Target = b1;
            mBool.CutMesh = b2;
            mBool.Compute();
            // mBool.ColorFaces();
            mBool.RemoveExternal();

            //MeshAutoRepair r = new MeshAutoRepair(mBool.Target);
            //r.Apply();
            //FaceGroupOptimizer opt = new FaceGroupOptimizer(mBool.Target);
            //opt.ClipFins(false);
            // test_MeshClean.MeshClean(mBool.Target);

            if (false)
            {
                // fragmentation
                Remesher r = new Remesher(mBool.Target);
                MeshConstraintUtil.FixAllBoundaryEdges(r);
                r.PreventNormalFlips = true;
                r.SetTargetEdgeLength(0.5);
                for (int k = 0; k < 20; ++k)
                    r.BasicRemeshPass();
            }

            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(mBool.Target) }, WriteOptions.Defaults);
            Console.WriteLine($"{result.message}, {result.code} file: {pOut}");
        }
    }
}
