using UnityEngine;

public static class VectorExtension {
  //Vector3
  public static Vector3 WithX(this Vector3 vec, float x) {
    return new Vector3(x, vec.y, vec.z);
  }

  public static Vector3 WithY(this Vector3 vec, float y) {
    return new Vector3(vec.x, y, vec.z);
  }

  public static Vector3 WithZ(this Vector3 vec, float z) {
    return new Vector3(vec.x, vec.y, z);
  }

  public static Vector3 AddX(this Vector3 vec, float x) {
    return new Vector3(vec.x + x, vec.y, vec.z);
  }

  public static Vector3 AddY(this Vector3 vec, float y) {
    return new Vector3(vec.x, vec.y + y, vec.z);
  }

  public static Vector3 AddZ(this Vector3 vec, float z) {
    return new Vector3(vec.x, vec.y, vec.z + z);
  }

  //Vector2
  public static Vector3 WithX(this Vector2 vec, float x) {
    return new Vector2(x, vec.y);
  }

  public static Vector3 WithY(this Vector2 vec, float y) {
    return new Vector2(vec.x, y);
  }

  public static Vector3 AddX(this Vector2 vec, float x) {
    return new Vector2(vec.x + x, vec.y);
  }

  public static Vector3 AddY(this Vector2 vec, float y) {
    return new Vector2(vec.x, vec.y + y);
  }
}
