using Unity.Properties;

namespace Game.Runtime.uGUI
{
    public class NestedItem
    {
        [CreateProperty]
        public string TestPath { get; }

        public NestedItem(string testPath)
        {
            TestPath = testPath;
        }
    }
}