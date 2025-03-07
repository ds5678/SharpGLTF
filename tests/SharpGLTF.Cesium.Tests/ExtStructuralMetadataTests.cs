﻿using NUnit.Framework;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using SharpGLTF.Validation;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SharpGLTF.Schema2.Tiles3D
{
    using VBTexture1 = VertexBuilder<VertexPosition, VertexTexture1, VertexEmpty>;

    [Category("Toolkit.Scenes")]
    public class ExtStructuralMetadataTests
    {
        [SetUp]
        public void SetUp()
        {
            Tiles3DExtensions.RegisterExtensions();
        }

        /*
        [Test(Description = "First test with ext_structural_metadata")]
        public void TriangleWithMetadataTest()
        {
            TestContext.CurrentContext.AttachGltfValidatorLinks();
            var material = MaterialBuilder.CreateDefault().WithDoubleSide(true);
            var mesh = new MeshBuilder<VertexPosition>("mesh");
            var prim = mesh.UsePrimitive(material);

            prim.AddTriangle(new VertexPosition(-10, 0, 0), new VertexPosition(10, 0, 0), new VertexPosition(0, 10, 0));

            var scene = new SceneBuilder();
            scene.AddRigidMesh(mesh, Matrix4x4.Identity);
            var model = scene.ToGltf2();

            var schema = new StructuralMetadataSchema();
            schema.Id = "schema_001";
            schema.Name = "schema 001";
            schema.Description = "an example schema";
            schema.Version = "3.5.1";
            var classes = new Dictionary<string, StructuralMetadataClass>();
            var treeClass = new StructuralMetadataClass();
            treeClass.Name = "Tree";
            treeClass.Description = "Woody, perennial plant.";
            classes["tree"] = treeClass;
            var ageProperty = new ClassProperty();
            ageProperty.Description = "The age of the tree, in years";
            ageProperty.Type = ElementType.SCALAR;
            ageProperty.ComponentType = DataType.UINT32;
            ageProperty.Required = true;

            treeClass.Properties.Add("age", ageProperty);

            schema.Classes = classes;

            var propertyTable = new PropertyTable("tree", 1, "PropertyTable");
            var agePropertyTableProperty = model.GetPropertyTableProperty(new List<int>() { 100 });
            propertyTable.Properties.Add("age", agePropertyTableProperty);

            model.SetPropertyTable(propertyTable, schema);

            // create files
            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.AttachToCurrentTest("cesium_ext_structural_metadata_basic_triangle.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_basic_triangle.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_basic_triangle.plotly");
        }*/

        [Test(Description = "ext_structural_metadata with FeatureId Texture and Property Table")]
        // sample see https://github.com/CesiumGS/3d-tiles-samples/tree/main/glTF/EXT_structural_metadata/FeatureIdTextureAndPropertyTable
        public void FeatureIdTextureAndPropertytableTest()
        {
            TestContext.CurrentContext.AttachGltfValidatorLinks();

            var img0 = "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";
            var imageBytes0 = Convert.FromBase64String(img0);
            var imageBuilder0 = ImageBuilder.From(imageBytes0);

            var img1 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAJElEQVR42mNgYmBgoAQzDLwBgwcwY8FDzIDBDRiR8KgBNDAAAOKBAKByX2jMAAAAAElFTkSuQmCC";
            var imageBytes1 = Convert.FromBase64String(img1);
            var imageBuilder1 = ImageBuilder.From(imageBytes1);

            var material = MaterialBuilder
                .CreateDefault()
                .WithMetallicRoughnessShader()
                .WithBaseColor(imageBuilder0, new Vector4(1, 1, 1, 1))
                .WithDoubleSide(true)
                .WithAlpha(Materials.AlphaMode.OPAQUE)
                .WithMetallicRoughness(0, 1)
                .WithMetallicRoughness(imageBuilder1);

            var mesh = VBTexture1.CreateCompatibleMesh("mesh");
            var prim = mesh.UsePrimitive(material);
            prim.AddTriangle(
                new VBTexture1(new VertexPosition(0, 0, 0), new Vector2(0, 1)),
                new VBTexture1(new VertexPosition(1, 0, 0), new Vector2(1, 1)),
                new VBTexture1(new VertexPosition(0, 1, 0), new Vector2(0, 0)));

            prim.AddTriangle(
                new VBTexture1(new VertexPosition(1, 0, 0), new Vector2(1, 1)),
                new VBTexture1(new VertexPosition(1, 1, 0), new Vector2(1, 0)),
                new VBTexture1(new VertexPosition(0, 1, 0), new Vector2(0, 0)));

            var scene = new SceneBuilder();
            scene.AddRigidMesh(mesh, Matrix4x4.Identity);
            var model = scene.ToGltf2();

            // --------------------------------------------------------------

            var rootMetadata = model.UseStructuralMetadata();
            var schema = rootMetadata.UseEmbeddedSchema("FeatureIdTextureAndPropertyTableSchema");

            // define schema

            var buildingComponentsClass = schema
                .UseClassMetadata("buildingComponents")
                .WithNameAndDesc("Building components");

            var componentProp = buildingComponentsClass
                .UseProperty("component")
                .WithNameAndDesc("Component")
                .WithValueType(ElementType.STRING);

            var yearProp = buildingComponentsClass
                .UseProperty("yearBuilt")
                .WithNameAndDesc("Year built")
                .WithValueType(ElementType.SCALAR, DataType.INT16);            

            var propertyTable = buildingComponentsClass
                .AddPropertyTable(4, "Example property table");

            propertyTable
                .UseProperty(componentProp)
                .SetValues1D("Wall", "Door", "Roof", "Window");

            propertyTable
                .UseProperty(yearProp)
                .SetValues1D(1960, 1996, 1985, 2002);            

            // Set the FeatureIds, pointing to the red channel of the texture

            var featureId = new FeatureIDBuilder(propertyTable);            

            var primitive = model.LogicalMeshes[0].Primitives[0];
            primitive.AddMeshFeatureIds((featureId, model.LogicalTextures[0], new[] {0}));

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);

            model.AttachToCurrentTest("cesium_ext_structural_metadata_featureid_texture_and_property_table.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_featureid_texture_and_property_table.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_featureid_texture_and_property_table.plotly");
        }


        [Test(Description = "ext_structural_metadata with simple property texture")]
        // sample see https://github.com/CesiumGS/3d-tiles-samples/tree/main/glTF/EXT_structural_metadata/SimplePropertyTexture
        public void SimplePropertyTextureTest()
        {
            TestContext.CurrentContext.AttachGltfValidatorLinks();

            var img0 = "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";
            var imageBytes0 = Convert.FromBase64String(img0);
            var imageBuilder0 = ImageBuilder.From(imageBytes0);

            var img1 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABLUlEQVR42mVSSxbDIAh0GzUxKZrmCF3n/oerIx9pupgHIswAGtblE7bIKN0vqSOyXSOjPLAtktv9sCFxmcXj7EgsFj8zN00yYxrBZZJBRYk2LdC4WCDUfAdab7bpDm1lCyBW+7lpDnyNS34gcTQRltTPbAeEdFjcSQ0X9EOhGPYjhgLA7xh3kjxEEpMj1qQj7iAzAYoPELzYtuwK02M06WywAFDfX1MdJEoOtSZ7Allz1mYmWZDNL0pNF6ezu9jsQJUcNK7qzbWvMdSYQ8Jo7KKK8/uo4dxreHe0/HgF2/IqBen/za+Di69Sf8cZz5jmk+hcuhdd2tWLz8IE5MbFnRWT+yyU5vZJRtAOqlvq6MDeOrstu0UidsoO0Ak9xGwE+67+34salNEBSCxX7Bexg0rbq6TFvwAAAABJRU5ErkJggg==";
            var imageBytes1 = Convert.FromBase64String(img1);
            var imageBuilder1 = ImageBuilder.From(imageBytes1);

            var material = MaterialBuilder
                .CreateDefault()
                .WithMetallicRoughnessShader()
                .WithBaseColor(imageBuilder0, new Vector4(1, 1, 1, 1))
                .WithDoubleSide(true)
                .WithAlpha(Materials.AlphaMode.OPAQUE)
                .WithMetallicRoughness(0, 1)
                .WithMetallicRoughness(imageBuilder1);

            var mesh = VBTexture1.CreateCompatibleMesh("mesh");
            var prim = mesh.UsePrimitive(material);
            prim.AddTriangle(
                new VBTexture1(new VertexPosition(0, 0, 0), new Vector2(0, 1)),
                new VBTexture1(new VertexPosition(1, 0, 0), new Vector2(1, 1)),
                new VBTexture1(new VertexPosition(0, 1, 0), new Vector2(0, 0)));

            prim.AddTriangle(
                new VBTexture1(new VertexPosition(1, 0, 0), new Vector2(1, 1)),
                new VBTexture1(new VertexPosition(1, 1, 0), new Vector2(1, 0)),
                new VBTexture1(new VertexPosition(0, 1, 0), new Vector2(0, 0)));

            var scene = new SceneBuilder();
            scene.AddRigidMesh(mesh, Matrix4x4.Identity);
            var model = scene.ToGltf2();

            // --------------------------------------------------------------

            var rootMetadata = model.UseStructuralMetadata();
            var schema = rootMetadata.UseEmbeddedSchema("SimplePropertyTextureSchema");

            // define schema 

            var exampleMetadataClass = schema
                .UseClassMetadata("buildingComponents")
                .WithNameAndDesc("Building properties");

            exampleMetadataClass
                .UseProperty("insideTemperature")
                .WithNameAndDesc("Inside temperature")
                .WithValueType(ElementType.SCALAR, DataType.UINT8);

            exampleMetadataClass
                .UseProperty("outsideTemperature")
                .WithNameAndDesc("Outside temperature")
                .WithValueType(ElementType.SCALAR, DataType.UINT8);

            exampleMetadataClass
                .UseProperty("insulation")
                .WithNameAndDesc("Insulation Thickness")
                .WithValueType(ElementType.SCALAR, DataType.UINT8, true);

            // define texture property

            var buildingPropertyTexture = exampleMetadataClass.AddPropertyTexture();    
            
            buildingPropertyTexture.CreateProperty("insideTemperature", model.LogicalTextures[1], new int[] {0});
            buildingPropertyTexture.CreateProperty("outsideTemperature", model.LogicalTextures[1], new int[] {1});
            buildingPropertyTexture.CreateProperty("insulation", model.LogicalTextures[1], new int[] {2});

            // assign to primitive

            var primitive = model.LogicalMeshes[0].Primitives[0];            
            primitive.AddPropertyTexture(buildingPropertyTexture);            

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.AttachToCurrentTest("cesium_ext_structural_metadata_simple_property_texture.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_simple_property_texture.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_simple_property_texture.plotly");
        }

        [Test(Description = "ext_structural_metadata with Multiple Feature IDs and Properties")]
        // sample see https://github.com/CesiumGS/3d-tiles-samples/tree/main/glTF/EXT_structural_metadata/MultipleFeatureIdsAndProperties
        public void MultipleFeatureIdsAndPropertiesTest()
        {
            TestContext.CurrentContext.AttachGltfValidatorLinks();
            var material = MaterialBuilder.CreateDefault().WithDoubleSide(true);
            var mesh = new MeshBuilder<VertexPosition, VertexWithFeatureIds, VertexEmpty>("mesh");
            var prim = mesh.UsePrimitive(material);

            // first triangle has _feature_id_0 = 0 and _feature_id_1 = 1
            var vt0 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 0, 1);
            var vt1 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(1, 0, 0), new Vector3(0, 0, 1), 0, 1);
            var vt2 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(0, 1, 0), new Vector3(0, 0, 1), 0, 1);

            // second triangle has _feature_id_0 = 1 and _feature_id_1 = 0
            var vt3 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(1, 1, 0), new Vector3(0, 0, 1), 1, 0);
            var vt4 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 1, 0);
            var vt5 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(1, 0, 0), new Vector3(0, 0, 1), 1, 0);

            prim.AddTriangle(vt0, vt1, vt2);
            prim.AddTriangle(vt3, vt4, vt5);

            var scene = new SceneBuilder();
            scene.AddRigidMesh(mesh, Matrix4x4.Identity);

            var model = scene.ToGltf2();

            // --------------------------------------------------------------

            var rootMetadata = model.UseStructuralMetadata();
            var schema = rootMetadata.UseEmbeddedSchema("MultipleFeatureIdsAndPropertiesSchema");

            // define schema

            var exampleMetadataClass = schema
                .UseClassMetadata("exampleMetadataClass")
                .WithNameAndDesc("Example metadata class", "An example metadata class");

            var vec3Property = exampleMetadataClass
                .UseProperty("example_VEC3_FLOAT32")
                .WithNameAndDesc("Example VEC3 FLOAT32 property", "An example property, with type VEC3, with component type FLOAT32")
                .WithValueType(ElementType.VEC3, DataType.FLOAT32);

            var stringProperty = exampleMetadataClass
                .UseProperty("example_STRING")
                .WithNameAndDesc("Example STRING property", "An example property, with type STRING")
                .WithValueType(ElementType.STRING);

            // define table

            var examplePropertyTable = exampleMetadataClass.AddPropertyTable(2, "Example property table");

            examplePropertyTable
                .UseProperty(vec3Property)
                .SetValues1D(new Vector3(3, 3.0999999046325684f, 3.200000047683716f), new Vector3(103, 103.0999999046325684f, 103.200000047683716f));

            examplePropertyTable
                .UseProperty(stringProperty)
                .SetValues1D("Rain 🌧", "Thunder ⛈");

            // assign to primitive

            var featureId0 = new FeatureIDBuilder(examplePropertyTable, 0);
            var featureId1 = new FeatureIDBuilder(examplePropertyTable, 1);

            model.LogicalMeshes[0].Primitives[0].AddMeshFeatureIds( (featureId0, null, null), (featureId1, null, null) );

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.AttachToCurrentTest("cesium_ext_structural_metadata_featureid_attribute_and_property_table.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_featureid_attribute_and_property_table.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_featureid_attribute_and_property_table.plotly");
        }

        // sample see https://github.com/CesiumGS/3d-tiles-samples/tree/main/glTF/EXT_structural_metadata/FeatureIdAttributeAndPropertyTable
        [Test(Description = "ext_structural_metadata with FeatureIdAttributeAndPropertyTable")]        
        public void FeatureIdAndPropertyTableTest()
        {
            TestContext.CurrentContext.AttachGltfValidatorLinks();
            var material = MaterialBuilder.CreateDefault().WithDoubleSide(true);
            var mesh = new MeshBuilder<VertexPosition, VertexWithFeatureId, VertexEmpty>("mesh");
            var prim = mesh.UsePrimitive(material);

            // All the vertices in the triangle have the same feature ID
            var vt0 = VertexBuilder.GetVertexWithFeatureId(new Vector3(-1, 0, 0), new Vector3(0, 0, 1), 0);
            var vt1 = VertexBuilder.GetVertexWithFeatureId(new Vector3(1, 0, 0), new Vector3(0, 0, 1), 0);
            var vt2 = VertexBuilder.GetVertexWithFeatureId(new Vector3(0, 1, 0), new Vector3(0, 0, 1), 0);

            prim.AddTriangle(vt0, vt1, vt2);

            var scene = new SceneBuilder();
            scene.AddRigidMesh(mesh, Matrix4x4.Identity);

            var model = scene.ToGltf2();

            // --------------------------------------------------------------

            var rootMetadata = model.UseStructuralMetadata();
            var schema = rootMetadata.UseEmbeddedSchema("FeatureIdAttributeAndPropertyTableSchema");

            // define schema

            var exampleMetadataClass = schema
                .UseClassMetadata("exampleMetadataClass")
                .WithNameAndDesc("Example metadata class", "An example metadata class");

            var vector3Property = exampleMetadataClass
                .UseProperty("example_VEC3_FLOAT32")
                .WithNameAndDesc("Example VEC3 FLOAT32 property", "An example property, with type VEC3, with component type FLOAT32")
                .WithValueType(ElementType.VEC3, DataType.FLOAT32);

            var matrix4x4Property = exampleMetadataClass
                .UseProperty("example_MAT4_FLOAT32")
                .WithNameAndDesc("Example MAT4 FLOAT32 property", "An example property, with type MAT4, with component type FLOAT32")
                .WithValueType(ElementType.MAT4, DataType.FLOAT32);

            // define table

            var examplePropertyTable = exampleMetadataClass.AddPropertyTable(1, "Example property table");

            examplePropertyTable
                .UseProperty(vector3Property)
                .SetValues1D(new Vector3(3, 3.0999999046325684f, 3.200000047683716f));

            examplePropertyTable
                .UseProperty(matrix4x4Property)
                .SetValues1D(Matrix4x4.Identity);

            // assign to primitive

            var featureId = new FeatureIDBuilder(examplePropertyTable, 0);

            model.LogicalMeshes[0].Primitives[0].AddMeshFeatureIds((featureId, null, null));

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.AttachToCurrentTest("cesium_ext_structural_metadata_multiple_featureids_and_properties.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_multiple_featureids_and_properties.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_multiple_featureids_and_properties.plotly");
        }

        // sample see https://github.com/CesiumGS/3d-tiles-samples/blob/main/glTF/EXT_structural_metadata/ComplexTypes/
        [Test(Description = "ext_structural_metadata with complex types")]        
        public void ComplexTypesTest()
        {
            TestContext.CurrentContext.AttachGltfValidatorLinks();
            var material = MaterialBuilder.CreateDefault().WithDoubleSide(true);
            var mesh = new MeshBuilder<VertexPosition, VertexWithFeatureId, VertexEmpty>("mesh");
            var prim = mesh.UsePrimitive(material);

            // All the vertices in the triangle have the same feature ID
            var vt0 = VertexBuilder.GetVertexWithFeatureId(new Vector3(-1, 0, 0), new Vector3(0, 0, 1), 0);
            var vt1 = VertexBuilder.GetVertexWithFeatureId(new Vector3(1, 0, 0), new Vector3(0, 0, 1), 0);
            var vt2 = VertexBuilder.GetVertexWithFeatureId(new Vector3(0, 1, 0), new Vector3(0, 0, 1), 0);

            prim.AddTriangle(vt0, vt1, vt2);

            var scene = new SceneBuilder();
            scene.AddRigidMesh(mesh, Matrix4x4.Identity);

            var model = scene.ToGltf2();

            // --------------------------------------------------------------

            var rootMetadata = model.UseStructuralMetadata();
            var schema = rootMetadata.UseEmbeddedSchema("FeatureIdAttributeAndPropertyTableSchema");

            // define schema            

            var exampleMetadataClass = schema
                .UseClassMetadata("exampleMetadataClass")
                .WithNameAndDesc("Example metadata class A", "First example metadata class");

            // enums

            var exampleEnum = schema.UseEnumMetadata("exampleEnumType", ("ExampleEnumValueA", 0), ("ExampleEnumValueB", 1), ("ExampleEnumValueC", 2));

            // class properties

            var uint8ArrayProperty = exampleMetadataClass
                .UseProperty("example_variable_length_ARRAY_normalized_UINT8")
                .WithNameAndDesc("Example variable-length ARRAY normalized INT8 property","An example property, with type ARRAY, with component type UINT8, normalized, and variable length")
                .WithArrayType(ElementType.SCALAR,DataType.UINT8,false);

            var fixedLengthBooleanProperty = exampleMetadataClass
                .UseProperty("example_fixed_length_ARRAY_BOOLEAN")
                .WithNameAndDesc("Example fixed-length ARRAY BOOLEAN property", "An example property, with type ARRAY, with component type BOOLEAN, and fixed length ")
                .WithArrayType(ElementType.BOOLEAN, null, false, 4);

            var variableLengthStringArrayProperty = exampleMetadataClass
                .UseProperty("example_variable_length_ARRAY_STRING")
                .WithNameAndDesc("Example variable-length ARRAY STRING property", "An example property, with type ARRAY, with component type STRING, and variable length")
                .WithArrayType(ElementType.STRING);

            var fixed_length_ARRAY_ENUM = exampleMetadataClass
                .UseProperty("example_fixed_length_ARRAY_ENUM")
                .WithNameAndDesc("Example fixed-length ARRAY ENUM property", "An example property, with type ARRAY, with component type ENUM, and fixed length")
                .WithEnumArrayType(exampleEnum, 2);

            // property tables

            var examplePropertyTable = exampleMetadataClass.AddPropertyTable(1, "Example property table");

            // Question: The table declares a feature count of 1, but then, these properties define different number of items
            
            examplePropertyTable
                .UseProperty(uint8ArrayProperty)
                .SetValues1D<byte>(0, 1, 2, 3, 4, 5, 6, 7);

            examplePropertyTable
                .UseProperty(fixedLengthBooleanProperty)
                .SetValues1D<Boolean>(true, false, true, false);

            examplePropertyTable
                .UseProperty(variableLengthStringArrayProperty)
                .SetValues1D("Example string 1", "Example string 2", "Example string 3");

            examplePropertyTable
                .UseProperty(fixed_length_ARRAY_ENUM)
                .SetValues1D<int>(0, 1);

            // add to primitive            

            var featureId = new FeatureIDBuilder(examplePropertyTable, 0);

            model.LogicalMeshes[0].Primitives[0].AddMeshFeatureIds((featureId, null, null));

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.AttachToCurrentTest("cesium_ext_structural_metadata_complex_types.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_complex_types.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_complex_types.plotly");
        }

        // Sample see https://github.com/CesiumGS/3d-tiles-samples/blob/main/glTF/EXT_structural_metadata/MultipleClasses/
        [Test(Description = "ext_structural_metadata with multiple classes")]        
        public void MultipleClassesTest()
        {
            var material = MaterialBuilder.CreateDefault().WithDoubleSide(true);

            var mesh = new MeshBuilder<VertexPositionNormal, VertexWithFeatureIds, VertexEmpty>("mesh");
            var prim = mesh.UsePrimitive(material);

            // All the vertices in the triangle have the same feature ID
            var vt0 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(-10, 0, 0), new Vector3(0, 0, 1), 0, 0);
            var vt1 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(10, 0, 0), new Vector3(0, 0, 1), 0, 0);
            var vt2 = VertexBuilder.GetVertexWithFeatureIds(new Vector3(0, 10, 0), new Vector3(0, 0, 1), 0, 0);

            prim.AddTriangle(vt0, vt1, vt2);
            var scene = new SceneBuilder();
            scene.AddRigidMesh(mesh, Matrix4x4.Identity);
            var model = scene.ToGltf2();

            // --------------------------------------------------------------

            var rootMetadata = model.UseStructuralMetadata();
            var schema = rootMetadata.UseEmbeddedSchema("MultipleClassesSchema");            

            // classes

            var classA = schema
                .UseClassMetadata("exampleMetadataClassA")
                .WithNameAndDesc("Example metadata class A","First example metadata class");

            var classAp0 = classA.UseProperty("example_FLOAT32")
                .WithNameAndDesc("Example FLOAT32 property", "An example property, with component type FLOAT32")
                .WithValueType(ElementType.SCALAR, DataType.FLOAT32);

            var classAp1 = classA.UseProperty("example_INT64")
                .WithNameAndDesc("Example INT64 property", "An example property, with component type INT64")
                .WithValueType(ElementType.SCALAR, DataType.INT64);

            var classB = schema.UseClassMetadata("exampleMetadataClassB")
                .WithNameAndDesc("Example metadata class B", "Second example metadata class");

            var classBp0 = classB.UseProperty("example_UINT16")
                .WithNameAndDesc("Example UINT16 property", "An example property, with component type UINT16")
                .WithValueType(ElementType.SCALAR, DataType.UINT16);

            var classBp1 = classB.UseProperty("example_FLOAT64")
                .WithNameAndDesc("Example FLOAT64 property", "An example property, with component type FLOAT64")
                .WithValueType(ElementType.SCALAR, DataType.FLOAT64);

            // properties

            var firstPropertyTable = classA.AddPropertyTable(1, "First example property table");
            firstPropertyTable.UseProperty(classAp0).SetValues1D<float>(100);
            firstPropertyTable.UseProperty(classAp1).SetValues1D<long>(101);

            var secondPropertyTable = classB.AddPropertyTable(1, "Second example property table");
            secondPropertyTable.UseProperty(classBp0).SetValues1D<ushort>(102);
            secondPropertyTable.UseProperty(classBp1).SetValues1D<double>(103);

            // features

            // FeatureID 0: featureCount=1, attribute=0, porpertyTable=0 
            var featureId0 = new FeatureIDBuilder(firstPropertyTable, 0);
            // FeatureID 1: featureCount=1, attribute=1, porpertyTable=1
            var featureId1 = new FeatureIDBuilder(secondPropertyTable, 1);
            
            model.LogicalMeshes[0].Primitives[0].AddMeshFeatureIds((featureId0, null,null), (featureId1,null,null));

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.AttachToCurrentTest("cesium_ext_structural_metadata_multiple_classes.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_multiple_classes.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_multiple_classes.plotly");
        }

        
        // Sample see https://github.com/CesiumGS/3d-tiles-samples/blob/main/glTF/EXT_structural_metadata/PropertyAttributesPointCloud/
        [Test(Description = "ext_structural_metadata with pointcloud and custom attributes")]
        public void CreatePointCloudWithCustomAttributesTest()
        {
            var material = new MaterialBuilder("material1").WithUnlitShader();
            var mesh = new MeshBuilder<VertexPosition, VertexPointcloud, VertexEmpty>("mesh");
            var pointCloud = mesh.UsePrimitive(material, 1);
            var redColor = new Vector4(1f, 0f, 0f, 1f);
            var rand = new Random();
            for (var x = -10; x < 10; x++)
            {
                for (var y = -10; y < 10; y++)
                {
                    for (var z = -10; z < 10; z++)
                    {
                        // intensity values is based on x-axis values
                        // classification of points is 0 or 1 (random)
                        var vt0 = VertexBuilder.GetVertexPointcloud(new Vector3(x, y, z), redColor, x, rand.Next(0, 2));

                        pointCloud.AddPoint(vt0);
                    }
                }
            }
            var model = ModelRoot.CreateModel();
            model.CreateMeshes(mesh);

            // create a scene, a node, and assign the first mesh (the terrain)
            model.UseScene("Default")
                .CreateNode().WithMesh(model.LogicalMeshes[0]);

            // --------------------------------------------------------------

            var rootMetadata = model.UseStructuralMetadata();

            // external references are problematic because the idea behind SharpGLTF is that all files are loaded into memory, so you don't
            // need to track resources in disk while working with models. The whole mechanism is too complex to be worth the pain of implementing it.
            // so my idea is that the UseExternalSchema returns a ISchemaProxy interface or something like that, that has pretty much the same methods
            // of an actual schema, so the API usage remains the same for both an embedded and an external schema.

            // var schemaUri = new Uri("MetadataSchema.json", UriKind.Relative);            
            // var schemaProxy = rootMetadata.UseExternalSchema(schemaUri);

            var schema = rootMetadata.UseEmbeddedSchema("externalSchema");

            var externalClass = schema.UseClassMetadata("exampleMetadataClass");

            var propertyAttribute = rootMetadata.AddPropertyAttribute(externalClass);

            var intensityProperty = propertyAttribute.CreateProperty("intensity");             
            intensityProperty.Attribute = "_INTENSITY";

            var classificationProperty = propertyAttribute.CreateProperty("classification");
            classificationProperty.Attribute = "_CLASSIFICATION";

            var ctx = new ValidationResult(model, ValidationMode.Strict, true);
            model.AttachToCurrentTest("cesium_ext_structural_metadata_with_pointcloud_attributes.glb");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_with_pointcloud_attributes.gltf");
            model.AttachToCurrentTest("cesium_ext_structural_metadata_with_pointcloud_attributes.plotly");
        }
    }
}
