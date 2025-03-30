using System.Globalization;
using System.Xml;
using ocicat.Graphics.Rendering;

namespace ocicat.Graphics;

public class Spritesheet
{
    public readonly Texture SpritesheetTexture;
    Dictionary<string, Sprite> _sprites = new();

    public Spritesheet(Renderer renderer, string textureFile, string layoutFile)
    {
        SpritesheetTexture = Texture.Create(renderer, textureFile);
        XmlDocument document = new XmlDocument();
        document.Load(layoutFile);
        
        XmlNode root = document.SelectSingleNode("Sprites");
        foreach (XmlNode node in root.ChildNodes)
        {
            string name = node.SelectSingleNode("Name").InnerText;
            
            string[] offsetString = node.SelectSingleNode("Offset").InnerText.Split(',');
            string[] sizeString = node.SelectSingleNode("Size").InnerText.Split(',');
            
            Vector2 offset = new Vector2(float.Parse(offsetString[0], CultureInfo.InvariantCulture), float.Parse(offsetString[1], CultureInfo.InvariantCulture));
            Vector2 size = new Vector2(float.Parse(sizeString[0], CultureInfo.InvariantCulture), float.Parse(sizeString[1], CultureInfo.InvariantCulture));
            
            Sprite sprite = new(this, offset, size);
            _sprites.Add(name, sprite);
        }
    }

    public Sprite GetSprite(string name)
    {
        return _sprites[name];
    }
}

public class Sprite
{
    public readonly Spritesheet Spritesheet;
    public readonly Vector2 Offset;
    public readonly Vector2 Size;
    
    public Sprite(Spritesheet spritesheet, Vector2 position, Vector2 size)
    {
        Spritesheet = spritesheet;
        Offset = position;
        Size = size;
    }
}