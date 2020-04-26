using g3;
using g3.mesh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace geometry3Test
{
    internal class test_PlanarRemesher
    {
        internal static void Test_remesh()
        {
            Console.WriteLine("Testing planar remesh.");
            var tests = new[]
            {
                (file: "planarRemesher_face.obj", count: 3),
                (file: "planarRemesher_partial.obj", count: 4),
                (file: "planarRemesher_3d.obj", count: 12)
            };

                        
            foreach (var test in tests)
            {
                DMesh3 b1 = TestUtil.LoadTestInputMesh(test.file);
                var pOut = Path.Combine(Path.GetTempPath(), test.file);
                PlanarRemesher planarRemesher = new PlanarRemesher(b1);
                planarRemesher.Remesh();
                if (test.count != -1)
                {
                    if (test.count != b1.VertexCount)
                    {
                        IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(b1) }, WriteOptions.Defaults);
                        Console.WriteLine($"{result.message}, {result.code} file: {pOut}");
                        throw new Exception("Incorrect vertex count.");
                    }
                }               
            }            
        }
    }
}