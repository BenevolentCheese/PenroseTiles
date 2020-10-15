using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ColorExtensions;

public class BowDrawer : MonoBehaviour {
    readonly struct Tri {
        readonly public(Vector3 A, Vector3 B, Vector3 C) vecs;
        readonly public Color color;
        readonly public int type;

        public Tri((Vector3, Vector3, Vector3) v, Color c, int triType) {
            this.vecs = v;
            this.color = c;
            this.type = triType;
        }
    };

    private float div = 100f;
    private float gRatio = (1 + Mathf.Sqrt(5f)) / 2f;
    private float divCount = 1;
    private int startCount = 10;

    private Sprite mySprite;
    private SpriteRenderer sr;
    private Color color = Color.clear;
    private bool randoColor = true;

    private SpriteMask mask;
    private Material mat;

    private List<Tri> tris;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.clear;
        transform.position = Vector3.zero;
    }

    void Start() {
        QualitySettings.antiAliasing = 2;
        QualitySettings.SetQualityLevel(5);
        mat = MaterialFactory.CreateLineMaterial();

        Texture2D tex = new Texture2D(1200, 250);
        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.zero, 100.0f);
        sr.sprite = mySprite;
        sr.material = mat;

        // mask = gameObject.AddComponent(typeof(SpriteMask)) as SpriteMask;
        // mask.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width / 2f, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        tris = CreateTriangles();
    }

    List<Tri> CreateTriangles() {
        float th = 36f * Mathf.Deg2Rad;
        float rad = mySprite.rect.xMax / div;
        List<float> thetas = Enumerable.Range(0, startCount).Select((num, index) => th * (float) num).ToList();

        List<Tri> tris = new List<Tri>();
        float m1 = th;
        float m2 = 0;
        foreach (float theta in thetas) {
            float swap = m1;
            m1 = m2;
            m2 = swap;
            Tri t = new Tri(
                (
                    Vector3.zero,
                    new Vector3(rad * Mathf.Cos(theta + m1), rad * Mathf.Sin(theta + m1), 0),
                    new Vector3(rad * Mathf.Cos(theta + m2), rad * Mathf.Sin(theta + m2), 0)
                ),
                RandomSaturatedColor(),
                0);
            tris.Add(t);
        }
        return tris;
    }

    public void clicky() {
        tris = Subdivide(tris);
    }

    public void reset() {
        tris = CreateTriangles();
    }

    List<Tri> Subdivide(List<Tri> list) {
        List<Tri> l = new List<Tri>();
        Color? hold0 = null;
        Color? hold1 = null;
        Color? holdX = null;
        foreach (Tri t in list) {
            if (t.type == 0) {
                Color c = hold0 ?? RandomColor();
                if (hold0.HasValue) {
                    hold0 = null;
                } else {
                    hold0 = c;
                }
                holdX = t.color;

                Vector3 P = t.vecs.A + (t.vecs.B - t.vecs.A) / gRatio;
                l.Add(new Tri((t.vecs.C, P, t.vecs.B), t.color, 0));
                l.Add(new Tri((P, t.vecs.C, t.vecs.A), c, 1));
            } else {
                Color c = hold1 ?? RandomColor();
                if (hold1.HasValue) {
                    hold1 = null;
                } else {
                    hold1 = c;
                }
                Color cx = holdX ?? RandomColor();
                if (holdX.HasValue) {
                    holdX = null;
                } else {
                    holdX = cx;
                }

                Vector3 Q = t.vecs.B + (t.vecs.A - t.vecs.B) / gRatio;
                Vector3 R = t.vecs.B + (t.vecs.C - t.vecs.B) / gRatio;
                l.Add(new Tri((R, t.vecs.C, t.vecs.A), c, 1));
                l.Add(new Tri((Q, R, t.vecs.B), t.color, 1));
                l.Add(new Tri((R, Q, t.vecs.A), cx, 0));

                holdX = null;
            }
        }
        return l;
    }

    public void OnRenderObject() {
        mat.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.TRIANGLES);
        GL.wireframe = false;
        foreach (Tri t in tris) {
            if (randoColor)
                GL.Color(t.color);
            else
                GL.Color(t.type == 0 ?
                    ColorRGB(80, 140, 255) :
                    ColorRGB(218, 80, 45));
            GL.Vertex(t.vecs.A);
            GL.Vertex(t.vecs.B);
            GL.Vertex(t.vecs.C);
        }
        GL.End();

        if (!randoColor) {
            foreach (Tri t in tris) {
                GL.Begin(GL.LINES);
                GL.Color(ColorRGB(180, 255, 100));
                GL.Vertex(t.vecs.B);
                GL.Vertex(t.vecs.A);

                GL.Vertex(t.vecs.A);
                GL.Vertex(t.vecs.C);
                GL.End();
            }
        } else if (false) {
            foreach (Tri t in tris) {
                GL.Begin(GL.LINES);
                GL.Color(Color.black);
                GL.Vertex(t.vecs.B);
                GL.Vertex(t.vecs.A);

                GL.Vertex(t.vecs.A);
                GL.Vertex(t.vecs.C);

                GL.Vertex(t.vecs.C);
                GL.Vertex(t.vecs.B);
                GL.End();
            }
        }

        GL.PopMatrix();
    }
}
