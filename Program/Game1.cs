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
        private Effect _fractalRenderer;
        private float _zoom = 3;
        private int _maxIterations = 250;
        private Vector2 _constant = new Vector2(0.346f, 0.346f);
        private Vector2 _pan = Vector2.Zero;
        private FractalType fractalType;

        public Game1()
        {
            fractalType = FractalType.Mandelbrot;
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
            if (fractalType == FractalType.Julia)
            {
                _fractalRenderer = Content.Load<Effect>("JuliaSet");
                _fractalRenderer.Parameters["Constant"].SetValue(_constant);
            }
            else if (fractalType == FractalType.Mandelbrot)
            {
                _fractalRenderer = Content.Load<Effect>("Mandelbrot");
            }
            _fractalRenderer.Parameters["Zoom"].SetValue(_zoom);
            _fractalRenderer.Parameters["MaxIterations"].SetValue(_maxIterations);
            _fractalRenderer.Parameters["Aspect"].SetValue(new Vector2(1, (float)GraphicsDevice.DisplayMode.Height / GraphicsDevice.DisplayMode.Width));
            _fractalRenderer.Parameters["Pan"].SetValue(_pan);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Controls();

            if (fractalType == FractalType.Julia)
            {
                _constant += new Vector2(0.000003f, 0.000003f);
                _fractalRenderer.Parameters["Constant"].SetValue(_constant);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spritebatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, _fractalRenderer, null);
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
            _fractalRenderer.Parameters["Zoom"].SetValue(_zoom);
            _fractalRenderer.Parameters["Pan"].SetValue(_pan);
            _fractalRenderer.Parameters["MaxIterations"].SetValue(_maxIterations);
        }

        public enum FractalType
        {
            Mandelbrot,
            Julia
        }
    }
}