using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RSG;
using Unity.Messenger;
using Unity.Messenger.Models;
using Unity.Messenger.Style;
using Unity.UIWidgets.debugger;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine.UI;
using InputField = Unity.Messenger.InputField;
using Text = Unity.UIWidgets.widgets.Text;
using Window = Unity.Messenger.Window;

namespace ConnectApp.screens {
    public class LoginPage : StatefulWidget {
        public LoginPage(
            Key key = null
        ) : base(key: key) {
        }

        public override State createState() {
            return new _LoginPageState();
        }
    }

    public class _LoginPageState : State<LoginPage> {
        readonly FocusNode _emailFocusNode = new FocusNode();
        readonly FocusNode _passwordFocusNode = new FocusNode();
        private string _email = "";
        private string _password = "";
        FocusScopeNode _focusScopeNode;

        bool _isEmailFocus;
        bool _isPasswordFocus;

        public override void initState() {
            base.initState();
            this._isEmailFocus = true;
            this._isPasswordFocus = false;
            this._emailFocusNode.addListener(this._focusNodeListener);
            this._passwordFocusNode.addListener(this._focusNodeListener);
        }

        void _focusNodeListener() {
            if (this._isEmailFocus == this._emailFocusNode.hasFocus &&
                this._isPasswordFocus == this._passwordFocusNode.hasFocus) {
                return;
            }

            if (!(this._emailFocusNode.hasFocus && this._passwordFocusNode.hasFocus)) {
                this._isEmailFocus = this._emailFocusNode.hasFocus;
                this._isPasswordFocus = this._passwordFocusNode.hasFocus;
                this.setState(() => { });
            }
        }
        
        public static void LoginByEmail(string email, string password) {
            var promise = new Promise<LoginInfo>();
            var para = new Dictionary<string, string> {
                {"email", email},
                {"password", password}
            };
            Utils.Post<LoginInfo>($"/api/connectapp/v2/auth/live/login", data: para, (loginInfo) => {
                Window.loggedIn = true;
                Window.currentUserId = loginInfo.userId;
                Window.OnLoggedIn();
            });
        }

        void _login() {
            this._emailFocusNode.unfocus();
            this._passwordFocusNode.unfocus();
            LoginByEmail(this._email, this._password);
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: Colors.white,
                child: new Center(
                    child: this._buildContent(context)
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            return new GestureDetector(
                onTap: () => {
                    if (this._emailFocusNode.hasFocus) {
                        this._emailFocusNode.unfocus();
                    }

                    if (this._passwordFocusNode.hasFocus) {
                        this._passwordFocusNode.unfocus();
                    }
                },
                child: new Container(
                    width: 450,
                    color: Colors.white,
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            this._buildTopView(),
                            this._buildMiddleView(context),
                            this._buildBottomView()
                        }
                    )
                )
            );
        }
        
        public static IPromise<string> FetchCreateUnityIdUrl() {
            var promise = new Promise<string>();
            Utils.Get<Dictionary<string, string>>(
                "/api/connectapp/v1/authUrl?redirect_to=%2F&locale=zh_CN&is_reg=true",
                urlDict => {
                promise.Resolve(urlDict["url"]);
            });
            return promise;
        }

