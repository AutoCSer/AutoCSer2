/// <reference path = "./load.page.ts" />
'use strict';
//输入框输入长度控件	<input inputlength="{Length:500,ViewId:'YYY',ErrorViewId:'ZZZ'}" id="XXX" /><div id="YYY" view="true" style="display:none">还可以输入=@InputLength字</div><div id="ZZZ" view="true" style="display:none">已经超出=@InputLength字</div>
module AutoCSer {
    export class InputLength implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, Length: 0, ViewId: null, LengthName: 'InputLength', ViewData: null, ErrorViewId: null, ErrorLengthName: 'InputLength', ErrorViewData: null };
        private Id: string;
        private Event: DeclareEvent;
        private Length: number;
        private ViewId: string;
        private LengthName: string;
        private ViewData: Object;
        private ErrorViewId: string;
        private ErrorLengthName: string;
        private ErrorViewData: Object;

        private Element: HTMLInputElement;
        constructor(Parameter) {
            Pub.GetParameter(this, InputLength.DefaultParameter, Parameter);
            if (!this.ViewData) this.ViewData = {};
            if (!this.ErrorViewData) this.ErrorViewData = {};
            Declare.TryStart(this, this.Event);
        }
        Start(Event: DeclareEvent) {
            var Element = HtmlElement.$IdElement(this.Id) as HTMLInputElement;
            if (Element != this.Element) {
                this.Element = Element;
                HtmlElement.$AddEvent(Element, ['keypress', 'keyup', 'blur'], Pub.ThisEvent(this, this.Check));
            }
        }
        private Check() {
            var Length = (HtmlElement.$IdElement(this.Id) as HTMLInputElement).value.length;
            if (Length <= this.Length) {
                if (this.ErrorViewId) View.Views[this.ErrorViewId].Hide();
                if (this.ViewId) {
                    this.ViewData[this.LengthName] = this.Length - Length;
                    View.Views[this.ViewId].Show(this.ViewData);
                }
            }
            else {
                if (this.ViewId) View.Views[this.ViewId].Hide();
                if (this.ErrorViewId) {
                    this.ErrorViewData[this.ErrorLengthName] = Length - this.Length;
                    View.Views[this.ErrorViewId].Show(this.ErrorViewData);
                }
            }
        }
        GetValue () {
            var Value = (HtmlElement.$IdElement(this.Id) as HTMLInputElement).value;
            return Value.length <= this.Length ? Value : null;
        }
    }
    Declare.Create(InputLength, 'InputLength', 'focus', DeclareType.EventElement);
}