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
    var TouchTopParameter = /** @class */ (function () {
        function TouchTopParameter() {
        }
        return TouchTopParameter;
    }());
    AutoCSer.TouchTopParameter = TouchTopParameter;
    var TouchTop = /** @class */ (function (_super) {
        __extends(TouchTop, _super);
        function TouchTop(Parameter) {
            if (Parameter === void 0) { Parameter = null; }
            var _this = _super.call(this) || this;
            AutoCSer.Pub.GetParameterEvents(_this, TouchTop.DefaultParameter, TouchTop.DefaultEvents, Parameter);
            _this.CheckFunction = AutoCSer.Pub.ThisEvent(_this, _this.Check);
            AutoCSer.HtmlElement.$AddEvent(document, ['touchstart'], AutoCSer.Pub.ThisEvent(_this, _this.Start));
            AutoCSer.HtmlElement.$AddEvent(document, ['touchmove'], AutoCSer.Pub.ThisEvent(_this, _this.Move));
            AutoCSer.HtmlElement.$AddEvent(document, ['touchend'], AutoCSer.Pub.ThisEvent(_this, _this.End));
            AutoCSer.HtmlElement.$AddEvent(document, ['touchcancel'], AutoCSer.Pub.ThisEvent(_this, _this.Cancel));
            return _this;
        }
        TouchTop.prototype.Start = function (Event) {
            this.Cancel();
            this.Step = 1;
            this.StartTop = this.EndTop = Event.Event.touches[0].pageY;
        };
        TouchTop.prototype.Move = function (Event) {
            if (this.Step == 1)
                this.Step = 2;
            if (this.Step == 2) {
                this.EndTop = Event.Event.touches[0].pageY;
                this.Check();
            }
        };
        TouchTop.prototype.Check = function () {
            if (this.Step == 2 && (this.EndTop - this.StartTop) > this.Top && AutoCSer.HtmlElement.$GetScrollTop() == 0) {
                this.Cancel();
                this.OnTop.Function();
            }
        };
        TouchTop.prototype.End = function (Event) {
            if (this.Step == 2) {
                this.EndTop = Event.Event.changedTouches[0].pageY;
                if (!this.Interval)
                    this.Interval = setInterval(this.CheckFunction, this.CheckTimeout);
                this.Check();
            }
        };
        TouchTop.prototype.Cancel = function () {
            this.Step = 0;
            if (this.Interval) {
                clearInterval(this.Interval);
                this.Interval = 0;
            }
        };
        TouchTop.DefaultParameter = { Top: 12, CheckTimeout: 200 };
        TouchTop.DefaultEvents = { OnTop: null };
        TouchTop.Default = new TouchTop();
        return TouchTop;
    }(TouchTopParameter));
    AutoCSer.TouchTop = TouchTop;
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=touchTop.js.map