using g3;
using g3.mesh;
using gs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace geometry3Test
{
    internal class test_Bool
    {
        private static DMesh3 TestFiles(string file1, string file2, MeshBoolean.boolOperation op)
        {
            Console.WriteLine($"Testing {op} on : {file1}, {file2}");
            DMesh3 b1 = TestUtil.LoadTestInputMesh(file1);
            DMesh3 b2 = TestUtil.LoadTestInputMesh(file2);
            Stopwatch s = new Stopwatch();
            s.Start();
            var ret = PerformBoolean(b1, b2, op);
            s.Stop();
            if (!ret.IsClosed())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Mesh is not closed.");
                Console.ResetColor();
            }
            var outName = $"Bool{op}_{file1}_{file2}".Replace(".obj", "");
            var outF = TestUtil.WriteTestOutputMesh(ret, outName + ".obj");
            Console.WriteLine($"Model: {outF} in {s.ElapsedMilliseconds} ms.");
            return ret;
        }

        private static DMesh3 PerformBoolean(DMesh3 b1, DMesh3 b2, MeshBoolean.boolOperation op)
        {
            var mBool = new MeshBoolean();
            mBool.Target = b1;
            mBool.Tool = b2;
            mBool.Compute(op);
            
            PlanarRemesher p = new PlanarRemesher(mBool.Result);
            p.Remesh();

            MergeCoincidentEdges mrg = new MergeCoincidentEdges(mBool.Result);
            mrg.ApplyIteratively();

            MeshRepairOrientation rep = new MeshRepairOrientation(mBool.Result);
            rep.OrientComponents();
            rep.SolveGlobalOrientation();

            return mBool.Result;
        }

        internal static void test_all()
        {
            test_union();
            test_subtraction();
            test_subtraction2();
            test_intersection();
        }

        internal static void test_union()
        {
            TestFiles("box1.obj", "box2.obj", MeshBoolean.boolOperation.Union);
        }

        internal static void test_subtraction()
        {
            TestFiles("box1.obj", "box2.obj", MeshBoolean.boolOperation.Subtraction);
        }
        internal static void test_subtraction2()
        {
            TestFiles("box1.obj", "boxThinTall.obj", MeshBoolean.boolOperation.Subtraction);
        }

        internal static void test_intersection()
        {
            TestFiles("box1.obj", "box2.obj", MeshBoolean.boolOperation.Intersection);
        }
    }
}