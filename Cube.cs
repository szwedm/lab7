using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab7 {

    public class Cube {

        protected Matrix worldTranslation = Matrix.Identity;
        protected Matrix worldRotation = Matrix.Identity;
        protected VertexPositionColor[] vertices;
        protected VertexPositionColor[] axisLines;
        protected BasicEffect effect;
        protected Vector3 pos;
        protected float size;
        protected float rotationSpeed;
        protected Color cubeColor;

        public Cube(GraphicsDevice graphicsDevice, Vector3 position, float rotationSpeed, float size, Color color) {
            vertices = new VertexPositionColor[36];
            this.size = size;
            this.pos = position;
            this.rotationSpeed = rotationSpeed;
            this.cubeColor = color;
            buildCube();
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
        }

        private void buildCube() {
            vertices[0] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * 1), cubeColor);
            vertices[1] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * 1), cubeColor);
            vertices[2] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * 1), cubeColor);
            vertices[3] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * 1), cubeColor);
            vertices[4] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * 1), cubeColor);
            vertices[5] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * 1), cubeColor);

            vertices[6] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * 1), cubeColor);
            vertices[7] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * 1), cubeColor);
            vertices[8] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * -1), cubeColor);
            vertices[9] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * -1), cubeColor);
            vertices[10] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * 1), cubeColor);
            vertices[11] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * -1), cubeColor);

            vertices[12] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * -1), cubeColor);
            vertices[13] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * -1), cubeColor);
            vertices[14] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * -1), cubeColor);
            vertices[15] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * -1), cubeColor);
            vertices[16] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * -1), cubeColor);
            vertices[17] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * -1), cubeColor);

            vertices[18] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * -1), cubeColor);
            vertices[19] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * -1), cubeColor);
            vertices[20] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * 1), cubeColor);
            vertices[21] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * 1), cubeColor);
            vertices[22] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * 1), cubeColor);
            vertices[23] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * -1), cubeColor);

            vertices[24] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * 1), cubeColor);
            vertices[25] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * -1), cubeColor);
            vertices[26] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * -1), cubeColor);
            vertices[27] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * -1), cubeColor);
            vertices[28] = new VertexPositionColor(new Vector3(size * -1, size * -1, size * 1), cubeColor);
            vertices[29] = new VertexPositionColor(new Vector3(size * 1, size * -1, size * 1), cubeColor);

            vertices[30] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * 1), cubeColor);
            vertices[31] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * -1), cubeColor);
            vertices[32] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * -1), cubeColor);
            vertices[33] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * -1), cubeColor);
            vertices[34] = new VertexPositionColor(new Vector3(size * 1, size * 1, size * 1), cubeColor);
            vertices[35] = new VertexPositionColor(new Vector3(size * -1, size * 1, size * 1), cubeColor);
        }

        public virtual void Update(Matrix view, Matrix projection) {
            worldTranslation = Matrix.CreateTranslation(pos);

            worldRotation *= Matrix.CreateRotationY(MathHelper.ToRadians(rotationSpeed));

            effect.World = worldRotation * worldTranslation * worldRotation;
            effect.View = view;
            effect.Projection = projection;
        }

        public virtual void Draw() {

            effect.CurrentTechnique.Passes[0].Apply();

            effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
               PrimitiveType.TriangleList,
               vertices,
               0,
               vertices.Length / 3);
        }
    }
}
