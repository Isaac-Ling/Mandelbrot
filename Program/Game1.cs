using System;
using System.IO;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Mandelbrot
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spritebatch; 
        private Texture2D _texture;
        private Effect _mandelRenderer;
        private float _zoom = 3;
        private int _maxIterations = 250;
        private Vector2 _pan = Vector2.Zero;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            _texture = new Texture2D(GraphicsDevice, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spritebatch = new SpriteBatch(GraphicsDevice);
            _mandelRenderer = Content.Load<Effect>("Mandelbrot");
            _mandelRenderer.Parameters["Zoom"].SetValue(_zoom);
            _mandelRenderer.Parameters["MaxIterations"].SetValue(_maxIterations);
            _mandelRenderer.Parameters["Aspect"].SetValue(new Vector2(1, (float)GraphicsDevice.DisplayMode.Height / GraphicsDevice.DisplayMode.Width));
            _mandelRenderer.Parameters["Pan"].SetValue(_pan);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Controls();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spritebatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, _mandelRenderer, null);
            GraphicsDevice.Clear(Color.White);
            _spritebatch.Draw(_texture, Vector2.Zero, Color.White);
            _spritebatch.End();

            base.Draw(gameTime);
        }

        private void Controls()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.X))
            {
                _zoom *= 1.02f;
                SetParameters();
            }
            if (state.IsKeyDown(Keys.Z))
            {
                _zoom /= 1.02f;
                SetParameters();
            }
            if (state.IsKeyDown(Keys.Up))
            {
                _pan.Y += 0.005f * _zoom;
                SetParameters();
            }
            if (state.IsKeyDown(Keys.Down))
            {
                _pan.Y -= 0.005f * _zoom;
                SetParameters();
            }
            if (state.IsKeyDown(Keys.Left))
            {
                _pan.X += 0.005f * _zoom;
                SetParameters();
            }
            if (state.IsKeyDown(Keys.Right))
            {
                _pan.X -= 0.005f * _zoom;
                SetParameters();
            }
        }

        private void SetParameters()
        {
            _mandelRenderer.Parameters["Zoom"].SetValue(_zoom);
            _mandelRenderer.Parameters["Pan"].SetValue(_pan);
            _mandelRenderer.Parameters["MaxIterations"].SetValue(_maxIterations);
        }
    }
}