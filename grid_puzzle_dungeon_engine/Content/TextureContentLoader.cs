using Microsoft.Xna.Framework.Graphics;

namespace grid_puzzle_dungeon_engine.Content;

public class TextureContentLoader : GenericContentLoader<string, Texture2D>
{

    public static TextureContentLoader Instance { get; } = new TextureContentLoader();

    static TextureContentLoader()
    {
    }

    private TextureContentLoader()
    {
    }
    
}