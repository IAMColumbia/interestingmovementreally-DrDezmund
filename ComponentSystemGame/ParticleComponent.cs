using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleMovementJump
{
    class ParticleComponent : SpriteComponent
    {
        private Vector2 velocity;
        private float gravity = 200;

        public ParticleComponent(float RandomX, float RandomY)
        {
            velocity = new Vector2(Game1.RandomRange(-(int)RandomX, (int)RandomX), Game1.RandomRange(0, (int)RandomY));
        }

        public override void Start()
        {
            sprite.depth = 1f;
        }

        public override void Update()
        {
            velocity.Y += gravity * Game1.DeltaTime;
            sprite.position = sprite.position + ((velocity * (Game1.DeltaTime)));

            if (sprite.position.Y > Game1.graphics.GraphicsDevice.Viewport.Height + sprite.sprite.Height)
                sprite.Delete();
        }
    }
}
