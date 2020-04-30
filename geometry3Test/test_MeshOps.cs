﻿using g3;
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

        internal static void test_cut_forStudy()
        {
            Console.WriteLine("Testing cut coplanar.");
            DMesh3 b1 = TestUtil.LoadTestInputMesh("Tri1.obj");
            DMesh3 b2 = TestUtil.LoadTestInputMesh("Tri2.obj");

            Stopwatch s = new Stopwatch();
            s.Start();
            var meshCut = new MeshMeshCut();
            meshCut.Target = b1;
            meshCut.CutMesh = b2;
            meshCut.Compute();
            meshCut.RemoveContained();
            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            var outF = TestUtil.WriteTestOutputMesh(meshCut.Target, "MeshOps_CutForStudy.obj");
            Console.WriteLine($"Written to: {outF}");
        }


        internal static void test_cut_coplanar()
        {
            Console.WriteLine("Testing cut coplanar.");
            var b1= test_Bool.MakeBox(
                center: new Vector3d(0, 0, 0),
                size: new Vector3d(1, 1, 1)
                );
            var b2 = test_Bool.MakeBox(
                center: new Vector3d(0, 0, .5),
                size: new Vector3d(1.0, 1.0, 1)
                );

            Stopwatch s = new Stopwatch();
            s.Start();
            var meshCut = new MeshMeshCut();
            meshCut.Target = b1;
            meshCut.CutMesh = b2;
            meshCut.Compute();
            meshCut.RemoveContained();
            Console.WriteLine($"Done in {s.ElapsedMilliseconds} ms. ");
            var outF = TestUtil.WriteTestOutputMesh(meshCut.Target, "MeshOps_CutCoplanar.obj");
            Console.WriteLine($"Written to: {outF}");
        }



        internal static void test_cut_contained()
        {
            Console.WriteLine("Testing cut Contained.");
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
            Console.WriteLine("Testing cut external.");
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

        internal static void test_MergeCoincidentEdges()
        {
            var tests = new[]
            {
                (file: "box1_NeedHealing.obj", count: 8), // this works with a simple apply
                (file: "NeedHealing.obj", count: 16)      // this works with Apply Iteratively
            };

            Console.WriteLine("test_MergeCoincidentEdges...");

            foreach (var test in tests)
            {
                Console.Write($" - {test.file}... ");
                DMesh3 b1 = TestUtil.LoadTestInputMesh(test.file);
                MergeCoincidentEdges merg = new MergeCoincidentEdges(b1);
                merg.ApplyIteratively();
                if (b1.VertexCount != test.count)
                {
                    var outF = TestUtil.WriteTestOutputMesh(b1, "MeshOps_MergeCoincidentEdges.obj");
                    Console.WriteLine($"Error, shape written to: {outF}");
                    break;
                }
                else
                {
                    Console.WriteLine("Ok");
                }
            }
        }
    }
}
