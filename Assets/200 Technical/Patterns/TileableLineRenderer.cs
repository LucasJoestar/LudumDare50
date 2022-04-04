// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
	public class TileableLineRenderer : MonoBehaviour
    {
        #region Global Members
        [Section("TileableLineRenderer")]

        [SerializeField] private LineRenderer lineRenderer;
        #endregion
    }
}
