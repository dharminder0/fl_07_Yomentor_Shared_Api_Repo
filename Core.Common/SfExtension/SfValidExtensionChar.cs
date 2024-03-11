namespace Core.Common.SfExtension {
    public static class SfValidExtensionChar {
        public static bool IsValidRecordId(string recordId) {
            return recordId.Length == 15 || recordId.Length == 18;
            // Add additional validation logic if needed
        }
    }
}
