/// <reference path = "./load.page.ts" />
'use strict';
//鼠标拖拽	<div body="true" floatcenter="true" id="YYY" style="display:none"><div mousemove="{MoveId:'YYY'}" id="XXX"></div></div>
var AutoCSer;
(function (AutoCSer) {
    var MouseMove = /** @class */ (function () {
        function MouseMove(Parameter) {
            AutoCSer.Pub.GetParameter(this, MouseMove.DefaultParameter, Parameter);
            this.MoveEvent = AutoCSer.Pub.ThisEvent(this, this.Move);
            this.StopEvent = AutoCSer.Pub.ThisFunction(this, this.Stop);
            AutoCSer.Declare.TryStart(this, this.Event);
        }
        MouseMove.prototype.Start = function (Event) {
            if (this.IsStart)
                this.Stop();
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                AutoCSer.HtmlElement.$(document.body).AddEvent('mousedown,mouseup,blur', this.StopEvent).AddEvent('mousemove', this.MoveEvent);
            }
            var Element = AutoCSer.HtmlElement.$IdElement(this.MoveId);
            Element.style.position = 'absolute';
            this.Left = Element.offsetLeft;
            this.Top = Element.offsetTop;
            this.ClientX = Event.MouseX;
            this.ClientY = Event.MouseY;
            this.IsStart = true;
        };
        MouseMove.prototype.Move = function (Event) {
            AutoCSer.HtmlElement.$Id(this.MoveId).Left(this.Left + Event.MouseX - this.ClientX).Top(this.Top + Event.MouseY - this.ClientY);
        };
        MouseMove.prototype.Stop = function () {
            if (this.IsStart) {
                AutoCSer.HtmlElement.$(document.body).DeleteEvent('mousedown,mouseup,blur', this.StopEvent).DeleteEvent('mousemove', this.MoveEvent);
                this.IsStart = false;
            }
        };
        MouseMove.DefaultParameter = { Id: null, Event: null, MoveId: null };
        return MouseMove;
    }());
    AutoCSer.MouseMove = MouseMove;
    AutoCSer.Declare.Create(MouseMove, 'MouseMove', 'mousedown', AutoCSer.DeclareType.ParentName);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=mouseMove.js.map