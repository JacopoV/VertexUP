using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using VertexUP.GameObjects;

namespace VertexUP
{
    public class RenderingPolygon : RenderingBackground
    {
        public BoundingSphere boundingSphereCube;
        public BoundingSphere boundingSphereTetra;
        public Model cube;
        public Model tetra;
        public ModelMeshCollection cubeMesh;
        public ModelMeshCollection tetraMesh;

        public Texture2D lightGreen, lightBlue;

        public RenderingPolygon(ContentManager Content) : base(Content)
        {

            lightGreen = content.Load<Texture2D>("Textures/green_texture");
            lightBlue = content.Load<Texture2D>("Textures/blue_texture");

            cube = content.Load<Model>("Models/Cube");
            tetra = content.Load<Model>("Models/Tetra");
            cubeMesh = cube.Meshes;
            tetraMesh = tetra.Meshes;

            boundingSphereCube = new BoundingSphere();
            foreach (ModelMesh mesh in cubeMesh)
            {
                if (boundingSphereCube.Radius == 0)
                    boundingSphereCube = mesh.BoundingSphere;
                else
                    boundingSphereCube = BoundingSphere.CreateMerged(boundingSphereCube, mesh.BoundingSphere);
            }

            boundingSphereTetra = new BoundingSphere();
            foreach (ModelMesh mesh in tetraMesh)
            {
                if (boundingSphereTetra.Radius == 0)
                    boundingSphereTetra = mesh.BoundingSphere;
                else
                    boundingSphereTetra = BoundingSphere.CreateMerged(boundingSphereTetra, mesh.BoundingSphere);
            }

            boundingSphereCube.Radius *= 0.8f;
            boundingSphereTetra.Radius *= 0.8f;

        }

        public void Draw()
        {
            base.Draw();

            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;

            for (int i = 0; i < gs.polygons.Count; i++)
            {

                Polygon polygon = gs.polygons.ElementAt(i);
                ModelMeshCollection meshColl = null;
                switch (polygon.objectType)
                {

                    case 0 : //cube
                        meshColl = cubeMesh;
                        break;

                    case 1:  //tetra
                        meshColl = tetraMesh;
                        break;

                }


                foreach (ModelMesh mesh in meshColl)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.TextureEnabled = true;

                        if (polygon.objectType == 0)
                            effect.Texture = lightBlue;
                        else
                            effect.Texture = lightGreen;

                        if (polygon.selected)
                        {
                            if (polygon.objectType == 0)
                              effect.AmbientLightColor = new Vector3(0.99f,0.99f,0.99f);
                            else
                                effect.AmbientLightColor = new Vector3(0.0f, 0.99f, 0.0f);
                        }

                        effect.World = Matrix.CreateRotationX(MathHelper.ToRadians(polygon.degrees.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(polygon.degrees.Z));
                        effect.World *= Matrix.CreateTranslation(polygon.coordinates.X, polygon.coordinates.Y, polygon.coordinates.Z);
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    mesh.Draw();
                }

            }

        }

    }
}
