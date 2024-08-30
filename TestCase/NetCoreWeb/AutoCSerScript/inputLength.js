/// <reference path = "./load.page.ts" />
'use strict';
//输入框输入长度控件	<input inputlength="{Length:500,ViewId:'YYY',ErrorViewId:'ZZZ'}" id="XXX" /><div id="YYY" view="true" style="display:none">还可以输入=@InputLength字</div><div id="ZZZ" view="true" style="display:none">已经超出=@InputLength字</div>
var AutoCSer;
(function (AutoCSer) {
    var InputLength = /** @class */ (function () {
        function InputLength(Parameter) {
            AutoCSer.Pub.GetParameter(this, InputLength.DefaultParameter, Parameter);
            if (!this.ViewData)
                this.ViewData = {};
            if (!this.ErrorViewData)
                this.ErrorViewData = {};
            AutoCSer.Declare.TryStart(this, this.Event);
        }
        InputLength.prototype.Start = function (Event) {
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                AutoCSer.HtmlElement.$AddEvent(Element, ['keypress', 'keyup', 'blur'], AutoCSer.Pub.ThisEvent(this, this.Check));
            }
        };
        InputLength.prototype.Check = function () {
            var Length = AutoCSer.HtmlElement.$IdElement(this.Id).value.length;
            if (Length <= this.Length) {
                if (this.ErrorViewId)
                    AutoCSer.View.Views[this.ErrorViewId].Hide();
                if (this.ViewId) {
                    this.ViewData[this.LengthName] = this.Length - Length;
                    AutoCSer.View.Views[this.ViewId].Show(this.ViewData);
                }
            }
            else {
                if (this.ViewId)
                    AutoCSer.View.Views[this.ViewId].Hide();
                if (this.ErrorViewId) {
                    this.ErrorViewData[this.ErrorLengthName] = Length - this.Length;
                    AutoCSer.View.Views[this.ErrorViewId].Show(this.ErrorViewData);
                }
            }
        };
        InputLength.prototype.GetValue = function () {
            var Value = AutoCSer.HtmlElement.$IdElement(this.Id).value;
            return Value.length <= this.Length ? Value : null;
        };
        InputLength.DefaultParameter = { Id: null, Event: null, Length: 0, ViewId: null, LengthName: 'InputLength', ViewData: null, ErrorViewId: null, ErrorLengthName: 'InputLength', ErrorViewData: null };
        return InputLength;
    }());
    AutoCSer.InputLength = InputLength;
    AutoCSer.Declare.Create(InputLength, 'InputLength', 'focus', AutoCSer.DeclareType.EventElement);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=inputLength.js.map