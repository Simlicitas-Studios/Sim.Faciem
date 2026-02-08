using Unity.Properties;

namespace Game.Runtime.uGUI
{
    public class NestedItem
    {
        [CreateProperty]
        public string TestPath { get; }
        
        [CreateProperty]
        public bool TestBool { get; }

        public NestedItem(string testPath, bool testBool)
        {
            TestPath = testPath;
            TestBool = testBool;
        }
    }
}