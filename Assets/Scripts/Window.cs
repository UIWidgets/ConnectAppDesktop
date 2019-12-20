using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;
using RSG;
using Unity.Messenger.Components;
using Unity.Messenger.Models;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.widgets;
using WebSocketSharp;

namespace Unity.Messenger {
    public partial class Window : UIWidgetsPanel {
        private static Window _instance;

        private static void UpdateWindowCanvas() {
            if (_instance != null)
                using (_instance.window.getScope()) {
                    HomePage.currentState.setState();
                }
        }

        public static void OnLoggedIn() {
            Utils.Get<DiscoverChannelsResponse>(
                "/api/connectapp/v1/channels?discover=true"
            ).Then(discoverResponse => {
                foreach (var key in discoverResponse.channelMap.Keys)
                    if (!discoverResponse.channelMap[key].groupId.IsNullOrEmpty())
                        discoverResponse.channelMap[key].topic = discoverResponse
                            .groupFullMap[discoverResponse.channelMap[key].groupId].description;

                DiscoverChannels.AddRange(
                    discoverResponse.discoverList
                        .Select(channelId => discoverResponse.channelMap[channelId])
                );
                UpdateWindowCanvas();
                InitializeWebSocket().Then(SendConnectFrame);
            });
        }

        private static void SendConnectFrame() {
            var frameSz = JsonConvert.SerializeObject(new ConnectFrame {
                opCode = 1,
                data = new ConnectFrameData {
                    loginSession = Utils.getCookie("LS"),
                    commitId = "0e8d784",
                    properties = new Dictionary<string, object>(),
                    clientType = "connect",
                    isApp = true,
                },
            });
            _client.Send(frameSz);
        }

        public static void OnLoggedOut() {
            loggedIn = false;
            currentUserId = null;
            socketConnected = false;
            Users.Clear();
            ReadStates.Clear();
            Messages.Clear();
            Channels.Clear();
            Members.Clear();
            Groups.Clear();
            HasMoreMembers.Clear();
            DiscoverChannels.Clear();
            if (_instance != null)
                using (_instance.window.getScope()) {
                    HomePage.currentState.Clear();
                }

            Utils.clearCookie();
            UserInfoManager.clearUserInfo();
            _client.Close();
            UpdateWindowCanvas();
        }

        private static bool _appFocused;
        internal static bool loggedIn;
        internal static string currentUserId;
        internal static bool socketConnected = false;
        private static WebSocket _client;
        private static Timer _pingTimer;
        private static Timer _timeoutTimer;
        private static long _lastPingTimestamp;
        internal static bool reconnecting = false;

        private const string DefaultWorkspaceId = "05a748aedac0c000";

        private static readonly Dictionary<string, string> OverrideNames = new Dictionary<string, string> {
            ["00b6435ce0000000"] = "Unity Connect 1号大厅",
            ["00b4f9c5e0000000"] = "Unity Connect 2号大厅",
        };

        private static readonly Dictionary<Type, Action<IFrame>> Processors = new Dictionary<Type, Action<IFrame>> {
            [typeof(PingFrame)] = frame => ProcessPingFrame((PingFrame) frame),
            [typeof(ReadyFrame)] = frame => ProcessReadyFrame((ReadyFrame) frame),
            [typeof(MessageCreateFrame)] = frame => ProcessMessageCreateFrame((MessageCreateFrame) frame),
            [typeof(MessageDeleteFrame)] = frame => ProcessMessageDeleteFrame((MessageDeleteFrame) frame),
            [typeof(MessageUpdateFrame)] = frame => ProcessMessageUpdateFrame((MessageUpdateFrame) frame),
            [typeof(ChannelDeleteFrame)] = frame => ProcessChannelDeleteFrame((ChannelDeleteFrame) frame),
            [typeof(ChannelCreateFrame)] = frame => ProcessChannelCreateFrame((ChannelCreateFrame) frame),
            [typeof(ChannelUpdateFrame)] = frame => ProcessChannelUpdateFrame((ChannelUpdateFrame) frame),
        };

        internal static readonly Dictionary<string, bool> PullFlags = new Dictionary<string, bool>();
        private static readonly Dictionary<string, User> Users = new Dictionary<string, User>();
        private static readonly Dictionary<string, ReadState> ReadStates = new Dictionary<string, ReadState>();
        internal static readonly Dictionary<string, List<Message>> Messages = new Dictionary<string, List<Message>>();
        internal static readonly List<Message> NewMessages = new List<Message>();
        internal static readonly Dictionary<string, Channel> Channels = new Dictionary<string, Channel>();
        internal static readonly Dictionary<string, Group> Groups = new Dictionary<string, Group>();

        private static readonly Dictionary<string, List<ChannelMember>> Members =
            new Dictionary<string, List<ChannelMember>>();

        private static readonly Dictionary<string, bool> HasMoreMembers = new Dictionary<string, bool>();

        private static readonly List<Channel> DiscoverChannels = new List<Channel>();

        private static IPromise InitializeWebSocket() {
            _client = new WebSocket("wss://connect-gateway.unity.com/v1");
            _client.OnClose += (sender, e) => {
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    reconnecting = true;
                    UpdateWindowCanvas();
                    InitializeWebSocket().Then(SendConnectFrame);
                });
            };
            _client.OnMessage += (sender, args) => {
                var frame = JsonConvert.DeserializeObject<IFrame>(args.Data);
                var type = frame.GetType();
                if (Processors.ContainsKey(type)) {
                    Processors[type](frame);
                    UnityMainThreadDispatcher.Instance().Enqueue(UpdateWindowCanvas);
                }
            };
            return _client.AsyncConnect();
        }

        private readonly Application m_Application = new Application();

        protected override void OnEnable() {
            base.OnEnable();
            _instance = this;
            UIWidgets.ui.Window.onFrameRateCoolDown = () => { };
            m_Application.OnEnable();
            loggedIn = UserInfoManager.isLogin();
            currentUserId = loggedIn ? UserInfoManager.getUserInfo().userId : null;
            if (loggedIn) OnLoggedIn();
        }

        protected override void OnDisable() {
            _instance = null;
            base.OnDisable();
        }

        protected override void Update() {
            base.Update();
            UnityMainThreadDispatcher.Instance().Update();
        }

        protected override Widget createWidget() {
            return m_Application.CreateWidget(
                users: Users,
                readStates: ReadStates,
                messages: Messages,
                channels: Channels,
                members: Members,
                hasMoreMembers: HasMoreMembers,
                groups: Groups,
                pullFlags: PullFlags,
                discoverChannels: DiscoverChannels
            );
        }
    }
}