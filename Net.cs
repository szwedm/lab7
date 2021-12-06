using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab6 {

    public class Net {

        private Matrix world = Matrix.Identity;
        private VertexPositionColor[] vertices;
        private VertexPositionColor[] axisLines;
        Vector3 startPos, endPos;
        private BasicEffect effect;

        public Net(GraphicsDevice graphicsDevice, Vector3 startPos, Vector3 endPos) {
            this.startPos = startPos;
            this.endPos = endPos;
            axisLines = new VertexPositionColor[6];
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
        }

        public void Load() {
            LoadCoordinateSystem();
            LoadNet();
        }

        private void LoadCoordinateSystem() {
            axisLines[0] = new VertexPositionColor(-(Vector3.Distance(startPos, endPos) / 2 + 10) * Vector3.UnitX, Color.Red);
            axisLines[1] = new VertexPositionColor((Vector3.Distance(startPos, endPos) / 2 + 10) * Vector3.UnitX, Color.Red);
            axisLines[2] = new VertexPositionColor(-(Vector3.Distance(startPos, endPos) / 2 + 10) * Vector3.UnitY, Color.Green);
            axisLines[3] = new VertexPositionColor((Vector3.Distance(startPos, endPos) / 2 + 10) * Vector3.UnitY, Color.Green);
            axisLines[4] = new VertexPositionColor(-(Vector3.Distance(startPos, endPos) / 2 + 10) * Vector3.UnitZ, Color.Blue);
            axisLines[5] = new VertexPositionColor((Vector3.Distance(startPos, endPos) / 2 + 10) * Vector3.UnitZ, Color.Blue);
        }

        private void LoadNet() {
            List<VertexPositionColor> lines = new List<VertexPositionColor>();
            int count = (int)(endPos.X - startPos.X) / 40;
            for (int i = 0; i <= 40; i++) {
                lines.Add(new VertexPositionColor(new Vector3(startPos.X, startPos.Y, startPos.Z + count * i), Color.White));
                lines.Add(new VertexPositionColor(new Vector3(endPos.X, endPos.Y, startPos.Z + count * i), Color.White));
                lines.Add(new VertexPositionColor(new Vector3(startPos.X + count * i, startPos.Y, startPos.Z), Color.White));
                lines.Add(new VertexPositionColor(new Vector3(startPos.X + count * i, endPos.Y, endPos.Z), Color.White));
            }
            this.vertices = lines.ToArray();
        }

        public void DrawWithCoordinateSystem(Matrix view, Matrix projection) {
            effect.World = world;
            effect.View = view;
            effect.Projection = projection;

            effect.CurrentTechnique.Passes[0].Apply();
            effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                axisLines,
                0,
                3);

            effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
               PrimitiveType.LineList,
               vertices,
               0,
               vertices.Length / 2);
        }

        public void Draw(Matrix view, Matrix projection) {
            effect.World = world;
            effect.View = view;
            effect.Projection = projection;

            effect.CurrentTechnique.Passes[0].Apply();

            effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
               PrimitiveType.LineList,
               vertices,
               0,
               vertices.Length / 2);
        }
    }
}
