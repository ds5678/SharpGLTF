﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using NUnit.Framework;

using SharpGLTF.Schema2;
using SharpGLTF.Validation;

namespace SharpGLTF.Materials
{
    [Category("Toolkit.Materials")]
    public class MaterialBuilderTests
    {
        [Test]
        public void TestMaterialEquality()
        {
            // Checking if two materials are the same or not is conceptually ambiguous.
            // The static method AreEqualByContent allows to check if two materials represent
            // the same physical material, even if they're two different references.
            // ... And we could use it for general equality checks, but then, since
            // MaterialBuilder is NOT inmutable, it can mean that two materials can be equal
            // at a given time, and non equal at another. Furthermore, it would imply having
            // a hash code that changes over time. As a consequence, it could be impossible
            // to use MaterialBuilder as a dictionary Key.

            var srcMaterial = _CreateUnlitMaterial();

            var clnMaterial = srcMaterial.Clone();

            // srcMaterial and clnMaterial are two different objects, so plain equality checks must apply to reference checks
            Assert.That(clnMaterial != srcMaterial);
            Assert.That(clnMaterial, Is.Not.EqualTo(srcMaterial));
            Assert.That(clnMaterial.GetHashCode(), Is.Not.EqualTo(srcMaterial.GetHashCode()));

            // checking the materials represent the same "material" must be made with AreEqualByContent method.
            Assert.That(MaterialBuilder.AreEqualByContent(srcMaterial, clnMaterial));

            var bag = new HashSet<MaterialBuilder>();
            bag.Add(srcMaterial);
            bag.Add(clnMaterial);

            Assert.That(bag, Has.Count.EqualTo(2));
        }

        [Test]
        public void CreateUnlit()
        {
            var material = _CreateUnlitMaterial();

            Assert.That(MaterialBuilder.AreEqualByContent(material, _Schema2Roundtrip(material)));
            Assert.That(MaterialBuilder.AreEqualByContent(material, material.Clone()));
        }        

        [Test]
        public void CreateMetallicRoughness()
        {
            var material = _CreateMetallicRoughnessMaterial();

            Assert.That(MaterialBuilder.AreEqualByContent(material, _Schema2Roundtrip(material)));
            Assert.That(MaterialBuilder.AreEqualByContent(material, material.Clone()));
        }        

        [Test]
        public void CreateClearCoat()
        {
            var material = _CreateClearCoatMaterial();

            Assert.That(MaterialBuilder.AreEqualByContent(material, _Schema2Roundtrip(material)));
            Assert.That(MaterialBuilder.AreEqualByContent(material, material.Clone()));
        }        

        [Test]
        public void CreateVolume()
        {
            var material = _CreateVolumeMaterial();

            Assert.That(MaterialBuilder.AreEqualByContent(material, _Schema2Roundtrip(material)));
            Assert.That(MaterialBuilder.AreEqualByContent(material, material.Clone()));
        }        

        [Test]
        public void CreateSpecularGlossiness()
        {
            var material = _CreateSpecularGlossinessMaterial();

            Assert.That(MaterialBuilder.AreEqualByContent(material, _Schema2Roundtrip(material)));
            Assert.That(MaterialBuilder.AreEqualByContent(material, material.Clone()));
        }        

        [Test]
        public void CreateSpecularGlossinessWithFallback()
        {
            var material = _CreateSpecularGlossinessMaterialWithFallback();

            // check
            Assert.That(MaterialBuilder.AreEqualByContent(material, _Schema2Roundtrip(material)), Is.True);
            Assert.That(MaterialBuilder.AreEqualByContent(material, material.Clone()), Is.True);
        }

        private static MaterialBuilder _CreateUnlitMaterial()
        {
            var assetsPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets");
            var tex1 = System.IO.Path.Combine(assetsPath, "shannon.png");

            var material = new MaterialBuilder("Unlit Material")
                .WithDoubleSide(true) // notice that DoubleSide enables double face rendering. This is an example, but it's usually NOT NECCESARY.
                .WithAlpha(AlphaMode.MASK, 0.7f);

            material.WithUnlitShader()
                .WithBaseColor(tex1, new Vector4(0.7f, 0, 0f, 0.8f));

            return material;
        }

        private static MaterialBuilder _CreateMetallicRoughnessMaterial()
        {
            var assetsPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets");
            var tex1 = System.IO.Path.Combine(assetsPath, "shannon.png");

            var material = new MaterialBuilder("Metallic Roughness Material")
                .WithAlpha(AlphaMode.MASK, 0.6f)
                .WithEmissive(tex1, new Vector3(0.2f, 0.3f, 0.1f), 6)
                .WithNormal(tex1, 0.3f)
                .WithOcclusion(tex1, 0.4f);

            material.WithMetallicRoughnessShader()
                .WithBaseColor(tex1, new Vector4(0.7f, 0, 0f, 0.8f))
                .WithMetallicRoughness(tex1, 0.2f, 0.4f);                

            // example of setting additional parameters for a given channel.
            material.GetChannel(KnownChannel.BaseColor)
                .Texture
                .WithCoordinateSet(1)
                .WithSampler(TextureWrapMode.CLAMP_TO_EDGE, TextureWrapMode.MIRRORED_REPEAT, TextureMipMapFilter.LINEAR_MIPMAP_LINEAR, TextureInterpolationFilter.NEAREST)
                .WithTransform(Vector2.One * 0.2f, Vector2.One * 0.3f, 0.1f, 2);

            return material;
        }

