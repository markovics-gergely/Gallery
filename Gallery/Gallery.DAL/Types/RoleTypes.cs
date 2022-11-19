namespace Gallery.DAL.Types
{
    /// <summary>
    /// Types of user roles
    /// </summary>
    public static class RoleTypes
    {
        /// <summary>
        /// Administrator role of the application
        /// </summary>
        public const string Admin = "Admin";
        /// <summary>
        /// Regular user role of the application
        /// </summary>
        public const string Regular = "Regular";
        /// <summary>
        /// All possible role of the application
        /// </summary>
        public const string All = "Admin, Regular";

        /// <summary>
        /// Scope name of the role
        /// </summary>
        public const string RoleScope = "roles";
    }
}
