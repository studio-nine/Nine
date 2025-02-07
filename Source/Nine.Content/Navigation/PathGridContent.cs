namespace Nine.Serialization.Navigation
{
    using System.Xaml;

    /// <summary>
    /// Contains attached properties for path grid.
    /// </summary>
    static class PathGridContent
    {
        #region Attached Properties
        static readonly AttachableMemberIdentifier IsPathProperty = new AttachableMemberIdentifier(typeof(PathGridContent), "IsPath");
        static readonly AttachableMemberIdentifier IsObstacleProperty = new AttachableMemberIdentifier(typeof(PathGridContent), "IsObstacle");

        /// <summary>
        /// Gets or sets whether the target is a path.
        /// </summary>
        public static bool GetIsPath(object target)
        {
            bool isPath = false;
            AttachablePropertyServices.TryGetProperty(target, IsPathProperty, out isPath);
            return isPath;
        }

        /// <summary>
        /// Gets or sets whether the target is a path.
        /// </summary>
        public static void SetIsPath(object target, bool value)
        {
            AttachablePropertyServices.SetProperty(target, IsPathProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the target is an obstacle.
        /// </summary>
        public static bool GetIsObstacle(object target)
        {
            bool isObstacle = false;
            AttachablePropertyServices.TryGetProperty(target, IsObstacleProperty, out isObstacle);
            return isObstacle;
        }

        /// <summary>
        /// Gets or sets whether the target is an obstacle.
        /// </summary>
        public static void SetIsObstacle(object target, bool value)
        {
            AttachablePropertyServices.SetProperty(target, IsObstacleProperty, value);
        }
        #endregion
    }
}