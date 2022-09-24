namespace DiscordLogHook.Data {
    internal class AllowedMentions {
        public MentionTypes[] parse;
        public string[] roles;
        public string[] users;
        public bool replied_user;
    }

    internal enum MentionTypes {
        roles, users, everyone
    }
}
