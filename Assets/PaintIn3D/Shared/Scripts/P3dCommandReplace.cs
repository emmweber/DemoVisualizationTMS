﻿using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class manages the replace painting command.</summary>
	public class P3dCommandReplace : P3dCommand
	{
		public P3dHashedTexture Texture;
		public Color            Color;

		public static P3dCommandReplace Instance = new P3dCommandReplace();

		private static Stack<P3dCommandReplace> pool = new Stack<P3dCommandReplace>();
		
		private static Material cachedMaterial;

		private static int cachedMaterialHash;

		public override bool RequireMesh { get { return false; } }

		static P3dCommandReplace()
		{
			BuildMaterial(ref cachedMaterial, ref cachedMaterialHash, "Hidden/Paint in 3D/Replace");
		}

		public static void Blit(RenderTexture renderTexture, Texture texture, Color tint)
		{
			var material = Instance.SetMaterial(texture, tint);

			Instance.Apply(material);

			P3dHelper.Blit(renderTexture, material, Instance.Pass);
		}

		public static void BlitFast(RenderTexture renderTexture, Texture texture, Color tint)
		{
			var material = Instance.SetMaterial(texture, tint);

			Instance.Apply(material);

			Graphics.Blit(default(Texture), renderTexture, material);
		}

		public override void Apply(Material material)
		{
			base.Apply(material);

			material.SetTexture(P3dShader._Texture, Texture);
			material.SetColor(P3dShader._Color, P3dHelper.FromGamma(Color));
		}

		public override void Pool()
		{
			pool.Push(this);
		}

		public override void Transform(Matrix4x4 posMatrix, Matrix4x4 rotMatrix)
		{
		}

		public override P3dCommand SpawnCopy()
		{
			var command = SpawnCopy(pool);

			command.Texture = Texture;
			command.Color   = Color;

			return command;
		}

		public Material SetMaterial(Texture texture, Color color)
		{
			Material = new P3dHashedMaterial(cachedMaterial, cachedMaterialHash);
			Texture  = texture;
			Color    = color;

			return cachedMaterial;
		}
	}
}