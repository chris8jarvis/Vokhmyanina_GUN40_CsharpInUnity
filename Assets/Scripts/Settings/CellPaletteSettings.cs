using UnityEngine;

[CreateAssetMenu(fileName = "CellPaletteSettings", menuName = "Game/Cell Palette Settings")]
public class CellPaletteSettings : ScriptableObject
{
    [Header("Cell Selection Materials")]
    public Material selectedMaterial;
    public Material availableMaterial;
    public Material attackMaterial;
    
    [Header("Unit Selection Material")]
    public Material unitSelectedMaterial;
}