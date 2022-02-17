using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMovementJump
{
    public class SpriteComponent
    {
        public Sprite sprite;

        private bool started = false;

        public void Init(Sprite attachedSprite)
        {
            sprite = attachedSprite;
        }

        public void UpdateComponent()
        {
            if (!started)
                StartComponent();

            if (sprite != null)
                Update();
            else
                throw new Exception("Forgot to assign attached sprite in constructor");
        }

        public void StartComponent()
        {
            if (started)
                return;
            
            started = true;

            if (sprite != null)
                Start();
            else
                throw new Exception("Forgot to assign attached sprite in constructor");
        }

        public virtual void Update()
        {
            //Component Implementation
        }

        public virtual void Start()
        {
            //Component Implementation
        }
    }
}
