using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SimpleMovementJump
{
    public class Sprite
    {
        public enum Origin { TopLeft, MiddleCenter};

        public Texture2D sprite;
        public Vector2 position;
        public float rotation;          
        public Vector2 origin;
        public float depth = .5f;


        public bool DeleteMe = false;

        public List<SpriteComponent> components;

        public Sprite()
        {
            Game1.instance.RegisterSprite(this);
            components = new List<SpriteComponent>();
        }

        public void SetSprite(Texture2D newSprite, Origin newOrigin)
        {
            switch (newOrigin)
            {
                case Origin.TopLeft:
                    SetSprite(newSprite, new Vector2(0, 0));
                    break;
                case Origin.MiddleCenter:
                    SetSprite(newSprite, new Vector2(newSprite.Width/2, newSprite.Height/2));
                    break;
            }
        }

        public void SetSprite(Texture2D newSprite, Vector2 newOrigin)
        {
            sprite = newSprite;
            origin = newOrigin;
        }

        public void AddComponent(SpriteComponent component)
        {
            components.Add(component);
            component.Init(this);
        }

        public void Update()
        {
            foreach(SpriteComponent s in components)
            {
                s.UpdateComponent();
            }
        }

        public Vector2 Forward()
        {
            return new Vector2(MathF.Cos(rotation), MathF.Sin(rotation));
        }

        public void Delete()
        {
            DeleteMe = true;
            sprite = null;
            components = null;
        }
    }
}
