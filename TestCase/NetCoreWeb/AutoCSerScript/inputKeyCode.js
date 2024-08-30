/// <reference path = "./load.page.ts" />
'use strict';
//键盘事件	<input inputkeycode="{Keys:{13:AutoCSer.Pub.Alert}}" id="XXX" />
var AutoCSer;
(function (AutoCSer) {
    var InputKeyCode = /** @class */ (function () {
        function InputKeyCode(Parameter) {
            AutoCSer.Pub.GetParameterEvents(this, InputKeyCode.DefaultParameter, InputKeyCode.DefaultEvents, Parameter);
            if (!this.Keys)
                this.Keys = {};
            AutoCSer.Declare.TryStart(this, this.Event);
        }
        InputKeyCode.prototype.Start = function (Event) {
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                AutoCSer.HtmlElement.$AddEvent(Element, ['keyup', 'keypress'], AutoCSer.Pub.ThisEvent(this, this.OnKey));
            }
        };
        InputKeyCode.prototype.OnKey = function (Event) {
            var Value = this.Keys[Event.KeyCode];
            if (Value)
                Value(Event);
            this.OnAnyKey.Function(Event);
        };
        InputKeyCode.DefaultParameter = { Id: null, Event: null, Keys: null };
        InputKeyCode.DefaultEvents = { OnAnyKey: null };
        return InputKeyCode;
    }());
    AutoCSer.InputKeyCode = InputKeyCode;
    AutoCSer.Declare.Create(InputKeyCode, 'InputKeyCode', 'focus', AutoCSer.DeclareType.EventElement);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=inputKeyCode.js.map