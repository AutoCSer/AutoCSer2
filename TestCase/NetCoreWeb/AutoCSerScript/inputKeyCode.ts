/// <reference path = "./load.page.ts" />
'use strict';
//键盘事件	<input inputkeycode="{Keys:{13:AutoCSer.Pub.Alert}}" id="XXX" />
module AutoCSer {
    export class InputKeyCode implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, Keys: null };
        private static DefaultEvents = { OnAnyKey: null };
        private Id: string;
        private Event: DeclareEvent;
        private Keys: { [key: number]: Function };
        private OnAnyKey: Events;

        private Element: HTMLInputElement;
        constructor(Parameter) {
            Pub.GetParameterEvents(this, InputKeyCode.DefaultParameter, InputKeyCode.DefaultEvents, Parameter);
            if (!this.Keys) this.Keys = {};
            Declare.TryStart(this, this.Event);
        }
        Start(Event: DeclareEvent) {
            var Element = HtmlElement.$IdElement(this.Id) as HTMLInputElement;
            if (Element != this.Element) {
                this.Element = Element;
                HtmlElement.$AddEvent(Element, ['keyup', 'keypress'], Pub.ThisEvent(this, this.OnKey));
            }
        }
        private OnKey(Event: BrowserEvent) {
            var Value = this.Keys[Event.KeyCode];
            if (Value) Value(Event);
            this.OnAnyKey.Function(Event);
        }
    }
    Declare.Create(InputKeyCode, 'InputKeyCode', 'focus', DeclareType.EventElement);
}