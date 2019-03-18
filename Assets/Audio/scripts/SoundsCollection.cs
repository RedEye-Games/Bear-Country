using UnityEngine;

[CreateAssetMenu(menuName = "audio/sounds collection")]
public class SoundsCollection : ScriptableObject
{
    public Sound[] tilePickedUpSound; // "Click1" + "slide"
    public Sound[] specialTilePickedUpSound; // "clickPop" + click1 + slide
    public Sound[] tileSuccessfullyPlacedSound; // "uh-huh" + "drop"
    public Sound[] tileUnsuccessfullyPlacedSound; // "Click1Low
    public Sound[] tileFlippedSound; // click1
    public Sound[] tileRotatedSound; // click1
}
