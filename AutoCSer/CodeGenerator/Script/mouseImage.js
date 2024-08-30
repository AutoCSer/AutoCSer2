/// <reference path = "./load.page.ts" />
'use strict';
//鼠标覆盖图片修改	<img mouseimage="{OverSrc:'over.jpg',OutSrc:'out.jpg'}" id="XXX" />
var AutoCSer;
(function (AutoCSer) {
    var MouseImage = /** @class */ (function () {
        function MouseImage(Parameter) {
            AutoCSer.Pub.GetParameter(this, MouseImage.DefaultParameter, Parameter);
            AutoCSer.Declare.TryStart(this, this.Event);
        }
        MouseImage.prototype.Start = function (Event) {
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                AutoCSer.HtmlElement.$AddEvent(Element, ['mouseout'], AutoCSer.Pub.ThisEvent(this, this.Out));
            }
            Element.src = this.OverSrc;
        };
        MouseImage.prototype.Out = function (Event) {
            AutoCSer.HtmlElement.$IdElement(this.Id).src = this.OutSrc;
        };
        MouseImage.DefaultParameter = { Id: null, Event: null, OverSrc: null, OutSrc: null };
        return MouseImage;
    }());
    AutoCSer.MouseImage = MouseImage;
    AutoCSer.Declare.Create(MouseImage, 'MouseImage', 'mouseover', AutoCSer.DeclareType.ParentName);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=mouseImage.js.map