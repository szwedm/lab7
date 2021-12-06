using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab7 {

    public class Game1 : Game {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;

        private Texture2D grassTexture, roofTexture, wallTexture;

        private SpriteFont font;

        private Vector3 cameraPos = new Vector3(0f, 0f, 4f);
        private Vector3 cameraTarget = Vector3.Zero;

        private Matrix world, view, projection;

        float angleX = 0.0f;
        float angleY = 0.0f;
        float zoom = 0.0f;

        private KeyboardState currentKeyState, previousKeyState;
        private string strKey;

        private VertexPositionColor[] axisLines;
        private List<VertexPositionTexture> geometryWalls;
        private List<VertexPositionTexture> geometryGround;
        private List<VertexPositionTexture> geometryRoof;

        const float xAxisLength = 10.0f;
        const float yAxisLength = 10.0f;
        const float zAxisLength = 10.0f;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;
            graphics.PreferMultiSampling = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                1000f);

            // RasterizerState
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rs;


            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>(@"font");
            grassTexture = Content.Load<Texture2D>(@"grass1");
            roofTexture = Content.Load<Texture2D>(@"roofing1");
            wallTexture = Content.Load<Texture2D>(@"wall1");

            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = false;

            axisLines = CreateAxesGeometry();
            geometryWalls = CreateWallsGeometry();
            geometryGround = CreateGroundGeometry();
            geometryRoof = CreateRoofGeometry();
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState kbd = Keyboard.GetState();
            GetKeyboardState();

            if (kbd.IsKeyDown(Keys.Escape)) {
                Exit();
            }

            if (kbd.IsKeyDown(Keys.Right)) {
                angleY += 0.02f;
            }
            if (kbd.IsKeyDown(Keys.Left)) {
                angleY -= 0.02f;
            }
            if (kbd.IsKeyDown(Keys.Up)) {
                angleX += 0.02f;
            }
            if (kbd.IsKeyDown(Keys.Down)) {
                angleX -= 0.02f;
            }
            if (kbd.IsKeyDown(Keys.Q)) {
                zoom += 2f;
            }
            if (kbd.IsKeyDown(Keys.A)) {
                zoom -= 2f;
            }

            world = Matrix.Identity;
            view = Matrix.CreateLookAt(cameraPos, cameraTarget, Vector3.Up);
            view = Matrix.CreateTranslation(new Vector3(0, 0, zoom)) * view;
            view = Matrix.CreateRotationX(angleX) * Matrix.CreateRotationY(angleY) * view;

            strKey = RenderPressedKeysString(kbd.GetPressedKeys());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = wallTexture;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, geometryWalls.ToArray(), 0, geometryWalls.Count / 3);
            }

            basicEffect.Texture = grassTexture;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, geometryGround.ToArray(), 0, geometryGround.Count / 3);
            }

            basicEffect.Texture = roofTexture;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, geometryRoof.ToArray(), 0, geometryRoof.Count / 3);
            }

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = false;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, axisLines, 0, 3);
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(font, strKey, Vector2.Zero, Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public KeyboardState GetKeyboardState() {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            return currentKeyState;
        }

        public bool IsPressed(Keys key) {
            return currentKeyState.IsKeyDown(key);
        }

        public bool HasBeenPressed(Keys key) {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }

        private string RenderPressedKeysString(Keys[] keys) {
            string str = "Keys: ";
            var pressedKeys = keys;
            if (pressedKeys.Length > 0) {
                str += string.Join(" + ", pressedKeys);
            }
            return str;
        }

        private VertexPositionColor[] CreateAxesGeometry() {
            var result = new VertexPositionColor[6];
            result[0] = new VertexPositionColor(-xAxisLength / 2f * Vector3.UnitX, Color.Red);
            result[1] = new VertexPositionColor(xAxisLength / 2f * Vector3.UnitX, Color.Red);
            result[2] = new VertexPositionColor(-yAxisLength / 2f * Vector3.UnitY, Color.Green);
            result[3] = new VertexPositionColor(yAxisLength / 2f * Vector3.UnitY, Color.Green);
            result[4] = new VertexPositionColor(-zAxisLength / 2f * Vector3.UnitZ, Color.Blue);
            result[5] = new VertexPositionColor(zAxisLength / 2f * Vector3.UnitZ, Color.Blue);
            return result;
        }

        private List<VertexPositionTexture> CreateWallsGeometry() {
            var front = CreateFace(Vector3.Up / 2.0f + Vector3.Backward / 2.0f, Vector3.Up, Vector3.Backward, 2f, 1f, 2f, 1f);

            var right = CreateFace(Vector3.Up / 2.0f + Vector3.Right, Vector3.Up, Vector3.Right, 1f, 1f, 1f, 1f);

            var left = CreateFace(Vector3.Up / 2.0f + Vector3.Left, Vector3.Up, Vector3.Left, 1f, 1f, 1f, 1f);

            var back = CreateFace(Vector3.Up / 2.0f + Vector3.Forward / 2.0f, Vector3.Up, Vector3.Forward, 2f, 1f, 2f, 1f);

            return front.Concat(right).Concat(left).Concat(back).ToList();
        }


        private List<VertexPositionTexture> CreateGroundGeometry(float groundSizeX = 10.0f, float groundSizeY = 10.0f) {
            var groundUp = CreateFace(Vector3.Zero, Vector3.Forward, Vector3.Up, groundSizeX, groundSizeY, 2.0f, 2.0f);
            var groundDown = CreateFace(Vector3.Zero, Vector3.Forward, Vector3.Down, groundSizeX, groundSizeY, 2.0f, 2.0f);

            return groundUp.Concat(groundDown).ToList();
        }

        private List<VertexPositionTexture> CreateRoofGeometry(float groundSizeX = 10.0f, float groundSizeY = 10.0f) {
            var roofBase = CreateFace(Vector3.Up * 1.0f, Vector3.Forward, Vector3.Down, 2.0f, 1.0f, 1.0f, 1.0f);

            float d = (float)Math.Sqrt(2);

            var roofSide1 = CreateFace(Vector3.Up + Vector3.Backward / 2.0f, Vector3.Up + Vector3.Forward, Vector3.Up - Vector3.Forward, 2.0f, d / 2.0f, 3.0f, 2.0f, true);

            var roofSide2 = CreateFace(Vector3.Up + Vector3.Forward / 2.0f, Vector3.Up + Vector3.Backward, Vector3.Up - Vector3.Backward, 2.0f, d / 2.0f, 3.0f, 2.0f, true);

            var sides = new VertexPositionTexture[6];
            sides[0] = new VertexPositionTexture(new Vector3(1f, 1f, 0.5f), new Vector2(0, 2));
            sides[1] = new VertexPositionTexture(new Vector3(1f, 1.5f, 0f), new Vector2(1, 0));
            sides[2] = new VertexPositionTexture(new Vector3(1f, 1f, -0.5f), new Vector2(2, 2));

            sides[3] = new VertexPositionTexture(new Vector3(-1f, 1f, 0.5f), new Vector2(0, 2));
            sides[5] = new VertexPositionTexture(new Vector3(-1f, 1.5f, 0f), new Vector2(1, 0));
            sides[4] = new VertexPositionTexture(new Vector3(-1f, 1f, -0.5f), new Vector2(2, 2));


            return roofBase.Concat(roofSide1).Concat(roofSide2).Concat(sides.ToList()).ToList();

        }

        private List<VertexPositionTexture> CreateFace(Vector3 origin, Vector3 up, Vector3 normal, float width, float height, float textMultiX, float textMultiY, bool originAtBottom = false) {
            List<VertexPositionTexture> verts = new List<VertexPositionTexture>();

            up.Normalize();
            if (originAtBottom) origin = origin + up * height / 2.0f;

            normal.Normalize();
            Vector3 right = Vector3.Cross(up, normal);
            right.Normalize();
            Vector3 left = -right;
            Vector3 down = -up;

            var v0 = origin + (width / 2f) * left + (height / 2f) * down;
            var v1 = origin + (width / 2f) * left + (height / 2f) * up;
            var v2 = origin + (width / 2f) * right + (height / 2f) * up;
            var v3 = origin + (width / 2f) * right + (height / 2f) * down;

            var v0Tex = new Vector2(0, textMultiY);
            var v1Tex = new Vector2(0, 0);
            var v2Tex = new Vector2(textMultiX, 0);
            var v3Tex = new Vector2(textMultiX, textMultiY);

            verts.Add(new VertexPositionTexture(v0, v0Tex));
            verts.Add(new VertexPositionTexture(v1, v1Tex));
            verts.Add(new VertexPositionTexture(v2, v2Tex));

            verts.Add(new VertexPositionTexture(v2, v2Tex));
            verts.Add(new VertexPositionTexture(v3, v3Tex));
            verts.Add(new VertexPositionTexture(v0, v0Tex));

            return verts;
        }
    }
}
