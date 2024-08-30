/// <reference path = "./load.page.ts" />
'use strict';
//默认密码输入框	<input defaultpassword="{PasswordId:'YYY'}" id="XXX" type="text" /><input id="YYY" type="text" />
module AutoCSer {
    export class DefaultPassword implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, PasswordId: null };
        private Id: string;
        private Event: DeclareEvent;
        private PasswordId: string;

        private Element: HTMLInputElement;
        constructor(Parameter) {
            Pub.GetParameter(this, DefaultPassword.DefaultParameter, Parameter);
            Declare.TryStart(this, this.Event);
        }
        Start(Event: DeclareEvent) {
            var Element = HtmlElement.$Id(this.Id), Input = Element.Element0() as HTMLInputElement, Password = HtmlElement.$Id(this.PasswordId);
            if (Input != this.Element) {
                this.Element = Input;
                Password.AddEvent('blur', Pub.ThisFunction(this, this.OnBlur));
            }
            Element.Display(0);
            Password.Display(1).Focus0();
        }
        private OnBlur () {
            var Password = HtmlElement.$Id(this.PasswordId);
            if (!(Password.Element0() as HTMLInputElement).value) {
                Password.Display(0);
                HtmlElement.$Id(this.Id).Display(1);
            }
        }
    }
    Declare.Create(DefaultPassword, 'DefaultPassword', 'focus', DeclareType.EventElement);
}