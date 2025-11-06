namespace Utils
{

    public static class Database
    {
        public const int MaxRetryAttempts = 3;
        public const int RetryDelayMS = 100;
        public const string DbPath = "PlayerSaveData.db";
        public const string DbPathParam = "dbPath";
    }

    public static class Repository
    {
        public const string PlayerStateTable = "PlayerState";
        public const string PlayerDocumentsTable = "PlayerDocuments";
        public const string PlayerGeneralItemsTable = "PlayerGeneralItems";
        public const int PlayerId = 1;

        public const string ProgressStateTable = "GameProgressState";
        public const string PersuadedSinnersTable = "PersuadedSinners";
        public const string CompletedEventsTable = "CompletedStoryEvents";
        public const string DiscoveredDocumentsTable = "DiscoveredDocuments";
        public const string WorldStateChangesTable = "WorldStateChanges";
        public const int ProgressId = 1;

    }

    public static class SceneLoaderConst
    {
        public const string PreviousSceneKey = "forggeting.PreviousScenePath";
        public const string BootstrapscenePath = "Assets/Scenes/Initialization.unity";
    }
}
