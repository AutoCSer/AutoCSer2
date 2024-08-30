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
//鼠标点击菜单	<div id="YYY"></div><div clickmenu="{MenuId:'YYY',IsDisplay:1,Type:'Bottom',Top:5,Left:-5}" onclick="void(0);" id="XXX"></div>
var AutoCSer;
(function (AutoCSer) {
    var ManyClickMenu = /** @class */ (function (_super) {
        __extends(ManyClickMenu, _super);
        function ManyClickMenu(Parameter) {
            var _this = _super.call(this, Parameter) || this;
            AutoCSer.Pub.GetEvents(_this, ManyClickMenu.DefaultEvents, Parameter);
            AutoCSer.HtmlElement.$(document.body).AddEvent('click', AutoCSer.Pub.ThisEvent(_this, _this.Check));
            _this.Reset(Parameter, Parameter.DeclareElement);
            return _this;
        }
        ManyClickMenu.prototype.Reset = function (Parameter, Element) {
            if (Element != this.Element) {
                if (this.Element)
                    this.HideMenu();
                this.Element = Element;
                this.IsOver = false;
                this.OnReset.Function(this, Parameter, Element);
            }
            this.Show();
        };
        ManyClickMenu.prototype.Check = function (Event) {
            if (!Event.$ParentName("manyclickmenu"))
                this.Hide();
        };
        ManyClickMenu.prototype.Show = function () {
            if (this.IsOver)
                this.Hide();
            else {
                this.OnStart.Function(this);
                this.ShowMenu();
                if (this.IsMove)
                    this['To' + (this.Type || 'Bottom')](Event, AutoCSer.HtmlElement.$(this.Element));
                this.OnShowed.Function();
                this.IsOver = true;
            }
        };
        ManyClickMenu.prototype.Hide = function () {
            this.HideMenu();
            this.IsOver = false;
        };
        ManyClickMenu.DefaultEvents = { OnReset: null };
        return ManyClickMenu;
    }(AutoCSer.Menu));
    AutoCSer.ManyClickMenu = ManyClickMenu;
    AutoCSer.Declare.CreateMany(ManyClickMenu, 'ManyClickMenu', 'click', AutoCSer.DeclareManyType.ResetParentName);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=manyClickMenu.js.map