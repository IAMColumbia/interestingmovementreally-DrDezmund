using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SimpleMovementJump
{
    class MovementComponent : SpriteComponent
    {
        //Graphics
        public Texture2D ParticleTex;
        //Input
        public Keys rightKey = Keys.D;
        public Keys leftKey = Keys.A;
        public Keys jumpKey = Keys.W;
        //Movement
        public float Friction = 10.0f;                 //Friction to slow down when no input is set
        public float Accel = 10;               //Acceloration
        public float BoostPower = 1200f;
        public float MaxSpeed = 200;
        public Vector2 GravityDir = new Vector2(0, 1);            //Direction for gravity
        public float GravityAccel = 600.0f;             //Acceloration from gravity
        public int ParticleAmount = 1;
        public float ParticleDelay = .08f;
        public float TurnSpeed = 4f;

        private Vector2 boostVector;
        private bool isGrounded = false;
        private Vector2 velocity;
        private Vector2 input;
        private float maxX = 0f;
        private float actualRotation;
        private double lastTimeParticleEffect = -1337f;

        public MovementComponent()
        {
            input = new Vector2();
            velocity = new Vector2();
            boostVector = new Vector2();
        }

        public override void Start()
        {
            sprite.depth = 1f;
        }

        public override void Update()
        {
            if (sprite.position.Y > Game1.instance.GraphicsDevice.Viewport.Height - sprite.origin.Y) //HACKY GROUND CHECK
            {
                sprite.position.Y = Game1.instance.GraphicsDevice.Viewport.Height - sprite.origin.Y;
                velocity.Y = 0;
                isGrounded = true;
            }
            else
                isGrounded = false;

            
            input = Vector2.Zero;

            input.X += KeyboardHandler.instance.IsHoldingKey(rightKey) ? 1f : 0f;
            input.X += KeyboardHandler.instance.IsHoldingKey(leftKey) ? -1f : 0f;

            //rotate sprite
            actualRotation += input.X * Game1.DeltaTime * TurnSpeed;

            sprite.rotation = (actualRotation - MathF.PI / 2f) + 1f; //magic number funny rotation

            boostVector.Y = MathF.Sin(actualRotation);
            boostVector.X = MathF.Cos(actualRotation);

            if (KeyboardHandler.instance.IsHoldingKey(jumpKey))
            {
                velocity = velocity + boostVector * Game1.DeltaTime * -BoostPower;
                isGrounded = false;

                if (Game1.Time > lastTimeParticleEffect + ParticleDelay)
                {
                    for (int i = 0; i < ParticleAmount; i++)
                    {
                        Sprite particleSprite = new Sprite();
                        particleSprite.SetSprite(ParticleTex, Sprite.Origin.MiddleCenter);
                        ParticleComponent particleComponent = new ParticleComponent(100, 200);
                        particleSprite.position = sprite.position + sprite.Forward() * 30;
                        particleSprite.AddComponent(particleComponent);
                    }

                    lastTimeParticleEffect = Game1.Time;
                }
            }

            if (isGrounded && Math.Abs(input.X) < .5f)
            {
                if (velocity.X > 0) //If the pacman has a positive direction in X(moving right)
                    velocity.X = Math.Max(0, velocity.X - Friction); //Reduce X by friction amount but don't go below 0 
                else //Else pacman has a negative direction.X (moving left)
                    velocity.X = Math.Min(0, velocity.X + Friction); //Add friction amount until X is 0
            }

            if (isGrounded)
            {
                if (input.X < 0)
                    velocity.X = Math.Max((MaxSpeed * -1.0f), velocity.X - Accel);
                if (input.X > 0)
                    velocity.X = Math.Min(MaxSpeed, velocity.X + Accel);
            }

            sprite.position = sprite.position + ((velocity * (Game1.DeltaTime)));
            velocity = velocity + (GravityDir * GravityAccel) * (Game1.DeltaTime);

            //Keep sprite On Screen
            maxX = Game1.graphics.GraphicsDevice.Viewport.Width - sprite.sprite.Width;

            if (sprite.position.X - sprite.origin.X > maxX)
            {
                velocity.X = Math.Min(0, velocity.X);
                sprite.position.X = maxX + sprite.origin.X;
            }
            else if(sprite.position.X < 0 + sprite.origin.X)
            {
                velocity.X = Math.Max(0, velocity.X);
                sprite.position.X = 0 + sprite.origin.X;
            }

        }
    }
}