        private static MaterialBuilder _CreateVolumeMaterial()
        {
            var assetsPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets");
            var tex1 = System.IO.Path.Combine(assetsPath, "shannon.png");

            var material = new MaterialBuilder("Volume Material")
                .WithAlpha(AlphaMode.MASK, 0.6f);

            material.WithMetallicRoughnessShader()
                .WithBaseColor(tex1, new Vector4(0.7f, 0, 0f, 0.8f))
                .WithMetallicRoughness(tex1, 0.2f, 0.4f);

            material.WithVolumeAttenuation(Vector3.One * 0.3f, 0.6f)
                .WithVolumeThickness(tex1, 0.4f);

            return material;
        }

        private static MaterialBuilder _CreateIridescenceMaterial()
        {
            var assetsPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets");
            var tex1 = System.IO.Path.Combine(assetsPath, "shannon.png");

            var material = new MaterialBuilder("Volume Material")
                .WithAlpha(AlphaMode.OPAQUE);

            material.WithMetallicRoughnessShader()
                .WithBaseColor(tex1, new Vector4(0.7f, 0, 0f, 0.8f))
                .WithMetallicRoughness(tex1, 0.2f, 0.4f);

            material
                .WithIridiscence(default, 0, 1.3f)
                .WithIridiscenceThickness(default, 100, 400);

            return material;
        }

        private static MaterialBuilder _CreateClearCoatMaterial()
        {
            var assetsPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets");
            var tex1 = System.IO.Path.Combine(assetsPath, "shannon.png");

            var material = new MaterialBuilder("Clear Coat Material")
                .WithAlpha(AlphaMode.MASK, 0.6f)
                .WithEmissive(tex1, new Vector3(0.2f, 0.3f, 0.1f))
                .WithNormal(tex1, 0.3f)
                .WithOcclusion(tex1, 0.4f);

            material.WithMetallicRoughnessShader()
                .WithBaseColor(tex1, new Vector4(0.7f, 0, 0f, 0.8f))
                .WithMetallicRoughness(tex1, 0.2f, 0.4f);

            material.WithClearCoat(tex1, 1)
                .WithClearCoatNormal(tex1)
                .WithClearCoatRoughness(tex1, 1);

            return material;
        }

        [Obsolete("SpecularGlossiness has been deprecated by Khronos")]
        private static MaterialBuilder _CreateSpecularGlossinessMaterialWithFallback()
        {
            var assetsPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets");
            var tex1 = System.IO.Path.Combine(assetsPath, "shannon.webp");
            var tex2 = System.IO.Path.Combine(assetsPath, "shannon.png");

            var primary = new MaterialBuilder("primary")

                // fallback and primary material must have exactly the same properties
                .WithDoubleSide(true)
                .WithAlpha(AlphaMode.MASK, 0.75f)
                .WithEmissive(tex1, new Vector3(0.2f, 0.3f, 0.1f), 4)
                .WithNormal(tex1, 0.3f)
                .WithOcclusion(tex1, 0.4f);

            // primary must use Specular Glossiness shader.
            primary.WithSpecularGlossinessShader()
                .WithDiffuse(tex1, new Vector4(0.7f, 0, 0f, 1.0f))
                .WithSpecularGlossiness(tex1, new Vector3(0.7f, 0, 0f), 0.8f);

            // set fallback textures for engines that don't support WEBP texture format
            primary.GetChannel(KnownChannel.Normal).Texture.FallbackImage = tex2;
            primary.GetChannel(KnownChannel.Emissive).Texture.FallbackImage = tex2;
            primary.GetChannel(KnownChannel.Occlusion).Texture.FallbackImage = tex2;
            primary.GetChannel(KnownChannel.Diffuse).Texture.FallbackImage = tex2;
            primary.GetChannel(KnownChannel.SpecularGlossiness).Texture.FallbackImage = tex2;

            // set fallback material for engines that don't support Specular Glossiness shader.
            primary.WithMetallicRoughnessFallback(tex1, new Vector4(0.7f, 0, 0, 1), String.Empty, 0.6f, 0.7f);
            primary.CompatibilityFallback.GetChannel(KnownChannel.BaseColor).Texture.FallbackImage = tex2;

            return primary;
        }

        [Obsolete("SpecularGlossiness has been deprecated by Khronos")]
        private static MaterialBuilder _CreateSpecularGlossinessMaterial()
        {
            var assetsPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets");
            var tex1 = System.IO.Path.Combine(assetsPath, "shannon.png");

            var material = new MaterialBuilder()
                .WithAlpha(AlphaMode.MASK, 0.6f)
                .WithEmissive(tex1, new Vector3(0.2f, 0.3f, 0.1f))
                .WithNormal(tex1, 0.3f)
                .WithOcclusion(tex1, 0.4f);

            material.WithSpecularGlossinessShader()
                .WithDiffuse(tex1, new Vector4(0.7f, 0, 0f, 0.8f))
                .WithSpecularGlossiness(tex1, new Vector3(0.7f, 0, 0f), 0.8f);

            return material;
        }        

        private static MaterialBuilder _Schema2Roundtrip(MaterialBuilder srcMaterial)
        {
            // converts a MaterialBuilder to a Schema2.Material and back to a MaterialBuilder

            var dstModel = Schema2.ModelRoot.CreateModel();
            var dstMaterial = dstModel.CreateMaterial(srcMaterial.Name);

            srcMaterial.CopyTo(dstMaterial); // copy MaterialBuilder to Schema2.Material.

            var ctx = new ValidationResult(dstModel,ValidationMode.Strict, true);
            dstModel.ValidateReferences(ctx.GetContext());
            dstModel.ValidateContent(ctx.GetContext());

            var rtpMaterial = new MaterialBuilder(dstMaterial.Name);

            dstMaterial.CopyTo(rtpMaterial);// copy Schema2.Material to MaterialBuilder.

            return rtpMaterial;
        }
    }
}
