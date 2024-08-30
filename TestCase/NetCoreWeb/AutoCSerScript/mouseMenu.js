/// <reference path = "./menu.ts" />
/*include:menu*/
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
//鼠标覆盖菜单	<div id="YYY"></div><div mousemenu="{MenuId:'YYY',IsDisplay:1,Type:'Bottom',Top:5,Left:-5}" id="XXX"></div>
var AutoCSer;
(function (AutoCSer) {
    var MouseMenu = /** @class */ (function (_super) {
        __extends(MouseMenu, _super);
        function MouseMenu(Parameter) {
            var _this = _super.call(this, Parameter) || this;
            AutoCSer.Pub.GetParameterEvents(_this, MouseMenu.DefaultParameter, MouseMenu.DefaultEvents, Parameter);
            _this.HideFunction = AutoCSer.Pub.ThisFunction(_this, _this.Hide);
            _this.MouseOutFunction = AutoCSer.Pub.ThisFunction(_this, _this.MouseOut);
            AutoCSer.Declare.TryStart(_this, _this.Event);
            return _this;
        }
        MouseMenu.prototype.Start = function (Event) {
            this.OnStart.Function(this);
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                AutoCSer.HtmlElement.$AddEvent(Element, ['mouseout'], this.MouseOutFunction);
                if (this.IsMouseMove)
                    AutoCSer.HtmlElement.$AddEvent(Element, ['mousemove'], AutoCSer.Pub.ThisEvent(this, this.ReShow));
                this.CheckMenuParameter(true);
            }
            this.ClearInterval();
            this.IsOver = true;
            this.Show(Event);
        };
        MouseMenu.prototype.CheckMenuParameter = function (IsStart) {
            if (IsStart === void 0) { IsStart = false; }
            var Element = AutoCSer.HtmlElement.$Id(this.MenuId);
            if (Element.Element0()) {
                var Parameter = AutoCSer.HtmlElement.$Attribute(Element.Element0(), 'mousemenu');
                if (Parameter != null) {
                    var Id = eval('(' + Parameter + ')').Id;
                    if (Id != this.Id) {
                        Parameter = null;
                        var Menu = AutoCSer.Declare.Getters['MouseMenu'](Id, true);
                        if (Menu) {
                            Menu.Remove();
                            Element.Set('mousemenu', '{Id:"' + this.Id + '"}');
                            return;
                        }
                    }
                }
                if (IsStart) {
                    if (Parameter == null)
                        Element.Set('mousemenu', '{Id:"' + this.Id + '"}');
                    Element.AddEvent('mouseout', this.MouseOutFunction);
                }
            }
        };
        MouseMenu.prototype.Show = function (Event) {
            this.ShowMenu();
            if (this.IsMove)
                this.ReShow(Event, AutoCSer.HtmlElement.$Id(this.Id));
            this.OnShowed.Function();
        };
        MouseMenu.prototype.MouseOut = function () {
            this.IsOver = false;
            this.ClearInterval();
            this.HideInterval = setTimeout(this.HideFunction, this.Timeout);
        };
        MouseMenu.prototype.ClearInterval = function () {
            if (this.HideInterval) {
                clearTimeout(this.HideInterval);
                this.HideInterval = 0;
            }
        };
        MouseMenu.prototype.Hide = function () {
            this.ClearInterval();
            this.HideMenu();
        };
        MouseMenu.prototype.Remove = function () {
            this.ClearInterval();
            this.ShowView = null;
            var Element = AutoCSer.HtmlElement.$Id(this.Id);
            Element.DeleteEvent('mouseout', this.MouseOutFunction);
            AutoCSer.HtmlElement.$Id(this.MenuId).Set('mousemenu', '').DeleteEvent('mouseout', this.MouseOutFunction);
            this.Element = null;
        };
        MouseMenu.prototype.ReShow = function (Event, Element) {
            this.OnMove.Function(Event, this);
            this['To' + (this.Type || 'Mouse')](Event, Element);
        };
        MouseMenu.prototype.ToMouse = function (Event, Element) {
            this.CheckScroll(Event.MouseX, Event.MouseY);
        };
        MouseMenu.DefaultParameter = { Timeout: 100, IsMouseMove: 0 };
        MouseMenu.DefaultEvents = { OnMove: null };
        return MouseMenu;
    }(AutoCSer.Menu));
    AutoCSer.MouseMenu = MouseMenu;
    var MouseMenuEnum = /** @class */ (function () {
        function MouseMenuEnum(Value, Show) {
            this.Value = Value;
            this.Show = Show || Value;
        }
        MouseMenuEnum.prototype.ToJson = function (IsIgnore, IsNameQuery, IsSortName, Parents) {
            return AutoCSer.Pub.ToJson(this.Value, IsIgnore, IsNameQuery, IsSortName, Parents);
        };
        MouseMenuEnum.prototype.toString = function () {
            return this.Value;
        };
        return MouseMenuEnum;
    }());
    AutoCSer.MouseMenuEnum = MouseMenuEnum;
    AutoCSer.Declare.Create(MouseMenu, 'MouseMenu', 'mouseover', AutoCSer.DeclareType.ParameterId);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=mouseMenu.js.map