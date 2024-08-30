/// <reference path = "./load.page.ts" />
'use strict';
//鼠标覆盖处理	<div mouseover="{}" id="XXX"></div>
var AutoCSer;
(function (AutoCSer) {
    var MouseOver = /** @class */ (function () {
        function MouseOver(Parameter) {
            AutoCSer.Pub.GetParameterEvents(this, MouseOver.DefaultParameter, MouseOver.DefaultEvents, Parameter);
            AutoCSer.Declare.TryStart(this, this.Event);
        }
        MouseOver.prototype.Start = function (Event) {
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                AutoCSer.HtmlElement.$AddEvent(Element, ['mouseout'], AutoCSer.Pub.ThisEvent(this, this.Out));
            }
            this.OnOver.Function(Event, Element);
        };
        MouseOver.prototype.Out = function (Event) {
            this.OnOut.Function(Event, AutoCSer.HtmlElement.$IdElement(this.Id));
        };
        MouseOver.DefaultParameter = { Id: null, Event: null };
        MouseOver.DefaultEvents = { OnOver: null, OnOut: null };
        return MouseOver;
    }());
    AutoCSer.MouseOver = MouseOver;
    AutoCSer.Declare.Create(MouseOver, 'MouseOver', 'mouseover', AutoCSer.DeclareType.ParentName);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=mouseOver.js.map