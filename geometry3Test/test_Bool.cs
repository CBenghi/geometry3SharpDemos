using g3;
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
            mBool.Compute();
            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(mBool.Result) }, WriteOptions.Defaults);
            Console.WriteLine($"{result.message } file: {pOut}");
        }
    }
}