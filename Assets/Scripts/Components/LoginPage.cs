using System.Collections.Generic;
using RSG;
using Unity.Messenger;
using Unity.Messenger.Models;
using Unity.Messenger.Widgets;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Application = UnityEngine.Application;
using Window = Unity.Messenger.Window;

namespace ConnectApp.screens {
    public class LoginPage : StatefulWidget {
        public LoginPage(
            Key key = null
        ) : base(key: key) { }

        public override State createState() {
            return new _LoginPageState();
        }
    }

    public class _LoginPageState : State<LoginPage> {
        private readonly FocusNode _emailFocusNode = new FocusNode();
        private readonly FocusNode _passwordFocusNode = new FocusNode();
        private string _email = "";
        private string _password = "";
        private FocusScopeNode _focusScopeNode;
        private bool _isLoginLoading;
        private bool _isLoginFailed;

        private bool _isEmailFocus;
        private bool _isPasswordFocus;

        public override void initState() {
            base.initState();
            _isEmailFocus = true;
            _isPasswordFocus = false;
            _emailFocusNode.addListener(_focusNodeListener);
            _passwordFocusNode.addListener(_focusNodeListener);
        }

        private void _focusNodeListener() {
            if (_isEmailFocus == _emailFocusNode.hasFocus &&
                _isPasswordFocus == _passwordFocusNode.hasFocus)
                return;

            if (!(_emailFocusNode.hasFocus && _passwordFocusNode.hasFocus)) {
                _isEmailFocus = _emailFocusNode.hasFocus;
                _isPasswordFocus = _passwordFocusNode.hasFocus;
                setState(() => { });
            }
        }

        private void loginByEmail(string email, string password) {
            var promise = new Promise<LoginInfo>();
            var para = new Dictionary<string, string> {
                {"email", email},
                {"password", password}
            };
            _isLoginLoading = true;
            _isLoginFailed = false;
            Utils.Post<LoginInfo>($"/api/connectapp/v2/auth/live/login", data: para, (loginInfo) => {
                Window.loggedIn = loginInfo.userId != null;
                Window.currentUserId = loginInfo.userId;
                _isLoginLoading = false;
                if (Window.loggedIn) {
                    Window.OnLoggedIn();
                    UserInfoManager.saveUserInfo(loginInfo);
                }
                else {
                    _isLoginFailed = true;
                }
            });
        }

        private void _login() {
            _emailFocusNode.unfocus();
            _passwordFocusNode.unfocus();
            loginByEmail(_email, _password);
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: Colors.white,
                child: new Center(
                    child: _buildContent(context)
                )
            );
        }

        private Widget _buildContent(BuildContext context) {
            return new GestureDetector(
                onTap: () => {
                    if (_emailFocusNode.hasFocus) _emailFocusNode.unfocus();

                    if (_passwordFocusNode.hasFocus) _passwordFocusNode.unfocus();
                },
                child: new Container(
                    width: 450,
                    color: Colors.white,
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            _buildTopView(),
                            _buildMiddleView(context),
                            _buildBottomView()
                        }
                    )
                )
            );
        }

        public static IPromise<string> FetchCreateUnityIdUrl() {
            var promise = new Promise<string>();
            Utils.Get<Dictionary<string, string>>(
                "/api/connectapp/v1/authUrl?redirect_to=%2F&locale=zh_CN&is_reg=true",
                urlDict => { promise.Resolve(urlDict["url"]); });
            return promise;
        }

        private Widget _buildTopView() {
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
                                    onTap: () => FetchCreateUnityIdUrl().Then(url => Application.OpenURL(url)),
                                    child: new Text(
                                        "创建 Unity ID",
                                        style: new TextStyle(
                                            height: 1,
                                            fontSize: 16,
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
                                fontSize: 32,
                                fontFamily: "Roboto-Bold",
                                color: Colors.black
                            )
                        )
                    )
                }
            );
        }

        private Widget _buildMiddleView(BuildContext context) {
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
                                    fontSize: 14,
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
                                        _isEmailFocus ? new Color(0xFF2196F3) : new Color(0xFFE6E6E6),
                                        _isEmailFocus ? 2 : 1
                                    )
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: _emailFocusNode,
                                maxLines: 1,
                                autofocus: true,
                                style: new TextStyle(
                                    height: 1.33f,
                                    fontSize: 16,
                                    fontFamily: "Roboto-Regular",
                                    color: new Color(0xFF212121)
                                ),
                                cursorColor: new Color(0xFF2196F3),
                                keyboardType: TextInputType.emailAddress,
                                onChanged: text => _email = text,
                                onSubmitted: _ => {
                                    if (null == _focusScopeNode) _focusScopeNode = FocusScope.of(context);

                                    _focusScopeNode.requestFocus(_passwordFocusNode);
                                }
                            )
                        ),
                        new Container(height: 16),
                        new Container(
                            child: new Text(
                                "密码",
                                style: new TextStyle(
                                    height: 1.46f,
                                    fontSize: 14,
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
                                        _isPasswordFocus ? new Color(0xFF2196F3) : new Color(0xFFE6E6E6),
                                        _isPasswordFocus ? 2 : 1
                                    )
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: _passwordFocusNode,
                                maxLines: 1,
                                autofocus: false,
                                obscureText: true,
                                style: new TextStyle(
                                    height: 1.33f,
                                    fontSize: 16,
                                    fontFamily: "Roboto-Regular",
                                    color: new Color(0xFF212121)
                                ),
                                cursorColor: new Color(0xFF2196F3),
                                onChanged: text => _password = text,
                                onSubmitted: _ => _login()
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildBottomView() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 32),
                        new GestureDetector(
                            onTap: _login,
                            child: new Container(
                                padding: EdgeInsets.zero,
                                child: new Container(
                                    height: 48,
                                    decoration: new BoxDecoration(
                                        new Color(0xFF2196F3),
                                        borderRadius: BorderRadius.all(14)
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
                                                        fontSize: 16,
                                                        fontFamily: "Roboto-Regular",
                                                        color: Color.white
                                                    )
                                                )
                                            ),
                                            new Align(
                                                alignment: Alignment.centerRight,
                                                child: _isLoginLoading
                                                    ? new Container(
                                                        padding: EdgeInsets.only(right: 12),
                                                        child: new Loading(24, true))
                                                    : new Container()
                                            ),
                                        }
                                    )
                                )
                            )
                        ),
                        new Container(height: 16),
                        new GestureDetector(
                            onTap: () => Application.OpenURL("https://id.unity.com/password/new"),
                            child: new Container(
                                child: new Text(
                                    "忘记密码",
                                    style: new TextStyle(
                                        height: 1,
                                        fontSize: 14,
                                        fontFamily: "Roboto-Regular",
                                        color: new Color(0xFF616161)
                                    )
                                )
                            )
                        ),
                        new Container(height: 32),
                        _isLoginFailed
                            ? new Text("登录失败", style: new TextStyle(
                                height: 1,
                                fontSize: 14,
                                fontFamily: "Roboto-Regular",
                                color: new Color(0xFFF44336)
                            ))
                            : (Widget) new Container()
                    }
                )
            );
        }
    }
}