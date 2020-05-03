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
            var ret = ComputeBoolean(b1, b2, op, true);
            IsNotNullAndClosed(ret);
            return ret;
        }


        internal static void test_all()
        {
            test_coplanar_tetra();
            test_coplanar_box();
            test_multiple();
            test_union();
            test_subtraction();
            test_subtraction2();
            test_intersection();
        }

        internal static void test_coplanar_tetra()
        {
            Console.WriteLine("Testing: test_coplanar_tetra");
            
            var shape = test_MeshOps.MakeTetra(new Vector3d(0, 0, 0), 2);
            var toolShape = test_MeshOps.MakeTetra(new Vector3d(0, 0, 1), 2);
            DMesh3 ret = ComputeBoolean(shape, toolShape, MeshBoolean.boolOperation.Subtraction, true);
            IsNotNullAndClosed(ret);

            // reset and try other operation
            shape = test_MeshOps.MakeTetra(new Vector3d(0, 0, 0), 2);
            toolShape = test_MeshOps.MakeTetra(new Vector3d(0, 0, 1), 2);
            ret = ComputeBoolean(shape, toolShape, MeshBoolean.boolOperation.Intersection, true);
            IsNotNullAndClosed(ret);

            // reset and try other operation
            shape = test_MeshOps.MakeTetra(new Vector3d(0, 0, 0), 2);
            toolShape = test_MeshOps.MakeTetra(new Vector3d(0, 0, 1), 2);
            ret = ComputeBoolean(shape, toolShape, MeshBoolean.boolOperation.Union, true);
            IsNotNullAndClosed(ret);

            Console.WriteLine("Done");
        }

        static bool SaveSuccessfulBooleanTests = true;

        private static bool IsNotNullAndClosed(DMesh3 ret)
        {
            if (ret == null)
            {
                TestUtil.ConsoleError($"Null return from test.");
                return false;
            }
            if (!ret.IsClosed())
            {
                var outF = TestUtil.WriteTestOutputMesh(ret);
                TestUtil.ConsoleError($"Error in boolean: test_coplanar_tetra: {outF}");
                return false;
            }
            if (SaveSuccessfulBooleanTests)
            {
                var outF = TestUtil.WriteTestOutputMesh(ret);
                Console.WriteLine($"Ok: {outF}");
            }
            return true;
        }

        internal static void test_coplanar_box()
        {
            var outer = MakeBox(
                 center: new Vector3d(0, 0, 0),
                 size: new Vector3d(1, 1, 1)
                 );
            var hole = MakeBox(
                center: new Vector3d(0, 0, .5),
                size: new Vector3d(1, 1, 1)
                );
            DMesh3 ret = ComputeBoolean(outer, hole, MeshBoolean.boolOperation.Subtraction, true);
            IsNotNullAndClosed(ret);
        }

        public static void test_multiple()
        {
            var outer = MakeBox(
                center: new Vector3d(0, 0, 0),
                size: new Vector3d(3, 3, 1)
                );
            var hole = MakeBox(
                center: new Vector3d(0, 0, 0),
                size: new Vector3d(1, 1, 2)
                );
            DMesh3 ret = ComputeBoolean(outer, hole, MeshBoolean.boolOperation.Subtraction, true);

            List<DMesh3> cuts = new List<DMesh3>();

            cuts.Add(MakeBox(
                center: new Vector3d(1.5, 1.5, 0),
                size: new Vector3d(.5, .5, .5)
                ));
            cuts.Add(MakeBox(
                center: new Vector3d(-1.5, -1.5, 0),
                size: new Vector3d(.5, .5, .5)
                ));
            cuts.Add(MakeBox(
                center: new Vector3d(1.5, -1.5, 0),
                size: new Vector3d(.5, .5, .5)
                ));
            cuts.Add(MakeBox(
                center: new Vector3d(-1.5, 1.5, 0),
                size: new Vector3d(.5, .5, .5)
                ));

            foreach (var anotherHole in cuts)
            {
                ret = ComputeBoolean(ret, anotherHole, MeshBoolean.boolOperation.Subtraction, true);
                if (!IsNotNullAndClosed(ret))
                    break;
            }
        }

        private static DMesh3 ComputeBoolean(DMesh3 outer, DMesh3 hole, MeshBoolean.boolOperation op, bool full = true)
        {
            if (!outer.IsClosed() || !hole.IsClosed())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid operand.");
                Console.ResetColor(); 
                return null;
            }
            var mBool = new MeshBoolean();
            mBool.Target = outer;
            mBool.Tool = hole;
            mBool.Compute(op);
            var ret = mBool.Result;
            
            if (full)
            {
                //PlanarRemesher p = new PlanarRemesher(ret);
                //p.Remesh();

                MergeCoincidentEdges mrg = new MergeCoincidentEdges(ret);
                mrg.ApplyIteratively();
                Debug.Write("Closed: " + mBool.Result.IsClosed());

                MeshRepairOrientation rep = new MeshRepairOrientation(ret);
                rep.OrientComponents();
                rep.SolveGlobalOrientation();
            }
            return ret;
        }

        internal static DMesh3 MakeBox(Vector3d center, Vector3d size)
        {
            TrivialBox3Generator boxgen = new TrivialBox3Generator();
            boxgen.WantNormals = false;
            boxgen.Box = new Box3d(center, size / 2);
            boxgen.Generate();
            DMesh3 mesh = new DMesh3(false, false, false, false);
            boxgen.MakeMesh(mesh);
            return mesh;
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