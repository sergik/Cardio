using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Effects.Bloom
{
    public class BloomComponent : DrawableGameComponent
    {
        public enum IntermediateBuffer
        {
            PreBloom,
            BlurredHorizontally,
            BlurredBothWays,
            FinalResult,
        }

        private SpriteBatch _spriteBatch;

        private Effect _bloomCombineEffect;
        private Effect _bloomExtractEffect;
        private Effect _gaussianBlurEffect;

        private RenderTarget2D _renderTarget1;
        private RenderTarget2D _renderTarget2;
        private RenderTarget2D _sceneRenderTarget;


        // Choose what display settings the bloom should use.

        private BloomSettings _settings = new BloomSettings();


        // Optionally displays one of the intermediate buffers used
        // by the bloom postprocess, so you can see exactly what is
        // being drawn into each rendertarget.

        private IntermediateBuffer _showBuffer = IntermediateBuffer.FinalResult;

        private float[] _sampleWeights;
        private Vector2[] _sampleOffsets;

        private EffectParameter _weightsParameter;
        private EffectParameter _offsetsParameter;

        public BloomSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public IntermediateBuffer ShowBuffer
        {
            get { return _showBuffer; }
            set { _showBuffer = value; }
        }

        public BloomComponent(Game game)
            : base(game)
        {
            if (game == null)
                throw new ArgumentNullException("game");
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _bloomExtractEffect = Game.Content.Load<Effect>("Shaders\\BloomExtract");
            _bloomCombineEffect = Game.Content.Load<Effect>("Shaders\\BloomCombine");
            _gaussianBlurEffect = Game.Content.Load<Effect>("Shaders\\GaussianBlur");

            _weightsParameter = _gaussianBlurEffect.Parameters["SampleWeights"];
            _offsetsParameter = _gaussianBlurEffect.Parameters["SampleOffsets"];

            // Look up the resolution and format of our main backbuffer.
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;

            SurfaceFormat format = pp.BackBufferFormat;

            // Create a texture for rendering the main scene, prior to applying bloom.
            _sceneRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
                format, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.DiscardContents);

            // Create two rendertargets for the bloom processing. These are half the
            // size of the backbuffer, in order to minimize fillrate costs. Reducing
            // the resolution in this way doesn't hurt quality, because we are going
            // to be blurring the bloom images in any case.
            width /= 4;
            height /= 4;

            _renderTarget1 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
            _renderTarget2 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);

            _sampleWeights = new float[_weightsParameter.Elements.Count];
            _sampleOffsets = new Vector2[_weightsParameter.Elements.Count];
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            _sceneRenderTarget.Dispose();
            _renderTarget1.Dispose();
            _renderTarget2.Dispose();
        }

        /// <summary>
        /// This should be called at the very start of the scene rendering. The bloom
        /// component uses it to redirect drawing into its custom rendertarget, so it
        /// can capture the scene image in preparation for applying the bloom filter.
        /// </summary>
        public void BeginDraw()
        {
            if (Visible)
            {
                GraphicsDevice.SetRenderTarget(_sceneRenderTarget);
            }
        }


        /// <summary>
        /// This is where it all happens. Grabs a scene that has already been rendered,
        /// and uses postprocess magic to add a glowing bloom effect over the top of it.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            // Pass 1: draw the scene into rendertarget 1, using a
            // shader that extracts only the brightest parts of the image.
            _bloomExtractEffect.Parameters["BloomThreshold"].SetValue(
                Settings.BloomThreshold);

            DrawFullscreenQuad(_sceneRenderTarget, _renderTarget1,
                _bloomExtractEffect,
                IntermediateBuffer.PreBloom);

            // Pass 2: draw from rendertarget 1 into rendertarget 2,
            // using a shader to apply a horizontal gaussian blur filter.
            SetBlurEffectParameters(1.0f / _renderTarget1.Width, 0);

            DrawFullscreenQuad(_renderTarget1, _renderTarget2,
                _gaussianBlurEffect,
                IntermediateBuffer.BlurredHorizontally);

            // Pass 3: draw from rendertarget 2 back into rendertarget 1,
            // using a shader to apply a vertical gaussian blur filter.
            SetBlurEffectParameters(0, 1.0f / _renderTarget1.Height);

            DrawFullscreenQuad(_renderTarget2, _renderTarget1,
                _gaussianBlurEffect,
                IntermediateBuffer.BlurredBothWays);

            // Pass 4: draw both rendertarget 1 and the original scene
            // image back into the main backbuffer, using a shader that
            // combines them to produce the final bloomed result.
            GraphicsDevice.SetRenderTarget(null);

            EffectParameterCollection parameters = _bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(Settings.BloomIntensity);
            parameters["BaseIntensity"].SetValue(Settings.BaseIntensity);
            parameters["BloomSaturation"].SetValue(Settings.BloomSaturation);
            parameters["BaseSaturation"].SetValue(Settings.BaseSaturation);

            GraphicsDevice.Textures[1] = _sceneRenderTarget;

            Viewport viewport = GraphicsDevice.Viewport;

            DrawFullscreenQuad(_renderTarget1,
                viewport.Width, viewport.Height,
                _bloomCombineEffect,
                IntermediateBuffer.FinalResult);
        }


        /// <summary>
        /// Helper for drawing a texture into a rendertarget, using
        /// a custom shader to apply postprocessing effects.
        /// </summary>
        private void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget,
            Effect effect, IntermediateBuffer currentBuffer)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            DrawFullscreenQuad(texture,
                renderTarget.Width, renderTarget.Height,
                effect, currentBuffer);
        }


        /// <summary>
        /// Helper for drawing a texture into the current rendertarget,
        /// using a custom shader to apply postprocessing effects.
        /// </summary>
        private void DrawFullscreenQuad(Texture2D texture, int width, int height,
            Effect effect, IntermediateBuffer currentBuffer)
        {
            // If the user has selected one of the show intermediate buffer options,
            // we still draw the quad to make sure the image will end up on the screen,
            // but might need to skip applying the custom pixel shader.
            if (_showBuffer < currentBuffer)
            {
                effect = null;
            }

            _spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            _spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            _spriteBatch.End();
        }


        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        private void SetBlurEffectParameters(float dx, float dy)
        {
            // The first sample always has a zero offset.
            _sampleWeights[0] = ComputeGaussian(0);
            _sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = _sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < _sampleWeights.Length / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                _sampleWeights[i * 2 + 1] = weight;
                _sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                _sampleOffsets[i * 2 + 1] = delta;
                _sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < _sampleWeights.Length; i++)
            {
                _sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            _weightsParameter.SetValue(_sampleWeights);
            _offsetsParameter.SetValue(_sampleOffsets);
        }


        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        private float ComputeGaussian(float n)
        {
            float theta = Settings.BlurAmount;

            return (float) ((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}