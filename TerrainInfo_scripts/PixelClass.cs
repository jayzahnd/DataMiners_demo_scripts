using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelClass {

    public string name;

    public enum pixelReader { BLACK, RED, GREEN, BLUE, GREY, MAGENTA, DARK_RED, TEAL_GREEN, DARK_GREY };
    public pixelReader colourCompare;

    public int pixelIndex;
    public Color32 pixelColor32;
}
