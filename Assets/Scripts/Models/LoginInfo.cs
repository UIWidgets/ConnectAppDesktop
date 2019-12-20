using System;

namespace Unity.Messenger.Models {
    [Serializable]
    public class LoginInfo {
        public string LSKey;
        public string userId;
        public string userFullName;
        public string userAvatar;
        public string authId;
        public bool anonymous;
        public string title;
        public string coverImageWithCDN;
    }
}