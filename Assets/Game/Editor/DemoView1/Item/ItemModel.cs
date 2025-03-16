using Unity.Properties;

namespace Game.Editor.DemoWindow1.Item
{
    public struct ItemModel
    {
        [CreateProperty]
        public string Name { get; set; }
        
        [CreateProperty]
        public float Price { get; set; }
        
        [CreateProperty]
        public ItemRarity Rarity { get; set; }

        public ItemModel(string name, float price, ItemRarity rarity)
        {
            Name = name;
            Price = price;
            Rarity = rarity;
        }
    }
}