﻿using System.Linq;
using System.Numerics;
using NUnit.Framework;
using SharpGLTF.Schema2;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Validation;
using SharpGLTF.Scenes;

namespace SharpGLTF.Schema2.Tiles3D
{

    [Category("Cesium")]
    public partial class ExtPrimitiveOutlineTests
    {
        [SetUp]
        public void SetUp()
        {
            Tiles3DExtensions.RegisterExtensions();
        }

        [Test(Description = "Creates a simple triangle with Cesium outlining")]
        public void CreateCesiumOutlineTriangleScene()
        {
            TestContext.CurrentContext.AttachGltfValidatorLinks();

            var material = MaterialBuilder.CreateDefault();

            var mesh = new MeshBuilder<VertexPosition>("mesh");

            var prim = mesh.UsePrimitive(material);
            prim.AddTriangle(new VertexPosition(-10, 0, 0), new VertexPosition(10, 0, 0), new VertexPosition(0, 10, 0));

            var scene = new SceneBuilder();

            scene.AddRigidMesh(mesh, Matrix4x4.Identity);

            var model = scene.ToGltf2();

            var outlines = new uint[] { 0, 1, 1, 2, 2, 0};            
            model.LogicalMeshes[0].Primitives[0].SetCesiumOutline(outlines);

            var cesiumOutlineExtension = (CesiumPrimitiveOutline)model.LogicalMeshes[0].Primitives[0].Extensions.FirstOrDefault();
            Assert.That(cesiumOutlineExtension.Indices, Is.Not.Null);            
            Assert.That(outlines, Is.EqualTo(cesiumOutlineExtension.Indices.AsIndicesArray()));

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.ValidateContent(ctx.GetContext());

            scene.AttachToCurrentTest("cesium_outline_triangle.glb");
            scene.AttachToCurrentTest("cesium_outline_triangle.gltf");
            scene.AttachToCurrentTest("cesium_outline_triangle.plotly");
        }
    }
}