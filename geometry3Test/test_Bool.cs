using g3;
using g3.mesh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace geometry3Test
{
    internal class test_Bool
    {
        internal static void test_union()
        {
            Console.WriteLine("Testing boolean union.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("box1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("box2.obj");
            var pOut = Path.ChangeExtension(Path.GetTempFileName(), "obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var mBool = new MeshBoolean();
            mBool.Target = b1;
            mBool.Tool = b2;
            mBool.Compute(MeshBoolean.boolOperation.Union);

            PlanarRemesher p = new PlanarRemesher(mBool.Result);
            p.Remesh();

            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(mBool.Result) }, WriteOptions.Defaults);
            Console.WriteLine($"{result.message } file: {pOut}");
        }

        internal static void test_subtraction()
        {
            Console.WriteLine("Testing boolean subtraction.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("box1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("box2.obj");
            var pOut = Path.ChangeExtension(Path.GetTempFileName(), "obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var mBool = new MeshBoolean();
            mBool.Target = b1;
            mBool.Tool = b2;
            mBool.Compute(MeshBoolean.boolOperation.Subtraction);

            PlanarRemesher p = new PlanarRemesher(mBool.Result);
            p.Remesh();

            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(mBool.Result) }, WriteOptions.Defaults);
            Console.WriteLine($"{result.message } file: {pOut}");
        }

        internal static void test_intersection()
        {
            Console.WriteLine("Testing boolean intersection.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("box1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("box2.obj");
            var pOut = Path.ChangeExtension(Path.GetTempFileName(), "obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var mBool = new MeshBoolean();
            mBool.Target = b1;
            mBool.Tool = b2;
            mBool.Compute(MeshBoolean.boolOperation.Intersection);

            PlanarRemesher p = new PlanarRemesher(mBool.Result);
            p.Remesh();

            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(mBool.Result) }, WriteOptions.Defaults);
            Console.WriteLine($"{result.message } file: {pOut}");
        }
    }
}