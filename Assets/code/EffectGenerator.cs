using UnityEngine;
using UnityEditor;

public class EffectGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/生成特效 (Generate Effects)")]
    public static void GenerateEffects()
    {
        CreateBoilingEffect();
        CreateExplosionEffect();
        Debug.Log("特效即生成完毕！请在场景中找到 'BoilingEffect' 和 'ExplosionEffect_Prefab'。");
    }

    static void CreateBoilingEffect()
    {
        GameObject go = new GameObject("BoilingEffect");
        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        
        // 主模块
        var main = ps.main;
        main.startLifetime = 2f;
        main.startSpeed = 1f;
        main.startSize = 0.3f;
        main.startColor = new Color(1f, 1f, 1f, 0.5f); // 半透明白
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.loop = true;

        // 发射模块
        var emission = ps.emission;
        emission.rateOverTime = 20f; // 每秒20个泡泡

        // 形状模块
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.rotation = new Vector3(-90, 0, 0); // 向上发射

        // 颜色随时间变化 (白色 -> 透明)
        var col = ps.colorOverLifetime;
        col.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.cyan, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        col.color = grad;

        // 添加一个小球渲染，或者默认材质
        ParticleSystemRenderer renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        go.transform.position = Vector3.zero;
        Selection.activeGameObject = go;
    }

    static void CreateExplosionEffect()
    {
        GameObject go = new GameObject("ExplosionEffect_Prefab");
        ParticleSystem ps = go.AddComponent<ParticleSystem>();

        // 主模块
        var main = ps.main;
        main.duration = 1f;
        main.startLifetime = 0.5f;
        main.startSpeed = 10f;
        main.startSize = 0.5f;
        main.startColor = Color.red;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy; // 播放完自动销毁

        // 发射模块 (爆发)
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 30) }); // 0秒时爆发30个

        // 形状模块
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;

        // 颜色随时间变化 (红 -> 黄 -> 透明)
        var col = ps.colorOverLifetime;
        col.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.yellow, 0.5f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        col.color = grad;

        ParticleSystemRenderer renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        // 保存为 Prefab (可选，这里直接生成在场景里方便你拖拽)
        // 为了方便你使用，我让它直接生成在场景里，你可以把它拖成 prefab
        go.transform.position = new Vector3(2, 0, 0);
    }
#endif
}