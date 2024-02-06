public static class GlobalValues
{
    public static bool CanDragInventoryItems { get; set; }
    public static bool CanLookAround { get; set; } = true;
    public static float Sensitivity { get; set; } = 1;
    public static int GraphicsQualityIndex { get; set; }
    public static int FixedFPS { get; set; } = 30;
    public static float CameraFarDistance { get; set; } = 100f;
    public static float CameraFOV { get; set; } = 60f;
    public static bool EnableShadows { get; set; }
    public static bool EnableGrass { get; set; }
    public static bool AdministratorBuild { get; set; } = true;
}