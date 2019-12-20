using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RSG;
using Unity.Messenger.Widgets;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity.Messenger
{
    public partial class Utils
    {
        public static void Get<T>(
            string url,
            Action<T> action)
        {
            Get<T>(
                url
            ).Then(action);
        }

        public static IPromise<T> Get<T>(
            string url)
        {
            return new Promise<T>(isSync: true, resolver: (resolve, reject) =>
            {
                var request = UnityWebRequest.Get($"https://connect.unity.com{url}");
                request.SetRequestHeader("X-Requested-With", "XMLHTTPREQUEST");
                if (getCookie().isNotEmpty()) {
                    request.SetRequestHeader("Cookie", getCookie());
                }

                request.SendWebRequest().completed += operation =>
                {
                    var content = DownloadHandlerBuffer.GetContent(request);
                    var response = JsonConvert.DeserializeObject<T>(content);
                    resolve(response);
                    if (request.GetResponseHeaders().ContainsKey("Set-Cookie")) {
                        var cookie = request.GetResponseHeaders()["Set-Cookie"];
                        updateCookie(cookie);
                    }
                };
            });
        }

        public static void Post<T>(
            string url,
            object data,
            Action<T> action
        )
        {
            Post<T>(
                url,
                data
            ).Then(action);
        }

        public static IPromise<T> Post<T>(
            string url,
            object data)
        {
            return new Promise<T>(isSync: true, resolver: (resolve, reject) =>
            {
                
                var request = new UnityWebRequest(
                    $"https://connect.unity.com{url}",
                    UnityWebRequest.kHttpVerbPOST
                )
                {
                    downloadHandler = new DownloadHandlerBuffer(),
                };
                if (data != null)
                {
                    var requestBody = JsonConvert.SerializeObject(data);
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestBody));
                }
                request.SetRequestHeader("X-Requested-With", "XMLHTTPREQUEST");
                request.SetRequestHeader("Content-Type", "application/json");
                if (getCookie().isNotEmpty()) {
                    request.SetRequestHeader("Cookie", getCookie());
                }

                request.SendWebRequest().completed += operation =>
                {
                    var content = DownloadHandlerBuffer.GetContent(request);
                    var response = JsonConvert.DeserializeObject<T>(content);
                    resolve(response);
                    if (request.GetResponseHeaders().ContainsKey("Set-Cookie")) {
                        var cookie = request.GetResponseHeaders()["Set-Cookie"];
                        updateCookie(cookie);
                    }
                };
            });
        }

        public static void PostForm<T>(
            string url,
            List<IMultipartFormSection> formSections,
            Action<T> action,
            out Func<float> progress)
        {
            PostForm<T>(
                url,
                formSections,
                out progress
            ).Then(action);
        }

        public static IPromise<T> PostForm<T>(
            string url,
            List<IMultipartFormSection> formSections,
            out Func<float> progress)
        {
            var request = UnityWebRequest.Post(
                $"https://connect.unity.com{url}",
                formSections
            );
            request.SetRequestHeader("X-Requested-With", "XMLHTTPREQUEST");
            if (getCookie().isNotEmpty()) {
                request.SetRequestHeader("Cookie", getCookie());
            }
            progress = () => request.uploadProgress;
            
            return new Promise<T>(isSync: true, resolver: (resolve, reject) =>
            {
                request.SendWebRequest().completed += operation =>
                {
                    var content = DownloadHandlerBuffer.GetContent(request);
                    var response = JsonConvert.DeserializeObject<T>(content);
                    resolve(response);
                    if (request.GetResponseHeaders().ContainsKey("Set-Cookie")) {
                        var cookie = request.GetResponseHeaders()["Set-Cookie"];
                        updateCookie(cookie);
                    }
                };
                
            });
        }
        
        public const string COOKIE = "Cookie";
        
        static string _cookieHeader() {
            if (PlayerPrefs.GetString(COOKIE).isNotEmpty()) {
                return PlayerPrefs.GetString(COOKIE);
            }

            return "";
        }

        public static void clearCookie() {
            PlayerPrefs.SetString(COOKIE, "");
            PlayerPrefs.Save();
        }

        public static string getCookie() {
            return _cookieHeader();
        }

        public static string getCookie(string key) {
            var cookie = getCookie();
            if (cookie.isNotEmpty()) {
                var cookieArr = cookie.Split(';');
                foreach (var c in cookieArr) {
                    var carr = c.Split('=');

                    if (carr.Length != 2) {
                        continue;
                    }

                    var name = carr[0].Trim();
                    var value = carr[1].Trim();
                    if (name == key) {
                        return value;
                    }
                }
            }

            return "";
        }

        public static void updateCookie(string newCookie) {
            var cookie = PlayerPrefs.GetString(COOKIE);
            var cookieDict = new Dictionary<string, string>();
            var updateCookie = "";
            if (cookie.isNotEmpty()) {
                var cookieArr = cookie.Split(';');
                foreach (var c in cookieArr) {
                    var name = c.Split('=').first();
                    cookieDict.Add(name, c);
                }
            }

            if (newCookie.isNotEmpty()) {
                var newCookieArr = newCookie.Split(',');
                foreach (var c in newCookieArr) {
                    var item = c.Split(';').first();
                    var name = item.Split('=').first();
                    if (cookieDict.ContainsKey(name)) {
                        cookieDict[name] = item;
                    }
                    else {
                        cookieDict.Add(name, item);
                    }
                }

                var updateCookieArr = cookieDict.Values;
                updateCookie = string.Join(";", updateCookieArr);
            }

            if (updateCookie.isNotEmpty()) {
                PlayerPrefs.SetString(COOKIE, updateCookie);
                PlayerPrefs.Save();
            }
        }
    }
}