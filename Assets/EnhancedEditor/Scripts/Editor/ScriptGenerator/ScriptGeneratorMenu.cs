// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
//
// Notes:
//
// ============================================================================ //

using UnityEditor;

namespace EnhancedEditor.Editor
{
    public static partial class ScriptGenerator
    {
        [MenuItem(ScriptCreatorSubMenu + "Behaviour Template", false, MenuItemOrder)]
        public static void CreateBehaviourTemplate() => ScriptGeneratorWindow.GetWindow("EnhancedEditor/Editor/ScriptTemplates/BehaviourTemplate.txt");

        [MenuItem(ScriptCreatorSubMenu + "Scriptable Template", false, MenuItemOrder)]
        public static void CreateScriptableTemplate() => ScriptGeneratorWindow.GetWindow("EnhancedEditor/Editor/ScriptTemplates/ScriptableTemplate.txt");
    }
}