        Widget _buildTopView() {
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new Container(
                        height: 44,
                        padding: EdgeInsets.only(8, 8, 8),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new GestureDetector(
                                    onTap: () => FetchCreateUnityIdUrl().Then(url => UnityEngine.Application.OpenURL(url)),
                                    child: new Text(
                                        "创建 Unity ID",
                                        style: new TextStyle(
                                            height: 1,
                                            fontSize: 28,
                                            fontFamily: "Roboto-Medium",
                                            color: new Color(0xFF2196F3)
                                        )
                                    )
                                )
                            }
                        )
                    ),
                    new Container(height: 16),
                    new Container(
                        padding: EdgeInsets.symmetric(horizontal: 16),
                        child: new Text(
                            "登录你的Unity账号",
                            style: new TextStyle(
                                height: 1.11f,
                                fontSize: 48,
                                fontFamily: "Roboto-Bold",
                                color: Colors.black
                            )
                        )
                    )
                }
            );
        }

        Widget _buildMiddleView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 32),
                        new Container(
                            child: new Text(
                                "邮箱",
                                style: new TextStyle(
                                    height: 1.46f,
                                    fontSize: 24,
                                    fontFamily: "Roboto-Medium",
                                    color: new Color(0xFF797979)
                                )
                            )
                        ),
                        new Container(
                            height: 46,
                            decoration: new BoxDecoration(
                                new Color(0x00000000),
                                border: new Border(
                                    bottom: new BorderSide(
                                        this._isEmailFocus ? new Color(0xFF2196F3) : new Color(0xFFE6E6E6),
                                        this._isEmailFocus ? 2 : 1
                                    )
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: this._emailFocusNode,
                                maxLines: 1,
                                autofocus: true,
                                style: new TextStyle(
                                    height: 1.33f,
                                    fontSize: 28,
                                    fontFamily: "Roboto-Regular",
                                    color: new Color(0xFF212121)
                                ),
                                cursorColor: new Color(0xFF2196F3),
                                keyboardType: TextInputType.emailAddress,
                                onChanged: text => this._email = text,
                                onSubmitted: _ => {
                                    if (null == this._focusScopeNode) {
                                        this._focusScopeNode = FocusScope.of(context);
                                    }

                                    this._focusScopeNode.requestFocus(this._passwordFocusNode);
                                }
                            )
                        ),
                        new Container(height: 16),
                        new Container(
                            child: new Text(
                                "密码",
                                style: new TextStyle(
                                    height: 1.46f,
                                    fontSize: 24,
                                    fontFamily: "Roboto-Medium",
                                    color: new Color(0xFF797979)
                                )
                            )
                        ),
                        new Container(
                            height: 46,
                            decoration: new BoxDecoration(
                                new Color(0x00000000),
                                border: new Border(
                                    bottom: new BorderSide(
                                        this._isPasswordFocus ? new Color(0xFF2196F3) : new Color(0xFFE6E6E6),
                                        this._isPasswordFocus ? 2 : 1
                                    )
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: this._passwordFocusNode,
                                maxLines: 1,
                                autofocus: false,
                                obscureText: true,
                                style: new TextStyle(
                                    height: 1.33f,
                                    fontSize: 28,
                                    fontFamily: "Roboto-Regular",
                                    color: new Color(0xFF212121)
                                ),
                                cursorColor: new Color(0xFF2196F3),
                                onChanged: text => this._password = text,
                                onSubmitted: _ => this._login()
                            )
                        )
                    }
                )
            );
        }

        Widget _buildBottomView() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 32),
                        new GestureDetector(
                            onTap: this._login,
                            child: new Container(
                            padding: EdgeInsets.zero,
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    new Color(0xFF2196F3),
                                    borderRadius: BorderRadius.all(24)
                                ),
                                child: new Stack(
                                    children: new List<Widget> {
                                        new Align(
                                            alignment: Alignment.center,
                                            child: new Text(
                                                "确定",
                                                maxLines: 1,
                                                style: new TextStyle(
                                                    height: 1.33f,
                                                    fontSize: 28,
                                                    fontFamily: "Roboto-Regular",
                                                    color: Color.white
                                                )
                                            )
                                        )
                                    }
                                )
                            )
                            )
                        ),
                        new Container(height: 16),
                        new GestureDetector(
                            onTap: () => UnityEngine.Application.OpenURL("https://id.unity.com/password/new"),
                            child: new Container(
                            child: new Text(
                                "忘记密码",
                                style: new TextStyle(
                                height: 1,
                                fontSize: 24,
                                fontFamily: "Roboto-Regular",
                                color: new Color(0xFF616161)
                            )
                            )
                            )
                        )
                    }
                )
            );
        }
    }
}

