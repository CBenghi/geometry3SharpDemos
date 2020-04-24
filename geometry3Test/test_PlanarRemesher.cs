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
            var files = new List<string>();

            // files.Add("planarRemesher_face.obj");
            // files.Add("planarRemesher_partial.obj");
            files.Add("planarRemesher_3d.obj");
                        
            foreach (var file in files)
            {
                DMesh3 b1 = TestUtil.LoadTestInputMesh(file);
                var pOut = Path.Combine(Path.GetTempPath(), file);
                PlanarRemesher planarRemesher = new PlanarRemesher(b1);
                planarRemesher.Remesh();

                IOWriteResult result = StandardMeshWriter.WriteFile(pOut, new List<WriteMesh>() { new WriteMesh(b1) }, WriteOptions.Defaults);
                Console.WriteLine($"{result.message}, {result.code} file: {pOut}");
            }            
        }
    }
}