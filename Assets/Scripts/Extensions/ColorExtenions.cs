using UnityEngine;

public static class ColorExtensions {
  public static Color ColorRGB(int r, int g, int b) {
    return new Color((float) r / 255f, (float) g / 255f, (float) b / 255f);
  }

  public static Color RandomColor() {
    return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
  }

  public static Color RandomSaturatedColor() {
    float r = Random.Range(0f, 1f);
    float g = Random.Range(0f, 1f);
    float b = Random.Range(0f, 1f);

    if (r >= g && r >= b) r = 1f;
    else if (g >= b) g = 1f;
    else b = 1f;

    if (r <= g && r <= b) r = 0f;
    else if (g <= b) g = 0f;
    else b = 0f;

    return new Color(r, g, b);
  }

  public static Color Fade(this Color c, float amount = 0.8f) {
    return new Color(c.r * amount, c.g * amount, c.b * amount);
  }
}
