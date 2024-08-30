/// <reference path = "./load.page.ts" />
'use strict';
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var AutoCSer;
(function (AutoCSer) {
    var PollParameter = /** @class */ (function () {
        function PollParameter() {
        }
        return PollParameter;
    }());
    AutoCSer.PollParameter = PollParameter;
    var Poll = /** @class */ (function (_super) {
        __extends(Poll, _super);
        function Poll(Parameter) {
            if (Parameter === void 0) { Parameter = null; }
            var _this = _super.call(this) || this;
            AutoCSer.Pub.GetParameterEvents(_this, Poll.DefaultParameter, Poll.DefaultEvents, Parameter);
            if (!_this.Query)
                _this.Query = {};
            if (_this.Path == null)
                _this.Path = _this.IsView ? '/poll.html' : 'poll';
            _this.GetFunction = AutoCSer.Pub.ThisFunction(_this, _this.Get);
            _this.VerifyFunction = AutoCSer.Pub.ThisFunction(_this, _this.Verify);
            _this.OnVerifyFunction = AutoCSer.Pub.ThisFunction(_this, _this.OnVerify);
            AutoCSer.IndexPool.Push(_this);
            return _this;
        }
        Poll.prototype.Start = function (Verify) {
            if (Verify != null)
                this.VerifyInfo = Verify;
            if (!this.Identity)
                setTimeout(this.GetFunction, this.Identity = 1);
        };
        Poll.prototype.Close = function () {
            AutoCSer.IndexPool.Pop(this);
        };
        Poll.prototype.Stop = function () {
            this.Identity = 0;
        };
        Poll.prototype.Get = function () {
            if (this.Identity) {
                if (this.OnMessage.Get().length) {
                    AutoCSer.Pub.AppendJs((this.Domain == null ? '//__POLLDOMAIN__' : (this.Domain ? '//' + this.Domain : '')) + '__AJAX__?__AJAXCALL__=' + this.Path + '&__CALLBACK__=' + AutoCSer.IndexPool.ToString(this) + '.OnGet' + (this.IsView ? 'View' : '') + '&__JSON__=' + AutoCSer.Pub.ToJson({ verify: this.VerifyInfo, query: this.Query }, true, false).Escape() + (AutoCSer.Pub.IE ? '&t=' + (new Date).getTime() : ''), null, null, AutoCSer.Pub.ThisFunction(this, this.OnError, [this.Identity]));
                }
                else
                    setTimeout(this.GetFunction, 1000);
            }
        };
        Poll.prototype.OnGet = function (Value) {
            this.OnGetView(Value ? Value.Result : null);
        };
        Poll.prototype.OnGetView = function (Value) {
            if (this.Identity) {
                ++this.Identity;
                if (Value) {
                    if (!this.VerifyPath || Value.isVerify) {
                        this.VerifyTimeout = 100;
                        if (this.ReturnName) {
                            if (Value[this.ReturnName])
                                this.OnMessage.Function(Value[this.ReturnName]);
                        }
                        else
                            this.OnMessage.Function(Value);
                        if (this.Identity)
                            setTimeout(this.GetFunction, 100);
                    }
                    else {
                        if ((this.VerifyTimeout *= 2) > 2000)
                            this.VerifyTimeout = 2000;
                        this.Verify();
                    }
                }
                else
                    setTimeout(this.GetFunction, 2000);
            }
        };
        Poll.prototype.OnError = function (Event, Identity) {
            if (Identity == this.Identity)
                setTimeout(AutoCSer.Pub.ThisFunction(this, this.Check, [Identity]), 2000);
        };
        Poll.prototype.Check = function (Identity) {
            if (Identity == this.Identity)
                setTimeout(this.GetFunction, 8000);
        };
        Poll.prototype.Verify = function () {
            AutoCSer.HttpRequest.Post(this.VerifyPath, null, this.OnVerifyFunction);
        };
        Poll.prototype.OnVerify = function (Value) {
            if (Value && Value.Result && Value.Result.IsVerify) {
                this.VerifyInfo = Value.Result;
                if (this.Identity)
                    setTimeout(this.GetFunction, this.VerifyTimeout);
            }
            else if (this.Identity)
                setTimeout(this.VerifyFunction, 8000);
        };
        Poll.DefaultParameter = { Domain: null, Path: null, VerifyPath: 'poll.Verify', Query: null, IsView: true, ReturnName: null, VerifyTimeout: 100 };
        Poll.DefaultEvents = { OnMessage: null };
        Poll.Default = new Poll();
        return Poll;
    }(PollParameter));
    AutoCSer.Poll = Poll;
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=poll.js.map