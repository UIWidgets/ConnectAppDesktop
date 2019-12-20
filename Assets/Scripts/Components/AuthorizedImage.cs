using Newtonsoft.Json;
using Unity.Messenger.Models;
using Unity.Messenger.Widgets;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine.Networking;

namespace Unity.Messenger.Components
{
    public class AuthorizedImage : StatefulWidget
    {
        public AuthorizedImage(
            string url
        )
        {
            Url = url;
        }

        internal readonly string Url;

        public override State createState()
        {
            return new AuthorizedImageState();
        }
    }

    public class AuthorizedImageState : State<AuthorizedImage>
    {
        public override void initState()
        {
            base.initState();
            var request = UnityWebRequest.Get(
                widget.Url
            );
            request.SetRequestHeader("Cookie", $"LS={Window.loginSession}");
            var asyncOperation = request.SendWebRequest();
            asyncOperation.completed += operation =>
            {
                if (mounted)
                {
                    var content = DownloadHandlerBuffer.GetContent(request);
                    using (WindowProvider.of(context).getScope())
                    {
                        var response = JsonConvert.DeserializeObject<GetMessagesResponse>(content);
                    }
                }
            };
        }

        public override Widget build(BuildContext context)
        {
            return Image.network(
                ""
            );
        }
    }
}