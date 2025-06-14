namespace RecipeShare.Web.Components.Layout.Sections.Footer
{
    public class SnapZoneConfig
    {
        public int SnapZoneCount { get; set; } = 6; // Default max
        public int Min => 3;
        public int Max => 6;

        public int ClampedCount => Math.Clamp(SnapZoneCount, Min, Max);
    }
}
