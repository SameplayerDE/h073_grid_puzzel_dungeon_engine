using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace grid_puzzle_dungeon_engine.Graphics;

public class Animation
{
    public List<AnimationStep> AnimationSteps;
    public int Index;
    public TimeSpan ElapsedTime;
    public string TextureKey;

    public AnimationStep CurrentStep => AnimationSteps[Index];

    public Animation()
    {
        AnimationSteps = new List<AnimationStep>();
        Index = 0;
        ElapsedTime = TimeSpan.Zero;
    }

    public void Update(GameTime gameTime)
    {
        var animationStep = AnimationSteps[Index];

        if (animationStep.DisplayDuration <= ElapsedTime)
        {
            Index += 1;
            ElapsedTime = TimeSpan.Zero;
        }
        else
        {
            ElapsedTime += gameTime.ElapsedGameTime;
        }

        if (Index >= AnimationSteps.Count)
        {
            Index = 0;
        }
    }
    
    public static Animation FromFile(string path)
    {
        using var file = File.OpenText(path);
        using var reader = new JsonTextReader(file);
            
        var document = (JObject)JToken.ReadFrom(reader);
                    
        var animation = document["animation"];

        if (animation == null)
        {
            throw new NullReferenceException();
        }

        var loadedAnimation = new Animation();
        var animationSteps = animation["steps"];

        if (animationSteps == null)
        {
            throw new NullReferenceException();
        }
                    
        foreach (var animationStep in animationSteps)
        {
            var section = animationStep["section"];
            var duration = animationStep["duration"];

            if (section == null || duration == null)
            {
                throw new NullReferenceException();
            }
                        
            var width = section.Value<int>("width");
            var height = section.Value<int>("height");
            var x = section.Value<int>("x");
            var y = section.Value<int>("y");

            loadedAnimation.AnimationSteps.Add(new AnimationStep()
            {
                DisplayDuration = TimeSpan.FromMilliseconds((int)duration),
                Section = new Rectangle(x, y, width, height)
            });
        }
                    
        return loadedAnimation;
    }
    
}